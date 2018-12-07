using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Common;

namespace Jazz.Helper.DataBase.Common
{

    public class SQLCmdHelper
    {
        public static object GetKeyField<T>()
        {
            return null;
        }

        public static PropertyInfo[] GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        #region 格式化SQL语句
        /// <summary>
        /// 格式化SQL语句
        /// </summary>
        /// <param name="strsql">SQL语句</param>
        /// <returns></returns>
        public static string FormatSQL(string strsql)
        {
            strsql = strsql.ToLower();
            switch (DBConfig.DbType)
            {
                case DBConfig.DatabaseType.SqlServer:
                    return strsql;
                case DBConfig.DatabaseType.Oracle:
                    return strsql;
                case DBConfig.DatabaseType.MySql:
                    strsql = strsql.Replace(",condition,", ",`condition`,");
                    strsql = strsql.Replace("getdate()", "now()");
                    strsql = strsql.Replace("isnull(", "ifnull(");
                    return strsql;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }
        #endregion

        #region 拼接 Insert SQL语句
        /// <summary>
        /// 哈希表生成Insert语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public static StringBuilder InsertSql(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null)
                {
                    sb_prame.Append("," + key);
                    sp.Append("," + DBConfig.DbParmChar + "" + key);
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }

        /// <summary>
        /// 泛型方法，反射生成InsertSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>int</returns>
        public static StringBuilder InsertSql<T>(T entity)
        {
            Type type = entity.GetType();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(type.Name);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    sb_prame.Append("," + (prop.Name));
                    sp.Append("," + DBConfig.DbParmChar + "" + (prop.Name));
                }
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            return sb;
        }
        #endregion

        #region 拼接 Update SQL语句
        /// <summary>
        /// 哈希表生成UpdateSql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="pkName">主键</param>
        /// <returns></returns>
        public static StringBuilder UpdateSql(string tableName, Hashtable ht, string pkName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(tableName);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (string key in ht.Keys)
            {
                if (ht[key] != null && pkName != key)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(key);
                        sb.Append("=");
                        sb.Append(DBConfig.DbParmChar + key);
                    }
                    else
                    {
                        sb.Append("," + key);
                        sb.Append("=");
                        sb.Append(DBConfig.DbParmChar + key);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(DBConfig.DbParmChar + pkName);
            return sb;
        }
        /// <summary>
        /// 泛型方法，反射生成UpdateSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="pkName">主键</param>
        /// <returns>int</returns>
        public static StringBuilder UpdateSql<T>(T entity, string pkName)
        {
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(type.Name);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null && GetKeyField<T>().ToString() != prop.Name)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(prop.Name);
                        sb.Append("=");
                        sb.Append(DBConfig.DbParmChar + prop.Name);
                    }
                    else
                    {
                        sb.Append("," + prop.Name);
                        sb.Append("=");
                        sb.Append(DBConfig.DbParmChar + prop.Name);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(DBConfig.DbParmChar + pkName);
            return sb;
        }
        /// <summary>
        /// 泛型方法，反射生成UpdateSql语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>int</returns>
        public static StringBuilder UpdateSql<T>(T entity)
        {
            string pkName = GetKeyField<T>().ToString();
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(type.Name);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null && pkName != prop.Name)
                {
                    if (isFirstValue)
                    {
                        isFirstValue = false;
                        sb.Append(prop.Name);
                        sb.Append("=");
                        sb.Append(DBConfig.DbParmChar + prop.Name);
                    }
                    else
                    {
                        sb.Append("," + prop.Name);
                        sb.Append("=");
                        sb.Append(DBConfig.DbParmChar + prop.Name);
                    }
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append(DBConfig.DbParmChar + pkName);
            return sb;
        }
        #endregion

