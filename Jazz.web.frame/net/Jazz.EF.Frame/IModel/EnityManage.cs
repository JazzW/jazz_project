﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attr= Jazz.EF.Frame.IAttribute;
using Jazz.Common.Web;

namespace Jazz.EF.Frame.IModel
{
    public  class EnityManage<TEnity>where TEnity:InterfaceDBModel
    {
        public static Attr.ColumnPro[] getKeys()
        {
            Type T=typeof(TEnity);
            var fields = (Attr.IAttribute)T.GetCustomAttributes(typeof(Attr.IAttribute), true).FirstOrDefault();
            if (fields != null)
            {
                var keys = fields.Columns.Where(p => p.Key == true).ToArray();
                return keys;
            }
            else
                return null;
        }

        public static void SetKeysVal(TEnity model,params object[] vals)
        {
            Type T=typeof(TEnity);
            Attr.ColumnPro[] keys = EnityManage<TEnity>.getKeys();
            var pros = T.GetProperties().Where(p => keys.Any(e => e.ColName == p.Name)).ToList();
            int i = 0;
            foreach (var pro in pros)
            {
                if (vals.Length <= i) break;
                pro.SetValue(model, vals[i]);
                i++;
            }
        }

        public static void SetVal(string ProName, object val, TEnity model)
        {
            Type T = typeof(TEnity);
            var pros = T.GetProperties().Where(p => p.Name == ProName).FirstOrDefault();
            if (pros != null)
            {
                pros.SetValue(model, val);
            }
        }

        public static object GetVal(string ProName, TEnity model)
        {
            Type T = typeof(TEnity);
            var pros = T.GetProperties().Where(p => p.Name == ProName).FirstOrDefault();
            if (pros != null)
            {
                return pros.GetValue(model);
            }
            else
                return null;
        }

        public static S GetVal<S>(string ProName,TEnity model)
        {
            Type T = typeof(TEnity);
            var pros = T.GetProperties().Where(p => p.Name == ProName).FirstOrDefault();
            if (pros != null)
            {
                return (S)pros.GetValue(model);
            }
            else
                return default(S);
        }
    }
}
