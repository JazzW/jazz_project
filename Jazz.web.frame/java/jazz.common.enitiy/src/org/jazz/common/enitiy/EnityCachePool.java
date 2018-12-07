package org.jazz.common.enitiy;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * 实体反射缓存池
 * @author Wangjc
 *@author 2018.10.12
 */
public class  EnityCachePool {
	/**
	 * 实体反射缓存池
	 */
	private static Map<String,EnitiyCache> CachePool=new ConcurrentHashMap<String,EnitiyCache>();
	
	/**
	 * 获得实体反射缓存
	 * @param cls 实体类
	 * @return {@code EnitiyCache} 实体缓存
	 * @throws InstantiationException
	 * @throws IllegalAccessException
	 */
	public static EnitiyCache getCache(Class<?> cls) 
			throws InstantiationException, IllegalAccessException
	{
		EnitiyCache cache= EnityCachePool.CachePool.get(cls.getName());
		if(cache==null){
			 EnityCachePool.CachePool.put(cls.getName(), new EnitiyCache(cls));
			cache=  EnityCachePool.CachePool.get(cls.getName());
		}
		return cache;
	}
	
	/**
	 * 获得实体反射缓存
	 * @param name 实体类名字
	 * @return {@code EnitiyCache} 实体缓存
	 * @throws InstantiationException
	 * @throws IllegalAccessException
	 * @throws ClassNotFoundException
	 */
	public static EnitiyCache getCache(String name) 
			throws InstantiationException, IllegalAccessException, ClassNotFoundException
	{
		EnitiyCache cache=  EnityCachePool.CachePool.get(name);
		if(cache==null){
			 EnityCachePool.CachePool.put(name, new EnitiyCache(Class.forName(name)));
			cache=  EnityCachePool.CachePool.get(name);
		}
		return cache;
	}
}
