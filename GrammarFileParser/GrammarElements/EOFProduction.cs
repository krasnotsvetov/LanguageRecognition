using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class EOFProduction : TerminalProduction
    {
        public EOFProduction() : base("eof")
        {
        }
    }
}
