package org.jazz.common.web.config;

import java.util.Map;

import org.jazz.common.ienum.CompareSymbol;

public abstract class IFiliter {
	
	public String ColName;

    public Object Val;

    public CompareSymbol Symbol;
    
    public String toSqlCmdExp(Map<String,Object> pars)
    {
        
        String sql = "";
    
        return sql;
    }

}
