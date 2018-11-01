using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using ActiproSoftware.Windows.Controls.Docking.Serialization;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Controls = Imi.Framework.Wpf.Controls;
using Imi.Framework.UX.Wpf;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Shell.Settings;
using Imi.SupplyChain.UX.Infrastructure;
using ActiproSoftware.Windows.Controls.Docking;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public partial class DashboardView : UserControl, IDashboardView, IBuilderAware
    {
        private DashboardPresenter _presenter;
        private bool _settingsSaved = false;
        
        public DashboardView()
        {
            this.InitializeComponent();

            _settingsSaved = false;

            dashboardWorkspace.SmartPartActivated += (s, e) =>
            {
                if (SmartPartActivated != null)
                {
                    SmartPartActivated(s, e);
                }
            };

            dashboardWorkspace.SmartPartClosing += (s, e) =>
            {
                if (SmartPartClosing != null)
                {
                    SmartPartClosing(s, e);
                }
            };
        }
        
        [CreateNew]
        public DashboardPresenter Presenter
        {
            get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }
     
        public void OnBuiltUp(string id)
        {
            _presenter.OnViewReady();
        }

        public void OnTearingDown()
        {
        }

        public void LoadLayout()
        {
            try
            {
                dashboardWorkspace.BeginLoadLayout();

                if (!string.IsNullOrEmpty(Presenter.Settings.Layout))
                {
                    DockSiteLayoutSerializer serializer = new DockSiteLayoutSerializer();
                    serializer.SerializationBehavior = DockSiteSerializationBehavior.All;
                    serializer.LoadFromString(Presenter.Settings.Layout, dashboardWorkspace);
                }
            }
            finally
            {
                dashboardWorkspace.EndLoadLayout();
            }
        }

        public void LoadUserSettings(DashboardSettingsRepository settings)
        {
            Presenter.Settings = settings;
        }
        
        public void SaveUserSettings(DashboardSettingsRepository settings)
        {
            if (!_settingsSaved)
            {
                if (_presenter.Settings == null)
                {
                    settings.ActivationLinks.Clear();

                    int count = 0;

                    foreach (DocumentWindow document in dashboardWorkspace.DocumentWindows)
                    {
                        document.Name = string.Format("Window{0}", count);
                        count++;

                        ISmartPartInfoProvider provider = document.Content as ISmartPartInfoProvider;

                        if (provider != null)
                        {
                            ISmartPartInfo info = provider.GetSmartPartInfo(typeof(ShellSmartPartInfo));

                            ShellSmartPartInfo shellSmartPartInfo = info as ShellSmartPartInfo;

                            if (shellSmartPartInfo != null && shellSmartPartInfo.Hyperlink != null)
                            {
                                settings.ActivationLinks.Add(shellSmartPartInfo.Hyperlink);
                            }
                        }
                    }

                    DockSiteLayoutSerializer serializer = new DockSiteLayoutSerializer();
                    serializer.SerializationBehavior = DockSiteSerializationBehavior.All;
                    settings.Layout = serializer.SaveToString(dashboardWorkspace);
                }
                _settingsSaved = true;
            }
        }
                
        #region IWorkspace Members

        public void Activate(object smartPart)
        {
            dashboardWorkspace.Activate(smartPart);
        }

        public object ActiveSmartPart
        {
            get { return dashboardWorkspace.ActiveSmartPart; }
        }

        public void ApplySmartPartInfo(object smartPart, ISmartPartInfo smartPartInfo)
        {
            dashboardWorkspace.ApplySmartPartInfo(smartPart, smartPartInfo);
        }

        public void Close(object smartPart)
        {
            dashboardWorkspace.Close(smartPart);
        }

        public void Hide(object smartPart)
        {
            dashboardWorkspace.Hide(smartPart);
        }

        public void Show(object smartPart)
        {
            dashboardWorkspace.Show(smartPart);
            dashboardWorkspace.DocumentWindows[dashboardWorkspace.DocumentWindows.Count - 1].MoveToNewHorizontalContainer();
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {
            dashboardWorkspace.Show(smartPart, smartPartInfo);
            dashboardWorkspace.DocumentWindows[dashboardWorkspace.DocumentWindows.Count - 1].MoveToNewHorizontalContainer();
        }

        public void Arrange()
        {
            dashboardWorkspace.TileDocumentsHorizontally();
        }

        public event EventHandler<WorkspaceEventArgs> SmartPartActivated;

        public event EventHandler<WorkspaceCancelEventArgs> SmartPartClosing;

        public ReadOnlyCollection<object> SmartParts
        {
            get { return dashboardWorkspace.SmartParts; }
        }

        #endregion
                
        public void Refresh()
        {
            Presenter.Refresh();
        }
    }
}
