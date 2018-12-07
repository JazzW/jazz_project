using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.SOA.JMS.Service
{
    public class MessageService
    {
        static DBRepository _Repository = new DBRepository();

        public static void saveMessage(Class.BaseDBModel model)
        {
            _Repository.IUpdate(model);
      
        }


    }
}
