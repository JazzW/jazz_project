package org.jazz.helper.common;

import java.lang.annotation.Annotation;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.HashMap;
import java.util.Map;

/**
 * ·´Éä»º´æ×ÓÏî
 * @author wjc
 *
 */
public class reflectCache {
	private Class<?> cls;
	
	private Object Instance;
	
	public Class<?> getCls() {
		return cls;
	}
	
	public Object getInstance() {
		return Instance;
	}

	private void setInstance(Object instance) {
		Instance = instance;
	}

	public void setCls(Class<?> cls) 
			throws InstantiationException, IllegalAccessException {
		this.cls = cls;
		this.setInstance(cls.newInstance());
		this.fieldMap.clear();
		for(Field f:cls.getDeclaredFields()){
			this.fieldMap.put(f.getName(), f);
		}
		
		this.methodMap.clear();
		for(Method m : cls.getDeclaredMethods())
		{
			this.methodMap.put(m.getName(), m);
		}

		this.annotationMap.clear();
		for(Annotation a:cls.getDeclaredAnnotations())
		{
			this.annotationMap.put(a.annotationType().getName(),a );
		}
	}
	
	public reflectCache(){
		
	}
	
	public reflectCache(Class<?> cls) 
			throws InstantiationException, IllegalAccessException{
		this.setCls(cls);
	}
	
	private Map<String,Field> fieldMap = new HashMap<String,Field>();
	
	private Map<String,Method> methodMap = new HashMap<String,Method>();
				
	private Map<String,Annotation> annotationMap=new HashMap<String,Annotation>();

	public Field getField(String name)
	{
		return this.fieldMap.get(name);
	}
	
	public Method getMethod(String name)
	{
		return this.methodMap.get(name);
	}
	
	public Annotation getAnnotation(String name)
	{
		return this.annotationMap.get(name);
	}
}
