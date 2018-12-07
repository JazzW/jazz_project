using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebFrameWork.ADO.Models
{
    public abstract class IDBModel
    {
        /// <summary>
        /// 非空字典设置默认值
        /// </summary>
        public virtual void SetDefualt()
        {
            Type T = this.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                if (!attr.Null)
                {
                    if(f.GetType()==typeof(string))
                    {
                        f.SetValue(this,"");
                    }
                    else if(f.GetType()== typeof(DateTime))
                    {
                        f.SetValue(this,DateTime.Now);
                    }
                    else if(f.GetType()== typeof(int))
                    {
                        f.SetValue(this, -1);
                    }
                    else if (f.GetType() == typeof(int))
                    {
                        f.SetValue(this, -1f);
                    }
                    else
                    {
                        f.SetValue(this, null);
                    }
                    
                }
            }
        }

        /// <summary>
        /// 转字典
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string,object> toDic()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type T = this.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                if(attr.Show)
                {
                    object val = f.GetValue(this);
                    var change = f.GetCustomAttributes(typeof(DBAttr.ChangeAttribute), true).ToArray();
                    if(change.Length>0)
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
        public virtual SqlParameter[] toParamters()
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            Type T = this.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                SqlParameter par = new SqlParameter("@" + f.Name, f.GetValue(this));
                if(attr.T!=null)
                    par.SqlDbType = attr.T;
                if (f.PropertyType == typeof(string))
                {
                    if(attr.Size>0)
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
        public virtual System.Data.DataRow toDataRow(System.Data.DataTable dt)
        {
           System.Data.DataRow row= dt.NewRow();
           Type T = this.GetType();
           var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length> 0).ToArray();
            foreach (var f in fields)
            {
                row[GetCol(new SqlParameter("@" + f.Name, null))] = f.GetValue(this);
            }
            return row;
        }

        /// <summary>
        /// 负责一行数据中的数据值
        /// </summary>
        /// <param name="row"></param>
        public virtual void CopyByDataRow(System.Data.DataRow row)
        {
            Type T = this.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                f.SetValue(this,row[GetCol(new SqlParameter("@" + f.Name,null))]);
            }
        }

        /// <summary>
        /// 通用检查
        /// </summary>
        /// <returns></returns>
        public virtual bool check()
        {
            Type T = this.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
              var attr= (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
              if (attr.Check && !attr.Null)
                { 
                    if(f.GetType()==typeof(string))
                    {
                        if (string.IsNullOrWhiteSpace(f.GetValue(this).ToString()) || f.GetValue(this).ToString().Length > attr.Size)
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
        [ScriptIgnore]
        public virtual SqlParameter[] Keys
        {
            get
            {
                List<SqlParameter> pars = new List<SqlParameter>();
                Type T = this.GetType();
                var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.KeyAttribute), true).Length > 0).ToArray();
                foreach (var f in fields)
                {
                    SqlParameter par = new SqlParameter("@" + f.Name, f.GetValue(this));
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
        }

        /// <summary>
        /// 自增型列
        /// </summary>
        [ScriptIgnore]
        public virtual string[] IdenityKey
        {
            get
            {
                List<string> pars = new List<string>();
                Type T = this.GetType();
                var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.IdenityKeyAttribute), true).Length > 0).ToArray();
                foreach (var f in fields)
                {
                    SqlParameter par = new SqlParameter("@" + f.Name, null);
                    pars.Add(GetCol(par));
                }
                return pars.ToArray();
            }
        }

        /// <summary>
        /// 获得列名
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public virtual string GetCol(SqlParameter par)
        {
            Type T = this.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.KeyAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                if("@" + f.Name==par.ParameterName)
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

        /// <summary>
        /// 获得显示名
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        public virtual string GetDisplay(SqlParameter par)
        {
            Type T = this.GetType();
            var fields = T.GetProperties().Where(e => e.GetCustomAttributes(typeof(DBAttr.KeyAttribute), true).Length > 0).ToArray();
            foreach (var f in fields)
            {
                if ("@" + f.Name == par.ParameterName)
                {
                    try
                    {
                        var attr = (DBAttr.DBAttribute)f.GetCustomAttributes(typeof(DBAttr.DBAttribute), true).First();
                        return attr.Display;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///导出多个显示列
        /// </summary>
        [ScriptIgnore]
        public virtual string ListCols
        {
            get { return " * "; }
        }

        /// <summary>
        ///导出单个显示列
        /// </summary>
        [ScriptIgnore]
        public virtual string ItemCols
        {
            get { return " * "; }
        }

        [ScriptIgnore]
        public virtual string TBName
        {
            get
            {
                try
                {
                    Type T = this.GetType();
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
        }

        [ScriptIgnore]
        public virtual string GoablStr
        {
            get
            {
                try
                {
                    Type T = this.GetType();
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
        }

        [ScriptIgnore]
        public virtual string[] SimpleCol
        {
            get
            {
                Type T = this.GetType();
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

        public virtual FiliterCollect[] getfiliterConfig()
        {
            return null;
        }

    }
}
