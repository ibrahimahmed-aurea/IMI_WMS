using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX.Services;
using Imi.Framework.Wpf.Controls;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using Imi.SupplyChain.Services.Settings.DataContracts;
using Imi.SupplyChain.UX.Shell.Views;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Shell.Services
{
    public class FavoritesService : IFavoritesService
    {
        [WcfServiceDependency]
        public ISettingsService SettingsService { get; set; }

        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }

        private string _assemblyName;
        private Dictionary<string, XmlDocument> _favoritesDictionary = null;
        private Dictionary<string, ShellDrillDownMenuItem> _favoritesUpdateCache;

        public FavoritesService()
        {
            _favoritesUpdateCache = new Dictionary<string, ShellDrillDownMenuItem>();
            
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string[] fullNameStrings = executingAssembly.FullName.Split(new char[] { ',' });
            _assemblyName = fullNameStrings[0];
        }

        public XmlDocument GetFavorites(string applicationName)
        {
            if (_favoritesDictionary == null)
            {
                LoadFavorites();
            }
            
            if ((_favoritesDictionary != null) && (_favoritesDictionary.ContainsKey(applicationName)))
            {
                return _favoritesDictionary[applicationName];
            }
            else
            {
                return null;
            }
        }

        public void LoadFavorites()
        {
            _favoritesDictionary = new Dictionary<string, XmlDocument>();

            string containerName = GetContainerName();

            FindBlobRequest request = new FindBlobRequest
            {
                FindBlobParameters = new FindBlobParameters
                {
                    ContainerName = containerName,
                    BlobName = "%"
                }
            };

            FindBlobResponse response = SettingsService.FindBlob(request);

            if ((response != null) && (response.FindBlobResult != null))
            {
                foreach (Blob blob in response.FindBlobResult)
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(blob.Data);

                    _favoritesDictionary.Add(blob.Name, xmlDocument);
                }
            }
        }

        private string GetContainerName()
        {
            return string.Format("{0}/Favorites", _assemblyName);
        }

        private Blob CreateBlob(string applicationName, XmlDocument favorites)
        {
            Blob blob = null;

            StringBuilder xmlText = new StringBuilder();
            StringWriter w = new StringWriter(xmlText);
            favorites.Save(w);

            if (xmlText.Length > 0)
            {

                blob = new Blob
                             {
                                Data = xmlText.ToString(),
                                Name = applicationName,
                             };
            }

            return blob;
        }

        #region IFavoritesService Members

        public void SaveFavorites()
        {
            BlobCollection blobs = new BlobCollection();

            foreach (string applicationName in _favoritesUpdateCache.Keys)
            {
                ShellDrillDownMenuItem favoritesMenu = _favoritesUpdateCache[applicationName];
                XmlDocument xmlDocument = XmlToShellDrillDownMenuItemTransformer.Transform(favoritesMenu);

                Blob blob = CreateBlob(applicationName, xmlDocument);

                if (blob != null)
                {
                    blobs.Add(blob);
                }
            }

            CreateOrUpdateBlobParameters parameters = new CreateOrUpdateBlobParameters
                                                            {
                                                                ContainerName = GetContainerName(),
                                                                Blobs = blobs
                                                            };

            CreateOrUpdateBlobRequest request = new CreateOrUpdateBlobRequest();
            request.CreateOrUpdateBlobParameters = parameters;

            SettingsService.CreateOrUpdateBlob(request);
        }

        public void QueueForUpdate(string applicationName, DrillDownMenuItem favoritesMenu)
        {
            _favoritesUpdateCache[applicationName] = favoritesMenu as ShellDrillDownMenuItem;
        }

        #endregion
    }
}
