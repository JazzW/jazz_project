using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZZ.Document.Mapper.Class.Word
{
    public class WordMapTable:WordMapItem
    {

        public DataTable Table { get { return (DataTable)this.Get(); } }

        public override object Get()
        {
            var info = (WordMapGobalInfo)GetInfo();
            if (info.Model == 1)
                return info.getWordProxyOpenXml().GetTabel(Key);
            return info.getWordProxyDOCX().GetTabel(Key);
        }

        public override void Set<T>(object obj)
        {
            base.Set<T>(obj);
        }


        public override void Insert(MapItem item)
        {
            switch (item.Tpye())
            {
                case MapFileType.excel:
                    item.Set<DataTable>(this.Table);
                    break;
                case MapFileType.word:
                    item.Set<DataTable>(this.Table);
                    break;
            }
        }

        public override void Replace(MapItem item)
        {
            base.Replace(item);
        }

        public override void Delete()
        {
            base.Delete();
        }
    }
}
