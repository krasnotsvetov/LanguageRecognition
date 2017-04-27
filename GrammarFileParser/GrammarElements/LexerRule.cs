using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class LexerRule
    {
        public string Regex { get; private set; }
        public string ActionName { get; private set; }

        public LexerRule(int id, string regex, string actionName = null)
        {
            this.ActionName = actionName;
            this.Regex = regex;
            this.id = id;
        }

        public int ID
        {
            get
            {
                return id;
            }
        }
        private int id;
    }
}
