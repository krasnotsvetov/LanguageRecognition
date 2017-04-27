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
	public class powExpressionNode : INode
	{
		public string ProductionName { get { return "powExpression";}}
		public unaryExpressionNode unaryExpression {get; private set;} = null;
		public PowOpNode PowOp {get; private set;} = null;
		public powExpressionNode powExpression {get; private set;} = null;
		public powExpressionNode(unaryExpressionNode unaryExpression, PowOpNode PowOp, powExpressionNode powExpression)
		{
			this.unaryExpression = unaryExpression;
			this.PowOp = PowOp;
			this.powExpression = powExpression;
		}
		public powExpressionNode(unaryExpressionNode unaryExpression)
		{
			this.unaryExpression = unaryExpression;
		}
	}
}
