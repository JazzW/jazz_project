package org.jazz.common.enitiy.attribute;

import java.lang.annotation.*;

/**
 * 数据库实体字段注解
 * @author wjc
 *
 */
@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface DBItemAttribute {
	
	   public int Size() default 0;

       public boolean Null() default false;

       public boolean Check() default false;
       
       public boolean Show() default true;

       public String Display();

       public String ColName();
}
