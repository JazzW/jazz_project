using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFrameWork.ADO
{
    public abstract class IBaseBLL<T> where T : Models.IDBModel
    {

        /// <summary>
        /// 单原操做
        /// </summary>
        /// <param name="model">执行对象</param>
        /// <param name="opCode">执行编码 1:update  2:delete 3:add 4:select</param>
        /// <returns></returns>
        public delegate bool Objaction(T model, int opCode);

        /// <summary>
        /// 错误操做
        /// </summary>
        /// <param name="model"></param>
        /// <param name="opCode"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public delegate bool ExHandle(T model, int opCode, Exception ex);


    }

}
