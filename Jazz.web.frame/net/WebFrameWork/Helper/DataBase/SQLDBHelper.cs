using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
//using System.Data.SQLite;

namespace WebFrameWork.Helper
{
    public class SQLDBHelper
    {
        /// <summary>  
        /// 连接字符串  
        /// </summary>  
        //public static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["conn1"].ConnectionString;
        public static  string connectionString = "server=.;uid=sa;pwd=123;database=HTDB;Min Pool Size = 5;Max Pool Size=100;";

        #region ExecuteNonQuery命令
        /// <summary>  
        /// 对数据库执行增、删、改命令  
        /// </summary>  
        /// <param name="safeSql">T-Sql语句</param>  
        /// <returns>受影响的记录数</returns>  
        public static int ExecuteNonQuery(string safeSql)
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

        /// <summary>  
        /// 对数据库执行增、删、改命令  
        /// </summary>  
        /// <param name="sql">T-Sql语句</param>  
        /// <param name="values">参数数组</param>  
        /// <returns>受影响的记录数</returns>  
        public static int ExecuteNonQuery(string sql, SqlParameter[] values)
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
        #endregion

        #region ExecuteScalar命令
        /// <summary>  
        /// 查询结果集中第一行第一列的值  
        /// </summary>  
        /// <param name="safeSql">T-Sql语句</param>  
        /// <returns>第一行第一列的值</returns>  
        public static int ExecuteScalar(string safeSql)
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

        /// <summary>  
        /// 查询结果集中第一行第一列的值  
        /// </summary>  
        /// <param name="sql">T-Sql语句</param>  
        /// <param name="values">参数数组</param>  
        /// <returns>第一行第一列的值</returns>  
        public static int ExecuteScalar(string sql, SqlParameter[] values)
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
        /// <summary>  
        /// 创建数据读取器  
        /// </summary>  
        /// <param name="safeSql">T-Sql语句</param>  
        /// <param name="Connection">数据库连接</param>  
        /// <returns>数据读取器对象</returns>  
        public static SqlDataReader ExecuteReader(string safeSql, SqlConnection Connection)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            SqlCommand cmd = new SqlCommand(safeSql, Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>  
        /// 创建数据读取器  
        /// </summary>  
        /// <param name="sql">T-Sql语句</param>  
        /// <param name="values">参数数组</param>  
        /// <param name="Connection">数据库连接</param>  
        /// <returns>数据读取器</returns>  
        public static SqlDataReader ExecuteReader(string sql, SqlParameter[] values, SqlConnection Connection)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            SqlCommand cmd = new SqlCommand(sql, Connection);
            cmd.Parameters.AddRange(values);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        #endregion

        #region ExecuteDataTable命令
        /// <summary>  
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataTable  
        /// </summary>  
        /// <param name="type">命令类型(T-Sql语句或者存储过程)</param>  
        /// <param name="safeSql">T-Sql语句或者存储过程的名称</param>  
        /// <param name="values">参数数组</param>  
        /// <returns>结果集DataTable</returns>  
        public static DataTable ExecuteDataTable(CommandType type, string safeSql, params SqlParameter[] values)
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

        /// <summary>  
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataTable  
        /// </summary>  
        /// <param name="safeSql">T-Sql语句</param>  
        /// <returns>结果集DataTable</returns>  
        public static DataTable ExecuteDataTable(string safeSql)
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

        /// <summary>  
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataTable  
        /// </summary>  
        /// <param name="sql">T-Sql语句</param>  
        /// <param name="values">参数数组</param>  
        /// <returns>结果集DataTable</returns>  
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] values)
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
        /// <summary>  
        /// 取出数据  
        /// </summary>  
        /// <param name="safeSql">sql语句</param>  
        /// <param name="tabName">DataTable别名</param>  
        /// <param name="values"></param>  
        /// <returns></returns>  
        public static DataSet GetDataSet(string safeSql, string tabName, params SqlParameter[] values)
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
        /// <summary>  
        /// 批量修改数据  
        /// </summary>  
        /// <param name="ds">修改过的DataSet</param>  
        /// <param name="strTblName">表名</param>  
        /// <returns></returns>  
        public static int ExecureData(DataSet ds, string strTblName)
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

        public static int ExecureData(DataSet ds, string SQL, string strTblName)
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

        public static int ExecureData(DataSet ds, string SQL, string strTblName, SqlParameter[] pars)
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

