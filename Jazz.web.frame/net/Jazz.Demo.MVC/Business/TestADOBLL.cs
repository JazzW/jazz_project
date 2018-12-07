using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Frame = Jazz.ADO.Frame;
using Jazz.Common.Web;
using Jazz.Helper.Common;
using Jazz.Common.IOC;

namespace Jazz.Demo.API.Business
{
    public class TestADOBLL:IBusiness<Models.Model_Table>
    {
        //DIHelper _Seriver;

        static Unity UnityHelper;

        public TestADOBLL()
        {
            //_Seriver = new DIHelper();
            //_Seriver.AddSingleton<IRepository<Models.Model_Table>, Frame.IClass.ADORepository<Models.Model_Table>>(
            //    _ => new Frame.IClass.ADORepository<Models.Model_Table>());

        }

        public override IRepository<Models.Model_Table> Repository
        {
            get
            {
                if (UnityHelper == null)
                {
                    UnityHelper = new Unity(System.Web.Hosting.HostingEnvironment.MapPath(@"\Business\unity.config"));
                }
                //return _Seriver.getService<IRepository<Models.Model_Table>>();
                return UnityHelper.GetService<IRepository<Models.Model_Table>>();
             
            }
            set
            {
                base.Repository = value;
            }
        }

        [Common.FuncTran(ActionMess="ADO查询")]
        public override System.Collections.Generic.List<Jazz.Demo.API.Models.Model_Table> ISelectList<Tkey>(Jazz.Common.Web.ITableConfig Config)
        {
            return base.ISelectList<Tkey>(Config);
        }
    }
}