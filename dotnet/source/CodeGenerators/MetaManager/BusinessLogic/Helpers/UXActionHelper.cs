using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    public class UXActionHelper : IUXActionHelper
    {
        private IModelService ModelService { get; set; }

        [Transaction(ReadOnly = false)]
        public UXAction CreateUXActionForMappableObject(IMappableUXObject uxObject, Application application, string name, string caption)
        {
            uxObject = (IMappableUXObject)ModelService.GetDomainObject(uxObject.Id, ModelService.GetDomainObjectType(uxObject));
            application = ModelService.GetDomainObject<Application>(application.Id);

            UXAction uxAction = CreateUXAction(uxObject, application, name, caption);

            if (uxAction != null)
            {
                CreateUXActionMap(uxAction, uxObject);

                ModelService.MergeSaveDomainObject(uxAction);
            }
            
            return uxAction;
        }
        
        [Transaction(ReadOnly = false)]
        private UXAction CreateUXAction(IMappableUXObject uxObject, Application application, string name, string caption)
        {
            UXAction uxAction = new UXAction();

            if (uxObject is Dialog)
            {
                Dialog d = ModelService.GetDomainObject<Dialog>(uxObject.Id);

                if (d != null)
                {
                    if (string.IsNullOrEmpty(caption))
                        uxAction.Caption = "Show " + d.Title;
                    else
                        uxAction.Caption = caption;

                    if (string.IsNullOrEmpty(name))
                        uxAction.Name = "Show" + d.Name;
                    else
                        uxAction.Name = name;
                }

                uxAction.MappedToObject = d;
                uxAction.Application = application;
            }
            else if (uxObject is ServiceMethod)
            {
                ServiceMethod s = ModelService.GetDomainObject<ServiceMethod>(uxObject.Id);

                if (s != null)
                {
                    if (string.IsNullOrEmpty(caption))
                        uxAction.Caption = s.Name;
                    else
                        uxAction.Caption = caption;

                    if (string.IsNullOrEmpty(name))
                        uxAction.Name = "Run" + s.Name;
                    else
                        uxAction.Name = name;
                }

                uxAction.MappedToObject = s;
                uxAction.Application = application;
            }
            else if (uxObject is Workflow)
            {
                Workflow workflow = ModelService.GetDomainObject<Workflow>(uxObject.Id);

                if (string.IsNullOrEmpty(caption))
                    uxAction.Caption = workflow.Name;
                else
                    uxAction.Caption = caption;

                if (string.IsNullOrEmpty(name))
                    uxAction.Name = string.Format("Run{0}Workflow", workflow.Name);
                else
                    uxAction.Name = name;

                uxAction.MappedToObject = workflow;
                uxAction.Application = application;
            }
            else if (uxObject is CustomDialog)
            {
                CustomDialog cd = ModelService.GetDomainObject<CustomDialog>(uxObject.Id);

                if (string.IsNullOrEmpty(caption))
                    uxAction.Caption = cd.Name;
                else
                    uxAction.Caption = caption;

                if (string.IsNullOrEmpty(name))
                    uxAction.Name = "ShowCustom" + cd.Name;
                else
                    uxAction.Name = name;

                uxAction.MappedToObject = cd;
                uxAction.Application = application;
            }
            else
                return null;

            // Get all actions to be able to change the name automatically if needed.
            IList<UXAction> allActions = ModelService.GetAllDomainObjectsByApplicationId<UXAction>(application.Id);

            if (allActions != null)
            {
                int i = 1;

                string testName = uxAction.Name.ToUpper();

                // Create a new unique name if it exists
                while (allActions.Count(action => action.Name.ToUpper() == testName) > 0)
                {
                    i++;
                    testName = uxAction.Name.ToUpper() + i.ToString();
                }

                // Set the new name
                if (i > 1)
                {
                    uxAction.Name = uxAction.Name + i.ToString();
                }
            }

            return uxAction;
        }

        [Transaction(ReadOnly = false)]
        private void CreateUXActionMap(UXAction action, IMappableUXObject uxObject)
        {

            action.RequestMap = new PropertyMap();

            IDomainObject targetObject = uxObject;

            if (uxObject is Dialog)
            {
                Dialog currentDialog = ModelService.GetDomainObject<Dialog>(uxObject.Id);

                if (currentDialog != null)
                {
                    // Check if the interfaceview is defined.
                    if (currentDialog.InterfaceView == null)
                    {
                        throw new Exception(string.Format("The dialog ({0}) {1} has no interfaceview defined!"
                                                          , currentDialog.Id.ToString()
                                                          , currentDialog.Name));
                    }

                    // Check if the interfaceviews servicemethod is defined.
                    if (currentDialog.InterfaceView.ServiceMethod == null)
                    {
                        throw new Exception(string.Format("The dialog ({0}) {1} has an interfaceview but there is no defined ServiceMethod for the view ({2}) {3}!"
                                                          , currentDialog.Id.ToString()
                                                          , currentDialog.Name
                                                          , currentDialog.InterfaceView.Id.ToString()
                                                          , currentDialog.InterfaceView.Name));
                    }
                }
                else
                {
                    throw new Exception(string.Format("The dialog that had the id {0} doesn't exist!"
                                                      , uxObject.Id));
                }

                targetObject = ((Dialog)uxObject).InterfaceView;
            }
            
            ModelService.CreateAndSynchronizePropertyMaps(targetObject, action);
        }
    }
}
