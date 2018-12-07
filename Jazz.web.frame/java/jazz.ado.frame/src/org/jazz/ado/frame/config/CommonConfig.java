package org.jazz.ado.frame.config;
import org.jazz.common.enitiy.EnitiyManager;

public class CommonConfig {
	public static void set(boolean OpenEnitiyCache){
		EnitiyManager.useCache=OpenEnitiyCache;
	}
}	
