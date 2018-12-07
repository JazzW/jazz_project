using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jazz.Helper.Common;

namespace Jazz.Demo.API.Common
{

    public class Logger
    {
        static EF.Frame.IClass.EFRepository<Models.LoggerMess> Repository = new EF.Frame.IClass.EFRepository<Models.LoggerMess>();

        public static string LogTBName { get; set; }

        public static bool log(Models.LoggerMess mess)
        {
           return Repository.IInsert(mess);
        }
    }
}