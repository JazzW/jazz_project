using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using WebFrameWork.EF;
using System.Data.Common;

namespace WebFrameWork.EF
{
    /// <summary>
    /// EF 通用数据访问层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class EFDAL<T, S>:IDisposable
        where T :Model.IDBModel
        where S : DbContext
    {
        protected S _MyDataBase;

        protected Func<S> InitDB;

        protected Action<S> disposeDB;

        public void dispose(S DB)
        {
            if (this.disposeDB != null)
                this.disposeDB(DB);
        }

        public void Dispose()
        {
            if(_MyDataBase!=null)
                _MyDataBase.Dispose();
        }
        
        public List<T> ISelect<Tkey>(Model.EFSelectConfig<Tkey> config)
        {
            S _db = null;
            try
            {
                _db = InitDB();
                return _db.Set<T>().Where(config.toExp<T>()).OrderBy(config.toOrderExp<T>()).Skip((config.Page - 1) * config.Length).Take(config.Length).ToList();
            }
            catch
            {
                return null;
            }
            finally
            {
                this.dispose(_db);
            }
        }

        public Task<List<T>> ISelectAsync<Tkey>(Model.EFSelectConfig<Tkey> config)
        {
            S _db = null;
            try
            {
                _db = InitDB();
                return _db.Set<T>().Where(config.toExp<T>()).OrderBy(config.toOrderExp<T>()).Skip((config.Page - 1) * config.Length).Take(config.Length).ToListAsync();
            }
            catch
            {
                return null;
            }
            finally
            {
                this.dispose(_db);
            }
        }

        public List<T> ISelect(Expression<Func<T, bool>> exp)
        {
            S _db = null;
            try
            {
                _db = InitDB();
                return _db.Set<T>().Where(exp).ToList();
            }
            catch
            {
                return null;
            }
            finally
            {
                this.dispose(_db);
            }
        }

        public Task<List<T>> ISelectAsync(Expression<Func<T, bool>> exp)
        {
            S _db = null;
            try
            {
                _db = InitDB();
                return _db.Set<T>().Where(exp).ToListAsync<T>();
            }
            catch
            {
                return null;
            }
            finally
            {
                this.dispose(_db);
            }
        }

        public bool IAdd(params T[] model)
        {
            S _db = null;
            try
            {
                _db = InitDB();
                _db.Set<T>().AddRange(model);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                this.dispose(_db);
            }
        }

        public bool IUpdate(T model, params object[] KeysVal)
        {
            S _db = null;
            try
            {
                _db = InitDB();

                EF.Model.EnityManage<T>.SetKeysVal(model, KeysVal);

                _db.Set<T>().Attach(model);
                _db.Entry<T>(model).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

            finally
            {
                this.dispose(_db);
            }
        }

        public List<M> IrunSQL2<M>(string sqlstr, params DbParameter[] pars)
        {
            S _db = null;
            try
            {
                _db = InitDB();
                return _db.Database.SqlQuery<M>(sqlstr, pars).Cast<M>().ToList();
            }
            catch
            {
                return null;
            }

            finally
            {
                this.dispose(_db);
            }
        }

        public M IrunSQL<M>(string sqlstr, params DbParameter[] pars)
        {
            S _db = null;
            try
            {
                return _db.Database.SqlQuery<M>(sqlstr, pars).FirstOrDefault();
            }
            catch
            {
                return default(M);
            }

            finally
            {
                this.dispose(_db);
            }
        }
     }
}
