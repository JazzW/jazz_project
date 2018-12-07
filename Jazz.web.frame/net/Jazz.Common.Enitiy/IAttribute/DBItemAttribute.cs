using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Enitiy.IAttribute
{
    public class DBItemAttribute:Attribute
    {

        public System.Data.SqlDbType T { get; set; }

        public int Size { get; set; }

        public bool Null { get; set; }

        public bool Check { get; set; }

        public bool Show { get; set; }

        public string Display { get; set; }

        public string ColName { get; set; }
    }
}
