using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZ.ILogger
{
    /// <summary>
    /// 消息记录的基类
    /// </summary>
    public  interface InterfaceLogger
    {
        /// <summary>
        /// 异常信息记录
        /// </summary>
        /// <param name="message"></param>
        void Error(String message);

        /// <summary>
        /// 一般信息记录
        /// </summary>
        /// <param name="message"></param>
        void Info(String message);

    }

    /// <summary>
    /// 全局记录对象工厂
    /// </summary>
    public class LoggerFactory
    {
        private static InterfaceLogger _log;

        /// <summary>
        /// 获得logger对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T getLogger<T>() where T : InterfaceLogger
        {
            return (T)_log;
        }

        /// <summary>
        /// 获得logger对象
        /// </summary>
        /// <returns></returns>
        public static InterfaceLogger getLogger()
        {
            return _log;
        }

        /// <summary>
        /// 设置logger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        public static void setLogger<T>(T logger) where T : InterfaceLogger
        {
            _log = logger;
        }
    }
}
