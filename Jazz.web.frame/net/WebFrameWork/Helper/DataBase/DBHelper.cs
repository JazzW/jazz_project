using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace WebFrameWork.Helper
{
    public class DbHelper
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DatabaseType DbType { get; set; }
        /// <summary>
        /// 数据库命名参数符号
        /// </summary>
        public static string DbParmChar { get; set; }

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
    }
}
