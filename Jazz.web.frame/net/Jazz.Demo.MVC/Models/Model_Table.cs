using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jazz.Common.Web;
using Jazz.EFframe.IMap;
using Frame = Jazz.EF.Frame;
using Jazz.Common.Enitiy.IAttribute;

namespace Jazz.Demo.API.Models
{
    [Model_Table(Table="Table")]
    [DBModel(TBName="[Table]")]
    public class Model_Table:InterfaceDBModel
    {
        [Key]
        [DBItem(ColName="Id")]
        public int Id { get; set; }

          [DBItem(ColName = "Name",Size=50)]
        public string Name { get; set; }

          [DBItem(ColName = "Mess")]
        public string Mess { get; set; }
    }


    public class Model_TableAttribute : Frame.IAttribute.IAttribute
    {
        public Model_TableAttribute()
        {
            this.Columns = new List<Frame.IAttribute.ColumnPro>();
            Columns.Add(new Frame.IAttribute.ColumnPro() { ColName = "Id", ColType = typeof(int), Key = true });
            Columns.Add(new Frame.IAttribute.ColumnPro() { ColName = "Name", ColType = typeof(string), Key = false });
            Columns.Add(new Frame.IAttribute.ColumnPro() { ColName = "Mess", ColType = typeof(string), Key = false });
        }
    }

    public class Model_TableMapping : BaseDomainMapping<Model_Table>
    {
        public override void Init()
        {
            this.ToTable("Table");
            this.HasKey(l => l.Id);
            this.Property(l => l.Name).HasMaxLength(50);//设置Name属性长度为200 并且是必填

        }
    }
}