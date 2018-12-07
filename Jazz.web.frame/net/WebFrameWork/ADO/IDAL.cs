using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using WebFrameWork.Helper;

namespace WebFrameWork.ADO
{

    /// <summary>
    /// 通用型数据逻辑层
    /// </summary>
    /// <typeparam name="T">继承BaseDBModel的数据模型</typeparam>
    public abstract class IBaseDAL<T> where T : Models.IDBModel
    {
        public string ITable { get; set; }

        /// <summary>
        /// 数据表单原操做
        /// </summary>
        /// <param name="model">执行对象</param>
        /// <param name="opCode">执行编码 1:update  2:delete 3:add 4:select</param>
        /// <returns></returns>
        public delegate bool DBaction(T model, int opCode);

        /// <summary>
        /// 错误操做
        /// </summary>
        /// <param name="model"></param>
        /// <param name="opCode"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public delegate bool ExHandle(T model, int opCode, Exception ex);

        /// <summary>
        /// 通用插入模型
        /// </summary>
        /// <param name="model"></param>
        public void IInsert(T model)
        {
            string sqlstr = @"Insert into dbo.[{0}]({2}) values ({1})";
            SqlParameter[] pars = model.toParamters();
            string parstr = "";
            string colstr = "";
            foreach (var par in pars)
            {
                if (!model.IdenityKey.Contains(model.GetCol(par)) && par.Value!=null)
                {
                    parstr += "," + par.ParameterName;
                    colstr += "," + model.GetCol(par);
                }
            }
            if (parstr.Length > 0)
                parstr = parstr.Remove(0, 1);
            if (colstr.Length > 0)
                colstr = colstr.Remove(0, 1);
            sqlstr = string.Format(sqlstr, ITable, parstr,colstr);
            if (SQLDBHelper.ExecuteNonQuery(sqlstr, pars) == 0)
            {
                throw new Exception("插入失败");
            }
        }

        public void IInserts(T[] models)
        {
            string sqlstr = @"Insert into dbo.[{0}] ({2}) values {1}";
            string parstr = "";
            string colstr = "";
            List<SqlParameter> sqlpar = new List<SqlParameter>();

            SqlParameter[] cols = models[0].toParamters();
            foreach (var par in cols)
            {
                if (!models[0].IdenityKey.Contains(models[0].GetCol(par)))
                {
                    colstr += "," + models[0].GetCol(par);
                }
            }
            if (colstr.Length > 0)
                colstr = colstr.Remove(0, 1);

            int ri = 0;
            foreach (var model in models)
            {
                SqlParameter[] pars = model.toParamters();
                string str = "";
                pars.AsParallel().ForAll((e) => e.ParameterName = e.ParameterName + ri.ToString());
                foreach (var par in pars)
                {
                    if (!model.IdenityKey.Contains(model.GetCol(par).Replace(ri.ToString(), "")))
                    {
                        str += "," + par.ParameterName;
                    }
                }
                if (str.Length > 0)
                    str = str.Remove(0, 1);
                parstr += string.Format(",({0})", str);
                sqlpar.AddRange(pars);
                ri++;
            }
            if (parstr.Length > 0)
                parstr = parstr.Remove(0, 1);
            sqlstr = string.Format(sqlstr, ITable, parstr, colstr);
            if (SQLDBHelper.ExecuteNonQuery(sqlstr, sqlpar.ToArray()) == 0)
            {
                throw new Exception("插入失败");
            }
        }

        /// <summary>
        /// 通用删除模型
        /// </summary>
        /// <param name="model"></param>
        public void IDelete(T model)
        {
            string sqlstr = "Delete from dbo.[{0}] where {1}";
            SqlParameter[] keys = model.Keys;
            string keystr = "";
            foreach (var key in keys)
            {
                if (keystr.Length > 0)
                    keystr += string.Format(" and [{0}]={1} ", model.GetCol(key), key.ParameterName);
                else
                    keystr = string.Format("[{0}]={1} ", model.GetCol(key), key.ParameterName);
            }
            sqlstr = string.Format(sqlstr, ITable, keystr);
            if (SQLDBHelper.ExecuteNonQuery(sqlstr, keys) == 0)
            {
                throw new Exception("插入失败");
            }

        }

