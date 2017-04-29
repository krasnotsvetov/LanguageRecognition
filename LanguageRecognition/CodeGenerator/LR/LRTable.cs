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
    public class LRTable : BaseTable
    {

        
        /// <summary>
        /// It's C set from dragonbook.
        /// </summary>
        internal List<LRNode> CSet = new List<LRNode>();

        private int nodeIndex = 0;



        public LRTable(GrammarBuilder builder) : base(builder)
        {
        }

        public override void Build()
        {
            ConstructCSet();
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
                            throw new Exception("The grammar is not LR(1)");
                        }
                    } else
                    {
                        if (rule.Nonterminal.Equals(grammarBuilder.StartNonTerminal))
                        {
                            var requiredState = new TableElement(TableCellState.Accept, -1);
                            if (!Table[i].ContainsKey(new EOFProduction()) || Table[i][new EOFProduction()].Equals(requiredState))
                            {
                                Table[i][new EOFProduction()] = requiredState;
                            } else
                            {
                                throw new Exception("The grammar is not LR(1)");
                            }
                        } else
                        {
                            var requiredState = new TableElement(TableCellState.Reduce, rule.RuleIndex);
                            if (!Table[i].ContainsKey(element.TerminalProduction) || Table[i][element.TerminalProduction].Equals(requiredState)) {
                                Table[i][element.TerminalProduction] = requiredState;
                            } else
                            {
                                throw new Exception("The grammar is not LR(1)");
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

       


        LRNode Closure(LRNode node)
        {
            bool isChanged = true;

            while (isChanged)
            {
                isChanged = false;
                int prevCount = node.Elements.Count;
                HashSet<LRNodeElement> toAdd = new HashSet<LRNodeElement>();
                foreach (var element in node.Elements)
                {
                    var rule = element.GrammarRule;
                    if (rule.DotPos >= rule.ProductionElements.Count) continue;
                    var pe = rule.ProductionElements[rule.DotPos];
                    var next = rule.DotPos + 1 < rule.ProductionElements.Count ? rule.ProductionElements[rule.DotPos + 1] : new EpsilonProduction();
                    if (pe is NonTerminalProduction)
                    {
                        foreach (var r in grammarBuilder.NonTerminals[pe.Name].GetAllRules())
                        {
                            foreach (var terminal in CalculateFirst(next, element.TerminalProduction)) {
                                toAdd.Add(new LRNodeElement(r.Clone(), terminal));
                            }
                        }
                    }
                }

                foreach (var e in toAdd)
                {
                    node.Elements.Add(e);
                }
                if (node.Elements.Count != prevCount)
                {
                    isChanged = true;
                }
            }

            return node;
        }


        /// <summary>
        /// GOTO function from dragonbook
        /// </summary>
        /// <param name="I"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        LRNode Goto(LRNode I, IProductionElement X)
        {
            LRNode node = new LRNode();
            foreach (var e in I.Elements)
            {
                var rule = e.GrammarRule;
                var terminal = e.TerminalProduction;
                if (rule.DotPos < rule.ProductionElements.Count)
                {
                    var pe = rule.ProductionElements[rule.DotPos];
                    //TODO : think about removing first part of condition.
                    if (pe.GetType().Equals(X.GetType()) && X.Name.Equals(pe.Name))
                    {
                        var newRule = rule.Clone();
                        newRule.DotPos++;
                        node.Elements.Add(new LRNodeElement(newRule, terminal));
                    }
                }
            }
            return Closure(node);
        }

        /// <summary>
        /// Construct main sets for LR table
        /// </summary>
        void ConstructCSet()
        {
            CSet.Add(Closure(new LRNode(new LRNodeElement(grammarBuilder.StartNonTerminal.GetAllRules().First().Clone(), new EOFProduction()), 0)));
            nodeIndex = 1;
            var helpSet = new HashSet<LRNode>();
            helpSet.Add(CSet[0]);
            bool isChanged = true;
            while (isChanged)
            {
                isChanged = false;
                int prevCount = CSet.Count;
                List<LRNode> toAdd = new List<LRNode>();
                List<IProductionElement> grammarSymbols = grammarBuilder.NonTerminals.Values.Select(t => (IProductionElement)new NonTerminalProduction(t.Name)).ToList();
                grammarSymbols.AddRange(grammarBuilder.Terminals.Values.Select(t => (IProductionElement)new TerminalProduction(t.Name)).ToList());
                    
                foreach (var I in CSet)
                {
                    foreach (var symbol in grammarSymbols)
                    {
                        var gt = Goto(I, symbol);
                        if (gt.Elements.Count != 0)
                        {

                            int index = CSet.IndexOf(I);

                            if (!helpSet.Contains(gt))
                            { 
                                helpSet.Add(gt);
                                toAdd.Add(gt);
                                gt.Index = nodeIndex++;
                            }  else
                            {
                                if (toAdd.Contains(gt))
                                {
                                    gt.Index = CSet.Count + toAdd.IndexOf(gt);
                                } else
                                {
                                    gt.Index = CSet.IndexOf(gt);
                                }
                            }
                            I.Edges[symbol] = gt;
                        }
                    }

                }
                CSet.AddRange(toAdd);
                if (CSet.Count != prevCount)
                {
                    isChanged = true;
                }
            }

        }


        /// <summary>
        /// Calculate FIRST from concat element and a
        /// </summary>
        /// <param name="element"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private List<TerminalProduction> CalculateFirst(IProductionElement element, TerminalProduction a)
        {
            if (element is EpsilonProduction || element is EOFProduction)
            {
                return new List<TerminalProduction>() { a };
            }

            if (element is TerminalProduction)
            {
                return new List<TerminalProduction>() { (TerminalProduction)element};
            }

            var t = (element as NonTerminalProduction).NonTerminal;
            var rv = new List<TerminalProduction>();
            rv.AddRange(t.First);
            if (t.First.Contains(new EpsilonProduction()))
            {
                rv.Add(a);
            }
            return rv;
        }
    }
}
