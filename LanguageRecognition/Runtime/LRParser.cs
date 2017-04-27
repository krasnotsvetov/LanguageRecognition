using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition.Runtime
{
    public abstract class LRParser
    {
        private Dictionary<int, Dictionary<string, TableElement>> table;
        private LexicalAnalyzer analyzer;

        public LRParser(LexicalAnalyzer analyzer) 
        {
            this.analyzer = analyzer;
            ConstructTableFromString();
        }

        public abstract string GetStringTable();
        public abstract string GetNamespace();
        public abstract int GetAmountOfProductionInRule(int index);
        public abstract string GetGrammarRuleNonTerminalName(int index);


        public void ConstructTableFromString()
        {
            table = new Dictionary<int, Dictionary<string, TableElement>>();
            string[] a = GetStringTable().Split('\n');

            int pos = 0;
            int tableCount = int.Parse(a[0]);
            pos++;

            for (int i = 0; i < tableCount; i++)
            {
                table[i] = new Dictionary<string, TableElement>();
                int elementCount = int.Parse(a[pos++]);
                for (int j = 0; j < elementCount; j++)
                {
                    string[] v = a[pos++].Split(' ');
                    table[i][v[0]] = new TableElement((TableCellState)int.Parse(v[2]), int.Parse(v[1]));
                }
                
            }
        }


        private Stack<int> nodes = new Stack<int>();
        

        //TODO : now it's object, because string and INode can be shifted from input to this stack.
        private Stack<INode> symbols = new Stack<INode>();

        protected INode Parse()
        {
            nodes.Push(0);

            analyzer.NextToken();
            bool isAccept = false;
            while (!isAccept)
            {
                if (!table[nodes.Peek()].ContainsKey(analyzer.CurrentTokenName))
                {
                    throw new Exception($"Unexpected symbol {analyzer.CurrentTokenValue} at position {analyzer.CurrentTokenPosition}");
                }
                var tableState = table[nodes.Peek()][analyzer.CurrentTokenName];
                switch (tableState.State)
                {
                    case TableCellState.Shift:
                       
                        nodes.Push(tableState.Num);

                        Type type = null;
                        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            type = a.GetType($"{GetNamespace()}.{analyzer.CurrentTokenName}Node");
                            if (type != null) break;
                        }

                        var terminal = (INode)Activator.CreateInstance(type, new object[] { analyzer.CurrentTokenValue });
                        symbols.Push(terminal);
                        analyzer.NextToken();
                        break;
                    case TableCellState.Reduce:
                        List<INode> temp = new List<INode>();
                        for (int i = 0; i < GetAmountOfProductionInRule(tableState.Num); i++)
                        {
                            temp.Add(symbols.Pop());
                            nodes.Pop();
                        }
                        temp.Reverse();

                        //TODO : generate methods in Generator.cs which will return INode by passing ruleIndex and stack without reflection,
                        // because reflection is too slow(need check all constructors)
                        type = null;

                        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            type = a.GetType($"{GetNamespace()}.{GetGrammarRuleNonTerminalName(tableState.Num)}Node");
                            if (type != null) break;
                        }
                        var t = (INode)Activator.CreateInstance(type, temp.ToArray());

                        symbols.Push(t);
                        nodes.Push(table[nodes.Peek()][((INode)t).ProductionName].Num);
                        break; 
                    case TableCellState.Accept:
                        isAccept = true;
                        break;
                }
            }
            return (INode)symbols.Peek();
        }
        
    }
}
