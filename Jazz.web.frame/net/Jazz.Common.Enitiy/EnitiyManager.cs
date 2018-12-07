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
    public static class EnitiyManager
    {
        public static bool useCache = false;

        /// <summary>
        /// 非空字典设置默认值
        /// </summary>
        public static void SetDefualt<TEnity>(this TEnity model)
        {
            if (useCache)
            {
                EnitiyManagerByCache.SetDefualt<TEnity>(model);
                return;
            }
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                if (!attr.Null)
                {
                    if (f.GetType() == typeof(string))
                    {
                        f.SetValue(model, "");
                    }
                    else if (f.GetType() == typeof(DateTime))
                    {
                        f.SetValue(model, DateTime.Now);
                    }
                    else if (f.GetType() == typeof(int))
                    {
                        f.SetValue(model, -1);
                    }
                    else if (f.GetType() == typeof(int))
                    {
                        f.SetValue(model, -1f);
                    }
                    else
                    {
                        f.SetValue(model, null);
                    }

                }
            }
        }

        /// <summary>
        /// 转字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> toDic<TEnity>(this TEnity model)
        {
            if (useCache)
                return EnitiyManagerByCache.toDic<TEnity>(model);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                if (attr.Show)
                {
                    object val = f.GetValue(model);

                   dic.Add(f.Name, val);
                    
                }
            }

            return dic;
        }

        public static Dictionary<string, System.Data.Common.DbParameter> toDic<TEnity>(this TEnity model,bool IsMsSql=true)
        {
            if (useCache)
                return EnitiyManagerByCache.toDic<TEnity>(model, IsMsSql);
            Dictionary<string, System.Data.Common.DbParameter> dic = new Dictionary<string, System.Data.Common.DbParameter>();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                if (attr.Show)
                {
                    object val = f.GetValue(model);
                    System.Data.Common.DbParameter par;
                    if (IsMsSql)
                        par = new SqlParameter("@"+attr.ColName, val);
                    else
                        par = new System.Data.OleDb.OleDbParameter("@" + attr.ColName, val);
                    if (f.PropertyType == typeof(string))
                    {
                        if (attr.Size > 0)
                            par.Size = attr.Size;
                    }
                    dic.Add(attr.ColName, par);

                }
            }

            return dic;
        }

        public static Dictionary<string, System.Data.Common.DbParameter>[] toDic<TEnity>(this TEnity[] models, bool IsMsSql = true)
        {
            if (useCache)
                return EnitiyManagerByCache.toDic<TEnity>(models, IsMsSql);
            List<Dictionary<string, DbParameter>> dics = new List<Dictionary<string, DbParameter>>();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
  
            int index=0;
            foreach (var model in models)
            {
                Dictionary<string, DbParameter> dic = new Dictionary<string,DbParameter>();
                foreach (var f in fields)
                {
                    var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
        
                    object val = f.GetValue(model);
                    System.Data.Common.DbParameter par;
                    if (IsMsSql)
                        par = new SqlParameter("@" + attr.ColName+index.ToString(), val);
                    else
                        par = new System.Data.OleDb.OleDbParameter("@" + attr.ColName+index.ToString(), val);
                    if (f.PropertyType == typeof(string))
                    {
                        if (attr.Size > 0)
                            par.Size = attr.Size;
                    }
                    dic.Add(attr.ColName, par);
                    
                }
                dics.Add(dic);
                index++;
            }

  

            return dics.ToArray();
        }

        public static string toWhereSql<TEnity>(this TEnity[] models,out DbParameter[] Outpars, bool IsMsSql = true)
        {
            if (useCache)
                return EnitiyManagerByCache.toWhereSql<TEnity>(models, out Outpars, IsMsSql);
            string sqlstring="";
            List<DbParameter> dbpars=new List<DbParameter>();
            int index = 0;
            foreach (var model in models)
            {
                var pars = model.Keys<TEnity>(IsMsSql);
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
                    sqlstring+= string.Format(" or ({0})", sql);
                dbpars.AddRange(pars);
                index++;
            }
            Outpars = dbpars.ToArray();
            return sqlstring;

        }

        public static string toWhereSql<TEnity>(this TEnity model,out DbParameter[] pars, bool IsMsSql = true)
        {
            if (useCache)
                return EnitiyManagerByCache.toWhereSql<TEnity>(model, out pars, IsMsSql);
            pars = model.Keys<TEnity>(IsMsSql);
            string sql="";
            foreach(var par in pars)
            {
                if(sql.Length==0)
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
        public static System.Data.Common.DbParameter[] toParamters<TEnity>(this TEnity model, bool IsMsSql = true)
        {
            if (useCache)
                return EnitiyManagerByCache.toParamters<TEnity>(model, IsMsSql);
            List<System.Data.Common.DbParameter> pars = new List<System.Data.Common.DbParameter>();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                System.Data.Common.DbParameter par;
                object val = f.GetValue(model);
                if (IsMsSql)
                    par = new SqlParameter("@" + attr.ColName, val);
                else
                    par = new System.Data.OleDb.OleDbParameter("@" + attr.ColName, val);
                if (f.PropertyType == typeof(string))
                {
                    if (attr.Size > 0)
                        par.Size = attr.Size;
                }
                pars.Add(par);
            }
            return pars.ToArray();
        }

        /// <summary>
        /// 生成表行
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static System.Data.DataRow toDataRow<TEnity>(this  TEnity model, System.Data.DataTable dt)
        {
            if (useCache)
                return EnitiyManagerByCache.toDataRow<TEnity>(model, dt);
            System.Data.DataRow row = dt.NewRow();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                row[GetCol<TEnity>(new SqlParameter("@" + f.Name, null))] = f.GetValue(model);
            }
            return row;
        }

        /// <summary>
        /// 负责一行数据中的数据值
        /// </summary>
        /// <param name="row"></param>
        public static TEnity CopyByDataRow<TEnity>(this TEnity model, System.Data.DataRow row)
        {
            if (useCache)
                return EnitiyManagerByCache.CopyByDataRow<TEnity>(model, row);
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                string col=EnitiyManager.GetCol<TEnity>(new SqlParameter("@" + f.Name, null));
                if (row[col] is DBNull)
                    continue;
                f.SetValue(model, row[col]);
            }
            return model;
        }

        /// <summary>
        /// 通用检查
        /// </summary>
        /// <returns></returns>
        public static bool check<TEnity>(this TEnity model)
        {
            if (useCache)
                return EnitiyManagerByCache.check<TEnity>(model);
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                if (attr.Check && !attr.Null)
                {
                    if (f.GetType() == typeof(string))
                    {
                        if (string.IsNullOrWhiteSpace(f.GetValue(model).ToString()) || f.GetValue(model).ToString().Length > attr.Size)
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

        public static DbParameter[] Keys<TEnity>(this TEnity model, bool IsMsSql=true)
        {
            if (useCache)
                return EnitiyManagerByCache.Keys<TEnity>(model, IsMsSql);
            List<DbParameter> pars = new List<DbParameter>();
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.KeyAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                System.Data.Common.DbParameter par;
                object val = f.GetValue(model);
                try
                {
                    var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                    if (IsMsSql)
                        par = new SqlParameter("@" + attr.ColName, val);
                    else
                        par = new System.Data.OleDb.OleDbParameter("@" + attr.ColName, val);
                    if (f.PropertyType == typeof(string))
                    {
                        if (attr.Size > 0)
                            par.Size = attr.Size;
                    }
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

        public static string[] IdenityKey<TEnity>(this TEnity model)
        {
            if (useCache)
                return EnitiyManagerByCache.IdenityKey<TEnity>(model);
            List<string> pars = new List<string>();
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.KeyAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (IAttribute.KeyAttribute)f.GetCustomAttributes(typeof(IAttribute.KeyAttribute), true).First();
                if(attr.IdenityKey)
                {
                    SqlParameter par = new SqlParameter("@" + f.Name, null);
                    pars.Add(GetCol<TEnity>(par));
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
            if (useCache)
                return EnitiyManagerByCache.GetCol<TEnity>(par);
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                if ("@" + f.Name == par.ParameterName)
                {
                    try
                    {
                        var attr = (IAttribute.DBItemAttribute)f.GetCustomAttributes(typeof(IAttribute.DBItemAttribute), true).First();
                        return attr.ColName;
                    }
                    catch
                    {
                        return f.Name;
                    }
                }
            }
            return null;
        }


        public static string TBName<TEnity>()
        {
            if (useCache)
                return EnitiyManagerByCache.TBName<TEnity>();
            try
            {
                Type T = typeof(TEnity);
                IAttribute.DBModelAttribute tb = (IAttribute.DBModelAttribute)T.GetCustomAttributes(typeof(IAttribute.DBModelAttribute), true).FirstOrDefault();
                if (tb != null)
                    return tb.TBName;
                else
                    return null;
            }
            catch
            {
                return null;
            }

        }


        public static string GoablStr<TEnity>()
        {
            if (useCache)
                return EnitiyManagerByCache.GoablStr<TEnity>();
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
            if (useCache)
                return EnitiyManagerByCache.SimpleCol<TEnity>();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(IAttribute.SearchKeyAttribute), true).Length > 0).ToArray();
            string[] ouput = new string[fields.Length];
            int i = 0;
            foreach (var f in fields)
            {
                ouput[i] = f.Name;
                i++;
            }
            return ouput;

        }

        public static string GetItemCols<TEnity>()
        {
            if (useCache)
                return EnitiyManagerByCache.GetItemCols<TEnity>();
            Type T = typeof(TEnity);
            IAttribute.DBModelAttribute tb = (IAttribute.DBModelAttribute)T.GetCustomAttributes(typeof(IAttribute.DBModelAttribute), true).FirstOrDefault();
            if (tb != null)
                return tb.ItemCols;
            else
                return null;
            

        }

        public static string GetListCols<TEnity>()
        {
            if (useCache)
                return EnitiyManagerByCache.GetListCols<TEnity>();
            Type T = typeof(TEnity);
            IAttribute.DBModelAttribute tb = (IAttribute.DBModelAttribute)T.GetCustomAttributes(typeof(IAttribute.DBModelAttribute), true).FirstOrDefault();
            if (tb != null)
                return tb.ListCols;
            else
                return null;

        }
    }
}
