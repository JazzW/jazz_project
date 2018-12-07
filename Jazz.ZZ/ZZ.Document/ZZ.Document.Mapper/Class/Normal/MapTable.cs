using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class
{
    /// <summary>
    /// 值映射的表映射,源于[value-table]
    /// </summary>
    public class MapTable : MapVaItem
    {
        public System.Data.DataTable Table { get; set; }

        public override object Get()
        {
            return this.Table;
        }
    }
}
