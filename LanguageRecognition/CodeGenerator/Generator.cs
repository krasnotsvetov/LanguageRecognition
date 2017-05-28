using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GrammarFileParser;
using GrammarFileParser.GrammarElements;
using System.Xml;
using Microsoft.Build.Evaluation;

namespace LanguageRecognition.CodeGenerator
{

    //TODO list ::
    // 1) each class must have UpperCamelCase name, fix it
    // 2) move generator to partial classes. M
    //     a) methods which generate vocabulary
    //     b) methods which generate Nodes
    //     c) methods which generate table 
    public class Generator : IDisposable
    {

        private List<string> generatedClassNames = new List<string>();

        private string namespaceName;
        private string grammarName;
        private string outputPath;
        private string projectFilePath;
        private ITable table;

        private GrammarBuilder grammarBuilder;

        private List<string> usings = new List<string>()
        {
            "using System;",
            "using System.Collections.Generic;",
            "using System.Linq;",
            "using System.Text;",
            "using System.Threading.Tasks;",
            "using static LanguageRecognition.Runtime.LexicalAnalyzer;",
            "using System.Text.RegularExpressions;",
            "using LanguageRecognition.Runtime;",

        };

        private FileStream grammarStream;

        public Generator(string namespaceName, string grammarName, string grammarFilePath, string outputPath, string projectFilePath)
        {
            this.grammarStream = new FileStream(grammarFilePath, FileMode.Open);
            this.namespaceName = namespaceName;
            this.grammarName = grammarName;
            this.grammarBuilder = new GrammarBuilder(grammarStream);
            this.outputPath = outputPath;
            this.projectFilePath = projectFilePath;
        }


        public void Generate()
        {
            grammarBuilder.Build();

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            //GenerateNodeInterface();
            GenerateVocabulary();
            GenerateTerminalNodes();
            GenerateNonTerminalsNode();


            table = new LALRTable(grammarBuilder);
            table.Build();

            GenereteParserFile();

            InsertInProjectFile();
        }

        private void InsertInProjectFile()
        {
            var project = new Project(projectFilePath);
            List<ProjectItem> toRemove = new List<ProjectItem>();
            foreach (var pi in project.AllEvaluatedItems)
            {
                foreach (var metadataElement in pi.Metadata)
                {
                    if (metadataElement.Name.Equals("isGenerated") && metadataElement.UnevaluatedValue.Equals("true"))
                    {
                        toRemove.Add(pi);
                        break;
                    }
                }
            }
            project.RemoveItems(toRemove);
            foreach (var name in generatedClassNames)
            {
                project.AddItem("Compile", $"Generated\\{name}", new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("isGenerated", "true") });
            }
            project.Save();
        }

        private void GenerateNodeInterface()
        {
            generatedClassNames.Add($"INode.cs");
            using (var sw = new StreamWriter(new FileStream($"{outputPath}INode.cs", FileMode.Create)))
            {
                sw.WriteLine($"namespace {namespaceName}.Generated");
                sw.WriteLine("{");
                sw.WriteLine(GetTabs(1) + $"public interface INode");
                sw.WriteLine(GetTabs(1) + "{");
                sw.WriteLine();
                sw.WriteLine(GetTabs(1) + "}");
                sw.WriteLine("}");
            }
        }

        private void GenerateTerminalNodes()
        {
            foreach (var t in grammarBuilder.Terminals.Values)
            {
                GenerateTerminalNode(t);
            }
        }

