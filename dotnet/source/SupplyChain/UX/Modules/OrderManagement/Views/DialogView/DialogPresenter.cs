using System;
using System.Collections.Generic;
using System.Linq;
using dvSlave;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Modules.OrderManagement.Views.Constants;
using Imi.SupplyChain.UX.Shell.Services;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Configuration;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.SupplyChain.UX.Shell.Views;
using Imi.SupplyChain.UX.Shell;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public class DialogPresenter : Presenter<IDialogView>
    {
        WorkItem workItem;
        IList<ShellAction> actions = new List<ShellAction>();

        private static string ASSEMBLY_FILE = "Imi.SupplyChain.UX.Modules.OrderManagement.dll";

        [EventPublication(Imi.SupplyChain.UX.Shell.EventTopicNames.StartMenuItemExecuted, PublicationScope.Global)]
        public event EventHandler<StartMenuItemExecutedEventArgs> MenuItemExecuted;

        [ServiceDependency]
        public IActionCatalogService ActionCatalog { get; set; }

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService { get; set; }

        [ServiceDependency]
        public IModuleLoaderService ModuleLoaderService { get; set; }

        [ServiceDependency]
        public IShellModuleService ShellModuleService { get; set; }

        public DialogPresenter([ServiceDependency] WorkItem workItem)
        {
            this.workItem = workItem;
        }

        public override void OnViewShow()
        {
            View.SetFocus();
        }


        private const string Separator= "||";

        // Called when action list is to be updated on screen
        public void ShowActions(string actionString)
        {
            //System.Diagnostics.Debug.WriteLine("Entering ShowActions on Dialog " + View.Id + ",    " + actionString);

            ICollection<ShellAction> serviceActions = ShellInteractionService.Actions;

            // remove old Action data
            ICollection<ShellAction> tempActions = new List<ShellAction>(); ;
            foreach (ShellAction shellAction in serviceActions)
            {
                tempActions.Add(shellAction);
            }

            foreach (ShellAction shellAction in tempActions)
            {
                ActionCatalog.RemoveActionImplementation(shellAction.Id);
                serviceActions.Remove(shellAction);
            }
            actions.Clear();
            if (actionString.Length == 0)
                return;
            
            // decode actionString and add new Actions
            char[] delimiter = {';'};
            string[] actionsArray = actionString.Split(delimiter);
            ShellAction action;
            for (int i = 0; i < actionsArray.Length - 1; i = i + 2)
            {
                action = new ShellAction(workItem);
                action.Id = (string)actionsArray.GetValue(i) + Separator + View.Id; // make sure Id is unique!
                action.Caption = (string)actionsArray.GetValue(i + 1);
                action.IsAuthorized = true;
                action.IsEnabled = true;
                action.Parameters = action.Id;
                actions.Add(action);

                ActionCatalog.RegisterActionImplementation(action.Id, OnAction);
                serviceActions.Add(action);
                //System.Diagnostics.Debug.WriteLine("action " + action.Caption + " was added");
            }
        }

        public IList<ShellAction> GetActions()
        {
            return actions;
        }

        // called when user clicks an action
        public void OnAction(WorkItem context, object caller, object target)
        {
            string actionKey = (string) target;
            //System.Diagnostics.Debug.WriteLine(actionKey + " selected on Dialog " +  View.Id);
            object content = View.TrimControl;
            if (content is inUC)
            {
                inUC trimProgram = content as inUC;
                int pos = actionKey.IndexOf(Separator);
                string key = actionKey.Substring(0, pos);
                trimProgram.ipc(1, key);
            }
        }

        public void SaveSearch(string searchStr)
        {
            char[] delimiter = { ';' };
            string[] strArray = searchStr.Split(delimiter);
            string caption = strArray[0];
            string programName = strArray[1];
            string parameters = strArray[2];
 
            ShellMenuItem item = new ShellMenuItem();
            item.IsEnabled = true;
            item.IsAuthorized = true;
            item.Operation = programName;
            item.Id = programName;
            item.Caption = caption;
            item.EventTopic = Imi.SupplyChain.UX.Modules.OrderManagement.Views.Constants.EventTopicNames.StartOMSProgramWithData;
            item.Parameters = parameters;
            item.AssemblyFile = ASSEMBLY_FILE;

            ShellInteractionService.AddToFavorites(item);
        }

        public void SaveInDashboard(string searchStr)
        {
            char[] delimiter = { ';' };
            string[] strArray = searchStr.Split(delimiter);
            string programName = strArray[0];
            string parameters = strArray[1];

            ShellDrillDownMenuItem dItem = new ShellDrillDownMenuItem();
            dItem.IsEnabled = true;
            dItem.IsAuthorized = true;
            dItem.Operation = programName;
            dItem.Id = programName;
            dItem.EventTopic = Imi.SupplyChain.UX.Modules.OrderManagement.Views.Constants.EventTopicNames.StartOMSProgramWithData;
            dItem.Parameters = parameters;
            dItem.AssemblyFile = ASSEMBLY_FILE;

            StartMenuItemExecutedEventArgs eventArgs = new StartMenuItemExecutedEventArgs(dItem, null, StartOption.Dashboard);
            eventArgs.Module = ShellModuleService.ActiveModule;

            EventHandler<StartMenuItemExecutedEventArgs> temp = MenuItemExecuted;

            // When adding dialogs to the Dashboard we need to be sure that the Dashboard module activation is finished.
            // To be abel to know this we temporarly listen to the ModuleActivated event and then activate the module.
            // When the activation thread is done we get the event and can then fire the MenuItemExecuted event.
            // This procedure is a must when we add a dialog without first activating the Dashboard in the GUI.

            EventHandler<DataEventArgs<IShellModule>> moduleActivatedHandler = null;
            moduleActivatedHandler = (s, e1) =>
            {
                if (e1.Data.Id == DashboardModule.ModuleId && temp != null)
                {
                    temp(this, eventArgs);
                    temp = null;
                    ShellModuleService.ModuleActivated -= moduleActivatedHandler;
                }
            };

            ShellModuleService.ModuleActivated += moduleActivatedHandler;
            ShellModuleService.ActiveModule = ShellModuleService.Modules.Where((m) => { return m.Id == DashboardModule.ModuleId; }).FirstOrDefault();
        }
    }
}
