package org.jazz.common.enitiy.attribute;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/**
 * 数据库映射实体主键注解
 * @author wjc
 *
 */
@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface KeyAttribute {
	/**
	 * 是否为自增主键
	 * @return
	 */
	public boolean IdenityKey();
	
}
