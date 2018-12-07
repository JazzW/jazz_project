package org.jazz.common.enitiy;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;

/**
 * 使用实体缓存池{@link EnityCachePool}进行实体操做
 * @author Wangjc
 * @author 2018.10.12
 */
class EnitiyManagerByCache {
	/**
	 * 通过缓存池获得关于实体的表格民称
	 * @param cls 实体类
	 * @return {@code String} 表格名字
	 * @throws InstantiationException
	 * @throws IllegalAccessException
	 */
	public  static <TEnity> String TBName(Class<TEnity> cls) 
			 throws InstantiationException, IllegalAccessException
    {
	  return EnityCachePool.getCache(cls).getTBName();
    }

    public static <TEnity> String[] SimpleCol(Class<TEnity> cls) 
    		throws InstantiationException, IllegalAccessException
    {
    	  return EnityCachePool.getCache(cls).getSimpleCol();

    }

    public static <TEnity> String GetItemCols(Class<TEnity> cls) 
    		throws InstantiationException, IllegalAccessException
    {

    	return EnityCachePool.getCache(cls).getItemCols();

    }

    public static <TEnity> String GetListCols(Class<TEnity> cls) 
    		throws InstantiationException, IllegalAccessException
    {

    	return	EnityCachePool.getCache(cls).getListCols();

    }
    
    public static <TEnity> String GetCol(String Name,Class<TEnity> cls)
    		throws InstantiationException, IllegalAccessException
    {
    	return EnityCachePool.getCache(cls).getColName(Name);
    }
	    
    public static <TEnity> TEnity toTEnity(Map<String,Object> map,Class<TEnity> cls)
    {
    	TEnity res = null;
		try {
			res = cls.newInstance();
			for(Entry<String, Field> f:	EnityCachePool.getCache(cls).getfieldMap().entrySet()){
				f.getValue().setAccessible(true);
				f.getValue().set(res,map.get( f.getKey()));
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
	    
    public static <TEnity> Map<String,Object> toDic(Map<String,Object> pars,TEnity model)
    		throws IllegalArgumentException, IllegalAccessException,
    		InstantiationException, SecurityException
    {
    	Class<?> cls=model.getClass();
    	Map<String,Object> res=new HashMap<String,Object>();
    	for(Entry<String, Field> f:	EnityCachePool.getCache(cls).getfieldMap().entrySet()){
    		f.getValue().setAccessible(true);
			res.put(f.getKey(), "?"+f.getKey());
			pars.put(f.getKey(), f.getValue().get(model));	
    	}
    	
    	return res;
    }
	    
	@SafeVarargs
	public static <TEnity> List<Map<String, Object>> toDic(Map<String,Object> pars,TEnity... models) 
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
      	Class<?> cls=models[0].getClass();
    	List<Map<String,Object>> result=new ArrayList<Map<String,Object>>();
    	int index=1;

		for(TEnity model : models){
			Map<String,Object> res=new HashMap<String,Object>();
	    	for(Entry<String, Field> f:	EnityCachePool.getCache(cls).getfieldMap().entrySet()){
	    		f.getValue().setAccessible(true);
				res.put(f.getKey(), "?"+f.getKey()+index);
				pars.put(f.getKey()+index, f.getValue().get(model));	
	    	}
	    	index++;
	    	result.add(res);
    	}
    	return result;
    }
	    
    public static <TEnity> Map<String,Object> getKeys(Map<String,Object> pars,TEnity model) 
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
  	
      	Class<?> cls=model.getClass();
    	Map<String,Object> res=new HashMap<String,Object>();
      	for(Entry<String, Field> f:	EnityCachePool.getCache(cls).getfieldMap().entrySet()){
    		f.getValue().setAccessible(true);
			res.put(f.getKey(), "?"+f.getKey());
			pars.put(f.getKey(), f.getValue().get(model));	
    	}   	
    	return res;

    }
    
    public static <TEnity> Map<String,Object> getKeys(Map<String,Object> pars,TEnity model,int sindex) 
    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
    {
  	
      	Class<?> cls=model.getClass();
    	Map<String,Object> res=new HashMap<String,Object>();
      	for(Entry<String, Field> f:	EnityCachePool.getCache(cls).getfieldMap().entrySet()){
    		f.getValue().setAccessible(true);
			res.put(f.getKey(), "?"+f.getKey()+sindex);
			pars.put(f.getKey()+sindex, f.getValue().get(model));	
    	}	
    	return res;

    }
    
    public static String toWhereSql(Map<String,Object> pars)
	    {
	        StringBuffer sqlstring=new StringBuffer("");

	        for(String key :pars.keySet()){
	        	  if(sqlstring.length()==0)
	        		  sqlstring.append(String.format(" %s = %s ",key,pars.get(key).toString()));
	              else
	            	  sqlstring.append(String.format(" and %s= %s ",key,pars.get(key).toString()));
	        }
	        
	        return sqlstring.toString();

	    }
	    
	    @SafeVarargs
	public static <TEnity> String toWhereSql(Map<String,Object> pars,TEnity...  models)
	    		throws IllegalArgumentException, IllegalAccessException, InstantiationException, SecurityException
	    {
	    	StringBuffer sql=new StringBuffer("");
	    	int index=0;
	    	for(TEnity model:models){
	    	    index++;
	    		Map<String,Object> res=EnitiyManagerByCache.getKeys(pars, model,index);
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
