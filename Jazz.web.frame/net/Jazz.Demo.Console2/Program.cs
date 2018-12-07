using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.JMS;
using Apache.NMS;

namespace Jazz.Demo.Console2
{
    class Program
    {
        static void Main(string[] args)
        {
            JmsBaseConfig baseConfig = new JmsBaseConfig();
            using (JmsConfig Config = JmsConfig.CreateNewConfig(baseConfig, "queue-11-02", JmsSessionTypeEnum.PonintToPoint))
            {
                JmsData data = new JmsData();
                var session = Config.setNewjvmSession(Apache.NMS.AcknowledgementMode.AutoAcknowledge);
                var consumer = Config.getNewConsumer();
                while (true)
                {
                    //data.Data = session.CreateTextMessage(DateTime.Now.ToString() + ":" + System.Console.ReadLine());
                    //producer.PTPsend(data);

                    consumer.PTPreceive((m) =>
                    {
                        try
                        {
                            System.Console.WriteLine(((ITextMessage)m).Text);
                        }
                        catch (Exception e)
                        {
                        } return null;
                    });
                }
                //System.Console.Read();
            }   
        }
    }
}
