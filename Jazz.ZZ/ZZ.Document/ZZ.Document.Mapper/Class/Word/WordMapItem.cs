using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class.Word
{
    public class WordMapItem:MapItem
    {
        

        WordMapGobalInfo Info;

        public string BookMark { get; set; }

        public int Key { get; set; }

        public int MaxIndex { get; set; }

        public void SetInfo(MapGobalInfo info)
        {
            Info = (WordMapGobalInfo)info;
        }

        public MapGobalInfo GetInfo() { return Info; }

        public virtual Object Get()
        {
            return null;
        }

        public virtual void Set<T>(Object obj) 
        {
            Type t = typeof(T);
            if (Info.Model == 1)
            {
                if (t == typeof(object))
                {
                    Info.getWordProxyOpenXml().InsertText(this.BookMark, obj.ToString(),this.MaxIndex);
                }
                else if (t == typeof(System.Data.DataTable))
                {
                    Info.getWordProxyOpenXml().UpdateTable((System.Data.DataTable)obj,this.Key,this.MaxIndex);
                }
            }
            else
            {
                if (t == typeof(object))
                {
                    Info.getWordProxyDOCX().InsertText(this.BookMark, obj.ToString());
                }
                else if (t == typeof(System.Data.DataTable))
                {
                    Info.getWordProxyDOCX().InsetTable(this.Key, (System.Data.DataTable)obj);
                }
            }
        }

        public virtual void Insert(MapItem item) { }

        public virtual void Replace(MapItem item) { }

        public virtual void Delete() { }

        public virtual void Dispose()
        {
            Info = null;
        }

        public MapFileType Tpye() 
        {
            return MapFileType.word;
        }
    }
}
