using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class Terminal
    {
        public LexerRule LexerRule { get; internal set;}

        public string Name { get; private set; }
         

        public Terminal(string name)
        {
            this.Name = name;
        }

        public Terminal(string name, LexerRule lexerRule)
        {
            this.Name = name;
            this.LexerRule = lexerRule; 
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
    }
}
