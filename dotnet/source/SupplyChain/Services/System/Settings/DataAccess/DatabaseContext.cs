using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using Imi.Framework.DataAccess;

namespace Imi.SupplyChain.Settings.DataAccess
{
    public class DatabaseContext
    {
        private static IApplicationContext ctx = null;

        protected static IApplicationContext Context
        {
            get
            {
                // Get application service context
                if (ctx == null)
                {
                    ctx = ContextRegistry.GetContext();
                }

                return ctx;
            }
        }

        public static T1 CreateDao<T1>()
        {
            string typeName = typeof(T1).Name;
            return (T1)Context[typeName];
        }

        public static void Initialise()
        {
            // Just make sure we initialize
            IApplicationContext c = Context;
        }

        public static ITransactionScope CreateTransactionScope()
        {
            return Context["ITransactionScope"] as ITransactionScope;
        }
    }
}
