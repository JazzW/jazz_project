using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WebFrameWork.Helper;
using WebFrameWork.ADO.Models;

namespace WebFrameWork.ADO
{
    public  class MBaseDAL<T> where T : Models.IDBModel
    {
       protected  Func<T> _CreateFunc;

       protected DBAttr.TBAttribute _TbAttr;

       protected Type _Type;

        public void set(Func<T> CreateFunc)
        {

            T model = CreateFunc();
            _CreateFunc = CreateFunc;
            _Type = model.GetType();
             try
            {
                _TbAttr = (DBAttr.TBAttribute)_Type.GetCustomAttributes(typeof(DBAttr.TBAttribute), true).First();
            }
            catch
            {
                throw new Exception("类缺少TBAttribute");
            }
            
        }

        public static MBaseDAL<T> createDAL(Func<T>  CreateFunc)
        {
            MBaseDAL<T> DAL = new MBaseDAL<T>();
            DAL.set(CreateFunc);
            return DAL;
        }

        public void IInsert(T model)
        {
            string sqlstr = @"Insert into dbo.[{0}]({2}) values ({1})";
            SqlParameter[] pars = model.toParamters();
            string parstr = "";
            string colstr = "";
            foreach (var par in pars)
            {
                if (!model.IdenityKey.Contains(model.GetCol(par)) && par.Value != null)
                {
                    parstr += "," + par.ParameterName;
                    colstr += "," + model.GetCol(par);
                }
            }
            if (parstr.Length > 0)
                parstr = parstr.Remove(0, 1);
            if (colstr.Length > 0)
                colstr = colstr.Remove(0, 1);
            sqlstr = string.Format(sqlstr, _TbAttr.TBName, parstr, colstr);
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
            sqlstr = string.Format(sqlstr, _TbAttr.TBName, parstr, colstr);
            if (SQLDBHelper.ExecuteNonQuery(sqlstr, sqlpar.ToArray()) == 0)
            {
                throw new Exception("插入失败");
            }
        }

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
            sqlstr = string.Format(sqlstr, _TbAttr.TBName, keystr);
            if (SQLDBHelper.ExecuteNonQuery(sqlstr, keys) == 0)
            {
                throw new Exception("插入失败");
            }

        }

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
                sqlstr = string.Format(sqlstr, _TbAttr.TBName, updateStr, keystr);
                if (SQLDBHelper.ExecuteNonQuery(sqlstr, pars) == 0)
                {
                    throw new Exception("更新失败");
                }
                return;
            }
            throw new Exception("更新失败");
        }

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
            sqlstr = string.Format(sqlstr,_TbAttr.TBName, keystr, colstr);
            DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr, keys);
            if (dt.Rows.Count > 0)
                model.CopyByDataRow(dt.Rows[0]);
            else
                throw new Exception("查询错误");
            return model;
        }

        public T[] ISeleteEnitys(Models.SQLFiliter[] filters, string type, int page = -1, int length = -1)
        {

            T model = _CreateFunc();
            string colstr = model.ListCols;
            string sqlstr = string.Format("Select {1} from [{0}]", _TbAttr.TBName, colstr);
            if (filters == null || filters.Length == 0)
            {
                if (page > 0 && length > 0)
                {
                    sqlstr = @"select {4} from dbo.[{0}] where {1} in(select top {3} [{1}] from {0}) 
                                                    and {1} not in(select top {2} [{1}] from {0}) 
                                                    ";

                    sqlstr = string.Format(sqlstr, _TbAttr.TBName, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, colstr);
                }

                DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = _CreateFunc();
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

                    sqlstr = string.Format(sqlstr, _TbAttr.TBName, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, sqlf, colstr);
                }
                else
                {
                    sqlstr += " where " + sqlf;
                }
                DataTable dt = SQLDBHelper.ExecuteDataTable(sqlstr, pars);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = _CreateFunc();
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

        public T[] ISeleteEnitys(Models.FilterConfig[] config, int page = -1, int length = -1)
        {
            if (config == null || config.Length == 0)
            {
                return this.ISeleteEnitys(null, "", page, length);
            }
            else
            {
                T model = _CreateFunc();
                string sqlf = "";
                List<SqlParameter> SQLpars = new List<SqlParameter>();
                foreach (var fig in config)
                {
                    string ty = " and ";
                    Models.SQLFiliter[] filiter = fig.toFiliter(model.GoablStr, ref ty);
                    if (filiter == null)
                        continue;
                    string strf = "";
                    List<SqlParameter> pars = new List<SqlParameter>();
                    foreach (var fi in filiter)
                    {
                        strf += ty + fi.ToString();
                        pars.Add(fi.Par);
                    }
                    if (strf.Length > 0)
                        strf = strf.Remove(0, ty.Length);
                    if(sqlf.Length==0)
                        sqlf = string.Format(" ({0}) ", strf);
                    else
                        sqlf +=" and "+string.Format(" ({0}) ", strf);
                    SQLpars.AddRange(pars);
                }
                string sql = "";
                if (page > 0 && length > 0)
                {
                    sql = @"select {5} from dbo.[{0}] where [{1}] in(select top {3} [{1}] from {0} where {4}) 
                                                    and [{1}] not in(select top {2} [{1}] from {0} where {4}) 
                                                    and {4}";

                    sql = string.Format(sql, _TbAttr.TBName, model.GetCol(model.Keys[0]), (page - 1) * length, (page) * length, sqlf, model.ListCols);
                }
                else
                {
                    sql = @"select {1} from {0} where {2}";
                    sql = string.Format(sql, _TbAttr.TBName, model.ListCols, sqlf);
                }
                DataTable dt = SQLDBHelper.ExecuteDataTable(sql, SQLpars.ToArray());
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = _CreateFunc();
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
