using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.Enum;
using System.Linq.Expressions;
using System.Data.Common;
using Jazz.Helper.DataBase.LINQ;
namespace Jazz.Common.Web
{
   public  class ISelectConfig
    {
        public LinkEnum Link { get; set; }

        public List<IFiliter> Filiters { get; set; }

        public ISelectConfig(params IFiliter[] fs)
        {
            Filiters = fs.ToList();
        }

        public  string toSqlCmdExp<T>(ref DbParameter[] pars)
        {

            string sql = "";
            int i=0;
            foreach (var f in Filiters)
            {
                var par = pars[i];
                if (sql.Length == 0)
                    sql = f.toSqlCmdExp<T>(ref par);
                else
                {
                    switch (this.Link)
                    {
                        case LinkEnum.and:
                            sql +=" and "+ f.toSqlCmdExp<T>(ref par);
                            break;
                        case LinkEnum.or:
                            sql +=" or "+ f.toSqlCmdExp<T>(ref par);
                            break;
                    }
                }

                i++;
            }
            if (sql.Length == 0)
                sql = " 1=1 ";
            return sql;
        }

        public  Expression<Func<T, bool>> toLamadaExp<T>()
        {
            Expression<Func<T, bool>> exp = null;
            foreach (var f in Filiters)
            {
                if (exp == null)
                    exp = f.toLamadaExp<T>();
                else
                    switch (this.Link)
                    {
                        case LinkEnum.and:
                            exp = LamdaHelper.createAnd<T>(exp, f.toLamadaExp<T>());
                            break;
                        case LinkEnum.or:
                            exp = LamdaHelper.createOr<T>(exp, f.toLamadaExp<T>());
                            break;
                    }
            }
            return exp;
        }
    }
}
