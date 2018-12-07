using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class
{
    /// <summary>
    /// 映射任务，源于[Mapper]
    /// </summary>
    public class MapTask : IDisposable
    {
        /// <summary>
        /// 源映射的全局信息类
        /// </summary>
        public Class.MapGobalInfo SoureInfo { get; set; }

        /// <summary>
        /// 目标映射的全局信息类
        /// </summary>
        public Class.MapGobalInfo TargetInfo { get; set; }

        /// <summary>
        /// 源映射文件类型
        /// </summary>
        public Class.MapFileType SoureType { get; set; }

        /// <summary>
        /// 目标映射文件类型
        /// </summary>
        public Class.MapFileType TargetType { get; set; }

        /// <summary>
        /// 映射对子集合
        /// </summary>
        public List<MapCollection> Mappers { get; set; }

        /// <summary>
        /// 开始任务
        /// </summary>
        public void Start()
        {
            int index = 1;
            foreach (var map in Mappers)
            {
                ZZ.ILogger.LoggerFactory.getLogger().Info(string.Format("Map({0}/{1}):{2}",index,Mappers.Count,map.Log));
                map.TargetMap.SetInfo(TargetInfo);
                if (map.SoureMap != null)
                    map.SoureMap.SetInfo(SoureInfo);
                try
                {
                    switch (map.MapperType)
                    {
                        case MapType.insert:
                            map.SoureMap.Insert(map.TargetMap);
                            break;
                        case MapType.replace:
                            map.SoureMap.Replace(map.TargetMap);
                            break;
                        case MapType.delete:
                            map.TargetMap.Delete();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ZZ.ILogger.LoggerFactory.getLogger().Error(string.Format("Map({0}/{1}):{2}", index, Mappers.Count, ex.Message));
                }
                index++;
            }
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void Dispose()
        {
            
            foreach (var map in Mappers)
            {
                if(map.SoureMap!=null)map.SoureMap.Dispose();
                if(map.TargetMap!=null)map.TargetMap.Dispose();
            }
            Mappers.Clear();
            this.SoureInfo.Dispose();
            this.TargetInfo.Dispose();
        }
    }
}
