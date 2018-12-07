package org.jazz.common.jms;

import java.util.function.Function;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageConsumer;
import javax.jms.MessageProducer;

public class jmsService {
   public static void PTPsend(jmsData data,jmsConfig config) throws JMSException
   {
	  MessageProducer producer= config.getNewProducer();
	  producer.send(data.get_mess());
	  producer.close();
   }
   
   public static jmsResult PTPreceive(jmsConfig config,Function<Message,Object> onMessagefunc) throws JMSException
   {
	  MessageConsumer consumer=config.getNewConsumer();
	  jmsResult res=new jmsResult();
//	  consumer.setMessageListener(new MessageListener() {
//          @Override
//          public void onMessage(Message message) {
//        	 res.setData(onMessagefunc.apply(message));
//          }
//      });
//	  consumer.close();
	  Message receive = consumer.receive();
//	  receiveMess
	   if(null != receive){
	      res.setData(onMessagefunc.apply(receive));
	      // receive.acknowledge();
	   }else{
	       //没有值嘛
		   System.out.println("it not have message");
	   //
	   }
	  
	  return res;
   }
   
   public static jmsResult PTPreceive(MessageConsumer consumer,Function<Message,Object> onMessagefunc) throws JMSException
   {
	  jmsResult res=new jmsResult();
	  Message receive = consumer.receive();
	   if(null != receive){
	      res.setData(onMessagefunc.apply(receive));
	      // receive.acknowledge();
	   }else{
	       //没有值嘛
		   System.out.println("it not have message");
	   //
	   }
	  
	  return res;
   }
   
   public static void TOPsend(jmsData data,jmsConfig config)
   {
   }
   
   public static jmsResult TOPreceive(jmsConfig config)
   {
   
	   return null;
   }
}
