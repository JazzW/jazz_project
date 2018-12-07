package org.jazz.common.async;

import java.lang.reflect.Method;
import java.util.concurrent.Callable;

public class AsyncTask<T> implements Callable<T>{

	private Method _method;
	
	private Object[] _pars;
	
	private Object _target;
	
	public AsyncTask(Method method,Object target,Object... pars)
	{
		super();
		this._method=method;
		this._pars=pars;
		this._target=target;
		
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public T call() throws Exception {
		// TODO Auto-generated method stub
		
		return  (T)this._method.invoke(_target, _pars);
	}

}
