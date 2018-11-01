using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX.Services;
using Microsoft.Practices.CompositeUI;
using System.Threading;

namespace Imi.SupplyChain.UX
{
    public class AuthenticatedUserCondition : IActionCondition
    {
        public bool CanExecute(string action, WorkItem context, object caller, object target)
        {
            bool result = Thread.CurrentPrincipal.Identity.IsAuthenticated;
            return result;
        }
    }

    public class NotImplemented : IActionCondition
    {
        public bool CanExecute(string action, WorkItem context, object caller, object target)
        {
            return false;
        }
    }
       
    public class DataAvailableInGrid : IActionCondition
    {
        private Type dataType;

        public DataAvailableInGrid(Type type)
        {
            this.dataType = type;
        }

        public bool CanExecute(string action, WorkItem context, object caller, object target)
        {
            ICollection<object> data = context.Items.FindByType(dataType);
            bool result = (data.Count > 0) ;
            return result;
        }
    }
}
