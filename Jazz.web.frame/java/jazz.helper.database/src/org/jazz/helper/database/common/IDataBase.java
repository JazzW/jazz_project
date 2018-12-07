package org.jazz.helper.database.common;

import java.sql.Connection;
import java.sql.SQLException;
import java.util.HashMap;
import java.util.List;

public interface IDataBase {
	
	/**
	 * ������ݿ�����
	 * @return
	 */
	Connection getConnection();
	
	/**
	 * ��ʼ����
	 * @return {@code Connection} ���ݿ�����
	 */
	Connection beginTran();
	
	/**
	 * �ع�����
	 * @param con
	 * @throws SQLException
	 */
	void rollbackTran(Connection con) throws SQLException;
	
	/**
	 * �ύ����
	 * @param con
	 * @return
	 */
	int commintTran(Connection con);
	
	/**
	 * ִ�����ݿ������޸����
	 * @param safeSql 
	 * @param conn ������������ʱ���������ַ���
	 * @return
	 */
	int ExecuteNonQuery(String safeSql,Connection conn);
	
	/**
	 * ִ�����ݿ��޸����
	 * @param safeSql
	 * @param params 
	 * @param conn ������������ʱ���������ַ���
	 * @return
	 */
	int ExecuteNonQuery(String safeSql,HashMap<String,Object> params,Connection conn);
	
	/**
	 * ��ò�ѯͼ
	 * @param safeSql
	 * @return {@code List<HashMap<String,Object>>}
	 */
	List<HashMap<String,Object>> ExecuteDataTable(String safeSql);
	
	/**
	 * ��ò�ѯͼ
	 * @param Sql
	 * @param params
	 * @return {@code List<HashMap<String,Object>>}
	 */
	List<HashMap<String,Object>> ExecuteDataTable(String Sql,HashMap<String,Object> params);
	
	/**
	 * ��ò�ѯͼ
	 * @param sql sql���
	 * @param params ����ͼ
	 * @param cls ʵ����
	 * @return {@code List<HashMap<String,Object>>}
	 */
	<T> List<T> ExecuteDataTable(String sql,HashMap<String,Object> params,Class<T> cls);
	
	/**
	 * ���ͳ��ֵ
	 * @param safeSql
	 * @return
	 */
	int ExecuteScalar(String safeSql);

	/**
	 * ���ͳ��ֵ
	 * @param sql
	 * @param params
	 * @return
	 */
    int ExecuteScalar(String sql,HashMap<String,Object> params);

	
}
