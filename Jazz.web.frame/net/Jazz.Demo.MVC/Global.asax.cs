using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Jazz.EF.Frame.IModel;
using Jazz.EF.Frame.IClass;
using ADO=Jazz.ADO.Frame.IClass;

namespace Jazz.Demo.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            MyDbContext.AssemblyName = typeof(Models.Model_TableMapping);
            ADO.Frame.IClass.IConfig.setCache(true);
            IConfig.ConnectString = @"
                    server=.;uid=sa;pwd=123;database=SDA_DB;Min Pool Size = 5;Max Pool Size=100;";
            ADO.Frame.IClass.IConfig.ConnectString = @"
                    server=.;uid=sa;pwd=123;database=SDA_DB;Min Pool Size = 5;Max Pool Size=100;";
            ADO.Frame.IClass.IConfig.DbType = Jazz.Helper.DataBase.Common.DBConfig.DatabaseType.SqlServer;
        }
    }
}
