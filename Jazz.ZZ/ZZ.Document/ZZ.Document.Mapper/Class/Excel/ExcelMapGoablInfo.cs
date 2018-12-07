using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using excel=Microsoft.Office.Interop.Excel;

namespace ZZ.Document.Mapper.Class.Excel
{
    /// <summary>
    /// excel文件的全局类
    /// </summary>
    public class ExcelMapGoablInfo:MapGobalInfo
    {
        private Boolean _Opening=false;

        public Boolean Opening
        {
            get {
                if (OpeningApp != null)
                    _Opening = this.IsFileInUse();
                return _Opening; 
            }
        }

        public static Microsoft.Office.Interop.Excel.Application OpeningApp;

        private ZZ.Excel.Helper.OpenXML.ExcelProxy _ExcelOpenXMl;

        private ZZ.Excel.Helper.Office.ExcelProxy _ExcelOffice;

        public ZZ.Excel.Helper.OpenXML.ExcelProxy getExcelOpenXMl() {
    
            if (_ExcelOpenXMl == null)
            {
                _ExcelOpenXMl = ZZ.Excel.Helper.OpenXML.ExcelProxy.Open(this.FilePath);
            }
            return _ExcelOpenXMl;
            
        }

        public ZZ.Excel.Helper.Office.ExcelProxy getExcelOffice() {
            if (_ExcelOffice == null)
            {
                _ExcelOffice = new ZZ.Excel.Helper.Office.ExcelProxy(OpeningApp);
            }
            return _ExcelOffice;
        }

        public override MapFileType MapFileType
        {
            get
            {
                return MapFileType.excel;
            }
        }

        public override T GetApp<T>()
        {
            return base.GetApp<T>();
        }

        public override void Dispose()
        {
            if (_ExcelOpenXMl != null)
            {
                if (string.IsNullOrWhiteSpace(this.SaveAsPath))
                {
                    _ExcelOpenXMl.Save();
                }
                else
                {
                    _ExcelOpenXMl.SaveAs(this.SaveAsPath);
                }
                _ExcelOpenXMl.Dispose();
            }
            if (_ExcelOffice != null)
                _ExcelOffice.dispose();
            _ExcelOpenXMl = null;
            _ExcelOffice = null;
             OpeningApp = null;
        }

        public  bool IsFileInUse()  
        {  
            bool inUse = true;  
  
            FileStream fs = null;  
            try  
            {  
  
                fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read,  
  
                FileShare.None);  
  
                inUse = false;  
            }  
            catch  
            {   
            }  
            finally  
            {  
                if (fs != null)  
  
                    fs.Close();  
            }  
            return inUse;//true表示正在使用,false没有使用  
        }
    }
}
