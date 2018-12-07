package org.jazz.helper.hadoop.common;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;

import org.apache.hadoop.fs.BlockLocation;
import org.apache.hadoop.fs.LocatedFileStatus;



public class hadoopPathData {
	
	public static hadoopPathData  createFile(LocatedFileStatus status) throws Exception{
		hadoopPathData f=new hadoopPathData();
		f.isFile= status.isFile();
		f.Path=status.getPath().toString();
		f.Name=status.getPath().getName();
		f.Group=status.getGroup();
		if(f.isFile){
			f.Length= status.getLen();
			f.Replication=status.getReplication();
			f.Blocks=new ArrayList<hadoopBlockData>();
		     BlockLocation[] blockLocations = status.getBlockLocations();
		        for(BlockLocation bl: blockLocations) {
		        	
		        	hadoopBlockData b=new hadoopBlockData();
		        	b.Hosts=bl.getHosts();
		        	b.Names=bl.getNames();
		        	b.Offset=bl.getOffset();
		        	b.Length= bl.getLength();
		        	
		        	f.Blocks.add(b);
		        }	
		}
		return f;
		
	}
	
	public static List<hadoopPathData>  createFile(LocatedFileStatus... status) throws Exception{
		List<hadoopPathData> res=new ArrayList<hadoopPathData>();
		for(LocatedFileStatus s:status){
			res.add(hadoopPathData.createFile(s));
		}
		
		return res;
	}
	
	public static void  Show(hadoopPathData data) throws Exception{
		for(Field f: hadoopPathData.class.getFields() ){
			if(f.get(data)!=null){
			System.out.println(f.getName()+":"+f.get(data).toString());
			}
		}
	}
	
	public Boolean isFile;
	
	public Long Length; 
	
	public String Name;
	
	public String Path;
	
	public String Group;
	
	public Short Replication;
	
	public List<hadoopBlockData> Blocks;
}
