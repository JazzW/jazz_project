using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Web.Config
{
    public enum StatisTypeEnum
    {
        none,
        avg,
        sum,
        count,
        max,
        min,
        var,
        stdev
    }


    public class StatisItem
    {
        private bool _show = true;

        public string DispalyName { get; set; }

        public StatisTypeEnum Type { get; set; }

        public string ColFormate { get; set; }

        public bool Show { get { return _show; } set { _show = value; } }

        public bool Distinct { get; set; }

        public override string ToString()
        {
            string staticStr = "";
            switch (Type)
            {
                case StatisTypeEnum.avg:
                    staticStr = "AVG({0}) as [{1}]";
                    break;
                case StatisTypeEnum.count:
                    staticStr = "Count({0}) as [{1}]";
                    break;
                case StatisTypeEnum.max:
                    staticStr = "MAX({0}) as [{1}]";
                    break;
                case StatisTypeEnum.min:
                    staticStr = "MIN({0}) as [{1}]";
                    break;
                case StatisTypeEnum.sum:
                    staticStr = "SUM({0}) as [{1}]";
                    break;
                case StatisTypeEnum.var:
                    staticStr = "VAR({0}) as [{1}]";
                    break;
                case StatisTypeEnum.stdev:
                    staticStr = "Stdev({0}) as [{1}]";
                    break;
                case StatisTypeEnum.none:
                    staticStr = "{0} as [{1}]";
                    break;
            }
            staticStr = string.Format(staticStr, (this.Distinct ? " distinct " : "") + ColFormate, DispalyName);
            return staticStr;
        }

        public static string getStatisStr(string str, string incol, string outcol)
        {
            StatisTypeEnum en;
            if (System.Enum.TryParse(str.ToLower(), out en))
            {
                string staticStr = "";
                switch (en)
                {
                    case StatisTypeEnum.avg:
                        staticStr = "AVG({0}) as [{1}]";
                        break;
                    case StatisTypeEnum.count:
                        staticStr = "Count({0}) as [{1}]";
                        break;
                    case StatisTypeEnum.max:
                        staticStr = "MAX({0}) as [{1}]";
                        break;
                    case StatisTypeEnum.min:
                        staticStr = "MIN({0}) as [{1}]";
                        break;
                    case StatisTypeEnum.sum:
                        staticStr = "SUM({0}) as [{1}]";
                        break;
                    case StatisTypeEnum.var:
                        staticStr = "VAR({0}) as [{1}]";
                        break;
                    case StatisTypeEnum.stdev:
                        staticStr = "Stdev({0}) as [{1}]";
                        break;
                    case StatisTypeEnum.none:
                        staticStr = "{0} as [{1}]";
                        break;
                }
                return string.Format(staticStr, incol, outcol);
            }
            else
            {
                throw new Exception("违法参数");
            }
        }
    }
}
