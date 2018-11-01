using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Security.Permissions;

namespace Imi.SupplyChain.UX.Views
{

    enum MapDisplayStyle
	{
	         BirdRoute, RoadRoute
	}

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [global::System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class MapBrowserController : IMapController
    {
        private WebBrowser browser;
        private IList<MapLocation> currentRoute;
        
        public MapBrowserController(WebBrowser browser)
        {
            this.browser = browser;
            this.browser.ObjectForScripting = this;


            // Load HTML document as a stream, assumption is that BaseMap.htm is in same directory as this class
            Assembly ass = Assembly.GetExecutingAssembly();
            Stream source = ass.GetManifestResourceStream(string.Format("{0}.MapView.BaseMap.htm",this.GetType().Namespace));

            // Navigate to HTML document stream
            this.browser.NavigateToStream(source);

        }

        public void DropRoute()
        {
            if (currentRoute != null)
            {
                currentRoute.Clear();
            }
        }

        public void GetDirections(List<string> stops)
        {
            // Clear map
            InitMap();

            foreach (string stop in stops)
            {
                object[] args = new object[] { stop };
                browser.InvokeScript("AddStop", args);

            }

            // Clear map
            browser.InvokeScript("GetDirections");

        }


        public void InitMap()
        {
            // Clear map
            browser.InvokeScript("InitMap");
        }

        public void ShowPushPin(LatLong position, PushPin pin)
        {
            // Show pushpin
            object[] args = new object[] { pin.Identity
                                          ,position.Latitude
                                          ,position.Longitude
                                          ,pin.Title
                                          ,pin.Description };
            browser.InvokeScript("ShowPushPin", args);
        }

        public void ShowPolyline(IList<MapLocation> locations)
        {
            // Clear map
            InitMap();
            ShowPolylineInternal(locations);
        }

        private void ShowPolylineInternal(IList<MapLocation> locations)
        {
            // Can only show polyline if there are more than one location
            if (locations.Count > 1)
            {
                // Create polyline
                foreach (MapLocation location in locations)
                {
                    object[] args = new object[] { location.Position.Latitude
                                                  ,location.Position.Longitude };
                    browser.InvokeScript("AddLinePoint", args);
                }

                // Show polyline
                {
                    object[] args = new object[] { "aMapId" };
                    browser.InvokeScript("ShowPolyline", args);
                }
            }

            // Add pushpins in reverse order 
            // TODO implement this in javascript instead
            for (int i = locations.Count; i > 0; i--)
            {
                if (locations[i - 1].PushPin != null)
                {
                    LatLong position = locations[i - 1].Position;
                    PushPin pin = locations[i - 1].PushPin;
                    ShowPushPin(position, pin);
                }
            }

            {
                object[] args = new object[] { "aMapId" };
                browser.InvokeScript("SetMapView", args);
            }

        }

        public void ShowRoadRoute(IList<MapLocation> locations)
        {
            // Clear map
            InitMap();

            // Can only show lines if there are more than one location
            if (locations.Count > 1)
            {
                // Create polyline
                foreach (MapLocation location in locations)
                {
                    if (location.PushPin != null)
                    {
                        object[] args = new object[] { location.Position.Latitude
                                                      ,location.Position.Longitude };
                        browser.InvokeScript("AddStopPoint", args);
                    }
                }

                // Show directions
                {
                    browser.InvokeScript("GetDirections");
                }
            }
        }

        public void GetPushPins()
        {
            // Add pushpins in reverse order 
            // TODO implement this in javascript instead
            for (int i = currentRoute.Count; i > 0; i--)
            {
                MapLocation location = currentRoute[i - 1];
                if (location.PushPin != null)
                {
                    ShowPushPin(location.Position, location.PushPin);
                }
            }
        }

        public void FindAddress(string address)
        {
            object[] args = new object[] { address };
            browser.InvokeScript("FindAddress", args);
        }

        public void SetDefaultMapLocation(LatLong position)
        {
            object[] args = new object[] { position.Latitude, position.Longitude };
            browser.InvokeScript("DefaultMap", args);
        }

        private MapDisplayStyle displayStyle = MapDisplayStyle.BirdRoute;

        public void ToggleDisplayStyle()
        {
            if (displayStyle == MapDisplayStyle.BirdRoute)
                displayStyle = MapDisplayStyle.RoadRoute;
            else
                displayStyle = MapDisplayStyle.BirdRoute;

            if (currentRoute != null)
            {
                if (displayStyle == MapDisplayStyle.BirdRoute)
                {
                    ShowPolyline(currentRoute);
                }
                else
                {
                    ShowRoadRoute(currentRoute);
                }
            }
        }

        public void ShowRoute(IList<MapLocation> locations)
        {
            currentRoute = locations;

            if (displayStyle == MapDisplayStyle.BirdRoute)
            {
                ShowPolyline(currentRoute);
            }
            else
            {
                ShowRoadRoute(currentRoute);
            }
            
        }
    }
}
