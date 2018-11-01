using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.UX.Infrastructure;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using dvSlave;
using System.Windows.Threading;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    [SmartPart]
    public partial class DialogView  : UserControl, IDialogView, IBuilderAware, IActionProvider, ISmartPartInfoProvider
	{
        private bool OKToCloseFlag = false;
        private bool isWorkbenchParent = false;
        private IList<IDialogView> childPrograms;
        private IDialogView parentProgram;
        public string ProgramName { get; set; }
        public string ProgramDescription { get; set; }
        public int Id { get; set; }
        public string StartParameters { get; set; }

        public DialogView()
		{
			this.InitializeComponent();
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(IsVisibleChangedEventHandler);
            
            dialogWorkspace.SmartPartClosed += new EventHandler<WorkspaceEventArgs>(DialogWorkspaceSmartPartClosed);
            dialogWorkspace.SmartPartActivated += new EventHandler<WorkspaceEventArgs>(DialogWorkspaceSmartPartActivated);
            this.Loaded += new RoutedEventHandler(DialogViewLoaded);
            this.Unloaded += new RoutedEventHandler(DialogViewUnLoaded);
            childPrograms = new List<IDialogView>();
		}

        public void OKToClose()
        {
            OKToCloseFlag = true;
        }

        public bool IsOKToClose()
        {
            return OKToCloseFlag;
        }

        private void DialogWorkspaceSmartPartActivated(object sender, WorkspaceEventArgs e)
        {
            ISmartPartInfo info = dialogWorkspace.ActiveSmartPartInfo;

            // TODO fixme
            if (info == null)
            {
                info = new SmartPartInfo() { Title = "??", Description = "" };
            }

            if (!dialogList.Contains(e.SmartPart))
            {
                breadCrumb.AddDirectory(info.Title, e.SmartPart);
                dialogList.Add(e.SmartPart);
            }
        }

        void DialogViewLoaded(object sender, RoutedEventArgs e)
        {
        }

        void DialogViewUnLoaded(object sender, RoutedEventArgs e)
        {
        }

        public object TrimControl
        {
            get
            {
                if (dialogWorkspace.SmartParts.Count > 0)
                    return dialogWorkspace.SmartParts[dialogWorkspace.SmartParts.Count - 1];
                else
                    return 0;
            }
        }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                if (presenter != null)
                {
                    presenter.OnViewShow();
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

        private List<object> dialogList = new List<object>();

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
        }

        public void Show(object smartPart)
        {
            dialogWorkspace.Show(smartPart);
        }

        public void Show(object smartPart, ISmartPartInfo smartPartInfo)
        {
            dialogWorkspace.Show(smartPart, smartPartInfo);
        }

        private void CloseSmartPartsAfter(object view)
        {
            int index = dialogList.IndexOf(view);
            
            if (index < (dialogList.Count - 1))
            {
                object viewToClose = dialogList[index + 1];
                dialogWorkspace.Close(viewToClose);
            }
        }

        private void DialogWorkspaceSmartPartClosed(object sender, WorkspaceEventArgs e)
        {
            dialogList.Remove(e.SmartPart);
            breadCrumb.CloseToDirectory(dialogList[dialogList.Count - 1]);
        }
        
        private void BreadCrumbPreviewDirectoryCloseTo(object sender, BreadCrumbDirectoryEventArgs e)
        {
            CloseSmartPartsAfter(e.Directory);
            e.Handled = true;
        }

        public IList<IDialogView> GetChildPrograms()
        {
            return childPrograms;
        }

        public void AddChildProgram(IDialogView childProgram)
        {
            childPrograms.Add(childProgram);
        }
        public void RemoveChildProgram(IDialogView childProgram)
        {
            childPrograms.Remove(childProgram);
        }

        public IDialogView ParentProgram
        {
            get
            {
                return parentProgram;
            }
            set
            {
                parentProgram = value;
            }
        }

        public bool IsWorkbenchParent
        {
            get
            {
                return isWorkbenchParent;
            }
            set
            {
                isWorkbenchParent = value;
            }
        }

        public string toString()
        {
            return ProgramName;
        }

        #region IActionProvider Members

        public ICollection<ShellAction> GetActions()
        {
            return presenter.GetActions();
        }

        #endregion

        public void ShowActions(string actionString)
        {
            presenter.ShowActions(actionString);
        }

        public void SaveSeach(string searchStr)
        {
            presenter.SaveSearch(searchStr);
        }

        public void SetFocus()
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate()
                {
                    this.Focus();
                }));
        }

        public void SaveInDashboard(string searchStr)
        {
            presenter.SaveInDashboard(searchStr);
        }


        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            ShellHyperlink hyperlink = new ShellHyperlink();
            hyperlink.ModuleId = ModuleController.ModuleId;
            hyperlink.Data.Add("ProgramType", "trim");
            hyperlink.Data.Add("DialogId", ProgramName);
            hyperlink.Data.Add("DialogDescription", ProgramDescription); 
            hyperlink.Data.Add("Parameters", StartParameters);
            return new ShellSmartPartInfo("Title", "Description", hyperlink);
        }

        #endregion
    }
}
