import java.lang.reflect.InvocationTargetException;
import java.util.Date;
import java.util.concurrent.ExecutionException;

import org.jazz.ado.frame.config.ADODBConfig;
import org.jazz.ado.frame.iclass.ADORepositoryFactory;
import org.jazz.ado.frame.iclass.IRepository;
import org.jazz.common.aop.AopProxy;
import org.jazz.common.aop.AopSerive;
import org.jazz.common.async.AsyncSeriver;
import org.jazz.common.ienum.CompareSymbol;
import org.jazz.common.ienum.LinkEnum;
import org.jazz.common.web.config.Filiter;
import org.jazz.common.web.config.SelectConfig;
import org.jazz.common.web.config.TableConfig;
import org.jazz.helper.database.sql.MsSql;


public class run {

	public static void main(String[] args) 
			throws Exception {
		// TODO 自动生成的方法存根
		
		//AopProxy<mains> MainProxy=AopSerive.createProxy(mains.class,funcMess.class, null);
		//MainProxy.run(MainProxy.Method("run"), null, "run");

		ADODBConfig.init("C:/Users/wjc/Workspaces/MyEclipse 2017 CI/jazz.main/src/jdbc.properties");
		Filiter<String> f1=new Filiter<String>("Id",1,CompareSymbol.greater); 	
    	Filiter<String> f2=new Filiter<String>("Id",0,CompareSymbol.greater);
	
    	SelectConfig config=new SelectConfig(f1,f2);
    	config.Link=LinkEnum.or;

    	TableConfig tconfigs=new TableConfig(config);
    	tconfigs.OrderCol="Id";
    	tconfigs.Page=1;
    	tconfigs.Length=10;
    	
		IRepository<Test> ado=ADORepositoryFactory.getADORepository(Test.class,MsSql.class);
		
		//@SuppressWarnings("rawtypes")
		//AopProxy<IRepository> adoProxy= AopSerive.createProxy(IRepository.class,funcMess.class, ado);	
		//List<Test> res= (List<Test>) adoProxy.run("ISelectList", null, tconfigs) ;
		//adoProxy.run((configs)->ado.ISelectListCount((TableConfig)configs[0]),null,tconfigs);
		
		IRepository<Test> adoProxy2=AopSerive.createProxy2(ado, funcMess.class,IRepository.class);
		ado.ISelectListCount(tconfigs);
		Date ds=new Date();
		
		adoProxy2.ISelectListCount(tconfigs);
		adoProxy2.ISelectListCount(tconfigs);
		
		System.out.println((new Date().getTime()-ds.getTime()));
		ds=new Date();
		AsyncSeriver.runAsync(
			()->adoProxy2.ISelectListCount(tconfigs),
			()->adoProxy2.ISelectListCount(tconfigs)
				);
		System.out.println((new Date().getTime()-ds.getTime()));
		
		//ado.ISelectList(tconfigs);
		//System.out.println(ado.ISelectListCount(tconfigs));
		//for(Test r :res){
		//	System.out.println(r.getId()+" "+r.getName()+" "+r.getMess());
		//}
	}
	

}
