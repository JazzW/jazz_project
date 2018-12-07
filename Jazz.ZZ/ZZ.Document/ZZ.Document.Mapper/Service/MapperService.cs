using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ZZ.Document.Mapper.Service
{
    /// <summary>
    /// Mapper核心服务
    /// </summary>
    public class MapperService
    {
        /// <summary>
        /// 执行文件映射任务
        /// </summary>
        /// <param name="MapTasks">多个映射任务,与参数files为一对一关系</param>
        /// <param name="Files">[源文件路径,目标文件路径,另存为文件路径]当长度为2时候直接保存至目标文件</param>
        public static void Map(List<Class.MapTask> MapTasks,List<String[]> Files)
        {
            if (Files.Count < MapTasks.Count) throw new Exception("task count is not equal files count!please check count of Mapper in config");
            int index = 0;
            foreach (var task in MapTasks)
            {
                ZZ.ILogger.LoggerFactory.getLogger().Info(string.Format("Tasks({0}/{1}):Task start running!", index+1, MapTasks.Count));
                if(Files[index].Length>=2 ){
                    if (task.SoureType == Class.MapFileType.excel)
                        task.SoureInfo = new Class.Excel.ExcelMapGoablInfo { FilePath = Files[index][0] };
                    else if (task.SoureType == Class.MapFileType.word)
                        task.SoureInfo = new Class.Word.WordMapGobalInfo { FilePath = Files[index][0] };

                    if (task.TargetType == Class.MapFileType.excel)
                        task.TargetInfo= new Class.Excel.ExcelMapGoablInfo { FilePath = Files[index][1] };
                    else if (task.TargetType == Class.MapFileType.word)
                        task.TargetInfo = new Class.Word.WordMapGobalInfo { FilePath = Files[index][1] };

                    if (Files[index].Length > 2)
                        task.TargetInfo.SaveAsPath = Files[index][2];

                    try
                    {
                        task.Start();
                        ZZ.ILogger.LoggerFactory.getLogger().Info(string.Format("Tasks({0}/{1}):Task run finish!", index + 1, MapTasks.Count));
                    }
                    catch(Exception ex){
                        ZZ.ILogger.LoggerFactory.getLogger().Error(string.Format("Tasks({0}/{1}):Task run failed!{2}", index + 1, MapTasks.Count,ex.Message));
                    }
                    task.Dispose();
                }

                index++;
            }
        }

        /// <summary>
        /// 通过XML文件生成映射任务
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        public static List<Class.MapTask> getMapTaskfromConfig(string filepath)
        {
            var result = new List<Class.MapTask>();
            var main=  XElement.Load(filepath);
            foreach(XElement el in main.Elements("Mapper"))
            {
                var task = new Class.MapTask() { Mappers=new List<Class.MapCollection>()};
                Class.MapFileType SourceType = (Class.MapFileType)Enum.Parse(typeof(Class.MapFileType), el.Attribute("source").Value);
                Class.MapFileType TargetType = (Class.MapFileType)Enum.Parse(typeof(Class.MapFileType), el.Attribute("target").Value);
                task.SoureType = SourceType;
                task.TargetType = TargetType;
                foreach (XElement _el in el.Elements("Map"))
                {
                    task.Mappers.Add(getMapFromXML(_el,SourceType,TargetType));
                }
                result.Add(task);
            }
            return result;
        }

        /// <summary>
        /// 从xml完成映射对子
        /// </summary>
        /// <param name="xle"></param>
        /// <param name="SourceType"></param>
        /// <param name="TargetType"></param>
        /// <returns></returns>
        private static Class.MapCollection getMapFromXML(XElement xle, Class.MapFileType SourceType,Class.MapFileType TargetType)
        {
            return new Class.MapCollection()
            {
                MapperType =xle.Attribute("type")==null?
                Class.MapType.insert:(Class.MapType)Enum.Parse(typeof(Class.MapType), xle.Attribute("type").Value),
                TargetMap = getMapItemfromXML(xle.Element("Target"),TargetType),
                SoureMap = getMapItemfromXML(xle.Element("Source"),SourceType),
                Log = xle.Attribute("desc") == null ? "" : xle.Attribute("desc").Value
            };
      
        }

        /// <summary>
        /// 从xml完成映射子项
        /// </summary>
        /// <param name="xle"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        private static Class.MapItem getMapItemfromXML(XElement xle, Class.MapFileType filetype)
        {
            if (xle == null) return null;
            if(filetype==Class.MapFileType.excel){
                switch(xle.Attribute("type").Value)
                {
                    case "text":
                        return new Class.Excel.ExcelMapCell
                        {
                            CellCol = int.Parse(xle.Attribute("column").Value),
                            CellRow = xle.Attribute("row") == null ? 0 : int.Parse(xle.Attribute("row").Value),
                            SheetName = xle.Attribute("sheet").Value,
                            StartKey =xle.Attribute("start-key") == null?null: xle.Attribute("start-key").Value,
                            LeftColumn = xle.Attribute("offset-left") == null ? 1 : int.Parse(xle.Attribute("offset-left").Value),
                            TopRow = xle.Attribute("offset-top") == null ? 0 : int.Parse(xle.Attribute("offset-top").Value)
                        };
                    case "table":
                        return new Class.Excel.ExcelMapTable
                        {
                            SheetName = xle.Attribute("sheet").Value,
                            CellCol = int.Parse(xle.Attribute("column").Value),
                            CellRow = xle.Attribute("row") == null ? 0 : int.Parse(xle.Attribute("row").Value),
                            ColumnLength = int.Parse(xle.Attribute("col-length").Value),
                            RowLength = xle.Attribute("row-length") == null ? 0 : int.Parse(xle.Attribute("row-length").Value),
                            LeftColumn = xle.Attribute("offset-left") == null ? 1 : int.Parse(xle.Attribute("offset-left").Value),
                            TopRow = xle.Attribute("offset-top") == null ? 0 : int.Parse(xle.Attribute("offset-top").Value),
                            StartKey = xle.Attribute("start-key") == null ? null : xle.Attribute("start-key").Value,
                        };
                    case "chart":
                        break;
                    case "value-table":
                        return new Class.MapTable
                        {
                            Table = Class.MapGoablCollection.getVal<System.Data.DataTable>(xle.Attribute("key").Value.ToString())
                        };
                    case "value-object":
                        return new Class.MapText
                        {
                            Text = Class.MapGoablCollection.getVal(xle.Attribute("key").Value.ToString()).ToString()
                        };
                    case "method":
                        break;
                }
            }
            else if (filetype == Class.MapFileType.word)
            {
                switch (xle.Attribute("type").Value)
                {
                    case "text":
                        return new Class.Word.WordMapText
                        {
                            BookMark=xle.Attribute("bookmark").Value,
                            MaxIndex = xle.Attribute("max-index") == null ? -1 : int.Parse(xle.Attribute("max-index").Value),
                            IfKey=xle.Attribute("if-key")==null?null:xle.Attribute("if-key").Value,
                            elseKey = xle.Attribute("else-key") == null ? null : xle.Attribute("else-key").Value
                        };
                    case "table":
                        return new Class.Word.WordMapTable
                        {
                           Key=int.Parse(xle.Attribute("key").Value),
                           MaxIndex = xle.Attribute("column-row") == null ? -1 : int.Parse(xle.Attribute("column-row").Value)
                        };
                    case "chart":
                        return new Class.Word.WordMapChart
                        {
                            Key = int.Parse(xle.Attribute("key").Value),
                            XIndex =xle.Attribute("x-index")==null?0:int.Parse(xle.Attribute("x-index").Value)
                        };
                    case "value":
                        break;
                    case "method":
                        break;
                }
            }
            return null;
 
        }
    }
}
