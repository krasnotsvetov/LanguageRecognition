using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition.CodeGenerator
{
    public interface ITable
    {
        String PackTableToString();
        void Build();
    }

}
