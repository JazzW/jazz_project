package org.jazz.common.aop;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * Aop注解，实现方法的aop
 * @author Wangjc
 * @author 2018.10.10
 */
@Target({ElementType.METHOD,ElementType.TYPE})
@Retention(RetentionPolicy.RUNTIME)
public @interface AopAttribute {
	/**
	 * {@code Class<? extends AopSeriverInterface>} 通过Aop服务
	 * @return {@code Class<? extends AopSeriverInterface>}
	 */
	public	Class<? extends AopSeriverInterface> serive();
	
	/**
	 * 对应class，method，field的描述
	 * @return {@code String}
	 */
	public String desc();
	
}
