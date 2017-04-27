using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class EpsilonProduction : TerminalProduction
    {
        public EpsilonProduction() : base("epsilon")
        {
        }
    }
}
