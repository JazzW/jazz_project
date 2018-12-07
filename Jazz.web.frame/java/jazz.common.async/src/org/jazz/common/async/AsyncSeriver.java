package org.jazz.common.async;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.FutureTask;

public class AsyncSeriver {
	public static <T> Future<T> createTask(Callable<T> seriver)
	{
		return new FutureTask<T>(seriver);
	}
	
	public static <T> T runAsync(Callable<T> seriver) 
			throws InterruptedException, ExecutionException
	{
		ExecutorService es = Executors.newFixedThreadPool(1);
		Future<T> res= es.submit(seriver);
		es.shutdown();
		return res.get();
	}
	
	@SafeVarargs
	public static <T> List<T> runAsync(Callable<T>... serivers) 
			throws InterruptedException, ExecutionException
	{
		ExecutorService es = Executors.newCachedThreadPool();
		List<Future<T>> res= es.invokeAll(Arrays.asList(serivers));
		es.shutdown();
		List<T> output=new ArrayList<T>();
		for(Future<T> task :res){
			output.add(task.get());
		}
		return output;
	}
	
	public static <T> List<T> runAsync(List<? extends Callable<T>> serivers) 
			throws InterruptedException, ExecutionException
	{
		ExecutorService es = Executors.newCachedThreadPool();
		List<Future<T>> res= es.invokeAll(serivers);
		es.shutdown();
		List<T> output=new ArrayList<T>();
		for(Future<T> task :res){
			output.add(task.get());
		}
		return output;
	}
	
	public static <T> List<T> runAsync(Collection<? extends Callable<T>> serivers) 
			throws InterruptedException, ExecutionException
	{
		ExecutorService es = Executors.newCachedThreadPool();
		List<Future<T>> res= es.invokeAll(serivers);
		es.shutdown();
		List<T> output=new ArrayList<T>();
		for(Future<T> task :res){
			output.add(task.get());
		}
		return output;
	}
	
}