        public static void BulkToDB(DataTable dt, string tbName)
        {

            try
            {
                //创建一个数据库连接  
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

        /// <summary>
        /// 创建自动执行任务
        /// </summary>
        /// <param name="jobName">作业名字</param>
        /// <param name="TSQL">执行TSQL语句</param>
        /// <param name="freqTypre">执行的频率 4：天 8：周 16：月 </param>
        /// <param name="HHMMSS">执行时间hhmmss</param>
        /// <param name="freqRe">执行间隔</param>
        public static void CreateAutoTask(string jobName, string TSQL, int freqTypre, string HHMMSS, int freqRe)
        {
            string sqlstr = string.Format(@"--定义创建作业
                                DECLARE @jobid uniqueidentifier, @jobname sysname
                                SET @jobname = N'{0}'

                                IF EXISTS(SELECT * FROM msdb.dbo.sysjobs WHERE name=@jobname)
                                EXEC msdb.dbo.sp_delete_job @job_name=@jobname

                                EXEC msdb.dbo.sp_add_job
                                @job_name = @jobname,
                                @job_id = @jobid OUTPUT

                                --定义作业步骤
                                DECLARE @sql nvarchar(4000),@dbname sysname
                                SELECT @dbname=DB_NAME()  --作业步骤在当前数据库中执行
                                EXEC msdb.dbo.sp_add_jobstep
                                @job_id = @jobid,
                                @step_name = N'步骤一',
                                @subsystem = 'TSQL', --步骤的类型,一般为TSQL
                                @database_name=@dbname,
                                @command = '{1}'

                                --创建调度(使用后面专门定义的几种作业调度模板)
                                EXEC msdb..sp_add_jobschedule
                                @job_id = @jobid,
                                @name = N'第一个调度',
                                @freq_type={2},                --执行的频率 4：天 8：周 16：月 
                                @freq_interval=1,            --第几天
                                @freq_subday_type=0x1,       --重复方式,0x1=在指定的时间,0x4=多少分钟,0x8=多少小时执行一次。0x1和@active_start_time一起使用，@active_start_time指定开始执行的时间，代表在@freq_type指定的频率间隔内只执行一次
                                                          --若是0x4或0x8，只要指定@freq_subday_interval， @freq_subday_interval代表每多少分钟（当@freq_subday_type=0x4）或小时（当@freq_subday_type=0x8）执行的次数
                                @freq_subday_interval=2,     --重复周期数,这里每小时执行一次
                                @active_start_date = NULL,   --作业执行的开始日期,为NULL时表示当前日期,格式为YYYYMMDD
                                @active_end_date = 99991231, --作业执行的停止日期,默认为99991231,格式为YYYYMMDD
                                @active_start_time = {3},  --作业执行的开始时间,格式为HHMMSS
                                @active_end_time = 030000,    --作业执行的停止时间,格式为HHMMSS
                                @freq_recurrence_factor = {4}   --执行间隔
                                -- 添加目标服务器
                                EXEC msdb.dbo.sp_add_jobserver 
                                @job_id = @jobid,
                                @server_name = N'(local)'", jobName, TSQL, freqTypre, HHMMSS, freqRe);
            try
            {
                ExecuteNonQuery(sqlstr);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }

    //public class SQLiteHepler
    //{
    //    public static string connectionString = "";

    //    public static DataTable getTables()
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            try
    //            {
    //                Connection.Open();
    //                return Connection.GetSchema("TABLES");
    //            }
    //            catch
    //            {
    //                return null;
    //            }
    //        }
    //    }

    //    #region ExecuteNonQuery命令
    //    public static int ExecuteNonQuery(string safeSql)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            Connection.Open();
    //            SQLiteTransaction  trans = Connection.BeginTransaction();
    //            try
    //            {
    //                SQLiteCommand cmd = new SQLiteCommand(safeSql, Connection);
    //                cmd.Transaction = trans;

    //                if (Connection.State != ConnectionState.Open)
    //                {
    //                    Connection.Open();
    //                }
    //                int result = cmd.ExecuteNonQuery();
    //                trans.Commit();
    //                return result;
    //            }
    //            catch
    //            {
    //                trans.Rollback();
    //                return 0;
    //            }
    //        }
    //    }

    //    public static int ExecuteNonQuery(string sql,SQLiteParameter[] values)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            Connection.Open();
    //            SQLiteTransaction trans = Connection.BeginTransaction();
    //            try
    //            {
    //                SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
    //                cmd.Transaction = trans;
    //                cmd.Parameters.AddRange(values);
    //                if (Connection.State != ConnectionState.Open)
    //                {
    //                    Connection.Open();
    //                }
    //                int result = cmd.ExecuteNonQuery();
    //                trans.Commit();
    //                if (result < 0) return 0;
    //                return result;
    //            }
    //            catch (Exception ex)
    //            {
    //                trans.Rollback();
    //                return 0;
    //            }
    //        }
    //    }

    //    #endregion

    //    #region ExecuteScalar命令
    //    /// <summary>  
    //    /// 查询结果集中第一行第一列的值  
    //    /// </summary>  
    //    /// <param name="safeSql">T-Sql语句</param>  
    //    /// <returns>第一行第一列的值</returns>  
    //    public static int ExecuteScalar(string safeSql)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            if (Connection.State != ConnectionState.Open)
    //                Connection.Open();
    //            SQLiteCommand cmd = new SQLiteCommand(safeSql, Connection);
    //            int result = Convert.ToInt32(cmd.ExecuteScalar());
    //            return result;
    //        }
    //    }

    //    /// <summary>  
    //    /// 查询结果集中第一行第一列的值  
    //    /// </summary>  
    //    /// <param name="sql">T-Sql语句</param>  
    //    /// <param name="values">参数数组</param>  
    //    /// <returns>第一行第一列的值</returns>  
    //    public static int ExecuteScalar(string sql, SQLiteParameter[] values)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            if (Connection.State != ConnectionState.Open)
    //                Connection.Open();
    //            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
    //            cmd.Parameters.AddRange(values);
    //            int result = Convert.ToInt32(cmd.ExecuteScalar());
    //            return result;
    //        }
    //    }
    //    #endregion

    //    #region ExecuteReader命令
    //    /// <summary>  
    //    /// 创建数据读取器  
    //    /// </summary>  
    //    /// <param name="safeSql">T-Sql语句</param>  
    //    /// <param name="Connection">数据库连接</param>  
    //    /// <returns>数据读取器对象</returns>  
    //    public static SQLiteDataReader ExecuteReader(string safeSql, SQLiteConnection Connection)
    //    {
    //        if (Connection.State != ConnectionState.Open)
    //            Connection.Open();
    //        SQLiteCommand cmd = new SQLiteCommand(safeSql, Connection);
    //        SQLiteDataReader reader = cmd.ExecuteReader();
    //        return reader;
    //    }

    //    /// <summary>  
    //    /// 创建数据读取器  
    //    /// </summary>  
    //    /// <param name="sql">T-Sql语句</param>  
    //    /// <param name="values">参数数组</param>  
    //    /// <param name="Connection">数据库连接</param>  
    //    /// <returns>数据读取器</returns>  
    //    public static SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] values, SQLiteConnection Connection)
    //    {
    //        if (Connection.State != ConnectionState.Open)
    //            Connection.Open();
    //        SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
    //        cmd.Parameters.AddRange(values);
    //        SQLiteDataReader reader = cmd.ExecuteReader();
    //        return reader;
    //    }
    //    #endregion

    //    #region ExecuteDataTable命令
    //    /// <summary>  
    //    /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataTable  
    //    /// </summary>  
    //    /// <param name="type">命令类型(T-Sql语句或者存储过程)</param>  
    //    /// <param name="safeSql">T-Sql语句或者存储过程的名称</param>  
    //    /// <param name="values">参数数组</param>  
    //    /// <returns>结果集DataTable</returns>  
    //    public static DataTable ExecuteDataTable(CommandType type, string safeSql, params SQLiteParameter[] values)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            if (Connection.State != ConnectionState.Open)
    //                Connection.Open();
    //            DataSet ds = new DataSet();
    //            SQLiteCommand cmd = new SQLiteCommand(safeSql, Connection);
    //            cmd.CommandType = type;
    //            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
    //            da.Fill(ds);
    //            return ds.Tables[0];
    //        }
    //    }

    //    /// <summary>  
    //    /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataTable  
    //    /// </summary>  
    //    /// <param name="safeSql">T-Sql语句</param>  
    //    /// <returns>结果集DataTable</returns>  
    //    public static DataTable ExecuteDataTable(string safeSql)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            if (Connection.State != ConnectionState.Open)
    //                Connection.Open();
    //            DataSet ds = new DataSet();
    //            SQLiteCommand cmd = new SQLiteCommand(safeSql, Connection);
    //            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
    //            try
    //            {
    //                da.Fill(ds);
    //            }
    //            catch (Exception ex)
    //            {

    //            }
    //            return ds.Tables[0];
    //        }
    //    }

    //    /// <summary>  
    //    /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataTable  
    //    /// </summary>  
    //    /// <param name="sql">T-Sql语句</param>  
    //    /// <param name="values">参数数组</param>  
    //    /// <returns>结果集DataTable</returns>  
    //    public static DataTable ExecuteDataTable(string sql, params SQLiteParameter[] values)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            if (Connection.State != ConnectionState.Open)
    //                Connection.Open();
    //            DataSet ds = new DataSet();
    //            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
    //            cmd.CommandTimeout = 0;
    //            cmd.Parameters.AddRange(values);
    //            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
    //            da.Fill(ds);
    //            return ds.Tables[0];
    //        }
    //    }
    //    #endregion

    //    #region GetDataSet命令
    //    /// <summary>  
    //    /// 取出数据  
    //    /// </summary>  
    //    /// <param name="safeSql">sql语句</param>  
    //    /// <param name="tabName">DataTable别名</param>  
    //    /// <param name="values"></param>  
    //    /// <returns></returns>  
    //    public static DataSet GetDataSet(string safeSql, string tabName, params SQLiteParameter[] values)
    //    {
    //        using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //        {
    //            if (Connection.State != ConnectionState.Open)
    //                Connection.Open();
    //            DataSet ds = new DataSet();
    //            SQLiteCommand cmd = new SQLiteCommand(safeSql, Connection);

    //            if (values != null)
    //                cmd.Parameters.AddRange(values);

    //            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
    //            try
    //            {
    //                da.Fill(ds, tabName);
    //            }
    //            catch (Exception ex)
    //            {

    //            }
    //            return ds;
    //        }
    //    }
    //    #endregion

    //    #region ExecureData命令
    //    /// <summary>  
    //    /// 批量修改数据  
    //    /// </summary>  
    //    /// <param name="ds">修改过的DataSet</param>  
    //    /// <param name="strTblName">表名</param>  
    //    /// <returns></returns>  
    //    public static int ExecureData(DataSet ds, string strTblName)
    //    {
    //        try
    //        {
    //            //创建一个数据库连接  
    //            using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //            {
    //                if (Connection.State != ConnectionState.Open)
    //                    Connection.Open();

    //                //创建一个用于填充DataSet的对象  
    //                SQLiteCommand myCommand = new SQLiteCommand("SELECT * FROM " + strTblName, Connection);
    //                SQLiteDataAdapter myAdapter = new SQLiteDataAdapter();
    //                //获取SQL语句，用于在数据库中选择记录  
    //                myAdapter.SelectCommand = myCommand;

    //                //自动生成单表命令，用于将对DataSet所做的更改与数据库更改相对应  
    //                SQLiteCommandBuilder myCommandBuilder = new SQLiteCommandBuilder(myAdapter);

    //                return myAdapter.Update(ds, strTblName);  //更新ds数据  
    //            }

    //        }
    //        catch (Exception err)
    //        {
    //            throw err;
    //        }
    //    }

    //    public static int ExecureData(DataSet ds, string SQL, string strTblName)
    //    {
    //        try
    //        {
    //            //创建一个数据库连接  
    //            using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //            {
    //                if (Connection.State != ConnectionState.Open)
    //                    Connection.Open();

    //                //创建一个用于填充DataSet的对象  
    //                SQLiteCommand myCommand = new SQLiteCommand(SQL, Connection);
    //                SQLiteDataAdapter myAdapter = new SQLiteDataAdapter();
    //                //获取SQL语句，用于在数据库中选择记录  
    //                myAdapter.SelectCommand = myCommand;

    //                //自动生成单表命令，用于将对DataSet所做的更改与数据库更改相对应  
    //                SQLiteCommandBuilder myCommandBuilder = new SQLiteCommandBuilder(myAdapter);

    //                return myAdapter.Update(ds, strTblName);  //更新ds数据  
    //            }

    //        }
    //        catch (Exception err)
    //        {
    //            throw err;
    //        }
    //    }

    //    public static int ExecureData(DataSet ds, string SQL, string strTblName, SQLiteParameter[] pars)
    //    {
    //        try
    //        {
    //            //创建一个数据库连接  
    //            using (SQLiteConnection Connection = new SQLiteConnection(connectionString))
    //            {
    //                if (Connection.State != ConnectionState.Open)
    //                    Connection.Open();

    //                //创建一个用于填充DataSet的对象  
    //                SQLiteCommand myCommand = new SQLiteCommand(SQL, Connection);
    //                myCommand.Parameters.AddRange(pars);
    //                SQLiteDataAdapter myAdapter = new SQLiteDataAdapter();
    //                //获取SQL语句，用于在数据库中选择记录  
    //                myAdapter.SelectCommand = myCommand;

    //                //自动生成单表命令，用于将对DataSet所做的更改与数据库更改相对应  
    //                SQLiteCommandBuilder myCommandBuilder = new SQLiteCommandBuilder(myAdapter);

    //                return myAdapter.Update(ds, strTblName);  //更新ds数据  
    //            }

    //        }
    //        catch (Exception err)
    //        {
    //            throw err;
    //        }
    //    }

    //    #endregion
    //}
}
