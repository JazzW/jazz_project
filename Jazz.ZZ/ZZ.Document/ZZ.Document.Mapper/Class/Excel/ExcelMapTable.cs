using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZZ.Document.Mapper.Class.Excel
{
    /// <summary>
    /// excel 操做表的映射类,源于[type="table"]
    /// </summary>
    public  class ExcelMapTable:ExcelMapItem
    {
        /// <summary>
        /// 开始列序号,源于[column]，必填
        /// </summary>
        public int StartColumn { get { return this.CellCol; } }

        /// <summary>
        /// 开始行序号,源于[row],选填
        /// </summary>
        public int StartRow { get { return this.CellRow; } }

        /// <summary>
        /// 列长度，源于[col-length]，必填
        /// </summary>
        public int ColumnLength { get; set; }
        
        /// <summary>
        /// 行长度,源于[row-length],选填
        /// </summary>
        public int RowLength { get; set; }

        /// <summary>
        /// 映射出来的table对象
        /// </summary>
        public DataTable Table { get {return  (DataTable)this.Get(); } }

        public override object Get()
        {
            var info = (ExcelMapGoablInfo)GetInfo();
            if (info.Opening)
            {
                if (string.IsNullOrWhiteSpace(StartKey))
                {
                    return info.getExcelOffice().
                        GetTable(this.SheetName, this.StartColumn, this.StartRow, this.ColumnLength, this.RowLength);
                }
                else
                {
                    return info.getExcelOffice().
                        GetTable(this.SheetName, this.StartColumn, this.StartRow, this.ColumnLength, this.StartKey, "", this.LeftColumn, this.TopRow);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(StartKey))
                {
                    return info.getExcelOpenXMl().
                        GetTable(this.SheetName, this.StartColumn, this.StartRow, this.ColumnLength, this.RowLength);
                }
                else
                {
                    return info.getExcelOpenXMl().
                        GetTable(this.SheetName, this.StartColumn,this.StartRow, this.ColumnLength, this.StartKey,"",this.LeftColumn,this.TopRow);
                }
            }

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
