using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace Jazz.Helper.DataBase.LINQ
{
    public static class LamdaHelper
    {
        /// <summary>
        /// 创建lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return p => true;
        }

        /// <summary>
        /// 创建lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return p => false;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="TKey">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T, S>(string propertyName, S propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(S));//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName1 == p.propertyName2
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName1, string propertyName2)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member1 = Expression.PropertyOrField(parameter, propertyName1);
            MemberExpression member2 = Expression.PropertyOrField(parameter, propertyName2);
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member1, member2), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T, S>(string propertyName, S propertyValue)
        {

            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(S));//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName > propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThan<T, S>(string propertyName, S propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(S));//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>  propertyValue > p.propertyName 
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThan<T, S>(string propertyName, S propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(S));//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName >= propertyValue
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T, S>(string propertyName, S propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(S));//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>propertyValue >= p.propertyName 
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThanOrEqual<T, S>(string propertyName, S propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(S));//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T">对象名称（类名）</typeparam>
        /// <param name="propertyName">字段名称（数据库中字段名称）</param>
        /// <param name="propertyValue">数据值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
        }

        public static Expression<Func<T, bool>> union<T>(Expression<Func<T, bool>> lamda1, Expression<Func<T, bool>> lamda2)
        {

            return Expression.Lambda<Func<T, bool>>(Expression.And(lamda1.Body, lamda2.Body));
        }

        public static Expression<Func<T, bool>> createOr<T>(this Expression<Func<T, bool>> expr1,
                                                          Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
                return expr2;
            else if (expr2 == null)
                return expr1;

            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Replace(expr1.Body);
            var right = visitor.Replace(expr2.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter); ;
        }

        public static Expression<Func<T, bool>> createAnd<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
                return expr2;
            else if (expr2 == null)
                return expr1;

            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Replace(expr1.Body);
            var right = visitor.Replace(expr2.Body);
            var body = Expression.And(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter); ;
        }

        /// <summary>
        /// 创建lambda表达式：!(p=>p.Tkey)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Expression<Func<T, Tkey>> getKey<T, Tkey>(string fieldName)
        {
            var type = typeof(T);
            var property = type.GetProperty(fieldName);
            var param = Expression.Parameter(typeof(T), "i");
            var propertyAcess = Expression.MakeMemberAccess(param, property);
            var sortExpression = Expression.Lambda<Func<T, Tkey>>(propertyAcess, param);

            return sortExpression;
        }
    }

    internal class NewExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpression _NewParameter { get; private set; }
        public NewExpressionVisitor(ParameterExpression param)
        {
            this._NewParameter = param;
        }
        public Expression Replace(Expression exp)
        {
            return this.Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this._NewParameter;
        }
    }
}
