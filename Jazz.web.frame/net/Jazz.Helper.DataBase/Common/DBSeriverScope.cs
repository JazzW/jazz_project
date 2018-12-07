using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Jazz.Helper.DataBase.Common
{
    public class DBSeriverScope
    {
        public static bool run(DBAction[] actions)
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            options.Timeout = new TimeSpan(0, 2, 0);
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    Parallel.ForEach<DBAction>(actions, (act) =>
                    {
                        act.execute();
                    });

                    scope.Complete();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool run(DBAction action)
        {
            try
            {
                action.execute();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
