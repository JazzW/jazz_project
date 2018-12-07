using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.Web;
using DB=Jazz.Helper.DataBase;

namespace Jazz.ADO.Frame.IClass
{
    public class RepositoryFactory
    {
        public static IRepository<T> getRepository<T>() where T:class,InterfaceDBModel 
        {
            return new ADORepository<T>();
        }
    }

    public class ADORepository<T> : IDAL<T> where T :class, InterfaceDBModel 
    {
        public ADORepository()
        {
            DB.Common.DBConfig.ConnectionString = IConfig.ConnectString;
            DB.Common.DBConfig.DbType = IConfig.DbType;
            DB.Common.IDataBase db;
            switch(IConfig.DbType)
            {
                case DB.Common.DBConfig.DatabaseType.SqlServer:
                    db=new DB.SQL.MSSQL();
                    break;
                default:
                    db=new DB.SQL.MSSQL();
                    break;
            }
            DB.Common.DBConfig.set(IConfig.DbType, db);
            this.MyDBManger = new DB.Common.DBManger(db);
        }
    }
}
