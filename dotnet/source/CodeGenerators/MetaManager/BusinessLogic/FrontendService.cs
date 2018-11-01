using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;
using Spring.Context;
using Spring.Context.Support;
using Spring.Transaction.Interceptor;
using System.Reflection;

namespace Cdc.MetaManager.BusinessLogic
{
    public class FrontendService : IFrontendService
    {
        private IApplicationContext ctx = null;

        private void GetContext()
        {
            // Get application service context
            if (ctx == null)
            {
                ctx = ContextRegistry.GetContext();
            }
        }

        #region IFrontendService Members

        [Transaction(ReadOnly = false)]
        public IList<T> GetAllDomainObjectsByApplicationId<T>(Guid ApplicationId)
        {
            GetContext();

            if (typeof(T) == typeof(DataAccess.Domain.Application))
            {
                IList<DataAccess.Domain.Application> tmpapplist = new List<DataAccess.Domain.Application>();
                tmpapplist.Add(((DataAccess.Dao.IApplicationDao)ctx["ApplicationDao"]).FindById(ApplicationId));
                return ((IList<T>)((object)tmpapplist));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.CustomDialog))
            {
                return ((IList<T>)((object)((DataAccess.Dao.ICustomDialogDao)ctx["CustomDialogDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.DataSource))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IDataSourceDao)ctx["DataSourceDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Dialog))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IDialogDao)ctx["DialogDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Hint))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IHintDao)ctx["HintDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.HintCollection))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IHintCollectionDao)ctx["HintCollectionDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Issue))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IIssueDao)ctx["IssueDao"]).FindAllIssues(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.MappedProperty))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IMappedPropertyDao)ctx["MappedPropertyDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.Menu))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IMenuDao)ctx["MenuDao"]).FindAll(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.MenuItem))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IMenuItemDao)ctx["MenuItemDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.Module))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IModuleDao)ctx["ModuleDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Report))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IReportDao)ctx["ReportDao"]).FindAllReports(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.ReportQuery))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IReportQueryDao)ctx["ReportQueryDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.UXAction))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IUXActionDao)ctx["UXActionDao"]).FindAll(ApplicationId)));
            }
            else if (typeof(T) == typeof(DataAccess.Domain.UXSession))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IUXSessionDao)ctx["UXSessionDao"]).FindByApplicationId(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.ViewAction))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IViewActionDao)ctx["ViewActionDao"]).FindAll(ApplicationId)));
            //}
            //else if (typeof(T) == typeof(DataAccess.Domain.ViewComponent))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IViewComponentDao)ctx["ViewComponentDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.View))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IViewDao)ctx["ViewDao"]).FindAll(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.ViewNode))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IViewNodeDao)ctx["ViewNodeDao"]).FindAll(ApplicationId)));
            //}
            else if (typeof(T) == typeof(DataAccess.Domain.Workflow))
            {
                return ((IList<T>)((object)((DataAccess.Dao.IWorkflowDao)ctx["WorkflowDao"]).FindAll(ApplicationId)));
            }
            //else if (typeof(T) == typeof(DataAccess.Domain.WorkflowDialog))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IWorkflowDialogDao)ctx["WorkflowDialogDao"]).FindAll(ApplicationId)));
            //}
            //else if (typeof(T) == typeof(DataAccess.Domain.WorkflowSubworkflow))
            //{
            //    return ((IList<T>) ((object) ((DataAccess.Dao.IWorkflowSubworkflowDao)ctx["WorkflowSubworkflowDao"]).FindAll(ApplicationId)));
            //}
            return null;
        }

        [Transaction(ReadOnly = false)]
        public T GetInitializedDomainObject<T>(Guid domainObjectId)
        {
            object theObject = null;
            GetContext();

            if (typeof(T) == typeof(DataAccess.Domain.Application))
            {
                theObject = (object)((DataAccess.Dao.IApplicationDao)ctx["ApplicationDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.CustomDialog))
            {
                theObject = (object) ((DataAccess.Dao.ICustomDialogDao)ctx["CustomDialogDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.DataSource))
            {
                theObject = (object) ((DataAccess.Dao.IDataSourceDao)ctx["DataSourceDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Dialog))
            {
                theObject = (object) ((DataAccess.Dao.IDialogDao)ctx["DialogDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Hint))
            {
                theObject = (object) ((DataAccess.Dao.IHintDao)ctx["HintDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.HintCollection))
            {
                theObject = (object) ((DataAccess.Dao.IHintCollectionDao)ctx["HintCollectionDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Issue))
            {
                theObject = (object) ((DataAccess.Dao.IIssueDao)ctx["IssueDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.MappedProperty))
            {
                theObject = (object) ((DataAccess.Dao.IMappedPropertyDao)ctx["MappedPropertyDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Menu))
            {
                theObject = (object) ((DataAccess.Dao.IMenuDao)ctx["MenuDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.MenuItem))
            {
                theObject = (object) ((DataAccess.Dao.IMenuItemDao)ctx["MenuItemDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Module))
            {
                theObject = (object) ((DataAccess.Dao.IModuleDao)ctx["ModuleDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Report))
            {
                theObject = (object) ((DataAccess.Dao.IReportDao)ctx["ReportDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.ReportQuery))
            {
                theObject = (object) ((DataAccess.Dao.IReportQueryDao)ctx["ReportQueryDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.UXAction))
            {
                theObject = (object) ((DataAccess.Dao.IUXActionDao)ctx["UXActionDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.UXSession))
            {
                theObject = (object)((DataAccess.Dao.IUXSessionDao)ctx["UXSessionDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.ViewAction))
            {
                theObject = (object) ((DataAccess.Dao.IViewActionDao)ctx["ViewActionDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.ViewComponent))
            {
                theObject = (object) ((DataAccess.Dao.IViewComponentDao)ctx["ViewComponentDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.View))
            {
                theObject = (object) ((DataAccess.Dao.IViewDao)ctx["ViewDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.ViewNode))
            {
                theObject = (object) ((DataAccess.Dao.IViewNodeDao)ctx["ViewNodeDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.Workflow))
            {
                theObject = (object) ((DataAccess.Dao.IWorkflowDao)ctx["WorkflowDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.WorkflowDialog))
            {
                theObject = (object) ((DataAccess.Dao.IWorkflowDialogDao)ctx["WorkflowDialogDao"]).FindById(domainObjectId);
            }
            else if (typeof(T) == typeof(DataAccess.Domain.WorkflowSubworkflow))
            {
                theObject = (object) ((DataAccess.Dao.IWorkflowSubworkflowDao)ctx["WorkflowSubworkflowDao"]).FindById(domainObjectId);
            }

            if (theObject != null)
            {
                foreach (PropertyInfo pi in theObject.GetType().GetProperties())
                {
                    object proxy = pi.GetGetMethod().Invoke(theObject, new object[0]);
                    if (proxy != null)
                    {
                        Type[] interfaces = proxy.GetType().GetInterfaces();
                        foreach (Type iface in interfaces)
                        {
                            if (iface == typeof(NHibernate.Proxy.INHibernateProxy) ||
                                iface == typeof(NHibernate.Collection.IPersistentCollection))
                            {
                                if (!NHibernate.NHibernateUtil.IsInitialized(proxy))
                                {
                                    NHibernate.NHibernateUtil.Initialize(proxy);
                                }

                                break;
                            }
                        }

                    }
                }
            }

            return (T)theObject;
        }

        #endregion
    }
}
