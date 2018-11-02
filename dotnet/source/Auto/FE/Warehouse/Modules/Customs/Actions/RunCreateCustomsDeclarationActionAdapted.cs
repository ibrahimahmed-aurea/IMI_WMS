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
using Imi.Framework.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Services;
using Imi.SupplyChain.UX.Views;
using Imi.SupplyChain.UX.Modules.Warehouse.Infrastructure.Services;
using Imi.SupplyChain.Warehouse.UX.Contracts.CustomsDeclaration.ServiceContracts;
using Imi.SupplyChain.Warehouse.UX.Contracts.CustomsDeclaration.DataContracts;

namespace Imi.SupplyChain.Warehouse.UX.Modules.Customs.Actions
{
    public class RunCreateCustomsDeclarationAction : RunCreateCustomsDeclarationActionBase
    {
        public override void OnRunCreateCustomsDeclaration(WorkItem context, object caller, object target)
        {
            if (context.Items.Contains("IgnoreClose"))
                context.Items.Remove(context.Items["IgnoreClose"]);

            if (context.Items.Contains("DialogResult"))
                context.Items.Remove(context.Items["DialogResult"]);

            RunCreateCustomsDeclarationActionParameters actionParameters = target as RunCreateCustomsDeclarationActionParameters;



            CreateCustomsDeclarationRequest serviceRequest = new CreateCustomsDeclarationRequest();
            serviceRequest.CreateCustomsDeclarationParameters = new CreateCustomsDeclarationParameters();

            serviceRequest.CreateCustomsDeclarationParameters.CustomsDeclarationType = actionParameters.CustomsDeclarationType;
            serviceRequest.CreateCustomsDeclarationParameters.CustomsClearanceType = actionParameters.CustomsClearanceType;
            serviceRequest.CreateCustomsDeclarationParameters.CustomsDeclarantId = actionParameters.CustomsDeclarantId;
            serviceRequest.CreateCustomsDeclarationParameters.UserId = actionParameters.UserId;
            serviceRequest.CreateCustomsDeclarationParameters.ClientId = actionParameters.ClientId;
            serviceRequest.CreateCustomsDeclarationParameters.WarehouseId = actionParameters.WarehouseId;
            serviceRequest.CreateCustomsDeclarationParameters.PayCustomsDeclaration = actionParameters.PayCustomsDeclaration;
            serviceRequest.CreateCustomsDeclarationParameters.CustomsDeclaration = actionParameters.CustomsDeclaration;
            try
            {
                CreateCustomsDeclarationResponse serviceResponse = CustomsDeclarationService.CreateCustomsDeclaration(serviceRequest);

                context.Items.Add(serviceResponse, "CreateCustomsDeclarationResponse");

                context.Items.Add(DialogResult.Ok, "DialogResult");

                // postback in parent workitem
                Bookmark bookmark = new Bookmark() { RowIdentity = serviceResponse.CreateCustomsDeclarationResult.RowIdentities.FirstOrDefault(), MultipleSelection = serviceResponse.CreateCustomsDeclarationResult.RowIdentities.Select(i => new Bookmark() { RowIdentity = i }).ToList() };
                context.Parent.Items.Add(bookmark);

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
