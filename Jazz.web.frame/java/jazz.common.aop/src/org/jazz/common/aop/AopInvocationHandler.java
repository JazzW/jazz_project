package org.jazz.common.aop;

import java.lang.reflect.InvocationHandler;
import java.lang.reflect.Method;

public class AopInvocationHandler implements InvocationHandler {

	private Object target;
	
	private Class<? extends AopSeriverInterface> seriveCls;
	
	public AopInvocationHandler()
	{
		super();
	}
	
	public AopInvocationHandler(Object obj)
	{
		super();
		this.setTarget(obj);
	}
	
	public AopInvocationHandler(Object obj,Class<? extends AopSeriverInterface> seriveCls)
	{
		super();
		this.setTarget(obj);
		this.setSeriveCls(seriveCls);
	}
	
	public Object getTarget() {
		return target;
	}

	public void setTarget(Object target) {
		this.target = target;
	
	}
	
	public Class<? extends AopSeriverInterface> getSeriveCls() {
		return seriveCls;
	}

	public void setSeriveCls(Class<? extends AopSeriverInterface> seriveCls) {
		this.seriveCls = seriveCls;
	}
	
	@Override
	public Object invoke(Object proxy, Method method, Object[] args) throws Throwable {
		// TODO Auto-generated method stub
		
		Object res=null;
		AopAttribute atr= method.getAnnotation(AopAttribute.class); 
	    AopSeriverInterface servie=null;
	    AopMess mess=new AopMess();

	    mess.setMethodName(method.getName());
	    if(atr!=null)
    	{
    		servie= atr.serive().newInstance();
    	    mess.setMethodDesc(atr.desc());
    	}
	    else
	    	servie= this.seriveCls.newInstance();
	    if(servie!=null)
	    {
		    servie.beforeRun(mess,args);
		    try
		    {
		    	res=method.invoke(target, args);
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
	    	res=method.invoke(target, args);
	    }
	    return res;
	}



}


