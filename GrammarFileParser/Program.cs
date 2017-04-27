using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            GrammarBuilder builder = new GrammarBuilder(new FileStream("test.grammar", FileMode.Open));
            builder.Build();
        }
    }
}
