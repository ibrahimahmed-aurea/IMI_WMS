using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Microsoft.Practices.CompositeUI.SmartParts;
using System.Windows;

namespace Imi.SupplyChain.UX
{
    public class DynamicViewLoader : ContentPresenter, IDataView, ISmartPartInfoProvider, IDetailViewToggled
    {
        public event EventHandler<DataEventArgs<object>> ViewLoaded;
        private object _data;
        private object _parameters;
        private IDataView _view;
        private bool? _refreshDataOnShow;
        private bool? _isDetailView;
        private bool _focus;
        private string _title;
        private bool _update;

        public DynamicViewLoader()
            : this(null)
        { 
        }

        public DynamicViewLoader(Type viewType)
        {
            ViewType = viewType;
                        
            IsVisibleChanged += (s, e) =>
                {
                    if ((bool)e.NewValue && Content == null)
                    {
                        Load();
                    }
                };
        }

        public string Title
        {
            get
            {
                if (_view != null)
                    return _view.Title;
                else
                    return _title;
            }
            set
            {
                if (_view != null)
                    _view.Title = value;
                else
                    _title = value;
            }
        }

        [ServiceDependency]
        public WorkItem WorkItem { get; set; }

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService { get; set; }

        public Type ViewType { get; set; }

        private void Load()
        {
            ShellInteractionService.ShowProgress();
            
            try
            {
                _view = WorkItem.SmartParts.AddNew(ViewType) as IDataView;

                EventHandler<DataEventArgs<object>> temp = ViewLoaded;

                if (temp != null)
                    temp(this, new DataEventArgs<object>(_view));
                
                if (_data != null)
                    _view.PresentData(_data);

                if (_isDetailView != null)
                    _view.IsDetailView = _isDetailView.Value;

                if (_update)
                    _view.Update(_parameters);

                if (_focus)
                    _view.SetFocus();

                if (!string.IsNullOrEmpty(_title))
                    _view.Title = _title;

                if (_refreshDataOnShow != null)
                    _view.RefreshDataOnShow = _refreshDataOnShow.Value;

                Content = _view;
            }
            finally
            {
                ShellInteractionService.HideProgress();
            }
        }

        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            if (_view is ISmartPartInfoProvider)
            {
                return ((ISmartPartInfoProvider)_view).GetSmartPartInfo(smartPartInfoType);
            }
            else
            {
                return new SmartPartInfo(Title, null);
            }
        }

        #endregion

        #region IDataView Members

        public void PresentData(object data)
        {
            if (_view == null)
            {
                this._data = data;
                this._parameters = null;
                _update = false;
            }
            else
                _view.PresentData(data);
                
        }

        public void Update(object parameters)
        {
            if (_view == null)
            {
                this._parameters = parameters;
                this._data = null;
                _update = true;
            }
            else
                _view.Update(parameters);
        }

        public new bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                if (value)
                {
                    base.Visibility = System.Windows.Visibility.Visible;
                    
                    if (Parent is TabItem)
                    {
                        ((TabItem)Parent).Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    base.Visibility = System.Windows.Visibility.Collapsed;

                    if (Parent is TabItem)
                    {
                        ((TabItem)Parent).Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public void UpdateProgress(int progressPercentage)
        {
            if (_view != null)
                _view.UpdateProgress(progressPercentage);
        }

        public bool RefreshDataOnShow
        {
            get
            {
                if (_view != null)
                    return _view.RefreshDataOnShow;
                else
                    return _refreshDataOnShow.Value;
            }
            set
            {
                if (_view != null)
                    _view.RefreshDataOnShow = value;
                else
                    _refreshDataOnShow = value;
            }
        }

        public bool IsDetailView
        {
            get
            {
                if (_view != null)
                    return _view.IsDetailView;
                else
                    return _isDetailView.Value;
            }
            set
            {
                if (_view != null)
                    _view.IsDetailView = value;
                else
                    _isDetailView = value;
            }
        }

        public void SetFocus()
        {
            if (_view != null)
                _view.SetFocus();
            else
                _focus = true;
        }

        #endregion

        public void BringCurrentItemIntoView()
        {
            IDetailViewToggled detailView = _view as IDetailViewToggled;

            if (detailView != null)
                detailView.BringCurrentItemIntoView();
        }
    }
}
