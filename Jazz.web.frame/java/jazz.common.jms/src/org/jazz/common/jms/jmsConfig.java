package org.jazz.common.jms;

import javax.jms.Destination;
import javax.jms.JMSException;
import javax.jms.MessageConsumer;
import javax.jms.MessageProducer;
import javax.jms.Session;

public class jmsConfig {
	
	public static jmsConfig CreateNewConfig(jmsBaseConfig baseconfig,String SessionName,jmsSessionTypeEnum SessionType)
	{
	   jmsConfig config=new jmsConfig();
	   config.set_BaseConfig(baseconfig);
	   config.set_SessionName(SessionName);
	   config.set_SessionType(SessionType);
	   return config;
	}
	
	private jmsBaseConfig _BaseConfig;
	
	private String _SessionName;
	
	private jmsSessionTypeEnum _SessionType;
	
	private Session _session;
	
	private boolean _hasUpdated=true;
	
	
	public jmsBaseConfig get_BaseConfig() {
		return _BaseConfig;
	}

	public void set_BaseConfig(jmsBaseConfig _BaseConfig) {
		this.set_hasUpdated(true);
		this._BaseConfig = _BaseConfig;
	}

	public String get_SessionName() {
		return _SessionName;
	}

	public void set_SessionName(String _SessionName) {
		this.set_hasUpdated(true);
		this._SessionName = _SessionName;
	}

	public jmsSessionTypeEnum get_SessionType() {
		return _SessionType;
	}

	public void set_SessionType(jmsSessionTypeEnum _SessionType) {
		this.set_hasUpdated(true);
		this._SessionType = _SessionType;
	}
	
	public Session setNewjvmSession(boolean UseTran,int SessionType ) throws JMSException
	{
		this.set_hasUpdated(true);
		this.get_BaseConfig().getJvmConnection().start();
		_session =this.get_BaseConfig().getJvmConnection().createSession(UseTran,SessionType);
		return _session;
	}
	
	
	public MessageProducer getNewProducer() throws JMSException
	{
		if(_SessionType==jmsSessionTypeEnum.PonintToPoint)
		{
			Destination destination = _session.createQueue(this._SessionName);
	        //根据session，创建一个接收者对象
	        return _session.createProducer(destination);
		}
		else if(_SessionType==jmsSessionTypeEnum.TopicToPoint)
		{
			Destination destination = _session.createTopic(this._SessionName);
	        //根据session，创建一个接收者对象
	        return _session.createProducer(destination);
		}
		
		throw new JMSException("Session Type can not be finded!");
	}
	
	public MessageConsumer getNewConsumer() throws JMSException
	{
		if(_SessionType==jmsSessionTypeEnum.PonintToPoint)
		{
			Destination destination = _session.createQueue(this._SessionName);
	        //根据session，创建一个接收者对象
	        return _session.createConsumer(destination);
		}
		else if(_SessionType==jmsSessionTypeEnum.TopicToPoint)
		{
			Destination destination = _session.createTopic(this._SessionName);
	        //根据session，创建一个接收者对象
	        return _session.createConsumer(destination);
		}
		
		throw new JMSException("Session Type can not be finded!");
	}

	public boolean is_hasUpdated() {
		return _hasUpdated;
	}

	public void set_hasUpdated(boolean _hasUpdated) {
		this._hasUpdated = _hasUpdated;
	}
	
	public void Dispose() throws JMSException
	{
		if(this._session!=null)
		{
			this._session.close();
		}
		
		if(this._BaseConfig.getJvmConnection()!=null)
		{
			this._BaseConfig.getJvmConnection().close();
		}
	}
}
