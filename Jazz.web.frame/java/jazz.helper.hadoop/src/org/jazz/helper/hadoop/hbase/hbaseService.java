package org.jazz.helper.hadoop.hbase;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.hbase.HBaseConfiguration;
import org.apache.hadoop.hbase.HColumnDescriptor;
import org.apache.hadoop.hbase.HTableDescriptor;
import org.apache.hadoop.hbase.TableName;
import org.apache.hadoop.hbase.client.Admin;
import org.apache.hadoop.hbase.client.Connection;
import org.apache.hadoop.hbase.client.ConnectionFactory;
import org.apache.hadoop.hbase.client.Delete;
import org.apache.hadoop.hbase.client.Get;
import org.apache.hadoop.hbase.client.Result;
import org.apache.hadoop.hbase.client.ResultScanner;
import org.apache.hadoop.hbase.client.Scan;
import org.apache.hadoop.hbase.client.Table;
import org.apache.hadoop.hbase.filter.CompareFilter;
import org.apache.hadoop.hbase.filter.Filter;
import org.apache.hadoop.hbase.filter.FilterList;
import org.apache.hadoop.hbase.filter.SingleColumnValueFilter;


public class hbaseService {
	
   public static Configuration createConfig(String Ip,String Port){
	   Configuration conf= HBaseConfiguration.create();
       conf.set("hbase.zookeeper.quorum",Ip);
       conf.set("hbase.zookeeper.property.clientPort", Port);
       return conf;
   }
	
   public  Configuration configuration;
   
   private Connection connection;
   
   private Admin admin;
   
   public hbaseService(Configuration conf)
   {
	   this.configuration=conf;
   }
   
   public  Connection getConnection() throws Exception
   {
	   if(this.connection==null)
	   {
		   this.connection= ConnectionFactory.createConnection(configuration);
	   }
	   return this.connection;
   }
   
   public  Admin getAdmin() throws Exception
   {
	   if(this.admin==null)
	   {
		   this.admin= this.getConnection().getAdmin();
	   }
	   return this.admin;
   }
   
   public void Close() throws Exception
   {
	   if(this.admin!=null)
	   {
		   this.admin.close();
	   }
	   if(this.connection!=null)
	   {
		   this.connection.close();
	   }
   }
   
   public void CreateTable(String tableName,String... Colfamily) throws Exception{
	   Admin admin=this.getAdmin();
	   TableName tbName = TableName.valueOf(tableName);
	   if(admin.tableExists(tbName)){
		   System.err.println("表" + tableName + "已存在！");
		   throw new Exception("Table" + tableName + " is existed！");
	   }
	   HTableDescriptor HTD = new HTableDescriptor(tbName);
       for(String cf : Colfamily){
           HColumnDescriptor HCD =new HColumnDescriptor(cf);
           HTD.addFamily(HCD);
       }
       admin.createTable(HTD);
   }
   
   public void deleteTable(String tableName) throws Exception {
       Admin admin = this.getAdmin();
       TableName tbName = TableName.valueOf(tableName);
       if (admin.tableExists(tbName)) {
           admin.disableTable(tbName);
           admin.deleteTable(tbName);
           System.err.println("表" + tableName + "已删除");
       }else{
           System.err.println("表" + tableName + "不存在！");
       }
   }

   public void deleteRow(String tableName,String... rowKeys) throws Exception{
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
    
       List<Delete> list = new ArrayList<Delete>();
       for(String rkey:rowKeys){
    	   Delete delete = new Delete(rkey.getBytes());
    	   list.add(delete);
       }
       tb.delete(list);
       tb.close();
   }

   public List<hbaseResult> getRow(String tableName,String... rowKeys) throws Exception{
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
	   List<Get> list = new ArrayList<Get>();
	   for(String rkey:rowKeys){
		   Get get = new Get(rkey.getBytes());
		   list.add(get);
	   }
	   List<hbaseResult> res= hbaseResult.createRes(tb.get(list));
	   tb.close();
	   return res;
              
   }
   
   public List<hbaseResult> getBetweenRow(String tableName,String startRow,String stopRow) throws Exception{
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
       Scan scan = new Scan();    
    
       scan.setStartRow(startRow.getBytes());
       scan.setStopRow(stopRow.getBytes());    
       List<hbaseResult> res= hbaseResult.createRes(tb.getScanner(scan));
       tb.close();
       return res;
   }
  
