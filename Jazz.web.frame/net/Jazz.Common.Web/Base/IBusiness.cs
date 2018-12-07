using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Web
{
    public abstract class IBusiness<T,R> where T:InterfaceDBModel where R:IRepository<T>
    {
        public virtual R Repository { get; set; }

        public virtual List<T> ISelectList<Tkey>(ITableConfig Config)
        {
            return Repository.ISelectList<Tkey>(Config);
        }

        public virtual Task<List<T>> ISelectListAsync<Tkey>(ITableConfig Config)
        {
            return Repository.ISelectListAsync<Tkey>(Config);
        }

        public virtual int ISelectListCount<Tkey>(ITableConfig Config)
        {
            return Repository.ISelectListCount<Tkey>(Config);
        }

        public virtual T ISelectFirst<Tkey>(ITableConfig Config)
        {
            return Repository.ISelectFirst<Tkey>(Config);
        }

        public virtual bool IInsert(params T[] models)
        {
            return Repository.IInsert(models);
        }

        public virtual Task<bool> IInsertAsync(params T[] models)
        {
            return Repository.IInsertAsync(models);
        }

        public virtual bool IUpdate(params T[] models)
        {
            return Repository.IUpdate(models);
        }

        public virtual Task<bool> IUpdateAsync(params T[] models)
        {
            return Repository.IUpdateAsync(models);
        }

        public virtual bool IDelete(params T[] models)
        {
            return Repository.IDelete(models);
        }

        public virtual Task<bool> IDeleteAsync(params T[] models)
        {
            return Repository.IDeleteAsync(models);
        }

    }


    public abstract class IBusiness<T>
        where T : InterfaceDBModel
    {
        public virtual IRepository<T> Repository { get; set; }

        public virtual List<T> ISelectList<Tkey>(ITableConfig Config)
        {
            return Repository.ISelectList<Tkey>(Config);
        }

        public virtual Task<List<T>> ISelectListAsync<Tkey>(ITableConfig Config)
        {
            return Repository.ISelectListAsync<Tkey>(Config);
        }

        public virtual T ISelectFirst<Tkey>(ITableConfig Config)
        {
            return Repository.ISelectFirst<Tkey>(Config);
        }

        public virtual bool IInsert(params T[] models)
        {
            return Repository.IInsert(models);
        }

        public virtual Task<bool> IInsertAsync(params T[] models)
        {
            return Repository.IInsertAsync(models);
        }

        public virtual bool IUpdate(params T[] models)
        {
            return Repository.IUpdate(models);
        }

        public virtual Task<bool> IUpdateAsync(params T[] models)
        {
            return Repository.IUpdateAsync(models);
        }

        public virtual bool IDelete(params T[] models)
        {
            return Repository.IDelete(models);
        }

        public virtual Task<bool> IDeleteAsync(params T[] models)
        {
            return Repository.IDeleteAsync(models);
        }

    }
}
