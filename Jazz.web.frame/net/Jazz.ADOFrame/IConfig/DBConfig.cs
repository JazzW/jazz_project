using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using database = Jazz.Helper.DataBase;

namespace Jazz.ADOFrame.IConfig
{
    public class DBConfig
    {
        public static void set(database.Common.DBConfig.DatabaseType dbtype,string connect,database.Common.IDataBase db)
        {
            database.Common.DBConfig.DbType = dbtype;
            database.Common.DBConfig.ConnectionString = connect;
            database.Common.DBConfig.set(dbtype,db);
        }
    }
}
