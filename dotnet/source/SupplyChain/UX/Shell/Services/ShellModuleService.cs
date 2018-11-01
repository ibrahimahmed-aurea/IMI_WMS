using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Infrastructure;
using System.Collections.ObjectModel;
using Imi.SupplyChain.UX.Shell.Views;
using System.Xml;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.Threading;
using Imi.SupplyChain.Services.Authorization.DataContracts;
using System.Windows;
using System.Configuration;
using Imi.SupplyChain.UX.Shell.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;

namespace Imi.SupplyChain.UX.Shell.Services
{
    public class ShellModuleService : IShellModuleService
    {
        private class AuthOperation : IAuthOperation
        {
            public AuthOperation()
            {
            }

            public ShellDrillDownMenuItem MenuItem { get; set; }

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
        
        private ShellConfigurationSection _config;
        private IDictionary<IShellModule, IShellPresentationModel> _modelDictionary;
        private IFavoritesService _favoritesService;
        private IShellModule _activeModule;
        private IDictionary<IShellModule, WorkItem> _workItemDictionary;
        private IAuthorizationService _authorizationService;
        private IUserSessionService _userSessionService;
                        
        [EventPublication(EventTopicNames.ModuleActivated, PublicationScope.Global)]
        public event EventHandler<DataEventArgs<IShellModule>> ModuleActivated;

        [EventPublication(EventTopicNames.ModuleActivating, PublicationScope.Global)]
        public event EventHandler<DataEventArgs<IShellModule>> ModuleActivating;

        [EventPublication(EventTopicNames.ModuleLoaded, PublicationScope.Global)]
        public event EventHandler<DataEventArgs<IShellModule>> ModuleLoaded;

        public ShellModuleService([ServiceDependency] IFavoritesService favoritesService, [ServiceDependency] IAuthorizationService authorizationService, [ServiceDependency] IUserSessionService userSessionService)
        {
            _modelDictionary = new Dictionary<IShellModule, IShellPresentationModel>();
            _workItemDictionary = new Dictionary<IShellModule, WorkItem>();
            _config = ConfigurationManager.GetSection(ShellConfigurationSection.SectionKey) as ShellConfigurationSection;
            _favoritesService = favoritesService;
            _authorizationService = authorizationService;
            _userSessionService = userSessionService;
        }

        public IShellModule ActiveModule
        {
            get
            {
                return _activeModule;
            }
            set
            {
                ActivateModule(value);
            }
        }

        private void ActivateModule(IShellModule module)
        {
            if (_activeModule != module)
            {
                _activeModule = module;
                
                EventHandler<DataEventArgs<IShellModule>> temp = ModuleActivating;

                if (temp != null)
                    temp(this, new DataEventArgs<IShellModule>(_activeModule));
                
                IShellPresentationModel model = GetPresentationModel(_activeModule);

                if (!model.IsInitialized)
                {
                    InitializeModel(model);
                }
                else
                {
                    OnModuleActivated();
                }
            }
        }

        private void OnModuleActivated()
        {
            ShellInteractionService interactionService = null;
                        
            var modules = from m in Modules
                          where m != _activeModule
                          select m;

            foreach (IShellModule module in modules)
            {
                interactionService = GetWorkItem(module).Services.Get<IShellInteractionService>() as ShellInteractionService;
                interactionService.OnModuleDeactivated(new EventArgs());
            }

            interactionService = GetWorkItem(_activeModule).Services.Get<IShellInteractionService>() as ShellInteractionService;
            interactionService.OnModuleActivated(new EventArgs());

            EventHandler<DataEventArgs<IShellModule>>  temp = ModuleActivated;

            if (temp != null)
                temp(this, new DataEventArgs<IShellModule>(_activeModule));
        }

        private void InitializeModel(IShellPresentationModel model)
        {
            model.IsInitialized = true;
            model.StartMenuTopItem = new ShellDrillDownMenuItem() { Caption = StringResources.LoadingLabel_Content, IsEnabled = false };
            
            ThreadPool.QueueUserWorkItem(InitializeModelThread, model);
        }

        public void RegisterModule(IShellModule module, WorkItem workItem)
        {
            IShellPresentationModel model = new ShellPresentationModel();
            model.Module = module;
            model.InstanceName = _userSessionService.InstanceName;
            
            _modelDictionary.Add(module, model);
            _workItemDictionary.Add(module, workItem);

            EventHandler<DataEventArgs<IShellModule>> temp = ModuleLoaded;

            if (temp != null)
                temp(this, new DataEventArgs<IShellModule>(module));
        }

