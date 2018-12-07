using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Jazz.Helper.Common;

namespace Jazz.Common.IOC
{
    public static class IIOC
    {
        public static List<IIOC_Item> ItemList { get; set; }

        public static MethodBase Method<T>(this T obj, string MethodName)
        {
            return null;
        }

        public static object Property<T>(this T obj, string ProName)
        {
            return null;
        }

    }

    public class IIOC_Item
    {
        public string ClassName { get; set; }

        public IocConig[] IOCPropertyName { get; set; }

        public IocConig[] IOCMethodName { get; set; }
    }

    public class IocConig
    {
        public string Name { get; set; }

        public string ClassName { get; set; }

        public string AseName { get; set; }

        public string NameSpace { get; set; }

        public IocTypeEnum Type { get; set; }

        public MethodBase Method<T>()
        {
            if (Type != IocTypeEnum.Method) return null;
           return ReflectHelper.getMethod<T>(Name);
        }

        public object Property<T>()
        {
            if (Type != IocTypeEnum.Proporty) return null;

            return ReflectHelper.getProperty<T>(Name);
        }

        public MethodBase Method()
        {
            if (Type != IocTypeEnum.Method) return null;
            return ReflectHelper.getMethod(ClassName,Name);
        }

    }

    public enum IocTypeEnum
    {
        Method,
        Proporty,
        Instance
    }
}
