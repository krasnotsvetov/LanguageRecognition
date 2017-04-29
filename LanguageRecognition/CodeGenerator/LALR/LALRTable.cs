using GrammarFileParser;
using GrammarFileParser.GrammarElements;
using LanguageRecognition.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition.CodeGenerator
{
    class LALRTable : BaseTable
    {

        internal List<LALRNode> CSet = new List<LALRNode>();
        private LRTable lrTable;

        public LALRTable(GrammarBuilder builder) : base(builder)
        {

        }
        public override void Build()
        {
            lrTable = new LRTable(grammarBuilder);
            lrTable.Build();
            Dictionary<LRNode, List<LRNode>> kernelFactorization = new Dictionary<LRNode, List<LRNode>>(new KernelFactorizationEqualityComparer());

            //factorize LR table by kernel
            foreach (var node in lrTable.CSet)
            {
                if (!kernelFactorization.ContainsKey(node))
                {
                    kernelFactorization[node] = new List<LRNode>();
                }
                kernelFactorization[node].Add(node);
            }

            //build new indices
            Dictionary<int, int> newIndices = new Dictionary<int, int>();
            lrTable.CSet.ToList().ForEach(t => newIndices[t.Index] = t.Index);

            //build new CSet
            int index = 0;
            foreach (var kvp in kernelFactorization)
            {
                var node = new LALRNode();
                node.Index = index++;

                Dictionary<GrammarRule, HashSet<TerminalProduction>> productions = new Dictionary<GrammarRule, HashSet<TerminalProduction>>(new GrammarRuleWithDotComparer());
                foreach (var n in kvp.Value)
                {
                    newIndices[n.Index] = node.Index;
                    foreach (var e in n.Elements)
                    {
                        if (!productions.ContainsKey(e.GrammarRule))
                        {
                            productions[e.GrammarRule] = new HashSet<TerminalProduction>();
                        }
                        productions[e.GrammarRule].Add(e.TerminalProduction);
                    }
                }
                foreach (var p in productions)
                {
                    node.Elements.Add(new LALRNodeElement(p.Key, p.Value));
                }
                CSet.Add(node);
            }

            //all grammar symbols
            var grammarSymbols = grammarBuilder.NonTerminals.Values.Select(t => (IProductionElement)new NonTerminalProduction(t.Name)).ToList();
            grammarSymbols.AddRange(grammarBuilder.Terminals.Values.Select(t => (IProductionElement)new TerminalProduction(t.Name)).ToList());

            //build main GOTO graph
            foreach (var node in lrTable.CSet)
            {
                foreach (var symbol in grammarSymbols)
                {
                    if (node.Edges.ContainsKey(symbol))
                    {
                        int u = newIndices[node.Index];
                        int v = newIndices[node.Edges[symbol].Index];
                        CSet[u].Edges[symbol] = CSet[v];
                    }
                }
            }

            //free memory
            lrTable = null;
            BuildTable();
        }


        private void BuildTable()
        {
            for (int i = 0; i < CSet.Count; i++)
            {
                Table[i] = new Dictionary<IProductionElement, TableElement>();
            }

            for (int i = 0; i < CSet.Count; i++)
            {
                var node = CSet[i];

                foreach (var element in node.Elements)
                {
                    var rule = element.GrammarRule;

                    if (rule.DotPos < rule.ProductionElements.Count)
                    {
                        var pe = rule.ProductionElements[rule.DotPos];
                        if (pe is NonTerminalProduction) continue;
                        var requiredState = new TableElement(TableCellState.Shift, node.Edges[pe].Index);
                        if (!Table[i].ContainsKey(pe) || Table[i][pe].Equals(requiredState))
                        {
                            Table[i][pe] = new TableElement(TableCellState.Shift, node.Edges[pe].Index);
                        }
                        else
                        {
                            throw new Exception("The grammar is not LALR");
                        }
                    }
                    else
                    {
                        if (rule.Nonterminal.Equals(grammarBuilder.StartNonTerminal))
                        {
                            var requiredState = new TableElement(TableCellState.Accept, -1);
                            if (!Table[i].ContainsKey(new EOFProduction()) || Table[i][new EOFProduction()].Equals(requiredState))
                            {
                                Table[i][new EOFProduction()] = requiredState;
                            }
                            else
                            {
                                throw new Exception("The grammar is not LALR");
                            }
                        }
                        else
                        {
                            foreach (var terminalProduction in element.TerminalProductions)
                            {
                                var requiredState = new TableElement(TableCellState.Reduce, rule.RuleIndex);
                                if (!Table[i].ContainsKey(terminalProduction) || Table[i][terminalProduction].Equals(requiredState))
                                {
                                    Table[i][terminalProduction] = requiredState;
                                }
                                else
                                {
                                    throw new Exception("The grammar is not LALR");
                                }
                            }
                        }
                    }
                }

                foreach (var nonterminal in grammarBuilder.NonTerminals.Values)
                {
                    var pe = new NonTerminalProduction(nonterminal.Name);
                    if (node.Edges.ContainsKey(pe))
                    {
                        var requiredState = new TableElement(TableCellState.Goto, node.Edges[pe].Index);
                        if (!Table[i].ContainsKey(pe) || Table[i][pe].Equals(requiredState))
                        {
                            Table[i][pe] = requiredState;
                        }
                        else
                        {
                            throw new Exception("The grammar is not LR(1)");
                        }
                    }
                }
            }
        }

        private class KernelFactorizationEqualityComparer : IEqualityComparer<LRNode>
        {
            public bool Equals(LRNode x, LRNode y)
            {

                HashSet<GrammarRule> checkSet = new HashSet<GrammarRule>(new GrammarRuleWithDotComparer());
                x.Elements.ToList().ForEach(t => checkSet.Add(t.GrammarRule));

                HashSet<GrammarRule> checkSetB = new HashSet<GrammarRule>(new GrammarRuleWithDotComparer());
                y.Elements.ToList().ForEach(t => checkSetB.Add(t.GrammarRule));

                if (checkSet.Count != checkSetB.Count) return false;

                foreach (var t in checkSetB)
                {
                    if (!checkSet.Contains(t))
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(LRNode obj)
            {
                HashSet<GrammarRule> checkSet = new HashSet<GrammarRule>();
                obj.Elements.ToList().ForEach(t => checkSet.Add(t.GrammarRule));

                int hc = checkSet.Count;
                foreach (var t in checkSet)
                {
                    hc = unchecked(hc * 314159 + t.GetHashCode() * 73693 + t.DotPos * 20963);
                }
                return hc;
            }
        }

        private class GrammarRuleWithDotComparer : IEqualityComparer<GrammarRule>
        {
            public bool Equals(GrammarRule x, GrammarRule y)
            {
                return (x.DotPos == y.DotPos && x.Equals(y));
            }

            public int GetHashCode(GrammarRule obj)
            {
                return obj.GetHashCode() * 73693  + obj.DotPos * 20963;
            }
        }
    }
}
