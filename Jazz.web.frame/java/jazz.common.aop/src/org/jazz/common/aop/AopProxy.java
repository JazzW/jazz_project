package org.jazz.common.aop;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.function.Function;

public class AopProxy<T> {
	
	private Class<T> _cls;
	
	private Object _obj;
	
	private Class<? extends AopSeriverInterface> _seriveCls;
	
	public AopProxy(Class<T> cls,Object obj)
	{
		this.set_cls(cls);
		this.set_obj(obj);
	}

	public AopProxy(Class<T> cls,Class<? extends AopSeriverInterface> serive, Object obj)
	{
		this._cls=cls;
		this._seriveCls=serive;
		this._obj=obj;
	}
	
	public Class<T> get_cls() {
		return _cls;
	}

	public void set_cls(Class<T> _cls) {
		this._cls = _cls;
	}

	public Object get_obj() {
		return _obj;
	}

	public void set_obj(Object _obj) {
		this._obj = _obj;
	}
	
	public Class<? extends AopSeriverInterface> get_seriveCls() {
		return _seriveCls;
	}

	public void set_seriveCls(Class<? extends AopSeriverInterface> _seriveCls) {
		this._seriveCls = _seriveCls;
	}
	
	public Method Method(String funcName)
	{
		Method method=null;
	    Method[] methods= _cls.getMethods();
	    for(Method m:methods)
	    {
	    	if(m.getName().equals(funcName)){
	    		method=m;
	    	}
	    }
	    return method;
	}
	

	
	public Object run(Function<Object[],Object> func,AopMess mess,Object... pars) 
			throws InstantiationException, IllegalAccessException
	{
		Object res=null;
	    AopSeriverInterface servie= this._seriveCls.newInstance();
	    if(servie!=null)
	    {
		    servie.beforeRun(mess, pars);
		    try
		    {
		    	res=func.apply(pars);
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
	    	res=func.apply(pars);
	    }
	    return res;
	}
	
	public Object run(String method,AopMess mess,Object... pars)
			throws IllegalAccessException, IllegalArgumentException, InvocationTargetException, InstantiationException
	{
		if(this._seriveCls==null){
			return this.runByAttribute(this.Method(method), mess, pars);
		}
		else
			return this.runBySerive(this.Method(method), mess, pars);
	}
	
	public Object run(Method method,AopMess mess,Object... pars) 
			throws IllegalAccessException, IllegalArgumentException, InvocationTargetException, InstantiationException
	{
		if(this._seriveCls==null){
			return this.runByAttribute(method, mess, pars);
		}
		else
			return this.runBySerive(method, mess, pars);
	}
	
	public Object runByAttribute(Method method,AopMess mess,Object... pars) 
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
		    	res=method.invoke(_obj, pars);
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
	
	public Object runBySerive(Method method,AopMess mess,Object... pars) 
			throws IllegalAccessException, IllegalArgumentException, InvocationTargetException, InstantiationException
	{
		Object res=null;
	    AopSeriverInterface servie= this._seriveCls.newInstance();
	    if(servie!=null)
	    {
		    servie.beforeRun(mess, pars);
		    try
		    {
		    	res=method.invoke(_obj, pars);
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
