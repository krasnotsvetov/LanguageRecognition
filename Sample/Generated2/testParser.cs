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
	public class testParser : LRParser
	{
		public testParser(LexicalAnalyzer analyzer) : base(analyzer)
		{
		}
		public override string GetNamespace()
		{
			return @"Sample.Generated.Generated";
		}
		public override int GetAmountOfProductionInRule(int index)
		{
			 Dictionary<int,int> dict = new Dictionary<int,int>()
			{
				{0, 2},
				{1, 3},
				{2, 0},
				{3, 2},
				{4, 3},
				{5, 0},
				{6, 1},
				{7, 3},
				{8, 1},
			};

			return dict[index];
		}
		public override string GetGrammarRuleNonTerminalName(int index)
		{
			 Dictionary<int,string> dict = new Dictionary<int,string>()
			{
				{0, "e"},
				{1, "ea"},
				{2, "ea"},
				{3, "t"},
				{4, "ta"},
				{5, "ta"},
				{6, "f"},
				{7, "f"},
				{8, "startRule"},
			};

			return dict[index];
		}
		public eNode Parse()
		{
			return (eNode)base.Parse();
		}
		public override string GetStringTable()
		{
			return @"30
5
N 4 1
OpenBracket 5 1
e 1 4
t 2 4
f 3 4
1
eof -1 2
3
PlusOP 7 1
eof 2 0
ea 6 4
5
MulOP 9 1
PlusOP 5 0
epsilon 5 0
eof 5 0
ta 8 4
4
MulOP 6 0
epsilon 6 0
PlusOP 6 0
eof 6 0
5
N 13 1
OpenBracket 14 1
e 10 4
t 11 4
f 12 4
1
eof 0 0
4
N 4 1
OpenBracket 5 1
t 15 4
f 3 4
3
PlusOP 3 0
epsilon 3 0
eof 3 0
3
N 4 1
OpenBracket 5 1
f 16 4
1
CloseBracket 17 1
3
PlusOP 19 1
CloseBracket 2 0
ea 18 4
5
MulOP 21 1
PlusOP 5 0
epsilon 5 0
CloseBracket 5 0
ta 20 4
4
MulOP 6 0
epsilon 6 0
PlusOP 6 0
CloseBracket 6 0
5
N 13 1
OpenBracket 14 1
e 22 4
t 11 4
f 12 4
3
PlusOP 7 1
eof 2 0
ea 23 4
5
MulOP 9 1
PlusOP 5 0
epsilon 5 0
eof 5 0
ta 24 4
4
MulOP 7 0
epsilon 7 0
PlusOP 7 0
eof 7 0
1
CloseBracket 0 0
4
N 13 1
OpenBracket 14 1
t 25 4
f 12 4
3
PlusOP 3 0
epsilon 3 0
CloseBracket 3 0
3
N 13 1
OpenBracket 14 1
f 26 4
1
CloseBracket 27 1
1
eof 1 0
3
PlusOP 4 0
epsilon 4 0
eof 4 0
3
PlusOP 19 1
CloseBracket 2 0
ea 28 4
5
MulOP 21 1
PlusOP 5 0
epsilon 5 0
CloseBracket 5 0
ta 29 4
4
MulOP 7 0
epsilon 7 0
PlusOP 7 0
CloseBracket 7 0
1
CloseBracket 1 0
3
PlusOP 4 0
epsilon 4 0
CloseBracket 4 0
";
		}
	}
}
