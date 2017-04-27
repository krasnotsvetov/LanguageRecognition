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
	public class eaNode : INode
	{
		public string ProductionName { get { return "ea";}}
		public PlusOPNode PlusOP {get; private set;} = null;
		public tNode t {get; private set;} = null;
		public eaNode ea {get; private set;} = null;
		public eaNode()
		{

		}
		public eaNode(PlusOPNode PlusOP, tNode t, eaNode ea)
		{
			this.PlusOP = PlusOP;
			this.t = t;
			this.ea = ea;
		}
	}
}
