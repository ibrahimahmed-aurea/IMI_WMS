using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Views
{
    public class InlineDetailPresenter : Presenter<IInlineDetailView>
    {
        
        [ServiceDependency]
        public IActionCatalogService ActionCatalog
        {
            get;
            set;
        }
        
        [ServiceDependency]
        public IShellInteractionService ShellInteractionService { get; set; }

        public bool CanActionExecute(string actionName)
        {
            return ActionCatalog.CanExecute(actionName, WorkItem, this, null);
        }
                
        public bool ExecuteAction(string actionName)
        {
            try
            {
                ShellInteractionService.ShowProgress();

                try
                {
                    ActionCatalog.Execute(actionName, WorkItem, this, null);
                    return true;
                }
                finally
                {
                    ShellInteractionService.HideProgress();
                }
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                ShellInteractionService.ShowMessageBox(StringResources.ActionException_Text, ex.Message, ex.ToString(), Infrastructure.MessageBoxButton.Ok, Infrastructure.MessageBoxImage.Error);
            }

            return false;
        }
    }
}
