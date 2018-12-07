using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZZ.Document.Mapper.Class;
using ZZ.Document.Mapper.Class.Excel;
using ZZ.Document.Mapper.Class.Word;
using ZZ.Document.Mapper.Service;

namespace ZZ.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            #region demo1

            //MapCollection map = new MapCollection()
            //{
            //    MapperType = MapType.insert,
            //    TargetMap = new WordMapTable
            //    {
            //        Key=0
            //    },
            //    SoureMap = new ExcelMapTable
            //    {
            //        CellCol = 1,
            //        CellRow = 3,
            //        ColumnLength = 3,
            //        RowLength = 3,
            //        SheetName = "Sheet1"
            //    }
            //};
            //List<MapCollection> Maps = new List<MapCollection>();
            //Maps.Add(map);
            //MapTask Task = new MapTask()
            //{
            //    Mappers = Maps,
            //    TargetInfo = new WordMapGobalInfo()
            //    {
            //        FilePath=@"C:\Users\wjc\Desktop\target.docx",
            //    },
            //    SoureInfo = new ExcelMapGoablInfo()
            //    {
            //        FilePath = @"C:\Users\wjc\Desktop\soure.xlsx",
            //    }
            //};
            //try
            //{

            //   Task.Start();
            //}
            //catch { }
            //Task.Dispose();

            #endregion

            #region demo2

            string desc = @"利润的本质是企业盈利的表现形式。与剩余价值相比利润不仅在质上是相同的，而且在量上也是相等的，利润所不同的只是，剩余价值是对可变资本而言的，利润是对全部成本而言的。因此，收益一旦转化为利润，利润的起源以及它所反映的物质生产就被赚了”（《马克思恩格斯全集》第25卷，第56页），因而就具有了繁多的赚钱形式。在资本主义社会，利润的本质就是：它是资本的产物，同劳动完全无关。";
            string descs = WordChar.getNum(1) + desc;
            descs +=WordChar.getEnter(1)+WordChar.getTab(1)+ WordChar.getNum(2)+desc;

            MapGoablCollection.addObj("year_start", DateTime.Now.Year - 1);
            MapGoablCollection.addObj("year_end", DateTime.Now.Year);
            MapGoablCollection.addObj("desc",descs);
            MapGoablCollection.addObj("isnum", true);

            List<string[]> files=new List<string[]>();
            files.Add(new string[]{
                @"C:\Users\wjc\Desktop\soure.xlsx",
                @"C:\Users\wjc\Desktop\target.docx",
                @"C:\Users\wjc\Desktop\target_1.docx",
            });
            files.Add(new string[]{
                @"C:\Users\wjc\Desktop\soure.xlsx",
                @"C:\Users\wjc\Desktop\target.xlsx",
            });
            var tasks = MapperService.getMapTaskfromConfig
                (@"C:\Users\wjc\Documents\visual studio 2013\Projects\ZZ.Excel\ZZ.Document.Mapper\Service\demp.xml");
            MapperService.Map(tasks, files);

            #endregion
        }
    }
}
