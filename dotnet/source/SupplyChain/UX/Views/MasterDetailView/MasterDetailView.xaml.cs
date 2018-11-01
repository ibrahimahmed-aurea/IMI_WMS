using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX.Wpf;
using System.Diagnostics;
using Controls = Imi.Framework.Wpf.Controls;
using System.Windows.Threading;
using Xceed.Wpf.DataGrid;
using System.Reflection;
using Imi.Framework.UX;
using Xceed.Wpf.DataGrid.Views;

namespace Imi.SupplyChain.UX.Views
{
    public delegate void BringItemIntoViewDelegate();

    [SmartPart]
    public partial class MasterDetailView : UserControl, IMasterDetailView, IBuilderAware, ISmartPartInfoProvider
    {
        private GridLength prevGridLength;
        private GridLengthAnimation animation;
        private Controls.DataGrid grid;
                        
        public bool IsDetailVisible
        {
            get { return (bool)GetValue(IsDetailVisibleProperty); }
            set { SetValue(IsDetailVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDetailVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDetailVisibleProperty =
            DependencyProperty.Register("IsDetailVisible", typeof(bool), typeof(MasterDetailView), new UIPropertyMetadata(false));
        
        public bool HasDetailView
        {
            get { return (bool)GetValue(HasDetailViewProperty); }
            set { SetValue(HasDetailViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasDetailView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasDetailViewProperty =
            DependencyProperty.Register("HasDetailView", typeof(bool), typeof(MasterDetailView), new UIPropertyMetadata(false));
        
        public MasterDetailView()
        {
            this.InitializeComponent();
            this.IsVisibleChanged += IsVisibleChangedEventHandler;
            this.SizeChanged += SizeChangedEventHandler;
            
            detailGridSplitter.DragStarted += DetailGridSplitterDragStartedEventHandler;
            detailGridSplitter.DragCompleted += DetailGridSplitterDragCompletedEventHandler;

            prevGridLength = new GridLength(0, GridUnitType.Pixel);

            animation = new GridLengthAnimation();
            animation.Completed += AnimationCompletedEventHandler;

            masterWorkspace.SmartPartClosed += (s, e) =>
            {
                Presenter.Close();
            };
        }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (presenter != null)
                {
                    presenter.OnViewShow();
                }
            }
        }

        private void SizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            SetMaxHeight();
        }

        private void DetailGridSplitterDragStartedEventHandler(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            SetMaxHeight();
        }

        private void DetailGridSplitterDragCompletedEventHandler(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            OnDetailRowHeightUpdated();
        }

        private void SetMaxHeight()
        {
            detailRow.MaxHeight = Math.Max(ActualHeight - (searchPanelRow.ActualHeight + 100), 0);
        }
                
        private void AnimationCompletedEventHandler(object sender, EventArgs e)
        {
            IsDetailVisible = (detailRow.Height.Value > 0);

            if (IsDetailVisible)
                detailWorkspace.Visibility = Visibility.Visible;
            else
                detailWorkspace.Visibility = Visibility.Hidden;

            if (masterWorkspace.ActiveSmartPart is IDetailViewToggled)
            {
                (masterWorkspace.ActiveSmartPart as IDetailViewToggled).BringCurrentItemIntoView();
            }
        }
        
        private void OnDetailRowHeightUpdated()
        {
            prevGridLength = detailRow.Height;
                        
            IsDetailVisible = (detailRow.Height.Value > 0);

            if (IsDetailVisible)
                detailWorkspace.Visibility = Visibility.Visible;
            else
                detailWorkspace.Visibility = Visibility.Hidden;

            detailPaneToggleButton.IsChecked = !IsDetailVisible;
        }
                        
        private MasterDetailPresenter presenter;

        [CreateNew]
        public MasterDetailPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }
        
        private void DetailButtonClickEventHandler(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleDetail();
        }

        public void OnBuiltUp(string id)
        {
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {
        }

        public void ShowDetail(bool? show)
        {
            if (show == null)
            {
                ToggleDetail();
            }
            else
            {
                if (show != IsDetailVisible)
                    ToggleDetail();
            }
        }

        private void ToggleDetail()
        {
            if (detailWorkspace.SmartParts.Count > 0)
            {
                animation.From = detailRow.Height;

                if (detailRow.Height.Value > 0)
                {
                    animation.To = new GridLength(0, GridUnitType.Pixel);
                    detailPaneToggleButton.IsChecked = true;
                }
                else
                {
                    detailWorkspace.Visibility = Visibility.Visible;

                    if ((detailRow.ActualHeight == 0) && (prevGridLength.Value == 0))
                    {
                        prevGridLength = new GridLength(350, GridUnitType.Pixel);
                    }

                    detailPaneToggleButton.IsChecked = false;

                    animation.To = prevGridLength;
                }

                animation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100));
                detailRow.BeginAnimation(RowDefinition.HeightProperty, animation);
            }
        }

        private void DetailPaneToggleButtonClickEventHandler(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleDetail();
        }

        #region IMasterDetailView Members

        public void ShowInSearchWorkspace(object view)
        {
            searchWorkspace.Visibility = Visibility.Visible;
            searchWorkspace.Show(view);
        }

        public void ShowInMasterWorkspace(object view)
        {
            masterWorkspace.Show(view);
        }

        public void ShowInDetailWorkspace(object view)
        {
            detailWorkspace.Show(view);
            splitterRow.Height = new GridLength(7, GridUnitType.Pixel);
            HasDetailView = true;
        }

        public void LoadUserSettings(MasterDetailViewSettingsRepository settings)
        {
            if (HasDetailView)
            {
                detailRow.Height = new GridLength(settings.DetailRowHeight);
                OnDetailRowHeightUpdated();
            }
        }

        public void SaveUserSettings(MasterDetailViewSettingsRepository settings)
        {
            settings.DetailRowHeight = detailRow.ActualHeight;
        }
                
        #endregion

        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            ISmartPartInfoProvider provider = masterWorkspace.ActiveSmartPart as ISmartPartInfoProvider;

            if (provider != null)
                return provider.GetSmartPartInfo(typeof(SmartPartInfo));
            else
                return new SmartPartInfo();
        }

        #endregion

        private void SelectionChangedEventHandler(object sender, DataGridSelectionChangedEventArgs e)
        {
            grid = e.OriginalSource as Controls.DataGrid;
        }

        public IList<string> GetCurrentItemInfo()
        {
            IList<string> infos = new List<string>();

            if (grid != null && grid.SelectedItems.Count == 1)
            {
                int i = 0;

                foreach (Column column in grid.VisibleColumns)
                {
                    PropertyInfo info = grid.SelectedItem.GetType().GetProperty(column.FieldName);
  
                    if (info != null)
                    {
                        object value = info.GetValue(grid.SelectedItem, null);

                        if (value != null)
                        {
                            if (!(value is bool || value is bool?))
                            {
                                infos.Add(value.ToString());
                                i++;
                            }
                        }
                    }

                    if (i == 2)
                    {
                        break;
                    }
                }
            }

            return infos;
        }

    }
}
