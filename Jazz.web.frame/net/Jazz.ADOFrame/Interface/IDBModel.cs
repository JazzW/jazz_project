using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Jazz.Common.Web;

namespace Jazz.ADOFrame.Interface
{
    public abstract class IDBModel:InterfaceDBModel
    {
        /// <summary>
        ///导出多个显示列
        /// </summary>
        public virtual  string getListCols()
        {
            return " * "; 
        }

        /// <summary>
        ///导出单个显示列
        /// </summary>

        public virtual string getItemCols()
        {
            return " * "; 
        }
    }

}
