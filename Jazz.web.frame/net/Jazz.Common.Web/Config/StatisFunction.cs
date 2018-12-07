using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Jazz.Common.Web.Config
{
    public class StatisFunction
    {
        
        private string[] _GColName;

        private string[] _OColName;

        private StatisItem[] _SColName;

        public StatisItem[] getSCol()
        {
            return this._SColName;
        }

        public String[] getOCol()
        {
            return this._OColName;
        }

        public String getOColSql()
        {
            string orderByStr = "";

            if (_OColName.Length > 0)
            {
                orderByStr = " order by {0} ";
                string cols = "";
                foreach (string col in _OColName)
                {
                    cols += "," + col;
                }
                if (cols.Length > 0)
                    cols = cols.Remove(0, 1);
                orderByStr = string.Format(orderByStr, cols);
            }
            return orderByStr;
        }

        public StatisFunction()
        {

        }

        //'SCOL:Name/AVG_ID/..'
        public string SCOL
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _SColName = new StatisItem[0];
                    return;
                }
                var strs = value.Split('/');
                List<StatisItem> staticItems = new List<StatisItem>();
                 foreach(string str in strs)
                 {
                     if (str.Length < 1) continue;
                     StatisItem item = new StatisItem();
                     if(str.Contains("-"))
                     {
                         var _strs=str.Split('-');
                        switch (_strs[0].ToLower())
                        {
                            case "avg":
                                item.Type = StatisTypeEnum.avg;
                                break;
                            case "sum":
                                item.Type = StatisTypeEnum.sum;
                                break;
                            case "count":
                                item.Type = StatisTypeEnum.count;
                                break;
                            case "max":
                                item.Type = StatisTypeEnum.max;
                                break;
                            case "min":
                                item.Type = StatisTypeEnum.min;
                                break;
                            case "var":
                                item.Type = StatisTypeEnum.var;
                                break;
                            case "stdev":
                                item.Type = StatisTypeEnum.stdev;
                                break;
                            default:
                                item.Type = StatisTypeEnum.none;
                                break;
                        }
                        if (_strs[0] == "*")
                        {
                            
                            item.DispalyName = str;
                            item.ColFormate =str.Replace("*-", "").Replace("|","/");
                            if (str.Contains(';') || str.Contains('?') ||
                                      str.Contains('\'') || str.Contains('\\') ||
                                       str.Contains("--"))
                            continue;
                            item.Distinct = false;
                            staticItems.Add(item);
                            continue;
                        }
                         if (_strs[1].Contains("@"))
                         {
                             item.Distinct = true;
                             item.ColFormate = getColName(_strs[1].Remove(0,1));
                         }
                         else
                         {
                             item.Distinct = false;
                             item.ColFormate = getColName(_strs[1]);
                         }
                         item.DispalyName = str;
                     }
                     else
                     {
                         item.ColFormate = getColName( str);
                         item.DispalyName = str;
                     }
                     staticItems.Add(item);

                 }
                 _SColName = staticItems.ToArray();
            }
        }

        public string OtherCol { get; set; }

        //'OCOL:Name/AVG_ID/..'
        public string GCOL
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _GColName = new string[0];
                    return;
                }
                var strs = value.Split('/');
                List<string> staticItems = new List<string>();
                foreach (string str in strs)
                {
                    if (str.Length < 1) continue;
                    staticItems.Add(getColName(str));

                }
                _GColName = staticItems.ToArray();
            }
            get
            {
                if (_GColName.Length > 0)
                    return "1";
                else
                    return "";
            }
        }

        //'GCOL:AES-Name/ID/..'
        public string OCOL
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _OColName = new string[0];
                    return;
                }
                var strs = value.Split('/');
                List<string> staticItems = new List<string>();
                for (int i = 0; i < strs.Length;i++ )
                {
                    if (strs[i].Split('-').Length > 1)
                    {
                        var _strs = strs[i].Split('-');
                        for (int t = 2; t < _strs.Length; t++)
                        {
                            _strs[1]+="-"+_strs[t];
                        }
                        string orderType = "";
                        if (_strs[0].ToUpper() == "ASC")
                            orderType = "asc";
                        else if (_strs[0].ToUpper() == "DESC")
                            orderType = "desc";

                        staticItems.Add("["+getColName(_strs[1]) + "] " + orderType);
                    }
                }
                _OColName = staticItems.ToArray();
            }
        }

        private string getColName(string str)
        {
            return str;
        }

        public void InitFunc()
        {

        }



        public bool Check(IEnum Tenum)
        {
            if (Tenum == null) return true;
            foreach (var col in this._SColName)
            {
                Tenum.check(col.ColFormate);
            }
      
            foreach (var col in this._OColName)
            {
                if(!this._SColName.Any(e=>col.Contains("["+e.DispalyName+"]"))){
                    return false;
                }
            }

            foreach (var col in this._GColName)
            {
                if (this._SColName.Any(e => e.ColFormate != col && e.Type==Jazz.Common.Web.Config.StatisTypeEnum.none))
                {
                    return false;
                }
            }

            return true;
        }

        public string toSQL(ITableConfig configs, string sqlSource, out DbParameter[] pars,bool skipOrder=false)
        {

            string selectStr = "";
            if(_SColName.Length>0)
            {
                foreach(var col in _SColName)
                {
                    if (!col.Show) continue;
                    selectStr += "," + col.ToString();
                }
                if (selectStr.Length > 0)
                {
                    selectStr = selectStr.Remove(0, 1);
                    if (!string.IsNullOrWhiteSpace(this.OtherCol))
                    {
                        selectStr += "," + this.OtherCol;
                    }
                }
            }
                          
            string groupByStr="";
            if(_GColName.Length>0)
            {
                groupByStr=" group by {0} ";
                string cols="";
                foreach(string col in _GColName)
                {
                    cols+=","+col;
                }
                if(cols.Length>0)
                    cols=cols.Remove(0, 1);
                groupByStr=string.Format(groupByStr,cols);
            }

            string orderByStr = "";
  
            if (_OColName.Length > 0 && !skipOrder)
            {
                orderByStr = " order by {0} ";
                string cols = "";
                foreach (string col in _OColName)
                {
                    cols += "," + col;
                }
                if (cols.Length > 0)
                    cols = cols.Remove(0, 1);
                orderByStr = string.Format(orderByStr, cols);
            }
            

            string sqlf = "1=1";

            if (configs != null)
            {
                sqlf = "";
                sqlf = configs.toSqlCmdExp<Object>(Helper.DataBase.Common.DBConfig.DatabaseType.SqlServer, out pars);
            }
            else
                pars = null;

            string sql = "";
            if(configs.Page>0 && configs.Page>0)
                sql = Jazz.Helper.DataBase.Common.SQLCmdHelper.SelectFrame
                (string.Format(" top {0} {1}", configs.Page * configs.Length, selectStr), sqlSource, sqlf,groupByStr+" "+orderByStr);
            else
                sql = Jazz.Helper.DataBase.Common.SQLCmdHelper.SelectFrame
               (string.Format(" {0} ",  selectStr), sqlSource, sqlf, groupByStr + " " + orderByStr);
            sql = Jazz.Helper.DataBase.Common.SQLCmdHelper.PageFrame(sql, configs.Length, configs.Page);

            return sql;
        }
    }
}
