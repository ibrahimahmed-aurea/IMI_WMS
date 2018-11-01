using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using Cdc.MetaManager.DataAccess;
using System.Diagnostics;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Reflection;
using Domain = Cdc.MetaManager.DataAccess.Domain;
using Spring.Data.NHibernate.Support;
using Spring.Data.NHibernate;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using Cdc.MetaManager.BusinessLogic.Helpers;

namespace Cdc.MetaManager.BusinessLogic
{
    public class DialogService : IDialogService
    {
        public IDialogDao DialogDao { get; set; }
        public IModuleDao ModuleDao { get; set; }
        public IPropertyMapDao PropertyMapDao { get; set; }
        public IViewDao ViewDao { get; set; }
        public IViewActionDao ViewActionDao { get; set; }
        public IMappedPropertyDao MappedPropertyDao { get; set; }
        public IQueryPropertyDao QueryPropertyDao { get; set; }
        public IStoredProcedurePropertyDao StoredProcedurePropertyDao { get; set; }
        public IServiceMethodDao ServiceMethodDao { get; set; }
        public IQueryDao QueryDao { get; set; }
        public IStoredProcedureDao StoredProcedureDao { get; set; }
        public IActionDao ActionDao { get; set; }
        public IViewNodeDao ViewNodeDao { get; set; }
        public IUXSessionDao UXSessionDao { get; set; }
        public IPropertyDao PropertyDao { get; set; }
        public IUXActionDao UXActionDao { get; set; }
        public IBusinessEntityDao BusinessEntityDao { get; set; }
        public IApplicationService ApplicationService { get; set; }
        public ICustomDialogDao CustomDialogDao { get; set; }
        public IMenuService MenuService { get; set; }
        public IDataSourceDao DataSourceDao { get; set; }
        public IIssueDao IssueDao { get; set; }
        public IWorkflowDao WorkflowDao { get; set; }
        public IWorkflowDialogDao WorkflowDialogDao { get; set; }
        public IWorkflowSubworkflowDao WorkflowSubworkflowDao { get; set; }
        public IHintDao HintDao { get; set; }
        public IReportDao ReportDao { get; set; }
        public IViewHelper ViewHelper { get; set; }

        private void WalkTheNodeTree(ViewNode node)
        {
            NHibernateUtil.Initialize(node);

            InitializeView(node.View);

            foreach (ViewAction viewAction in node.ViewActions)
            {
                NHibernateUtil.Initialize(viewAction);
                NHibernateUtil.Initialize(viewAction.Action);
            }

            foreach (ViewNode child in node.Children)
            {
                WalkTheNodeTree(child);
            }
        }

        [Transaction(ReadOnly = true)]
        public IList<UXSessionProperty> GetUXSessionProperties(Application application)
        {
            return ApplicationService.GetUXSessionProperties(application);
        }

