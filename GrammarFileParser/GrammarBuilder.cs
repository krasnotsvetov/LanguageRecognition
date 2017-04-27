using Antlr4.Runtime;
using GrammarFileParser.GrammarElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GrammarFileParser.GrammarFileGrammarParser;

namespace GrammarFileParser
{
    public class GrammarBuilder
    {
        public Dictionary<string, NonTerminal> NonTerminals { get; private set; } = new Dictionary<string, NonTerminal>();
        public Dictionary<string, Terminal> Terminals { get; private set; } = new Dictionary<string, Terminal>();

        /// <summary>
        /// Dictionary for tokens which defind in grammar rules;
        /// </summary>
        private Dictionary<string, string> GivenNames = new Dictionary<string, string>();

        public NonTerminal StartNonTerminal { get; private set; }

        public NonTerminal InputStartNonTerminal { get; private set; }

        private AntlrInputStream inputStream;
        private int productionRuleID = 0;

        public GrammarBuilder(FileStream stream)
        {
            inputStream = new AntlrInputStream(stream);
        }

        /// <summary>
        /// Initialize NonTerminals and Terminals with rules. 
        /// 
        /// </summary>
        public void Build()
        {
            GrammarFileGrammarLexer speakLexer = new GrammarFileGrammarLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
            GrammarFileGrammarParser grammarParser = new GrammarFileGrammarParser(commonTokenStream);


            foreach (var line in grammarParser.file().line())
            {
                if (line.lexerRule() != null)
                {
                    BuildLexerRule(line.lexerRule());
                } else
                {
                    BuildGrammarRule(line.grammarRule());
                }
            }

            InputStartNonTerminal = StartNonTerminal = NonTerminals[firstRuleName];

            if (NonTerminals.ContainsKey("startRule"))
            {
                throw new Exception("The name \"startRule\" is reserved, change the name for this nonterminal");
            }
            NonTerminals.Add("startRule", new NonTerminal("startRule"));
            NonTerminals["startRule"].AddRule(new Production(productionRuleID++, new NonTerminalProduction(firstRuleName)));
            firstRuleName = "startRule";
            StartNonTerminal = NonTerminals[firstRuleName];

            ///Checking, that we have all terminals and nonterminals, which was mentioned in rules
            foreach (var nonTerminals in NonTerminals.Values)
            {
                foreach(var rule in nonTerminals.Productions)
                {
                    foreach (var element in rule.ProductionElements)
                    {
                        if (element is TerminalProduction)
                        {
                            var e = element as TerminalProduction;
                            if (e.needSetTerminal)
                            {
                                if (!Terminals.ContainsKey(e.Name))
                                {
                                    throw new Exception($"Nonterminal {e} is not defined");
                                }
                                e.Terminal = Terminals[e.Name];
                            }
                        } else if (element is NonTerminalProduction)
                        {
                            var e = element as NonTerminalProduction;
                            if (!NonTerminals.ContainsKey(e.Name))
                            {
                                throw new Exception($"Nonterminal {e} is not defined");
                            }
                            e.NonTerminal = NonTerminals[e.Name];
                        }
                    }
                }
            }

            //Calculate FIRST and FOLLOW for each nonterminal

            //First:
            CalculateFirst();

            //Follow
            CalculateFollow();

        }


