using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class.Word
{
    public  class WordMapChart:WordMapItem
    {
        public System.Data.DataTable DataSource { get; set; }

        public int XIndex { get; set; }

        public override object Get()
        {
            return DataSource;
        }

        public override void Set<T>(object obj)
        {
            WordMapGobalInfo Info = (WordMapGobalInfo)this.GetInfo();
            Info.getWordProxyOpenXml().UpdateChart((System.Data.DataTable)obj, this.XIndex, this.Key);
        }

        public override void Insert(MapItem item)
        {
            base.Insert(item);
        }


        public override void Replace(MapItem item)
        {
            base.Replace(item);
        }

        public override void Delete()
        {
            base.Delete();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
