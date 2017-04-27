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
	public class ExpressionVocabulary : IVocabulary
	{
		public Dictionary<int, Regex> Tokens { get {return _tokens;} }
		public Dictionary<int, string> Names { get {return _names;} }
		public Dictionary<int, TokenAction> Actions { get {return _actions;} }
		public Dictionary<int,Regex> _tokens = new Dictionary<int,Regex>()
		{
			{0, new Regex(@"^[_a-zA-Z][_a-zA-Z0-9]*$")},
			{1, new Regex(@"^[0-9]+$")},
			{2, new Regex(@"^\s$")},
			{3, new Regex(@"^\n+$")},
			{4, new Regex(@"^=$")},
			{5, new Regex(@"^\+$")},
			{6, new Regex(@"^-$")},
			{7, new Regex(@"^\*$")},
			{8, new Regex(@"^\^$")},
			{9, new Regex(@"^\($")},
			{10, new Regex(@"^\)$")},
			{11, new Regex(@"^;$")},
		};

		public Dictionary<int,string> _names = new Dictionary<int,string>()
		{
			{0, "Id"},
			{1, "Number"},
			{2, "WhiteSpace"},
			{3, "NewLine"},
			{4, "EqualOp"},
			{5, "SumOp"},
			{6, "SubOp"},
			{7, "MulOp"},
			{8, "PowOp"},
			{9, "OpenBracket"},
			{10, "CloseBracket"},
			{11, "Semicolon"},
		};

		public Dictionary<int,TokenAction> _actions = new Dictionary<int,TokenAction>()
		{
			{0, (analyzer) => TokenState.Nothing},
			{1, (analyzer) => TokenState.Nothing},
			{2, (analyzer) => TokenState.Skip},
			{3, (analyzer) => TokenState.Skip},
			{4, (analyzer) => TokenState.Nothing},
			{5, (analyzer) => TokenState.Nothing},
			{6, (analyzer) => TokenState.Nothing},
			{7, (analyzer) => TokenState.Nothing},
			{8, (analyzer) => TokenState.Nothing},
			{9, (analyzer) => TokenState.Nothing},
			{10, (analyzer) => TokenState.Nothing},
			{11, (analyzer) => TokenState.Nothing},
		};

	}
}
