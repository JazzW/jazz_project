using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace Jazz.EFframe.IMap
{
    public abstract class BaseDomainMapping<T> : EntityTypeConfiguration<T>
           where T :class
    {

        public BaseDomainMapping()
        {
            Init();
        }
        /// <summary>
        /// 初始化代码
        /// </summary>
        public virtual void Init()
        {
            Console.WriteLine("Init");
        }
    }

    public class BaseDomain
    {

    }
}
