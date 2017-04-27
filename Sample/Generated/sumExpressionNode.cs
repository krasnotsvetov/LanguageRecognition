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
	public class sumExpressionNode : INode
	{
		public string ProductionName { get { return "sumExpression";}}
		public sumExpressionNode sumExpression {get; private set;} = null;
		public sumNode sum {get; private set;} = null;
		public termNode term {get; private set;} = null;
		public sumExpressionNode(sumExpressionNode sumExpression, sumNode sum, termNode term)
		{
			this.sumExpression = sumExpression;
			this.sum = sum;
			this.term = term;
		}
		public sumExpressionNode(termNode term)
		{
			this.term = term;
		}
	}
}
