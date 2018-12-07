using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Helper.Common
{
    public interface IMess { }

    public abstract class FuncRunAttribute : Attribute
    {
        public virtual void BeforeRun(IMess mess)
        {

        }

        public virtual void AfterRun(IMess mess)
        {

        }

        public virtual void SuccessRun(IMess mess,object res)
        {

        }

        public virtual void ErrorRun(IMess mess,Exception ex)
        {

        }
    }

}
