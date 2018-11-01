using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Reflection;
using Spring.Transaction.Interceptor;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class ModuleHelper : IModuleHelper
    {
        public IViewHelper viewHelper { get; set; }
        public IModelService modelService { get; set; }

        public static void InitializeUXComponentForAllViews(IList<Cdc.MetaManager.DataAccess.Domain.Module> modules, Dictionary<Guid, IDomainObject> loadedObjects)
        {
            foreach (Cdc.MetaManager.DataAccess.Domain.Module module in modules)
            {
                DialogHelper.InitializeUXComponentForAllViews(module.Dialogs, loadedObjects);
            }
        }

        [Transaction(ReadOnly = true)]
        public void FindAllModulesAndServicesReferancedByModule(DataAccess.Domain.Module module, List<DataAccess.Domain.Module> dependantModules, List<Service> dependantServices)
        {
            module = modelService.GetDomainObject<DataAccess.Domain.Module>(module.Id);

            if (dependantModules == null) { dependantModules = new List<DataAccess.Domain.Module>(); }
            if (dependantServices == null) { dependantServices = new List<Service>(); }

            FindAllModulesAndServicesReferancedByModuleRecursion(module, dependantModules, dependantServices);
        }
                
        private void FindAllModulesAndServicesReferancedByModuleRecursion(DataAccess.Domain.Module module, List<DataAccess.Domain.Module> dependantModules, List<Service> dependantServices)
        {
            List<Cdc.MetaManager.DataAccess.Domain.Module> newDependencies = new List<Cdc.MetaManager.DataAccess.Domain.Module>();

            foreach (Dialog dialog in module.Dialogs)
            {
                List<View> views = new List<View>();

                if (dialog.SearchPanelView != null)
                {
                    views.Add(dialog.SearchPanelView);
                }


                foreach (ViewNode viewNode in dialog.ViewNodes)
                {
                    if (viewNode.View != null)
                    {
                        views.Add(viewNode.View);

                    }

                    foreach (ViewAction viewAction in viewNode.ViewActions)
                    {
                        if (viewAction.Type != ViewActionType.JumpTo)
                        {
                            if (viewAction.Action.Dialog != null)
                            {
                                if (!dependantModules.Contains(viewAction.Action.Dialog.Module))
                                {
                                    dependantModules.Add(viewAction.Action.Dialog.Module);
                                    newDependencies.Add(viewAction.Action.Dialog.Module);
                                }
                            }
                            else if (viewAction.Action.Workflow != null)
                            {
                                if (!dependantModules.Contains(viewAction.Action.Workflow.Module))
                                {
                                    dependantModules.Add(viewAction.Action.Workflow.Module);
                                    newDependencies.Add(viewAction.Action.Workflow.Module);
                                }
                            }
                            else if (viewAction.Action.ServiceMethod != null)
                            {
                                if (!dependantServices.Contains(viewAction.Action.ServiceMethod.Service))
                                {
                                    dependantServices.Add(viewAction.Action.ServiceMethod.Service);
                                }
                            }
                        }
                    }
                }

                foreach (View view in views)
                {
                    if (view.ServiceMethod != null)
                    {
                        if (!dependantServices.Contains(view.ServiceMethod.Service))
                        {
                            dependantServices.Add(view.ServiceMethod.Service);
                        }
                    }

                    foreach (DataSource dataSource in view.DataSources)
                    {
                        if (dataSource.ServiceMethod != null)
                        {
                            if (!dependantServices.Contains(dataSource.ServiceMethod.Service))
                            {
                                dependantServices.Add(dataSource.ServiceMethod.Service);
                            }
                        }
                    }

                    if (view.VisualTree != null)
                    {
                        foreach (UXComponent component in ViewHelper.GetAllComponents<UXComponent>(view.VisualTree))
                        {
                            List<PropertyInfo> props = component.GetType().GetProperties().Where(p => p.PropertyType == typeof(Dialog)).ToList();

                            if (props.Count() > 0)
                            {
                                viewHelper.InitializeUXComponent(component);

                                foreach (PropertyInfo info in props)
                                {
                                    Dialog dependency = info.GetValue(component, null) as Dialog;

                                    if (dependency != null)
                                    {
                                        if (!dependantModules.Contains(dependency.Module))
                                        {
                                            dependantModules.Add(dependency.Module);
                                            newDependencies.Add(dependency.Module);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                props = component.GetType().GetProperties().Where(p => p.PropertyType == typeof(ServiceMethod)).ToList();

                                if (props.Count() > 0)
                                {
                                    viewHelper.InitializeUXComponent(component);

                                    foreach (PropertyInfo info in props)
                                    {
                                        ServiceMethod dependency = info.GetValue(component, null) as ServiceMethod;

                                        if (dependency != null)
                                        {
                                            if (!dependantServices.Contains(dependency.Service))
                                            {
                                                dependantServices.Add(dependency.Service);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            foreach (Workflow workflow in module.Workflows)
            {
                foreach (WorkflowDialog workflowDialog in workflow.Dialogs)
                {
                    if (!dependantModules.Contains(workflowDialog.Dialog.Module))
                    {
                        dependantModules.Add(workflowDialog.Dialog.Module);
                        newDependencies.Add(workflowDialog.Dialog.Module);
                    }
                }

                foreach (WorkflowSubworkflow subworkflow in workflow.Subworkflows)
                {
                    if (!dependantModules.Contains(subworkflow.SubWorkflow.Module))
                    {
                        dependantModules.Add(subworkflow.SubWorkflow.Module);
                        newDependencies.Add(subworkflow.SubWorkflow.Module);
                    }
                }

                foreach (WorkflowServiceMethod workflowServiceMethod in workflow.ServiceMethods)
                {
                    if (!dependantServices.Contains(workflowServiceMethod.ServiceMethod.Service))
                    {
                        dependantServices.Add(workflowServiceMethod.ServiceMethod.Service);
                    }
                }
            }

            foreach (DataAccess.Domain.Module dependency in newDependencies)
            {
                FindAllModulesAndServicesReferancedByModuleRecursion(dependency, dependantModules, dependantServices);
            }
        }
    }
}
