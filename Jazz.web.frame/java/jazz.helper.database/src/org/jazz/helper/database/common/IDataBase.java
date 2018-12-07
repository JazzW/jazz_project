package org.jazz.helper.database.common;

import java.sql.Connection;
import java.sql.SQLException;
import java.util.HashMap;
import java.util.List;

public interface IDataBase {
	
	/**
	 * 获得数据库连接
	 * @return
	 */
	Connection getConnection();
	
	/**
	 * 开始事务
	 * @return {@code Connection} 数据库连接
	 */
	Connection beginTran();
	
	/**
	 * 回滚事务
	 * @param con
	 * @throws SQLException
	 */
	void rollbackTran(Connection con) throws SQLException;
	
	/**
	 * 提交事务
	 * @param con
	 * @return
	 */
	int commintTran(Connection con);
	
	/**
	 * 执行数据库数据修改语句
	 * @param safeSql 
	 * @param conn 启用事务事务时传入连接字符串
	 * @return
	 */
	int ExecuteNonQuery(String safeSql,Connection conn);
	
	/**
	 * 执行数据库修改语句
	 * @param safeSql
	 * @param params 
	 * @param conn 启用事务事务时传入连接字符串
	 * @return
	 */
	int ExecuteNonQuery(String safeSql,HashMap<String,Object> params,Connection conn);
	
	/**
	 * 获得查询图
	 * @param safeSql
	 * @return {@code List<HashMap<String,Object>>}
	 */
	List<HashMap<String,Object>> ExecuteDataTable(String safeSql);
	
	/**
	 * 获得查询图
	 * @param Sql
	 * @param params
	 * @return {@code List<HashMap<String,Object>>}
	 */
	List<HashMap<String,Object>> ExecuteDataTable(String Sql,HashMap<String,Object> params);
	
	/**
	 * 获得查询图
	 * @param sql sql语句
	 * @param params 参数图
	 * @param cls 实体类
	 * @return {@code List<HashMap<String,Object>>}
	 */
	<T> List<T> ExecuteDataTable(String sql,HashMap<String,Object> params,Class<T> cls);
	
	/**
	 * 获得统计值
	 * @param safeSql
	 * @return
	 */
	int ExecuteScalar(String safeSql);

	/**
	 * 获得统计值
	 * @param sql
	 * @param params
	 * @return
	 */
    int ExecuteScalar(String sql,HashMap<String,Object> params);

	
}
