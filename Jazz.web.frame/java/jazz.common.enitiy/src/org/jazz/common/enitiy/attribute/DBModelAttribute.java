package org.jazz.common.enitiy.attribute;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
/**
 * 数据库映射实体注解
 * @author wjc
 *
 */
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface DBModelAttribute {
	/**
	 * 表格名称
	 * @return
	 */
	  public String TBName ();

	  /**
	   * 表格描述
	   * @return
	   */
      public String TBMess () default "";

      /**
       * 多实体查询字段
       * @return
       */
      public String ListCols () default "*";

      /**
       * 单一实体查询字段
       * @return
       */
      public String ItemCols() default "*";
      
}
