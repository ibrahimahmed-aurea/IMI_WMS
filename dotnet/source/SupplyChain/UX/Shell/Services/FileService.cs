using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.Services.Settings.DataContracts;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using Imi.SupplyChain.UX.Infrastructure.Services;

namespace Imi.SupplyChain.UX.Shell.Services
{
    public class FileService : IFileService
    {
        [WcfServiceDependency]
        public ISettingsService SettingsService { get; set; }

        private const string _containerName = "/Imi.SupplyChain.UX.Shell/Configuration/Files";

        public IEnumerable<string> GetFileNames()
        {
            List<string> nameList = new List<string>();

            var blobs = GetBlobs("%");

            if ((blobs != null) && (blobs.Count > 0))
            {
                IEnumerable<string> result = from Blob b in blobs
                                             select b.Name;

                if (result.Count() > 0)
                {
                    nameList.AddRange(result);
                }

            }

            return nameList;
        }

        public string GetFile(string fileName)
        {
            var blobs = GetBlobs(fileName);

            if ((blobs != null) && (blobs.Count > 0))
            {
                IEnumerable<string> result = from Blob b in blobs
                                             select b.Data;

                if (result.Count() > 0)
                {
                    return result.First();
                }

            }

            return string.Empty;
        }

        private IList<Blob> GetBlobs(string fileName)
        {
            FindBlobRequest request = new FindBlobRequest
            {
                FindBlobParameters = new FindBlobParameters
                {
                    ContainerName = _containerName,
                    BlobName = fileName
                }
            };

            FindBlobResponse response = SettingsService.FindBlob(request);

            if ((response != null) && (response.FindBlobResult != null))
            {
                return response.FindBlobResult;
            }

            return new List<Blob>();
        }
    }
}
