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
	public class expressionStatementNode : INode
	{
		public string ProductionName { get { return "expressionStatement";}}
		public expressionNode expression {get; private set;} = null;
		public SemicolonNode Semicolon {get; private set;} = null;
		public expressionStatementNode expressionStatement {get; private set;} = null;
		public expressionStatementNode(expressionNode expression, SemicolonNode Semicolon, expressionStatementNode expressionStatement)
		{
			this.expression = expression;
			this.Semicolon = Semicolon;
			this.expressionStatement = expressionStatement;
		}
		public expressionStatementNode(expressionNode expression, SemicolonNode Semicolon)
		{
			this.expression = expression;
			this.Semicolon = Semicolon;
		}
	}
}
