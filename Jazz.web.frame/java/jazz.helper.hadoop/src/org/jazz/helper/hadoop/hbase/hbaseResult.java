package org.jazz.helper.hadoop.hbase;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.apache.hadoop.hbase.KeyValue;
import org.apache.hadoop.hbase.client.Result;
import org.apache.hadoop.hbase.client.ResultScanner;

public class hbaseResult {
	
  private List<hbaseObj> Entrys;
  
  public List<hbaseObj> getEntrys() {
		return Entrys;
  }
  
  public hbaseResult(){
	  this.Entrys=new ArrayList<hbaseObj>();
  }
  
  @SuppressWarnings("deprecation")
  public static hbaseResult createRes(Result result)
  {
	  hbaseResult res=new hbaseResult();
	  for(KeyValue value:result.raw()){
		  hbaseObj obj=new hbaseObj();
		  obj.Fmaily=new String(value.getFamily());
		  obj.RowKey=new String(value.getRow());
		  obj.Qualifier=new String(value.getQualifier());
		  obj.Value=new String(value.getValue());
		  obj.Timestamp=value.getTimestamp();
		  
		  res.Entrys.add(obj);
      }
	  return res;
  }
  
  public static List<hbaseResult> createRes(Result[] results)
  {
	  List<hbaseResult> res=new ArrayList<hbaseResult>();
	  for(Result result:results){
   	   		res.add(hbaseResult.createRes(result));
      }
	  return res;
  }

  public static List<hbaseResult> createRes(ResultScanner results) throws Exception
  {
	  List<hbaseResult> res=new ArrayList<hbaseResult>();
	  for(Result rr=results.next();rr!=null;rr=results.next()){  
          res.add(hbaseResult.createRes(rr));
      }
	  return res;
  }

  public static void show(hbaseResult res)
  {
      for(hbaseObj entry : res.Entrys){
    	  StringBuffer str=new StringBuffer();
    	  str.append(" Family:"+entry.Fmaily);
    	  str.append(" RowKey:"+entry.RowKey);
    	  str.append(" Qualifier:"+entry.Qualifier);
    	  str.append(" Value:"+entry.Value);
    	  str.append(" Timestamp:"+ GetTimeByStamp(entry.Timestamp));
    	  System.out.println("******"+str.toString()+"******");
      }
  }
  
  public static void show(List<hbaseResult> res)
  {
      for(hbaseResult re:res){
    	  hbaseResult.show(re);
      }
  }
  
  private static String GetTimeByStamp(long timestamp)
  {
   long datatime=timestamp;
   Date date=new Date(datatime);
   SimpleDateFormat format=new SimpleDateFormat("yyyy-MM-dd HH:MM:ss");
   String timeresult=format.format(date);
   return timeresult;
  
  }
}
