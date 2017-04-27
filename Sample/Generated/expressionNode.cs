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
	public class expressionNode : INode
	{
		public string ProductionName { get { return "expression";}}
		public mutableNode mutable {get; private set;} = null;
		public EqualOpNode EqualOp {get; private set;} = null;
		public expressionNode expression {get; private set;} = null;
		public sumExpressionNode sumExpression {get; private set;} = null;
		public expressionNode(mutableNode mutable, EqualOpNode EqualOp, expressionNode expression)
		{
			this.mutable = mutable;
			this.EqualOp = EqualOp;
			this.expression = expression;
		}
		public expressionNode(sumExpressionNode sumExpression)
		{
			this.sumExpression = sumExpression;
		}
	}
}
