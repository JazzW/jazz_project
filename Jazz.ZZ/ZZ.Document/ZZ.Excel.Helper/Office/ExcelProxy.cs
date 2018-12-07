using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;

namespace ZZ.Excel.Helper.Office
{
    public class ExcelProxy
    {
         public string mFilename;
        public Microsoft.Office.Interop.Excel.Application app;
        public Microsoft.Office.Interop.Excel.Workbooks wbs;
        public Microsoft.Office.Interop.Excel.Workbook wb;

        public ExcelProxy()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public ExcelProxy(Microsoft.Office.Interop.Excel.Application App)
        {
            app = App;
            wbs = app.Workbooks;
            wb = app.ActiveWorkbook;
        }

        public void Create()//创建一个Microsoft.Office.Interop.Excel对象
        {
            app = new Microsoft.Office.Interop.Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(true);
        }
        public static ExcelProxy Open(string FileName)//打开一个Microsoft.Office.Interop.Excel文件
        {
            ExcelProxy proxy = new ExcelProxy();
            proxy.app = new Microsoft.Office.Interop.Excel.Application();
            proxy.wbs = proxy.app.Workbooks;
            proxy.wb = proxy.wbs.Add(FileName);
            proxy.mFilename = FileName;
            return proxy;
        }
       
        public Microsoft.Office.Interop.Excel.Worksheet GetSheet(string SheetName)
        //获取一个工作表
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName];
            return s;
        }
        public Microsoft.Office.Interop.Excel.Worksheet AddSheet(string SheetName)
        //添加一个工作表
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = SheetName;
            return s;
        }

        public System.Data.DataTable GetTable(String SheetName, int SCol, int SRow, int ColLength, int RowLength)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            for (int i = 0; i < ColLength; i++)
            {
                result.Columns.Add(i.ToString());
            }
            for (int j = 0; j < RowLength; j++)
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
            int rowindex = SRow-1;
            while (true)
            {
                if (rowindex > 200) break;
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
            int rowindex = toprow+1;
            while (true)
            {
                if (rowindex > 200) break;
                try
                {
                    if (sheet.Cells[rowindex-toprow, SCol-leftcol].Value.ToString() == startkey) srow = rowindex;
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
           var sheet= this.GetSheet(sheetName);
           int rowindex = toprow+1;
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

        public void DelSheet(string SheetName)//删除一个工作表
        {
            ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName]).Delete();
        }

        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(Microsoft.Office.Interop.Excel.Worksheet Sheet, string NewSheetName)//重命名一个工作表二
        {

            Sheet.Name = NewSheetName;

            return Sheet;
        }

        public void SetCellValue(string ws, int x, int y, object value)
        {

            GetSheet(ws).Cells[x, y] = value;
        }


        public void SetCellProperty(string wsn, int Startx, int Starty, int Endx, int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        {
            //name = "宋体";
            //size = 12;
            //color = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
            //HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;

            Microsoft.Office.Interop.Excel.Worksheet ws = GetSheet(wsn);
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;

            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }


        public void UniteCells(string ws, int x1, int y1, int x2, int y2)
        {
            GetSheet(ws).get_Range(GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]).Merge(Type.Missing);

        }

        public void InsertTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {

            for (int i = 0; i  <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j  <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(ws).Cells[startX+i, j + startY] = dt.Rows[i][j].ToString();

                }

            }

        }

        public void AddTable(System.Data.DataTable dt, string ws, int startX, int startY)
        {

            for (int i = 0; i  <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j  <= dt.Columns.Count - 1; j++)
                {

                    GetSheet(ws).Cells[i + startX, j + startY] = dt.Rows[i][j];

                }

            }

        }

        public void InsertPictures(string Filename, string ws)
        {

            GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        }

    
        public void InsertActiveChart(Microsoft.Office.Interop.Excel.XlChartType ChartType, string ws, int DataSourcesX1, int DataSourcesY1, int DataSourcesX2, int DataSourcesY2, Microsoft.Office.Interop.Excel.XlRowCol ChartDataType)
        {
            ChartDataType = Microsoft.Office.Interop.Excel.XlRowCol.xlColumns;
            wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                wb.ActiveChart.ChartType = ChartType;
                wb.ActiveChart.SetSourceData(GetSheet(ws).get_Range(GetSheet(ws).Cells[DataSourcesX1, DataSourcesY1], GetSheet(ws).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
                wb.ActiveChart.Location(Microsoft.Office.Interop.Excel.XlChartLocation.xlLocationAsObject, ws);
            }
        }
        
        public bool Save()
        {
            if (mFilename == "")
            {
                return false;
            }
            else
            {
                try
                {
                    wb.Save();
                    return true;
                }

                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public bool SaveAs(object FileName)
        //文档另存为
        {
            try
            {
                wb.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                return true;

            }

            catch (Exception ex)
            {
                return false;

            }
        }
        public void Close()
        //关闭一个Microsoft.Office.Interop.Excel对象，销毁对象
        {
            //wb.Save();
            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            wbs.Close();
            app.Quit();
            wb = null;
            wbs = null;
            app = null;
            GC.Collect();
        }


        public void dispose()
        {
            wb = null;
            wbs = null;
            app = null;
        }
    }
}
