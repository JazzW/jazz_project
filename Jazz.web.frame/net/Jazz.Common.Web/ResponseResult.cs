using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Web
{

    /// <summary>
    /// 操作提示
    /// </summary>
    public class ResponseResult
    {
        protected int _State;
        protected String _Msg;
        protected String _Log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">1成功，2提示,3错误</param>
        /// <param name="msg"></param>
        public ResponseResult(int status, String msg)
        {
            this._State = status;
            this._Msg = msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">1成功，2提示,3错误</param>
        /// <param name="msg"></param>
        public ResponseResult(int status, String msg, String log)
        {
            this._State = status;
            this._Msg = msg;
            this._Log = log;
        }


        public ResponseResult(int obj)
        {
            switch (obj)
            {
                case 1:
                    this._Msg = "操作成功";
                    this.State = 1;
                    break;
                case 2:
                    this._Msg = "没有相关权限";
                    this.State = 2;
                    break;
                default:
                    this._Msg = "未知错误";
                    this.State = 3;
                    break;
            }
        }

        public ResponseResult() { }

        /// <summary>
        /// 状态 1成功，2提示,3错误
        /// </summary>
        public int State { get { return _State; } set { _State = value; } }

        /// <summary>
        /// 提示语
        /// </summary>
        public String Msg { get { return _Msg; } set { _Msg = value; } }

        /// <summary>
        /// 错误提示日志
        /// </summary>
        public String Log { get { return _Log; } set { _Log = value; } }

    }

    /// <summary>
    /// 操作提示（带返回数据）
    /// </summary>
    /// <typeparam name="X">数据类型</typeparam>
    public class ResponseResult<X> : ResponseResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="status">状态 1成功，2提示,3错误</param>
        /// <param name="msg">提示信息</param>
        /// <param name="data">返回数据</param>
        public ResponseResult(int status, String msg, X data)
        {
            this._State = status;
            this._Msg = msg;
            this.Data = data;
        }

        public ResponseResult() { }

        public X Data { get; set; }
    }

    /// <summary>
    /// 操作提示（带返回数据）
    /// </summary>
    /// <typeparam name="X">数据类型1</typeparam>
    /// <typeparam name="Y">数据类型2</typeparam>
    public class ResponseResult<X, Y> : ResponseResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="status">状态 1成功，2提示,3错误</param>
        /// <param name="msg">提示信息</param>
        /// <param name="data1">返回数据1</param>
        /// <param name="data2">返回数据2</param>
        public ResponseResult(int status, String msg, X data1, Y data2)
        {
            this._State = status;
            this._Msg = msg;
            this.Data1 = data1;
            this.Data2 = data2;
        }

        public X Data1 { get; set; }

        public Y Data2 { get; set; }
    }

    public class ResponseResult<X, Y, Z> : ResponseResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="status">状态 1成功，2提示,3错误</param>
        /// <param name="msg">提示信息</param>
        /// <param name="data1">返回数据1</param>
        /// <param name="data2">返回数据2</param>
        public ResponseResult(int status, String msg, X data1, Y data2, Z data3)
        {
            this._State = status;
            this._Msg = msg;
            this.Data1 = data1;
            this.Data2 = data2;
            this.Data3 = data3;
        }

        public X Data1 { get; set; }

        public Y Data2 { get; set; }

        public Z Data3 { get; set; }
    }
}
