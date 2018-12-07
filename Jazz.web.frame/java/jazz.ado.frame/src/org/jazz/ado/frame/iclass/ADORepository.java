package org.jazz.ado.frame.iclass;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.jazz.common.enitiy.EnitiyManager;
import org.jazz.common.web.config.TableConfig;
import org.jazz.helper.database.common.IDataBase;
import org.jazz.helper.database.common.SQLCmdHelper;

/**
 * {@code IRepository}��adoʵ�ֳ����࣬��ɻ������ݷ��ʲ����
 * @author Wangjc 
 * @author 2018.10.10
 * @param <T> ���ݿ�ӳ���class��
 */
public abstract class ADORepository<T> implements IRepository<T>  {

	/**
	 * ���ݿ�ʵ�壬{@see IDataBase}
	 */
	private IDataBase Instance;
	
	/**
	 * ���ݿ�ӳ���{@code Class}��
	 */
	private Class<T> cls;
	
	/**
	 * ������ݿ�ʵ��{@code IDataBase}ʵ����
	 * @return {@code IDataBase}
	 */
	public IDataBase getInstance()
	{
		return this.Instance;
	}
	
	/**
	 * �������ݿ�ʵ�壨����������ʹ�ã�
	 * @param Ins ���ݿ�ʵ��{@code IDataBase}ʵ����
	 */
	public void setInstance(IDataBase Ins)
	{
		this.Instance=Ins;
	}
	
	/**
	 * ��� ���ݿ�ӳ���{@code Class}��
	 * @return {@code Class<?>} cls
	 */
	public Class<T> getcls()
	{
		return this.cls;
	}
	
	/**
	 * ���� ���ݿ�ӳ���{@code Class}�ࣨ����������ʹ�ã�
	 * @param cls ���ݿ�ӳ���{@code Class}��
	 */
	public void setcls(Class<T> cls)
	{
		this.cls=cls;
	}
	

	@Override
	public List<T> ISelectList(TableConfig Config) throws Exception{
		// TODO �Զ����ɵķ������
		HashMap<String,Object> pars=new HashMap<String,Object>();
		
		String sql=SQLCmdHelper.SelectFrame
				(" top "+(Config.Length*Config.Page)+ EnitiyManager.GetListCols(cls), EnitiyManager.TBName(cls), Config.toSqlCmdExp(pars), Config.toOrderSql());
		sql=SQLCmdHelper.PageFrame(sql, Config.Length, Config.Page);
		
		List<HashMap<String,Object>> res= this.Instance.ExecuteDataTable(sql, pars);
		if(res!=null){
			List<T> _res=new ArrayList<T>();
			for(HashMap<String, Object> map : res){
				_res.add(EnitiyManager.toTEnity(map, cls));
			}
			return _res;
		}
		return null;
	}

	@Override
	public int ISelectListCount(TableConfig Config)throws Exception {
		// TODO �Զ����ɵķ������
		HashMap<String,Object> pars=new HashMap<String,Object>();
		
		String sql=SQLCmdHelper.SelectFrame
				(" * ", EnitiyManager.TBName(cls), Config.toSqlCmdExp(pars), "");
		sql=SQLCmdHelper.CountFrame(sql);

		return this.Instance.ExecuteScalar(sql, pars);
		
	}

	@Override
	public T ISelectFirst(TableConfig Config)throws Exception {
		HashMap<String,Object> pars=new HashMap<String,Object>();
		
		String sql=SQLCmdHelper.SelectFrame
				(" top 1 "+ EnitiyManager.GetListCols(cls), EnitiyManager.TBName(cls), Config.toSqlCmdExp(pars), Config.toOrderSql());
		
		List<HashMap<String,Object>> res= this.Instance.ExecuteDataTable(sql, pars);
		if(res!=null){

			return EnitiyManager.toTEnity(res.get(0), cls);
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean IInsert(T... models)  throws Exception
	{
		HashMap<String,Object> pars=new HashMap<String,Object>();
		try {
			String sql=SQLCmdHelper.InsertListFrame(EnitiyManager.TBName(cls), EnitiyManager.toDic(pars, models));
			
			return this.Instance.ExecuteNonQuery(sql,pars,null)>0?true :false;
			
		} catch (IllegalArgumentException | IllegalAccessException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean IUpdate(T... models) throws Exception {
		// TODO �Զ����ɵķ������
		HashMap<String,Object> pars=new HashMap<String,Object>();
		try
		{
		Map<String,Object> keys=EnitiyManager.getKeys(new HashMap<String,Object>(), models[0]);
		Map<String,Object> dict=EnitiyManager.toDic(pars, models[0]);
		for(String key :keys.keySet())
		{
			dict.remove(key);
		}
		String sql=SQLCmdHelper.UpdateFrame(EnitiyManager.TBName(cls), dict,EnitiyManager.toWhereSql(keys));
				
		return this.Instance.ExecuteNonQuery(sql,pars,null)>0?true :false;
		}
		catch (IllegalArgumentException | IllegalAccessException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		}
		return false;
		
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean IDelete(T... models) throws Exception{
		// TODO �Զ����ɵķ������
		HashMap<String,Object> pars=new HashMap<String,Object>();
		try
		{
		
			String sql=SQLCmdHelper.DeleteFrame(EnitiyManager.TBName(cls), EnitiyManager.toWhereSql(pars,models));
				
			return this.Instance.ExecuteNonQuery(sql,pars,null)>0?true :false;
		}
		catch (IllegalArgumentException | IllegalAccessException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		}
		return false;
	}

}