        #region 拼接 Delete SQL语句
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql(string tableName, string pkName)
        {
            return new StringBuilder("Delete From " + tableName + " Where " + pkName + " = " + DBConfig.DbParmChar + pkName + "");
        }
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">多参数</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder("Delete From " + tableName + " Where 1=1");
            foreach (string key in ht.Keys)
            {
                sb.Append(" AND " + key + " = " + DBConfig.DbParmChar + "" + key + "");
            }
            return sb;
        }
        /// <summary>
        /// 拼接删除SQL语句
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static StringBuilder DeleteSql<T>(T entity)
        {
            Type type = entity.GetType();
            PropertyInfo[] props = type.GetProperties();
            StringBuilder sb = new StringBuilder("Delete From " + type.Name + " Where 1=1");
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    sb.Append(" AND " + prop.Name + " = " + DBConfig.DbParmChar + "" + prop.Name + "");
                }
            }
            return sb;
        }
        #endregion

        #region 拼接 Select SQL语句
        /// <summary>
        /// 拼接 查询 SQL语句
        /// </summary>
        /// <returns></returns>
        public static StringBuilder SelectSql<T>() where T : new()
        {
            string tableName = typeof(T).Name;
            PropertyInfo[] props = GetProperties(new T().GetType());
            StringBuilder sbColumns = new StringBuilder();
            foreach (PropertyInfo prop in props)
            {
                string propertytype = prop.PropertyType.ToString();
                sbColumns.Append(prop.Name + ",");
            }
            if (sbColumns.Length > 0) sbColumns.Remove(sbColumns.ToString().Length - 1, 1);
            string strSql = "SELECT {0} FROM {1} WHERE 1=1 ";
            strSql = string.Format(strSql, sbColumns.ToString(), tableName + " ");
            return new StringBuilder(strSql);
        }
        /// <summary>
        /// 拼接 查询 SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static StringBuilder SelectSql(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM " + tableName + " WHERE 1=1 ");
            return strSql;
        }
        /// <summary>
        /// 拼接 查询条数 SQL语句
        /// </summary>
        /// <returns></returns>
        public static StringBuilder SelectCountSql<T>() where T : new()
        {
            string tableName = typeof(T).Name;//获取表名
            return new StringBuilder("SELECT Count(1) FROM " + tableName + " WHERE 1=1 ");
        }
        /// <summary>
        /// 拼接 查询最大数 SQL语句
        /// </summary>
        /// <param name="propertyName">属性字段</param>
        /// <returns></returns>
        public static StringBuilder SelectMaxSql<T>(string propertyName) where T : new()
        {
            string tableName = typeof(T).Name;//获取表名
            return new StringBuilder("SELECT MAX(" + propertyName + ") FROM " + tableName + "  WHERE 1=1 ");
        }
        #endregion

        #region 查询框架


        public static string RunProcFrame(string ProcName, Dictionary<string, DbParameter> dict)
        {
            string sqlFrame = "exec {0} {1}";

            string setsql = "";

            foreach (var key in dict.Keys)
            {
               setsql = string.Format(",{0}={1} ", key, dict[key].ParameterName);
            }

            return string.Format(sqlFrame,ProcName,setsql);
        }

        public static string InsertFrame(string TB, Dictionary<string, DbParameter> dict)
        {
            string sqlFrame = "insert into [{0}] ({1}) values({2})";
            string colsql = "";
            string valsql = "";

            foreach (var key in dict.Keys)
            {
                colsql += "," + key;
                valsql += "," + dict[key].ParameterName;
            }

            if (colsql.Length > 0)
                colsql = colsql.Remove(0, 1);
            if (valsql.Length > 0)
                valsql = colsql.Remove(0, 1);

            return string.Format(sqlFrame, TB, colsql, valsql) ;
        }

        public static string InsertListFrame(string TB, Dictionary<string, DbParameter>[] dicts)
        {
            string sqlFrame = "insert into [{0}] ({1}) values {2}";



            return sqlFrame;
        }

        public static string UpdateFrame(string TB, Dictionary<string, DbParameter> dict, string WhereSql)
        {
            string sqlFrame = "Update [{0}] set {1} where {2}";

            string setsql = "";

            foreach (var key in dict.Keys)
            {
                if(setsql.Length==0)
                    setsql = string.Format(" {0}={1} ", key, dict[key].ParameterName);
                else
                    setsql = string.Format("and {0}={1} ", key, dict[key].ParameterName);
            }

            return string.Format(sqlFrame,TB,setsql,WhereSql);
        }

        public static string DeleteFrame(string TB, string WhereSql)
        {
            string sqlFrame = "delete [{0}] where {1}";

            return  string.Format(sqlFrame,TB,WhereSql);
        }

        public static string SelectFrame(string ColsSql, string SrcSql,string WhereSql,string OrderSql)
        {
            string sqlFrame = "select {0} from {1} where {2} {3}";

            return string.Format(sqlFrame,ColsSql,SrcSql,WhereSql,OrderSql);
        }

        public static string SelectFrame(string[] Cols,string insql)
        {
            string selectsql="";
            foreach(string col in Cols)
            {
               selectsql+=string.Format(",[{0}]",col);
            }
            if(selectsql.Length>0)
                selectsql=selectsql.Remove(0,1);
            string sqlFrame = "select {0} from ({1}) fsrc ";

            return string.Format(sqlFrame,selectsql,insql);
        }

        public static string SelectToatlFrame(string SrcSql, string WhereSql)
        {
            string sqlFrame = "select Count(1) from {0} where {1}";

            return string.Format(sqlFrame, SrcSql, WhereSql);
        }


        public static string PageFrame(string inSQL, int length, int page)
        {
            if (length > -1 && page > -1)
            {
                string sframe = @"select * from(
                                    SELECT  ROW_NUMBER() over(order by (select 0)) as [row],fr1.* from
                                    ({0}) fr1
                                        ) fr2 where fr2.row>{1}  and fr2.row<={2}";

                string outSQL = string.Format(sframe, inSQL, (page - 1) * length, (page) * length);

                return outSQL;
            }
            return inSQL;
        }

        public static string CountFrame(string inSQL)
        {
            string sframe = @"select count(1) from ({0}) fr";
            string outSQL = string.Format(sframe, inSQL);
            return outSQL;
        }

        public static string FiliterFrame()
        {
            return null;
        }


        #endregion
    }
}
