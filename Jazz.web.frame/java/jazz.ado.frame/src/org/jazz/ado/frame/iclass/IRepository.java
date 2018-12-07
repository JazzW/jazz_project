package org.jazz.ado.frame.iclass;

import java.util.List;

import org.jazz.common.web.config.TableConfig;
/**
 * 数据库访问层接口
 * @author Wangjc
 * @author 2018.10.10
 * @param <T> 数据库实体映射class
 */
public interface IRepository<T> {
	/**
	 * 通过筛选获得实体列表
	 * @param Config 通用查询标准{@link TableConfig}
	 * @return {@code List<T>} 实体列表
	 */
	 List<T> ISelectList(TableConfig Config) throws Exception;

	 /**
	  * 通过筛选获得实体列表数量，不支持分页设置
	  * @param Config 通用查询标准{@link TableConfig}
	  * @return {@code int} 查询结果总长度
	  */
     int ISelectListCount(TableConfig Config) throws Exception;

     /**
      * 通过筛选获得第一个实体
      * @param Config 通用查询标准{@link TableConfig}
      * @return {@code T} 实体
      */
     T ISelectFirst(TableConfig Config) throws Exception;

     /**
      * 插入多个实体
      * @param models {@code T。。。} 实体列表
      * @return {@code boolean} 实现结果：true-成功，false-失败
      */
     @SuppressWarnings("unchecked")
     boolean IInsert(T... models) throws Exception;

     /**
      * 更新多个实体
      * @param models {@code T。。。} 实体列表
      * @return {@code boolean} 实现结果：true-成功，false-失败
      */
     @SuppressWarnings("unchecked")
     boolean IUpdate(T... models) throws Exception;
 
     /**
      * 删除多个实体
      * @param models {@code T。。。} 实体列表
      * @return {@code boolean} 实现结果：true-成功，false-失败
      */
     @SuppressWarnings("unchecked")
     boolean IDelete(T... models) throws Exception;


}
