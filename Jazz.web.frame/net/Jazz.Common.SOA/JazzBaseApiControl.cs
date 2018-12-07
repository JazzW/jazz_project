using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using Jazz.Common.Web;
using Jazz.Helper.Web;

namespace Jazz.Common.SOA
{
    public abstract class JazzBaseApiControl<T,B,R>:ApiController
        where T:InterfaceDBModel 
        where R:IRepository<T>  
        where B:IBusiness<T,R>
    {
        public virtual IUserModel User { get; set; }

        public virtual B Business { get; set; }

        protected virtual string ToJsonResult(object data)
        {
            return data.ToJson();
        }
   
        protected virtual HttpResponseMessage Success(string message)
        {
            return new HttpResponseMessage { Content = new StringContent(
                new ResponseResult (){ State=1,Msg=message }.ToJson(),
                Encoding.GetEncoding("UTF-8"),
                "application/json"
                ) };
        }
 
        protected virtual HttpResponseMessage Success(string message, object data)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                    new ResponseResult<object>() { State = 1, Msg = message,Data=data }.ToJson(),
                    Encoding.GetEncoding("UTF-8"),
                    "application/json"
                    )
            };
        }

        protected virtual HttpResponseMessage Error(string message)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                    new ResponseResult() { State = 3, Msg = message }.ToJson(),
                    Encoding.GetEncoding("UTF-8"),
                    "application/json"
                    )
            };
        }
    }

    public abstract class JazzBaseApiControl<T,B> : ApiController 
        where T : InterfaceDBModel  
        where B:IBusiness<T>
    {
        public virtual IUserModel User { get; set; }

        public virtual B Business { get; set; }

        protected virtual string ToJsonResult(object data)
        {
            return data.ToJson();
        }

        protected virtual HttpResponseMessage Success(string message)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                    new ResponseResult() { State = 1, Msg = message }.ToJson(),
                    Encoding.GetEncoding("UTF-8"),
                    "application/json"
                    )
            };
        }

        protected virtual HttpResponseMessage Success(string message, object data)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                    new ResponseResult<object>() { State = 1, Msg = message, Data = data }.ToJson(),
                    Encoding.GetEncoding("UTF-8"),
                    "application/json"
                    )
            };
        }

        protected virtual HttpResponseMessage Error(string message)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                    new ResponseResult() { State = 3, Msg = message }.ToJson(),
                    Encoding.GetEncoding("UTF-8"),
                    "application/json"
                    )
            };
        }
    }
}
