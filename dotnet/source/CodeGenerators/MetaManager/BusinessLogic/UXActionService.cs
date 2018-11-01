using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic
{
    public class UXActionService : IUXActionService
    {
        public IUXActionDao UXActionDao { get; set; }
        public IViewActionDao ViewActionDao { get; set; }
        public IPropertyMapDao PropertyMapDao { get; set; }
        public IMappedPropertyDao MappedPropertyDao { get; set; }
        public IDialogDao DialogDao { get; set; }
        public IServiceMethodDao ServiceMethodDao { get; set; }
        public IViewNodeDao ViewNodeDao { get; set; }
        public IApplicationService ApplicationService { get; set; }
        public ICustomDialogDao CustomDialogDao { get; set; }
        public IWorkflowDao WorkflowDao { get; set; }


        [Transaction(ReadOnly = true)]
        public UXAction GetUXActionByIdWithMap(Guid uxActionId)
        {
            UXAction action = UXActionDao.FindById(uxActionId);

            if (action.ServiceMethod != null)
            {
                NHibernateUtil.Initialize(action.ServiceMethod.MappedToAction);

                if (action.ServiceMethod.MappedToAction != null)
                    NHibernateUtil.Initialize(action.ServiceMethod.MappedToAction.MappedToObject);
            }

            if (action.RequestMap != null)
            {
                MetaManagerUtil.InitializePropertyMap(action.RequestMap);
            }

            if ((action.Dialog != null) && (action.Dialog.InterfaceView != null))
            {
                if (action.Dialog.InterfaceView.ServiceMethod != null)
                {
                    NHibernateUtil.Initialize(action.Dialog.InterfaceView.ServiceMethod);
                    NHibernateUtil.Initialize(action.Dialog.InterfaceView.ServiceMethod.MappedToAction);
                }
            }

            return action;
        }
        
        [Transaction(ReadOnly = false)]
        public ViewAction AddToView(UXAction action, ViewNode viewNode, ViewActionType viewActionType, MappedProperty mappedProperty)
        {
            ViewNode readViewNode = ViewNodeDao.FindById(viewNode.Id);
            UXAction readAction = UXActionDao.FindById(action.Id);
            
            NHibernateUtil.Initialize(readAction);
            NHibernateUtil.Initialize(readAction.Dialog);
            NHibernateUtil.Initialize(readAction.ServiceMethod);
            NHibernateUtil.Initialize(readAction.CustomDialog);
            NHibernateUtil.Initialize(readViewNode.View);

            IList<ViewAction> viewActions = ViewActionDao.FindAllByViewNodeId(viewNode.Id);

            int nextSequence = 1;

            if (viewActions != null && viewActions.Count > 0)
            {
                // Get the maxsequence
                nextSequence = viewActions.Max(vAction => vAction.Sequence);

                // Add one so we get a unique sequence.
                nextSequence++;
            }

            // Create the new Viewaction
            ViewAction viewAction = new ViewAction();
            viewAction.ViewNode = readViewNode;
            viewAction.Action = readAction;
            viewAction.Sequence = nextSequence;
            viewAction.Type = viewActionType;
            viewAction.DrilldownFieldMappedProperty = ((viewActionType == ViewActionType.Drilldown) || (viewActionType == ViewActionType.JumpTo)) ? mappedProperty : null;
            
            viewAction = ViewActionDao.Save(viewAction);

            NHibernateUtil.Initialize(viewAction.ViewNode);
            NHibernateUtil.Initialize(viewAction.ViewNode.ViewActions);
            NHibernateUtil.Initialize(viewAction.Action);
            NHibernateUtil.Initialize(viewAction.DrilldownFieldMappedProperty);

            return viewAction;
        } 



        [Transaction(ReadOnly = false)]
        public IList<UXAction> GetUXActionForMappableObject(IMappableUXObject uxObject)
        {
            IList<UXAction> actionList = null;

            if (uxObject is Dialog)
            {
                actionList = UXActionDao.FindAllByDialogId(uxObject.Id);
            }
            else if (uxObject is CustomDialog)
            {
                actionList = UXActionDao.FindAllByCustomDialogId(uxObject.Id);
            }
            else if (uxObject is Workflow)
            {
                actionList = UXActionDao.FindAllByWorkflowId(uxObject.Id);
            }
            else if (uxObject is ServiceMethod)
            {
                actionList = UXActionDao.FindAllByServiceMethodId(uxObject.Id);
            }

            if ((actionList != null) && (actionList.Count > 0))
            {
                foreach (UXAction action in actionList)
                {
                    if (action.MappedToObject is Dialog)
                        NHibernateUtil.Initialize(action.Dialog);

                    if (action.MappedToObject is CustomDialog)
                        NHibernateUtil.Initialize(action.CustomDialog);

                    if (action.MappedToObject is ServiceMethod)
                        NHibernateUtil.Initialize(action.ServiceMethod);
                }

                return actionList;
            }

            return null;
        }

    }
}
