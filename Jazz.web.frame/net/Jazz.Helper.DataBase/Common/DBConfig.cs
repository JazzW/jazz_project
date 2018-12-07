using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Jazz.Helper.DataBase.Common
{
    public class DBConfig
    {

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DatabaseType DbType { get; set; }

        public static void set(DatabaseType dbtype,IDataBase db)
        {
            DbType = dbtype;
            DBHelper.database = db;

        }

        /// <summary>
        /// 数据库命名参数符号
        /// </summary>
        public static string DbParmChar { get; set; }

        public static string ConnectionString { get; set; }

        public enum DatabaseType
        {
            /// <summary>
            /// 数据库类型：Oracle
            /// </summary>
            Oracle,
            /// <summary>
            /// 数据库类型：SqlServer
            /// </summary>
            SqlServer,
            /// <summary>
            /// 数据库类型：Access
            /// </summary>
            Access,
            /// <summary>
            /// 数据库类型：MySql
            /// </summary>
            MySql,
            /// <summary>
            /// 数据库类型：SQLite
            /// </summary>
            SQLite
        }

        public static DbParameter[] createParaemter(int Count)
        {
            DbParameter[] res;
            switch (DbType)
            {
                case DBConfig.DatabaseType.SqlServer:
                    res = new SqlParameter[Count];
                    Parallel.For(0,Count,
                        (e) =>
                        {
                            res[e] = new SqlParameter();
                        });
                    break;
                default:
                    res = new OleDbParameter[Count];
                    Parallel.For(0,Count,
                        (e) =>
                        {
                            res[e] = new OleDbParameter();
                        });
                    break;
            }
            return res;
        }
    }
}
