using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.Web;

namespace Jazz.SOA.JMS.Service
{
    public class DBRepository:IRepository<Class.BaseDBModel>
    {
        public Jazz.Helper.DataBase.Common.DBManger MyDBManger { get; set; }

        public System.Data.DataTable executeSQL(string sql, params System.Data.Common.DbParameter[] pars)
        {
            return null;
        }

        public List<Class.BaseDBModel> ISelectList<Tkey>(ITableConfig Config)
        {
            return null;
        }

        public int ISelectListCount<Tkey>(ITableConfig Config)
        {
            return -1;
        }

        public Task<List<Class.BaseDBModel>> ISelectListAsync<Tkey>(ITableConfig Config)
        {
            return null;
        }

        public Class.BaseDBModel ISelectFirst<Tkey>(ITableConfig Config)
        {
            return null;
        }

        public bool IInsert(params Class.BaseDBModel[] models)
        {
            return false;
        }

        public Task<bool> IInsertAsync(params Class.BaseDBModel[] models)
        {
            return null;
        }

        public bool IUpdate(params Class.BaseDBModel[] models)
        {
            return false;
        }

        public Task<bool> IUpdateAsync(params Class.BaseDBModel[] models)
        {
            return null;
        }

        public bool IDelete(params Class.BaseDBModel[] models)
        {
            return false;
        }

        public Task<bool> IDeleteAsync(params Class.BaseDBModel[] models)
        {
            return null;
        }
    }
}
