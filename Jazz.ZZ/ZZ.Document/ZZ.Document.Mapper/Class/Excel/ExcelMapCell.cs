using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class.Excel
{
    /// <summary>
    /// 操做单个单元格映射类,源于[type="text"]
    /// </summary>
    public class ExcelMapCell:ExcelMapItem
    {
        public object _value;

        public object Value { get {
            if (_value == null)
                _value = this.Get();
            return _value;
        }  }

        public override void Set<T>(Object obj)
        {
            base.Set<T>(obj);
        }

        public override object Get()
        {
            var info = (ExcelMapGoablInfo)GetInfo();
            if (this.StartKey == null)
            {
                if (info.Opening)
                {
                    return info.getExcelOffice().GetSheet(this.SheetName).Cells[CellRow, CellCol].Text;
                }
                else
                    return info.getExcelOpenXMl().GetSheet(this.SheetName).Cells[CellRow, CellCol].Text;
            }
            else
            {
                if (info.Opening)
                {
                    return info.getExcelOffice().GetValue(SheetName, CellCol, StartKey,LeftColumn);
                }
                else
                    return info.getExcelOpenXMl().GetValue(SheetName, CellCol, StartKey,LeftColumn);
            }
        }

        public override void Insert(MapItem item)
        {
            switch (item.Tpye())
            {
                case MapFileType.excel:
                    item.Set<object>(this.Value);
                    break;
                case MapFileType.word:
                    item.Set<object>(this.Value);
                    break;
            }
        }

        public override void Replace(MapItem item)
        {
            item.Delete();
            item.Insert(item);
        }

        public override void Delete()
        {
            base.Delete();
        }
    }
}
