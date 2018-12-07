package org.jazz.helper.database.common;


import java.util.List;
import java.util.Map;

public class SQLCmdHelper {
	 public static String RunProcFrame(String ProcName, Map<String, Object> dict)
     {
         String sqlFrame = "exec %s %s";

         String setsql = "";

         for(String key: dict.keySet())
         {
            setsql = String.format(",%s=%s ", key,dict.get(key).toString());
         }

         return String.format(sqlFrame,ProcName,setsql);
     }

     public static String InsertFrame(String TB,  Map<String, Object> dict)
     {
         String sqlFrame = "insert into %s (%s) values(%s)";
         String colsql = "";
         String valsql = "";

         for(String key: dict.keySet())
         {
             colsql += " " + key;
             valsql += ", " + dict.get(key).toString();
         }

         if (colsql.length() > 0)
             colsql = colsql.substring(1);
         if (valsql.length() > 0)
             valsql = colsql.substring(1);

         return String.format(sqlFrame, TB, colsql, valsql) ;
     }

     public static String InsertListFrame(String TB, List<Map<String, Object>> dicts)
     {
         String sqlFrame = "insert into %s (%s) values %s";
         StringBuilder colsql = new StringBuilder("");
         StringBuilder valsql = new StringBuilder("");
         for(String key: dicts.get(0).keySet())
         {
        	 if(colsql.length()==0)
        		 colsql.append(key);
        	 else
        		 colsql.append("," + key);
         }
 
         for(Map<String, Object> dict : dicts){
        	 StringBuilder str=new StringBuilder("");
	         for(String key: dict.keySet())
	         {
	        	 if(str.length()==0)
	        		 str.append(dict.get(key));
	        	 else
	        		 str.append(" , " + dict.get(key));
	         }
	         if(valsql.length()==0)
	        	 valsql.append(String.format("( %s )", str));
	         else
	        	 valsql.append(String.format(",( %s )", str));
         }
         
         
         return String.format(sqlFrame,TB,colsql,valsql);
     }

     public static String UpdateFrame(String TB, Map<String, Object> dict, String WhereSql)
     {
         String sqlFrame = "Update %s set %s where %s";

         String setsql = "";

         for(String key: dict.keySet())
         {
             if(setsql.length()==0)
                 setsql = String.format(" %s=%s ", key, dict.get(key).toString());
             else
                 setsql += String.format(", %s=%s ", key,dict.get(key).toString());
         }

         return String.format(sqlFrame,TB,setsql,WhereSql);
     }

     public static String DeleteFrame(String TB, String WhereSql)
     {
         String sqlFrame = "delete %s where %s";

         return  String.format(sqlFrame,TB,WhereSql);
     }

     public static String SelectFrame(String ColsSql, String SrcSql,String WhereSql,String OrderSql)
     {
         String sqlFrame = "select %s from %s where %s %s";

         return String.format(sqlFrame,ColsSql,SrcSql,WhereSql,OrderSql);
     }

     public static String SelectToatlFrame(String SrcSql, String WhereSql)
     {
         String sqlFrame = "select Count(1) from %s where %s";

         return String.format(sqlFrame, SrcSql, WhereSql);
     }


     public static String PageFrame(String inSQL, int length, int page)
     {
         if (length > -1 && page > -1)
         {
             String sframe = "select * from(SELECT  ROW_NUMBER() over(order by (select 0)) as [row],fr1.* from(%s) fr1) fr2 where fr2.row>%s  and fr2.row<=%s";

             String outSQL = String.format(sframe, inSQL, (page - 1) * length, (page) * length);

             return outSQL;
         }
         return inSQL;
     }

     public static String CountFrame(String inSQL)
     {
         String sframe = "select count(1) from (%s) fr";
         String outSQL = String.format(sframe, inSQL);
         return outSQL;
     }

     public static String FiliterFrame()
     {
         return null;
     }

}
