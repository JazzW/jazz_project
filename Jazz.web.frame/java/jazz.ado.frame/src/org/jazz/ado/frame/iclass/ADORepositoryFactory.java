package org.jazz.ado.frame.iclass;

import org.jazz.helper.database.common.IDataBase;

/**
 * ado数据访问数据{@code IRepository<T>}工厂
 * @author Wangjc
 * @author 2018.10.10
 */
public class ADORepositoryFactory {
	/**
	 * 获得{@link ADORepository<Enitiy>}的实例
	 * @param cls 映射实体的{@ Class<?>}
	 * @param dbc 数据库类的{@ Class<?>}
	 * @return {@code ADORepository<Enitiy>}实例
	 */
	public static <Enitiy,DB extends IDataBase> ADORepository<Enitiy> getADORepository(Class<Enitiy> cls,Class<DB> dbc)
	{
		ARepository<Enitiy> ado= new ARepository<Enitiy>();
		try {
			ado.setInstance(dbc.newInstance());
		} catch (InstantiationException e) {
			// TODO 自动生成的 catch 块
			e.printStackTrace();
		} catch (IllegalAccessException e) {
			// TODO 自动生成的 catch 块
			e.printStackTrace();
		}
		ado.setcls(cls);
		return ado;
	}
}
