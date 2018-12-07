using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class.Excel
{
    /// <summary>
    /// excel 映射基类,源于[type]
    /// </summary>
    public  abstract class ExcelMapItem:MapItem
    {
        ExcelMapGoablInfo Info;

        public void SetInfo(MapGobalInfo info)
        {
            Info = (ExcelMapGoablInfo)info;
        }

        public MapGobalInfo GetInfo()
        {
            return Info;
        }

        /// <summary>
        /// 识别标识，源于[start-key]
        /// </summary>
        public string StartKey { get; set; }

        /// <summary>
        /// 对象所在sheet，源于[sheet]
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 列的左偏移量，源于[offset-left]
        /// </summary>
        public int LeftColumn { get; set; }

        /// <summary>
        /// 行的上偏移量，源于[offset-top]
        /// </summary>
        public int TopRow { get; set; }

        /// <summary>
        /// 对象的开始单元格的列数，源于[column]
        /// </summary>

        public int CellCol { get; set; }

        /// <summary>
        /// 对象的开始单元格的行数，源于[row]
        /// </summary>
        public int CellRow { get; set; }

        public string Address
        {
            get { return null; }
            set { }
        }

        public MapFileType Tpye()
        {
            return MapFileType.excel;
        }

        public virtual Object Get() { return null; }

        public virtual void Set<T>(Object obj) {
            Type t = typeof(T);
            if (Info.Opening)
            {
                if (t == typeof(object))
                {
                    Info.getExcelOffice().GetSheet(this.SheetName).Cells[CellRow, CellCol].Value = obj;
                }
                else if (t == typeof(System.Data.DataTable))
                {
                    Info.getExcelOffice().AddTable((System.Data.DataTable)obj, this.SheetName, this.CellCol, this.CellRow);
                }
            }
            else
            {
                if (t == typeof(object))
                {
                    Info.getExcelOpenXMl().GetSheet(this.SheetName).Cells[CellRow, CellCol].Value = obj;
                }
                else if (t == typeof(System.Data.DataTable))
                {
                    Info.getExcelOpenXMl().SetTable(this.SheetName, (System.Data.DataTable)obj, this.CellCol, this.CellRow);
                }
            }
        }

        public virtual void Insert(MapItem item) 
        {
     
        }

        public virtual void Replace(MapItem item) { }

        public virtual void Delete() { }

        public virtual void Dispose()
        {
            this.Info = null;
        }
    }
}
