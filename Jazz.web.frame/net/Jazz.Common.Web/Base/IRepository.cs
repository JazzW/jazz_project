using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Web
{
    public interface IRepository<T> where T : InterfaceDBModel
    {  
        System.Data.DataTable  executeSQL(string sql,params System.Data.Common.DbParameter[] pars);

        List<T> ISelectList<Tkey>(ITableConfig Config);

        int ISelectListCount<Tkey>(ITableConfig Config);

        Task<List<T>> ISelectListAsync<Tkey>(ITableConfig Config);

        T ISelectFirst<Tkey>(ITableConfig Config);

        bool IInsert(params T[] models);

        Task<bool> IInsertAsync(params T[] models);

        bool IUpdate(params T[] models);

        Task<bool> IUpdateAsync(params T[] models);

        bool IDelete(params T[] models);

        Task<bool> IDeleteAsync(params T[] models);


    }
}
