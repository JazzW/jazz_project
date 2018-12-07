using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class
{
    /// <summary>
    /// 文件全局信息类抽象类
    /// </summary>
    public abstract class MapGobalInfo
    {
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 另存地址
        /// </summary>
        public string SaveAsPath { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileType { get { return this.FileName.Split('.').LastOrDefault(); } }

        /// <summary>
        /// 文件类型
        /// </summary>
        public virtual MapFileType MapFileType { get { return Class.MapFileType.excel; } }

        /// <summary>
        /// 获得App程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T GetApp<T>() { return default(T); }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public virtual void Dispose() { }

    }
}
