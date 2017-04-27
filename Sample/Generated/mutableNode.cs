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
	public class mutableNode : INode
	{
		public string ProductionName { get { return "mutable";}}
		public variableNode variable {get; private set;} = null;
		public mutableNode(variableNode variable)
		{
			this.variable = variable;
		}
	}
}
