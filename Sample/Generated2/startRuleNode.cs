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
	public class startRuleNode : INode
	{
		public string ProductionName { get { return "startRule";}}
		public eNode e {get; private set;} = null;
		public startRuleNode(eNode e)
		{
			this.e = e;
		}
	}
}
