using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.ADOFrame.IConfig
{
    public class FilterConfig
    {
        public int Page { get; set; }

        public int length { get; set; }

        public string simpleSearch { get; set; }

        public string combineSearch { get; set; }

        public SQLFiliter[] toFiliter(string tbname, ref string type)
        {
            if (string.IsNullOrWhiteSpace(combineSearch))
                return null;
            var strs = combineSearch.Split('/');
            if (strs.Length > 0)
            {
                List<SQLFiliter> filters = new List<SQLFiliter>();
                type = strs[0].ToLower() == "all" ? "and" : "or";

                for (int i = 1; i < strs.Length; i++)
                {
                    if (strs[i] == "") continue;
                    var ss = strs[i].Split('-');
                    if (ss.Length > 2)
                    {
                        ss[1] = strs[i].Replace(ss[0] + "-", "");
                    }
                    SQLFiliter f = new SQLFiliter(ss[0]);
                    int t = 1;
                    while (filters.Where(e => e.Par.ParameterName == "@" + ss[0]).ToList().Count != 0)
                    {
                        ss[0] += t.ToString();
                        t++;
                    }

                    if (ss[1].Contains('<'))
                    {
                        f.Symbol = SQLFiliter.symbol.less;
                        ss[1] = ss[1].Replace("<", "");
                        f.Par = new System.Data.SqlClient.SqlParameter("@" + ss[0], ss[1]);
                    }
                    else if (ss[1].Contains('>'))
                    {
                        f.Symbol = SQLFiliter.symbol.greater;
                        ss[1] = ss[1].Replace(">", "");
                        f.Par = new System.Data.SqlClient.SqlParameter("@" + ss[0], ss[1]);
                    }
                    else if (ss[0].Contains('?'))
                    {
                        f.Symbol = SQLFiliter.symbol.filterEq;
                        ss[0] = ss[0].Replace("?", "");
                        f.Fleid = ss[0].Replace("?", "");
                        t = 1;
                        while (filters.Where(e => e.Par.ParameterName == "@" + ss[0]).ToList().Count != 0)
                        {
                            ss[0] += t.ToString();
                            t++;
                        }
                        f.Par = new System.Data.SqlClient.SqlParameter("@" + ss[0], ss[1]);
                    }
                    else if (ss[1].Contains('%'))
                    {
                        f.Symbol = SQLFiliter.symbol.like;
                        ss[1] = ss[1].Replace("%", "");
                        f.Par = new System.Data.SqlClient.SqlParameter("@" + ss[0], "%" + ss[1] + "%");
                    }
                    else
                    {
                        f.Symbol = SQLFiliter.symbol.equal;
                        f.Par = new System.Data.SqlClient.SqlParameter("@" + ss[0], ss[1]);
                    }
                    f.TbName = tbname;
                    filters.Add(f);

                }
                return filters.ToArray();

            }
            return null;
        }

        public FilterConfig()
        {
            Page = -1;
            length = -1;
        }
    }
}