        private void GenerateTerminalNode(Terminal t)
        {
            generatedClassNames.Add($"{t.Name}Node.cs");

            using (var sw = new StreamWriter(new FileStream($"{outputPath}{t.Name}Node.cs", FileMode.Create)))
            {
                WriteUsings(sw);
                sw.WriteLine($"namespace {namespaceName}.Generated");
                sw.WriteLine("{");
                sw.WriteLine(GetTabs(1) + $"public class {t.Name}Node : INode");
                sw.WriteLine(GetTabs(1) + "{");
                sw.WriteLine(GetTabs(2) + "public string Text {get; private set;}");
                sw.WriteLine(GetTabs(2) + $"public string ProductionName {{ get {{ return \"{t.Name}\";}}}}");
                sw.WriteLine(GetTabs(2) + $"public {t.Name}Node(string text)");
                sw.WriteLine(GetTabs(2) + "{");
                sw.WriteLine(GetTabs(3) + "this.Text = text;");
                sw.WriteLine(GetTabs(2) + "}");
                sw.WriteLine(GetTabs(1) + "}");
                sw.WriteLine("}");
            }
        }


        private void GenerateNonTerminalsNode()
        {
            foreach (var t in grammarBuilder.NonTerminals.Values)
            {
                GenerateNonTerminalNode(t);
            }
        }


        private void GenerateNonTerminalNode(NonTerminal t)
        {
            Dictionary<string, int> requiredProperties = new Dictionary<string, int>();
            bool isNeedDefaultConstructor = false;
            foreach (var p in t.Productions)
            {
                if (p.ProductionElements.Count == 0)
                {
                    isNeedDefaultConstructor = true;
                }
                Dictionary<string, int> requiredPropertiesLocal = new Dictionary<string, int>();
                foreach (var e in p.ProductionElements)
                {
                   
                    //skip eof and terminals which is defined by string in grammar rules.
                    if (e is TerminalProduction && (e as TerminalProduction).Terminal == null) continue;
                    if (!requiredPropertiesLocal.ContainsKey(e.Name))
                    {
                        requiredPropertiesLocal[e.Name] = 1;
                    } else
                    {
                        requiredPropertiesLocal[e.Name]++;
                    }
                }

                foreach (var kvp in requiredPropertiesLocal)
                {
                    if (requiredProperties.ContainsKey(kvp.Key))
                    {
                        requiredProperties[kvp.Key] = Math.Max(kvp.Value, requiredProperties[kvp.Key]);
                    } else
                    {
                        requiredProperties[kvp.Key] = kvp.Value;
                    }
                }
            }

            generatedClassNames.Add($"{t.Name}Node.cs");
            using (var sw = new StreamWriter(new FileStream($"{outputPath}{t.Name}Node.cs", FileMode.Create)))
            {
                WriteUsings(sw);
                sw.WriteLine($"namespace {namespaceName}.Generated");
                sw.WriteLine("{");
                sw.WriteLine(GetTabs(1) + $"public class {t.Name}Node : INode");
                sw.WriteLine(GetTabs(1) + "{");
                sw.WriteLine(GetTabs(2) + $"public string ProductionName {{ get {{ return \"{t.Name}\";}}}}");
                //generate properties
                foreach (var kvp in requiredProperties)
                {
                    int count = kvp.Value;
                    while (count > 0)
                    {
                        count--;
                        string name = count == 0 ? kvp.Key : kvp.Key + count.ToString();
                        sw.WriteLine($"{GetTabs(2)}public {kvp.Key}Node {name} {{get; private set;}} = null;");
                    }
                }

                if (isNeedDefaultConstructor)
                {
                    sw.WriteLine($"{GetTabs(2)}public {t.Name}Node()");
                    sw.WriteLine(GetTabs(2) + "{");
                    sw.WriteLine();
                    sw.WriteLine(GetTabs(2) + "}");
                }

                //generate constructors for all rules in this nonterminal
                foreach (var p in t.Productions)
                {
                    //It means that we don't need a constructor for epsilon rules
                    if (p.ProductionElements.Count == 0) continue;
                    if (p.ProductionElements[0] is EOFProduction) continue; 

                    Dictionary<string, int> constructorArguments = new Dictionary<string, int>();
                    sw.Write($"{GetTabs(2)}public {t.Name}Node(");
                    int strCount = 0;
                    foreach (var e in p.ProductionElements)
                    {
                        if (e is TerminalProduction && (e as TerminalProduction).Terminal == null)
                        {
                            sw.Write($"string str{strCount++}");
                        }
                        else
                        {
                            if (!constructorArguments.ContainsKey(e.Name))
                            {
                                constructorArguments[e.Name] = 1;
                                sw.Write($"{e.Name}Node {e.Name}");
                            }
                            else
                            {
                                constructorArguments[e.Name]++;
                                sw.Write($"{e.Name}Node {e.Name}{constructorArguments[e.Name]}");

                            }
                        }
                        if (p.ProductionElements.Last() != e)
                        {
                            sw.Write(", ");
                        }
                    }
                    sw.WriteLine(")");
                    sw.WriteLine(GetTabs(2) + "{");
                    foreach (var kvp in constructorArguments)
                    {
                        int count = kvp.Value;
                        while (count > 0)
                        {
                            count--;
                            string name = count == 0 ? kvp.Key : kvp.Key + count.ToString();
                            sw.WriteLine($"{GetTabs(3)}this.{name} = {name};");
                        }
                    }
                    sw.WriteLine(GetTabs(2) + "}");
                }

                sw.WriteLine(GetTabs(1) + "}");
                sw.WriteLine("}");
            }
        }

