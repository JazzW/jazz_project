using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Jazz.ADOFrame.Interface;
using Jazz.ADOFrame.IAttribute;
using Jazz.Helper.DataBase.SQL;
using Jazz.Helper.DataBase.Common;

namespace Jazz.ADOFrame.IClass
{
    public class MDAL<T> where T : IDBModel
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

        public static MDAL<T> createDAL(Func<T>  CreateFunc)
        {
            MDAL<T> DAL = new MDAL<T>();
            DAL.set(CreateFunc);
            return DAL;
        }

        public void IInsert(T model)
        {
            string sqlstr = @"Insert into dbo.[{0}]({2}) values ({1})";
            SqlParameter[] pars = IModel.EnityManage<T>.toParamters(model);
            string parstr = "";
            string colstr = "";
            foreach (var par in pars)
            {
                if (!IModel.EnityManage<T>.IdenityKey(model).Contains(IModel.EnityManage<T>.GetCol(par)) && par.Value != null)
                {
                    parstr += "," + par.ParameterName;
                    colstr += "," + IModel.EnityManage<T>.GetCol(par);
                }
            }
            if (parstr.Length > 0)
                parstr = parstr.Remove(0, 1);
            if (colstr.Length > 0)
                colstr = colstr.Remove(0, 1);
            sqlstr = string.Format(sqlstr, _TbAttr.TBName, parstr, colstr);
            if (DBHelper.ExecuteNonQuery(sqlstr, pars) == 0)
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

            SqlParameter[] cols = IModel.EnityManage<T>.toParamters(models[0]);
            foreach (var par in cols)
            {
                if (!IModel.EnityManage<T>.IdenityKey(models[0]).Contains(IModel.EnityManage<T>.GetCol(par)))
                {
                    colstr += "," + IModel.EnityManage<T>.GetCol(par);
                }
            }
            if (colstr.Length > 0)
                colstr = colstr.Remove(0, 1);

            int ri = 0;
            foreach (var model in models)
            {
                SqlParameter[] pars = IModel.EnityManage<T>.toParamters(model);
                string str = "";
                pars.AsParallel().ForAll((e) => e.ParameterName = e.ParameterName + ri.ToString());
                foreach (var par in pars)
                {
                    if (!IModel.EnityManage<T>.IdenityKey(model).Contains(IModel.EnityManage<T>.GetCol(par).Replace(ri.ToString(), "")))
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
            if (DBHelper.ExecuteNonQuery(sqlstr, sqlpar.ToArray()) == 0)
            {
                throw new Exception("插入失败");
            }
        }

        public void IDelete(T model)
        {
            string sqlstr = "Delete from dbo.[{0}] where {1}";
            SqlParameter[] keys = IModel.EnityManage<T>.Keys(model);
            string keystr = "";
            foreach (var key in keys)
            {
                if (keystr.Length > 0)
                    keystr += string.Format(" and [{0}]={1} ", IModel.EnityManage<T>.GetCol(key), key.ParameterName);
                else
                    keystr = string.Format("[{0}]={1} ", IModel.EnityManage<T>.GetCol(key), key.ParameterName);
            }
            sqlstr = string.Format(sqlstr, _TbAttr.TBName, keystr);
            if (DBHelper.ExecuteNonQuery(sqlstr, keys) == 0)
            {
                throw new Exception("插入失败");
            }

        }

        public void IUpdate(T model, string[] key)
        {
            string sqlstr = "update dbo.[{0}] set {1} where {2}";
            SqlParameter[] pars = IModel.EnityManage<T>.toParamters(model);

            string keystr = "";
            SqlParameter[] keys = IModel.EnityManage<T>.Keys(model);
            int i = 0;
            foreach (var tkey in keys)
            {
                tkey.Value = key[i];
                if (keystr.Length > 0)
                    keystr += string.Format(" and [{0}]={1} ", IModel.EnityManage<T>.GetCol(tkey), tkey.ParameterName);
                else
                    keystr = string.Format(" [{0}]={1} ", IModel.EnityManage<T>.GetCol(tkey), tkey.ParameterName);
            }
            string updateStr = "";

            foreach (var par in pars)
            {
                if (!IModel.EnityManage<T>.Keys(model).Any(e => e.ParameterName == par.ParameterName))
                    updateStr += string.Format(",{0}={1}", IModel.EnityManage<T>.GetCol(par), par.ParameterName);
            }
            if (updateStr.Length > 0)
            {
                updateStr = updateStr.Remove(0, 1);
                sqlstr = string.Format(sqlstr, _TbAttr.TBName, updateStr, keystr);
                if (DBHelper.ExecuteNonQuery(sqlstr, pars) == 0)
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
            string colstr = model.getItemCols();
            SqlParameter[] keys = IModel.EnityManage<T>.Keys(model);
            int i = 0;
            foreach (var tkey in keys)
            {
                tkey.Value = key[i];
                i++;
                if (keystr.Length > 0)
                    keystr += string.Format(" and [{0}]={1} ", IModel.EnityManage<T>.GetCol(tkey), tkey.ParameterName);
                else
                    keystr = string.Format(" [{0}]={1} ", IModel.EnityManage<T>.GetCol(tkey), tkey.ParameterName);
            }
            sqlstr = string.Format(sqlstr,_TbAttr.TBName, keystr, colstr);
            DataTable dt = DBHelper.ExecuteDataTable(sqlstr, keys);
            if (dt.Rows.Count > 0)
                IModel.EnityManage<T>.CopyByDataRow(dt.Rows[0],model);
            else
                throw new Exception("查询错误");
            return model;
        }

        public T[] ISeleteEnitys(IConfig.SQLFiliter[] filters, string type, int page = -1, int length = -1)
        {

            T model = _CreateFunc();
            string colstr = model.getListCols();
            string sqlstr = string.Format("Select {1} from [{0}]", _TbAttr.TBName, colstr);
            if (filters == null || filters.Length == 0)
            {
                sqlstr = SQLCmdHelper.PageFrame(sqlstr, length, page);
                DataTable dt = DBHelper.ExecuteDataTable(sqlstr);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = _CreateFunc();
                        mo= IModel.EnityManage<T>.CopyByDataRow(dt.Rows[i],model);
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

                sqlstr += " where " + sqlf;
                sqlstr = SQLCmdHelper.PageFrame(sqlstr, length, page);
                DataTable dt = DBHelper.ExecuteDataTable(sqlstr, pars);
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = _CreateFunc();
                        IModel.EnityManage<T>.CopyByDataRow(dt.Rows[i], model);
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

        public T[] ISeleteEnitys(IConfig.FilterConfig[] config, int page = -1, int length = -1)
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
                    IConfig.SQLFiliter[] filiter = fig.toFiliter("", ref ty);
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

                sql = @"select {1} from {0} where {2}";
                sql = string.Format(sql, _TbAttr.TBName, model.getListCols(), sqlf);

                sql = SQLCmdHelper.PageFrame(sql, length, page);

                DataTable dt = DBHelper.ExecuteDataTable(sql, SQLpars.ToArray());
                if (dt.Rows.Count > 0)
                {
                    List<T> list = new List<T>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        T mo = _CreateFunc();
                        IModel.EnityManage<T>.CopyByDataRow(dt.Rows[i],model);
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
