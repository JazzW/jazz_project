using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Jazz.Helper.DataBase.LINQ;
using Jazz.EFframe.Interface;
using Jazz.Common.Web;

namespace Jazz.EFframe.IConfig
{
    public class EFSelectConfig<Tkey>
    {
        public EFSelectConfig()
        {
            Conifgs = new List<EFtableConfig>();
        }

        public EFSelectConfig(EFtableConfig config)
        {
            Conifgs = new List<EFtableConfig>();
            Conifgs.Add(config);
        }

        public int Page { get; set; }

        public int Length { get; set; }

        public List<EFtableConfig> Conifgs { get; set; }

        public string OrderCol { get; set; }

        public Expression<Func<T, Tkey>> toOrderExp<T>() where T : InterfaceDBModel
        {
            return LamdaHelper.GetOrderExpression<T, Tkey>(OrderCol);
        }

        public Expression<Func<T,bool>> toExp<T>() where T:InterfaceDBModel
        {
            Expression<Func<T, bool>> exp = null;
            foreach (var f in Conifgs)
            {
                if (exp == null)
                    exp = f.toExp<T>();
                else
                    exp = LamdaHelper.createAnd<T>(exp, f.toExp<T>());
            }
            return exp;
        }
    }


}
