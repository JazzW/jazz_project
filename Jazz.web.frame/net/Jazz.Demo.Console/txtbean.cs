using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Demo.Console
{
    [Serializable]
    public class txtbean
    {
        public String guid;

        public DateTime logtime;

        public String Message;
        public txtbean()
        {
            guid = Guid.NewGuid().ToString();
            logtime = DateTime.Now;
        }

        public override string ToString()
        {
            return guid + " , " + logtime + " , " + Message;
        }
    }
}
