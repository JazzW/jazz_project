using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jazz.Common.Web;
using Frame = Jazz.EF.Frame;
using Jazz.Helper.Common;
using Jazz.EF.Frame.IClass;

namespace Jazz.Demo.API.Business
{
    public  class MainBLL
    {
        public static TestBLL Test= new TestBLL();
        public static TestADOBLL test2 = new TestADOBLL();
    }

    public class TestBLL:IBusiness<Models.Model_Table>
    {
        DIHelper _Seriver;

        public TestBLL()
        {
            _Seriver = new DIHelper();
            _Seriver.AddSingleton<IRepository<Models.Model_Table>,EFRepository<Models.Model_Table>>
                (_ =>RepositoryFactory.getRepository<Models.Model_Table>() as EFRepository<Models.Model_Table>);
        }

        public override IRepository<Models.Model_Table> Repository
        {
            get
            {
                return _Seriver.getService<IRepository<Models.Model_Table>>();
            }
            set
            {
                base.Repository = value;
            }
        }

        [Common.FuncTran(ActionMess="查询table表的list")]
        public override List<Models.Model_Table> ISelectList<Tkey>(ITableConfig Config)
        {
            return base.ISelectList<Tkey>(Config);
        }
    }
}