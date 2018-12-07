package org.jazz.helper.hadoop.hbase;

import java.util.function.Function;

import org.apache.hadoop.hbase.client.Result;

public class hbaseReduce {
	public Function<Result,Object> Action;
	
	public hbaseReduce(Function<Result,Object> action){
		this.Action=action;
	}
}
