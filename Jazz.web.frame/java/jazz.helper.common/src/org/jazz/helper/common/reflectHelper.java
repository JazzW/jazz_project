package org.jazz.helper.common;

import java.lang.annotation.Annotation;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.List;

/**
 * 反射工具组件
 * @author wjc
 *
 */
public class reflectHelper {
	public static Method getMethod(Class<?> cls,String MethodName)
	{
		Method method=null;
	    Method[] methods= cls.getMethods();
	    for(Method m:methods)
	    {
	    	if(m.getName().equals(MethodName)){
	    		method=m;
	    	}
	    }
	    return method;
	}
	
	public static List<Method> getMethods(Class<?> cls,Class<? extends Annotation> AtrCls)
	{
	    Method[] methods= cls.getDeclaredMethods();
	    List<Method>  res=new ArrayList<Method>();
	    for(Method m:methods)
	    {
			Annotation atr=m.getAnnotation(AtrCls);
			if(atr!=null)
				res.add(m);
	    }
	    return res;
	}
	
	public static Method getMethod(Class<?> cls,String MethodName,Class<?>... parsCls)
			throws NoSuchMethodException, SecurityException
	{
	    return cls.getDeclaredMethod(MethodName, parsCls);
	}
	
	public static Field getField(Class<?> cls,String FieldName)
			throws NoSuchFieldException
	{
		return  cls.getDeclaredField(FieldName);
	}
	
	public static List<Field> getFields(Class<?> cls,Class<? extends Annotation> AtrCls)
			throws NoSuchFieldException
	{
		Field[] fs=  cls.getDeclaredFields();
		List<Field>  res=new ArrayList<Field>();
		for(Field f:fs)
		{
			Annotation atr=f.getAnnotation(AtrCls);
			if(atr!=null)
				res.add(f);
		}
		return res;
	}
	
	public static Class<?>[] getInterface(Class<?> cls)
	{
		return cls.getInterfaces();
	}

}
