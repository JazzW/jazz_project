package org.jazz.helper.hadoop.hbase;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;

import org.apache.hadoop.hbase.client.Put;

public class hbaseFamily {
	private String Name;
	  
	private Map<String,hbaseData> Data;

	public String getName() {
		return Name;
	}
	
	public void setName(String name) {
		Name = name;
	}
	
	public Map<String,hbaseData> getData() {
		return Data;
	}
	
	public void setData(Map<String,hbaseData> data) {
		Data = data;
	}
	
	public hbaseFamily()
	{
	   this.Data=new HashMap<String,hbaseData>();
	}
  
	public void addRow(String RowKey,hbaseData hData)
	{
		this.Data.put(RowKey, hData);
	}
	
	public List<Put> toPuts()
	{
		List<Put> res =new ArrayList<Put>();
		for(Entry<String, hbaseData> entry:this.Data.entrySet()){
			res.add(entry.getValue().toPut(entry.getKey(), this.Name));
		}
		return res;
	}
	

}
