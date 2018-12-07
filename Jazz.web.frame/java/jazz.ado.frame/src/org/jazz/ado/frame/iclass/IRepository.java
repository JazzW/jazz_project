package org.jazz.ado.frame.iclass;

import java.util.List;

import org.jazz.common.web.config.TableConfig;
/**
 * ���ݿ���ʲ�ӿ�
 * @author Wangjc
 * @author 2018.10.10
 * @param <T> ���ݿ�ʵ��ӳ��class
 */
public interface IRepository<T> {
	/**
	 * ͨ��ɸѡ���ʵ���б�
	 * @param Config ͨ�ò�ѯ��׼{@link TableConfig}
	 * @return {@code List<T>} ʵ���б�
	 */
	 List<T> ISelectList(TableConfig Config) throws Exception;

	 /**
	  * ͨ��ɸѡ���ʵ���б���������֧�ַ�ҳ����
	  * @param Config ͨ�ò�ѯ��׼{@link TableConfig}
	  * @return {@code int} ��ѯ����ܳ���
	  */
     int ISelectListCount(TableConfig Config) throws Exception;

     /**
      * ͨ��ɸѡ��õ�һ��ʵ��
      * @param Config ͨ�ò�ѯ��׼{@link TableConfig}
      * @return {@code T} ʵ��
      */
     T ISelectFirst(TableConfig Config) throws Exception;

     /**
      * ������ʵ��
      * @param models {@code T������} ʵ���б�
      * @return {@code boolean} ʵ�ֽ����true-�ɹ���false-ʧ��
      */
     @SuppressWarnings("unchecked")
     boolean IInsert(T... models) throws Exception;

     /**
      * ���¶��ʵ��
      * @param models {@code T������} ʵ���б�
      * @return {@code boolean} ʵ�ֽ����true-�ɹ���false-ʧ��
      */
     @SuppressWarnings("unchecked")
     boolean IUpdate(T... models) throws Exception;
 
     /**
      * ɾ�����ʵ��
      * @param models {@code T������} ʵ���б�
      * @return {@code boolean} ʵ�ֽ����true-�ɹ���false-ʧ��
      */
     @SuppressWarnings("unchecked")
     boolean IDelete(T... models) throws Exception;


}
