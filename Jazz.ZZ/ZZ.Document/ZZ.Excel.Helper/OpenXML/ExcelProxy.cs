using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using excel=OfficeOpenXml;

namespace ZZ.Excel.Helper.OpenXML
{
    public class ExcelProxy:IDisposable
    {

        excel.ExcelPackage _excel;

        public static ExcelProxy Open(String Path)
        {
            ExcelProxy proxy=new ExcelProxy();
            System.IO.FileInfo file = new System.IO.FileInfo(Path);
            excel.ExcelPackage excel = new excel.ExcelPackage(file);
            proxy._excel=excel;
            return proxy;

        }

        public void Dispose(){
           _excel.Dispose();
        }

        public excel.ExcelWorksheet GetSheet(String Name)
        {
           return _excel.Workbook.Worksheets[Name];
        }

        public excel.ExcelWorksheet GetSheet(int index)
        {
            return _excel.Workbook.Worksheets[index];
        }

        public System.Data.DataTable GetTable(String SheetName, int SCol, int SRow, int ColLength, int RowLength)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            for (int i = 0; i < ColLength; i++)
            {
                result.Columns.Add(i.ToString());
            }
            for (int j = 0; j < RowLength;j++)
            {
                System.Data.DataRow _row = result.NewRow();
                for (int i = 0; i < ColLength; i++)
                {
                    string txt = "";
                    try
                    {
                        txt = this.GetSheet(SheetName).Cells[SRow + j, SCol + i].Text;
                    }
                    catch { }
                    _row[i] = txt;
                }
                result.Rows.Add(_row);
            }

            return result;
        }

        public System.Data.DataTable GetTable(String SheetName, int SCol, int SRow, int ColLength, string startkey, string endkey = "", int leftcol = 1, int toprow = 0)
        {
            if (SRow == 0)
                return GetTable(SheetName, SCol, ColLength, startkey, endkey,leftcol,toprow);
            var sheet = this.GetSheet(SheetName);
            int srow = SRow;
            int rowlength = 0;
            int rowindex = SRow - 1;
            while (true)
            {
                if (rowindex > 200) break;
                //try
                //{
                //    if (sheet.Cells[rowindex, SCol].Value.ToString() == startkey) srow = rowindex + 2;
                //}
                //catch { }
                try
                {
                    if (srow > 0 && sheet.Cells[rowindex, SCol].Value.ToString() == endkey) rowlength = rowindex - srow;
                }
                catch
                {
                    rowlength = rowindex - srow;
                }
                if (rowlength > 0) break;
                rowindex++;
            }

            System.Data.DataTable result = new System.Data.DataTable();
            for (int i = 0; i < ColLength; i++)
            {
                result.Columns.Add(i.ToString());
            }
            for (int j = 0; j < rowlength; j++)
            {
                System.Data.DataRow _row = result.NewRow();
                for (int i = 0; i < ColLength; i++)
                {
                    string txt = "";
                    try
                    {
                        txt = this.GetSheet(SheetName).Cells[srow + j, SCol + i].Text;
                    }
                    catch { }
                    _row[i] = txt;
                }
                result.Rows.Add(_row);
            }

            return result;
        }

        public System.Data.DataTable GetTable(String SheetName, int SCol, int ColLength, string startkey, string endkey = "", int leftcol = 1, int toprow = 0)
        {
            var sheet = this.GetSheet(SheetName);
            int srow = 0;
            int rowlength = 0;
            int rowindex =toprow+ 1;
            while (true)
            {
                if (rowindex > 200) break;
                try
                {
                    if (sheet.Cells[rowindex-toprow, SCol - leftcol].Value.ToString() == startkey) srow = rowindex;
                }
                catch { }
                try
                {
                    if (srow > 0 && sheet.Cells[rowindex, SCol].Value.ToString() == endkey) rowlength = rowindex - srow;
                }
                catch
                {
                    rowlength = rowindex - srow;
                }
                if (rowlength > 0) break;
                rowindex++;
            }

            System.Data.DataTable result = new System.Data.DataTable();
            for (int i = 0; i < ColLength; i++)
            {
                result.Columns.Add(i.ToString());
            }
            for (int j = 0; j < rowlength; j++)
            {
                System.Data.DataRow _row = result.NewRow();
                for (int i = 0; i < ColLength; i++)
                {
                    string txt = "";
                    try
                    {
                        txt = this.GetSheet(SheetName).Cells[srow + j, SCol + i].Text;
                    }
                    catch { }
                    _row[i] = txt;
                }
                result.Rows.Add(_row);
            }

            return result;
        }

        public object GetValue(string sheetName, int Col, string startkey,int leftcol=1,int toprow=0)
        {
            var sheet = this.GetSheet(sheetName);
            int rowindex =toprow+ 1;
            int row = 0;
            while (true)
            {

                if (rowindex > 200) break;
                try
                {
                    if (sheet.Cells[rowindex-toprow, Col - leftcol].Value.ToString() == startkey) row = rowindex;
                }
                catch { }
                if (row > 0) break;
                rowindex++;
            }
            return sheet.Cells[row, Col].Text;
        }


        public void SetTable(string SheetName, System.Data.DataTable table, int SCol, int SRow)
        {
            for (int j = 0; j <table.Rows.Count; j++)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    object val = table.Rows[j][i];
                    try
                    {
                        val = int.Parse(val.ToString());
                    }
                    catch { }
                    this.GetSheet(SheetName).Cells[SRow + j, SCol + i].Value = val;
                }
            }
        }

        public void Save()
        {
            _excel.Save();
        }

        public void SaveAs(string path)
        {
            _excel.SaveAs(new System.IO.FileInfo(path));
        }
    }
}
