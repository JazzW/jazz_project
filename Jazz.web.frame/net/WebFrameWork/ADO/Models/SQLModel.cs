using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WebFrameWork.ADO.Models
{
    public class SQLFiliter
    {
        public enum symbol
        {
            less,
            greater,
            equal,
            like,
            filterEq
        }

        string _fleid;

        SqlParameter _par;

        symbol _symbol;

        public string Fleid
        {
            get { return _fleid; }
            set { _fleid = value; }
        }

        public SqlParameter Par
        {
            get { return _par; }
            set
            {
                _par = new SqlParameter(value.ParameterName, value.SqlDbType, value.Size);
                _par.Value = value.Value;
            }
        }

        public symbol Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public string TbName
        {
            set
            {
                _fleid = value + _fleid;
            }
        }

        public SQLFiliter(string fleid, SqlParameter par, symbol symbol)
        {
            _fleid = fleid;
            _symbol = symbol;
            _par = new SqlParameter(par.ParameterName, par.SqlDbType, par.Size);
            _par.Value = par.Value;
        }

        public SQLFiliter(string fleid)
        {
            _fleid = fleid;
        }

        public override string ToString()
        {
            string sql = "";
            switch (_symbol)
            {
                case symbol.equal:
                    sql = string.Format(" [{0}]={1} ", _fleid, _par.ParameterName);
                    break;
                case symbol.greater:
                    sql = string.Format(" [{0}]>{1} ", _fleid, _par.ParameterName);
                    break;
                case symbol.less:
                    sql = string.Format(" [{0}]<{1} ", _fleid, _par.ParameterName);
                    break;
                case symbol.like:
                    sql = string.Format(" [{0}] like {1} ", _fleid, _par.ParameterName);
                    break;
                case symbol.filterEq:
                    sql = string.Format(" charindex('/'+{1}+'/','/'+[{0}]+'/')>0 ", _fleid, _par.ParameterName);
                    break;
            }
            return sql;
        }

    }

    public class Table<T>
    {
        public T datas { get; set; }
        public int total { get; set; }
    }

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
                    if(ss.Length>2)
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
