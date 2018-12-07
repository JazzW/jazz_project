using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Helper.DataBase.Common
{


    public class DBAction
    {
        public IDataBase db { get; set; }

        public SqlCmd sqlcmd { get; set; }

        public Object result { get; private set; }

        public int State { get; private set; }

        public void execute()
        {
            this.result = sqlcmd.execute(db);
        }
    }

}
