using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class
{
    /// <summary>
    /// 映射对子，源于[Map]
    /// </summary>
    public class MapCollection
    {
        /// <summary>
        /// 对子的源映射，源于[Source]
        /// </summary>
        public MapItem SoureMap { get; set; }

        /// <summary>
        /// 对子的目标映射，源于[Target]
        /// </summary>
        public MapItem TargetMap { get; set; }

        /// <summary>
        /// 映射方式,源于[type],默认insert
        /// </summary>
        public MapType MapperType{get;set;}

        /// <summary>
        /// 对子映射信息，源于[desc]
        /// </summary>
        public string Log { get; set; }
    }


}
