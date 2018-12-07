using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using Jazz.EFframe.IMap;

namespace Jazz.EFframe.IModel
{
    /// <summary>
    /// 通用实体类操做库
    /// </summary>
    public class IDBContextManage : IDisposable
    {
        public DbContext MyDBContext { get; set; }

        public DbTransaction dbTransaction { get; set; }

        public IDBContextManage(string ConnectString)
        {
            MyDBContext = new MyDbContext(ConnectString);
        }

        public IDBContextManage()
        {

        }

        public void Dispose()
        {
            this.Close();
        }

        #region 事物提交
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public IDBContextManage BeginTrans()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)MyDBContext).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            dbTransaction = dbConnection.BeginTransaction();
            return this;
        }
        /// <summary>
        /// 提交当前操作的结果
        /// </summary>
        public int Commit()
        {
            try
            {
                int returnValue = MyDBContext.SaveChanges();
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                    this.Close();
                }
                return returnValue;
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
        /// <summary>
        /// 把当前操作回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            this.dbTransaction.Rollback();
            this.dbTransaction.Dispose();
            this.Close();
        }
        /// <summary>
        /// 关闭连接 内存回收
        /// </summary>
        public void Close()
        {
            MyDBContext.Dispose();
        }
        #endregion

        #region 执行 SQL 语句
        public int ExecuteBySql(string strSql)
        {
            if (dbTransaction == null)
            {
                return MyDBContext.Database.ExecuteSqlCommand(strSql);
            }
            else
            {
                MyDBContext.Database.ExecuteSqlCommand(strSql);
                return dbTransaction == null ? this.Commit() : 0;
            }
        }
        public int ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            if (dbTransaction == null)
            {
                return MyDBContext.Database.ExecuteSqlCommand(strSql, dbParameter);
            }
            else
            {
                MyDBContext.Database.ExecuteSqlCommand(strSql, dbParameter);
                return dbTransaction == null ? this.Commit() : 0;
            }
        }

        #endregion

        #region 对象实体 添加、修改、删除
        public int Insert<T>(T entity) where T : class
        {
            MyDBContext.Entry<T>(entity).State = EntityState.Added;
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Insert<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                MyDBContext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Delete<T>(T entity) where T : class
        {
            MyDBContext.Set<T>().Attach(entity);
            MyDBContext.Set<T>().Remove(entity);
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                MyDBContext.Set<T>().Attach(entity);
                MyDBContext.Set<T>().Remove(entity);
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Delete<T>(Expression<Func<T, bool>> condition) where T : class,new()
        {
            IEnumerable<T> entities = MyDBContext.Set<T>().Where(condition).ToList();
            return entities.Count() > 0 ? Delete(entities) : 0;
        }
        public int UpdateEx<T>(T entity) where T : class
        {
            MyDBContext.Set<T>().Attach(entity);
            MyDBContext.Entry(entity).State = EntityState.Modified;
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                MyDBContext.Set<T>().Attach(entity);
                MyDBContext.Entry<T>(entity).State = EntityState.Modified;
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        public int Update<T>(Expression<Func<T, bool>> condition) where T : class,new()
        {
            return 0;
        }
        #endregion

        #region 对象实体 查询
        public T FindEntity<T>(object keyValue) where T : class
        {
            return MyDBContext.Set<T>().Find(keyValue);
        }
        public T FindEntity<T>(Expression<Func<T, bool>> condition) where T : class,new()
        {
            return MyDBContext.Set<T>().Where(condition).FirstOrDefault();
        }
        public IQueryable<T> IQueryable<T>() where T : class,new()
        {
            return MyDBContext.Set<T>();
        }
        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class,new()
        {
            return MyDBContext.Set<T>().Where(condition);
        }
        public IEnumerable<T> FindList<T>() where T : class,new()
        {
            return MyDBContext.Set<T>().ToList();
        }
        public IEnumerable<T> FindList<T>(Func<T, object> keySelector) where T : class,new()
        {
            return MyDBContext.Set<T>().OrderBy(keySelector).ToList();
        }
        public IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition) where T : class,new()
        {
            return MyDBContext.Set<T>().Where(condition).ToList();
        }
        public IEnumerable<T> FindList<T>(string orderField, bool isAsc, int pageSize, int pageIndex, out int total) where T : class,new()
        {
            string[] _order = orderField.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = MyDBContext.Set<T>().AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<T>(resultExp);
            total = tempData.Count();
            tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
            return tempData.ToList();
        }
        public IEnumerable<T> FindList<T>(Expression<Func<T, bool>> condition, string orderField, bool isAsc, int pageSize, int pageIndex, out int total) where T : class,new()
        {
            string[] _order = orderField.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = MyDBContext.Set<T>().Where(condition);
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<T>(resultExp);
            total = tempData.Count();
            tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
            return tempData.ToList();
        }

        #endregion


    }
}
