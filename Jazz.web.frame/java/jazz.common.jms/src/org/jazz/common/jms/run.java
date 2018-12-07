package org.jazz.common.jms;

import java.util.Date;

import javax.jms.JMSException;
import javax.jms.MessageConsumer;
import javax.jms.Session;
import javax.jms.TextMessage;

public class run {

	public static void Demo() throws JMSException {
		// TODO Auto-generated method stub
		jmsBaseConfig baseConfig=new jmsBaseConfig();

		jmsConfig Config=jmsConfig.CreateNewConfig(baseConfig, "queue-11-02",jmsSessionTypeEnum.PonintToPoint);
		jmsData data=new jmsData();
		Session session=Config.setNewjvmSession(false, Session.AUTO_ACKNOWLEDGE);
		
		data.set_mess(session.createTextMessage("hello world:"+new Date().toString()));	
     	//jmsService.PTPsend(data, Config);
		MessageConsumer customer= Config.getNewConsumer();
		while(true)
		{
			jmsService.PTPreceive(customer, (m)->{try {
				System.out.println(((TextMessage)m).getText());
			} catch (JMSException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}return null;});
			System.out.println("Finish");
		}
		
//		Queue destination = session.createQueue(Config.get_SessionName());
//        MessageConsumer consumer = session.createConsumer(destination);
//        consumer.setMessageListener(new MessageListener() {
//            @Override
//            public void onMessage(Message message) {
//                try {
//                    //获取到接收的数据
//                    String text = ((TextMessage)message).getText();
//                    System.out.println(text);
//                } catch (JMSException e) {
//                    e.printStackTrace();
//                }
//            }
//        });
		
     	//Config.Dispose();
    	//System.out.println("Finish");
	}

}
