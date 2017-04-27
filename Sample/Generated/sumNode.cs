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
	public class sumNode : INode
	{
		public string ProductionName { get { return "sum";}}
		public SubOpNode SubOp {get; private set;} = null;
		public SumOpNode SumOp {get; private set;} = null;
		public sumNode(SubOpNode SubOp)
		{
			this.SubOp = SubOp;
		}
		public sumNode(SumOpNode SumOp)
		{
			this.SumOp = SumOp;
		}
	}
}
