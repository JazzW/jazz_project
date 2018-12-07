using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.JMS;
using Apache.NMS;

namespace Jazz.Demo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => { run(); }));
            }
            Task.WaitAll(tasks.ToArray());
        }

        static void run()
        {
            JmsBaseConfig baseConfig = new JmsBaseConfig();
            baseConfig._brokerURL = "tcp://193.112.224.45:61616/";
            using (JmsConfig Config = JmsConfig.CreateNewConfig(baseConfig, "queue-11-02", JmsSessionTypeEnum.PonintToPoint))
            {
                JmsData data = new JmsData();
                ISession session = Config.setNewjvmSession(Apache.NMS.AcknowledgementMode.AutoAcknowledge);
                var producer = Config.getNewProducer();
                while (true)
                {
                    string datastring=new txtbean() { Message = Guid.NewGuid().ToString() }.ToString();
                    // data.Data = session.CreateObjectMessage(new txtbean() { Message = System.Console.ReadLine() });
                    data.Data = session.CreateTextMessage(datastring);
                    System.Console.WriteLine("thread(" + System.Threading.Thread.CurrentThread.ManagedThreadId + "):" + datastring);
                    producer.PTPsend(data);
                    System.Threading.Thread.Sleep(1000);
                    //JmsService.PTPreceive(Config, (m) =>
                    //{
                    //    try
                    //    {
                    //        System.Console.WriteLine(((ITextMessage)m).Text);
                    //    }
                    //    catch (Exception e)
                    //    {
                    //    } return null;
                    //});
                }
                //System.Console.Read();
            }   
        }
    }
}
