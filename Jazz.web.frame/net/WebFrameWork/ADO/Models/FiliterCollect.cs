using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFrameWork.ADO.Models
{
    public enum inputTypeEnum
    {
        text,
        rate,
        date,
        number,
        numberRange,
        dateYear,
        dateRange
    }

    public class FiliterCollect
    {
        public FiliterCollect()
        {
            this.children = new FiliterCollect[0];
        }

        public string label { get; set; }

        public string value { get; set; }

        public bool input { get; set; }

        public string inputType { get; set; }

        public FiliterCollect[] children { get; set; }
    }
}
