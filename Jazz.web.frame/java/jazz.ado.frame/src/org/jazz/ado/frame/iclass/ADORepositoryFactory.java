package org.jazz.ado.frame.iclass;

import org.jazz.helper.database.common.IDataBase;

/**
 * ado���ݷ�������{@code IRepository<T>}����
 * @author Wangjc
 * @author 2018.10.10
 */
public class ADORepositoryFactory {
	/**
	 * ���{@link ADORepository<Enitiy>}��ʵ��
	 * @param cls ӳ��ʵ���{@ Class<?>}
	 * @param dbc ���ݿ����{@ Class<?>}
	 * @return {@code ADORepository<Enitiy>}ʵ��
	 */
	public static <Enitiy,DB extends IDataBase> ADORepository<Enitiy> getADORepository(Class<Enitiy> cls,Class<DB> dbc)
	{
		ARepository<Enitiy> ado= new ARepository<Enitiy>();
		try {
			ado.setInstance(dbc.newInstance());
		} catch (InstantiationException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		} catch (IllegalAccessException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		}
		ado.setcls(cls);
		return ado;
	}
}
