using LanguageRecognition.CodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 4)
            {
                Console.WriteLine("Usage:: <namespace_name>, <grammar_name>, <grammar_file_path>, <project_dir_path>");
            } else
            {
                new Generator(args[0], args[1], args[2], args[3]);
            }
            new Generator("Sample.Generated", "Expression", "Expressions.grammar", @"C:\workspace\Itmo\LanguageRecognition\Sample\Generated\").Generate();
        }
    }
}
