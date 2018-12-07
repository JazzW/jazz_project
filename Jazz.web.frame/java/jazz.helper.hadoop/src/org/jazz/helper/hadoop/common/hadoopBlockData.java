package org.jazz.helper.hadoop.common;


public class hadoopBlockData {

	public long Offset;
	
	public String[] Hosts;
	
	public String[] Names;
	
	public Long Length;
	
	public String toString(){
		StringBuffer s=new StringBuffer();
		s.append(" offset:"+this.Offset);
		s.append(" length:"+this.Length);
		String s1="";
		for(String f: this.Hosts ){
			s1+="/"+f;
		}
		String s2="";
		for(String f: this.Names ){
			s2+="/"+f;
		}
		s.append(" hosts:"+s1);
		s.append(" names:"+s2);
		return s.toString();
	}
}
