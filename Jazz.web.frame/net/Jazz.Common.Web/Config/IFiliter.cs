using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.Enum;
using System.Data.Common;
using System.Linq.Expressions;
using Jazz.Helper.DataBase.LINQ;

namespace Jazz.Common.Web
{
    public abstract class IFiliter
    {
        public string ColName { get; set; }

        public object Val { get; set; }

        public CompareSymbol Symbol { get; set; }

        public virtual string toSqlCmdExp<T>(ref DbParameter pars)
        {
            
            string sql = "";
            
            return sql;
        }

        public virtual  Expression<Func<T, bool>> toLamadaExp<T>()
        {
            return null;
        }
    }

    public class Filiter<S> : IFiliter
    {

        public override Expression<Func<T, bool>> toLamadaExp<T>()
        {
            Expression<Func<T, bool>> exp = null;
            switch (this.Symbol)
            {
                case CompareSymbol.equal:
                    exp = LamdaHelper.CreateEqual<T, S>(ColName, (S)Val);
                    break;
                case CompareSymbol.greater:
                    exp = LamdaHelper.CreateGreaterThan<T, S>(ColName, (S)Val);
                    break;
                case CompareSymbol.less:
                    exp = LamdaHelper.CreateLessThan<T, S>(ColName, (S)Val);
                    break;
                case CompareSymbol.like:
                    exp = LamdaHelper.GetContains<T>(ColName, Val.ToString());
                    break;
                default:
                    exp = null;
                    break;
            }

            return exp;
        }

        public override string toSqlCmdExp<T>(ref DbParameter pars)
        {
            string sql = "";
            pars.Value = (S)Val;
            switch (this.Symbol)
            {
                case CompareSymbol.equal:
                    sql = string.Format(" [{0}]={1} ",ColName, pars.ParameterName);
                    break;
                case CompareSymbol.greater:
                    sql = string.Format(" [{0}]>{1} ", ColName, pars.ParameterName);
                    break;
                case CompareSymbol.less:
                    sql = string.Format(" [{0}]<{1} ", ColName, pars.ParameterName);
                    break;
                case CompareSymbol.like:
                    sql = string.Format(" [{0}] like {1} ", ColName, pars.ParameterName);
                    break;
                default:
                    sql = "";
                    break;
            }

            return sql;
        }
    }
}
