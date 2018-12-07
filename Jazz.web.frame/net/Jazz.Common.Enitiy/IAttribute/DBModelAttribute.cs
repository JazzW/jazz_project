using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Enitiy.IAttribute
{
    public  class DBModelAttribute:Attribute
    {
        public string TBName { get; set; }

        public string TBMess { get; set; }

        public string ListCols { get; set; }

        public string ItemCols { get; set; }

        public DBModelAttribute()
        {
            this.ListCols = this.ItemCols = " * ";
        }

    }
}
