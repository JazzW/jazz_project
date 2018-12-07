using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Web
{
    public interface IRepositoryFactory
    {
        IRepository<T> getRepository<T>() where T:InterfaceDBModel;
    }
}
