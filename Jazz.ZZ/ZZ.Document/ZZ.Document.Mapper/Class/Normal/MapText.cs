using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class
{
    /// <summary>
    /// 值映射，的字符映射，源于[value-object]
    /// </summary>
    public class MapText : MapVaItem
    {
        public string Text { get; set; }

        public override object Get()
        {
            return this.Text;
        }

        public static MapItem CreateMapText(MapFileType type,params Object[] Args)
        {
            switch (type)
            {
                case MapFileType.excel:
                    return new Excel.ExcelMapCell
                    {
                        CellCol = int.Parse(Args[0].ToString()),
                        CellRow = int.Parse(Args[1].ToString())
                    };
                case MapFileType.word:
                    return new Word.WordMapText
                    {
                        BookMark = Args[0].ToString()
                    };
                default:
                    return new MapText{
                        Text=Args[0].ToString()
                    };
            }
        }
    }
}
