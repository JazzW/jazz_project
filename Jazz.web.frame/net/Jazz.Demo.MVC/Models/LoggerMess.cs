using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jazz.Helper.Common;
using Jazz.Common.Web;
using Jazz.EFframe.IMap;
using Frame = Jazz.EF.Frame;

namespace Jazz.Demo.API.Models
{
    public class LoggerMess : IMess,InterfaceDBModel
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public string LogMess { get; set; }

        public DateTime LogTime { get; set; }

        public double LogUsedTime { get; set; }

        public int LogState { get; set; }

        public LoggerMess() { this.LogTime = DateTime.Now; }
    }



    public class LogMapping : BaseDomainMapping<LoggerMess>
    {
        public override void Init()
        {
            this.ToTable("Log");
            this.HasKey(l => l.ID);
            this.Property(l => l.UserID).HasMaxLength(50).IsRequired();//设置Name属性长度为200 并且是必填
            this.Property(l => l.LogTime).IsRequired();
            this.Property(l => l.LogUsedTime).IsRequired();
            this.Property(l => l.LogState).IsRequired();
            this.Property(l => l.LogMess).IsRequired(); 

        }
    }

}