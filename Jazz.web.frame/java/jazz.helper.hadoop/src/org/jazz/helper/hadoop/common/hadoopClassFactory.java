package org.jazz.helper.hadoop.common;

import org.apache.hadoop.io.IntWritable;

import java.lang.reflect.Array;
import java.util.Map;

import org.apache.hadoop.io.*;

public class hadoopClassFactory {
	public static Class<?> getClass(Class<?> cls){
		if(cls==int.class){
			return IntWritable.class;
		}
		else if(cls==boolean.class){
			return BooleanWritable.class;
		}
		else if(cls==String.class){
			return Text.class;
		}
		else if(cls==int.class){
			return IntWritable.class;
		}
		else if(cls==long.class){
			return LongWritable.class;
		}
		else if(cls==double.class){
			return DoubleWritable.class;
		}
		else if(cls==Array.class){
			return ArrayWritable.class;
		}
		else if(cls==Map.class){
			return MapWritable.class;
		}
		else if(cls==Byte.class){
			return ByteWritable.class;
		}
		else if(cls==null){
			return NullWritable.class;
		}
		else if(cls==Object.class)
		{
		  return ObjectWritable.class;
		}
		else{
			return cls;
		}
		
	}
}
