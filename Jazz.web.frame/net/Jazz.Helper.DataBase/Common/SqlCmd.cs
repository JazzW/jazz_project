using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Jazz.Helper.DataBase.Common
{
   public enum DBActionTypeEnum
    {
        select,
        insert,
        update,
        delete,
        statis,
        exec,
    }

    public class SqlCmd
    {
        public string sql { get; set; }

        public DbParameter[] pars { get; set; }

        public DBActionTypeEnum actionType { get; set; }

        public object execute(IDataBase db)
        {
            object res = null;
            switch (actionType)
            {
                case DBActionTypeEnum.select:
                    res = db.ExecuteDataTable(this.sql, pars);
                    if (res == null)
                        throw new Exception("查询错误");
                    break;
                case DBActionTypeEnum.statis:
                    res = db.ExecuteScalar(sql, pars);
                    if(Convert.ToInt32(res)<0)
                        throw new Exception("查询错误");
                    break;
                default:
                    res = db.ExecuteNonQuery(this.sql, null, pars);
                    if (Convert.ToInt32(res) < 0)
                        throw new Exception("查询错误");
                    break;
            }
            return res;
        }

        public T execute<T>(IDataBase db)
        {
            object res = null;
            switch (actionType)
            {
                case DBActionTypeEnum.select:
                    res = db.ExecuteDataTable(this.sql, pars);
                    break;
                case DBActionTypeEnum.statis:
                    res = db.ExecuteScalar(sql, pars);
                    break;
                default :
                    res = db.ExecuteNonQuery(this.sql, null, pars);
                    break;
            }
            return (T)res;
        }
    }
}
