using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Doc = DocumentFormat.OpenXml;
using Chart = DocumentFormat.OpenXml.Drawing.Charts;
using Pack = DocumentFormat.OpenXml.Packaging;
using Sheet = DocumentFormat.OpenXml.Spreadsheet;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace ZZ.Excel.Helper.OpenXML
{
    public class WordProxy
    {
        private Pack.WordprocessingDocument _wordProcDoc;

        private Pack.MainDocumentPart _mainDocPart;

        private string _filepath;

 
        public void OpenDoc(string filePath,string backupPath=null)
        {
            if (backupPath == null)
                _filepath = filePath;
            else
            {
                _filepath = backupPath;
                File.Copy(filePath, _filepath);
            }
            _wordProcDoc = Pack.WordprocessingDocument.Open(_filepath, true);
            _mainDocPart = _wordProcDoc.MainDocumentPart;
        }

        public static WordProxy Open(string filePath, string backupPath = null)
        {
          
            WordProxy proxy = new WordProxy();
           
            proxy.OpenDoc(filePath,backupPath);
            return proxy;
        }

        public void OpenDoc(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            _wordProcDoc = Pack.WordprocessingDocument.Open(stream, true);
            _mainDocPart = _wordProcDoc.MainDocumentPart;
        }


        public FileStream GetStream()
        {
            FileStream stream = new FileStream(_filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
           
            return stream;
        }

        public byte[] GetByte()
        {
            byte[] myByte = null;
            using (FileStream stream = new FileStream(_filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                myByte = new byte[stream.Length];
                stream.Read(myByte, 0, myByte.Length);
            }
            return myByte;

        }

        public string GetHtml()
        {
            string html= DocxToHtml.GetHtml(_mainDocPart);
            return html;
        }

        public void CloseDoc()
        {
            _wordProcDoc.Close();
            _wordProcDoc.Dispose();
        }

        public void delete(string bookmark, int max = -1)
        {
            if (max > 0)
            {
                for (int i = 1; i <= max; i++)
                {
                    try
                    {
                        deletex(bookmark + "_" + i.ToString());

                    }
                    catch { }
                }
            }
            else
            {
                deletex(bookmark);
            }
        }

        private void deletex(string bookmark)
        {

            
            var bms = _mainDocPart.RootElement.Descendants<Word.BookmarkStart>().FirstOrDefault(e => e.Name == bookmark);
            var bme = _mainDocPart.RootElement.Descendants<Word.BookmarkEnd>().FirstOrDefault(e => e.Id.Value == bms.Id.Value);
            string bmid = bme.Id.Value;
            Doc.OpenXmlElement bm = bms;
            if(bme.Parent.LocalName!=bms.Parent.LocalName)
            {
                bm= getParent(bms, bme.Parent.LocalName);
            }

            Doc.OpenXmlElement next = bm.NextSibling();
            List<Doc.OpenXmlElement> del = new List<Doc.OpenXmlElement>();
            del.Add(bm);
            while (next != null)
            {
                if (next.LocalName == "bookmarkEnd")
                {
                    try
                    {
                        if (((Word.BookmarkEnd)next).Id.Value == bmid)
                        {
                            foreach (var d in del)
                                d.Remove();
                            break;
                        }
                    }
                    catch { del.Add(next); }
                }
                else
                {
                    del.Add(next);
                }
                next = next.NextSibling();
            }

            
        }

        private Doc.OpenXmlElement getParent(Doc.OpenXmlElement ele,string parentlocalname)
        {
            Doc.OpenXmlElement parent=ele.Parent;
            while (parent.Parent.LocalName != parentlocalname)
            {
                parent = parent.Parent;
            }
            return parent;
        }


        public string GetText(string bookmark)
        {
            var bm = _mainDocPart.RootElement.Descendants<Word.BookmarkStart>().Where(e => e.Name == bookmark).FirstOrDefault();
            Word.Run bookmarkText = bm.NextSibling<Word.Run>();
            return bookmarkText.GetFirstChild<Word.Text>().Text;
        }

        public DataTable GetTabel(int tbIndex)
        {
            DataTable result = new DataTable();
            Word.Table tb = _mainDocPart.RootElement.Descendants<Word.Table>().ToArray()[tbIndex];
            Word.TableRow[] rows = tb.Descendants<Word.TableRow>().ToArray();
            Word.TableRow headerRow = rows[0];
            Word.TableCell[] hcells = headerRow.Descendants<Word.TableCell>().ToArray();
            foreach (var cell in hcells)
            {
                result.Columns.Add(cell.InnerText);
            }
            for (int i = 1; i < rows.Length; i++)
            {
                DataRow row = result.NewRow();
                Word.TableCell[] cells = rows[i].Descendants<Word.TableCell>().ToArray();
                for (int j = 0; j < result.Columns.Count; j++)
                {
                    row[j] = cells[j].InnerText;
                }
                result.Rows.Add(row);
            }
            return result;
        }

        public void InsertText(string bookmark, string txt, int max = -1)
        {
            if (max > 0)
            {
                for (int i = 1; i <= max; i++)
                {
                    try
                    {
                        InsertTxt(bookmark + "_" + i.ToString(), txt);

                    }
                    catch { }
                }
            }
            else
            {
                InsertTxt(bookmark, txt);
            }
        }

        public void InsertTxt(string bookmark, string txt)
        {
            try
            {
                var bm = _mainDocPart.RootElement.Descendants<Word.BookmarkStart>().Where(e => e.Name == bookmark).FirstOrDefault();
                Word.Run bookmarkText = bm.NextSibling<Word.Run>();
                if (bookmarkText != null)  // if the bookmark has text replace it
                {
                    string[] ts = txt.Split(new string[] { @"\r\n" }, StringSplitOptions.None);
                    Word.Text tx= bookmarkText.GetFirstChild<Word.Text>();
                    tx.Text=ts[0];
                    var parent = bm.Parent;
                    for (int i = 1; i < ts.Length; i++)
                    {
                        Word.Run run= new Word.Run(new Word.RunProperties(bookmarkText.RunProperties.OuterXml), new Word.Break());
                        while (ts[i].IndexOf(@"\tab") == 0)
                        {
                            ts[i]=ts[i].Remove(0, 4);
                            run.Append(new Word.TabChar());
                        }
                        run.Append(new Word.Text(ts[i]));
                        parent.Append(run);
                    }
                }
                else  // otherwise append new text immediately after it
                {
                    var parent = bm.Parent;   // bookmark's parent element

                    Word.Text text = new Word.Text(txt);
                    Word.Run run = new Word.Run(new Word.RunProperties());
                    run.Append(text);
                    parent.Append(run);
                }
            }
            catch
            {

            }
        }

        public void InsertTable(string bookmark, DataTable dt)
        {
            try
            {
                var bm = _mainDocPart.RootElement.Descendants<Word.BookmarkStart>().Where(e => e.Name == bookmark).FirstOrDefault().Parent;
                Word.Text text = bm.Descendants<Word.Text>().FirstOrDefault();
                if (text != null)
                {
                    text.Text = "";
                }

                Word.TableGrid grid = new Word.TableGrid();
                int maxColNum = dt.Columns.Count;
                for (int i = 0; i < maxColNum; i++)
                {
                    grid.Append(new Word.TableGrid());
                }

                Word.TableProperties tbProp = new Word.TableProperties(
                   new Word.TableBorders(
                       new Word.TopBorder()
                       {
                           Val = new Doc.EnumValue<Word.BorderValues>(Word.BorderValues.Single),
                           Size = 2,
                           Color = new Doc.StringValue() { Value = System.Drawing.Color.Black.Name }
                       },
                       new Word.BottomBorder() { Val = new Doc.EnumValue<Word.BorderValues>(Word.BorderValues.Single), Size = 2 },
                       new Word.LeftBorder() { Val = new Doc.EnumValue<Word.BorderValues>(Word.BorderValues.Single), Size = 2 },
                       new Word.RightBorder() { Val = new Doc.EnumValue<Word.BorderValues>(Word.BorderValues.Single), Size = 2 },
                       new Word.InsideHorizontalBorder() { Val = new Doc.EnumValue<Word.BorderValues>(Word.BorderValues.Single), Size = 2 },
                       new Word.InsideVerticalBorder() { Val = new Doc.EnumValue<Word.BorderValues>(Word.BorderValues.Single), Size = 2 }
                       )
                    );

                Word.Table tb = new Word.Table(tbProp);


                Word.TableRow headerRow = new Word.TableRow();
                foreach (DataColumn col in dt.Columns)
                {
                    Word.TableCellProperties cellProp = new Word.TableCellProperties()
                    {
                        TableCellWidth = new Word.TableCellWidth() { Width = new Doc.StringValue() { Value = "20%" } },
                    };
                    Word.TableCell cell = new Word.TableCell(cellProp);
                    cell.Append(new Word.Paragraph(new Word.Run(new Word.Text(col.ColumnName))));
                    headerRow.Append(cell);
                }
                tb.Append(headerRow);

                foreach (DataRow Row in dt.Rows)
                {
                    Word.TableRow row = new Word.TableRow();
                    for (int i = 0; i < maxColNum; i++)
                    {
                        Word.TableCell cell = new Word.TableCell();
                        cell.Append(new Word.Paragraph(new Word.Run(new Word.Text(Row[i].ToString()))));
                        row.Append(cell);
                    }
                    tb.Append(row);
                }

                bm.InsertAfterSelf<Word.Paragraph>(new Word.Paragraph(new Word.Run(tb)));
            }
            catch
            {

            }
        }

        public void UpdateTable(DataTable dt, int tbIndex,int columnrow=1)
        {
            try
            {
                int maxColNum = dt.Columns.Count;
                Word.Table tb = _mainDocPart.RootElement.Descendants<Word.Table>().ToArray()[tbIndex];

                Word.TableRow[] rows = tb.Descendants<Word.TableRow>().ToArray();
                Word.TableRow headerRow = rows[0];
                Word.TableRowProperties rowPro = null;
              
                if (rows.Length > 1)
                    rowPro = rows[1].TableRowProperties;
                if (rowPro == null) rowPro = new Word.TableRowProperties();
                Word.TableCell[] hcells = headerRow.Descendants<Word.TableCell>().ToArray();
                Word.TableCellProperties cellPro = null;
                Word.ParagraphProperties cellPPrp = null;
              
                int i = 0;
                //foreach (DataColumn col in dt.Columns)
                //{
                //    if (i < hcells.Length)
                //    {
                //        Word.Paragraph pa = hcells[i].Descendants<Word.Paragraph>().FirstOrDefault();
                //        if (pa == null)
                //            hcells[i].Append(new Word.Paragraph(new Word.Run(new Word.Text(col.ColumnName))));
                //        else
                //            pa.Append(new Word.Run(new Word.Text(col.ColumnName)));
                //    }
                //    else
                //    {
                //        Word.TableCell cell = new Word.TableCell();
                //        cell.Append(new Word.Paragraph(new Word.Run(new Word.Text((col.ColumnName)))));
                //        headerRow.Append(cell);
                //    }
                //    i++;
                //}

                i = 0;
                foreach (DataRow Row in dt.Rows)
                {
                    if (i < rows.Length - columnrow)
                    {
                        Word.TableRow row = rows[i + columnrow];
                        Word.TableCell[] cells = row.Descendants<Word.TableCell>().ToArray();
                        if (cellPro == null)
                        {
                            cellPPrp = cells[0].Parent.Descendants<Word.ParagraphProperties>().FirstOrDefault();
                            cellPro = cells[0].TableCellProperties;
                        }
                        for (int j = 0; j < maxColNum; j++)
                        {
                            if (j < cells.Length)
                            {
                                Word.Paragraph pa = cells[j].Descendants<Word.Paragraph>().FirstOrDefault();
                                if (pa == null)
                                    cells[j].Append(new Word.Paragraph(new Word.Run(new Word.Text(Row[j].ToString()))));
                                else
                                    pa.Append(new Word.Run(new Word.Text(Row[j].ToString())));
                            }
                            else
                            {

                                Word.TableCell lcell = row.Descendants<Word.TableCell>().LastOrDefault();
                                Word.TableCell cell = new Word.TableCell() { TableCellProperties = new Word.TableCellProperties(lcell.TableCellProperties.OuterXml) };
                                cell.Append(new Word.Paragraph(new Word.Run(new Word.Text(Row[j].ToString()))));
                                lcell.InsertAfterSelf(cell);

                            }
                        }
                    }
                    else
                    {
                        Word.TableRow row = new Word.TableRow();
                        row.TableRowProperties = new Word.TableRowProperties(rowPro.OuterXml);
                        for (int j = 0; j < maxColNum; j++)
                        {
                            Word.TableCell cell = new Word.TableCell();
                            cell.TableCellProperties = new Word.TableCellProperties(cellPro.OuterXml);
                            cell.Append(new Word.Paragraph(new Word.Run(new Word.Text(Row[j].ToString()))) {
                                ParagraphProperties = new Word.ParagraphProperties(cellPPrp.OuterXml)
                            });
                            row.Append(cell);
                        }
                        tb.Append(row);
                    }
                    i++;
                }
            }
            catch
            {

            }
        }

        public void UpdateChart(string cellColumn, uint intRow, string newValue, bool axisValue, int index = 0)
        {
            Pack.ChartPart[] parts = _mainDocPart.ChartParts.ToArray();
            // Gets the Chart stream
            if (parts.Length == 0) throw new Exception("Can not find then chart!");
            Stream stream = parts[index].EmbeddedPackagePart.GetStream();

            using (Pack.SpreadsheetDocument wordSSDoc = Pack.SpreadsheetDocument.Open(stream, true))
            {
                // Navigate to the sheet where the chart data is located
                Pack.WorkbookPart workBookPart = wordSSDoc.WorkbookPart;
                Sheet.Sheet theSheet = workBookPart.Workbook.Descendants<Sheet.Sheet>().
                  Where(s => s.Name == "Sheet1").FirstOrDefault();
                if (theSheet != null)
                {
                    Sheet.Worksheet ws = ((Pack.WorksheetPart)workBookPart.GetPartById(theSheet.Id)).Worksheet;

                    // Get the cell which needs to be updated
                    Sheet.Cell theCell = InsertCellInWorksheet(cellColumn, intRow, ws);

                    // Update the cell value
                    theCell.CellValue = new Sheet.CellValue(newValue);
                    if (axisValue)
                    {
                        // We are updating the Series text
                        theCell.DataType = new Doc.EnumValue<Sheet.CellValues>(Sheet.CellValues.String);
                    }
                    else
                    {
                        // We are updating a numeric chart value
                        theCell.DataType = new Doc.EnumValue<Sheet.CellValues>(Sheet.CellValues.Number);
                    }

                    // Either one of these methods work. It is just to illustrate the different elements that the OpenXML goes through
                    //this.ModifyChartDetailed(cellColumn, intRow, newValue, axisValue);
                    this.ModifyChartSimplified(cellColumn, intRow, newValue, axisValue);

                    ws.Save();
                }
            }
        }

        public void UpdateChart(DataTable tbSource, int xindex, int index = 0)
        {
            Pack.ChartPart[] parts = _mainDocPart.ChartParts.ToArray();
            // Gets the Chart stream
            if (parts.Length == 0) throw new Exception("Can not find then chart!");
            Stream stream = parts[index].EmbeddedPackagePart.GetStream();
            using (Pack.SpreadsheetDocument wordSSDoc = Pack.SpreadsheetDocument.Open(stream, true))
            {
                Pack.WorkbookPart workBookPart = wordSSDoc.WorkbookPart;
                Sheet.Sheet theSheet = workBookPart.Workbook.Descendants<Sheet.Sheet>().
                  Where(s => s.Name == "Sheet1").FirstOrDefault();
                if (theSheet != null)
                {
                    Sheet.Worksheet ws = ((Pack.WorksheetPart)workBookPart.GetPartById(theSheet.Id)).Worksheet;

                    if (InsertTBInWorksheet(tbSource, xindex, ws))
                    {

                        //ModifyChartByDT(tbSource, index);
                        ws.Save();
                    }

                }
            }


        }

        private Sheet.Cell InsertCellInWorksheet(string columnName, uint rowIndex, Sheet.Worksheet worksheet)
        {
            Sheet.SheetData sheetData = worksheet.GetFirstChild<Sheet.SheetData>();
            string cellReference = columnName + rowIndex;
            Sheet.Row row;
            if (sheetData.Elements<Sheet.Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Sheet.Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Sheet.Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            if (row.Elements<Sheet.Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Sheet.Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                Sheet.Cell refCell = null;
                foreach (Sheet.Cell cell in row.Elements<Sheet.Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Sheet.Cell newCell = new Sheet.Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);
                worksheet.Save();
                return newCell;
            }
        }

        private bool InsertTBInWorksheet(DataTable dt, int xIndex, Sheet.Worksheet worksheet)
        {

            Sheet.SheetData sheetData = worksheet.GetFirstChild<Sheet.SheetData>();
            for (int rowIndex = 2; rowIndex < dt.Rows.Count + 2; rowIndex++)
            {
                Sheet.Row row;
                if (sheetData.Elements<Sheet.Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
                {
                    row = sheetData.Elements<Sheet.Row>().Where(r => r.RowIndex == rowIndex).First();
                    if (row.Elements<Sheet.Cell>().Where(c => c.CellReference.Value == "A" + rowIndex).Count() > 0)
                    {
                        Sheet.Cell cell = row.Elements<Sheet.Cell>().Where(c => c.CellReference.Value == "A" + rowIndex).First();
                        cell.DataType = new Doc.EnumValue<Sheet.CellValues>(Sheet.CellValues.Number);
                        if (cell.CellValue == null)
                            cell.CellValue = new Sheet.CellValue(dt.Rows[rowIndex - 2][xIndex].ToString());
                        else
                            cell.CellValue.Text = dt.Rows[rowIndex - 2][xIndex].ToString();
                    }
                    else
                    {
                        Sheet.Cell refCell = null;
                        foreach (Sheet.Cell ce in row.Elements<Sheet.Cell>())
                        {
                            if (string.Compare(ce.CellReference.Value, "A" + rowIndex, true) > 0)
                            {
                                refCell = ce;
                                break;
                            }
                        }

                        Sheet.Cell newCell = new Sheet.Cell()
                        {
                            CellReference = "A" + rowIndex,
                            CellValue = new Sheet.CellValue(dt.Rows[rowIndex - 2][xIndex].ToString()),
                            StyleIndex = 1,
                            DataType = new Doc.EnumValue<Sheet.CellValues>(Sheet.CellValues.Number)
                        };
                        row.InsertBefore(newCell, refCell);

                    }

                }
                else
                {
                    Sheet.Row lrow = sheetData.Elements<Sheet.Row>().Where(r => r.RowIndex == rowIndex - 1).First();
                    row = new Sheet.Row()
                    {
                        RowIndex = (uint)rowIndex,
                        Spans = new Doc.ListValue<Doc.StringValue>() { InnerText = "1:4" },
                        DyDescent = new Doc.DoubleValue(0.15)
                    };
                    lrow.InsertAfterSelf(row);
                    Sheet.Cell refCell = null;
                    foreach (Sheet.Cell ce in row.Elements<Sheet.Cell>())
                    {
                        if (string.Compare(ce.CellReference.Value, "A" + rowIndex, true) > 0)
                        {
                            refCell = ce;
                            break;
                        }
                    }

                    Sheet.Cell newCell = new Sheet.Cell()
                    {
                        CellReference = "A" + rowIndex,
                        CellValue = new Sheet.CellValue(dt.Rows[rowIndex - 2][xIndex].ToString()),
                        StyleIndex = 1,
                        DataType = new Doc.EnumValue<Sheet.CellValues>(Sheet.CellValues.Number)
                    };
                    row.InsertBefore(newCell, refCell);

                }

                int i = 1;
                for (int j = 1; j < dt.Columns.Count + 1; j++)
                {
                    if (j == xIndex + 1) continue;
                    string columnName = Convert.ToChar('A' + i).ToString();
                    i++;
                    string cellReference = columnName + rowIndex;
                    Sheet.Cell cell;
                    if (row.Elements<Sheet.Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
                    {
                        cell = row.Elements<Sheet.Cell>().Where(c => c.CellReference.Value == cellReference).First();
                        if (cell.CellValue == null)
                            cell.CellValue = new Sheet.CellValue(dt.Rows[rowIndex - 2][j - 1].ToString());
                        else
                            cell.CellValue.Text = dt.Rows[rowIndex - 2][j - 1].ToString();
                        cell.DataType = new Doc.EnumValue<Sheet.CellValues>(Sheet.CellValues.Number);
                    }
                    else
                    {
                        Sheet.Cell refCell = null;
                        foreach (Sheet.Cell ce in row.Elements<Sheet.Cell>())
                        {
                            if (string.Compare(ce.CellReference.Value, cellReference, true) > 0)
                            {
                                refCell = ce;
                                break;
                            }
                        }

                        Sheet.Cell newCell = new Sheet.Cell()
                        {
                            CellReference = cellReference,
                            CellValue = new Sheet.CellValue(dt.Rows[rowIndex - 2][j - 1].ToString()),
                            StyleIndex = 1,
                            DataType = new Doc.EnumValue<Sheet.CellValues>(Sheet.CellValues.Number)
                        };
                        row.InsertBefore(newCell, refCell);
                    }
                }
            }
            worksheet.Save();
            return true;
        }

        private void ModifyChartSimplified(string cellColumn, uint intRow, string cellValue, bool axisValue)
        {
            try
            {
                Pack.ChartPart c_p = _mainDocPart.ChartParts.FirstOrDefault();
                Chart.ScatterChartSeries bs1 =
                    c_p.ChartSpace.Descendants<Chart.ScatterChartSeries>().Where(
                    s =>
                        string.Compare(s.InnerText, "Sheet1!$" + cellColumn + "$1", true) > 0
                    ).First();

                var b1 = bs1.Descendants<DocumentFormat.OpenXml.Drawing.Charts.YValues>().FirstOrDefault();
                //var a1 = b1.Descendants<DocumentFormat.OpenXml.Drawing.Charts.numer>().FirstOrDefault();
                if (axisValue)
                {
                    Chart.NumericValue nv1 = bs1.Descendants<Chart.NumericValue>().First();
                    nv1.Text = cellValue;
                }
                else
                {
                    // 
                    DocumentFormat.OpenXml.Drawing.Charts.YValues v1 = bs1.Descendants<DocumentFormat.OpenXml.Drawing.Charts.YValues>().FirstOrDefault();
                    Chart.NumericPoint np = v1.Descendants<Chart.NumericPoint>().ElementAt((int)intRow - 2);
                    Chart.NumericValue nv = np.Descendants<Chart.NumericValue>().First();
                    nv.Text = cellValue;
                }
            }
            catch
            {
                // Chart Element is not in a recognizable format. Most likely the defined Chart is incorrect. Ignore the chart creation.
                return;
            }
        }

        private void ModifyChartByDT(DataTable dt, int xindex, int chartIndex = 0)
        {
            try
            {
                Pack.ChartPart[] parts = _mainDocPart.ChartParts.ToArray();
                // Gets the Chart stream
                if (parts.Length == 0) throw new Exception("Can not find then chart!");
                Pack.ChartPart c_p = parts[chartIndex];
                int k = 1;
                for (int j = 1; j < dt.Columns.Count + 1; j++)
                {
                    if (j == xindex + 1) continue;
                    string columnName = Convert.ToChar('A' + k).ToString();
                    k++;
                    Chart.ScatterChartSeries bs1 =
                        c_p.ChartSpace.Descendants<Chart.ScatterChartSeries>().Where(
                        s =>
                            string.Compare(s.InnerText, "Sheet1!$" + columnName + "$1", true) > 0
                        ).FirstOrDefault();
                    if (bs1 == null)
                    {
                        string LcolumnName = Convert.ToChar('A' + k - 2).ToString();
                        Chart.ScatterChartSeries Lbs =
                             c_p.ChartSpace.Descendants<Chart.ScatterChartSeries>().Where(
                             s =>
                                 string.Compare(s.InnerText, "Sheet1!$" + LcolumnName + "$1", true) > 0
                             ).FirstOrDefault();
                        bs1 = new Chart.ScatterChartSeries()
                        {
                            InnerXml = Lbs.InnerXml.Replace("Sheet1!$" + LcolumnName + "$1", "Sheet1!$" + columnName + "$1"),
                            Index = new Chart.Index() { Val = (uint)j },
                            Order = new Chart.Order() { Val = (uint)j }
                        };
                        Chart.NumericValue nv2 = bs1.Descendants<Chart.NumericValue>().First();
                        nv2.Text = dt.Columns[j - 1].ColumnName;
                        Lbs.InsertAfterSelf(bs1);
                        continue;
                    }

                    Chart.XValues xstrc = bs1.Descendants<Chart.XValues>().FirstOrDefault();
                    xstrc.Descendants<Chart.PointCount>().FirstOrDefault().Val = (uint)dt.Rows.Count;
                    Chart.NumericPoint[] xpt = xstrc.Descendants<Chart.NumericPoint>().ToArray();
                    for (int r = 1; r < dt.Rows.Count + 1; r++)
                    {
                        if (r <= xpt.Length)
                            xpt[r - 1].Descendants<Chart.NumericValue>().FirstOrDefault().Text = dt.Rows[r - 1][xindex].ToString();
                        else
                        {
                            Chart.NumericPoint lpt = xstrc.Descendants<Chart.NumericPoint>().LastOrDefault();
                            Chart.NumericPoint pt = new Chart.NumericPoint()
                            {
                                NumericValue = new Chart.NumericValue(dt.Rows[r - 1][xindex].ToString()),
                                Index = (uint)r - 1
                            };
                            lpt.InsertAfterSelf(pt);
                        }
                    }
                    Chart.NumericValue nv1 = bs1.Descendants<Chart.NumericValue>().First();
                    nv1.Text = dt.Columns[j - 1].ColumnName;
                    DocumentFormat.OpenXml.Drawing.Charts.YValues yval = bs1.Descendants<DocumentFormat.OpenXml.Drawing.Charts.YValues>().FirstOrDefault();
                    int co = yval.Descendants<Chart.NumericPoint>().Count();
                    Chart.PointCount count = yval.Descendants<Chart.PointCount>().FirstOrDefault();
                    count.Val = (uint)dt.Rows.Count;
                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                    {
                        if (i <= co)
                        {
                            Chart.NumericPoint np = yval.Descendants<Chart.NumericPoint>().ElementAt(i - 1);
                            Chart.NumericValue nv = np.Descendants<Chart.NumericValue>().First();
                            nv.Text = dt.Rows[i - 1][j - 1].ToString();
                        }
                        else
                        {
                            Chart.NumericPoint np = yval.Descendants<Chart.NumericPoint>().LastOrDefault();
                            np.InsertAfterSelf(new Chart.NumericPoint() { NumericValue = new Chart.NumericValue(dt.Rows[i - 1][j - 1].ToString()), Index = (uint)i - 1 });
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        public DataTable ConverRowToCol(DataTable dt)
        {

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add(dt.Columns[0].ColumnName);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string col = dt.Rows[i][0].ToString();
                int n = 1;
                while (dtNew.Columns.Contains(col))
                {
                    col += n.ToString();
                    n++;
                }
                dtNew.Columns.Add(col);
            }
            for (int j = 1; j < dt.Columns.Count; j++)
            {
                DataRow drNew = dtNew.NewRow();
                drNew[0] = j;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drNew[i + 1] = dt.Rows[i][j].ToString();
                }
                dtNew.Rows.Add(drNew);
            }
            return dtNew;
        }

        public class DocxToHtml
        {
            public static string GetHtml(Pack.MainDocumentPart Docx)
            {

                string html = "";
                Word.Paragraph[] pargs = Docx.RootElement.Descendants<Word.Paragraph>().ToArray();
                foreach (var p in pargs)
                {
                    string txt = "";
                    Word.Run[] rs = p.Descendants<Word.Run>().ToArray();
                    foreach (var r in rs)
                    {
                        if (r.InnerXml.Contains("<w:tab"))
                        {
                            txt += string.Format("<label {1}>{0}</label>", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", getStyle(r));
                            continue;
                        }
                        txt += string.Format("<label {1}>{0}</label>", r.InnerText, getStyle(r));
                    }

                    html += string.Format("<p {1}>{0}</p>", txt, getStyle(p));
                }
                return html;
            }

            private static string getStyle(Word.Paragraph p)
            {
                string style = @"style=""{0}""";

                Word.ParagraphProperties pro = p.Descendants<Word.ParagraphProperties>().FirstOrDefault();
                if (pro != null)
                {
                    string prost = "";

                    Word.Justification js = pro.Descendants<Word.Justification>().FirstOrDefault();
                    if (js != null)
                    {
                        if (js.Val == Word.JustificationValues.Center)
                        {
                            prost += "text-align:center;";
                        }
                    }

                    //Word.RunProperties rpr = p.Descendants<Word.RunProperties>().FirstOrDefault();
                    //if(rpr!=null)
                    //{
                    //    Word.FontSize fs = rpr.Descendants<Word.FontSize>().FirstOrDefault();
                    //    if(fs!=null)
                    //    {
                    //        prost += string.Format("font-size:{0}px;", fs.Val.Value);
                    //    }

                    //}

                    //Word.FontSizeComplexScript fscs = pro.Descendants<Word.FontSizeComplexScript>().FirstOrDefault();
                    //if(fscs!=null)
                    //{
                    //    prost += string.Format("font-size:{0}px;", fscs.Val.Value);
                    //}


                    return string.Format(style, prost);
                }

                return "";
            }

            private static string getStyle(Word.Run r)
            {
                string style = @"style=""{0}""";
                string prost = "";
                Word.FontSizeComplexScript fscs = r.Descendants<Word.FontSizeComplexScript>().FirstOrDefault();
                if (fscs != null)
                {
                    prost += string.Format("font-size:{0}px;", int.Parse(fscs.Val.Value) - 3);
                }

                return string.Format(style, prost);
            }
        }
    }
}
