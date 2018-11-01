using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Cdc.MetaManager.BusinessLogic;
using Spring.Context.Support;

namespace Cdc.MetaManager.BusinessLogic
{
    public static class MetaManagerServices
    {
        public static void GetContext()
        {
            if (ctx == null)
            {
                ctx = ContextRegistry.GetContext();
            }
        }

        public static void Dispose()
        {
            if (ctx != null)
            {
                ctx.Dispose();
                ctx = null;
            }
        }

        public static NHibernate.ISession GetCurrentSession()
        {
            GetContext();

            if (ctx != null)
            {
                return Spring.Data.NHibernate.SessionFactoryUtils.GetSession(GetSessionFactory(), false);
            }

            return null;
        }

        public static NHibernate.ISessionFactory GetSessionFactory()
        {
            GetContext();

            if (ctx != null)
            {
                return ((NHibernate.ISessionFactory)ctx["SessionFactory"]);
            }

            return null;
        }

        public static NHibernate.IInterceptor GetDomainInterceptor()
        {
            GetContext();

            if (ctx != null)
            {
                return ((NHibernate.IInterceptor)ctx["DomainInterceptor"]);
            }

            return null;
        }

        private static IApplicationContext ctx = null;

        public static IConfigurationManagementService GetConfigurationManagementService()
        {
            GetContext();
            
            if (ctx != null)
            {
                return ctx["ConfigurationManagementService"] as IConfigurationManagementService;
            }
            return null;
        }

        public static IModelService GetModelService()
        {
            GetContext();

            if (ctx != null)
            {
                return ctx["ModelService"] as IModelService;
            }
            return null;
        }

        public static IModelChangeNotificationService GetModelChangeNotificationService()
        {
            GetContext();

            if (ctx != null)
            {
                return ctx["ModelChangeNotificationService"] as IModelChangeNotificationService;
            }
            return null;
        }

        public static IApplicationService GetApplicationService()
        {
            GetContext();

            if (ctx != null)
            {
                return ctx["ApplicationService"] as IApplicationService;
            }
            return null;
        }

        public static IDialogService GetDialogService()
        {
            GetContext();

            if (ctx != null)
            {
                return ctx["DialogService"] as IDialogService;
            }
            return null;
        }

        public static IMenuService GetMenuService()
        {
            GetContext();

            if (ctx != null)
            {
                return ctx["MenuService"] as IMenuService;
            }
            return null;
        }

        public static IUXActionService GetUXActionService()
        {
            GetContext();

            if (ctx != null)
            {
                return ctx["UXActionService"] as IUXActionService;
            }
            return null;
        }

        public static IAnalyzeService GetAnalyzeService()
        {
            GetContext();

            if (ctx != null)
            {
                return ctx["AnalyzeService"] as IAnalyzeService;
            }
            return null;
        }
                
        public static DataAccess.Domain.UXSession GetUXSessionByApplicationId(Guid applicationId)
        {
            GetContext();
            if (ctx != null)
            {
                return ((DataAccess.Dao.IUXSessionDao)ctx["UXSessionDao"]).FindByApplicationId(applicationId);
            }

            return null;
        }

