package org.jazz.ado.frame.config;

import java.io.BufferedInputStream;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

import org.jazz.helper.database.common.DBConfig;

/**
 * ado框架唯一配置数据库入口，操做对象{@link DBConfig}
 * @author Wangjc
 * @author 2018.10.10
 */
public class ADODBConfig {
	/**
	 * 初始化数据库配置
	 * @param jdbcPath 。properties 文件绝对路径
	 */
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
