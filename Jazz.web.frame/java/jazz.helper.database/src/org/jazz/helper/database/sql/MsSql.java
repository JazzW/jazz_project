package org.jazz.helper.database.sql;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import org.jazz.helper.database.common.DBConfig;
import org.jazz.helper.database.common.IDataBase;
import org.jazz.helper.database.common.NSQL;

public class MsSql implements IDataBase {

	private static final String DRIVER=DBConfig.getDriver();
    private static final String URL=DBConfig.URL;
    private static final String USER=DBConfig.USER;
    private static final String PASSWORD=DBConfig.PASSWORD;	
	
	@Override
	public Connection getConnection() {
        Connection conn=null;
        try {
            Class.forName(DRIVER);
        } catch (ClassNotFoundException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        try {
            conn=DriverManager.getConnection(URL, USER, PASSWORD);
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return conn;
	}

	@Override
	public int ExecuteNonQuery(String safeSql, Connection conn) {
		int res=0;//��Ӱ�������
    	PreparedStatement pstmt = null;
	    ResultSet rs=null;
	    boolean tran=false;
	    try
	    {
	    	if(conn!=null)
	    	{
	    		tran=true;
	    	}
	    	else
	    	{
	    		conn=this.getConnection();
	    	}
	    	pstmt=conn.prepareStatement(safeSql);//װ��sql���
	        res=pstmt.executeUpdate();		    	
	    }
	    catch(SQLException e)
	    {
	    	e.printStackTrace();
	    	res= -1;
	    }
	    finally{
	    	if(!tran)
	    		closeAll(rs, pstmt, conn);
	    	else
	    		closeAll(rs, pstmt, null);
	    }
	    return res;
	}

	@Override
	public int ExecuteNonQuery(String Sql, HashMap<String,Object> params, Connection conn) {
		int res=0;//��Ӱ�������
    	PreparedStatement pstmt = null;
	    ResultSet rs=null;
	    boolean tran=false;
	    try
	    {
	    	if(conn!=null)
	    	{
	    		tran=true;
	    	}
	    	else
	    	{
	    		conn=this.getConnection();
	    	}
	    	NSQL sql1 = NSQL.parse(Sql);
	    	pstmt=conn.prepareStatement(sql1.getSql());//װ��sql���
    	   if(params!=null){
		          sql1.setParameters(pstmt,params);
            }
	        res=pstmt.executeUpdate();		    	
	    }
	    catch(SQLException e)
	    {
	    	e.printStackTrace();
	    	res= -1;
	    }
	    finally{
	    	if(!tran)
	    		closeAll(rs, pstmt, conn);
	    	else
	    		closeAll(rs, pstmt, null);
	    }
	    return res;
	}

	@Override
	public List<HashMap<String, Object>> ExecuteDataTable(String safeSql) {
		Connection conn=null;
    	ResultSet rs=null;
    	PreparedStatement pstmt = null;
    	try
    	{
	    	conn=getConnection();
	        pstmt=conn.prepareStatement(safeSql);
	        rs=pstmt.executeQuery();
	        List<HashMap<String,Object>> maps = new ArrayList<HashMap<String,Object>>();
	        ResultSetMetaData rsd=rs.getMetaData();//����ж���,ͨ���˶�����Եõ���Ľṹ���������������еĸ������е���������
            while(rs.next()){
            	HashMap<String,Object> map = new HashMap<String,Object>();
                for(int i=0;i<rsd.getColumnCount();i++){
                    String col_name=rsd.getColumnName(i+1);//�������
                    Object value=rs.getObject(col_name);//���������Ӧ��ֵ
                    map.put(col_name, value);
                }
                maps.add(map);
            }
            return maps;
    	}
    	catch(SQLException e)
	    {
    		e.printStackTrace();
	    }
	    finally{
	    	closeAll(rs, pstmt, conn);
	    }
	    return null;
	}

	@Override
	public List<HashMap<String, Object>> ExecuteDataTable(String Sql, HashMap<String,Object> params) {
		// TODO �Զ����ɵķ������
		Connection conn=null;
    	ResultSet rs=null;
    	PreparedStatement pstmt = null;
    	try
    	{
	    	conn=getConnection();
	    	NSQL sql1 = NSQL.parse(Sql);
	    	pstmt=conn.prepareStatement(sql1.getSql());//װ��sql���
    	   if(params!=null){
		          sql1.setParameters(pstmt,params);
            }
	        rs=pstmt.executeQuery();
	        List<HashMap<String,Object>> maps = new ArrayList<HashMap<String,Object>>();
	        ResultSetMetaData rsd=rs.getMetaData();//����ж���,ͨ���˶�����Եõ���Ľṹ���������������еĸ������е���������
            while(rs.next()){
            	HashMap<String,Object> map = new HashMap<String,Object>();
                for(int i=0;i<rsd.getColumnCount();i++){
                    String col_name=rsd.getColumnName(i+1);//�������
                    Object value=rs.getObject(col_name);//���������Ӧ��ֵ
                    map.put(col_name, value);
                }
                maps.add(map);
            }
            return maps;
    	}
    	catch(SQLException e)
	    {
    		e.printStackTrace();
	    }
	    finally{
	    	closeAll(rs, pstmt, conn);
	    }
	    return null;
	}

	@Override
	public <T> List<T> ExecuteDataTable(String sql,HashMap<String,Object> params, Class<T> cls) {
		// TODO �Զ����ɵķ������
		return null;
	}

	@Override
	public Connection beginTran() {
		// TODO �Զ����ɵķ������
		Connection con=this.getConnection();
		try {
			con.setAutoCommit(false);
		} catch (SQLException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		}

		return con;
	}

	@Override
	public int commintTran(Connection con) {
		// TODO �Զ����ɵķ������
		try {
			con.commit();
			this.closeAll(null, null, con);
		} catch (SQLException e) {
			// TODO �Զ����ɵ� catch ��
			e.printStackTrace();
		}
		return 1;
	}
	
	public void closeAll(ResultSet rs,PreparedStatement pstmt,Connection conn){
        try {
            if(rs!=null){
                rs.close();
            }
            if(pstmt!=null){
                pstmt.close();
            }
            if(conn!=null){
                conn.close();
            }
        } catch (SQLException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
	}

	@Override
	public void rollbackTran(Connection con) throws SQLException {
		// TODO Auto-generated method stub
		con.rollback();
	   this.closeAll(null, null, con);
	}

	@Override
	public int ExecuteScalar(String safeSql) {
		// TODO �Զ����ɵķ������
		return 0;
	}

	@Override
	public int ExecuteScalar(String sql, HashMap<String, Object> params) {
		// TODO �Զ����ɵķ������
		Connection conn=null;
    	ResultSet rs=null;
    	PreparedStatement pstmt = null;
    	try
    	{
	    	conn=getConnection();
	    	NSQL sql1 = NSQL.parse(sql);
	    	pstmt=conn.prepareStatement(sql1.getSql());//װ��sql���
    	   if(params!=null){
		          sql1.setParameters(pstmt,params);
            }
	        rs=pstmt.executeQuery();
            while(rs.next()){
               return (int)rs.getObject(1);
            }

    	}
    	catch(SQLException e)
	    {
    		e.printStackTrace();
	    }
	    finally{
	    	closeAll(rs, pstmt, conn);
	    }
	    return -1;
	}
	
}
