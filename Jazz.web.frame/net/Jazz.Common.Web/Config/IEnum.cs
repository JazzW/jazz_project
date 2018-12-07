using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Web.Config
{
    public interface IEnum
    {
        string check(string col);

        string map(string col);
    }
}
