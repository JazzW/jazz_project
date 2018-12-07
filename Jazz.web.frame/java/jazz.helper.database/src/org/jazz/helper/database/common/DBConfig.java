package org.jazz.helper.database.common;

import java.io.BufferedInputStream;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

import org.jazz.common.ienum.DBType;

public class DBConfig {
	public static  DBType DBType;
	public static  String URL;
	public static  String USER;
	public static String  PASSWORD;
	
	public static String getDriver()
	{
	  switch(DBConfig.DBType)
	  {
		  case MsSql:
			  return "com.microsoft.sqlserver.jdbc.SQLServerDriver";
		  default:
			break;
	  }
	  return "";
	}
	
	public static void init(String jdbcPath)
	{
		Properties props =new Properties();
		try {
			//props.load(Thread.currentThread().getContextClassLoader().getResourceAsStream("jdbc.properties"));
			InputStream in = new BufferedInputStream(new FileInputStream(jdbcPath)); 
			props.load(in);
			DBConfig.DBType= org.jazz.common.ienum.DBType.valueOf(props.getProperty("jdbc.DBType"));
			DBConfig.URL=props.getProperty("jdbc.url");
			DBConfig.USER=props.getProperty("jdbc.username");
			DBConfig.PASSWORD=props.getProperty("jdbc.password");
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
}
