using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;

namespace Cdc.MetaManager.BusinessLogic
{
    //public class PropertyDescriptor
    //{
    //    public PropertyDescriptor()
    //    {
    //        UpstreamProperties = new List<PropertyDescriptor>();
    //    }

    //    public IList<PropertyDescriptor> UpstreamProperties { get; set; }
    //    public PropertyDescriptor Source { get; set; }
    //    public MappedProperty Property { get; set; }
    //    public object Owner { get; set; }
    //}

    //public class ComponentReference
    //{
    //    public View View { get; set; }
    //    public UXComponent Component { get; set; }

    //    public override string ToString()
    //    {
    //        return string.Format("{0}.{1}", View.Name, Component.Name);
    //    }
    //}
        
    public interface IDialogService
    {
        //================DialogObjectViewer====================
        IList<ViewNode> GetViewNodesByViewId(Guid viewId);
        void SetDialogInterface(Dialog dialog, View view);
        void UnmapViewComponents(View readView);
        void GetViewNodeMap(ViewNode viewNode, out PropertyMap viewNodeMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        void SaveOrUpdateViewNodeMap(ViewNode viewNode, PropertyMap viewNodeMap);
        void GetViewToActionMap(ViewAction viewAction, out PropertyMap viewToActionMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        void MoveUp(Guid moverActionId, Guid staticActionId, out ViewAction moverAction, out ViewAction staticAction);
        void MoveUp(Guid moverViewNodeId, Guid staticViewNodeId, out ViewNode moverViewNode, out ViewNode staticViewNode);
        Dialog CreateOrUpdateSearchPanelView(Dialog dialog);
        ViewNode GetParentViewNode(ViewNode viewNode);
                
        //=================XamlViewer====================
        DataSource GetDataSourceById(Guid dataSourceId);
        void GetServiceComponentMap(View view, UXServiceComponent serviceComponent, out PropertyMap componentMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        void GetDataSourceToViewRequestMap(DataSource dataSource, View view, out PropertyMap dataSourceToViewRequestMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        void GetDataSourceToViewResponseMap(DataSource dataSource, View view, out PropertyMap dataSourceToViewResponseMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        DataSource SaveOrUpdateDataSourceMaps(DataSource dataSource, PropertyMap requestMap, PropertyMap responseMap);
        void DeleteUXComponent(UXComponent component);

        //===============================================

        Dialog GetDialogWithViewTree(Guid dialogId);
        IList<UXSessionProperty> GetUXSessionProperties(Application application);
        Dialog GetDialog(Guid dialogId);
        View GetViewById(Guid viewId);
        ViewNode GetViewNodeById(Guid viewNodeId);
        ViewNode SaveViewNode(ViewNode view);
        IList<Dialog> GetAllDialogsWithInterfaceView(Guid applicationId, DialogType? dialogType);
        IList<CustomDialog> GetAllCustomDialogs(Guid applicationId);
        IList<Module> GetAllModules(Guid applicationId);
        void CreateViewServiceMethodMap(View view, Guid serviceMethodId);
        IList<DbProperty> GetDbProperties(ServiceMethod serviceMethod);
        void GetViewRequestMap(View view, out PropertyMap requestMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties);
        void GetViewResponseMap(View view, out PropertyMap responseMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> requestProperties, out IList<IMappableProperty> targetProperties);
        ViewNode ConnectViewToViewNode(View view, ViewNode parentViewNode);
        IList<string> FindAllUniqueCustomDLLNames(Guid applicationId);
        void SaveAndDeleteMappedPropertiesInMap(PropertyMap propertyMap, IList<MappedProperty> deletedList);
        IList<View> GetViews(string entityName, string viewName, string title, FindViewTypes findViewTypes, Guid applicationId);
        DataSource CreateDataSourceMaps(DataSource dataSource, View view, Guid serviceMethodId);
        Workflow GetWorkflowById(Guid workflowId);
        Module GetModuleById(Guid moduleId);
        void UpdateWorkflows(Cdc.MetaManager.DataAccess.Domain.Module module);
        void UpdateWorkflow(Workflow workflow);
        IList<View> GetViewsByNameAndApplicationId(string viewName, Guid applicationId);
        IList<Dialog> FindDialogsByNameAndModule(Guid applicationId, string moduleName, string dialogName);
        long CountViewNodes(View view);
    }
}
