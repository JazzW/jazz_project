using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using Jazz.Common.Web;

namespace Jazz.EF.Frame.IClass
{
    /// <summary>
    /// DBContext 通用数据访问层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public abstract class EFDAL<T> : IRepository<T>
        where T : class,InterfaceDBModel
    {
        public IModel.IDBContextManage DBManger { get { return new IModel.IDBContextManage(ConnectString); } }

        protected string ConnectString { get; set; }



        public List<T> ISelectList<Tkey>(ITableConfig Config)
        {
            try
            {
                using (IModel.IDBContextManage DBManger = new IModel.IDBContextManage(ConnectString))
                {
                    return DBManger.MyDBContext.Set<T>()
                        .Where(Config.toLamadaExp<T>()).OrderBy(Config.toOrderExp<T, Tkey>())
                        .Skip((Config.Page - 1) * Config.Length).Take(Config.Length).ToList();
                }
                
            }
            catch(Exception ex)
            {

                throw ex;
            }
        }

        public int ISelectListCount<TKey>(ITableConfig Config)
        {
            return DBManger.MyDBContext.Set<T>().Where(Config.toLamadaExp<T>()).Count();
        }

        public Task<List<T>> ISelectListAsync<Tkey>(ITableConfig Config)
        {
            try
            {
                using (IModel.IDBContextManage DBManger = new IModel.IDBContextManage(ConnectString))
                {
                    return DBManger.MyDBContext.Set<T>()
                        .Where(Config.toLamadaExp<T>()).OrderBy(Config.toOrderExp<T, Tkey>())
                        .Skip((Config.Page - 1) * Config.Length).Take(Config.Length).ToListAsync();
                }
                
            }
            catch
            {
                return null;
            }
        }

        public T ISelectFirst<Tkey>(ITableConfig Config)
        {
            try
            {
                using (IModel.IDBContextManage DBManger = new IModel.IDBContextManage(ConnectString))
                {
                    return DBManger.MyDBContext.Set<T>()
                        .Where(Config.toLamadaExp<T>()).OrderBy(Config.toOrderExp<T, Tkey>())
                        .Skip((Config.Page - 1) * Config.Length).Take(Config.Length).FirstOrDefault();
                }
                
            }
            catch
            {
                return null;
            }
        }

        public bool IInsert(params T[] models)
        {
            using (IModel.IDBContextManage DBManger = new IModel.IDBContextManage(ConnectString))
            {
                if (DBManger.Insert<T>(models) > 0)
                    return true;
                return false;
            }
        }

        public Task<bool> IInsertAsync(params T[] models)
        {
          return null;
            
        }

        public bool IUpdate(params T[] models)
        {
            using (IModel.IDBContextManage DBManger = new IModel.IDBContextManage(ConnectString))
            {
                if (DBManger.Update<T>(models) > 0)
                    return true;
                return false;
            }
            
        }

        public Task<bool> IUpdateAsync(params T[] models)
        {
            return null;
        }

        public bool IDelete(params T[] models)
        {
            using (IModel.IDBContextManage DBManger = new IModel.IDBContextManage(ConnectString))
            {
                if (DBManger.Delete<T>(models) > 0)
                    return true;
                return false;
            }
            
        }

        public Task<bool> IDeleteAsync(params T[] models)
        {
            return null;
        }


     }
}
