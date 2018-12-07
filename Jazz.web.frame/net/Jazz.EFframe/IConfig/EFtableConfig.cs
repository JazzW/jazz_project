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
    public class EFtableConfig
    {

        public EFtableConfig(params EFFiliter[] fs)
        {
            filiter = fs.ToList();
        }

        public enum link
        {
            and,
            or
        }

        public link linkType { get; set; }

        public List<EFFiliter> filiter { get; set; }

        public Expression<Func<T, bool>> toExp<T>() where T : InterfaceDBModel
        {
            Expression<Func<T, bool>> exp = null;
            foreach(var f in filiter)
            {
                if (exp == null)
                    exp = f.toExp<T>();
                else
                    switch (this.linkType)
                    {
                        case link.and:
                            exp = LamdaHelper.createAnd<T>(exp, f.toExp<T>());
                            break;
                        case link.or:
                            exp = LamdaHelper.createOr<T>(exp, f.toExp<T>());
                            break;
                    }
            }
            return exp;
        }
    }
}
