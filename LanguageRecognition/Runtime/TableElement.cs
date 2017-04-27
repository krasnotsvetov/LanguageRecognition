using LanguageRecognition.CodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageRecognition.Runtime
{
    public class TableElement
    {
        public TableCellState State { get; private set; }
        public int Num { get; private set; }

        public TableElement(TableCellState cellState, int num)
        {
            this.State = cellState;
            this.Num = num;
        }

        public override bool Equals(object obj)
        {
            if (obj is TableElement)
            {
                var t = obj as TableElement;
                return t.Num.Equals(Num) && t.State.Equals(State);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return State.GetHashCode() + Num.GetHashCode();
        }
    }


    public enum TableCellState
    {
        Reduce = 0,
        Shift = 1,
        Accept = 2,
        Goto = 4
    }
}
