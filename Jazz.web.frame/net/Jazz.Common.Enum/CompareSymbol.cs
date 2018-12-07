using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Enum
{
    public enum CompareSymbol
    {  
        less,
        greater,
        equal,
        like,
        greaterORequal,
        lessORequal,
        notequal,
        rightlike,
        leftlike,
        filterEq,
    }

    public static partial class EnumExpand
    {
        public static string toString(this CompareSymbol symbol,bool UsePar=false)
        {
            if (UsePar)
            {
                switch (symbol)
                {
                    case CompareSymbol.equal:
                        return "{0}=={1}";
                    case CompareSymbol.greater:
                        return "{0}>{1}";
                    case CompareSymbol.greaterORequal:
                        return "{0}>={1}";
                    case CompareSymbol.leftlike:
                        return "{0} like {1}";
                    case CompareSymbol.less:
                        return "{0}<{1}";
                    case CompareSymbol.lessORequal:
                        return "{0}<={1}";
                    case CompareSymbol.like:
                        return "{0} like {1}";
                    case CompareSymbol.notequal:
                        return "{0}<>{1}";
                    case CompareSymbol.rightlike:
                        return "{0} like {1}";
                    case CompareSymbol.filterEq:
                        return " charindex('/'+{1}+'/','/'+[{0}]+'/')>0 ";
                    default:
                        return "";
                }
            }
            else
            {
                switch (symbol)
                {
                    case CompareSymbol.equal:
                        return "{0}=='{1}'";
                    case CompareSymbol.greater:
                        return "{0}>'{1}'";
                    case CompareSymbol.greaterORequal:
                        return "{0}>='{1}'";
                    case CompareSymbol.leftlike:
                        return "{0} like '%{1}'";
                    case CompareSymbol.less:
                        return "{0}<'{1}'";
                    case CompareSymbol.lessORequal:
                        return "{0}<='{1}'";
                    case CompareSymbol.like:
                        return "{0} like '%{1}%'";
                    case CompareSymbol.notequal:
                        return "{0}<>'{1}'";
                    case CompareSymbol.rightlike:
                        return "{0} like '{1}%'";
                    case CompareSymbol.filterEq:
                        return " charindex('/'+'{1}'+'/','/'+[{0}]+'/')>0 ";
                    default:
                        return "";
                }
            }
        }
    }
}
