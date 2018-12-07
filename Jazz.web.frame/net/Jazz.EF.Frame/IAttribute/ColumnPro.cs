using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.EF.Frame.IAttribute
{
    public class ColumnPro
    {
        public string ColName { get; set; }

        public Type ColType { get; set; }

        public bool Key { get; set; }

        public bool CNull { get; set; }

        public string[] Limit { get; set; }

    }

    public class ColumnPro<T> : ColumnPro
    {
        public ColumnPro(ColumnPro col, T val)
        {
            this.ColName = col.ColName;
            this.ColType = col.ColType;
            this.Key = col.Key;
            this.CNull = col.CNull;
            col.Limit.CopyTo(this.Limit, 0);
            this.Value = val;
        }

        public T Value { get; set; }
    }
}
