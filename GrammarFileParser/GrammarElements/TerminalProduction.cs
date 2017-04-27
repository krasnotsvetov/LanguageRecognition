using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class TerminalProduction : IProductionElement
    {
        public string Name { get; private set; }
        public Terminal Terminal { get; internal set; }

        internal bool needSetTerminal = false;

        internal TerminalProduction(string name, bool needSetTerminal) {
            this.Name = name;
            this.Terminal = null;
            this.needSetTerminal = needSetTerminal;
        }

        public TerminalProduction(string name)
        {
            this.Name = name;
            this.Terminal = null;
        }

        public override bool Equals(object obj)
        {
            if (obj is TerminalProduction)
            {
                return (obj as TerminalProduction).Name.Equals(Name);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
