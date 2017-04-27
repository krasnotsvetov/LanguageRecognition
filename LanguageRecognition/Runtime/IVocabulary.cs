using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LanguageRecognition.Runtime.LexicalAnalyzer;

namespace LanguageRecognition.Runtime
{
    public interface IVocabulary
    {
        Dictionary<int, Regex> Tokens { get; }
        Dictionary<int, string> Names { get; }
        Dictionary<int, TokenAction> Actions { get; }
    }
}
