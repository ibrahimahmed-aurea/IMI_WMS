using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Diagnostics;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX.Wpf;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Views
{
    [SmartPart]
    public partial class DialogView  : UserControl, IDialogView, IBuilderAware, ISmartPartInfoProvider
	{
        private List<object> dialogList = new List<object>();

        public DialogView()
		{
			this.InitializeComponent();
            this.IsVisibleChanged += IsVisibleChangedEventHandler;
            dialogWorkspace.SmartPartClosed += DialogWorkspaceSmartPartClosedEventHandler;
            dialogWorkspace.SmartPartActivated += DialogWorkspaceSmartPartActivatedEventHandler;
            modalWorkspace.SmartPartActivated += ModalWorkspaceSmartPartActivatedEventHandler;
            modalWorkspace.SmartPartClosed += ModalWorkspaceSmartPartClosedEventHandler;
		}

        private void ModalWorkspaceSmartPartClosedEventHandler(object sender, WorkspaceEventArgs e)
        {
            if (modalWorkspace.SmartParts.Count == 0)
                modalGrid.Visibility = Visibility.Hidden;
        }

        private void ModalWorkspaceSmartPartActivatedEventHandler(object sender, WorkspaceEventArgs e)
        {
            modalGrid.Visibility = Visibility.Visible;
        }

        private void DialogWorkspaceSmartPartActivatedEventHandler(object sender, WorkspaceEventArgs e)
        {
            ISmartPartInfo info = GetDialogSmartPartInfo();

            if (!dialogList.Contains(e.SmartPart))
            {
                dialogList.Add(e.SmartPart);
                breadCrumb.AddDirectory(info.Title, e.SmartPart);

                if (dialogList.Count > 1)
                {
                    BreadCrumbItem item = breadCrumb.Items[breadCrumb.Items.Count - 2] as BreadCrumbItem;
                    FrameworkElement element = dialogList[dialogList.Count - 2] as FrameworkElement;
                                        
                    if (Presenter.GetDialogType(dialogList[dialogList.Count - 1]) != DialogType.Create)
                        AppendHeaderInfo(item, element);
                }

                if (typeof(IImportEnabledView).IsAssignableFrom(e.SmartPart.GetType()))
                {
                    IImportEnabledView view = (IImportEnabledView)e.SmartPart;

                    view.ShowImportView += new EventHandler<ShowImportViewArgs>(view_ShowImportView);
                }
            }
            else
            {
                BreadCrumbItem item = breadCrumb.Items[breadCrumb.Items.Count - 1] as BreadCrumbItem;
                item.Header = info.Title;
            }
        }

        void view_ShowImportView(object sender, ShowImportViewArgs e)
        {
            if (e.ShowOnly)
            {
                DialogRow.Height = new GridLength(0);
                ImportRow.Height = new GridLength(100, GridUnitType.Star);
            }

            e.ImportView.PresentData(null);

            importWorkspace.Show(e.ImportView);
        }

        private ISmartPartInfo GetDialogSmartPartInfo()
        {
            ISmartPartInfoProvider provider = dialogWorkspace.ActiveSmartPart as ISmartPartInfoProvider;
            ISmartPartInfo info = null;

            if (provider != null)
                info = provider.GetSmartPartInfo(typeof(SmartPartInfo));
            else
                info = new SmartPartInfo() { Title = "??", Description = "" };
            return info;
        }

        private void AppendHeaderInfo(BreadCrumbItem item, FrameworkElement element)
        {
            MasterDetailView masterDetailView = element as MasterDetailView;
                        
            if ((masterDetailView != null) && (Presenter.GetDialogType(element) != DialogType.Create))
            {
                IList<string> currentItemInfo = masterDetailView.GetCurrentItemInfo();

                int i = 0;

                if (currentItemInfo.Count > 0)
                    item.Header += " ";
                
                foreach (string itemInfo in currentItemInfo)
                {
                    if (i > 0)
                        item.Header += ", ";
                    
                    i++;

                    string shortInfo = itemInfo;

                    if (shortInfo.Length > 15)
                        shortInfo = "..." + itemInfo.Substring(itemInfo.Length - 15, 15);
                    else
                        shortInfo = itemInfo;

                    item.Header += shortInfo;
                }
            }
        }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (presenter != null)
                {
                    presenter.OnViewShow();
                }

                if (mainGrid.Parent == null)
                {
                    Content = mainGrid;
                }
            }
            else
            {
                if (Content != null)
                {
                    Content = null;
                }
            }
        }
         
        private DialogPresenter presenter;

        [CreateNew]
        public DialogPresenter Presenter
        {
            get { return presenter; }
            set 
            { 
                presenter = value;
                presenter.View = this;
            }
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {
        }

        #endregion
                
        private void BreadCrumbDirectoryChanged(object sender, BreadCrumbDirectoryEventArgs e)
        {
            if (e.Directory != null)
            {
                object view = e.Directory;
                                                
                if (!dialogList.Contains(view))
                {
                    // New directory
                    dialogWorkspace.Show(view);
                }
            }

            if (dialogList.Count > 1)
            {
                breadCrumb.Visibility = Visibility.Visible;
            }
            else
            {
                breadCrumb.Visibility = Visibility.Collapsed;
            }
        }

        private void CloseSmartPartsAfter(object view)
        {
            int index = dialogList.IndexOf(view);

            for (int i = dialogList.Count -1; i > index ; i--)
            {
                object viewToClose = dialogList[i];
                dialogWorkspace.Close(viewToClose);

                //Close was cancelled
                if (dialogWorkspace.SmartParts.Contains(viewToClose))
                    break;
            }
        }

        private void DialogWorkspaceSmartPartClosedEventHandler(object sender, WorkspaceEventArgs e)
        {
            if (importWorkspace.ActiveSmartPart != null)
            {
                ImportRow.Height = new GridLength(0, GridUnitType.Auto);
                DialogRow.Height = new GridLength(100, GridUnitType.Star);
                importWorkspace.Close(importWorkspace.ActiveSmartPart);
            }

            if (dialogList.Remove(e.SmartPart))
            {
                if (dialogList.Count > 0)
                {
                    breadCrumb.CloseToDirectory(dialogList[dialogList.Count - 1]);
                }
            }
        }
        
        private void BreadCrumbPreviewDirectoryCloseTo(object sender, BreadCrumbDirectoryEventArgs e)
        {
            CloseSmartPartsAfter(e.Directory);
            e.Handled = true;
        }

        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            if (dialogList.Count > 0)
            {
                ISmartPartInfoProvider provider = dialogList[0] as ISmartPartInfoProvider;

                if (provider != null)
                {
                    return provider.GetSmartPartInfo(smartPartInfoType);
                }
            }

            return null;
        }

        #endregion
    }
}