        private void GenerateVocabulary ()  
        {
            generatedClassNames.Add($"{grammarName}Vocabulary.cs");
            using (var sw = new StreamWriter(new FileStream($"{outputPath}{grammarName}Vocabulary.cs", FileMode.Create)))
            {
                WriteUsings(sw);
                sw.WriteLine($"namespace {namespaceName}.Generated");
                sw.WriteLine("{");
                sw.WriteLine(GetTabs(1) + $"public class {grammarName}Vocabulary : IVocabulary");
                sw.WriteLine(GetTabs(1) + "{");
                sw.WriteLine(GetTabs(2) + "public Dictionary<int, Regex> Tokens { get {return _tokens;} }");
                sw.WriteLine(GetTabs(2) + "public Dictionary<int, string> Names { get {return _names;} }");
                sw.WriteLine(GetTabs(2) + "public Dictionary<int, TokenAction> Actions { get {return _actions;} }");

                Dictionary<string, string> _tokens = new Dictionary<string, string>();
                Dictionary<string, string> _names = new Dictionary<string, string>();
                Dictionary<string, string> _actions = new Dictionary<string, string>();
                int id = 0;
                foreach (var t in grammarBuilder.Terminals.Values)
                {
                    if (t.LexerRule.ActionName == null)
                    {
                        _actions[id.ToString()] = "(analyzer) => TokenState.Nothing";
                    }
                    else
                    if (t.LexerRule.ActionName.Equals("skip"))
                    {
                        _actions[id.ToString()] = "(analyzer) => TokenState.Skip";
                    }

                    _names[id.ToString()] = $"\"{t.Name}\"";

                    string tempRegex = t.LexerRule.Regex.Substring(1, t.LexerRule.Regex.Length - 2);
                    _tokens[id.ToString()] = $"new Regex(@\"^{tempRegex}$\")";
                    id++;
                }
                sw.WriteLine(GenerateDictionaryWithElements("int", "Regex", "_tokens", "public", _tokens, 2));
                sw.WriteLine(GenerateDictionaryWithElements("int", "string", "_names", "public", _names, 2));
                sw.WriteLine(GenerateDictionaryWithElements("int", "TokenAction", "_actions", "public", _actions, 2));

                sw.WriteLine(GetTabs(1) + "}");
                sw.WriteLine("}");
            };
        }

