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
	public class unaryExpressionNode : INode
	{
		public string ProductionName { get { return "unaryExpression";}}
		public SubOpNode SubOp {get; private set;} = null;
		public unaryExpressionNode unaryExpression {get; private set;} = null;
		public variableNode variable {get; private set;} = null;
		public constantNode constant {get; private set;} = null;
		public OpenBracketNode OpenBracket {get; private set;} = null;
		public sumExpressionNode sumExpression {get; private set;} = null;
		public CloseBracketNode CloseBracket {get; private set;} = null;
		public unaryExpressionNode(SubOpNode SubOp, unaryExpressionNode unaryExpression)
		{
			this.SubOp = SubOp;
			this.unaryExpression = unaryExpression;
		}
		public unaryExpressionNode(variableNode variable)
		{
			this.variable = variable;
		}
		public unaryExpressionNode(constantNode constant)
		{
			this.constant = constant;
		}
		public unaryExpressionNode(OpenBracketNode OpenBracket, sumExpressionNode sumExpression, CloseBracketNode CloseBracket)
		{
			this.OpenBracket = OpenBracket;
			this.sumExpression = sumExpression;
			this.CloseBracket = CloseBracket;
		}
	}
}