        public ReadOnlyCollection<IShellModule> Modules
        {
            get
            {
                var moduleList = new List<IShellModule>();

                foreach (IShellPresentationModel model in _modelDictionary.Values.OrderBy(m => m.Module.OrderIndex))
                    moduleList.Add(model.Module);

                return new ReadOnlyCollection<IShellModule>(moduleList);
            }
        }

        public IShellPresentationModel GetPresentationModel(IShellModule module)
        {
            return _modelDictionary[module];
        }

        public ReadOnlyCollection<IShellPresentationModel> PresentationModels
        {
            get
            {
                return new ReadOnlyCollection<IShellPresentationModel>(_modelDictionary.Values.ToList());
            }
        }

        private void InitializeModelThread(object state)
        {
            IShellPresentationModel model = state as IShellPresentationModel;
            
            try
            {
                ShellDrillDownMenuItem favoritesMenu = null;
                ShellDrillDownMenuItem startMenu = null;
                List<AuthOperation> operations = new List<AuthOperation>();

                XmlDocument favoritesDoc = _favoritesService.GetFavorites(model.Module.Id);

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    startMenu = XmlToShellDrillDownMenuItemTransformer.Transform(model.Module.GetMenu());

                    if (favoritesDoc != null)
                    {
                        favoritesMenu = XmlToShellDrillDownMenuItemTransformer.Transform(favoritesDoc);
                    }
                    else
                    {
                        favoritesMenu = new ShellDrillDownMenuItem()
                        {
                            Caption = StringResources.FavoritesMenu_Header,
                            IsFolder = true
                        };
                    }

                    AddOperations(startMenu, operations);
                    AddOperations(favoritesMenu, operations);

                }));

                bool isAuthorizationEnabled = true;

                if (operations.Count > 0)
                {
                    isAuthorizationEnabled = _authorizationService.CheckAuthorization(model.Module.Id, operations);
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (AuthOperation operation in operations)
                    {
                        operation.MenuItem.IsAuthorized = operation.IsAuthorized;
                    }

                    model.FavoritesMenuTopItem = favoritesMenu;

                    if (model.FavoritesMenuTopItem != null)
                    {
                        model.FavoritesMenuTopItem.TreeChanged += (s, e) =>
                        {
                            _favoritesService.QueueForUpdate(model.Module.Id, model.FavoritesMenuTopItem);
                        };
                    }

                    if (_config.HideUnauthorizedMenuItems)
                    {
                        HideUnauthorizedMenuItems(startMenu);
                    }

                    model.StartMenuTopItem = startMenu;

                    OnModuleActivated();

                    if (!isAuthorizationEnabled)
                    {
                        ShellInteractionService interactionService = GetWorkItem(model.Module).Services.Get<IShellInteractionService>() as ShellInteractionService;
                        interactionService.ShowNotification(new ShellNotification(StringResources.Authorization_Notification, null));
                    }
                }));
            }
            catch (Exception ex)
            { 
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    model.StartMenuTopItem = new ShellDrillDownMenuItem() { Caption = "", IsEnabled = false };
                    throw ex;
                }));
            }
        }

        private bool HideUnauthorizedMenuItems(ShellDrillDownMenuItem menuItem)
        {
            bool hasChildren = false;

            if (menuItem != null)
            {
                foreach (ShellDrillDownMenuItem childItem in menuItem.Children.ToArray())
                {
                    if (childItem.IsAuthorized && !childItem.IsFolder && !childItem.IsBackItem)
                        hasChildren = true;

                    hasChildren = hasChildren | HideUnauthorizedMenuItems(childItem);
                }

                if (menuItem.Parent != null)
                {
                    if ((menuItem.IsFolder && (!hasChildren)) || (!menuItem.IsAuthorized && !menuItem.IsBackItem))
                        menuItem.Parent.Children.Remove(menuItem);
                }
            }

            return hasChildren;
        }
        
        private void AddOperations(ShellDrillDownMenuItem menuItem, List<AuthOperation> operations)
        {
            if (menuItem != null)
            {
                if (!string.IsNullOrEmpty(menuItem.Operation))
                {
                    operations.Add(new AuthOperation() { MenuItem = menuItem, Operation = menuItem.Operation } );
                }

                foreach (ShellDrillDownMenuItem child in menuItem.Children)
                {
                    AddOperations(child, operations);
                }
            }
        }
               
        public WorkItem GetWorkItem(IShellModule module)
        {
            return _workItemDictionary[module];
        }
    }
}