        /// <summary>
        /// 通用更新模型，不更新主键信息
        /// </summary>
        /// <param name="model">更新模型</param>
        /// <param name="key"></param>
        public void IUpdate(T model, string[] key)
        {
            string sqlstr = "update dbo.[{0}] set {1} where {2}";
            SqlParameter[] pars = model.toParamters();

            string keystr = "";
            SqlParameter[] keys = model.Keys;
            int i = 0;
            foreach (var tkey in keys)
            {
                tkey.Value = key[i];
                if (keystr.Length > 0)
                    keystr += string.Format(" and [{0}]={1} ", model.GetCol(tkey), tkey.ParameterName);
                else
                    keystr = string.Format(" [{0}]={1} ", model.GetCol(tkey), tkey.ParameterName);
            }
            string updateStr = "";

            foreach (var par in pars)
            {
                if (!model.Keys.Any(e => e.ParameterName == par.ParameterName))
                    updateStr += string.Format(",{0}={1}", model.GetCol(par), par.ParameterName);
            }
            if (updateStr.Length > 0)
            {
                updateStr = updateStr.Remove(0, 1);
                sqlstr = string.Format(sqlstr, ITable, updateStr, keystr);
                if (SQLDBHelper.ExecuteNonQuery(sqlstr, pars) == 0)
                {
                    throw new Exception("更新失败");
                }
                return;
            }
            throw new Exception("更新失败");
        }

        /// <summary>
        /// 通过获得对象
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public T ISelectEnity(string[] key, T model)
        {
            string sqlstr = "select {2} from dbo.[{0}] where {1}";
            string keystr = "";
            string colstr = model.ItemCols;
            SqlParameter[] keys = model.Keys;
            int i = 0;
            foreach (var tkey in keys)
            {
                tkey.Value = key[i];
                i++;
                if (keystr.Length > 0)
                    keystr += string.Format(" and [{0}]={1} ", model.GetCol(tkey), tkey.ParameterName);
                else
                    keystr = string.Format(" [{0}]={1} ", model.GetCol(tkey), tkey.ParameterName);
            }
            sqlstr = string.Format(sqlstr, ITable, keystr, colstr);
            DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr, keys);
            if (dt.Rows.Count > 0)
                model.CopyByDataRow(dt.Rows[0]);
            else
                throw new Exception("查询错误");
            return model;
        }

