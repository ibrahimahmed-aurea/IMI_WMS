// Generated from template: .\UX\Module\ModuleActionTemplate.cst

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.Configuration;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.Warehouse.UX.Contracts.PickLoadCarrier.ServiceContracts;
using Imi.SupplyChain.Warehouse.UX.Contracts.PickLoadCarrier.DataContracts;
using Imi.Framework.Services;

namespace Imi.SupplyChain.Warehouse.UX.Modules.PackStation.Actions
{
    public class RunPackRowForPackStationAction : RunPackRowForPackStationActionBase
    {
        public override void OnRunPackRowForPackStation(WorkItem context, object caller, object target)
        {
            if (context.Items.Contains("IgnoreClose"))
                context.Items.Remove(context.Items["IgnoreClose"]);

            if (context.Items.Contains("DialogResult"))
                context.Items.Remove(context.Items["DialogResult"]);
            if (context.Items.Contains("RefreshPackStationOverview"))
                context.Items.Remove(context.Items["RefreshPackStationOverview"]);

            RunPackRowForPackStationActionParameters actionParameters = target as RunPackRowForPackStationActionParameters;



            PackRowForPackStationRequest serviceRequest = new PackRowForPackStationRequest();
            serviceRequest.PackRowForPackStationParameters = new PackRowForPackStationParameters();

            serviceRequest.PackRowForPackStationParameters.FromLoadCarrierId = actionParameters.FromLoadCarrierId;
            serviceRequest.PackRowForPackStationParameters.PickedQuantity = actionParameters.PickedQuantity;
            serviceRequest.PackRowForPackStationParameters.ToLoadCarrierId = actionParameters.ToLoadCarrierId;
            serviceRequest.PackRowForPackStationParameters.PickOrderLineNumber = actionParameters.PickOrderLineNumber;
            try
            {
                PackRowForPackStationResponse serviceResponse = PickLoadCarrierService.PackRowForPackStation(serviceRequest);

                context.Items.Add(DialogResult.None, "DialogResult");
                context.Items.Add(serviceResponse.PackRowForPackStationResult.RefreshGUI.Value, "RefreshPackStationOverview");

            }
            catch (FaultException<ValidationFault> ex)
            {
                if (ex.Detail.ValidationError == ValidationError.NullValue)
                    throw new Imi.SupplyChain.UX.NullValidationException(ex.Detail.Message, ex.Detail.ParameterName);
                else
                    throw new Imi.SupplyChain.UX.ValidationException(ex.Detail.Message, ex.Detail.ParameterName);
            }
        }
	}
}