        private void GenereteParserFile()
        {
            generatedClassNames.Add($"{grammarName}Parser.cs");
            using (var sw = new StreamWriter(new FileStream($"{outputPath}{grammarName}Parser.cs", FileMode.Create)))
            {
                WriteUsings(sw);
                sw.WriteLine($"namespace {namespaceName}.Generated");
                sw.WriteLine("{");
                sw.WriteLine(GetTabs(1) + $"public class {grammarName}Parser : LRParser");
                sw.WriteLine(GetTabs(1) + "{");

                sw.WriteLine(GetTabs(2) + $"public {grammarName}Parser(LexicalAnalyzer analyzer) : base(analyzer)");
                sw.WriteLine(GetTabs(2) + "{");
                sw.WriteLine(GetTabs(2) + "}");

                sw.WriteLine(GetTabs(2) + $"public override string GetNamespace()");
                sw.WriteLine(GetTabs(2) + "{");
                sw.WriteLine(GetTabs(3) + $"return @\"{namespaceName}.Generated\";");
                sw.WriteLine(GetTabs(2) + "}");

                sw.WriteLine(GetTabs(2) + $"public override int GetAmountOfProductionInRule(int index)");
                sw.WriteLine(GetTabs(2) + "{");

                var grammarRules = new List<GrammarRule>();
                foreach (var t in grammarBuilder.NonTerminals.Values)
                {
                    grammarRules.AddRange(t.GetAllRules());
                };
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var gr in grammarRules)
                {
                    dict[gr.RuleIndex.ToString()] = gr.ProductionElements.Count.ToString();
                }
                sw.WriteLine(GenerateDictionaryWithElements("int", "int", "dict", "", dict, 3));
                sw.WriteLine(GetTabs(3) + $"return dict[index];");
                sw.WriteLine(GetTabs(2) + "}");


                sw.WriteLine(GetTabs(2) + $"public override string GetGrammarRuleNonTerminalName(int index)");
                sw.WriteLine(GetTabs(2) + "{");

                Dictionary<string, string> dictNames = new Dictionary<string, string>();
                foreach (var gr in grammarRules)
                {
                    dictNames[gr.RuleIndex.ToString()] = "\"" + gr.Nonterminal.Name +"\"";
                }
                sw.WriteLine(GenerateDictionaryWithElements("int", "string", "dict", "", dictNames, 3));
                sw.WriteLine(GetTabs(3) + $"return dict[index];");
                sw.WriteLine(GetTabs(2) + "}");

                sw.WriteLine(GetTabs(2) + $"public {grammarBuilder.InputStartNonTerminal.Name}Node Parse()");
                sw.WriteLine(GetTabs(2) + "{");
                sw.WriteLine(GetTabs(3) + $"return ({grammarBuilder.InputStartNonTerminal.Name}Node)base.Parse();");
                sw.WriteLine(GetTabs(2) + "}");

                sw.WriteLine(GetTabs(2) + $"public override string GetStringTable()");
                sw.WriteLine(GetTabs(2) + "{");
                sw.WriteLine(GetTabs(3) + $"return @\"{table.PackTableToString()}\";");
                sw.WriteLine(GetTabs(2) + "}");

                sw.WriteLine(GetTabs(1) + "}");
                sw.WriteLine("}");
            }
        }

        private void WriteUsings(StreamWriter sw)
        {
            foreach (var u in usings)
            {
                sw.WriteLine(u);
            }
        }

        private string GetTabs(int offsetCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < offsetCount; i++)
            {
                sb.Append("\t");
            }
            return sb.ToString();
        }

        private string GenerateDictionaryWithElements(string firstGeneric, string secondGeneric, string name, string modifier, Dictionary<string, string> items, int tabsCount = 0)
        {
            StringBuilder rv = new StringBuilder();
            rv.Append($"{GetTabs(tabsCount)}{modifier} Dictionary<{firstGeneric},{secondGeneric}> {name} = new Dictionary<{firstGeneric},{secondGeneric}>()\n");
            rv.Append($"{GetTabs(tabsCount)}{{\n");
            foreach (var kvp in items)
            {
                rv.Append($"{GetTabs(1 + tabsCount)}{{{kvp.Key}, {kvp.Value}}},\n");
            }
            rv.Append($"{GetTabs(tabsCount)}}};\n");
            return rv.ToString();
        }

        public void Dispose()
        {
            grammarStream.Dispose();
        }
    }
}
