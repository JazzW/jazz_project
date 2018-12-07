

import org.jazz.common.aop.AopMess;
import org.jazz.common.aop.AopSeriverInterface;

public class funcMess implements AopSeriverInterface {

	@Override
	public void beforeRun(AopMess mess, Object... pars) {
		// TODO Auto-generated method stub
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		System.out.println("before run");
	}

	@Override
	public void successRun(AopMess mess, Object res) {
		// TODO Auto-generated method stub
		System.out.println("succeed run");	
	}

	@Override
	public void errorRun(AopMess mess, Exception ex) {
		// TODO Auto-generated method stub
		System.out.println("error run");
	}

	@Override
	public void afterRun(AopMess mess) {
		// TODO Auto-generated method stub
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		System.out.println("after run");
	}

}
