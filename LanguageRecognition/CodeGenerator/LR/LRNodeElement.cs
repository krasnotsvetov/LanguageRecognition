using GrammarFileParser.GrammarElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition.CodeGenerator
{
    public class LRNodeElement
    {
        public GrammarRule GrammarRule { get; private set; }
        public TerminalProduction TerminalProduction { get; private set; }

        public LRNodeElement(GrammarRule rule, TerminalProduction production)
        {
            GrammarRule = rule;
            TerminalProduction = production;
        }

        public override bool Equals(object obj)
        {
            if (obj is LRNodeElement)
            {
                var t = obj as LRNodeElement;
                return TerminalProduction.Equals(t.TerminalProduction) && GrammarRule.Equals(t.GrammarRule) && GrammarRule.DotPos.Equals(t.GrammarRule.DotPos);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return GrammarRule.GetHashCode() * 73693 + TerminalProduction.GetHashCode() + GrammarRule.DotPos * 20963;
        }

        public override string ToString()
        {
            return ($"{{{GrammarRule.ToString()}, {TerminalProduction}}}");
        }
    }

}
