using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace Jazz.Helper.DataBase.Common
{
    public interface IDataBase
    {
        DbConnection getConnection();

        #region ExecuteNonQuery命令

        int ExecuteNonQuery(string safeSql, DbTransaction tran);

        int ExecuteNonQuery(string sql, DbTransaction tran, params DbParameter[] values);

        #endregion

        #region ExecuteScalar命令
        int ExecuteScalar(string safeSql);

        int ExecuteScalar(string sql,params DbParameter[] values);

        #endregion

        #region ExecuteReader命令

        DbDataReader ExecuteReader(string safeSql, DbConnection Connection);

        DbDataReader ExecuteReader(string sql, DbConnection Connection, params DbParameter[] values);

        #endregion

        #region ExecuteDataTable命令

          DataTable ExecuteDataTable(CommandType type, string safeSql, params DbParameter[] values);

          DataTable ExecuteDataTable(string safeSql);

          DataTable ExecuteDataTable(string sql, params DbParameter[] values);

        #endregion

        #region GetDataSet命令

          DataSet GetDataSet(string safeSql, string tabName, params DbParameter[] values);

        #endregion

        #region ExecureData命令

          int ExecureData(DataSet ds, string strTblName);

          int ExecureData(DataSet ds, string SQL, string strTblName);

          int ExecureData(DataSet ds, string SQL, string strTblName, DbParameter[] pars);

          void BulkToDB(DataTable dt, string tbName);

        #endregion

    }


}
