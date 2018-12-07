using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jazz.Common.MVC
{
    public class JazzBaseAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] AuthorName;

        public bool Login = false;

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext.Session[MVCIConfig.UserSessionKey] != null)
            {
                if (AuthorName != null)
                {
                    if (AuthorName.Contains(httpContext.Session[MVCIConfig.UserSessionKey].ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new JsonResult()
            {
                Data = new
                {
                    status = 2,
                    Msg = "请求错误",
                    log = "权限错误"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
