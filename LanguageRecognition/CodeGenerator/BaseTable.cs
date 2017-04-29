using GrammarFileParser.GrammarElements;
using LanguageRecognition.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarFileParser;

namespace LanguageRecognition.CodeGenerator
{
    public abstract class BaseTable : ITable
    {
        protected GrammarBuilder grammarBuilder;

        public BaseTable(GrammarBuilder builder)
        {
            this.grammarBuilder = builder;
            Table = new Dictionary<int, Dictionary<IProductionElement, TableElement>>();
        }

        public Dictionary<int, Dictionary<IProductionElement, TableElement>> Table { get; protected set; }

        public abstract void Build();

        public virtual string PackTableToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Table.Count + "\n");
            for (int i = 0; i < Table.Count; i++)
            {
                sb.Append(Table[i].Count + "\n");
                foreach (var kvp in Table[i])
                {
                    sb.Append(kvp.Key.Name + " " + kvp.Value.Num + " " + (int)kvp.Value.State + "\n");
                }
            }
            return sb.ToString();
        }



    }
}
