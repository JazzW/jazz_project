using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFrameWork.ADO.Models
{
    public class DBAttr
    {
        public abstract class FuncRunAttribute:Attribute
        {
            public virtual void BeforeRun()
            {

            }

            public virtual void AfterRun() 
            { 

            }

            public virtual void SuccessRun()
            {

            }

            public virtual void ErrorRun(Exception ex)
            {

            }
        }

        public sealed class TBAttribute : Attribute
        {
            public string TBName { get; set; }

            public string ColGobalStr { get; set; }

        }

        public sealed class IdenityKeyAttribute : Attribute
        {

        }

        public sealed class KeyAttribute : Attribute
        {
        }

        public sealed class IsNullAttribute : Attribute
        {

        }

        public sealed class DBAttribute : Attribute
        {
            public System.Data.SqlDbType T { get; set; }

            public int Size { get; set; }

            public bool Null { get; set; }

            public bool Check { get; set; }

            public bool Show { get; set; }

            public string Display{get;set;}

            public string ColName{get;set;}
        }

        public sealed class ChangeAttribute:Attribute
        {
            public Func<object, object> ChangeFunc { get; set; }
        }

        public sealed class SearchKeyAttribute : Attribute
        {

        }
    }
}
