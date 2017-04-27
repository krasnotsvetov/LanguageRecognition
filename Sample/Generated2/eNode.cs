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
	public class eNode : INode
	{
		public string ProductionName { get { return "e";}}
		public tNode t {get; private set;} = null;
		public eaNode ea {get; private set;} = null;
		public eNode(tNode t, eaNode ea)
		{
			this.t = t;
			this.ea = ea;
		}
	}
}
