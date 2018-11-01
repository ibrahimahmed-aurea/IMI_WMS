using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI.SmartParts;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.Transportation.UX.Views.Route;
using Microsoft.Practices.CompositeUI;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.Transportation.UX.Views.RouteMap
{
	[SmartPart]
	public partial class RouteMapView  : System.Windows.Controls.UserControl, IRouteMapView, IActionProvider, ISmartPartInfoProvider, IBuilderAware
	{
        private Exception error;
        private bool isDetailView;
							
		private RouteMapPresenter presenter;
        private IMapController mapController;
        private bool webBrowserLoaded;

        public string Title { get; set; }

        [CreateNew]
        public RouteMapPresenter Presenter
        {
            get { return presenter; }
            set 
            { 
                presenter = value;
                presenter.View = this;
            }
        }

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService { get; set; }
		
        public RouteMapView()
        {
            isDetailView = true;
			RefreshDataOnShow = true;
			this.InitializeComponent();
            this.IsVisibleChanged +=new DependencyPropertyChangedEventHandler(IsVisibleChangedEventHandler);
		}

        private object lastRoute = null;

        private void ShowRoute()
        {
            if (webBrowserLoaded)
            {
                if (this.DataContext == null)
                    return;

                if (lastRoute == this.DataContext)
                    return;

                lastRoute = this.DataContext;

                IEnumerable<RouteMapViewResult> route = this.DataContext as IEnumerable<RouteMapViewResult>;

                IEnumerable<RouteMapViewResult> sortedRoute =
                    from RouteMapViewResult stop in route
                    orderby stop.StopSequenceNumber
                    select stop;
                    
                                                              
                List<MapLocation> mapLocations = new List<MapLocation>();

                foreach (RouteMapViewResult line in sortedRoute)
                {
                    double latitude, longitude;

                    if (!string.IsNullOrEmpty(line.Latitude) &&
                        !line.Latitude.Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                    {
                        line.Latitude = line.Latitude.Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        line.Latitude = line.Latitude.Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }

                    if (!string.IsNullOrEmpty(line.Longitude) &&
                        !line.Longitude.Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                    {
                        line.Longitude = line.Longitude.Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        line.Longitude = line.Longitude.Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }

                    if (double.TryParse(line.Latitude, out latitude) &&
                        double.TryParse(line.Longitude, out longitude))
                    {
                        try
                        {
                            //LatLong position = StringToLatLongConverter.ToLatLong(line.Instructions1);

                            LatLong position = new LatLong();
                            position.Latitude = latitude;
                            position.Longitude = longitude;

                            mapLocations.Add(new MapLocation()
                                        {
                                            PushPin = new PushPin
                                                        {
                                                            Identity = line.NodeId,
                                                            Title = string.Format("{0}. {1}", line.StopSequenceNumber, line.NodeDescription),
                                                            Description = string.Format("{0} {1}", line.TypeOfStopText, line.Instructions1),
                                                        },
                                            Position = position
                                        }
                                    );
                        }
                        catch (Exception)
                        {
                            // Suppress
                        }
                    }
                }

                mapController.InitMap();

                if (mapLocations.Count > 0)
                {
                    mapController.ShowRoute(mapLocations);
                }
                else
                {
                    mapController.SetDefaultMapLocation(StringToLatLongConverter.ToLatLong("N56 09 22.37 E13 45 58.82"));
                }
            }
        }

        private void WebBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            webBrowserLoaded = true;
            ShowRoute();
        }

		public void Update(object parameters)
		{
            presenter.UpdateView(parameters as RouteMapViewParameters);
		}
		
		public void Refresh()
		{
			presenter.RefreshView();
		}

        public void SetFocus() { }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                if (presenter != null)
                    presenter.OnViewShow();

                if (mapController == null)
                {
                    mapController = new MapBrowserController(this.webBrowser);
                    this.webBrowser.LoadCompleted += new LoadCompletedEventHandler(WebBrowserLoadCompleted);
                    Presenter.WorkItem.Items.Add(mapController);
                }
                else
                {
                    ShowRoute();
                }

            }
        }
		
		public bool RefreshDataOnShow
		{
			get;
			set;
		}

        private void ToggleDetailCommandExecutedEventHandler(object sender, ExecutedRoutedEventArgs e)
        {
            presenter.ShowDetail(null);
        }

        private void ErrorInfoButtonClickEventHanlder(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
                        
            ShellInteractionService.ShowMessageBox(StringResources.View_UpdateError, error.Message, error.ToString(), Imi.SupplyChain.UX.Infrastructure.MessageBoxButton.Ok, Imi.SupplyChain.UX.Infrastructure.MessageBoxImage.Error);
        }

        #region IRouteMapView Members

		public void PresentData(object data)
        {
            this.DataContext = data;

            ShowRoute();
                       
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(data);
			
			if (collectionView != null)
			{
				presenter.OnViewUpdated(collectionView.CurrentItem as RouteMapViewResult);
				collectionView.CurrentChanged += CurrentChangedEventHandler;
			}
			else
			{
				presenter.OnViewUpdated(data as RouteMapViewResult);
			}
        }

        private void CurrentChangedEventHandler(object sender, EventArgs e)
        {
            RouteMapViewResult viewResult = CollectionViewSource.GetDefaultView(DataContext).CurrentItem as RouteMapViewResult;
			presenter.OnViewUpdated(viewResult);
        }

        public void Search(object parameters)
        {
            presenter.Search(parameters);
        }

        public void UpdateProgress(int progressPercentage)
        {
            errorPanel.Visibility = Visibility.Hidden;
            webBrowser.Visibility = Visibility.Visible;

            if (progressPercentage < 100)
            {
                PresentData(null);
                progressBar.Visibility = Visibility.Visible;
            }
            else
            {
                progressBar.Visibility = Visibility.Hidden;
            }
        }

        public RouteMapViewResult CurrentItem
        {
            get;
            set;
        }

        public void EnableComponent(string name, bool isEnabled)
        {
        }

        public void SetComponentDataContext(string componentName, object data)
        {
        }

        public bool ExistsInCreateDialog { get; set; }

        public void ApplyUIAttentionLevel(string componentName, UIAttentionLevel level, RouteMapViewResult viewResult)
        {
            return;
        }

        public bool IsInlineView
        {
            get
            {
                return false;
            }
            set
            {
                return;
            }
        }

        public void OnViewUpdated()
        {
            throw new NotImplementedException();
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
                    Visibility = Visibility.Visible;
                else
                    Visibility = Visibility.Collapsed;
            }
        }

        public bool HandleError(Exception error)
        {
            this.error = error;
            webBrowser.Visibility = Visibility.Hidden;
            errorPanel.Visibility = Visibility.Visible;
            return true;
        }

        public void ShowComponent(string name, UXVisibility visibility)
        {
        }

        public void HandleValidationResult(Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult result)
        {
        }

        public bool Validate()
        {
            return true;
        }

		#endregion

        #region ISmartPartInfoProvider Members

        public ISmartPartInfo GetSmartPartInfo(Type smartPartInfoType)
        {
            if (string.IsNullOrEmpty(Title))
                return new SmartPartInfo("Route Map", "");
            else
                return new SmartPartInfo(Title, "");
        }

        #endregion
		
		#region IActionProvider Members

        public ICollection<ShellAction> GetActions()
        {
            return Presenter.GetActions();
        }

        #endregion

        #region IDetailViewToggled Members

        public void BringCurrentItemIntoView()
        {
            //OWDBGrid1.BringItemIntoView(OWDBGrid1.CurrentItem);
        }

        public bool IsDetailView
        {
            get
            {
                return isDetailView;
            }
            set
            {
                isDetailView = value;
            }
        }

        #endregion
		
		#region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            presenter.OnViewReady();
        }

        public void OnTearingDown()
        {
            
        }

        #endregion

        #region IDrillDownView Members

        public void EnableDrillDown(DrillDownArgs args)
        {
            
        }

        public bool IsDrillDownEnabled
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region IImportEnabledView Members

        public event EventHandler<ShowImportViewArgs> ShowImportView;


        public bool IsImportEnabled
        {
            get;
            set;
        }

        public void RaiseShowImportView(ShowImportViewArgs args)
        {
            if (ShowImportView != null)
            {
                ShowImportView(this, args);
            }
        }

        #endregion
    }
}
