package org.jazz.common.aop;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.lang.reflect.Proxy;

public class AopSerive {
	
	public static  Object run(Class<?> cls,String funcName,AopMess mess,Object... pars)
			throws NoSuchMethodException, 
			SecurityException, 
			IllegalAccessException, 
			IllegalArgumentException,
			InvocationTargetException,
			InstantiationException
	{
		
		Method method=null;
		Object res=null;
	    Method[] methods= cls.getMethods();
	    for(Method m:methods)
	    {
	    	if(m.getName().equals(funcName)){
	    		method=m;
	    	}
	    }
	    if(method!=null)
	    {
		    AopAttribute atr= method.getAnnotation(AopAttribute.class); 
		    AopSeriverInterface servie= atr.serive().newInstance();
		    servie.beforeRun(mess, pars);
		    try
		    {
		    	res=method.invoke(null, pars);
		    	servie.successRun(mess, res);
		    }
		    catch(Exception e)
		    {
		    	servie.errorRun(mess, e);
		    }
		    finally
		    {
		    	servie.afterRun(mess);
		    }
	    }
		return res;
	}
	
	public static  <T> AopProxy<T> createProxy(Class<T> cls,Object obj)
	{
	  return new AopProxy<T>(cls,obj); 
	}
	
	public static  <T> AopProxy<T> createProxy(Class<T> cls,Class<? extends AopSeriverInterface> serive,Object obj)
	{
	  return new AopProxy<T>(cls,serive,obj); 
	}
	
	@SuppressWarnings("unchecked")
	public static <T> T createProxy2(T target)
	{
		return  (T)Proxy.newProxyInstance(target.getClass().getClassLoader(), target.getClass().getInterfaces(),new AopInvocationHandler(target,null));
	}
	
	@SuppressWarnings("unchecked")
	public static <T> T createProxy2(T target,Class<? extends AopSeriverInterface> serivecls)
	{
		return  (T)Proxy.newProxyInstance(target.getClass().getClassLoader(), target.getClass().getInterfaces(),new AopInvocationHandler(target,serivecls));
	}
	
	@SuppressWarnings("unchecked")
	public static <T> T createProxy2(T target,Class<? extends AopSeriverInterface> serivecls,Class<?>... interfacecls)
	{
		return  (T)Proxy.newProxyInstance(target.getClass().getClassLoader(), interfacecls,new AopInvocationHandler(target,serivecls));
	}
	
	@SuppressWarnings("unchecked")
	public static <T> T createProxy2(Class<T> cls)
	{
		return  (T)Proxy.newProxyInstance(cls.getClassLoader(), cls.getInterfaces(),new AopInvocationHandler(null,null));
	}
	
	@SuppressWarnings("unchecked")
	public static <T> T createProxy2(Class<T> cls,Class<? extends AopSeriverInterface> serivecls)
	{
		return  (T)Proxy.newProxyInstance(cls.getClassLoader(), cls.getInterfaces(),new AopInvocationHandler(null,serivecls));
	}
	
	@SuppressWarnings("unchecked")
	public static <T> T createProxy2(Class<T> cls,Class<? extends AopSeriverInterface> serivecls,Class<?>... interfacecls)
	{
		return  (T)Proxy.newProxyInstance(cls.getClassLoader(), interfacecls,new AopInvocationHandler(null,serivecls));
	}

	
	public static Object run(Method method,AopMess mess,Object... pars) 
			throws IllegalAccessException, IllegalArgumentException, InvocationTargetException, InstantiationException
	{
		Object res=null;
		 AopAttribute atr= method.getAnnotation(AopAttribute.class); 
	    AopSeriverInterface servie= atr.serive().newInstance();
	    if(servie!=null)
	    {
		    servie.beforeRun(mess, pars);
		    try
		    {
		    	res=method.invoke(null, pars);
		    	servie.successRun(mess, res);
		    }
		    catch(Exception e)
		    {
		    	servie.errorRun(mess, e);
		    }
		    finally
		    {
		    	servie.afterRun(mess);
		    }
	    }
	    else
	    {
	    	res=method.invoke(null, pars);
	    }
	    return res;
	}
}
