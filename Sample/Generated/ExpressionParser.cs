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
	public class ExpressionParser : LRParser
	{
		public ExpressionParser(LexicalAnalyzer analyzer) : base(analyzer)
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
				{0, 1},
				{1, 3},
				{2, 2},
				{3, 3},
				{4, 1},
				{5, 3},
				{6, 1},
				{7, 1},
				{8, 1},
				{9, 1},
				{10, 3},
				{11, 1},
				{12, 3},
				{13, 1},
				{14, 2},
				{15, 1},
				{16, 1},
				{17, 3},
				{18, 1},
				{19, 1},
				{20, 1},
			};

			return dict[index];
		}
		public override string GetGrammarRuleNonTerminalName(int index)
		{
			 Dictionary<int,string> dict = new Dictionary<int,string>()
			{
				{0, "program"},
				{1, "expressionStatement"},
				{2, "expressionStatement"},
				{3, "expression"},
				{4, "expression"},
				{5, "sumExpression"},
				{6, "sumExpression"},
				{7, "sum"},
				{8, "sum"},
				{9, "mutable"},
				{10, "term"},
				{11, "term"},
				{12, "powExpression"},
				{13, "powExpression"},
				{14, "unaryExpression"},
				{15, "unaryExpression"},
				{16, "unaryExpression"},
				{17, "unaryExpression"},
				{18, "variable"},
				{19, "constant"},
				{20, "startRule"},
			};

			return dict[index];
		}
		public programNode Parse()
		{
			return (programNode)base.Parse();
		}
		public override string GetStringTable()
		{
			return @"50
14
Id 11 1
SubOp 13 1
OpenBracket 14 1
Number 12 1
program 1 4
expressionStatement 2 4
expression 3 4
sumExpression 4 4
mutable 5 4
term 6 4
powExpression 7 4
unaryExpression 8 4
variable 9 4
constant 10 4
1
eof -1 2
1
eof 0 0
1
Semicolon 15 1
4
Semicolon 4 0
SubOp 18 1
SumOp 17 1
sum 16 4
1
EqualOp 19 1
4
Semicolon 6 0
SubOp 6 0
SumOp 6 0
MulOp 20 1
4
Semicolon 11 0
SubOp 11 0
SumOp 11 0
MulOp 11 0
5
PowOp 21 1
Semicolon 13 0
SubOp 13 0
SumOp 13 0
MulOp 13 0
6
EqualOp 9 0
PowOp 15 0
Semicolon 15 0
SubOp 15 0
SumOp 15 0
MulOp 15 0
5
PowOp 16 0
Semicolon 16 0
SubOp 16 0
SumOp 16 0
MulOp 16 0
6
EqualOp 18 0
PowOp 18 0
Semicolon 18 0
SubOp 18 0
SumOp 18 0
MulOp 18 0
5
PowOp 19 0
Semicolon 19 0
SubOp 19 0
SumOp 19 0
MulOp 19 0
7
SubOp 13 1
OpenBracket 14 1
Id 24 1
Number 12 1
unaryExpression 22 4
variable 23 4
constant 10 4
10
SubOp 33 1
OpenBracket 34 1
Id 31 1
Number 32 1
sumExpression 25 4
term 26 4
powExpression 27 4
unaryExpression 28 4
variable 29 4
constant 30 4
14
eof 2 0
Id 11 1
SubOp 13 1
OpenBracket 14 1
Number 12 1
expressionStatement 35 4
expression 3 4
sumExpression 4 4
mutable 5 4
term 6 4
powExpression 7 4
unaryExpression 8 4
variable 9 4
constant 10 4
9
SubOp 13 1
OpenBracket 14 1
Id 24 1
Number 12 1
term 36 4
powExpression 7 4
unaryExpression 8 4
variable 23 4
constant 10 4
4
SubOp 8 0
OpenBracket 8 0
Id 8 0
Number 8 0
4
SubOp 7 0
OpenBracket 7 0
Id 7 0
Number 7 0
12
Id 11 1
SubOp 13 1
OpenBracket 14 1
Number 12 1
expression 37 4
sumExpression 4 4
mutable 5 4
term 6 4
powExpression 7 4
unaryExpression 8 4
variable 9 4
constant 10 4
8
SubOp 13 1
OpenBracket 14 1
Id 24 1
Number 12 1
powExpression 38 4
unaryExpression 8 4
variable 23 4
constant 10 4
8
SubOp 13 1
OpenBracket 14 1
Id 24 1
Number 12 1
powExpression 39 4
unaryExpression 8 4
variable 23 4
constant 10 4
5
PowOp 14 0
Semicolon 14 0
SubOp 14 0
SumOp 14 0
MulOp 14 0
5
PowOp 15 0
Semicolon 15 0
SubOp 15 0
SumOp 15 0
MulOp 15 0
5
PowOp 18 0
Semicolon 18 0
SubOp 18 0
SumOp 18 0
MulOp 18 0
4
CloseBracket 41 1
SubOp 18 1
SumOp 17 1
sum 40 4
4
CloseBracket 6 0
SubOp 6 0
SumOp 6 0
MulOp 42 1
4
CloseBracket 11 0
SubOp 11 0
SumOp 11 0
MulOp 11 0
5
PowOp 43 1
CloseBracket 13 0
SubOp 13 0
SumOp 13 0
MulOp 13 0
5
PowOp 15 0
CloseBracket 15 0
SubOp 15 0
SumOp 15 0
MulOp 15 0
5
PowOp 16 0
CloseBracket 16 0
SubOp 16 0
SumOp 16 0
MulOp 16 0
5
PowOp 18 0
CloseBracket 18 0
SubOp 18 0
SumOp 18 0
MulOp 18 0
5
PowOp 19 0
CloseBracket 19 0
SubOp 19 0
SumOp 19 0
MulOp 19 0
7
SubOp 33 1
OpenBracket 34 1
Id 31 1
Number 32 1
unaryExpression 44 4
variable 29 4
constant 30 4
10
SubOp 33 1
OpenBracket 34 1
Id 31 1
Number 32 1
sumExpression 45 4
term 26 4
powExpression 27 4
unaryExpression 28 4
variable 29 4
constant 30 4
1
eof 1 0
4
Semicolon 5 0
SubOp 5 0
SumOp 5 0
MulOp 20 1
1
Semicolon 3 0
4
Semicolon 10 0
SubOp 10 0
SumOp 10 0
MulOp 10 0
4
Semicolon 12 0
SubOp 12 0
SumOp 12 0
MulOp 12 0
9
SubOp 33 1
OpenBracket 34 1
Id 31 1
Number 32 1
term 46 4
powExpression 27 4
unaryExpression 28 4
variable 29 4
constant 30 4
5
PowOp 17 0
Semicolon 17 0
SubOp 17 0
SumOp 17 0
MulOp 17 0
8
SubOp 33 1
OpenBracket 34 1
Id 31 1
Number 32 1
powExpression 47 4
unaryExpression 28 4
variable 29 4
constant 30 4
8
SubOp 33 1
OpenBracket 34 1
Id 31 1
Number 32 1
powExpression 48 4
unaryExpression 28 4
variable 29 4
constant 30 4
5
PowOp 14 0
CloseBracket 14 0
SubOp 14 0
SumOp 14 0
MulOp 14 0
4
CloseBracket 49 1
SubOp 18 1
SumOp 17 1
sum 40 4
4
CloseBracket 5 0
SubOp 5 0
SumOp 5 0
MulOp 42 1
4
CloseBracket 10 0
SubOp 10 0
SumOp 10 0
MulOp 10 0
4
CloseBracket 12 0
SubOp 12 0
SumOp 12 0
MulOp 12 0
5
PowOp 17 0
CloseBracket 17 0
SubOp 17 0
SumOp 17 0
MulOp 17 0
";
		}
	}
}