        /// <summary>
        /// 含租房的通用获得对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="TColName">租户列名</param>
        /// <param name="TValue">租户数据</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public T ISelectEnity(string[] key, string TColName, string TValue, T model)
        {
            string sqlstr = "select {4} from dbo.[{0}] where [{1}]='{2}' and {3}";
            string colstr = model.ItemCols;
            string keystr = "";
            SqlParameter[] keys = model.Keys;
            int i = 0;
            foreach (var tkey in keys)
            {
                tkey.Value = key[i];
                i++;
                if (keystr.Length > 0)
                    keystr += string.Format("and [{0}]={1} ", model.GetCol(tkey), tkey.ParameterName);
                else
                    keystr = string.Format(" [{0}]={1} ", model.GetCol(tkey), tkey.ParameterName);
            }
            sqlstr = string.Format(sqlstr, ITable, keystr, TColName, TValue, colstr);
            DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr, keys);
            if (dt.Rows.Count > 0)
                model.CopyByDataRow(dt.Rows[0]);
            else
                throw new Exception("查询错误");
            return model;
        }

        /// <summary>
        /// 通用获得筛选统计
        /// </summary>
        /// <param name="filters">过滤条件</param>
        /// <param name="CreateFunc">初始化对象方法</param>
        /// <param name="type">or？and</param>
        /// <returns></returns>
        public int IGetCount(Models.SQLFiliter[] filters, Func<T> CreateFunc, string type)
        {
            string sqlstr = string.Format("Select Count(*) from [{0}]", ITable);
            T model = CreateFunc();

            if (filters == null || filters.Length == 0)
            {
                return SQLDBHelper.ExecuteScalar(sqlstr);

            }
            else
            {
                string sqlf = "";
                SqlParameter[] pars = new SqlParameter[filters.Length];
                for (int i = 0; i < filters.Length; i++)
                {
                    pars[i] = filters[i].Par;
                    if (i > 0)
                        sqlf += type + filters[i].ToString();
                    else
                        sqlf += filters[i].ToString();
                }

                sqlstr += " where "+sqlf;

                return SQLDBHelper.ExecuteScalar(sqlstr, pars);
            }
        }

        /// <summary>
        /// 含租户通用获得筛选统计
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="CreateFunc"></param>
        /// <param name="TColName"></param>
        /// <param name="TValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int IGetCount(Models.SQLFiliter[] filters, Func<T> CreateFunc, string TColName, string TValue, string type)
        {
            string sqlstr = string.Format("Select Count(*) from [{0}] Where [{1}]='{2}'", ITable, TColName, TValue);
            T model = CreateFunc();
            if (filters == null || filters.Length == 0)
            {
                return SQLDBHelper.ExecuteScalar(sqlstr);
            }
            else
            {
                string sqlf = "";
                SqlParameter[] pars = new SqlParameter[filters.Length];
                for (int i = 0; i < filters.Length; i++)
                {
                    pars[i] = filters[i].Par;
                    if (i > 0)
                        sqlf += type + filters[i].ToString();
                    else
                        sqlf += filters[i].ToString();
                }

                sqlstr += "and (" + sqlf + ")";
                return SQLDBHelper.ExecuteScalar(sqlstr, pars);
            }
        }

        /// <summary>
        /// 通用获得筛选列表
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="CreateFunc"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public T[] ISeleteEnitys(Models.SQLFiliter[] filters, Func<T> CreateFunc, string type,
            int page = -1, int length = -1,string simpleSearch=null)
        {

            T model = CreateFunc();
            string colstr = model.ListCols;
            string sqlstr = string.Format("Select {1} from [{0}]", ITable, colstr);
            if (filters == null || filters.Length == 0)
            {
                if (page > 0 && length > 0)
                {
                    sqlstr = @"select {4} from dbo.[{0}] where {1} in(select top {3} [{1}] from {0}) 
                                                    and {1} not in(select top {2} [{1}] from {0}) 
                                                    ";

                    sqlstr = string.Format(sqlstr, ITable, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, colstr);
                }

                DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = CreateFunc();
                        mo.CopyByDataRow(dt.Rows[i]);
                        list.Add(mo);
                    }

                    return list.ToArray();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string sqlf = "";
                SqlParameter[] pars = new SqlParameter[filters.Length];
                for (int i = 0; i < filters.Length; i++)
                {
                    pars[i] = filters[i].Par;
                    if (i > 0)
                        sqlf += type + filters[i].ToString();
                    else
                        sqlf += filters[i].ToString();
                }
                if (page > 0 && length > 0)
                {
                    sqlstr = @"select {5} from dbo.[{0}] where [{1}] in(select top {3} [{1}] from {0} where {4}) 
                                                    and [{1}] not in(select top {2} [{1}] from {0} where {4}) 
                                                    and {4}";

                    sqlstr = string.Format(sqlstr, ITable, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, sqlf, colstr);
                }
                else
                {
                    sqlstr +=" where "+sqlf;
                }
                DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr, pars);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = CreateFunc();
                        mo.CopyByDataRow(dt.Rows[i]);
                        list.Add(mo);
                    }

                    return list.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 含租户通用获得筛选列表
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="CreateFunc"></param>
        /// <param name="TColName"></param>
        /// <param name="TValue"></param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public T[] ISeleteEnitys(Models.SQLFiliter[] filters, Func<T> CreateFunc, string TColName, string TValue,
            string type, int page = -1, int length = -1, string simpleSearch = null)
        {
            T model = CreateFunc();
            string colstr = model.ListCols;
            string sqlstr = string.Format("Select {3} from [{0}] Where [{1}]='{2}'", ITable, TColName, TValue, colstr);
            if (filters == null || filters.Length == 0)
            {
                if (page > 0 && length > 0)
                {
                    sqlstr = @"select {6} from dbo.[{0}] where [{1}] in(select top {3} [{1}] from {0} Where {4}='{5}') 
                                                    and [{1}] not in(select top {2} [{1}] from {0} Where {4}='{5}') 
                                                    and {4}='{5}'";

                    sqlstr = string.Format(sqlstr, ITable, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, TColName, TValue, colstr);
                }

                DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = CreateFunc();
                        mo.CopyByDataRow(dt.Rows[i]);
                        list.Add(mo);
                    }

                    return list.ToArray();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                string sqlf = "";
                SqlParameter[] pars = new SqlParameter[filters.Length];
                for (int i = 0; i < filters.Length; i++)
                {
                    pars[i] = filters[i].Par;
                    if (i > 0)
                        sqlf += type + filters[i].ToString();
                    else
                        sqlf += filters[i].ToString();
                }
                if (page > 0 && length > 0)
                {
                    sqlstr = @"select {7} from dbo.[{0}] where [{1}] in(select top {3} [{1}] from {0} where {5}='{6}' and ({4})) 
                                                    and [{1}] not in(select top {2} [{1}] from {0} where {5}='{6}' and ({4})) 
                                                   and {5}='{6}' and ({4})";

                    sqlstr = string.Format(sqlstr, ITable, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, sqlf, TColName, TValue, colstr);
                }
                else
                {
                    sqlstr += "and (" + sqlf + ")";
                }
                DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr, pars);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = CreateFunc();
                        mo.CopyByDataRow(dt.Rows[i]);
                        list.Add(mo);
                    }

                    return list.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        public T[] ISeleteEnitys(Models.FilterConfig[] config, Func<T> CreateFunc,int page = -1, int length = -1)
        {
            if (config == null || config.Length==0)
            {
                return this.ISeleteEnitys(null, CreateFunc, "", page, length);
            }
            else
            {
                T model=CreateFunc();
                string sqlf = "";
                List<SqlParameter> SQLpars = new List<SqlParameter>();
                foreach (var fig in config)
                {
                    string ty=" and ";
                    Models.SQLFiliter[] filiter = fig.toFiliter(model.GoablStr, ref ty);
                    if (filiter == null)
                        continue;
                    string strf = "";
                    List<SqlParameter> pars = new List<SqlParameter>();
                    foreach(var fi in filiter)
                    {
                        strf += ty + fi.ToString();
                        pars.Add(fi.Par);
                    }
                    if (strf.Length > 0)
                        strf = strf.Remove(0, ty.Length);

                    sqlf += string.Format(" ({0}) ", strf);
                    SQLpars.AddRange(pars);
                }
                string sql="";
                if (page > 0 && length > 0)
                {
                    sql = @"select {5} from dbo.[{0}] where [{1}] in(select top {3} [{1}] from {0} where {4}) 
                                                    and [{1}] not in(select top {2} [{1}] from {0} where {4}) 
                                                    and {4}";

                    sql = string.Format(sql, ITable, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, sqlf, model.ListCols);
                }
                else
                {
                    sql = @"select {1} from {0} where {2}";
                    sql = string.Format(sql, this.ITable, model.ListCols, sqlf);
                }
                DataTable dt = SQLDBHelper.ExecuteDataTable(sql, SQLpars.ToArray());
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = CreateFunc();
                        mo.CopyByDataRow(dt.Rows[i]);
                        list.Add(mo);
                    }

                    return list.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