   /**
     * 
     * @param tableName
     * @param arr :familyname,qualifiername->val
     * @return
     * @throws Exception 
     * @throws IOException 
    */
   public List<hbaseResult> getResult(String tableName,String startRow,String stopRow,String... arr) throws  Exception
   {
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
	   Scan scan=new Scan();
	   if(!startRow.equals("") && !stopRow.equals(""))
	   {
	       scan.setStartRow(startRow.getBytes());
	       scan.setStopRow(stopRow.getBytes());
	   }
	   FilterList filterList = new FilterList();
	   for(String ar:arr){
		   String [] vs=ar.split("-");
		   Filter f;
		   if(vs[1].charAt(0)=='>')
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.GREATER,vs[1].getBytes());
			   // 添加下面这一行后，则只返回指定的cell，同一行中的其他cell不返回
			   //scan.addColumn(Bytes.toBytes(s[0]), Bytes.toBytes(s[1]));
		   }
		   else if (vs[1].charAt(0)=='<')
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.LESS,vs[1].getBytes());
		   }
		   else if (vs[1].charAt(0)=='!')
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.NOT_EQUAL,vs[1].getBytes());
		   }
		   else
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.EQUAL,vs[1].getBytes());
		   }
		   filterList.addFilter(f);
	   }
	   scan.setFilter(filterList);
	   
	   List<hbaseResult> res= hbaseResult.createRes(tb.getScanner(scan));
	   tb.close();
	   return res;
   }
   
   public void getRow(hbaseReduce reduce,String tableName,String... rowKeys) throws Exception{
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
	   List<Get> list = new ArrayList<Get>();
	   for(String rkey:rowKeys){
		   Get get = new Get(rkey.getBytes());
		   list.add(get);
	   }     
       for(Result result:tb.get(list)){
  	   		reduce.Action.apply(result);
       }
       tb.close(); 
   }
   
   public void getBetweenRow(hbaseReduce reduce,String tableName,String startRow,String stopRow) throws Exception{
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
       Scan scan = new Scan();    
    
       scan.setStartRow(startRow.getBytes());
       scan.setStopRow(stopRow.getBytes());    
       ResultScanner results=  tb.getScanner(scan);
	   for(Result rr=results.next();rr!=null;rr=results.next()){  
	          reduce.Action.apply(rr);
	   }
	   tb.close();
   }
  
   /**
     * 
     * @param tableName
     * @param arr :familyname,qualifiername->val
     * @return
     * @throws Exception 
     * @throws IOException 
    */
   public void getResult(hbaseReduce reduce,String tableName,String startRow,String stopRow,String... arr) throws  Exception
   {
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
	   Scan scan=new Scan();
	   if(!startRow.equals("") && !stopRow.equals(""))
	   {
	       scan.setStartRow(startRow.getBytes());
	       scan.setStopRow(stopRow.getBytes());
	   }
	   FilterList filterList = new FilterList();
	   for(String ar:arr){
		   String [] vs=ar.split("-");
		   Filter f;
		   if(vs[1].charAt(0)=='>')
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.GREATER,vs[1].getBytes());
			   // 添加下面这一行后，则只返回指定的cell，同一行中的其他cell不返回
			   //scan.addColumn(Bytes.toBytes(s[0]), Bytes.toBytes(s[1]));
		   }
		   else if (vs[1].charAt(0)=='<')
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.LESS,vs[1].getBytes());
		   }
		   else if (vs[1].charAt(0)=='!')
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.NOT_EQUAL,vs[1].getBytes());
		   }
		   else
		   {
			   String[] v=vs[0].split(",");
			   f= new SingleColumnValueFilter(v[0].getBytes(),v[1].getBytes(),
					   CompareFilter.CompareOp.EQUAL,vs[1].getBytes());
		   }
		   filterList.addFilter(f);
	   }
	   scan.setFilter(filterList);
	   
	   ResultScanner results=  tb.getScanner(scan);
	   for(Result rr=results.next();rr!=null;rr=results.next()){  
	          reduce.Action.apply(rr);
	   }
	   tb.close();
   }
   
   public void putData(String tableName,hbaseFamily... hfs) throws Exception
   {
	   Table tb = this.getConnection().getTable(TableName.valueOf(tableName));
	   for(hbaseFamily hf: hfs){
		   tb.put(hf.toPuts());
	   }
	   tb.close();
   }
   
   public void addAFamily(String tableName,String familyName) throws Exception{
	   TableName tablename=TableName.valueOf(tableName);
	   Admin adm=this.getAdmin();
	   adm.disableTable(tablename);
	   HTableDescriptor hDescriptor= adm.getTableDescriptor(tablename);
	   HColumnDescriptor hColumnDescriptor=new HColumnDescriptor(familyName);
	   hDescriptor.addFamily(hColumnDescriptor);
	   adm.modifyTable(tablename, hDescriptor);
	   adm.enableTable(tablename);  
	}  
}
