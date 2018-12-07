using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.ADOFrame.Interface;
using Jazz.ADOFrame.IAttribute;
using System.Data.SqlClient;
using Jazz.Common.Web;

namespace Jazz.ADOFrame.IModel
{
    public  class EnityManage<TEnity>where TEnity:InterfaceDBModel
    {
        /// <summary>
        /// 非空字典设置默认值
        /// </summary>
        public static void SetDefualt(TEnity model)
        {
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
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
        public static Dictionary<string, object> toDic(TEnity model)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                if (attr.Show)
                {
                    object val = f.GetValue(model);
                    var change = f.GetCustomAttributes(typeof(DBAttr.ChangeAttribute), true).ToArray();
                    if (change.Length > 0)
                    {
                        dic.Add(f.Name, ((DBAttr.ChangeAttribute)change[0]).ChangeFunc(val));
                    }
                    else
                    {
                        dic.Add(f.Name, val);
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 转换成sql参数
        /// </summary>
        /// <returns></returns>
        public static SqlParameter[] toParamters(TEnity model)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                SqlParameter par = new SqlParameter("@" + f.Name, f.GetValue(model));
                if (attr.T != null)
                    par.SqlDbType = attr.T;
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
        public static System.Data.DataRow toDataRow(System.Data.DataTable dt,TEnity model)
        {
            System.Data.DataRow row = dt.NewRow();
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                row[GetCol(new SqlParameter("@" + f.Name, null))] = f.GetValue(model);
            }
            return row;
        }

        /// <summary>
        /// 负责一行数据中的数据值
        /// </summary>
        /// <param name="row"></param>
        public static TEnity CopyByDataRow(System.Data.DataRow row, TEnity model)
        {
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                f.SetValue(model, row[EnityManage < TEnity > .GetCol(new SqlParameter("@" + f.Name, null))]);
            }
            return model;
        }

        /// <summary>
        /// 通用检查
        /// </summary>
        /// <returns></returns>
        public static bool check(TEnity model)
        {
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
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

        public static SqlParameter[] Keys(TEnity model)
        {

            List<SqlParameter> pars = new List<SqlParameter>();
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.KeyAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                SqlParameter par = new SqlParameter("@" + f.Name, f.GetValue(model));
                try
                {
                    var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                    if (f.PropertyType == typeof(string))
                    {
                        if (attr.Size > 0)
                            par.Size = attr.Size;
                    }
                }
                catch
                {

                }
                pars.Add(par);
            }
            return pars.ToArray();
  
        }

        /// <summary>
        /// 自增型列
        /// </summary>

        public static string[] IdenityKey(TEnity model)
        {

            List<string> pars = new List<string>();
            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.IdenityKeyAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                SqlParameter par = new SqlParameter("@" + f.Name, null);
                pars.Add(GetCol(par));
            }
            return pars.ToArray();
            
        }


        /// <summary>
        /// 获得列名
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public static string GetCol(SqlParameter par)
        {
            Type T = typeof(TEnity);
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                if ("@" + f.Name == par.ParameterName)
                {
                    try
                    {
                        var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                        return attr.ColName;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

      
        public virtual string TBName(TEnity model)
        {

            try
            {
                Type T = model.GetType();
                DBAttr.TBAttribute tb = (DBAttr.TBAttribute)T.GetCustomAttributes(typeof(DBAttr.TBAttribute), true).FirstOrDefault();
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


        public static  string GoablStr(TEnity model)
        {

            try
            {
                Type T = model.GetType();
                DBAttr.TBAttribute tb = (DBAttr.TBAttribute)T.GetCustomAttributes(typeof(DBAttr.TBAttribute), true).FirstOrDefault();
                if (tb != null)
                    return tb.ColGobalStr;
                else
                    return null;
            }
            catch
            {
                return null;
            }
            
        }


        public static string[] SimpleCol(TEnity model)
        {

            Type T = model.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.SearchKeyAttribute), true).Length > 0).ToArray();
            string[] ouput = new string[fields.Length];
            int i = 0;
            foreach (var f in fields)
            {
                ouput[i] = f.Name;
                i++;
            }
            return ouput;
 
        }


    }
}
