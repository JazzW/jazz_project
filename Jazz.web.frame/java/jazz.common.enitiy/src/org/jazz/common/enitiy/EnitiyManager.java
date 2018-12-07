package org.jazz.common.enitiy;


import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.jazz.common.enitiy.attribute.DBItemAttribute;
import org.jazz.common.enitiy.attribute.DBModelAttribute;
import org.jazz.common.enitiy.attribute.KeyAttribute;
import org.jazz.common.enitiy.attribute.SearchKeyAttribute;

/**
 * ʵ��ת��������
 * @author wjc
 *
 */
public  class EnitiyManager {
	/**
	 * ����ʵ�建���{@link EnitiyManagerByCache}
	 */
	public static boolean useCache=false;
	
	/**
	 * ���ʵ������ı�����֣�ʹ�ù���ע�� {@link DBModelAttribute}
	 * @param cls ʵ����
	 * @return {@code String} �������
	 * @throws InstantiationException
	 * @throws IllegalAccessException
	 */
    public  static <TEnity> String TBName(Class<TEnity> cls) 
    		throws InstantiationException, IllegalAccessException
    {
    	if(useCache)
    		return EnitiyManagerByCache.TBName(cls);
    	
    	DBModelAttribute atr= cls.getAnnotation(DBModelAttribute.class);
    	if(atr!=null)
    		return atr.TBName();
    	else
    		return "";
    }

    /**
     * ���ʵ��ȫ������ʱʹ���ֶΣ�ʹ�ù���ע�� {@link DBModelAttribute}
     * @param cls ʵ����
     * @return {@code String[]} �ֶ����� 
     * @throws InstantiationException
     * @throws IllegalAccessException
     */
    public static <TEnity> String[] SimpleCol(Class<TEnity> cls)
    		throws InstantiationException, IllegalAccessException
    {
    	if(useCache)
    		return EnitiyManagerByCache.SimpleCol(cls);
    	
    	List<String> res=new ArrayList<String>();
    	for(Field f:	cls.getDeclaredFields()){
    	  
    		SearchKeyAttribute atr= f.getAnnotation(SearchKeyAttribute.class);
    		if(atr!=null)
    		{
    			DBItemAttribute _atr=f.getAnnotation(DBItemAttribute.class);
    			if(_atr!=null)
    				res.add(_atr.ColName());
    		}
    	}
    	return (String[]) res.toArray();

    }

    /**
     * ���ʵ�嵥������ʱʹ���ֶΣ�ʹ�ù���ע�� {@link DBModelAttribute}
     * @param cls
     * @return {@code String} �ֶ�����
     * @throws InstantiationException
     * @throws IllegalAccessException
     */
    public static <TEnity> String GetItemCols(Class<TEnity> cls) 
    		throws InstantiationException, IllegalAccessException
    {
    	if(useCache)
    		return EnitiyManagerByCache.GetItemCols(cls);

    	DBModelAttribute atr= cls.getAnnotation(DBModelAttribute.class);
    	if(atr!=null)
    		return atr.ItemCols();
    	else
    		return "";

    }

    /**
     * ���ʵ��������ʱʹ���ֶΣ�ʹ�ù���ע�� {@link DBModelAttribute}
     * @param cls
     * @return {@code String} �ֶ�����
     * @throws InstantiationException
     * @throws IllegalAccessException
     */
    public static <TEnity> String GetListCols(Class<TEnity> cls) 
    		throws InstantiationException, IllegalAccessException
    {
    	if(useCache)
    		return EnitiyManagerByCache.GetListCols(cls);
    	DBModelAttribute atr= cls.getAnnotation(DBModelAttribute.class);
    	if(atr!=null)
    		return atr.ListCols();
    	else
    		return "";

    }
    
