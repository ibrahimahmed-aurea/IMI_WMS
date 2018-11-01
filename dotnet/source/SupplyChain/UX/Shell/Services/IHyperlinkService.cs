using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.Services.Settings.DataContracts;
using Imi.SupplyChain.Services.Settings.ServiceContracts;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.ServiceModel;
using System.Collections.Specialized;
using System.Collections;
using Imi.SupplyChain.UX.Infrastructure;

namespace Imi.SupplyChain.UX.Shell.Services
{
    [ServiceContract]
    public interface IHyperlinkService
    {
        [OperationContract]
        string GetInstanceIdentifier();

        [OperationContract]
        bool ExecuteHyperlink(Uri hyperlink);

        void ExecuteHyperlink(ShellHyperlink hyperlink);
        
        bool RedirectToExistingInstance(Uri hyperlink);
        void Start();
        void Stop();
    }
}
