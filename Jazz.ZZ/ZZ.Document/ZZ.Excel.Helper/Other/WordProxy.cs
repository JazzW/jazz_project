using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Novacode;

namespace ZZ.Excel.Helper.Other
{
    public class WordProxy:IDisposable
    {
        DocX docx;

        public static WordProxy Open(string filepath)
        {
            WordProxy proxy = new WordProxy();
            proxy.docx = DocX.Load(filepath);
            return proxy;
        }


        public string GetText(String bookmark)
        {
            return docx.Bookmarks[0].Paragraph.Text;
        }

        public void InsertText(string bookmark, string text)
        {
            docx.Bookmarks.FirstOrDefault(e => e.Name == bookmark).SetText(text);
        }

        public System.Data.DataTable GetTabel(int Key)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            Table tb = docx.Tables[Key];
            for (int i = 0; i < tb.ColumnCount; i++)
            {
                result.Columns.Add(i.ToString());
            }
            foreach(var row in tb.Rows)
            {
                System.Data.DataRow _row = result.NewRow();
                for(int i = 0; i < tb.ColumnCount; i++)
                {
                    _row[i] = row.Cells[i].Paragraphs.FirstOrDefault().Text;
                }
                result.Rows.Add(_row);
            }

            return result;
        }

        public void InsetTable(int Key,System.Data.DataTable dt)
        {
            Table tb = docx.Tables[Key];
         
            int num = dt.Rows.Count+1 - tb.RowCount;
            if (num>0)
            {
                for (int i = 0; i < num; i++)
                {
                    tb.InsertRow();
                }
            }
            else if(num<0)
            {
                for (int i = 0; i < -num; i++)
                {
                    tb.RemoveRow(tb.RowCount-1);
                }
            }

            int rowindex = 0;
            foreach (var row in tb.Rows)
            {
                if (rowindex == 0)
                {
 
                }
                else
                {
                    for (int i = 0; i < tb.ColumnCount; i++)
                    {
                        row.Cells[i].Paragraphs[0].InsertText(dt.Rows[rowindex-1][i].ToString());
                        //row.Cells[i].InsertParagraph(dt.Rows[rowindex][i].ToString());
                    }
                }
                rowindex++;
            }
        }

        public void Save()
        {
            docx.Save();
        }

        public void SaveAs(string path)
        {
            docx.SaveAs(path);
        }

        public void Dispose()
        {
            docx.Dispose();
        }
    }
}
