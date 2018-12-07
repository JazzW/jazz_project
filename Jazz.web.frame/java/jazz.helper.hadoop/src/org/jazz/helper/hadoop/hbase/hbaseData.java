package org.jazz.helper.hadoop.hbase;

import java.util.HashMap;
import java.util.Map;
import java.util.Map.Entry;

import org.apache.hadoop.hbase.client.Put;

public class hbaseData {
	
	private Map<String,Object> Data;
	
	public Map<String,Object> getData() {
		return Data;
	}
	
	public void setData(Map<String,Object> data) {
		Data = data;
	}
	
	public hbaseData()
	{
		this.Data=new HashMap<String,Object>();
	}
	
	public void addData(String Name,Object Value){
		this.Data.put(Name, Value);
	}
	
	public Put toPut(String RowKey,String FmailyName)
	{
		 Put put = new Put(RowKey.getBytes());
		 for(Entry<String, Object> entry:Data.entrySet()){
			 if(entry.getValue().getClass()!=byte[].class)
			 {
				 put.addColumn(FmailyName.getBytes(), entry.getKey().getBytes(), entry.getValue().toString().getBytes());
			 }
			 else
			 {
				 put.addColumn(FmailyName.getBytes(), entry.getKey().getBytes(), (byte[]) entry.getValue());
			 }
		 }
		 return put;
	}
}
