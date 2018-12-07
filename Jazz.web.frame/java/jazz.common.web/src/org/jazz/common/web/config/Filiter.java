package org.jazz.common.web.config;

import java.util.Map;

import org.jazz.common.ienum.CompareSymbol;

public class Filiter<S> extends IFiliter {
	
	public Filiter()
	{

	}
	
	public Filiter(String col,Object val,CompareSymbol symbol)
	{
		this.ColName=col;
		this.Val=val;
		this.Symbol=symbol;
	}
	
	public  String toSqlCmdExp(Map<String,Object> pars)
    {
        
    	 String sql = "";
    	 String parname="par_"+(pars.size()+1);
    	 pars.put(parname,this.Val);
         switch (this.Symbol)
         {
             case equal:
                 sql = String.format(" [%s]= ?%s ",this.ColName, parname);
                 break;
             case greater:
                 sql = String.format(" [%s]> ?%s ", this.ColName,parname);
                 break;
             case less:
                 sql = String.format(" [%s]< ?%s ", this.ColName, parname);
                 break;
             case like:
                 sql = String.format(" [%s] like ?%s ", this.ColName,parname);
                 break;
             default:
                 sql = "";
                 break;
         }

         return sql;
    }
}
