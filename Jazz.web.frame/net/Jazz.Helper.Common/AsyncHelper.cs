using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Helper.Common
{
    public class AsyncHelper
    {

        public static List<Object> runAsync(params Func<Object>[] funcs)
        {
            List<Task<Object>> tasks = new List<Task<Object>>();
            foreach (var func in funcs)
            {
                tasks.Add(new Task<Object>(func));
            }

            Task.WaitAll(tasks.ToArray());

            List<Object> res = new List<object>();
            foreach (var task in tasks)
            {
                res.Add(task.Result);
            }

            return res;
        }

    }
}
