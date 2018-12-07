using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jazz.SOA.JMS.Class
{
    public class BaseDBModelItem
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }

    public class BaseDBModel:Jazz.Common.Web.InterfaceDBModel
    {
        public string TableName { get; set; }

        public List<BaseDBModelItem> Datas { get; set; }

        public static BaseDBModel CreateModel(string tbName, Dictionary<string, Object> dict)
        {
            BaseDBModel res = new BaseDBModel() { TableName = tbName,Datas=new List<BaseDBModelItem>() };

            foreach (var entry in dict)
            {
               res.Datas.Add(new BaseDBModelItem(){Name=entry.Key,Value=entry.Value});
            }

            return res;
        }

        public static BaseDBModel CreateModel(string tbName, string json)
        {
            BaseDBModel res = new BaseDBModel() { TableName = tbName, Datas = new List<BaseDBModelItem>() };


            return res;
        }

        public static BaseDBModel CreateModel( string json)
        {
            BaseDBModel res = new BaseDBModel();

            return res;
        }

        public static BaseDBModel CreateModel(string tbName, XElement xml)
        {
            BaseDBModel res = new BaseDBModel() { TableName = tbName, Datas = new List<BaseDBModelItem>() };


            return res;
        }

        public static BaseDBModel CreateModel(XElement xml)
        {
            BaseDBModel res = new BaseDBModel();

            return res;
        }

        public static BaseDBModel CreateModel(string tbName, XElement xml)
        {
            BaseDBModel res = new BaseDBModel() { TableName = tbName, Datas = new List<BaseDBModelItem>() };

            return res;
        }

        public static BaseDBModel CreateModel(XElement xml)
        {
            BaseDBModel res = new BaseDBModel();

            return res;
        }
    }
}