        private void CalculateFirst()
        {
            bool isChanged = false;
            do
            {
                isChanged = false;
                foreach (var nonTerminal in NonTerminals.Values)
                {
                    foreach (var rule in nonTerminal.Productions)
                    {
                        int count = nonTerminal.First.Count;
                        if (rule.ProductionElements.Count == 0)
                        {
                            nonTerminal.First.Add(new EpsilonProduction());
                        }
                        else if (rule.ProductionElements[0] is NonTerminalProduction)
                        {
                            for (int i = 0; i < rule.ProductionElements.Count; i++)
                            {
                                if (rule.ProductionElements[i] is TerminalProduction)
                                {
                                    nonTerminal.First.Add(rule.ProductionElements[i] as TerminalProduction);
                                    break;
                                }

                                var curNonTerminal = (rule.ProductionElements[i] as NonTerminalProduction).NonTerminal;
                                bool hasEpsilon = false;
                                foreach (var firstElement in curNonTerminal.First)
                                {
                                    if (firstElement.Equals(new EpsilonProduction()))
                                    {
                                        hasEpsilon = true;
                                    }
                                    nonTerminal.First.Add(firstElement);
                                }
                                if (!hasEpsilon)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {//Terminal
                            nonTerminal.First.Add(rule.ProductionElements[0] as TerminalProduction);
                        }
                        if (count != nonTerminal.First.Count)
                        {
                            isChanged = true;
                        }
                    }
                }

            } while (isChanged);
        }

        private string firstRuleName = "";

        private void CalculateFollow()
        {
            bool isChanged = false;

            StartNonTerminal.Follow.Add(new EOFProduction());
            do
            {
                isChanged = false;
                foreach (var nonTerminal in NonTerminals.Values)
                {
                    foreach (var rule in nonTerminal.Productions)
                    {
                        for (int i = 0; i < rule.ProductionElements.Count; i++)
                        {
                            var productionElement = rule.ProductionElements[i];
                            if (productionElement is NonTerminalProduction)
                            {
                                var curNonterminal = (productionElement as NonTerminalProduction).NonTerminal;
                                int count = curNonterminal.Follow.Count;
                                var nextElement = i + 1 < rule.ProductionElements.Count ? rule.ProductionElements[i + 1] : null;
                                if (nextElement != null && nextElement is TerminalProduction)
                                {
                                    curNonterminal.Follow.Add(nextElement as TerminalProduction);
                                } else
                                {
                                    var nprod = nextElement as NonTerminalProduction;
                                    if (nextElement == null || nprod.NonTerminal.First.Contains(new EpsilonProduction()))
                                    {
                                        foreach (var fe in nonTerminal.Follow)
                                        {
                                            curNonterminal.Follow.Add(fe);
                                        }
                                    }

                                    if (nextElement != null)
                                    {
                                        foreach (var fe in nprod.NonTerminal.First)
                                        {
                                            if (!fe.Equals(new EpsilonProduction())) {
                                                curNonterminal.Follow.Add(fe);
                                            }
                                        }
                                    }
                                }
                                if (count != curNonterminal.Follow.Count)
                                {
                                    isChanged = true;
                                }
                            }
                        }
                    }
                }
            } while (isChanged);

        }
        /// <summary>
        /// Build all rules for nonterminal
        /// </summary>
        /// <param name="grammarRuleContext"></param>
        private void BuildGrammarRule(GrammarRuleContext grammarRuleContext)
        {
            string name = grammarRuleContext.grammarName().GetText();

            //save start rule
            if (firstRuleName.Equals("")) firstRuleName = name;

            if (NonTerminals.ContainsKey(name))
            {
                throw new GrammarBuilderException($"Nonterminal {name} is defined twice");
            }
            var nonTerminal = NonTerminals[name] = new NonTerminal(name);
            ProductionsContext productions = grammarRuleContext.productions();
            do
            {
                nonTerminal.Productions.Add(BuildProduction(productions.production()));
                productions = productions.productions();
            } while (productions != null);
        }

        /// <summary>
        /// Build arraylist rule from recurcieve list   .
        /// </summary>
        /// <param name="grammarRuleContext"></param>
        private Production BuildProduction(ProductionContext context)
        {
            var rule = new List<IProductionElement>();
            do
            {
                if (context.grammarName() != null)
                {
                    string name = context.grammarName().GetText();
                    rule.Add(new NonTerminalProduction(name));
                } else if (context.lexerToken() != null)
                {
                    if (context.lexerToken().STRING() != null)
                    {
                        throw new GrammarBuilderException("Lexer defenition in grammar rules is not support now");
                        var text = context.lexerToken().STRING().GetText();
                        rule.Add(new TerminalProduction(text));
                    }
                    else
                    {
                        string name = context.lexerToken().lexerRuleName().GetText();
                        rule.Add(new TerminalProduction(name, true));
                       
                    }
                } else
                {
                    //rule.Add(new EpsilonProduction());
                }
                context = context.production();
            } while (context != null);
            return new Production(productionRuleID++, rule);
        }


        private void BuildLexerRule(LexerRuleContext lexerRuleContext)
        {
            //TODO: change actionName to delegate.
            string actionName = null;
            if (lexerRuleContext.GrammarName() != null)
            {
                actionName = lexerRuleContext.GrammarName().GetText();
            }

            string name = lexerRuleContext.LexerName().GetText();
            if (Terminals.ContainsKey(name))
            {
                throw new GrammarBuilderException($"Terminal {name} is defined twice");
            }
            var terminal = Terminals[name] = new Terminal(name);

            string regexp = lexerRuleContext.STRING().GetText();

            terminal.LexerRule = new LexerRule(0, regexp, actionName);
        }

        public class GrammarBuilderException : Exception
        {
            public GrammarBuilderException(string msg) : base(msg)
            {

            }
        }
    }
}