        public static class Helpers
        {
            public static BusinessLogic.Helpers.IActionHelper ActionHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IActionHelper)ctx["ActionHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IApplicationHelper ApplicationHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IApplicationHelper)ctx["ApplicationHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IBusinessEntityHelper BusinessEntityHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IBusinessEntityHelper)ctx["BusinessEntityHelper"]; } return null; } }
            public static BusinessLogic.Helpers.CustomDialogHelper CustomDialogHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.CustomDialogHelper)ctx["CustomDialogHelper"]; } return null; } }
            public static BusinessLogic.Helpers.DataSourceHelper DataSourceHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.DataSourceHelper)ctx["DataSourceHelper"]; } return null; } }
            public static BusinessLogic.Helpers.DialogHelper DialogHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.DialogHelper)ctx["DialogHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IHintCollectionHelper HintCollectionHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IHintCollectionHelper)ctx["HintCollectionHelper"]; } return null; } }
            public static BusinessLogic.Helpers.HintHelper HintHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.HintHelper)ctx["HintHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IssueHelper IssueHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IssueHelper)ctx["IssueHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IMappedPropertyHelper MappedPropertyHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IMappedPropertyHelper)ctx["MappedPropertyHelper"]; } return null; } }
            public static BusinessLogic.Helpers.MenuHelper MenuHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.MenuHelper)ctx["MenuHelper"]; } return null; } }
            public static BusinessLogic.Helpers.MenuItemHelper MenuItemHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.MenuItemHelper)ctx["MenuItemHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IModuleHelper ModuleHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IModuleHelper)ctx["ModuleHelper"]; } return null; } }
            public static BusinessLogic.Helpers.PackageHelper PackageHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.PackageHelper)ctx["PackageHelper"]; } return null; } }
            public static BusinessLogic.Helpers.ProcedurePropertyHelper ProcedurePropertyHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.ProcedurePropertyHelper)ctx["ProcedurePropertyHelper"]; } return null; } }
            public static BusinessLogic.Helpers.PropertyCaptionHelper PropertyCaptionHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.PropertyCaptionHelper)ctx["PropertyCaptionHelper"]; } return null; } }
            public static BusinessLogic.Helpers.PropertyCodeHelper PropertyCodeHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.PropertyCodeHelper)ctx["PropertyCodeHelper"]; } return null; } }
            public static BusinessLogic.Helpers.PropertyHelper PropertyHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.PropertyHelper)ctx["PropertyHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IPropertyMapHelper PropertyMapHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IPropertyMapHelper)ctx["PropertyMapHelper"]; } return null; } }
            public static BusinessLogic.Helpers.PropertyStorageInfoHelper PropertyStorageInfoHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.PropertyStorageInfoHelper)ctx["PropertyStorageInfoHelper"]; } return null; } }
            public static BusinessLogic.Helpers.QueryPropertyHelper QueryPropertyHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.QueryPropertyHelper)ctx["QueryPropertyHelper"]; } return null; } }
            public static BusinessLogic.Helpers.QueryHelper QueryHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.QueryHelper)ctx["QueryHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IReportHelper ReportHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IReportHelper)ctx["ReportHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IReportQueryHelper ReportQueryHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IReportQueryHelper)ctx["ReportQueryHelper"]; } return null; } }
            public static BusinessLogic.Helpers.SchemaHelper SchemaHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.SchemaHelper)ctx["SchemaHelper"]; } return null; } }
            public static BusinessLogic.Helpers.ServiceHelper ServiceHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.ServiceHelper)ctx["ServiceHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IServiceMethodHelper ServiceMethodHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IServiceMethodHelper)ctx["ServiceMethodHelper"]; } return null; } }
            public static BusinessLogic.Helpers.StoredProcedureHelper StoredProcedureHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.StoredProcedureHelper)ctx["StoredProcedureHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IUXActionHelper UXActionHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IUXActionHelper)ctx["UXActionHelper"]; } return null; } }
            public static BusinessLogic.Helpers.UXSessionHelper UXSessionHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.UXSessionHelper)ctx["UXSessionHelper"]; } return null; } }
            public static BusinessLogic.Helpers.UXSessionPropertyHelper UXSessionPropertyHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.UXSessionPropertyHelper)ctx["UXSessionPropertyHelper"]; } return null; } }
            public static BusinessLogic.Helpers.ViewActionHelper ViewActionHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.ViewActionHelper)ctx["ViewActionHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IViewHelper ViewHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IViewHelper)ctx["ViewHelper"]; } return null; } }
            public static BusinessLogic.Helpers.ViewNodeHelper ViewNodeHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.ViewNodeHelper)ctx["ViewNodeHelper"]; } return null; } }
            public static BusinessLogic.Helpers.WorkflowDialogHelper WorkflowDialogHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.WorkflowDialogHelper)ctx["WorkflowDialogHelper"]; } return null; } }
            public static BusinessLogic.Helpers.WorkflowHelper WorkflowHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.WorkflowHelper)ctx["WorkflowHelper"]; } return null; } }
            public static BusinessLogic.Helpers.WorkflowServiceMethodHelper WorkflowServiceMethodHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.WorkflowServiceMethodHelper)ctx["WorkflowServiceMethodHelper"]; } return null; } }
            public static BusinessLogic.Helpers.WorkflowSubworkflowHelper WorkflowSubworkflowHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.WorkflowSubworkflowHelper)ctx["WorkflowSubworkflowHelper"]; } return null; } }
            public static BusinessLogic.Helpers.IImportChangeHelper ImportChangeHelper { get { GetContext(); if (ctx != null) { return (BusinessLogic.Helpers.IImportChangeHelper)ctx["ImportChangeHelper"]; } return null; } }
        }
    }
}
