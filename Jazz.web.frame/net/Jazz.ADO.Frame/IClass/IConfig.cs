using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using Jazz.Helper.DataBase.Common;

namespace Jazz.ADO.Frame.IClass
{
    public class IConfig
    {
        public static void setCache(bool enitiycache)
        {
            Jazz.Common.Enitiy.EnitiyManager.useCache = enitiycache;
        }

        public static string ConnectString { get; set; }

        public static DBConfig.DatabaseType DbType { get; set; }

        public static DbParameter[] createParaemter(int Count)
        {
            DbParameter[] res;
            switch (DbType)
            {
                case DBConfig.DatabaseType.SqlServer:
                    res = new SqlParameter[Count];
                    Parallel.ForEach<DbParameter>(res,
                        (e) =>
                        {
                            e = new SqlParameter();
                        });
                    break;
                default:
                    res = new OleDbParameter[Count];
                    Parallel.ForEach<DbParameter>(res, (e) => {
                        e = new OleDbParameter();
                    });
                    break;
            }
            return res;
        }
    }
}
