using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Utility;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.Threading;

namespace Imi.SupplyChain.UX.Services
{
    public class ActionProviderService : IActionProviderService
    {
        private class AuthOperation : IAuthOperation
        {
            public AuthOperation()
            {
            }

            public bool isDrillDown { get; set; }

            public ShellAction Action { get; set; }
                        
            private bool isAuthorized;

            public bool IsAuthorized
            {
                get
                {
                    return isAuthorized;
                }
                set
                {
                    isAuthorized = value;
                }
            }

            private string operation;

            public string Operation
            {
                get
                {
                    return operation;
                }
                set
                {
                    operation = value;
                }
            }
        }

        [ServiceDependency]
        public IActionCatalogService ActionCatalogService
        { 
            get;
            set; 
        }

        [ServiceDependency]
        public IAuthorizationService AuthorizationService 
        { 
            get; 
            set; 
        }

        [ServiceDependency]
        public WorkItem WorkItem
        { 
            get; 
            set; 
        }
                        
        
        private string applicationName;
        private SynchronizationContext currentContext;

        private Dictionary<object, IList<ShellAction>> actionsDictionary;
        private IList<ShellAction> authorizedActions;
        private IList<ShellAction> unauthorizedActions;

        private Dictionary<object, IList<ShellAction>> drillDownActionsDictionary;
        private IList<ShellAction> authorizedDrillDownActions;
        private IList<ShellAction> unauthorizedDrillDownActions;
        
        public ActionProviderService(string applicationName)
        {
            this.applicationName = applicationName;
            currentContext = SynchronizationContext.Current;

            actionsDictionary = new Dictionary<object, IList<ShellAction>>();
            authorizedActions = new List<ShellAction>();
            unauthorizedActions = new List<ShellAction>();

            drillDownActionsDictionary = new Dictionary<object, IList<ShellAction>>();
            authorizedDrillDownActions = new List<ShellAction>();
            unauthorizedDrillDownActions = new List<ShellAction>();
        }

        public ICollection<ShellAction> GetActions(object owner)
        {
            if (actionsDictionary.ContainsKey(owner))
                return actionsDictionary[owner];
            else
                return new List<ShellAction>();
        }

        public ShellAction GetDrillDownAction(object owner, string actionId)
        {
            if (drillDownActionsDictionary.ContainsKey(owner))
            {
                return drillDownActionsDictionary[owner].Where(a => a.Id == actionId).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public void RegisterAction(object owner, string name, string caption)
        {
            RegisterAction(owner, name, caption,null);
        }

        public void RegisterAction(object owner, string name, string caption, string operation)
        {
            Guard.ArgumentNotNullOrEmptyString(name, "name");
            Guard.ArgumentNotNullOrEmptyString(caption, "caption");
            Guard.ArgumentNotNull(owner, "owner");

            ShellAction action = new ShellAction(WorkItem) { Id = name, Caption = caption, Operation = operation, IsAuthorized = true, IsEnabled = true };

            if (string.IsNullOrEmpty(operation))
            {
                authorizedActions.Add(action);
            }
            else
            {
                action.IsEnabled = false;
                unauthorizedActions.Add(action);
            }

            if (!actionsDictionary.ContainsKey(owner))
                actionsDictionary[owner] = new List<ShellAction>();
                        
            actionsDictionary[owner].Add(action);
        }

        public void RegisterDrillDownAction(object owner, string name, string caption)
        {
            RegisterDrillDownAction(owner, name, caption, null);
        }

        public void RegisterDrillDownAction(object owner, string name, string caption, string operation)
        {
            Guard.ArgumentNotNullOrEmptyString(name, "name");
            Guard.ArgumentNotNullOrEmptyString(caption, "caption");
            Guard.ArgumentNotNull(owner, "owner");

            ShellAction action = new ShellAction(WorkItem) { Id = name, Caption = caption, Operation = operation, IsAuthorized = true, IsEnabled = true };

            if (string.IsNullOrEmpty(operation))
            {
                authorizedDrillDownActions.Add(action);
            }
            else
            {
                action.IsEnabled = false;
                action.IsAuthorized = false;
                unauthorizedDrillDownActions.Add(action);
            }

            if (!drillDownActionsDictionary.ContainsKey(owner))
                drillDownActionsDictionary[owner] = new List<ShellAction>();

            drillDownActionsDictionary[owner].Add(action);
        }
                        
        public void UpdateActions(object owner)
        {
            if (unauthorizedActions.Count > 0 || unauthorizedDrillDownActions.Count > 0)
            {
                List<AuthOperation> operationsList = new List<AuthOperation>();
                
                operationsList.AddRange(unauthorizedActions.Select(a => new AuthOperation() { Operation = a.Operation, Action = a, isDrillDown = false }));

                operationsList.AddRange(unauthorizedDrillDownActions.Select(a => new AuthOperation() { Operation = a.Operation, Action = a, isDrillDown = true }));
                
                unauthorizedActions.Clear();
                unauthorizedDrillDownActions.Clear();

                ThreadPool.QueueUserWorkItem(CheckAuthorization, operationsList);
            }
                       
            if (actionsDictionary.ContainsKey(owner))
            {
                foreach (ShellAction action in actionsDictionary[owner].Intersect(authorizedActions))
                {
                    action.IsEnabled = ActionCatalogService.CanExecute(action.Id, action.WorkItem, this, action) && action.IsAuthorized;
                }
            }
        }

        public object ExecuteSpecialFunction(string action, string name, object[] args, WorkItem context)
        {
            if (ActionCatalogService != null)
            {
                return ActionCatalogService.ExecuteSpecialFunction(action, name, args, context);
            }

            return null;
        }


        private void CheckAuthorization(object state)
        {
            //IList<ShellAction> actions = state as IList<ShellAction>;

            //var ops = (from a in actions
                      //select new AuthOperation() { Operation = a.Operation, Action = a } as IAuthOperation).ToArray();

            IAuthOperation[] ops = ((List<AuthOperation>)state).ToArray();

            try
            {
                AuthorizationService.CheckAuthorization(applicationName, ops);

                currentContext.Send(delegate(object data)
                {
                    foreach (AuthOperation operation in ops)
                    {
                        ShellAction action = operation.Action;

                        if (operation.isDrillDown)
                        {
                            authorizedDrillDownActions.Add(action);
                        }
                        else
                        {
                            authorizedActions.Add(action);
                        }

                        action.IsAuthorized = operation.IsAuthorized;
                        action.IsEnabled = ActionCatalogService.CanExecute(action.Id, action.WorkItem, this, action);
                    }
                }, null);
            }
            catch (Exception ex)
            { 
                currentContext.Send(delegate(object data)
                {
                    throw ex;
                }, null);
            }
        }
    }
}
