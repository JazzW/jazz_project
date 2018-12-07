using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Text;

namespace WebFrameWork.Helper
{
    public class LanguageHelper
    {
        public enum TranslateType
        {
            ColumnName,
            word,
        }

        public enum TranslateLang
        {
            Chinese,
            English,
        }

        /// <summary>
        /// 中英字典
        /// </summary>
        public static class ChineseEnglishDic
        {
            static string _filePath;
            public static string filePath
            {
                get
                {
                    return _filePath;
                }
                set
                {
                    if (_filePath != value)
                    {
                        _filePath = value;
                        Encoding encoding = Encoding.ASCII; //Encoding.ASCII;//
                        if (System.IO.File.Exists(filePath))
                        {
                            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                            StreamReader sr = new StreamReader(fs, encoding);
                            try
                            {
                                string strLine = "";
                                string[] aryLine = null;

                                while ((strLine = sr.ReadLine()) != null)
                                {
                                    aryLine = strLine.Split(',');
                                    if (aryLine.Length < 2) continue;
                                    _dictory.Add(aryLine[0].Trim(), aryLine[1].Trim());

                                }
                            }
                            catch
                            {

                            }
                            finally
                            {
                                sr.Close();
                                fs.Close();
                            }
                        }
                    }
                }
            }

            static Dictionary<string, string> _dictory = new Dictionary<string, string>();

            public static string getChinese(string value)
            {
                try
                {
                    return _dictory[value];
                }
                catch
                {
                    return "N";
                }
            }

            public static string getEnglish(string value)
            {
                try
                {
                    return _dictory.Where(e => e.Value == value).First().Key;
                }
                catch
                {
                    return "N";
                }
            }

        }

        public static string Translate(string sentense, TranslateType tType = TranslateType.ColumnName, TranslateLang lang = TranslateLang.Chinese)
        {
            string word = sentense;
            //if (tType == TranslateType.ColumnName)
            //{
            //    word = word.Remove(0, 3);
            //}
            switch (lang)
            {
                case TranslateLang.Chinese:
                    return ChineseEnglishDic.getChinese(sentense);
                case TranslateLang.English:
                    return ChineseEnglishDic.getEnglish(sentense);
                default:
                    return "N";
            }

        }

    }
}