    /**
     * ����ֶζ�Ӧ����е�������ʹ�ù���ע�� {@link DBItemAttribute}
     * @param Name �ֶ�����
     * @param cls	
     * @return ����
     * @throws InstantiationException
     * @throws IllegalAccessException
     */
    public static <TEnity> String GetCol(String Name,Class<TEnity> cls) 
    		throws InstantiationException, IllegalAccessException
    {
    	if(useCache)
    		return EnitiyManagerByCache.GetCol(Name, cls);
    	for(Field f:	cls.getDeclaredFields()){    	 
			if(f.getName().equals(Name)){
				DBItemAttribute _atr=f.getAnnotation(DBItemAttribute.class);
				if(_atr!=null)
					return _atr.ColName();
				else
					return Name;
			}
    						
    	}
    	return Name;
    }
    
    /**
     * ��ͼ�ϵ��ֶ��Լ��ֶ�ֵת��ʵ��
     * @param map
     * @param cls
     * @return
     */
    public static <TEnity> TEnity toTEnity(Map<String,Object> map,Class<TEnity> cls)
    {
    	if(useCache)
    		return EnitiyManagerByCache.toTEnity(map, cls);
    	TEnity res = null;
		try {
			res = cls.newInstance();
			for(Field f:	cls.getDeclaredFields()){
				f.setAccessible(true);
				f.set(res, map.get(EnitiyManager.GetCol( f.getName(), cls)));
			}			
		} catch (InstantiationException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		} catch (IllegalAccessException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		}
    	
        return res;
    }
    
    /**
     * ��ʵ��ת���ֵ����͵��࣬�����ݲ���������������
     * @param pars ����
     * @param model ʵ��
     * @return ʵ���ֵ�
     * @throws IllegalArgumentException
     * @throws IllegalAccessException
     * @throws InstantiationException
     * @throws SecurityException
     */
    public static <TEnity> Map<String,Object> toDic(Map<String,Object> pars,TEnity model)
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
    	if(useCache)
    		return EnitiyManagerByCache.toDic(pars, model);
    	Class<?> cls=model.getClass();
    	Map<String,Object> res=new HashMap<String,Object>();
    	for(Field f:cls.getDeclaredFields()){
    		f.setAccessible(true);
    		DBItemAttribute _atr=f.getAnnotation(DBItemAttribute.class);
			if(_atr!=null){
				res.put(_atr.ColName(), "?"+_atr.ColName());
				pars.put(_atr.ColName(), f.get(model));
			}
			else
			{
				res.put(f.getName(), "?"+f.getName());
				pars.put(f.getName(), f.get(model));
			}
    	}
    	
