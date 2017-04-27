using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class NonTerminal
    {
        public string Name { get; private set; }
        public List<Production> Productions { get; private set; } = new List<Production>();

        public HashSet<TerminalProduction> First { get; private set; } = new HashSet<TerminalProduction>();
        public HashSet<TerminalProduction> Follow { get; private set; } = new HashSet<TerminalProduction>();

        public NonTerminal(string name)
        {
            this.Name = name;
        }

        public NonTerminal(string name, IEnumerable<Production> productions)
        {
            this.Name = name;
            Productions.AddRange(productions);
        }

        public void AddRule(Production production)
        {
            Productions.Add(production);
        }


        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is NonTerminal)
            {
                return ((obj as NonTerminal).Name.Equals(Name));
            }
            return base.Equals(obj);
        }

        public IEnumerable<GrammarRule> GetAllRules()
        {
            return Productions.Select(t => new GrammarRule(this, t));
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
