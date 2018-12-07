using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jazz.Common.Enitiy
{
    public  class EnitiyCache
    {
        private Type T;
	
	    private String TBName;
	
	    private String[] SimpleCol;
	
	    private String ItemCols;
	
	    private String ListCols;

        private Dictionary<String, PropertyInfo> fieldMap = new Dictionary<String, PropertyInfo>();

        public Dictionary<String, PropertyInfo> getfieldMap()
	    {
		    return fieldMap;
	    }

        public PropertyInfo getFieldName(String Colname)
	    {
		    return this.fieldMap[Colname];
	    }
	
	    public String getColName(String Fieldname)
	    {
		    foreach (var entry  in fieldMap) {
                if(entry.Value.Name==Fieldname)
                    return entry.Key;
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


	    public Type getCls() {
		    return T;
	    }

	
	    public void setCls<Enity>()  {
		    this.T=typeof(Enity);
		    this.fieldMap.Clear();
		    foreach(var f in T.GetProperties()){
			     IAttribute.DBItemAttribute _atr=f.GetCustomAttribute<IAttribute.DBItemAttribute>();
			    if(_atr!=null){
				    this.fieldMap.Add(_atr.ColName,f);
			    }
			    else{
				    this.fieldMap.Add(f.Name, f);
			    }
			
		    }
		    IAttribute. DBModelAttribute atr= T.GetCustomAttribute<IAttribute. DBModelAttribute>();
    	    if(atr!=null)
    	    {
    		    this.ItemCols=atr.ItemCols;
    		    this.ListCols=atr.ListCols;
    		    this.TBName=atr.TBName;
    	    }
    	
	    }
	
	    public EnitiyCache()
	    {
	    }
    }
}
