using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Jazz.Helper.DataBase.Common
{
    public  class DBHelper
    {
        public static IDataBase database { get; set; }

        #region ExecuteNonQuery命令

        public static int ExecuteNonQuery(string safeSql)
        {
            return database.ExecuteNonQuery(safeSql, null);
        }

        public static int ExecuteNonQuery(string sql, params DbParameter[] values)
        {
            return database.ExecuteNonQuery(sql, null, values);
        }

        #endregion

        #region ExecuteScalar命令
        public static int ExecuteScalar(string safeSql)
        {
            return database.ExecuteScalar(safeSql);
        }

        public static int ExecuteScalar(string sql, params DbParameter[] values)
        {
            return database.ExecuteScalar(sql, values);
        }

        #endregion

        #region ExecuteReader命令

        public static DbDataReader ExecuteReader(string safeSql, DbConnection Connection)
        {
            return database.ExecuteReader(safeSql, Connection);
        }

        public static DbDataReader ExecuteReader(string sql, DbConnection Connection, params DbParameter[] values)
        {
            return database.ExecuteReader(sql, Connection, values);
        }

        #endregion

        #region ExecuteDataTable命令

        public static DataTable ExecuteDataTable(CommandType type, string safeSql, params DbParameter[] values)
        {
            return database.ExecuteDataTable(type, safeSql, values);
        }

        public static DataTable ExecuteDataTable(string safeSql)
        {
            return database.ExecuteDataTable(safeSql);
        }

        public static DataTable ExecuteDataTable(string sql, params DbParameter[] values)
        {
            return database.ExecuteDataTable(sql, values);
        }

        #endregion

        #region GetDataSet命令

        public static DataSet GetDataSet(string safeSql, string tabName, params DbParameter[] values)
        {
            return database.GetDataSet(safeSql, tabName, values);
        }

        #endregion

        #region ExecureData命令

        public static int ExecureData(DataSet ds, string strTblName)
        {
            return database.ExecureData(ds, strTblName);
        }

        public static int ExecureData(DataSet ds, string SQL, string strTblName)
        {
            return database.ExecureData(ds, SQL, strTblName);
        }

        public static int ExecureData(DataSet ds, string SQL, string strTblName, params DbParameter[] pars)
        {
            return database.ExecureData(ds, SQL, strTblName, pars);
        }

        public static void BulkToDB(DataTable dt, string tbName)
        {
            database.BulkToDB(dt, tbName);
        }


        #endregion
    }
}
