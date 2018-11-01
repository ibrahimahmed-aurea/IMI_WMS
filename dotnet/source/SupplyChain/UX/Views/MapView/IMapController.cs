using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Views
{
    public interface IMapController
    {
        void DropRoute();
        void InitMap();
        void ShowRoute(IList<MapLocation> locations);
        void SetDefaultMapLocation(LatLong position);
        void ToggleDisplayStyle();
    }
}
