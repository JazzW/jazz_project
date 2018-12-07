package org.jazz.helper.hadoop.common;

import java.util.List;

public class Demo {
	public static void main(String[] args)  throws Exception{

	 String uri = "hdfs://93.112.224.45:9000/";
   	 hadoopService hadoop=hadoopService.createService(hadoopService.createConfig(uri), uri);
   	 
//   	 String input="hdfs://localhost:9000/user/test2.csv";
//   	 String output="hdfs://localhost:9000/user/output/newoutput1";
//   	 String download="C:/Users/wjc/Desktop/test";
//   	 hadoopMRJob _job=hadoopMRJob.createhadoopMRJob("job1",Demo.class,hadoopCSVMapper.class
//   			 , hadoopCSVReducer.class, true, 0);
//   	 hadoopJobDriver driver= hadoop.createJobDriver(_job, String.class, int.class,
//   			 String.class, int.class).Start(input, output, true);
//        if(driver.Finish){
//       
//        }
//  	     driver.downloadResult(download, hadoop);
   	 List<hadoopPathData> res= hadoop.ListPathData("/user/");
   	 for(hadoopPathData data:res){
   	     System.out.println( "*******************" );
   	 	 hadoopPathData.Show(data);
   	 	 System.out.println( "*******************" );
   	 }
        hadoop.close();
	}
}
