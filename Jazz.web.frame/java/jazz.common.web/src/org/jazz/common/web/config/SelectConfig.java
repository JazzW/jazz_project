package org.jazz.common.web.config;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

import org.jazz.common.ienum.LinkEnum;

public class SelectConfig {
	public LinkEnum Link ;
	
	public List<IFiliter> Filiters;
	
	public SelectConfig(IFiliter... fs)
	{
		this.Filiters= Arrays.asList(fs);
	}
	
	public String toSqlCmdExp(Map<String,Object> pars)
    {

        String sql = "";
        for(IFiliter f : Filiters)
        {
            if (sql.length() == 0)
                sql = f.toSqlCmdExp(pars);
            else
            {
                switch (this.Link)
                {
                    case and:
                        sql +=" and "+ f.toSqlCmdExp(pars);
                        break;
                    case or:
                        sql +=" or "+ f.toSqlCmdExp(pars);
                        break;
                }
            }
        }
        if (sql.length() == 0)
            sql = " 1=1 ";
        return sql;
    }
}
