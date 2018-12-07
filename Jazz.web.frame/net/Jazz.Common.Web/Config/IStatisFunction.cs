using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Jazz.Common.Web.Config
{
    public interface IStatisFunction
    {
        void InitFunc();

        bool Check(IEnum Tenum);

        string toSQL(ITableConfig configs, string sqlSource, out DbParameter[] pars,bool skipOrder=false);

    }
}
