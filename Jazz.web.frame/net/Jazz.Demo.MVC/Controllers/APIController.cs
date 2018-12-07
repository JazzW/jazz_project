using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jazz.Common.SOA;
using Jazz.Common.Web;
using Frame=Jazz.EF.Frame;
using Jazz.Demo.API.Business;
using Jazz.Helper.Common;
using System.Threading.Tasks;

namespace Jazz.Demo.API.Controllers
{
    public class APIController :JazzBaseApiControl<Models.Model_Table,Business.TestBLL>
    {
        public override Business.TestBLL Business
        {
            get
            {
               
                return MainBLL.Test;
            }
            set
            {
                base.Business = value;
            }
        }
        /// <summary>
        /// this is use ef dal to get data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Models.Model_Table>> GetList()
        {

            ITableConfig configs = new ITableConfig() { Page = 1, Length = 2, OrderCol = "Id" };
            Models.LoggerMess mess = new Models.LoggerMess() { UserID = "A0001" };
            var res = await new Func<ITableConfig, List<Models.Model_Table>>(this.Business.ISelectList<int>)
                .RunAsync<List<Models.Model_Table>, ITableConfig>(mess, configs);
            return res;
        }

        /// <summary>
        /// this is use Ado get data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Models.Model_Table>> GetList2()
        {
            Filiter<string> f1 = new Filiter<string>() {ColName="Name",Symbol=Jazz.Common.Enum.CompareSymbol.equal,Val="Jazz" };
            ITableConfig configs = new ITableConfig(new ISelectConfig(f1) { Link=Jazz.Common.Enum.LinkEnum.and}) { Page = 1, Length = 2 };
            Models.LoggerMess mess = new Models.LoggerMess() { UserID = "A0001" };
            var res = await new Func<ITableConfig, List<Models.Model_Table>>(MainBLL.test2.ISelectList<int>)
                .RunAsync<List<Models.Model_Table>, ITableConfig>(mess, configs);

            return res;
        }
    }
}