using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jazz.Helper.Common;

namespace Jazz.Demo.API.Common
{
    public class FuncTranAttribute : FuncRunAttribute
    {
        public string ActionMess { get; set; }

        public override void AfterRun(IMess mess)
        {
            base.AfterRun(mess);
        }

        public override void BeforeRun(IMess mess)
        {
            base.BeforeRun(mess);
        }

        public override void ErrorRun(IMess mess, Exception ex)
        {
            Models.LoggerMess me = (Models.LoggerMess)mess;
            me.LogUsedTime = (DateTime.Now - me.LogTime).TotalMilliseconds;
            me.LogState = -1;
            me.LogMess = this.ActionMess;
            Common.Logger.log(me);
        }

        public override void SuccessRun(IMess mess, object res)
        {
            Models.LoggerMess me = (Models.LoggerMess)mess;
            me.LogUsedTime = (DateTime.Now - me.LogTime).TotalMilliseconds;
            me.LogState = 1;
            me.LogMess = this.ActionMess;
            Common.Logger.log(me);
        }
    }
}