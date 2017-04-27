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
	public class taNode : INode
	{
		public string ProductionName { get { return "ta";}}
		public MulOPNode MulOP {get; private set;} = null;
		public fNode f {get; private set;} = null;
		public taNode ta {get; private set;} = null;
		public taNode()
		{

		}
		public taNode(MulOPNode MulOP, fNode f, taNode ta)
		{
			this.MulOP = MulOP;
			this.f = f;
			this.ta = ta;
		}
	}
}
