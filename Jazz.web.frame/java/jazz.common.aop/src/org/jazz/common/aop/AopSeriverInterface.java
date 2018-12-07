package org.jazz.common.aop;

public interface AopSeriverInterface {
	
	void beforeRun(AopMess mess,Object... pars);
	
	void successRun(AopMess mess,Object res);
	
	void errorRun(AopMess mess,Exception ex);
	
	void afterRun(AopMess mess);
}
