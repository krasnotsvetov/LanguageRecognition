using GrammarFileParser.GrammarElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition.CodeGenerator
{
    public class LALRNodeElement
    {
        public GrammarRule GrammarRule { get; private set; }
        public List<TerminalProduction> TerminalProductions { get; private set; } = new List<TerminalProduction>();

        public LALRNodeElement(GrammarRule rule, IEnumerable<TerminalProduction> productions)
        {
            GrammarRule = rule;
            TerminalProductions.AddRange(productions);
        }

        
        public override bool Equals(object obj)
        {
            if (obj is LALRNodeElement)
            {
                var t = obj as LALRNodeElement;
                if (t.TerminalProductions.Count != TerminalProductions.Count || !GrammarRule.Equals(t.GrammarRule) || !GrammarRule.DotPos.Equals(t.GrammarRule.DotPos))
                {
                    return false;
                }
                for (int i = 0; i < TerminalProductions.Count; i++)
                {
                    if (!t.TerminalProductions[i].Equals(TerminalProductions[i])) return false;
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hc = TerminalProductions.Count;
            for (int i = 0; i < TerminalProductions.Count; ++i)
            {
                hc = unchecked(hc * 314159 + TerminalProductions[i].GetHashCode());
            }

            return GrammarRule.GetHashCode() * 73693 + hc + GrammarRule.DotPos * 20963;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in TerminalProductions)
            {
                sb.Append(t.ToString() + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            return ($"{{{GrammarRule.ToString()}, {sb.ToString()}}}");
        }
    }

}
