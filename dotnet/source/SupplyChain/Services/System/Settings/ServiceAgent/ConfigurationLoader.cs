using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Specialized;
using Imi.SupplyChain.Settings.BusinessLogic;
using Imi.SupplyChain.Settings.BusinessEntities;

namespace Imi.SupplyChain.Services.Settings.ServiceAgent
{
    public class ConfigurationLoader : MarshalByRefObject
    {
        private string _baseDirectory = null;
        private const string _containerName = "/Imi.SupplyChain.UX.Shell/Configuration";
        private Dictionary<string, DateTime> _filesLastWrite; 
        
        public ConfigurationLoader()
        {
            FileInfo serverConfigFileInfo = new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            _baseDirectory = serverConfigFileInfo.DirectoryName;
            _filesLastWrite = new Dictionary<string, DateTime>();
        }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(_baseDirectory))
            {
                StoreFiles();
            }
        }

        private void StoreFiles()
        {
            var containers = new Dictionary<string, CreateOrUpdateBlobParameters>();
            var files = Directory.GetFiles(_baseDirectory, "client.config");

            foreach (string fileName in files)
            {
                 DateTime lastWrite = File.GetLastWriteTimeUtc(fileName);

                //Check if file has been accessed since last execute 
                if (_filesLastWrite.ContainsKey(fileName))
                {  
                    if (_filesLastWrite[fileName] == lastWrite)
                    {
                        continue;
                    }
                }

                string fileContents = null;

                using (var reader = new StreamReader(fileName, Encoding.UTF8))
                {
                    fileContents = reader.ReadToEnd();
                }

                string blobName = Path.GetFileName(fileName);
                Blob blob = CreateBlob(blobName, fileContents);

                if (blob != null)
                {
                    string containerName = string.Format("{0}/Files", _containerName);

                    if (!string.IsNullOrEmpty(containerName))
                    {
                        if (!containers.Keys.Contains(containerName))
                        {
                            containers.Add(containerName, CreateContainer(containerName));
                        }

                        containers[containerName].Blobs.Add(blob);
                    }
                }

                //Set last accessed time
                if (!_filesLastWrite.ContainsKey(fileName))
                {
                    _filesLastWrite.Add(fileName, lastWrite);
                }
                else
                {
                    _filesLastWrite[fileName] = lastWrite;
                }
            }

            var action = new CreateOrUpdateBlobAction();

            foreach (string containerName in containers.Keys)
            {
                action.Execute(containers[containerName]);
            }
        }

        private CreateOrUpdateBlobParameters CreateContainer(string containerName)
        {
            return new CreateOrUpdateBlobParameters
            {
                ContainerName = containerName,
                Blobs = new List<Blob>()
            };
        }
                
        private Blob CreateBlob(string name, string data)
        {
            Blob blob = null;

            if (!string.IsNullOrEmpty(data))
            {
                blob = new Blob
                {
                    Data = data,
                    Name = name,
                };
            }

            return blob;
        }
    }
}
