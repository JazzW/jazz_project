package org.jazz.helper.hadoop.spark;



import org.apache.spark.SparkConf;

public class sparkService_Common {
	
	public static SparkConf createSparkConfig(String AppName,String MasterName){
		return  new SparkConf().setAppName(AppName).setMaster(MasterName);
	}
	
	
	
	public static class Lamada{
	
	}
}
