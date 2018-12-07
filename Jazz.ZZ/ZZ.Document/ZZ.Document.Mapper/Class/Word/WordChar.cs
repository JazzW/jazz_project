using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.Document.Mapper.Class.Word
{
     public class WordChar
    {
         public static string getNum(int num)
         {
             string str = "①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳㉑㉒㉓㉔㉕㉖㉗㉘㉙㉚㉛㉜㉝㉞㉟㊱㊲㊳㊴㊵㊶㊷㊸㊹㊺㊻㊼㊽㊾㊿";
             return str[num - 1].ToString();
         }

         public static string getTab(int num=1)
         {
             string res = "";
             for (int i = 0; i < num; i++)
             {
                 res += @"\tab";
             }
             return res;
         }

         public static string getEnter(int num=1)
         {
             string res = "";
             for (int i = 0; i < num; i++)
             {
                 res += @"\r\n";
             }
             return res;
         }

    }
}
