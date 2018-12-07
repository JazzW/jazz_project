package org.jazz.common.web.config;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

public class TableConfig {
	 public List<SelectConfig> Configs;

     public int Page;

     public int Length;

     public String OrderCol;
     
     public TableConfig(SelectConfig... cons){
    	 this.Configs=Arrays.asList(cons);
     }
     
     public  String toSqlCmdExp(Map<String,Object> pars)
     {

         String sql = "";
         for(SelectConfig f : Configs)
         {
             if (sql.length()==0)
                 sql = String.format(" (%s) ", f.toSqlCmdExp(pars));
             else
                 sql += String.format("and (%s) ", f.toSqlCmdExp(pars));
         }
         if (sql.length() == 0)
             sql = " 1=1 ";

         return sql;
     }
     
     public String toOrderSql()
     {
         if (this.OrderCol==null || "".equals(this.OrderCol))
             return "";
         String sql = " order by ";

         return sql+OrderCol;
     }
}
