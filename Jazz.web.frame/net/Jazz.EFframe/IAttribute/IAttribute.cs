using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jazz.EFframe.IAttribute
{
    public abstract class IAttribute : Attribute
    {
        public string Table { get; set; }

        public List<ColumnPro> Columns { get; set; }

        public virtual ColumnPro GetColumn(string ColumnName)
        {
            return Columns.Where(e => e.ColName == ColumnName).FirstOrDefault();
        }

    }


}
