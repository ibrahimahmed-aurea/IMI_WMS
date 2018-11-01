using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Imi.Framework.UX;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.UX;
using Imi.SupplyChain.Warehouse.UX.Modules.Customs;
using Imi.SupplyChain.Warehouse.UX.Modules.Customs.Actions;
using Imi.SupplyChain.Warehouse.UX.Modules.Customs.CreateCustomsDeclaration.Translators;
using Imi.SupplyChain.Warehouse.UX.Views.CustomsDeclaration;
using Imi.SupplyChain.Warehouse.UX.Contracts.CustomsDeclaration.ServiceContracts;

namespace Imi.SupplyChain.Warehouse.UX.Modules.Customs.CreateCustomsDeclaration
{
    public class CreateCustomsDeclarationActions : CreateCustomsDeclarationActionsBase
    {
        public override void OnCreateCustomsDeclarationRunCreateCustomsDeclaration(WorkItem context, object caller, object target)
        {
            WorkItem wi = GetModuleWorkItem(context);

            if (wi.Items.FindByType<RunCreateCustomsDeclarationAction>().Count == 0)
                wi.Items.AddNew<RunCreateCustomsDeclarationAction>();

            CreateCustomsDeclarationViewResult viewResult = null;
            RunCreateCustomsDeclarationActionParameters actionParameters = null;
            CreateCustomsDeclarationViewToRunCreateCustomsDeclarationActionTranslator translator = null;
            bool isItemSelected = false;

            if (context.Items.FindByType<CreateCustomsDeclarationViewToRunCreateCustomsDeclarationActionTranslator>().Count > 0)
                translator = context.Items.FindByType<CreateCustomsDeclarationViewToRunCreateCustomsDeclarationActionTranslator>().Last();
            else
                translator = context.Items.AddNew<CreateCustomsDeclarationViewToRunCreateCustomsDeclarationActionTranslator>();

            if (context.Items.FindByType<CreateCustomsDeclarationViewResult>().Count > 0)
            {
                viewResult = context.Items.FindByType<CreateCustomsDeclarationViewResult>().Last();
                isItemSelected = true;
            }
            else
            {
                viewResult = new CreateCustomsDeclarationViewResult();

                if (context.Items.FindByType<CreateCustomsDeclarationViewParameters>().Count() > 0)
                {
                    CreateCustomsDeclarationViewParameters viewParameters = context.Items.FindByType<CreateCustomsDeclarationViewParameters>().Last();

                    viewResult.CustomsDeclarantId = viewParameters.CustomsDeclarantId;
                }
            }

            actionParameters = translator.Translate(viewResult);
            actionParameters.IsItemSelected = isItemSelected;
            actionParameters.IsMultipleItemsSelected = false;

            ICreateCustomsDeclarationView view = context.SmartParts.FindByType<CreateCustomsDeclarationView>().Last();

            try
            {
                if (!view.Validate())
                    return;

                ActionCatalog.Execute(ActionNames.RunCreateCustomsDeclaration, context, caller, actionParameters);

                if (isItemSelected)
                {
                    if (context.Items.Get("CreateCustomsDeclarationResponse") != null)
                    {
                        CreateCustomsDeclarationResponse serviceResponce = (CreateCustomsDeclarationResponse)context.Items.Get("CreateCustomsDeclarationResponse");

                        if (serviceResponce.CreateCustomsDeclarationResult != null)
                        {
                            if (serviceResponce.CreateCustomsDeclarationResult.CustomsDeclarationId != null)
                            {
                                viewResult.CustomsDeclarationId = serviceResponce.CreateCustomsDeclarationResult.CustomsDeclarationId;
                            }
                        }
                    }
                }

                // Check if view should be closed.
                bool ignoreClosing = ((context.Items.Get("IgnoreClose") != null) &&
                                    ((bool)(context.Items.Get("IgnoreClose"))));

                if (!ignoreClosing)
                {
                    /* close wrapper if view was dynamically loaded */
                    object wrapper = context.Items.Get("Imi.SupplyChain.Warehouse.UX.Views.CustomsDeclaration.ICreateCustomsDeclarationView");

                    if (wrapper == null)
                        WorkspaceLocatorService.FindContainingWorkspace(context, view).Close(view);
                    else
                        WorkspaceLocatorService.FindContainingWorkspace(context, wrapper).Close(wrapper);
                }
            }
            catch (Imi.SupplyChain.UX.ValidationException ex)
            {
                view.HandleValidationResult(ValidationHelper.ConvertToResult((Imi.SupplyChain.UX.ValidationException)ex));
            }
        }
    }
}
