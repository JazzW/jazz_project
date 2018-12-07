using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jazz.Helper.Common
{
    public class ReflectHelper
    {
        public static T getInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public static object getInstance(Type T)
        {
            return Activator.CreateInstance(T, true);
        }

        public static object getInstance(string TypeName)
        {
           return   Activator.CreateInstance(Type.GetType(TypeName));
        }

        public static object getInstance(string TypeName,string AssemblyName)
        {
           return   Activator.CreateInstance(AssemblyName, TypeName);
        }

        public static Type getType(string TypeName)
        {
          return  Type.GetType(TypeName);
        }

        public static MethodBase getMethod(string TypeName, string MethodName)
        {
            var T = getType(TypeName);
            return  T.GetMethod(MethodName);
        }

        public static MethodBase getMethod<T>(string MethodName)
        {
            var T1 = typeof(T);
            return T1.GetMethod(MethodName);
        }

        public static MethodBase[] getMethod<T, A>() where A : Attribute
        {
            var T1 = typeof(T);
            return T1.GetMethods().Where(e => e.GetCustomAttributes(typeof(A), true).Length > 0).ToArray();
        }

        public static PropertyInfo getProperty<T>(string PropertyName)
        {
            var T1 = typeof(T);
            return T1.GetProperty(PropertyName);
        }

        public static PropertyInfo[] getProperty<T,A>()where A:Attribute
        {
            var T1 = typeof(T);
            return T1.GetProperties().Where(e => e.GetCustomAttributes(typeof(A), true).Length > 0).ToArray();
        }
    }
}
