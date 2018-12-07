using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace Jazz.Common.JMS
{
    public class JmsBaseConfig
    { //连接账号
        public String _userName = "";
        //连接密码
        public String _password = "";
        //连接地址
        public  String _brokerURL = "tcp://127.0.0.1:61616";

        private IConnectionFactory _factory;

        private IConnection _connection;

        public IConnection getJvmConnection()
	    {
            if (_connection == null)
                _connection = getJvmFactory().CreateConnection(_userName, _password);
		    return _connection;
	    }
	
	    public IConnectionFactory getJvmFactory()
	    {
		    if(_factory==null)
			    _factory=new ConnectionFactory(_brokerURL);
		    return _factory;
	    }
    }
}
