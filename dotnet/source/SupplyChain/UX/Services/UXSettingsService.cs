using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeUI;
using Imi.Framework.UX.Services;
using Imi.Framework.UX.Settings;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using Imi.SupplyChain.Services.Settings.DataContracts;
using Imi.SupplyChain.UX.Settings;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Services
{
    public class SettingsAppliedEventArgs : EventArgs
    {
        private object _target;
        private ISettingsProvider _provider;

        public SettingsAppliedEventArgs(object target, ISettingsProvider provider)
        {
            this._target = target;
            this._provider = provider;
        }

        public object Target 
        { 
            get
            {
                return _target;
            }
        }

        public ISettingsProvider Provider
        {
            get
            {
                return _provider;
            }
        }
    }

    public class UXSettingsService : IUXSettingsService
    {
        [WcfServiceDependency]
        public ISettingsService SettingsService { get; set; }

        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }

        private IDictionary<object, ISettingsProvider> _providerDictionary;
        private IDictionary<string, Blob> _blobDictionary;
        private string _containerName;
        private bool _useBackgroundThread;
        private bool _isApplying;
        private SynchronizationContext _synchronizationContext;

        public event EventHandler<SettingsAppliedEventArgs> SettingsApplied;

        public UXSettingsService()
            : this(null, false)
        {
        }

        public UXSettingsService(string containerName)
            : this(containerName, false)
        {
        }
               
        public UXSettingsService(string containerName, bool useBackgroundThread)
        { 
            _providerDictionary = new Dictionary<object, ISettingsProvider>();
            _blobDictionary = new Dictionary<string, Blob>();
            _containerName = containerName;
            _useBackgroundThread = useBackgroundThread;
            _synchronizationContext = SynchronizationContext.Current;
        }
                
        public string ContainerName 
        {
            get
            {
                return _containerName;
            }
            set
            {
                _containerName = value;
            }
        }
        
        public void AddProvider(object target, ISettingsProvider provider)
        {
            _providerDictionary[target] = provider;

            if (_isApplying)
                ApplySettings(target);
        }

        public void RemoveProvider(object target)
        {
            _providerDictionary.Remove(target);
        }

        public void SaveSettings()
        {
            if (string.IsNullOrEmpty(ContainerName))
                throw new ArgumentNullException("ContainerName");

            BlobCollection blobs = new BlobCollection();

            foreach (KeyValuePair<object, ISettingsProvider> providerEntry in _providerDictionary)
            {
                object settings = providerEntry.Value.SaveSettings(providerEntry.Key);

                Blob blob = CreateBlob(providerEntry.Value.GetKey(providerEntry.Key), settings);

                if (blob != null)
                {
                    blobs.Add(blob);
                }
            }

            if (_useBackgroundThread)
            {
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    try
                    {
                        CreateOrUpdateBlob(blobs);
                    }
                    catch (Exception ex)
                    {
                        _synchronizationContext.Send((state) =>
                        {
                            throw ex;
                        }, null);
                    }
                });
            }
            else
            {
                CreateOrUpdateBlob(blobs);
            }
        }

        private void CreateOrUpdateBlob(BlobCollection blobs)
        {
            CreateOrUpdateBlobParameters parameters = new CreateOrUpdateBlobParameters
            {
                ContainerName = GetContainerName(),
                Blobs = blobs
            };

            CreateOrUpdateBlobRequest request = new CreateOrUpdateBlobRequest();
            request.CreateOrUpdateBlobParameters = parameters;

            try
            {
                SettingsService.CreateOrUpdateBlob(request);
            }
            catch
            {
            }
        }

        public void LoadSettings()
        {
            if (_useBackgroundThread)
            {
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    try
                    {
                        InternalLoad();

                        _synchronizationContext.Send((state) =>
                            {
                                ApplySettings();
                            }, null);
                    }
                    catch (Exception ex)
                    {
                        _synchronizationContext.Send((state) =>
                        {
                            throw ex;
                        }, null);
                    }
                });
            }
            else
            {
                InternalLoad();
                ApplySettings();
            }
        }

        private void InternalLoad()
        {
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
                    _blobDictionary[blob.Name] = blob;
                }
            }
        }

        public void ApplySettings()
        {
            _isApplying = true;

            try
            {
                foreach (KeyValuePair<object, ISettingsProvider> providerEntry in _providerDictionary.ToArray())
                {
                    ApplySettings(providerEntry.Key);
                }
            }
            finally
            {
                _isApplying = false;
            }
        }

        public void ApplySettings(object target)
        {
            ISettingsProvider provider = _providerDictionary[target];

            string key = provider.GetKey(target);

            if (_blobDictionary.ContainsKey(key))
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(_blobDictionary[key].Data)))
                {
                    XmlSerializer serializer = new XmlSerializer(provider.GetSettingsType(target));
                    provider.LoadSettings(target, serializer.Deserialize(reader));
                }
            }
            else
            {
                provider.LoadSettings(target, null);
            }

            if (SettingsApplied != null)
            {
                SettingsApplied(this, new SettingsAppliedEventArgs(target, provider));
            }
        }
              
        private string GetContainerName()
        {
            return string.Format("{0}/Settings", ContainerName);
        }

        private Blob CreateBlob(string key, object value)
        {
            Blob blob = new Blob();
            blob.Name = key;
            StringBuilder sb = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                XmlSerializer serializer = new XmlSerializer(value.GetType());

                serializer.Serialize(writer, value);

                blob.Data = sb.ToString();
            }

            return blob;
        }
    }
}
