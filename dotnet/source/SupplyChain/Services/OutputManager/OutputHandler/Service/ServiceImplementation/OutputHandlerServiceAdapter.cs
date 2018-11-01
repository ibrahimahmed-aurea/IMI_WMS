using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.OM.Services.OutputHandler.ServiceContracts;
using Imi.SupplyChain.OM.OutputHandler.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Imi.SupplyChain.OM.Services.OutputHandler.DataContracts;

namespace Imi.SupplyChain.OM.Services.OutputHandler.ServiceImplementation
{
    public class OutputHandlerServiceAdapter : MarshalByRefObject, IOutputHandlerService
    {
        private CreateOutputAction _createOutputAction;
        private object _createOutputActionLock = new object();

        private FindPrinterInfoAction _findPrinterInfoAction;
        private object _findPrinterInfoActionLock = new object();

        public OutputHandlerServiceAdapter()
        {
            lock (_createOutputActionLock)
            {
                if (_createOutputAction == null)
                {
                    _createOutputAction = PolicyInjection.Create<CreateOutputAction>();
                }
            }
        }

        public void PassArgumentsFromServiceHost(System.ServiceModel.ServiceHost serviceHost)
        {
            if (_createOutputAction != null)
            {
                _createOutputAction.UpdateURL(serviceHost.Description.Endpoints.FirstOrDefault().ListenUri.ToString());
            }
        }

        public CreateOutputResponse CreateOutput(CreateOutputRequest request)
        {
            lock (_createOutputActionLock)
            {
                if (_createOutputAction == null)
                {
                    _createOutputAction = PolicyInjection.Create<CreateOutputAction>();
                }
            }

            CreateOutputResultCollection result = _createOutputAction.Execute(request.CreateOutputParametersCollection);

            CreateOutputResponse response = new CreateOutputResponse();

            response.CreateOutputResult = result;

            return response;
        }

        public FindPrinterInfoResponse FindPrinterInfo(FindPrinterInfoRequest request)
        {
            lock (_findPrinterInfoActionLock)
            {
                if (_findPrinterInfoAction == null)
                {
                    _findPrinterInfoAction = PolicyInjection.Create<FindPrinterInfoAction>();
                }
            }

            FindPrinterInfoResult result = _findPrinterInfoAction.Execute(request.FindPrinterInfoParameters);

            FindPrinterInfoResponse response = new FindPrinterInfoResponse();

            response.FindPrinterInfoResult = result;

            return response;
        }
    }
}
