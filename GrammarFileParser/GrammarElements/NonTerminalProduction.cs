using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class NonTerminalProduction : IProductionElement
    {
        public string Name { get; private set; }
        public NonTerminal NonTerminal { get; internal set;}

        public NonTerminalProduction(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is NonTerminalProduction) {
                return Name.Equals((obj as NonTerminalProduction).Name);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
