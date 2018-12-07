using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Helper.DataBase.NoSQL;
using MongoDB.Driver.Builders;

namespace Jazz.Demo.DBConsole
{
    class Program
    {
        static void Main(string[] args)
        {
        
            Mongo _mongo = new Mongo();
            //_mongo.InsertBean<PersonBean>("Beans", new PersonBean() { _name = "wang", _info = "123" });
            var corsur = _mongo.QueryBeans("Beans", Query.EQ("_name", "wang"));
            foreach (var c in corsur)
            {
              
                Console.WriteLine(c.ToString());
            }
            Console.Read();
        }
    }
}
