using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Common;
//using System.Data.SQLite;

namespace Jazz.Helper.DataBase.SQL
{
    public class MSSQL: Common.IDataBase
    {
        public  string connectionString { get { return Common.DBConfig.ConnectionString; } }

        public  DbConnection getConnection()
        {
            DbConnection dbconnection = new SqlConnection(connectionString);
            return dbconnection;
        }

        #region ExecuteNonQuery命令
        public  int ExecuteNonQuery(string safeSql,DbTransaction tran)
        {
            if (tran == null)
            {
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    Connection.Open();
                    SqlTransaction trans = Connection.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand(safeSql, Connection);
                        cmd.Transaction = trans;

                        if (Connection.State != ConnectionState.Open)
                        {
                            Connection.Open();
                        }
                        int result = cmd.ExecuteNonQuery();
                        trans.Commit();
                        return result;
                    }
                    catch
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(safeSql, tran.Connection as SqlConnection);
                    int result = cmd.ExecuteNonQuery();

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public  int ExecuteNonQuery(string sql, DbTransaction tran, params DbParameter[] values)
        {
            if (tran == null)
            {
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    Connection.Open();
                    SqlTransaction trans = Connection.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand(sql, Connection);
                        cmd.Transaction = trans;
                        cmd.Parameters.AddRange(values);
                        if (Connection.State != ConnectionState.Open)
                        {
                            Connection.Open();
                        }
                        int result = cmd.ExecuteNonQuery();
                        trans.Commit();
                        if (result < 0) return 0;
                        return result;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, tran.Connection as SqlConnection);
                    cmd.Parameters.AddRange(values);
                    int result = cmd.ExecuteNonQuery();
                    if (result < 0) return 0;
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
    
            }
        }

        #endregion

        #region ExecuteScalar命令

        public  int ExecuteScalar(string safeSql)
        {
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                SqlCommand cmd = new SqlCommand(safeSql, Connection);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
        }

        public  int ExecuteScalar(string sql, params DbParameter[] values)
        {
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                SqlCommand cmd = new SqlCommand(sql, Connection);
                cmd.Parameters.AddRange(values);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
        }

        #endregion

        #region ExecuteReader命令

        public  DbDataReader ExecuteReader(string safeSql, DbConnection Connection)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            SqlCommand cmd = new SqlCommand(safeSql, Connection as SqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public  DbDataReader ExecuteReader(string sql, DbConnection Connection, params DbParameter[] values)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            SqlCommand cmd = new SqlCommand(sql, Connection as SqlConnection);
            cmd.Parameters.AddRange(values);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        #endregion

        #region ExecuteDataTable命令

        public  DataTable ExecuteDataTable(CommandType type, string safeSql, params DbParameter[] values)
        {
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(safeSql, Connection);
                cmd.CommandType = type;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds.Tables[0];
            }
        }

        public  DataTable ExecuteDataTable(string safeSql)
        {
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(safeSql, Connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {

                }
                return ds.Tables[0];
            }
        }

        public  DataTable ExecuteDataTable(string sql, params DbParameter[] values)
        {
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(sql, Connection);
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddRange(values);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds.Tables[0];
            }
        }

        #endregion

        #region GetDataSet命令

        public  DataSet GetDataSet(string safeSql, string tabName, params DbParameter[] values)
        {
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(safeSql, Connection);

                if (values != null)
                    cmd.Parameters.AddRange(values);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    da.Fill(ds, tabName);
                }
                catch (Exception ex)
                {

                }
                return ds;
            }
        }

        #endregion

        #region ExecureData命令

        public  int ExecureData(DataSet ds, string strTblName)
        {
            try
            {
                //创建一个数据库连接  
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();

                    //创建一个用于填充DataSet的对象  
                    SqlCommand myCommand = new SqlCommand("SELECT * FROM " + strTblName, Connection);
                    SqlDataAdapter myAdapter = new SqlDataAdapter();
                    //获取SQL语句，用于在数据库中选择记录  
                    myAdapter.SelectCommand = myCommand;

                    //自动生成单表命令，用于将对DataSet所做的更改与数据库更改相对应  
                    SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(myAdapter);

                    return myAdapter.Update(ds, strTblName);  //更新ds数据  
                }

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public  int ExecureData(DataSet ds, string SQL, string strTblName)
        {
            try
            {
                //创建一个数据库连接  
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();

                    //创建一个用于填充DataSet的对象  
                    SqlCommand myCommand = new SqlCommand(SQL, Connection);
                    SqlDataAdapter myAdapter = new SqlDataAdapter();
                    //获取SQL语句，用于在数据库中选择记录  
                    myAdapter.SelectCommand = myCommand;

                    //自动生成单表命令，用于将对DataSet所做的更改与数据库更改相对应  
                    SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(myAdapter);

                    return myAdapter.Update(ds, strTblName);  //更新ds数据  
                }

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public  int ExecureData(DataSet ds, string SQL, string strTblName, params DbParameter[] pars)
        {
            try
            {
                //创建一个数据库连接  
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    if (Connection.State != ConnectionState.Open)
                        Connection.Open();

                    //创建一个用于填充DataSet的对象  
                    SqlCommand myCommand = new SqlCommand(SQL, Connection);
                    myCommand.Parameters.AddRange(pars);
                    SqlDataAdapter myAdapter = new SqlDataAdapter();
                    //获取SQL语句，用于在数据库中选择记录  
                    myAdapter.SelectCommand = myCommand;

                    //自动生成单表命令，用于将对DataSet所做的更改与数据库更改相对应  
                    SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(myAdapter);

                    return myAdapter.Update(ds, strTblName);  //更新ds数据  
                }

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public  void BulkToDB(DataTable dt, string tbName)
        {

            try
            {
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(Connection);
                    bulkCopy.DestinationTableName = tbName;
                    bulkCopy.BatchSize = dt.Rows.Count;
                    if (dt != null && dt.Rows.Count != 0)
                        bulkCopy.WriteToServer(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }

}
