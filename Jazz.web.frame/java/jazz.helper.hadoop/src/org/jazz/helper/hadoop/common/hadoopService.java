package org.jazz.helper.hadoop.common;

import java.io.BufferedInputStream;
import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.URI;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.LocatedFileStatus;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.fs.RemoteIterator;
import org.apache.hadoop.io.IOUtils;
import org.apache.hadoop.mapreduce.Job;

public class hadoopService {

	public static Configuration  createConfig(String uri){
		 Configuration config = new Configuration();
		 config.set("mapred.job.tracker",uri.replace("hdfs://", "").replace("/", ""));
		 return config;
	}
	
	public static hadoopService createService(Configuration config,String URI){
		hadoopService service=new hadoopService();
		service.configuration=config;
		service.uri=URI;
		System.out.println("******Create Configuration*******");
		System.out.println("Hadoop URI:"+URI);
		System.out.println("******"+new Date().toString()+"*******");
		return service;
	}
	
	public String getUri() {
		return uri;
	}

	public Configuration getconfiguration() {
		return configuration;
	}

	private Configuration configuration;
	
	private String uri;
	
	private FileSystem filesystem;
	
	public FileSystem getFileSystem() throws Exception{
		if(filesystem==null){
			this.filesystem= FileSystem.get(URI.create(uri), configuration);
		}
		return this.filesystem;
	}
	
	public void close() throws Exception{
		if(filesystem!=null){
		   filesystem.close();
		   filesystem=null;
		}
	}
	
	public void createDir(String path) throws Exception{
		FileSystem fs = this.getFileSystem();
		fs.mkdirs(new Path(path));
	}
	
	
	public void appendData(String HadoopPath,String Content){
		try {
			FileSystem fs = this.getFileSystem();
            //要追加的文件流，inpath为文件
            InputStream in = new BufferedInputStream(new ByteArrayInputStream(Content.getBytes()));
            OutputStream out = fs.append(new Path(HadoopPath));
            IOUtils.copyBytes(in, out,configuration);
            out.close();
            IOUtils.closeStream(in);
            in.close();
        } catch (Exception e) {
            e.printStackTrace();
        }

	}

	public void uploadFile(String Localpath,String HadoopPath) throws Exception{
		FileSystem fs = this.getFileSystem();
		fs.copyFromLocalFile(new Path(Localpath), new Path(HadoopPath));
	}
	
	public void downloadFile(String Localpath,String HadoopPath) throws Exception{
		FileSystem fs = this.getFileSystem();
		fs.copyToLocalFile( new Path(HadoopPath),new Path(Localpath));
	}

	public void delete(String path) throws Exception{
		FileSystem fs = this.getFileSystem();
		fs.delete(new Path(path),true);
	}

	public List<hadoopPathData> ListFiles(String path,boolean recursive) throws Exception {

	    FileSystem fs =this.getFileSystem();
	    RemoteIterator<LocatedFileStatus> remoteIterator = fs.listFiles(new Path(path), recursive);
		List<hadoopPathData> res=new ArrayList<hadoopPathData>();
	    while(remoteIterator.hasNext()) {
	        LocatedFileStatus status = remoteIterator.next();
	        res.add(hadoopPathData.createFile(status));
	    }
	    return res;
	}

	public List<hadoopPathData> ListPathData(String path) throws Exception {

	    FileSystem fs =this.getFileSystem();
	    RemoteIterator<LocatedFileStatus> remoteIterator = fs.listLocatedStatus(new Path(path));
		List<hadoopPathData> res=new ArrayList<hadoopPathData>();
	    while(remoteIterator.hasNext()) {
	        LocatedFileStatus status = remoteIterator.next();
	        res.add(hadoopPathData.createFile(status));
	    }
	    return res;
	}
	
	public hadoopJobDriver createJobDriver(hadoopMRJob mrJob,Class<?> MapKeyClass,Class<?> MapValueClass
			,Class<?> ReduceKeyClass,Class<?> ReduceValueClass) throws Exception{
		hadoopJobDriver driver=new hadoopJobDriver();
		Job job=mrJob.ConvertToJob(getconfiguration());
	    job.setMapOutputKeyClass(hadoopClassFactory.getClass(MapKeyClass));
	    job.setMapOutputValueClass(hadoopClassFactory.getClass(MapValueClass));
	
	     //指定本次mr 最终输出的 k v类型
	    job.setOutputKeyClass(hadoopClassFactory.getClass(ReduceKeyClass));
	    job.setOutputValueClass(hadoopClassFactory.getClass(ReduceValueClass));
	    driver.setJob(job);
		return driver;
	}
	
}
