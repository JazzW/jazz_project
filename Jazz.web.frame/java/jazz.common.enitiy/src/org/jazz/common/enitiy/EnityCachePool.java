package org.jazz.common.enitiy;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * ʵ�巴�仺���
 * @author Wangjc
 *@author 2018.10.12
 */
public class  EnityCachePool {
	/**
	 * ʵ�巴�仺���
	 */
	private static Map<String,EnitiyCache> CachePool=new ConcurrentHashMap<String,EnitiyCache>();
	
	/**
	 * ���ʵ�巴�仺��
	 * @param cls ʵ����
	 * @return {@code EnitiyCache} ʵ�建��
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
	 * ���ʵ�巴�仺��
	 * @param name ʵ��������
	 * @return {@code EnitiyCache} ʵ�建��
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
