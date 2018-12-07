using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Jazz.Helper.DataBase.SQL;
using Jazz.Helper.DataBase.Common;
using Jazz.Common.Web;
using Jazz.Common.Enitiy;
using System.Reflection;


namespace Jazz.ADO.Frame.IClass
{

    /// <summary>
    /// 通用型数据逻辑层
    /// </summary>
    /// <typeparam name="T">继承BaseDBModel的数据模型</typeparam>
    public abstract class IDAL<T> : IRepository<T>
           where T : class,InterfaceDBModel
    {
        public virtual DBManger MyDBManger { get; set; }

        public System.Data.DataTable executeSQL(string sql,params System.Data.Common.DbParameter[] pars)
        {
            return this.MyDBManger.ExecuteDataTable(sql, pars);
        }

        public List<T> ISelectList<Tkey>(ITableConfig Config)
        {
           DbParameter[] pars;
           string sql=SQLCmdHelper.SelectFrame(
                EnitiyManager.GetListCols<T>(),EnitiyManager.TBName<T>(),
                Config.toSqlCmdExp<Tkey>(DBConfig.DbType,out pars),
                Config.toOrderSql());
           
           sql = SQLCmdHelper.PageFrame(sql, Config.Length, Config.Page);
           var dt= this.MyDBManger.ExecuteDataTable(sql, pars);


           if (dt.Rows.Count > 0)
           {
               List<T> list = new List<T>();
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   T mo = Activator.CreateInstance<T>();
                   mo.CopyByDataRow<T>(dt.Rows[i]);
                   list.Add(mo);
               }

               return list;
           }
           else
           {
               return null;
           }
        }

        public int ISelectListCount<Tkey>(ITableConfig Config)
        {
            DbParameter[] pars2;
            string totalsql = SQLCmdHelper.SelectToatlFrame(EnitiyManager.TBName<T>(),
                 Config.toSqlCmdExp<Tkey>(DBConfig.DbType, out pars2));
            return MyDBManger.ExecuteScalar(totalsql, pars2);

        }

        public Task<List<T>> ISelectListAsync<Tkey>(ITableConfig Config)
        {

            return 
            Task.Factory.StartNew<List<T>>(() =>
            {
                DbParameter[] pars;
                string sql = SQLCmdHelper.SelectFrame(
                     EnitiyManager.GetListCols<T>(), EnitiyManager.TBName<T>(),
                     Config.toSqlCmdExp<Tkey>(DBConfig.DbType, out pars),
                     Config.toOrderSql());

                sql = SQLCmdHelper.PageFrame(sql, Config.Length, Config.Page);
                var dt = this.MyDBManger.ExecuteDataTable(sql, pars);

                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = Activator.CreateInstance<T>();
                        mo.CopyByDataRow<T>(dt.Rows[i]);
                        list.Add(mo);
                    }

                    return list;
                }
                else
                {
                    return null;
                }
            });
        }

        public T ISelectFirst<Tkey>(ITableConfig Config)
        {
            DbParameter[] pars;
            string sql = SQLCmdHelper.SelectFrame(
                 EnitiyManager.GetListCols<T>(), EnitiyManager.TBName<T>(),
                 Config.toSqlCmdExp<Tkey>(DBConfig.DbType, out pars),
                 Config.toOrderSql());

            sql = SQLCmdHelper.PageFrame(sql, Config.Length, Config.Page);
            var dt = this.MyDBManger.ExecuteDataTable(sql, pars);
            if (dt.Rows.Count > 0)
            {
                T mo = Activator.CreateInstance<T>();
                mo.CopyByDataRow<T>(dt.Rows[0]);
                return mo;
            }
            else
            {
                return null;
            }
        }

        public bool IInsert(params T[] models)
        {
            var dict=models.toDic<T>(DBConfig.DbType==DBConfig.DatabaseType.MySql?true:false);
            List<DbParameter> pars=new List<DbParameter>();
            foreach(var dic in dict)
                foreach(var key in dic.Keys)
                    pars.Add(dic[key]);
             string sql = SQLCmdHelper.InsertListFrame(EnitiyManager.TBName<T>(),dict );
             return this.MyDBManger.ExecuteNonQuery(sql,pars.ToArray())>0?true :false;
        }

        public Task<bool> IInsertAsync(params T[] models)
        {
            return null;
        }

        public bool IUpdate(params T[] models)
        {
            var dict = models[0].toDic<T>(DBConfig.DbType == DBConfig.DatabaseType.MySql ? true : false);
            var keys = models[0].Keys(DBConfig.DbType == DBConfig.DatabaseType.MySql ? true : false);
            foreach (var key in keys)
                dict.Remove(EnitiyManager.GetCol<T>(key));
            List<DbParameter> pars = new List<DbParameter>();
            foreach (var key in dict.Keys)
                pars.Add(dict[key]);
            DbParameter[] pars2;
            string wheresql=  models[0].toWhereSql<T>(out pars2, DBConfig.DbType == DBConfig.DatabaseType.MySql ? true : false);
            pars.AddRange(pars2);
            return (MyDBManger.ExecuteNonQuery(SQLCmdHelper.UpdateFrame(EnitiyManager.TBName<T>(), dict, wheresql), pars.ToArray()) > 0 ? true : false);
        }

        public Task<bool> IUpdateAsync(params T[] models){ return null; }

        public bool IDelete(params T[] models)
        {
            DbParameter[] pars;
            string wheresql = models.toWhereSql<T>(out pars, DBConfig.DbType == DBConfig.DatabaseType.MySql ? true : false);
            return MyDBManger.ExecuteNonQuery(SQLCmdHelper.DeleteFrame(EnitiyManager.TBName<T>(), wheresql), pars) > 0 ? true : false;
        }

        public Task<bool> IDeleteAsync(params T[] models) { return null; }

    }
}