    	return res;
    }
    
    /**
     * �����ʵ��ת���ֵ����͵��࣬�����ݲ���������������
     * @param pars ����
     * @param models ���ʵ��
     * @return ʵ���ֵ�
     * @throws IllegalArgumentException
     * @throws IllegalAccessException
     * @throws InstantiationException
     * @throws SecurityException
     */
	@SafeVarargs
	public static <TEnity> List<Map<String, Object>> toDic(Map<String,Object> pars,TEnity... models) 
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
    	if(useCache)
    		return EnitiyManagerByCache.toDic(pars, models);
      	Class<?> cls=models[0].getClass();
    	List<Map<String,Object>> result=new ArrayList<Map<String,Object>>();
    	int index=1;

		for(TEnity model : models){
			Map<String,Object> res=new HashMap<String,Object>();
	    	for(Field f:cls.getDeclaredFields()){
	    		f.setAccessible(true);
	    		DBItemAttribute _atr=f.getAnnotation(DBItemAttribute.class);
				if(_atr!=null){
					res.put(_atr.ColName(), "?"+_atr.ColName()+index);
					pars.put(_atr.ColName()+index, f.get(model));
				}
				else
				{
					res.put(f.getName(), "?"+f.getName()+index);
					pars.put(f.getName()+index, f.get(model));
				}
				
	    	}
	    	index++;
	    	result.add(res);
    	}
    	return result;
    }
    
	/**
	 * ���ʵ����Ӧ������������ע��{@link KeyAttribute}
	 * @param pars ����
	 * @param model ʵ��
	 * @return ����ͼ
	 * @throws IllegalArgumentException
	 * @throws IllegalAccessException
	 * @throws InstantiationException
	 * @throws SecurityException
	 */
    public static <TEnity> Map<String,Object> getKeys(Map<String,Object> pars,TEnity model) 
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
    	if(useCache)
    		return EnitiyManagerByCache.getKeys(pars, model);
      	Class<?> cls=model.getClass();
    	Map<String,Object> res=new HashMap<String,Object>();
    	for(Field f:cls.getDeclaredFields()){
    		f.setAccessible(true);
    		KeyAttribute _atr=f.getAnnotation(KeyAttribute.class);
			if(_atr!=null){
				DBItemAttribute __atr=f.getAnnotation(DBItemAttribute.class);
				if(__atr!=null){
					res.put(__atr.ColName(), "?"+__atr.ColName());
					pars.put(__atr.ColName(), f.get(model));
				}
				else
				{
					res.put(f.getName(), "?"+f.getName());
					pars.put(f.getName(), f.get(model));
				}				
			}
    	}    	
    	return res;

    }
    
    /**
     * ���ɲ��ظ���ʵ����Ӧ������������ע��{@link KeyAttribute}
     * @param pars
     * @param model
     * @param sindex
     * @return
     * @throws IllegalArgumentException
     * @throws IllegalAccessException
     * @throws InstantiationException
     * @throws SecurityException
     */
    public static <TEnity> Map<String,Object> getKeys(Map<String,Object> pars,TEnity model,int sindex) 
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
    	if(useCache)
    		return EnitiyManagerByCache.getKeys(pars, model,sindex);
      	Class<?> cls=model.getClass();
    	Map<String,Object> res=new HashMap<String,Object>();
    	for(Field f:cls.getDeclaredFields()){
    		f.setAccessible(true);
    		KeyAttribute _atr=f.getAnnotation(KeyAttribute.class);
			if(_atr!=null){
				DBItemAttribute __atr=f.getAnnotation(DBItemAttribute.class);
				if(__atr!=null){
					res.put(__atr.ColName(), "?"+__atr.ColName()+sindex);
					pars.put(__atr.ColName()+sindex, f.get(model));
				}
				else
				{
					res.put(f.getName(), "?"+f.getName()+sindex);
					pars.put(f.getName()+sindex, f.get(model));
				}				
			}
    	}    	
    	return res;

    }
    /**
     * ��Ӧ�Ĳ���ͼת��sql���
     * @param pars
     * @return
     */
    public static String toWhereSql(Map<String,Object> pars)
    {
    	if(useCache)
    		return EnitiyManagerByCache.toWhereSql(pars);
        StringBuffer sqlstring=new StringBuffer("");

        for(String key :pars.keySet()){
        	  if(sqlstring.length()==0)
        		  sqlstring.append(String.format(" %s = %s ",key,pars.get(key).toString()));
              else
            	  sqlstring.append(String.format(" and %s= %s ",key,pars.get(key).toString()));
        }
        
        return sqlstring.toString();

    }
    
    /**
     * ��Ӧ��ʵ��ת��sql��䣬�����ɲ���ͼ
     * @param pars ����ͼ
     * @param models ���ʵ��
     * @return sql���
     * @throws IllegalArgumentException
     * @throws IllegalAccessException
     * @throws InstantiationException
     * @throws SecurityException
     */
    @SafeVarargs
	public static <TEnity> String toWhereSql(Map<String,Object> pars,TEnity...  models)
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
    	if(useCache)
    		return EnitiyManagerByCache.toWhereSql(pars,models);
    	StringBuffer sql=new StringBuffer("");
    	int index=0;
    	for(TEnity model:models){
    	    index++;
    		Map<String,Object> res=EnitiyManager.getKeys(pars, model,index);
        	StringBuffer sqlstring=new StringBuffer("");
    	    for(String key :res.keySet()){
          	  if(sqlstring.length()==0)
          		  sqlstring.append(String.format(" %s= %s ",key,res.get(key).toString()));
	            else
	          	  sqlstring.append(String.format(" and %s = %s ",key,res.get(key).toString()));
    	    }
    	    if(sql.length()==0)
    	    	sql.append(" ("+sqlstring.toString()+") ");
    	    else
    	    	sql.append(" or ("+sqlstring.toString()+") ");
    	}
    	
    	return sql.toString();

    }
}
