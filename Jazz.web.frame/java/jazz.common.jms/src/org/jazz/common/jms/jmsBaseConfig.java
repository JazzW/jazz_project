package org.jazz.common.jms;

import javax.jms.Connection;
import javax.jms.ConnectionFactory;
import javax.jms.JMSException;

import org.apache.activemq.ActiveMQConnectionFactory;


public class jmsBaseConfig {
	 //连接账号
    private String _userName = "";
    //连接密码
    private String _password = "";
    //连接地址
    private String _brokerURL = "tcp://127.0.0.1:61616";
    
    private ConnectionFactory _factory;
    
    private Connection _connection;
    
	public String get_userName() {
		return _userName;
	}
	public void set_userName(String _userName) {
		this._userName = _userName;
	}
	public String get_password() {
		return _password;
	}
	public void set_password(String _password) {
		this._password = _password;
	}
	public String get_brokerURL() {
		return _brokerURL;
	}
	public void set_brokerURL(String _brokerURL) {
		this._brokerURL = _brokerURL;
	}
	
	public Connection getJvmConnection() throws JMSException
	{
		if(_connection==null)
			_connection= getJvmFactory().createConnection();
		return _connection;
	}
	
	public ConnectionFactory getJvmFactory()
	{
		if(_factory==null)
			_factory= new ActiveMQConnectionFactory(_userName, _password, _brokerURL);
		return _factory;
	}
}
