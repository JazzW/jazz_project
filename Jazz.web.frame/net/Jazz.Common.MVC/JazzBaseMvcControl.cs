using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Jazz.Common.Web;

namespace Jazz.Common.MVC
{
    public abstract class JazzBaseMvcControl<T,B,R>:Controller 
        where T:InterfaceDBModel 
        where R:IRepository<T>  
        where B:IBusiness<T,R>
    {
        public virtual IUserModel User { get; set; }

        public B Business { get; set; }


    }

    public abstract class JazzBaseControl<T> : Controller where T : InterfaceDBModel 
    {
        public virtual IUserModel User { get; set; }
    }
}
