package org.jazz.helper.hadoop.common;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.Mapper;
import org.apache.hadoop.mapreduce.Reducer;

public class hadoopMRJob {
	
	public String Name;
	
	public Class<?> JarClass;
	
	@SuppressWarnings("rawtypes")
	public Class<? extends Mapper> MapperClass;
	
	@SuppressWarnings("rawtypes")
	public Class<? extends Reducer> ReducerClass;
	
	public boolean UseCombine;
	
	public int ReduceTaskNum;
	
	@SuppressWarnings("rawtypes")
	public static hadoopMRJob createhadoopMRJob(String Name,Class<?> JarCls,Class<? extends Mapper> MapperCls,
			Class<? extends Reducer> ReducerCls,boolean Combine,int reduceTaskNum){
		 hadoopMRJob job=new hadoopMRJob();
		 job.Name=Name;
		 job.JarClass=JarCls;
		 job.MapperClass=MapperCls;
		 job.ReducerClass=ReducerCls;
		 job.UseCombine=Combine;
		 job.ReduceTaskNum=reduceTaskNum;
		 return job;
	}
	
	public Job ConvertToJob(Configuration config ) throws Exception{
		Job job=Job.getInstance(config);
		job.setJobName(Name);
		job.setJarByClass(JarClass);
		job.setMapperClass(MapperClass);
		job.setReducerClass(ReducerClass);
		if(ReduceTaskNum>0){
			job.setNumReduceTasks(ReduceTaskNum); //ReduceTask个数
		}
		if(this.UseCombine){
			job.setCombinerClass(ReducerClass);
		}
		return job;
	}
}
