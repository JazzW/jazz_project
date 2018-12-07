using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using ADO= WebFrameWork.ADO ;
using EF=WebFrameWork.EF;

namespace WebFrameWork.Helper
{
    public static class CommonHelper<T>
    {
        public static T Run(Func<T> Func)
        {

            ADO.Models.DBAttr.FuncRunAttribute attr =
                (ADO.Models.DBAttr.FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(ADO.Models.DBAttr.FuncRunAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                try
                {
                    attr.BeforeRun();
                    T ouput = Func();
                    attr.SuccessRun();
                    return ouput;
                }
                catch (Exception ex)
                {
                    attr.ErrorRun(ex);
                    return default(T);
                }
                finally
                {
                    attr.AfterRun();
                }
            }
            else
                return Func();
        }

        public static T Run<T1>(Func<T1,T> Func,T1 par1)
        {

            ADO.Models.DBAttr.FuncRunAttribute attr =
                (ADO.Models.DBAttr.FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(ADO.Models.DBAttr.FuncRunAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                try
                {
                    attr.BeforeRun();
                    T ouput = Func(par1);
                    attr.SuccessRun();
                    return ouput;
                }
                catch (Exception ex)
                {
                    attr.ErrorRun(ex);
                    return default(T);
                }
                finally
                {
                    attr.AfterRun();
                }
            }
            else
                return Func(par1);
        }

        public static T Run<T1,T2>(Func<T1,T2, T> Func, T1 par1,T2 par2)
        {

            ADO.Models.DBAttr.FuncRunAttribute attr =
                (ADO.Models.DBAttr.FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(ADO.Models.DBAttr.FuncRunAttribute), true).FirstOrDefault();
            if (attr != null)
            {

                try
                {
                    attr.BeforeRun();
                    T ouput = Func(par1,par2);
                    attr.SuccessRun();
                    return ouput;
                }
                catch (Exception ex)
                {
                    attr.ErrorRun(ex);
                    return default(T);
                }
                finally
                {
                    attr.AfterRun();
                }
            }
            else
                return Func(par1,par2);
        }

        public static Task<T> RunAsync(Func<T> Func)
        {
            Task<T> task =new Task<T>(() =>
            {
                ADO.Models.DBAttr.FuncRunAttribute attr =
                (ADO.Models.DBAttr.FuncRunAttribute)Func.Method.GetCustomAttributes(typeof(ADO.Models.DBAttr.FuncRunAttribute), true).FirstOrDefault();
                if (attr != null)
                {
                    try
                    {
                        attr.BeforeRun();
                        T ouput = Func();
                        attr.SuccessRun();
                        return ouput;
                    }
                    catch (Exception ex)
                    {
                        attr.ErrorRun(ex);
                        return default(T);
                    }
                    finally
                    {
                        attr.AfterRun();
                    }
                }
                else
                    return Func();
            });
            return task;
        }

        public static async void RunAsync(Action act, Action callback)
        {
            Func<Task> taskfunc = () =>
            {
                return Task.Run(() => { act(); });
            };
            await taskfunc();
            if (callback != null)
                callback();

        }

        public static async void RunAsync(Func<T> Func, Action<T> callback)
        {
            Func<Task<T>> taskfunc = () =>
            {
                return Task.Run(() => { return Func(); });
            };
            T alt = await taskfunc();
            if (callback != null)
                callback(alt);
        }

        public static Task<T> newTask(Func<T> func)
        {
            return new Task<T>(func);
        }

        public static Task newTask(Action act)
        {
            return new Task(act);
        }

        public static List<T> getNodes(string xml, Func<XElement, T> Tran)
        {
            XElement xel = XElement.Parse(xml);
            List<T> output = new List<T>();
            foreach (var item in xel.Nodes())
            {
                XElement xe = item as XElement;
                output.Add(Tran(xe));
            }
            return output;

        }

        public static string getNodeTxt(string xml, Func<XElement, bool> func, XName xname)
        {
            try
            {
                XElement xel = XElement.Parse(xml);
                var result = xel.Elements().Where(func).ToList();

                return result[0].Attribute(xname).Value;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static T CopyeFromJson(string jsonData)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var result = js.Deserialize<T>(jsonData);
                return result;
            }
            catch
            {
                throw (new Exception("输入参数存在错误"));
            }
        }

    }

    public class VerifyCode
    {
        public static byte[] GetVerifyCode(ref string code)
        {
            int codeW = 80;
            int codeH = 30;
            int fontSize = 16;
            string chkCode = string.Empty;
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.DarkBlue };
            string[] font = { "Times New Roman" };
            char[] character = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            //生成验证码字符串
            Random rnd = new Random();
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            //写入Session用于验证码校验，可以对校验码进行加密，提高安全性
            code = chkCode;

            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            //画噪线
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);

                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18, (float)0);
            }
            //将验证码写入图片内存流中，以image/png格式输出
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }
    }
}
