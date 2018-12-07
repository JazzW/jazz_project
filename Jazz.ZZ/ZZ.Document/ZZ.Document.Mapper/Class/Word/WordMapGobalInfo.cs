using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class.Word
{
    public  class WordMapGobalInfo:MapGobalInfo
    {
        public int Model = 1;

        private ZZ.Excel.Helper.OpenXML.WordProxy _WordProxyOpenXml;

        private ZZ.Excel.Helper.Other.WordProxy _WordProxyDOCX;

        public ZZ.Excel.Helper.OpenXML.WordProxy getWordProxyOpenXml()
        {
            if (_WordProxyOpenXml == null)
            {
                _WordProxyOpenXml = ZZ.Excel.Helper.OpenXML.WordProxy.Open(this.FilePath
                    , string.IsNullOrWhiteSpace(this.SaveAsPath) ? null : this.SaveAsPath);
            }
            return _WordProxyOpenXml;
        }

        public ZZ.Excel.Helper.Other.WordProxy getWordProxyDOCX()
        {
            if (_WordProxyDOCX == null)
            {
                _WordProxyDOCX = ZZ.Excel.Helper.Other.WordProxy.Open
                    (this.FilePath);
            }
            return _WordProxyDOCX;
        }

        public override MapFileType MapFileType
        {
            get
            {
                return Class.MapFileType.word;
            }
        }

        public override T GetApp<T>()
        {
            return base.GetApp<T>();
        }

        public override void Dispose()
        {
            if (_WordProxyOpenXml != null)
            {
                _WordProxyOpenXml.CloseDoc();
            }
            if (_WordProxyDOCX != null)
            {
                if (string.IsNullOrWhiteSpace(this.SaveAsPath))
                {
                    _WordProxyDOCX.Save();
                }
                else
                {
                    _WordProxyDOCX.SaveAs(this.SaveAsPath);
                }

                _WordProxyDOCX.Dispose();
            }
        }
    }
}
