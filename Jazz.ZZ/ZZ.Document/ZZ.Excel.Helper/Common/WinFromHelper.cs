using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZZ.Excel.Helper.Common
{
    public class WinFromHelper
    {
        public static string DirDialog(string Description,bool ShowNewFolderButton=false)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = Description;
            dialog.ShowNewFolderButton = ShowNewFolderButton;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                   
                    System.Windows.Forms.MessageBox.Show("文件夹路径不能为空", "提示");
                    return null;
                }
                return dialog.SelectedPath;
            }
            else
            {
                return null;
            }
        }

        public static string FileDialog(string title,params string[] FileType)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title =title;
            string filterString = "";
            foreach (var _t in FileType)
            {
                filterString += string.Format(",*.{0}", _t);
            }
            if (filterString.Length > 0)
            {
                filterString = filterString.Remove(0, 1);
            }
            dialog.Filter =string.Format("可选文件({0})|{0}",filterString);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = dialog.FileName;
                return file;
            }
            return null;
        }

        public static string[] FilesDialog(string title, params string[] FileType)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect =true;
            dialog.Title = title;
            string filterString = "";
            foreach (var _t in FileType)
            {
                filterString += string.Format(",*.{0}", _t);
            }
            if (filterString.Length > 0)
            {
                filterString = filterString.Remove(0, 1);
            }
            dialog.Filter = string.Format("可选文件({0})|{0}", filterString);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.FileNames;
            }
            return null;
        }

        public static string SaveAsDialog(params string[] FileType)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string filterString = "";
            foreach (var _t in FileType)
            {
                filterString += string.Format(",*.{0}", _t);
            }
            if (filterString.Length > 0)
            {
                filterString = filterString.Remove(0, 1);
            }
            saveFileDialog1.Filter = string.Format("文件格式({0})|{0}", filterString);
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            return null;
        }

        public static void OpenFile(string FilePath)
        {
            System.Diagnostics.Process.Start(FilePath);
        }

        public static void OpenDir(string dirPath)
        {
            System.Diagnostics.Process.Start("explorer.exe", dirPath); 
        }
    }
}
