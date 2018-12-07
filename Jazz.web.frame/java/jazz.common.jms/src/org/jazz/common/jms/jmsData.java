package org.jazz.common.jms;

import javax.jms.Message;

public class jmsData {
   private  Message _mess;

	public Message get_mess() {
		return _mess;
	}
	
	public void set_mess(Message _mess) {
		this._mess = _mess;
	}
}
