using System;
using System.Reflection;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.Services.Settings.DataContracts;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using Imi.SupplyChain.UX.Modules.OrderManagement.Services;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views
{
    public class ChooseUserPresenter : AsyncDataPresenter<IAOMWebServiceWrapper, IChooseUserView>
    {
        string _assemblyName;
        string defaultUser;

        [InjectionConstructor]
        public ChooseUserPresenter([ServiceDependency] IAOMWebServiceWrapper getUserSettingsService)
            : base(getUserSettingsService)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly(); 
            string[] fullNameStrings = executingAssembly.FullName.Split(new char[] { ',' });
            _assemblyName = fullNameStrings[0];
            defaultUser = string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName);
        }

        [ServiceDependency]
        public IOMSSessionContext OmsSessionContext { get; set; }

        [WcfServiceDependency]
        public ISettingsService SettingsService { get; set; }

        public void RefreshView() { }

        private string GetActiveUser()
        {
            if ((OmsSessionContext != null) && (!string.IsNullOrEmpty(OmsSessionContext.DomainUser)))
                return OmsSessionContext.DomainUser;

            return defaultUser;
        }

        public void SetDefaultUser()
        {
            string defaultUser = LoadDefaultUser();
            if (String.IsNullOrEmpty(defaultUser))
                defaultUser = OmsSessionContext.OMSLogicalUserId;

            if (!String.IsNullOrEmpty(defaultUser))
            {
                View.SelectedUserId = defaultUser;
            }
        }

        //public void Close()
        //{
        //    this.View.Close();
        //    WorkItem.SmartParts.Remove(this.View);
        //}
        
        public void SetSelectedOMSUser(OMSUser selectedOMSUser)
        {
            OmsSessionContext.OMSLogicalUserId = selectedOMSUser.UserId;
            SaveOrUpdateDefaultUser(selectedOMSUser.UserId);
        }

        protected void SaveOrUpdateDefaultUser(string user)
        {
            BlobCollection blobs = new BlobCollection();
            blobs.Add(CreateBlob(user));

            CreateOrUpdateBlobParameters parameters = new CreateOrUpdateBlobParameters
            {
                 ContainerName = GetContainerName(),
                 Blobs = blobs
            };

            CreateOrUpdateBlobRequest request = new CreateOrUpdateBlobRequest();
            request.CreateOrUpdateBlobParameters = parameters;

            SettingsService.CreateOrUpdateBlob(request);
        }

        protected string LoadDefaultUser()
        {
            string defaultUserId = null; ;
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
            if ((response != null) && (response.FindBlobResult != null) && (response.FindBlobResult.Count > 0))
            {
                foreach (Blob blob in response.FindBlobResult)
                {
                    defaultUserId = blob.Data;
                }
            }
            return defaultUserId;
        }

        private Blob CreateBlob(string aString)
        {
            Blob blob = null;
            if (aString.Length > 0)
            {
                blob = new Blob
                {
                    Data = aString,
                    Name = "OMSLogicUserId",
                };
            }
            return blob;
        }

        private string GetContainerName()
        {
            return string.Format("{0}/Settings", _assemblyName);
        }
    }
}
