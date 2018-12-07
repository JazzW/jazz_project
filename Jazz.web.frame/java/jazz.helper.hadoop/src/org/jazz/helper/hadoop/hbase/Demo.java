package org.jazz.helper.hadoop.hbase;

public class Demo {
   public void run() throws Exception{
     String tablename="testtable";
     
     hbaseService base=new hbaseService(hbaseService.createConfig("172.18.8.98", "2181"));
     
     base.addAFamily(tablename, "cf-new");

     hbaseData hb=new hbaseData(); 
     hb.addData("coll", "1234");
     
     hbaseFamily bf=new hbaseFamily();
     bf.setName("cf-new");
     bf.addRow("123",hb );
 
     base.putData(tablename, bf);
     hbaseReduce reduce=new hbaseReduce(e->{
     	hbaseResult.show(hbaseResult.createRes(e));
     	return null;
     });
     base.getRow(reduce,tablename, "123");
     base.Close();
   }
}
