using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using Jazz.EFframe.Interface;
using System.Data.Common;
using Jazz.Common.Web;

namespace Jazz.EFframe.IClass
{
    /// <summary>
    /// DBContext 通用数据访问层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public abstract class EFDAL<T> : IDisposable
        where T : class,InterfaceDBModel
    {
        /// <summary>
        /// 通用性DBContext常用功能
        /// </summary>
        public IModel.IDBContextManage DBManger { get; set; }

        protected string ConnectString;

        public virtual void Create()
        {
            DBManger = new IModel.IDBContextManage(ConnectString);
        }

        public void Dispose()
        {
            if (DBManger != null)
                DBManger.Dispose();
        }


        public IEnumerable<T> ISelect<Tkey>(IConfig.EFSelectConfig<Tkey> config)
        {
            try
            {
                return DBManger.MyDBContext.Set<T>()
                    .Where(config.toExp<T>()).OrderBy(config.toOrderExp<T>())
                    .Skip((config.Page - 1) * config.Length).Take(config.Length).ToList();
            }
            catch
            {
                return null;
            }
        }

        public Task<List<T>> ISelectAsync<Tkey>(IConfig.EFSelectConfig<Tkey> config)
        {
 
            try
            {
                return 
                    DBManger.MyDBContext.Set<T>().Where(config.toExp<T>()).OrderBy(config.toOrderExp<T>()).
                    Skip((config.Page - 1) * config.Length).Take(config.Length).ToListAsync();
            }
            catch
            {
                return null;
            }
            finally
            {
                DBManger.Close();
            }
        }

        public IEnumerable<T> ISelect(Expression<Func<T, bool>> exp)
        {
            try
            {
                return DBManger.MyDBContext.Set<T>().Where(exp).ToList();
            }
            catch
            {
                return null;
            }
        }

        public Task<List<T>> ISelectAsync(Expression<Func<T, bool>> exp)
        {
            try
            {
                return DBManger.MyDBContext.Set<T>().Where(exp).ToListAsync<T>();
            }
            catch
            {
                return null;
            }
        }

     }
}
