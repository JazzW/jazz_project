using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Jazz.Common.Enitiy
{
   public  class EnitiyManagerByCache
    {/// <summary>
        /// 非空字典设置默认值
        /// </summary>
        public static void SetDefualt<TEnity>( TEnity model)
        {
            Type T = typeof(TEnity);
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                var attr = (IAttribute.DBItemAttribute)f.Value.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                if (!attr.Null)
                {
                    if (f.GetType() == typeof(string))
                    {
                        f.Value.SetValue(model, "");
                    }
                    else if (f.GetType() == typeof(DateTime))
                    {
                        f.Value.SetValue(model, DateTime.Now);
                    }
                    else if (f.GetType() == typeof(int))
                    {
                        f.Value.SetValue(model, -1);
                    }
                    else if (f.GetType() == typeof(int))
                    {
                        f.Value.SetValue(model, -1f);
                    }
                    else
                    {
                        f.Value.SetValue(model, null);
                    }

                }
            }
        }

        /// <summary>
        /// 转字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> toDic<TEnity>(TEnity model)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                object val = f.Value.GetValue(model);

                dic.Add(f.Key, val);
           
            }

            return dic;
        }

        public static Dictionary<string, System.Data.Common.DbParameter> toDic<TEnity>(TEnity model, bool IsMsSql = true)
        {
            Dictionary<string, System.Data.Common.DbParameter> dic = new Dictionary<string, System.Data.Common.DbParameter>();
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                object val = f.Value.GetValue(model);
                System.Data.Common.DbParameter par;
                if (IsMsSql)
                    par = new SqlParameter("@" +f.Key, val);
                else
                    par = new System.Data.OleDb.OleDbParameter("@" +f.Key, val);
                dic.Add(f.Key, par);
                
            }

            return dic;
        }

        public static Dictionary<string, System.Data.Common.DbParameter>[] toDic<TEnity>( TEnity[] models, bool IsMsSql = true)
        {
            List<Dictionary<string, DbParameter>> dics = new List<Dictionary<string, DbParameter>>();
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            int index = 0;
            foreach (var model in models)
            {
                Dictionary<string, DbParameter> dic = new Dictionary<string, DbParameter>();
                foreach (var f in fields)
                {
                    object val = f.Value.GetValue(model);
                    System.Data.Common.DbParameter par;
                    if (IsMsSql)
                        par = new SqlParameter("@" + f.Key + index.ToString(), val);
                    else
                        par = new System.Data.OleDb.OleDbParameter("@" + f.Key + index.ToString(), val);

                    dic.Add(f.Key, par);

                }
                dics.Add(dic);
                index++;
            }



            return dics.ToArray();
        }

        public static string toWhereSql<TEnity>(TEnity[] models, out DbParameter[] Outpars, bool IsMsSql = true)
        {
            string sqlstring = "";
            List<DbParameter> dbpars = new List<DbParameter>();
            int index = 0;
            foreach (var model in models)
            {
                var pars = EnitiyManagerByCache.Keys<TEnity>(model);
                string sql = "";
                foreach (var par in pars)
                {
                    string col = par.ParameterName.Replace("@", "");
                    par.ParameterName += index.ToString();
                    if (sql.Length == 0)
                        sql = string.Format("{0}={1}", col, par.ParameterName);
                    else
                        sql += string.Format("and {0}={1}", col, par.ParameterName);
                }
                if (sqlstring.Length == 1)
                    sqlstring = string.Format("({0})", sql);
                else
                    sqlstring += string.Format(" or ({0})", sql);
                dbpars.AddRange(pars);
                index++;
            }
            Outpars = dbpars.ToArray();
            return sqlstring;

        }

        public static string toWhereSql<TEnity>(TEnity model, out DbParameter[] pars, bool IsMsSql = true)
        {
            pars = EnitiyManagerByCache.Keys<TEnity>(model);
            string sql = "";
            foreach (var par in pars)
            {
                if (sql.Length == 0)
                    sql = string.Format("{0}={1}", par.ParameterName.Replace("@", ""), par.ParameterName);
                else
                    sql += string.Format("and {0}={1}", par.ParameterName.Replace("@", ""), par.ParameterName);
            }

            return sql;
        }

        /// <summary>
        /// 转换成sql参数
        /// </summary>
        /// <returns></returns>
        public static System.Data.Common.DbParameter[] toParamters<TEnity>(TEnity model, bool IsMsSql = true)
        {
            List<System.Data.Common.DbParameter> pars = new List<System.Data.Common.DbParameter>();
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                System.Data.Common.DbParameter par;
                object val = f.Value.GetValue(model);
                if (IsMsSql)
                    par = new SqlParameter("@" + f.Key, val);
                else
                    par = new System.Data.OleDb.OleDbParameter("@" + f.Key, val);
                pars.Add(par);
            }
            return pars.ToArray();
        }

        /// <summary>
        /// 生成表行
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.Data.DataRow toDataRow<TEnity>(  TEnity model, System.Data.DataTable dt)
        {
            System.Data.DataRow row = dt.NewRow();
            Type T = typeof(TEnity);
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                row[f.Key] = f.Value.GetValue(model);
            }
            return row;
        }

        /// <summary>
        /// 负责一行数据中的数据值
        /// </summary>
        /// <param name="row"></param>
        public static TEnity CopyByDataRow<TEnity>(TEnity model, System.Data.DataRow row)
        {
            var fields = Enitiy.EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                if (row[f.Key] is DBNull)
                    continue;
                f.Value.SetValue(model, row[f.Key]);
            }
            return model;
        }

        /// <summary>
        /// 通用检查
        /// </summary>
        /// <returns></returns>
        public static bool check<TEnity>(TEnity model)
        {
            Type T = model.GetType();
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                var attr = (IAttribute.DBItemAttribute)f.Value.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                if (attr.Check && !attr.Null)
                {
                    if (f.GetType() == typeof(string))
                    {
                        if (string.IsNullOrWhiteSpace(f.Value.GetValue(model).ToString()) || f.Value.GetValue(model).ToString().Length > attr.Size)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 多个主键
        /// </summary>

        public static DbParameter[] Keys<TEnity>(TEnity model, bool IsMsSql = true)
        {

            List<DbParameter> pars = new List<DbParameter>();
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                System.Data.Common.DbParameter par;
                object val = f.Value.GetValue(model);
                try
                {
                    if (IsMsSql)
                        par = new SqlParameter("@" + f.Key, val);
                    else
                        par = new System.Data.OleDb.OleDbParameter("@" + f.Key, val);
                    pars.Add(par);
                }
                catch
                {

                }
            }
            return pars.ToArray();

        }



        /// <summary>
        /// 自增型列
        /// </summary>

        public static string[] IdenityKey<TEnity>(TEnity model)
        {

            List<string> pars = new List<string>();
            Type T = model.GetType();
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                var attr = (IAttribute.KeyAttribute)f.Value.GetCustomAttributes(typeof(IAttribute.KeyAttribute), true).First();
                if (attr.IdenityKey)
                {
                    SqlParameter par = new SqlParameter("@" + f.Key, null);
                    pars.Add(f.Key);
                }
            }
            return pars.ToArray();

        }


        /// <summary>
        /// 获得列名
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public static string GetCol<TEnity>(DbParameter par)
        {
            Type T = typeof(TEnity);
            var fields = EnityCachePool.getCache<TEnity>().getfieldMap();
            foreach (var f in fields)
            {
                if ("@" + f.Value.Name == par.ParameterName)
                {
                    try
                    {
                        return f.Key;
                    }
                    catch
                    {
                        return f.Value.Name;
                    }
                }
            }
            return null;
        }


        public static string TBName<TEnity>()
        {

            return EnityCachePool.getCache<TEnity>().getTBName();

        }


        public static string GoablStr<TEnity>()
        {

            try
            {
                Type T = typeof(TEnity);
                IAttribute.DBModelAttribute tb = (IAttribute.DBModelAttribute)T.GetCustomAttributes(typeof(IAttribute.DBModelAttribute), true).FirstOrDefault();
                if (tb != null)
                    return tb.TBMess;
                else
                    return null;
            }
            catch
            {
                return null;
            }

        }


        public static string[] SimpleCol<TEnity>()
        {

            return EnityCachePool.getCache<TEnity>().getSimpleCol();

        }

        public static string GetItemCols<TEnity>()
        {

            return EnityCachePool.getCache<TEnity>().getItemCols();


        }

        public static string GetListCols<TEnity>()
        {

            return EnityCachePool.getCache<TEnity>().getListCols();

        }
    }
}
