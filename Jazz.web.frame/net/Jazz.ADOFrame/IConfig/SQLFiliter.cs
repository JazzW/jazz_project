using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Jazz.ADOFrame.IConfig
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
}
