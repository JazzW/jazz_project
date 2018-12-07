﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Jazz.Helper.DataBase.LINQ;
using Jazz.EFframe.Interface;
using Jazz.Common.Web;

namespace Jazz.EFframe.IConfig
{
    public abstract class EFFiliter
    {
        public enum symbol
        {
            less,
            greater,
            equal,
            like
        }

        public string ColName { get; set; }

        public object Val { get; set; }

        public symbol Symbol { get; set; }

        public virtual Expression<Func<T, bool>> toExp<T>() where T : InterfaceDBModel
        {
            return null;
        }
    }

    public class EFFiliter<S> : EFFiliter
    {

        public override Expression<Func<T, bool>> toExp<T>()
        {
            Expression<Func<T, bool>> exp = null;
            switch (this.Symbol)
            {
                case symbol.equal:
                    exp = LamdaHelper.CreateEqual<T, S>(ColName, (S)Val);
                    break;
                case symbol.greater:
                    exp = LamdaHelper.CreateGreaterThan<T, S>(ColName, (S)Val);
                    break;
                case symbol.less:
                    exp = LamdaHelper.CreateLessThan<T, S>(ColName, (S)Val);
                    break;
                case symbol.like:
                    exp = LamdaHelper.GetContains<T>(ColName, Val.ToString());
                    break;
                default:
                    exp = null;
                    break;
            }

            return exp;
        }
    }
}
