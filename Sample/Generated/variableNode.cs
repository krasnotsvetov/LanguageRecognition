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
	public class variableNode : INode
	{
		public string ProductionName { get { return "variable";}}
		public IdNode Id {get; private set;} = null;
		public variableNode(IdNode Id)
		{
			this.Id = Id;
		}
	}
}
