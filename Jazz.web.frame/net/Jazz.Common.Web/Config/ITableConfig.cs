using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jazz.Common.Enum;
using System.Linq.Expressions;
using System.Data.Common;
using Jazz.Helper.DataBase.LINQ;
using System.Data.SqlClient;
using System.Data.OleDb;
using Jazz.Common.Enum;

namespace Jazz.Common.Web
{
    public  class ITableConfig
    {
        public ITableConfig(params ISelectConfig[] configs)
        {
            this.Configs = configs.ToList();
        }

      public List<ISelectConfig> Configs { get; set; }

      public int Page { get; set; }

      public int Length { get; set; }

      public string OrderCol { get; set; }

      public  string toSqlCmdExp<T>(Helper.DataBase.Common.DBConfig.DatabaseType dbtype,out DbParameter[] pars,int startIndex=0)
      {

          string sql = "";
          pars = getDbParameters(dbtype,startIndex);
          foreach (var f in Configs)
          {
              if (sql.Length==0)
                  sql = string.Format(" ({0}) ", f.toSqlCmdExp<T>(ref pars));
              else
                  sql += string.Format("and ({0}) ", f.toSqlCmdExp<T>(ref pars));
          }
          if (sql.Length == 0)
              sql = " 1=1 ";

          return sql;
      }

      private DbParameter[] getDbParameters(Helper.DataBase.Common.DBConfig.DatabaseType dbtype, int startIndex = 0)
      {
          int f_count=0;
          foreach (var f in Configs)
          {
              f_count += f.Filiters.Count;
          }
          DbParameter[] pars;
          switch (dbtype)
          {
              case Helper.DataBase.Common.DBConfig.DatabaseType.SqlServer:
                  pars = Helper.DataBase.Common.DBConfig.createParaemter(f_count);
                  break;
              default:
                  pars = Helper.DataBase.Common.DBConfig.createParaemter(f_count);
                  break;
          }
          int i=startIndex;
          foreach (var p in pars) {
              p.ParameterName = "@par_" + i.ToString();
              i++;
          }

          return pars;
          
      }

      public  Expression<Func<T, bool>> toLamadaExp<T>() where T : InterfaceDBModel
      {
          Expression<Func<T, bool>> exp = null;
          if (Configs.Count == 0)
              exp = LamdaHelper.True<T>();
          foreach (var f in Configs)
          {
              if (exp == null)
                  exp = f.toLamadaExp<T>();
              else
                  exp = LamdaHelper.createAnd<T>(exp, f.toLamadaExp<T>());
          }
          return exp;
      }

      public virtual Expression<Func<T, Tkey>> toOrderExp<T,Tkey>() where T : InterfaceDBModel
      {
          return LamdaHelper.GetOrderExpression<T,Tkey>(OrderCol);
      }

      public string toOrderSql()
      {
          if (string.IsNullOrWhiteSpace(this.OrderCol))
              return "";
          string sql = " order by ";

          return sql+OrderCol;
      }
    }
}
