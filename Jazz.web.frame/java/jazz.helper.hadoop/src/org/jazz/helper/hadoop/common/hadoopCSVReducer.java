package org.jazz.helper.hadoop.common;

import java.io.IOException;

import org.apache.hadoop.io.IntWritable;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Reducer;

public class hadoopCSVReducer extends Reducer<Text,IntWritable,Text,IntWritable> {
	@Override
    protected void reduce(Text key, Iterable<IntWritable> values, Context context) throws IOException, InterruptedException {
        //定义一个计数器
        int count = 0;
        //遍历一组迭代器，把每一个数量1累加起来就构成了单词的总次数

        for(IntWritable value:values){
            count +=value.get();
        }

        //把最终的结果输出
        context.write(key,new IntWritable(count));
    }
}
