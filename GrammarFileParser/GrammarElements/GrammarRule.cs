using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class GrammarRule
    {
        public NonTerminal Nonterminal { get; private set; }
        public List<IProductionElement> ProductionElements { get; private set; }
        public int RuleIndex { get; private set; }

        //For LALR automaton and table 

        /// <summary>
        /// Rules with different or same DotPos and same RuleIndex is equals.
        /// </summary>
        /// TODO: remove dotpos from GrammarRule and move in to LRNode
        public int DotPos { get; set; } = 0;
        public GrammarRule(NonTerminal nonterminal, Production production)
        {
            this.Nonterminal = nonterminal;
            this.ProductionElements = production.ProductionElements;
            this.RuleIndex = production.ID;
        }

        /// <summary>
        /// Clone grammar rule. The clone isn't deep.
        /// </summary>
        /// <returns></returns>
        public GrammarRule Clone()
        {
            var rv = new GrammarRule(Nonterminal, new Production(RuleIndex, ProductionElements));
            rv.DotPos = DotPos;
            return rv;
        }


        public override bool Equals(object obj)
        {
            if (obj is GrammarRule)
            {
                var t = obj as GrammarRule;
                if (t.RuleIndex == RuleIndex) return true;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hc = ProductionElements.Count + 3;
            hc = unchecked(hc * 314159 + Nonterminal.GetHashCode());
            hc = unchecked(hc * 314159 + RuleIndex);
            for (int i = 0; i < ProductionElements.Count; ++i)
            {
                hc = unchecked(hc * 314159 + ProductionElements[i].GetHashCode());
            }
            return hc;
        }

        public override string ToString()
        {
            string rv = Nonterminal.ToString() + " -> ";
            for (int i = 0; i < ProductionElements.Count; i++)
            {
                if (DotPos == i)
                {
                    rv += " @";
                }
                rv += " " + ProductionElements[i].ToString();
            }
            if (DotPos >= ProductionElements.Count)
            {
                rv += " @";
            }
            return rv;
        }
    }
}
