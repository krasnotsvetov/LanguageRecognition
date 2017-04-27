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
	public class termNode : INode
	{
		public string ProductionName { get { return "term";}}
		public termNode term {get; private set;} = null;
		public MulOpNode MulOp {get; private set;} = null;
		public powExpressionNode powExpression {get; private set;} = null;
		public termNode(termNode term, MulOpNode MulOp, powExpressionNode powExpression)
		{
			this.term = term;
			this.MulOp = MulOp;
			this.powExpression = powExpression;
		}
		public termNode(powExpressionNode powExpression)
		{
			this.powExpression = powExpression;
		}
	}
}
