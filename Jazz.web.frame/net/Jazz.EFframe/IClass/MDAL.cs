using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using Jazz.EFframe.Interface;
using Config= Jazz.EFframe.IConfig;
using Jazz.Common.Web;

namespace Jazz.EFframe.IClass
{
    /// <summary>
    /// EF  通用数据访问层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class EFMDAL<T, S> : IDisposable
        where T:class,InterfaceDBModel
        where S : DbContext
    {
        protected S _db;

        protected Func<S> InitDB;

        public IModel.IDBContextManage DBManger { get; set; }

        public void Create()
        {
            if (_db == null)
                _db = InitDB();
            DBManger = new IModel.IDBContextManage() { MyDBContext = _db };
        }

        public void Dispose()
        {
            if (_db != null)
                _db.Dispose();
            if (DBManger != null)
                DBManger.Dispose();
        }

        public List<T> ISelect<Tkey>(Config.EFSelectConfig<Tkey> config)
        {
            try
            {
                var whereExp = config.toExp<T>();
                if(whereExp==null)
                    return _db.Set<T>().OrderBy(config.toOrderExp<T>()).Skip((config.Page - 1) * config.Length).Take(config.Length).ToList();
                return _db.Set<T>().Where(whereExp).OrderBy(config.toOrderExp<T>()).Skip((config.Page - 1) * config.Length).Take(config.Length).ToList();
            }
            catch
            {
                return null;
            }

        }

        public Task<List<T>> ISelectAsync<Tkey>(Config.EFSelectConfig<Tkey> config)
        {
            try
            {
                return _db.Set<T>().Where(config.toExp<T>()).OrderBy(config.toOrderExp<T>()).Skip((config.Page - 1) * config.Length).Take(config.Length).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public List<T> ISelect(Expression<Func<T, bool>> exp)
        {
           try
            {
                if (exp == null)
                    return _db.Set<T>().ToList();
                return _db.Set<T>().Where(exp).ToList();
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
                return _db.Set<T>().Where(exp).ToListAsync<T>();
            }
            catch
            {
                return null;
            }

        }

    }
}
