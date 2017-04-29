using GrammarFileParser.GrammarElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition.CodeGenerator
{
    public class LALRNode
    {
        public HashSet<LALRNodeElement> Elements { get; private set; } = new HashSet<LALRNodeElement>();
        public Dictionary<IProductionElement, LALRNode> Edges { get; private set; } = new Dictionary<IProductionElement, LALRNode>();
        public int Index { get; internal set; } = -1;

        public LALRNode()
        {
            
        }

        public LALRNode(LALRNodeElement element, int index = -1)
        {
            this.Index = index;
            this.Elements.Add(element);
        }
        public override string ToString()
        {
            var rv = "";
            foreach (var element in Elements)
            {
                rv += element.ToString();
            }
            return rv;
        }


        /// <summary>
        /// Return true if elements of both instances is same. Edges can be different.
        /// </summary>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is LALRNode)
            {
                var t = (obj as LALRNode).Elements;
                if (Elements.Count != t.Count) return false;
                foreach (var e in Elements)
                {
                    if (!t.Contains(e))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }


        //TODO:: add lazy hash
        /// <summary>
        /// Return same value for instances which has same elements, edges can be different.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hc = Elements.Count;
            foreach (var t in Elements)
            {
                hc = unchecked(hc * 314159 + t.GetHashCode());
            }
            return hc;
        }
    }
}
