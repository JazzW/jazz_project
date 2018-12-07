using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.Web;

namespace Jazz.EF.Frame.IClass
{
    public class RepositoryFactory
    {
        public static IRepository<T> getRepository<T>() where T:class,InterfaceDBModel 
        {
            return new EFRepository<T>();
        }
    }

    public class EFRepository<T> : EFDAL<T> where T :class, InterfaceDBModel 
    {
        public EFRepository()
        {
            this.ConnectString = IConfig.ConnectString;
        }
    }
}
