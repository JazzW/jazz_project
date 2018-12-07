package org.jazz.common.enitiy.attribute;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
/**
 * ���ݿ�ӳ��ʵ��ע��
 * @author wjc
 *
 */
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface DBModelAttribute {
	/**
	 * �������
	 * @return
	 */
	  public String TBName ();

	  /**
	   * �������
	   * @return
	   */
      public String TBMess () default "";

      /**
       * ��ʵ���ѯ�ֶ�
       * @return
       */
      public String ListCols () default "*";

      /**
       * ��һʵ���ѯ�ֶ�
       * @return
       */
      public String ItemCols() default "*";
      
}
