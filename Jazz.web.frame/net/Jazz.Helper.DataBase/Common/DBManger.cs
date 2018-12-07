using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace Jazz.Helper.DataBase.Common
{
    public class DBManger:IDisposable
    {
        IDataBase database;

        DbTransaction dbTransaction;

        public DBManger(IDataBase database)
        {
            this.database = database;
        }

        public void Dispose()
        {
            if (dbTransaction != null)
                dbTransaction.Dispose();
        }

        #region 事物提交
 
        public DBManger BeginTrans()
        {
            DbConnection dbConnection = database.getConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            dbTransaction = dbConnection.BeginTransaction();
            return this;
        }

        public int Commit()
        {
            try
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                    this.Close();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (dbTransaction == null)
                {
                    this.Close();
                }
            }
        }

        public void Rollback()
        {
            this.dbTransaction.Rollback();
            this.dbTransaction.Dispose();
            this.Close();
        }

        public void Close()
        {
            DbConnection dbConnection = dbTransaction.Connection;
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }

        }

        public ResultT RunByTran<ResultT>(Func<ResultT> act)
        {
            this.BeginTrans();
            try
            {
                ResultT res= act();
                this.Commit();
                return res;
            }
            catch
            {
                this.Rollback();
                return default(ResultT);
            }
            finally
            {
                this.Close();
            }
        }

        #endregion

        #region ExecuteNonQuery命令

        public int ExecuteNonQuery(string safeSql)
        {
           return database.ExecuteNonQuery(safeSql, this.dbTransaction);
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] values)
        {
            return database.ExecuteNonQuery(sql, this.dbTransaction,values);
        }

        #endregion

        #region ExecuteScalar命令
        public int ExecuteScalar(string safeSql)
        {
            return database.ExecuteScalar(safeSql);
        }

        public int ExecuteScalar(string sql, params DbParameter[] values)
        {
            return database.ExecuteScalar(sql, values);
        }

        #endregion

        #region ExecuteReader命令

        public DbDataReader ExecuteReader(string safeSql, DbConnection Connection)
        {
            return database.ExecuteReader(safeSql, Connection);
        }

        public DbDataReader ExecuteReader(string sql, DbConnection Connection, params DbParameter[] values)
        {
            return database.ExecuteReader(sql, Connection, values);
        }

        #endregion

        #region ExecuteDataTable命令

        public DataTable ExecuteDataTable(CommandType type, string safeSql, params DbParameter[] values)
        {
            return database.ExecuteDataTable(type, safeSql, values);
        }

        public DataTable ExecuteDataTable(string safeSql)
        {
            return database.ExecuteDataTable(safeSql);
        }

        public DataTable ExecuteDataTable(string sql, params DbParameter[] values)
        {
            return database.ExecuteDataTable(sql, values);
        }

        #endregion

        #region GetDataSet命令

        public DataSet GetDataSet(string safeSql, string tabName, params DbParameter[] values)
        {
            return database.GetDataSet(safeSql, tabName, values);
        }

        #endregion

        #region ExecureData命令

        public int ExecureData(DataSet ds, string strTblName)
        {
            return database.ExecureData(ds, strTblName);
        }

        public int ExecureData(DataSet ds, string SQL, string strTblName)
        {
            return database.ExecureData(ds,SQL, strTblName);
        }

        public int ExecureData(DataSet ds, string SQL, string strTblName,params DbParameter[] pars)
        {
            return database.ExecureData(ds, SQL, strTblName,pars);
        }

        public void BulkToDB(DataTable dt, string tbName)
        {
            database.BulkToDB(dt, tbName);
        }


        #endregion
    }
}
