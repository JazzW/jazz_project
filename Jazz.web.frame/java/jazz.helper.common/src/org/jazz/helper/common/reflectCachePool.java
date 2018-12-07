package org.jazz.helper.common;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
/**
 * ∑¥…‰ª∫¥Ê≥ÿ
 * @author wjc
 *
 */
public class reflectCachePool {
	private static Map<String,reflectCache> CachePool=new ConcurrentHashMap<String,reflectCache>();
	
	public static reflectCache getCache(Class<?> cls) 
			throws InstantiationException, IllegalAccessException
	{
		reflectCache cache= reflectCachePool.CachePool.get(cls.getName());
		if(cache==null){
			reflectCachePool.CachePool.put(cls.getName(), new reflectCache(cls));
			cache= reflectCachePool.CachePool.get(cls.getName());
		}
		return cache;
	}
}
