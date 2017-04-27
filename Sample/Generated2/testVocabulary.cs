using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageRecognition.Runtime.LexicalAnalyzer;
using System.Text.RegularExpressions;
using LanguageRecognition.Runtime;
namespace Sample.Generated.Generated
{
	public class testVocabulary : IVocabulary
	{
		public Dictionary<int, Regex> Tokens { get {return _tokens;} }
		public Dictionary<int, string> Names { get {return _names;} }
		public Dictionary<int, TokenAction> Actions { get {return _actions;} }
		public Dictionary<int,Regex> _tokens = new Dictionary<int,Regex>()
		{
			{0, new Regex(@"^[0-9]$")},
			{1, new Regex(@"^\+$")},
			{2, new Regex(@"^\*$")},
			{3, new Regex(@"^\($")},
			{4, new Regex(@"^\)$")},
		};

		public Dictionary<int,string> _names = new Dictionary<int,string>()
		{
			{0, "N"},
			{1, "PlusOP"},
			{2, "MulOP"},
			{3, "OpenBracket"},
			{4, "CloseBracket"},
		};

		public Dictionary<int,TokenAction> _actions = new Dictionary<int,TokenAction>()
		{
			{0, (analyzer) => TokenState.Nothing},
			{1, (analyzer) => TokenState.Nothing},
			{2, (analyzer) => TokenState.Nothing},
			{3, (analyzer) => TokenState.Nothing},
			{4, (analyzer) => TokenState.Nothing},
		};

	}
}
