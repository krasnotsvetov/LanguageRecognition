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
	public class programNode : INode
	{
		public string ProductionName { get { return "program";}}
		public expressionStatementNode expressionStatement {get; private set;} = null;
		public programNode(expressionStatementNode expressionStatement)
		{
			this.expressionStatement = expressionStatement;
		}
	}
}
