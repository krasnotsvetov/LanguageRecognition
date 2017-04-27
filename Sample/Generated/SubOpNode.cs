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
	public class SubOpNode : INode
	{
		public string Text {get; private set;}
		public string ProductionName { get { return "SubOp";}}
		public SubOpNode(string text)
		{
			this.Text = text;
		}
	}
}
