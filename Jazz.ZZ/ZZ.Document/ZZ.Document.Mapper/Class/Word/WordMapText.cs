using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class.Word
{
    public class WordMapText:WordMapItem
    {
        public String Text
        {
            get { return ""; }
        }

        public string IfKey { get; set; }

        public string elseKey { get; set; }

        public override void Set<T>(Object obj)
        {
            base.Set<T>(obj);
        }

        public override object Get()
        {
            var info = (WordMapGobalInfo)this.GetInfo();
            if(info.Model==1)
            {
                return info.getWordProxyOpenXml().GetText(this.BookMark);
            }
            else
            {
                return info.getWordProxyDOCX().GetText(this.BookMark);

            }
        }

        public override void Delete()
        {
            var info = (WordMapGobalInfo)this.GetInfo();
            if (info.Model == 1)
            {
                if (IfKey != null)
                {
                    if (MapGoablCollection.getVal<Boolean>(IfKey))
                    {
                        info.getWordProxyOpenXml().delete(this.BookMark, this.MaxIndex);
                    }
                }
                else if (elseKey != null)
                {
                    if (!MapGoablCollection.getVal<Boolean>(elseKey))
                    {
                        info.getWordProxyOpenXml().delete(this.BookMark, this.MaxIndex);
                    }
                }
                else
                {
                    info.getWordProxyOpenXml().delete(this.BookMark, this.MaxIndex);
                }
            }
        }

    }
}
