package org.jazz.common.enitiy.attribute;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/***
 * ���ݿ�ӳ��ʵ���ȫ�������ֶ�ע��
 * @author wjc
 *
 */
@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface SearchKeyAttribute {

}