        [Transaction(ReadOnly = true)]
        public void GetViewRequestMap(View view, out PropertyMap requestMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            View readView = GetViewById(view.Id);

            sourceProperties = new List<IMappableProperty>();
            targetProperties = new List<IMappableProperty>();

            if (readView.Type == ViewType.Standard)
            {
                if ((readView.ServiceMethod != null) && (readView.ServiceMethod.RequestMap != null))
                {
                    targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.ServiceMethod.RequestMap).MappedProperties.Cast<IMappableProperty>());
                    sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.ServiceMethod.RequestMap).MappedProperties.Cast<IMappableProperty>());
                }
            }
            else if (readView.Type == ViewType.Custom)
            {
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.RequestMap).MappedProperties.Cast<IMappableProperty>());
                sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.RequestMap).MappedProperties.Cast<IMappableProperty>());
            }

            IList<UXSessionProperty> sessionProperties = ApplicationService.GetUXSessionProperties(readView.Application);

            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            requestMap = readView.RequestMap;

            MetaManagerUtil.InitializePropertyMap(requestMap);
        }


        [Transaction(ReadOnly = false)]
        public Workflow GetWorkflowById(Guid workflowId)
        {
            Workflow workflow = WorkflowDao.FindById(workflowId);

            NHibernateUtil.Initialize(workflow.Dialogs);
            NHibernateUtil.Initialize(workflow.ServiceMethods);
            NHibernateUtil.Initialize(workflow.Subworkflows);
            NHibernateUtil.Initialize(workflow.Module);
            NHibernateUtil.Initialize(workflow.Module.Application);

            MetaManagerUtil.InitializePropertyMap(workflow.RequestMap);

            return workflow;
        }

        [Transaction(ReadOnly = true)]
        public Cdc.MetaManager.DataAccess.Domain.Module GetModuleById(Guid moduleId)
        {
            Cdc.MetaManager.DataAccess.Domain.Module module = ModuleDao.FindById(moduleId);

            NHibernateUtil.Initialize(module.Application);

            return module;
        }


        [Transaction(ReadOnly = true)]
        public void GetViewToActionMap(ViewAction viewAction, out PropertyMap viewToActionMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            ViewAction readViewAction = ViewActionDao.FindById(viewAction.Id);

            PropertyMap actionRequestMap = readViewAction.Action.RequestMap;

            if (actionRequestMap != null)
            {
                sourceProperties = new List<IMappableProperty>(
                    (from MappedProperty p in MetaManagerUtil.InitializePropertyMap(actionRequestMap).MappedProperties
                     where !(p.Target is UXSessionProperty)
                     select p).Cast<IMappableProperty>());
            }
            else
            {
                sourceProperties = new List<IMappableProperty>();
            }

            PropertyMap viewResponseMap = readViewAction.ViewNode.View.ResponseMap;

            if ((viewResponseMap != null) && (viewResponseMap != null))
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(viewResponseMap).MappedProperties.Cast<IMappableProperty>());
            else
                targetProperties = new List<IMappableProperty>();

            IList<UXSessionProperty> sessionProperties = ApplicationService.GetUXSessionProperties(readViewAction.ViewNode.View.Application);

            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            viewToActionMap = readViewAction.ViewToActionMap;

            MetaManagerUtil.InitializePropertyMap(viewToActionMap);
        }

        [Transaction(ReadOnly = true)]
        public void GetServiceComponentMap(View view, UXServiceComponent serviceComponent, out PropertyMap componentMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            View readView = GetViewById(view.Id);

            ServiceMethod serviceMethod = this.ApplicationService.GetServiceMethodMapsById(serviceComponent.ServiceMethod.Id);

            if (serviceMethod != null)
                sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(serviceMethod.RequestMap).MappedProperties.Cast<IMappableProperty>());
            else
                sourceProperties = new List<IMappableProperty>();

            if (view.ResponseMap != null)
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.ResponseMap).MappedProperties.Cast<IMappableProperty>());
            else
                targetProperties = new List<IMappableProperty>();

            IList<UXSessionProperty> sessionProperties = this.ApplicationService.GetUXSessionProperties(readView.Application);

            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            if (serviceComponent.ComponentMap == null)
                componentMap = null;
            else
                componentMap = MetaManagerUtil.InitializePropertyMap(PropertyMapDao.FindById(serviceComponent.ComponentMap.Id));
        }

        [Transaction(ReadOnly = true)]
        public ViewNode GetParentViewNode(ViewNode viewNode)
        {
            ViewNode readViewNode = ViewNodeDao.FindById(viewNode.Id);

            ViewNode parentNode = readViewNode.Parent;

            while (parentNode != null)
            {
                if ((parentNode.View.ResponseMap != null) && (parentNode.View.ResponseMap.MappedProperties.Count > 0))
                    break;

                parentNode = parentNode.Parent;
            }

            if (parentNode != null)
            {
                NHibernateUtil.Initialize(parentNode.View.Application);
                NHibernateUtil.Initialize(parentNode.View.BusinessEntity);
                MetaManagerUtil.InitializePropertyMap(parentNode.View.ResponseMap);
            }

            return parentNode;
        }

        [Transaction(ReadOnly = true)]
        public void GetViewNodeMap(ViewNode viewNode, out PropertyMap viewNodeMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            ViewNode readViewNode = ViewNodeDao.FindById(viewNode.Id);

            if (readViewNode.View.RequestMap != null)
            {
                sourceProperties = new List<IMappableProperty>(
                    (from MappedProperty p in MetaManagerUtil.InitializePropertyMap(readViewNode.View.RequestMap).MappedProperties
                     where !(p.Target is UXSessionProperty)
                     select p).Cast<IMappableProperty>());
            }
            else
            {
                sourceProperties = new List<IMappableProperty>();
            }

            ViewNode parentNode = readViewNode.Parent;

            while (parentNode != null)
            {
                if ((parentNode.View.ResponseMap != null) && (parentNode.View.ResponseMap.MappedProperties.Count > 0))
                    break;

                parentNode = parentNode.Parent;
            }

            if (parentNode != null)
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(parentNode.View.ResponseMap).MappedProperties.Cast<IMappableProperty>());
            else
                targetProperties = new List<IMappableProperty>();

            IList<UXSessionProperty> sessionProperties = ApplicationService.GetUXSessionProperties(readViewNode.View.Application);

            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            viewNodeMap = readViewNode.ViewMap;

            if (viewNodeMap != null)
                MetaManagerUtil.InitializePropertyMap(viewNodeMap);
        }

        [Transaction(ReadOnly = true)]
        public void GetViewResponseMap(View view, out PropertyMap responseMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> requestProperties, out IList<IMappableProperty> targetProperties)
        {
            View readView = GetViewById(view.Id);

            sourceProperties = new List<IMappableProperty>();
            targetProperties = new List<IMappableProperty>();
            requestProperties = new List<IMappableProperty>();

            if (readView.RequestMap != null)
            {
                requestProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.RequestMap).MappedProperties.Cast<IMappableProperty>());
            }

            if (readView.Type == ViewType.Standard)
            {
                if ((readView.ServiceMethod != null) && (readView.ServiceMethod.ResponseMap != null))
                {
                    sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.ServiceMethod.ResponseMap).MappedProperties.Cast<IMappableProperty>());
                }
            }

            targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.ResponseMap).MappedProperties.Cast<IMappableProperty>());

            IList<UXSessionProperty> sessionProperties = ApplicationService.GetUXSessionProperties(readView.Application);

            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            responseMap = readView.ResponseMap;

            MetaManagerUtil.InitializePropertyMap(responseMap);
        }


        [Transaction(ReadOnly = false)]
        public void SaveOrUpdateViewNodeMap(ViewNode viewNode, PropertyMap viewNodeMap)
        {
            ViewNode readViewNode = ViewNodeDao.FindById(viewNode.Id);

            if (viewNodeMap != null)
            {
                PropertyMapDao.SaveOrUpdate(viewNodeMap);
                readViewNode.ViewMap = viewNodeMap;
            }

            ViewNodeDao.SaveOrUpdate(readViewNode);
        }

        [Transaction(ReadOnly = true)]
        public Dialog GetDialogWithViewTree(Guid dialogId)
        {
            Dialog dialog = DialogDao.FindById(dialogId);

            if (dialog != null)
            {
                NHibernateUtil.Initialize(dialog);
                NHibernateUtil.Initialize(dialog.SearchPanelView);
                NHibernateUtil.Initialize(dialog.InterfaceView);
                NHibernateUtil.Initialize(dialog.Module);

                if (dialog != null && dialog.RootViewNode != null)
                {
                    WalkTheNodeTree(dialog.RootViewNode);
                }
            }

            return dialog;
        }

        [Transaction(ReadOnly = true)]
        public void CreateViewServiceMethodMap(View view, Guid serviceMethodId)
        {

            ServiceMethod serviceMethod = ServiceMethodDao.FindById(serviceMethodId);

            view.RequestMap = new PropertyMap();
            view.ResponseMap = new PropertyMap();

            // Check if we need to loop the properties in map.
            if (view.ServiceMethod != null)
            {
                foreach (MappedProperty sourceProperty in serviceMethod.RequestMap.MappedProperties)
                {
                    MappedProperty mappedProperty = new MappedProperty();

                    mappedProperty.Source = sourceProperty;
                    mappedProperty.Target = sourceProperty.Target;
                    mappedProperty.Name = sourceProperty.Name;
                    mappedProperty.Sequence = sourceProperty.Sequence;
                    mappedProperty.PropertyMap = view.RequestMap;
                    mappedProperty.IsSearchable = true;
                    view.RequestMap.MappedProperties.Add(mappedProperty);

                }

                foreach (MappedProperty sourceProperty in serviceMethod.ResponseMap.MappedProperties)
                {
                    MappedProperty mappedProperty = new MappedProperty();

                    mappedProperty.Source = sourceProperty;
                    mappedProperty.Target = sourceProperty.Target;
                    mappedProperty.Name = sourceProperty.Name;
                    mappedProperty.Sequence = sourceProperty.Sequence;
                    mappedProperty.PropertyMap = view.ResponseMap;
                    view.ResponseMap.MappedProperties.Add(mappedProperty);


                }
            }
        }


        [Transaction(ReadOnly = true)]
        public Dialog GetDialog(Guid dialogId)
        {
            Dialog dialog = DialogDao.FindById(dialogId);

            if (dialog != null)
            {
                NHibernateUtil.Initialize(dialog);
            }

            return dialog;
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> FindDialogsByNameAndModule(Guid applicationId, string moduleName, string dialogName)
        {
            return DialogDao.FindDialogsByNameAndModule(applicationId, moduleName, dialogName);
        }

        [Transaction(ReadOnly = true)]
        public IList<Dialog> GetAllDialogsWithInterfaceView(Guid applicationId, DialogType? dialogType)
        {
            IList<Dialog> dialogList = null;

            if (dialogType == null)
                dialogList = DialogDao.FindAllDialogsWithInterfaceView(applicationId);
            else
                dialogList = DialogDao.FindAllDialogsWithInterfaceViewByDialogType(applicationId, (DialogType)dialogType);

            foreach (Dialog dialog in dialogList)
            {
                NHibernateUtil.Initialize(dialog.Module);
            }

            return dialogList;
        }

        [Transaction(ReadOnly = true)]
        public IList<CustomDialog> GetAllCustomDialogs(Guid applicationId)
        {
            return CustomDialogDao.FindAll(applicationId);
        }

        [Transaction(ReadOnly = true)]
        public IList<Domain.Module> GetAllModules(Guid applicationId)
        {
            IList<Domain.Module> modules = ModuleDao.FindAll(applicationId);

            foreach (Domain.Module module in modules)
            {
                NHibernateUtil.Initialize(module.Dialogs);
            }

            return modules;
        }


        [Transaction(ReadOnly = false)]
        public void UnmapViewComponents(View view)
        {
            View readView = ViewDao.FindById(view.Id);

            // Remove all maps for the ViewNodes connected to the View
            IList<ViewNode> viewNodeList = ViewNodeDao.FindAllByViewId(readView.Id);

            foreach (ViewNode viewNode in viewNodeList)
            {
                // Delete the ViewMap
                if (viewNode.ViewMap != null)
                {
                    PropertyMapDao.Delete(viewNode.ViewMap);
                }

                // Remove ViewMap from the ViewNode
                viewNode.ViewMap = null;

                // Save ViewNode
                ViewNodeDao.SaveOrUpdate(viewNode);
            }

            // Remove the connections from ViewComponents to MappedProperty
            WalkViewComponentsAndUnmap(readView.VisualTree);
        }

        [Transaction(ReadOnly = true)]
        public IList<ViewNode> GetViewNodesByViewId(Guid viewId)
        {
            return ViewNodeDao.FindAllByViewId(viewId);
        }


        [Transaction(ReadOnly = false)]
        private void WalkViewComponentsAndUnmap(UXComponent component)
        {
            if (component != null)
            {
                if (component is IBindable)
                {
                    IBindable bindable = component as IBindable;

                    bindable.MappedProperty = null;
                }

                UXContainer container = null;

                if (component is UXGroupBox)
                    container = (component as UXGroupBox).Container;
                else
                    container = component as UXContainer;

                if (container != null)
                {
                    foreach (UXComponent child in container.Children)
                        WalkViewComponentsAndUnmap(child);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public void UpdateWorkflows(Cdc.MetaManager.DataAccess.Domain.Module module)
        {
            IList<Workflow> workflows = WorkflowDao.FindWorkflows(module.Application.Id, module.Name, null);

            foreach (Workflow workflow in workflows)
                UpdateWorkflow(workflow);
        }

        [Transaction(ReadOnly = false)]
        public void UpdateWorkflow(Workflow workflow)
        {
            MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(workflow.Id, typeof(Workflow));

            Workflow readWorkflow = WorkflowDao.FindById(workflow.Id);

            XDocument x = XDocument.Load(XmlReader.Create(new StringReader(readWorkflow.WorkflowXoml)));

            var dialogs = from d in x.Descendants()
                          where d.Attribute("DialogId") != null
                          select d;

            foreach (XElement element in dialogs)
            {
                Dialog d = DialogDao.FindById(new Guid(element.Attribute("DialogId").Value));
                element.Name = XName.Get(WorkflowTypeFactory.GetDialogActivityClassName(d), element.Name.NamespaceName);
            }

            var serviceMethods = from s in x.Descendants()
                                 where s.Attribute("ServiceMethodId") != null
                                 select s;

            foreach (XElement element in serviceMethods)
            {
                ServiceMethod s = ServiceMethodDao.FindById(new Guid(element.Attribute("ServiceMethodId").Value));
                element.Name = XName.Get(WorkflowTypeFactory.GetServiceMethodActivityClassName(s), element.Name.NamespaceName);
            }

            readWorkflow.WorkflowXoml = x.ToString();

            WorkflowDao.SaveOrUpdate(readWorkflow);
        }

        private void InitializeView(View view)
        {
            NHibernateUtil.Initialize(view);
            NHibernateUtil.Initialize(view.Application);
            NHibernateUtil.Initialize(view.BusinessEntity);
            NHibernateUtil.Initialize(view.ServiceMethod);

            if (view.RequestMap != null)
            {
                MetaManagerUtil.InitializePropertyMap(view.RequestMap);
            }

            if (view.ResponseMap != null)
            {
                MetaManagerUtil.InitializePropertyMap(view.ResponseMap);
            }

            if ((view.ServiceMethod != null) && (view.ServiceMethod.RequestMap != null))
            {
                MetaManagerUtil.InitializePropertyMap(view.ServiceMethod.RequestMap);
            }

            if ((view.ServiceMethod != null) && (view.ServiceMethod.ResponseMap != null))
            {
                MetaManagerUtil.InitializePropertyMap(view.ServiceMethod.ResponseMap);
            }

            if (view.DataSources.Count > 0)
            {
                foreach (DataSource dataSource in view.DataSources)
                {
                    InitializeDataSource(dataSource);
                }
            }

            ViewHelper.InitializeUXComponent(view.VisualTree);
        }

        private void InitializeDataSource(DataSource dataSource)
        {
            MetaManagerUtil.InitializePropertyMap(dataSource.RequestMap);
            MetaManagerUtil.InitializePropertyMap(dataSource.ResponseMap);
            MetaManagerUtil.InitializePropertyMap(dataSource.ServiceMethod.RequestMap);
            MetaManagerUtil.InitializePropertyMap(dataSource.ServiceMethod.ResponseMap);
        }

        [Transaction(ReadOnly = true)]
        public DataSource GetDataSourceById(Guid dataSourceId)
        {
            DataSource dataSource = DataSourceDao.FindById(dataSourceId);

            InitializeDataSource(dataSource);

            return dataSource;
        }

        [Transaction(ReadOnly = true)]
        public View GetViewById(Guid viewId)
        {
            View view = ViewDao.FindById(viewId);

            InitializeView(view);

            return view;
        }

        [Transaction(ReadOnly = true)]
        public ViewNode GetViewNodeById(Guid viewNodeId)
        {
            ViewNode viewNode = ViewNodeDao.FindById(viewNodeId);

            NHibernateUtil.Initialize(viewNode.Dialog);
            NHibernateUtil.Initialize(viewNode.Parent);
            NHibernateUtil.Initialize(viewNode.ViewMap);
            NHibernateUtil.Initialize(viewNode);

            foreach (ViewAction va in viewNode.ViewActions)
            {
                NHibernateUtil.Initialize(va.Action);
            }

            InitializeView(viewNode.View);

            return viewNode;
        }


        [Transaction(ReadOnly = false)]
        private View SaveView(View view)
        {
            ViewDao.SaveOrUpdate(view);

            return view;
        }

        [Transaction(ReadOnly = false)]
        public ViewNode SaveViewNode(ViewNode viewNode)
        {
            return ViewNodeDao.SaveOrUpdate(viewNode);
        }

        [Transaction(ReadOnly = true)]
        public IList<DbProperty> GetDbProperties(ServiceMethod serviceMethod)
        {
            serviceMethod = ServiceMethodDao.FindById(serviceMethod.Id);

            if (serviceMethod != null)
            {
                if (serviceMethod.MappedToAction.Query != null)
                {
                    NHibernateUtil.Initialize(serviceMethod.MappedToAction.Query.Properties);

                    return serviceMethod.MappedToAction.Query.Properties.Cast<DbProperty>().ToList();
                }
                else if (serviceMethod.MappedToAction.StoredProcedure != null)
                {
                    NHibernateUtil.Initialize(serviceMethod.MappedToAction.StoredProcedure.Properties);

                    return serviceMethod.MappedToAction.StoredProcedure.Properties.Cast<DbProperty>().ToList();
                }
            }

            return null;
        }


        [Transaction(ReadOnly = false)]
        public void SetDialogInterface(Dialog dialog, View view)
        {
            Dialog readDialog = DialogDao.FindById(dialog.Id);

            if (readDialog != null)
            {
                readDialog.InterfaceView = view;

                DialogDao.SaveOrUpdate(readDialog);
            }
        }
                
        [Transaction(ReadOnly = false)]
        public void DeleteUXComponent(UXComponent component)
        {
            if (component != null)
            {
                foreach (PropertyInfo info in component.GetType().GetProperties())
                {
                    if (info.GetCustomAttributes(typeof(DomainReferenceAttribute), false).Count() > 0)
                    {
                        PropertyInfo idProperty = component.GetType().GetProperty(info.Name + "Id");

                        if (idProperty == null)
                            throw new Exception("Id property not found.");

                        Guid id = (Guid)idProperty.GetValue(component, null);

                        if (id != Guid.Empty)
                        {
                            if (info.PropertyType == typeof(PropertyMap))
                            {
                                PropertyMap map = PropertyMapDao.FindById(id);

                                if (map != null)
                                {
                                    PropertyMapDao.Delete(map);
                                }
                            }
                        }
                    }
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public void MoveUp(Guid moverActionId, Guid staticActionId, out ViewAction moverAction, out ViewAction staticAction)
        {
            moverAction = ViewActionDao.FindById(moverActionId);
            staticAction = ViewActionDao.FindById(staticActionId);

            int seq1 = moverAction.Sequence;
            int seq2 = staticAction.Sequence;

            if (seq1 != seq2)
            {
                int temp = seq2;
                seq2 = seq1;
                seq1 = temp;
            }
            else
            {
                seq2++;
            }

            moverAction.Sequence = seq1;
            staticAction.Sequence = seq2;

            ViewActionDao.SaveOrUpdate(moverAction);
            ViewActionDao.SaveOrUpdate(staticAction);
        }

        [Transaction(ReadOnly = false)]
        public void MoveUp(Guid moverViewNodeId, Guid staticViewNodeId, out ViewNode moverViewNode, out ViewNode staticViewNode)
        {
            moverViewNode = ViewNodeDao.FindById(moverViewNodeId);
            staticViewNode = ViewNodeDao.FindById(staticViewNodeId);

            int seq1 = moverViewNode.Sequence;
            int seq2 = staticViewNode.Sequence;

            if (seq1 != seq2)
            {
                int temp = seq2;
                seq2 = seq1;
                seq1 = temp;
            }
            else
            {
                seq2++;
            }

            moverViewNode.Sequence = seq1;
            staticViewNode.Sequence = seq2;

            ViewNodeDao.SaveOrUpdate(moverViewNode);
            ViewNodeDao.SaveOrUpdate(staticViewNode);
        }

        [Transaction(ReadOnly = true)]
        public IList<string> FindAllUniqueCustomDLLNames(Guid applicationId)
        {
            return ViewDao.FindAllUniqueCustomDLLNames(applicationId);
        }

        [Transaction(ReadOnly = false)]
        public ViewNode ConnectViewToViewNode(View view, ViewNode parentViewNode)
        {
            if (view != null && parentViewNode != null)
            {
                // First update/create the view
                View readView = ViewDao.FindById(view.Id);
                parentViewNode = ViewNodeDao.FindById(parentViewNode.Id);

                // Create viewnode to connect the view to
                ViewNode viewNode = new ViewNode();

                viewNode.Dialog = parentViewNode.Dialog;
                viewNode.Parent = parentViewNode;
                viewNode.Sequence = viewNode.Dialog.ViewNodes.Max(node => node.Sequence) + 1;
                viewNode.Title = null;
                viewNode.View = readView;

                // Save the new ViewNode
                viewNode = ViewNodeDao.SaveOrUpdate(viewNode);

                if (NHibernateUtil.IsInitialized(parentViewNode.Dialog) && NHibernateUtil.IsInitialized(parentViewNode.Dialog.ViewNodes))
                {
                    // Connect the ViewNode to the Dialogs ViewNodes
                    parentViewNode.Dialog.ViewNodes.Add(viewNode);
                }

                if (NHibernateUtil.IsInitialized(parentViewNode.Children))
                {
                    // Connect the ViewNode as a child to the Parent ViewNode
                    parentViewNode.Children.Add(viewNode);
                }

                return viewNode;
            }

            return null;
        }

        [Transaction(ReadOnly = false)]
        public void SaveAndDeleteMappedPropertiesInMap(PropertyMap propertyMap, IList<MappedProperty> deletedList)
        {
            foreach (MappedProperty mp in propertyMap.MappedProperties)
            {
                MappedPropertyDao.SaveOrUpdate(mp);
            }

            foreach (MappedProperty mp in deletedList)
            {
                MappedPropertyDao.Delete(mp);
            }
        }

        private UXComponent FindComponentInSearchPanel(UXComponent component, MappedProperty property)
        {
            if (component is IBindable)
                if ((component as IBindable).MappedProperty == property)
                    return component;

            UXContainer container = null;

            if (component is UXGroupBox)
                container = (component as UXGroupBox).Container;
            else
                container = component as UXContainer;

            if (container != null)
            {
                foreach (UXComponent child in container.Children)
                {
                    UXComponent result = FindComponentInSearchPanel(child, property);

                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        [Transaction(ReadOnly = false)]
        public Dialog CreateOrUpdateSearchPanelView(Dialog dialog)
        {
            dialog = this.GetDialogWithViewTree(dialog.Id);

            if (dialog != null)
            {
                View interfaceView = dialog.InterfaceView;

                View view = dialog.SearchPanelView;

                if (view != null)
                {
                    view = this.GetViewById(view.Id);
                    if (!view.IsLocked || view.LockedBy != Environment.UserName)
                    {
                        return dialog;
                    }
                }
                else
                {
                    if (!dialog.IsLocked || dialog.LockedBy != Environment.UserName)
                    {
                        return dialog;
                    }
                    view = new Cdc.MetaManager.DataAccess.Domain.View();
                    view.Application = interfaceView.Application;
                    view.BusinessEntity = interfaceView.BusinessEntity;
                    view.RequestMap = interfaceView.RequestMap;
                    view.ResponseMap = interfaceView.RequestMap;
                    view.Type = ViewType.Standard;
                    view.Name = string.Format("{0}SearchPanel", dialog.Name);
                    view.Title = view.Name;

                    MetaManagerServices.GetModelService().SaveDomainObject(view);

                    MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(view.Id, typeof(View));

                    dialog.SearchPanelView = view;

                    DialogDao.SaveOrUpdate(dialog);
                }

                if (view.VisualTree == null)
                    view.VisualTree = new UXSearchPanel("SearchPanel");

                view.Name = string.Format("{0}SearchPanel", dialog.Name);
                view.Title = view.Name;

                ViewDao.SaveOrUpdate(view);

                foreach (MappedProperty property in interfaceView.RequestMap.MappedProperties)
                {
                    if ((property.IsSearchable) && (FindComponentInSearchPanel(view.VisualTree, property) == null))
                    {
                        UXSearchPanelItem item = new UXSearchPanelItem();

                        item.Caption = property.Name;

                        UXTextBox textBox = new UXTextBox(property.Name);
                        textBox.MappedProperty = property;
                        textBox.Width = -1;
                        textBox.Height = 21;

                        item.Children.Add(textBox);
                        item.IsDefaultVisible = true;

                        view.VisualTree.Children.Add(item);
                    }
                    else if (!property.IsSearchable)
                    {
                        UXComponent component = FindComponentInSearchPanel(view.VisualTree, property);

                        if (component != null)
                            Helpers.ViewHelper.ReplaceComponentInVisualTree(component.Parent, null);
                    }
                }

                SaveView(view);
            }

            return dialog;
        }

        [Transaction(ReadOnly = true)]
        public IList<View> GetViews(string entityName, string viewName, string title, FindViewTypes findViewTypes, Guid applicationId)
        {
            return ViewDao.FindViews(entityName, viewName, title, findViewTypes, applicationId);
        }

        [Transaction(ReadOnly = true)]
        public IList<View> GetViewsByNameAndApplicationId(string viewName, Guid applicationId)
        {
            return ViewDao.FindByNameAndApplicationId(viewName, applicationId);
        }

        [Transaction(ReadOnly = false)]
        public DataSource CreateDataSourceMaps(DataSource dataSource, View view, Guid serviceMethodId)
        {
            ServiceMethod serviceMethod = ServiceMethodDao.FindById(serviceMethodId);

            if (serviceMethod != null)
            {
                MetaManagerUtil.InitializePropertyMap(serviceMethod.RequestMap);
                MetaManagerUtil.InitializePropertyMap(serviceMethod.ResponseMap);

                View readView = ViewDao.FindById(view.Id);

                dataSource.ServiceMethod = serviceMethod;
                dataSource.RequestMap = new PropertyMap();
                dataSource.RequestMap.IsCollection = serviceMethod.RequestMap.IsCollection;
                dataSource.ResponseMap = new PropertyMap();
                dataSource.ResponseMap.IsCollection = serviceMethod.ResponseMap.IsCollection;

                foreach (MappedProperty sourceProperty in dataSource.ServiceMethod.RequestMap.MappedProperties)
                {
                    MappedProperty mappedProperty = new MappedProperty();

                    mappedProperty.Source = sourceProperty;
                    mappedProperty.Target = null;
                    mappedProperty.Name = sourceProperty.Name;
                    mappedProperty.Sequence = sourceProperty.Sequence;
                    mappedProperty.PropertyMap = dataSource.RequestMap;
                    dataSource.RequestMap.MappedProperties.Add(mappedProperty);
                }

                foreach (MappedProperty sourceProperty in readView.ResponseMap.MappedProperties)
                {
                    MappedProperty mappedProperty = new MappedProperty();

                    mappedProperty.Source = sourceProperty;
                    mappedProperty.Target = null;
                    mappedProperty.IsEnabled = false;
                    mappedProperty.Name = sourceProperty.Name;
                    mappedProperty.Sequence = sourceProperty.Sequence;
                    mappedProperty.PropertyMap = dataSource.ResponseMap;
                    dataSource.ResponseMap.MappedProperties.Add(mappedProperty);
                }
            }

            return dataSource;
        }

        [Transaction(ReadOnly = true)]
        public void GetDataSourceToViewRequestMap(DataSource dataSource, View view, out PropertyMap dataSourceToViewRequestMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            DataSource readDataSource = DataSourceDao.FindById(dataSource.Id);
            View readView = ViewDao.FindById(view.Id);

            if (readDataSource != null &&
                readDataSource.ServiceMethod != null &&
                readDataSource.ServiceMethod.RequestMap != null)
            {
                sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readDataSource.ServiceMethod.RequestMap).MappedProperties.Cast<IMappableProperty>());
            }
            else
            {
                sourceProperties = new List<IMappableProperty>();
            }

            if (readView.ResponseMap != null)
            {
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.ResponseMap).MappedProperties.Cast<IMappableProperty>());
            }
            else
            {
                targetProperties = new List<IMappableProperty>();
            }

            IList<UXSessionProperty> sessionProperties = ApplicationService.GetUXSessionProperties(readView.Application);

            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            if (readDataSource != null)
            {
                dataSourceToViewRequestMap = readDataSource.RequestMap;
                MetaManagerUtil.InitializePropertyMap(dataSourceToViewRequestMap);
            }
            else
                dataSourceToViewRequestMap = null;
        }

        [Transaction(ReadOnly = true)]
        public void GetDataSourceToViewResponseMap(DataSource dataSource, View view, out PropertyMap dataSourceToViewResponseMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            DataSource readDataSource = DataSourceDao.FindById(dataSource.Id);
            View readView = ViewDao.FindById(view.Id);

            if (readView.ResponseMap != null)
            {
                sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readView.ResponseMap).MappedProperties.Cast<IMappableProperty>());
            }
            else
            {
                sourceProperties = new List<IMappableProperty>();
            }

            if (readDataSource != null &&
                readDataSource.ServiceMethod.ResponseMap != null)
            {
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readDataSource.ServiceMethod.ResponseMap).MappedProperties.Cast<IMappableProperty>());
            }
            else
            {
                targetProperties = new List<IMappableProperty>();
            }

            IList<UXSessionProperty> sessionProperties = ApplicationService.GetUXSessionProperties(readView.Application);

            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            if (readDataSource != null)
            {
                dataSourceToViewResponseMap = readDataSource.ResponseMap;
                MetaManagerUtil.InitializePropertyMap(dataSourceToViewResponseMap);
            }
            else
                dataSourceToViewResponseMap = null;
        }

        [Transaction(ReadOnly = false)]
        public DataSource SaveOrUpdateDataSourceMaps(DataSource dataSource, PropertyMap requestMap, PropertyMap responseMap)
        {
            DataSource readDataSource = DataSourceDao.FindById(dataSource.Id);

            if (readDataSource != null)
            {
                if (requestMap != null)
                {
                    readDataSource.RequestMap = PropertyMapDao.SaveOrUpdateMerge(requestMap);
                }

                if (responseMap != null)
                {
                    readDataSource.ResponseMap = PropertyMapDao.SaveOrUpdateMerge(responseMap);
                }

                return DataSourceDao.SaveOrUpdate(readDataSource);
            }
            else
                return null;
        }


        [Transaction(ReadOnly = true)]
        public long CountViewNodes(View view)
        {
            long noOfViews = ViewNodeDao.CountByViewId(view.Id);



            return noOfViews;
        }


    }

}
