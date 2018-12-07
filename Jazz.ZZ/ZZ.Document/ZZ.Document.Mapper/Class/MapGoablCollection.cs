using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace ZZ.Document.Mapper.Class
{
    /// <summary>
    /// 全局映射字典，用于MapVItem
    /// </summary>
    public  class MapGoablCollection
    {
        static ConcurrentDictionary<string, object> _dict = new ConcurrentDictionary<string, object>();

        public static void addObj(string key, object obj)
        {
            if (_dict.Keys.Contains(key)) return;
            while(!_dict.TryAdd(key, obj))
            {
                System.Threading.Thread.Sleep(500);
            }
        }

        public static T getVal<T>(string key)
        {
            return (T)_dict[key];
        }

        public static object getVal(string key)
        {
            return _dict[key];
        }

        public static void Clear()
        {
          
            _dict.Clear();
        }
    }
}
