using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Helper.Common
{
    public static class AopHelper
    {

        public static bool Run(this Action Func, IMess mess)
        {

            FuncRunAttribute attr =
                (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if (attr == null)
                attr =
                    (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                try
                {
                    attr.BeforeRun(mess);
                     Func();
                     attr.SuccessRun(mess,null);
                }
                catch (Exception ex)
                {
                    attr.ErrorRun(mess,ex);
                    return false;
                }
                finally
                {
                    attr.AfterRun(mess);
                }
            }
            else
                Func();
            return true;
        }

        public static T Run<T>(this Func<T> Func, IMess mess)
        {
            FuncRunAttribute attr = 
                (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if(attr==null)
                attr =
                    (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                try
                {
                    attr.BeforeRun(mess);
                    T ouput = Func();
                    attr.SuccessRun(mess,ouput);
                    return ouput;
                }
                catch (Exception ex)
                {
                    attr.ErrorRun(mess,ex);
                    return default(T);
                }
                finally
                {
                    attr.AfterRun(mess);
                }
            }
            else
                return Func();
        }

        public static T Run<T, T1>(this Func<T1, T> Func, IMess mess, T1 par1)
        {

            FuncRunAttribute attr =
                (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if (attr == null)
                attr =
                    (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                try
                {
                    attr.BeforeRun(mess);
                    T ouput = Func(par1);
                    attr.SuccessRun(mess,ouput);
                    return ouput;
                }
                catch (Exception ex)
                {
                    attr.ErrorRun(mess,ex);
                    return default(T);
                }
                finally
                {
                    attr.AfterRun(mess);
                }
            }
            else
                return Func(par1);
        }


        public static T Run<T, T1, T2>(this Func<T1, T2, T> Func,IMess mess, T1 par1, T2 par2)
        {
            FuncRunAttribute attr =
                (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if (attr == null)
                attr =
                    (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
            if (attr != null)
            {

                try
                {
                    attr.BeforeRun(mess);
                    T ouput = Func(par1, par2);
                    attr.SuccessRun(mess,ouput);
                    return ouput;
                }
                catch (Exception ex)
                {
                    attr.ErrorRun(mess,ex);
                    return default(T);
                }
                finally
                {
                    attr.AfterRun(mess);
                }
            }
            else
                return Func(par1, par2);
        }

        public static Task<bool> RunAsync(this Action Func,IMess mess)
        {
            Task<bool> task = Task.Factory.StartNew<bool>(() =>
            {
                FuncRunAttribute attr =
                    (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr == null)
                    attr =
                        (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr != null)
                {
                    try
                    {
                        attr.BeforeRun(mess);
                        Func();
                        attr.SuccessRun(mess,null);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        attr.ErrorRun(mess,ex);
                        return false;
                    }
                    finally
                    {
                        attr.AfterRun(mess);
                    }
                }
                else
                    Func();
                return true;
            });
            return task;
        }

        public static Task<T> RunAsync<T>(this Func<T> Func, IMess mess)
        {
            Task<T> task = Task.Factory.StartNew(() =>
            {
                FuncRunAttribute attr =
                    (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr == null)
                    attr =
                        (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr != null)
                {
                    try
                    {
                        attr.BeforeRun(mess);
                        T ouput = Func();
                        attr.SuccessRun(mess,ouput);
                        return ouput;
                    }
                    catch (Exception ex)
                    {
                        attr.ErrorRun(mess,ex);
                        return default(T);
                    }
                    finally
                    {
                        attr.AfterRun(mess);
                    }
                }
                else
                    return Func();
            });
            return task;
        }

        public static Task<T> RunAsync<T, T1>(this Func<T1, T> Func, IMess mess, T1 par1)
        {
            Task<T> task = Task.Factory.StartNew<T>(() =>
            {
                FuncRunAttribute attr =
                     (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr == null)
                    attr =
                        (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr != null)
                {
                    try
                    {
                        attr.BeforeRun(mess);
                        T ouput = Func(par1);
                        attr.SuccessRun(mess,ouput);
                        return ouput;
                    }
                    catch (Exception ex)
                    {
                        attr.ErrorRun(mess,ex);
                        return default(T);
                    }
                    finally
                    {
                        attr.AfterRun(mess);
                    }
                }
                else
                    return Func(par1);
            });
            return task;
        }

        public static Task<T> RunAsync<T, T1, T2>( this Func<T1, T2, T> Func, IMess mess, T1 par1, T2 par2)
        {

            Task<T> task = Task.Factory.StartNew(() =>
            {
                FuncRunAttribute attr =
                    (FuncRunAttribute)Func.Method.DeclaringType.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr == null)
                    attr =
                        (FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(FuncRunAttribute), true).FirstOrDefault();
                if (attr != null)
                {
                    try
                    {
                        attr.BeforeRun(mess);
                        T ouput = Func(par1,par2);
                        attr.SuccessRun(mess,ouput);
                        return ouput;
                    }
                    catch (Exception ex)
                    {
                        attr.ErrorRun(mess,ex);
                        return default(T);
                    }
                    finally
                    {
                        attr.AfterRun(mess);
                    }
                }
                else
                    return Func(par1,par2);
            });
            return task;
        }


    }
}
