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
	public class tNode : INode
	{
		public string ProductionName { get { return "t";}}
		public fNode f {get; private set;} = null;
		public taNode ta {get; private set;} = null;
		public tNode(fNode f, taNode ta)
		{
			this.f = f;
			this.ta = ta;
		}
	}
}
