using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazz.Common.Web
{
    public abstract class IUserModel
    {
        public virtual string UserName { get; set; }

        public virtual string UserID { get; set; }

        public virtual string UserRole { get; set; }

        public virtual bool IsNull()
        {
            if (string.IsNullOrWhiteSpace(UserID) && string.IsNullOrWhiteSpace(UserRole))
                return false;
            return true;
        }
    }
}
