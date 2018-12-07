package org.jazz.helper.hadoop.common;


import org.apache.hadoop.fs.Path;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.mapreduce.lib.output.FileOutputFormat;

public class hadoopJobDriver {
    private Job job;
    
    
    public boolean Finish=false;

	public Job getJob() {
		return job;
	}
	
	public void setJob(Job job) {
		this.job = job;
	}
	
	   
    public hadoopJobDriver Start(String InputPath,String OutputPath,boolean listen) throws Exception
    {
      FileInputFormat.setInputPaths(job,InputPath);
      FileOutputFormat.setOutputPath(job,new Path(OutputPath));
      this.Finish= this.getJob().waitForCompletion(listen);
      if(this.Finish){
    	  System.out.println("********Complete Succeed!");
      }
      else
      {
    	  System.out.println("********Complete Fail!");
      }
      return this;
    }
    
    public void downloadResult(String LocalPath,hadoopService Service) throws Exception{
       Path output=	FileOutputFormat.getOutputPath(getJob());
       Service.downloadFile(LocalPath, output.toString());
    }
    
    public void catResult(){
    	
    }
    
    public void tailResult(){
    	
    }
}
