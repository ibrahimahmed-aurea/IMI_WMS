using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Imi.Framework.Wpf.Controls;

namespace Imi.SupplyChain.UX.Shell.Services
{
    public interface IFavoritesService
    {
        XmlDocument GetFavorites(string applicatioName);
        void LoadFavorites();
        void SaveFavorites();
        void QueueForUpdate(string applicationName, DrillDownMenuItem favoritesMenu);
    }
}
