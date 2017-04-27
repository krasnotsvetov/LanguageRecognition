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
	public class fNode : INode
	{
		public string ProductionName { get { return "f";}}
		public NNode N {get; private set;} = null;
		public OpenBracketNode OpenBracket {get; private set;} = null;
		public eNode e {get; private set;} = null;
		public CloseBracketNode CloseBracket {get; private set;} = null;
		public fNode(NNode N)
		{
			this.N = N;
		}
		public fNode(OpenBracketNode OpenBracket, eNode e, CloseBracketNode CloseBracket)
		{
			this.OpenBracket = OpenBracket;
			this.e = e;
			this.CloseBracket = CloseBracket;
		}
	}
}
