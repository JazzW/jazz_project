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
 * 实体转化操做类
 * @author wjc
 *
 */
public  class EnitiyManager {
	/**
	 * 启用实体缓存池{@link EnitiyManagerByCache}
	 */
	public static boolean useCache=false;
	
	/**
	 * 获得实体关联的表格名字，使用关联注解 {@link DBModelAttribute}
	 * @param cls 实体类
	 * @return {@code String} 表格名字
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
     * 获得实体全局搜索时使用字段，使用关联注解 {@link DBModelAttribute}
     * @param cls 实体类
     * @return {@code String[]} 字段名字 
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
     * 获得实体单个搜索时使用字段，使用关联注解 {@link DBModelAttribute}
     * @param cls
     * @return {@code String} 字段名字
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
     * 获得实体多个搜索时使用字段，使用关联注解 {@link DBModelAttribute}
     * @param cls
     * @return {@code String} 字段名字
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
     * 获得字段对应表格中的列名，使用关联注解 {@link DBItemAttribute}
     * @param Name 字段名字
     * @param cls	
     * @return 列名
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
     * 将图上的字段以及字段值转成实体
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
			// TODO 自动生成的 catch 块
			e.printStackTrace();
		} catch (IllegalAccessException e) {
			// TODO 自动生成的 catch 块
			e.printStackTrace();
		}
    	
        return res;
    }
    
    /**
     * 将实体转成字典类型的类，并根据参数规则新增参数
     * @param pars 参数
     * @param model 实体
     * @return 实体字典
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
     * 将多个实体转成字典类型的类，并根据参数规则新增参数
     * @param pars 参数
     * @param models 多个实体
     * @return 实体字典
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
	 * 获得实体相应的主键，关联注解{@link KeyAttribute}
	 * @param pars 参数
	 * @param model 实体
	 * @return 参数图
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
     * 生成不重复的实体相应的主键，关联注解{@link KeyAttribute}
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
     * 相应的参数图转成sql语句
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
     * 相应的实体转成sql语句，并生成参数图
     * @param pars 参数图
     * @param models 多个实体
     * @return sql语句
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
