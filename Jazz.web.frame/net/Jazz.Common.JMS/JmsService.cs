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
    public static class JmsService
    {
        public static void PTPsend(this IMessageProducer producer, JmsData Data)
        {
            producer.Send(Data.Data);
        }

        public static void PTPsend(JmsConfig Config,JmsData Data) 
        {
            using (var producer = Config.getNewProducer())
            {
                producer.Send(Data.Data);
            }
        }

        public static JmsResult PTPreceive( this IMessageConsumer consumer,  Func<IMessage, Object> onMessagefunc)
        {
            JmsResult res = new JmsResult();
            IMessage receive = consumer.Receive();
            if (null != receive)
            {
                res.Data = onMessagefunc(receive);
                //receive.Acknowledge();
            }
            else
            {
            }
            return res;
        }

        public static JmsResult PTPreceive(JmsConfig Config,Func<IMessage,Object> onMessagefunc)
        {
            using (var consumer = Config.getNewConsumer())
            {
                JmsResult res = new JmsResult();
                IMessage receive = consumer.Receive();
                if (null != receive)
                {
                    res.Data = onMessagefunc(receive);
                    //receive.Acknowledge();
                }
                else
                {
                }
                return res;
            }
        }
   
        public static void TOPsend(JmsData data,JmsConfig config)
        {
        }

        public static JmsResult TOPreceive(JmsConfig config)
        {

            return null;
        }
   
    }
}
