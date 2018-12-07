using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;

namespace Jazz.Common.JMS
{
    public class JmsConfig : IDisposable
    {

        public static JmsConfig CreateNewConfig(JmsBaseConfig baseconfig, String SessionName, JmsSessionTypeEnum SessionType)
        {
            JmsConfig config = new JmsConfig();
            config.set_BaseConfig(baseconfig);
            config.set_SessionName(SessionName);
            config.set_SessionType(SessionType);
            return config;
        }

        private JmsBaseConfig _BaseConfig;

        private String _SessionName;

        private JmsSessionTypeEnum _SessionType;

        private ISession _session;

        private bool _hasUpdated = true;

        public JmsBaseConfig get_BaseConfig()
        {
            return _BaseConfig;
        }

        public void set_BaseConfig(JmsBaseConfig _BaseConfig)
        {
            this.set_hasUpdated(true);
            this._BaseConfig = _BaseConfig;
        }

        public String get_SessionName()
        {
            return _SessionName;
        }

        public void set_SessionName(String _SessionName)
        {
            this.set_hasUpdated(true);
            this._SessionName = _SessionName;
        }

        public JmsSessionTypeEnum get_SessionType()
        {
            return _SessionType;
        }

        public void set_SessionType(JmsSessionTypeEnum _SessionType)
        {
            this.set_hasUpdated(true);
            this._SessionType = _SessionType;
        }
        public bool is_hasUpdated()
        {
            return _hasUpdated;
        }

        public void set_hasUpdated(bool _hasUpdated)
        {
            this._hasUpdated = _hasUpdated;
        }

        public ISession setNewjvmSession(AcknowledgementMode SessionType)
        {
            this.set_hasUpdated(true);
            this.get_BaseConfig().getJvmConnection().Start();
            _session = this.get_BaseConfig().getJvmConnection().CreateSession(SessionType);
            return _session;
        }


        public IMessageProducer getNewProducer()
        {
            if (_SessionType == JmsSessionTypeEnum.PonintToPoint)
            {
                IDestination destination = SessionUtil.GetDestination(_session, _SessionName, DestinationType.Queue);
                //根据session，创建一个接收者对象
                return _session.CreateProducer(destination);
            }
            else if (_SessionType == JmsSessionTypeEnum.TopicToPoint)
            {
                IDestination destination = SessionUtil.GetDestination(_session, _SessionName, DestinationType.Topic);
                //根据session，创建一个接收者对象
                return _session.CreateProducer(destination);
            }

            throw new Exception("Session Type can not be finded!");
        }

        public IMessageConsumer getNewConsumer()
        {
            if (_SessionType == JmsSessionTypeEnum.PonintToPoint)
            {
                IDestination destination = SessionUtil.GetDestination(_session, _SessionName, DestinationType.Queue);
                //根据session，创建一个接收者对象
                return _session.CreateConsumer(destination);
            }
            else if (_SessionType == JmsSessionTypeEnum.TopicToPoint)
            {
                IDestination destination = SessionUtil.GetDestination(_session, _SessionName, DestinationType.Topic);
                //根据session，创建一个接收者对象
                return _session.CreateConsumer(destination);
            }

            throw new Exception("Session Type can not be finded!");
        }

        public void Dispose()
        {
            if (this._session != null)
            {
                this._session.Close();
            }

            if (this._BaseConfig.getJvmConnection() != null)
            {
                this._BaseConfig.getJvmConnection().Close();
            }
        }
    }
}
