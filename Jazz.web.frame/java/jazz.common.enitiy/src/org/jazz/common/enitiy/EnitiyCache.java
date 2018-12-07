package org.jazz.common.enitiy;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.Map;

import org.jazz.common.enitiy.attribute.DBItemAttribute;
import org.jazz.common.enitiy.attribute.DBModelAttribute;

/**
 * 实体缓存子项
 * @author wjc
 *
 */
public class EnitiyCache {
	
	private Class<?> cls;
	
	private String TBName;
	
	private String[] SimpleCol;
	
	private String ItemCols;
	
	private String ListCols;
	
	private Map<String,Field> fieldMap = new HashMap<String,Field>();
					
	public Map<String,Field> getfieldMap()
	{
		return fieldMap;
	}
	
	public Field getFieldName(String Colname)
	{
		return this.fieldMap.get(Colname);
	}
	
	public String getColName(String Fieldname)
	{
		for (Map.Entry<String,Field> entry :fieldMap.entrySet()) {
		    if(entry.getValue().getName().equals(Fieldname))
		    	return entry.getKey();
		}
		return "";

	}

	public String getTBName() {
		return TBName;
	}

	public String[] getSimpleCol() {
		return SimpleCol;
	}

	public String getItemCols() {
		return ItemCols;
	}

	public String getListCols() {
		return ListCols;
	}


	public Class<?> getCls() {
		return cls;
	}

	
	public void setCls(Class<?> cls)  {
		this.cls = cls;
		this.fieldMap.clear();
		for(Field f:cls.getDeclaredFields()){
			DBItemAttribute _atr=f.getAnnotation(DBItemAttribute.class);
			if(_atr!=null){
				this.fieldMap.put(_atr.ColName(), f);
			}
			else{
				this.fieldMap.put(f.getName(), f);
			}
			
		}
		DBModelAttribute atr= cls.getAnnotation(DBModelAttribute.class);
    	if(atr!=null)
    	{
    		this.ItemCols=atr.ItemCols();
    		this.ListCols=atr.ListCols();
    		this.TBName=atr.TBName();
    	}
    	
//    	List<String> res=new ArrayList<String>();
//    	for(Field f:	cls.getDeclaredFields()){
//    	  
//    		SearchKeyAttribute _atr= f.getAnnotation(SearchKeyAttribute.class);
//    		if(atr!=null)
//    		{
//    			DBItemAttribute __atr=f.getAnnotation(DBItemAttribute.class);
//    			if(_atr!=null)
//    				res.add(__atr.ColName());
//    		}
//    	}
//    	this.SimpleCol= (String[]) res.toArray();
	}
	
	public EnitiyCache()
	{
	}

	public EnitiyCache(Class<?> cls) 
	{
		this.setCls(cls);
	}
}
