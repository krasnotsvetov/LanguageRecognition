using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrammarFileParser.GrammarElements
{
    public class Production
    {
        public List<IProductionElement> ProductionElements { get; set; } = new List<IProductionElement>();
        public Production(int id, IEnumerable<IProductionElement> productionElements)
        {
            this.ProductionElements.AddRange(productionElements);
            this.id = id;
        }  

        public Production(int id, IProductionElement productionElement)
        {
            ProductionElements.Add(productionElement);
            this.id = id;
        }


        public int ID
        {
            get
            {
                return id;
            }
        }

        private int id;
    }
}
