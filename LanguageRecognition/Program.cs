using LanguageRecognition.CodeGenerator;
using System;
using System.Collections.Generic;
using System.IO;
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
                Console.WriteLine("Usage:: <namespace_name>, <grammar_name>, <grammar_file_path>, <csproj path>");
                return;
            } else
            {
                var outputPath = Path.GetDirectoryName(args[3]) + @"\Generated\";
                using(var generator = new Generator(args[0], args[1], args[2], outputPath, args[3])) {
                    generator.Generate();
                }
            }
        }
    }
}
