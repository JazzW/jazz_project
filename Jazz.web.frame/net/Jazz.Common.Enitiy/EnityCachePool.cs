using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Jazz.Common.Enitiy
{
    class EnityCachePool
    {
        private static IDictionary<String,EnitiyCache> CachePool=new ConcurrentDictionary<String,EnitiyCache>();
	

	    public static EnitiyCache getCache<T>() 
	    {
            String key=typeof(T).Name;
            if(!EnityCachePool.CachePool.ContainsKey(key))
            {
                return EnityCachePool.setCache<T>();
            }
		    return EnityCachePool.CachePool[key];
	    }

        private static EnitiyCache setCache<T>()
        {
            String key = typeof(T).Name;
            EnitiyCache mycache = new EnitiyCache();
            mycache.setCls<T>();
            EnityCachePool.CachePool.Add(key, mycache);
            EnitiyCache cache = EnityCachePool.CachePool[key];
            return cache;
        }

    }
}
