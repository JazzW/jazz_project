package org.jazz.common.aop;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * Aopע�⣬ʵ�ַ�����aop
 * @author Wangjc
 * @author 2018.10.10
 */
@Target({ElementType.METHOD,ElementType.TYPE})
@Retention(RetentionPolicy.RUNTIME)
public @interface AopAttribute {
	/**
	 * {@code Class<? extends AopSeriverInterface>} ͨ��Aop����
	 * @return {@code Class<? extends AopSeriverInterface>}
	 */
	public	Class<? extends AopSeriverInterface> serive();
	
	/**
	 * ��Ӧclass��method��field������
	 * @return {@code String}
	 */
	public String desc();
	
}
