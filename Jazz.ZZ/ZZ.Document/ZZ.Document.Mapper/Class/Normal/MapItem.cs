using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class
{
    /// <summary>
    /// 所有映射类的基类，源于[Source],[Target]
    /// </summary>
    public interface MapItem
    {
        /// <summary>
        /// 设置全局信息类的引用
        /// </summary>
        /// <param name="info"></param>
        void SetInfo(MapGobalInfo info);

        /// <summary>
        /// 获得全局信息类对象
        /// </summary>
        /// <returns></returns>
        MapGobalInfo GetInfo();

        /// <summary>
        /// 获得映射对象
        /// </summary>
        /// <returns></returns>
        Object Get();

        /// <summary>
        /// 根据映射地址实现设置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        void Set<T>(Object obj);

        /// <summary>
        /// 将该映射出来的对象插入到另一个映射类
        /// </summary>
        /// <param name="item"></param>
        void Insert(MapItem item);

        /// <summary>
        /// 将该映射出来的对象替换到另一个映射类
        /// </summary>
        /// <param name="item"></param>
        void Replace(MapItem item);

        /// <summary>
        /// 根据映射地址删除对象
        /// </summary>
        void Delete();

        /// <summary>
        /// 释放全局信息类的引用
        /// </summary>
        void Dispose();

        /// <summary>
        /// 映射类的文件类型[excel/word/none]
        /// </summary>
        /// <returns></returns>
        MapFileType Tpye();
    }

    /// <summary>
    /// 值映射基类，不存在映射地址以及文件全局信息类
    /// </summary>
    public abstract class MapVaItem:MapItem
    {
        public void SetInfo(MapGobalInfo info)
        {
        }

        public MapGobalInfo GetInfo() { return null; }

        public virtual Object Get()
        {
            return null;
        }

        public virtual void Set<T>(Object obj)
        {
           
        }

        public virtual void Insert(MapItem item) {
            item.Set<Object>(this.Get());
        }

        public virtual void Replace(MapItem item) { }

        public virtual void Delete() { }

        public virtual void Dispose()
        {
        }

        public MapFileType Tpye()
        {
            return MapFileType.none;
        }

        public static MapItem CreateMapItemFromDictory(string KeyName,Dictionary<string,object> dict,string valueType)
        {
            return null;
        }
    }
}
