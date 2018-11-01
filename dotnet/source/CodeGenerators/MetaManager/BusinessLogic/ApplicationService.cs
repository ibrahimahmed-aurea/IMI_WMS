using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using System.Collections;
using NHibernate;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.Framework.ExtensionMethods;

namespace Cdc.MetaManager.BusinessLogic
{
    public class ApplicationService : Cdc.MetaManager.BusinessLogic.IApplicationService
    {
        public IApplicationDao ApplicationDao { get; set; }
        public ISchemaDao SchemaDao { get; set; }
        public IPackageDao PackageDao { get; set; }
        public IActionDao ActionDao { get; set; }
        public IServiceDao ServiceDao { get; set; }
        public IUXSessionDao UXSessionDao { get; set; }
        public IMappedPropertyDao MappedPropertyDao { get; set; }
        public IPropertyMapDao PropertyMapDao { get; set; }
        public IServiceMethodDao ServiceMethodDao { get; set; }
        public IQueryDao QueryDao { get; set; }
        public IQueryPropertyDao QueryPropertyDao { get; set; }
        public IStoredProcedureDao StoredProcedureDao { get; set; }
        public IStoredProcedurePropertyDao StoredProcedurePropertyDao { get; set; }
        public IDeploymentGroupDao DeploymentGroupDao { get; set; }
        public IPropertyDao PropertyDao { get; set; }
        public IBusinessEntityDao BusinessEntityDao { get; set; }
        public IReportQueryDao ReportQueryDao { get; set; }
        public IHintDao HintDao { get; set; }
        public IViewDao ViewDao { get; set; }
        public IHintCollectionDao HintCollectionDao { get; set; }

        [Transaction(ReadOnly = true)]
        public IList<UXSessionProperty> GetUXSessionProperties(Application application)
        {
            IList<UXSessionProperty> properties = UXSessionDao.FindByApplicationId(application.Id).Properties;

            foreach (UXSessionProperty property in properties)
            {
                NHibernateUtil.Initialize(property);
            }

            return properties;
        }

        

        [Transaction(ReadOnly = true)]
        public void GetServiceMethodRequestMap(ServiceMethod serviceMethod, out PropertyMap requestMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            ServiceMethod readServiceMethod = GetServiceMethodMapsById(serviceMethod.Id);

            if (readServiceMethod != null && 
                readServiceMethod.RequestMap != null)
            {
                sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readServiceMethod.RequestMap).MappedProperties.Cast<IMappableProperty>());
            }
            else
            {
                sourceProperties = new List<IMappableProperty>();
            }

            if (readServiceMethod != null &&
                readServiceMethod.MappedToAction != null && 
                readServiceMethod.MappedToAction.RequestMap != null)
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readServiceMethod.MappedToAction.RequestMap).MappedProperties.Cast<IMappableProperty>());
            else
                targetProperties = new List<IMappableProperty>();

            if (readServiceMethod != null)
            {
                requestMap = readServiceMethod.RequestMap;

                MetaManagerUtil.InitializePropertyMap(requestMap);
            }
            else
            {
                requestMap = null;
            }
        }
        
        [Transaction(ReadOnly = true)]
        public void GetServiceMethodResponseMap(ServiceMethod serviceMethod, out PropertyMap responseMap, out IList<IMappableProperty> sourceProperties, out IList<IMappableProperty> targetProperties)
        {
            ServiceMethod readServiceMethod = GetServiceMethodMapsById(serviceMethod.Id);

            if (readServiceMethod != null &&
                readServiceMethod.RequestMap != null)
            {
                sourceProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readServiceMethod.ResponseMap).MappedProperties.Cast<IMappableProperty>());
            }
            else
            {
                sourceProperties = new List<IMappableProperty>();
            }

            if (readServiceMethod != null &&
                readServiceMethod.MappedToAction != null && 
                readServiceMethod.MappedToAction.RequestMap != null)
                targetProperties = new List<IMappableProperty>(MetaManagerUtil.InitializePropertyMap(readServiceMethod.MappedToAction.ResponseMap).MappedProperties.Cast<IMappableProperty>());
            else
                targetProperties = new List<IMappableProperty>();

            if (readServiceMethod != null)
            {
                responseMap = readServiceMethod.ResponseMap;

                MetaManagerUtil.InitializePropertyMap(responseMap);
            }
            else
            {
                responseMap = null;
            }
        }

     
        [Transaction(ReadOnly = true)]
        public ServiceMethod GetServiceMethodWithRequestMap(Guid serviceMethodId)
        {
            ServiceMethod serviceMethod = ServiceMethodDao.FindById(serviceMethodId);

            if (serviceMethod != null)
            {
                // Initialize Requestmap
                NHibernateUtil.Initialize(serviceMethod.RequestMap);
            }

            return serviceMethod;
        }


        [Transaction(ReadOnly = true)]
        public IList<ServiceMethod> GetAllServiceMethodsToQueriesByApplication(Guid applicationId)
        {
            IList<ServiceMethod> serviceMethods = ServiceMethodDao.FindAllQueriesByApplicationId(applicationId);
            IList<ServiceMethod> serviceMethodsRefCurProcs = ServiceMethodDao.FindAllRefCursorProcsByApplicationId(applicationId);

            foreach (ServiceMethod servMeth in serviceMethodsRefCurProcs)
                serviceMethods.Add(servMeth);

            foreach (ServiceMethod serviceMethod in serviceMethods)
            {
                NHibernateUtil.Initialize(serviceMethod.Service);

                if (serviceMethod.MappedToAction.MappedToObject.ObjectType == ActionMapTarget.Query)
                {
                    NHibernateUtil.Initialize(serviceMethod.MappedToAction.Query);
                }
                else if (serviceMethod.MappedToAction.MappedToObject.ObjectType == ActionMapTarget.StoredProcedure)
                {
                    NHibernateUtil.Initialize(serviceMethod.MappedToAction.StoredProcedure);
                }
            }

            return serviceMethods;
        }

        [Transaction(ReadOnly = true)]
        public ServiceMethod GetServiceMethodMapsById(Guid serviceMethodId)
        {
            ServiceMethod serviceMethod = ServiceMethodDao.FindById(serviceMethodId);

            if (serviceMethod != null)
            {
                NHibernateUtil.Initialize(serviceMethod.Service);
                NHibernateUtil.Initialize(serviceMethod.RequestMap.MappedProperties);
                NHibernateUtil.Initialize(serviceMethod.ResponseMap.MappedProperties);
                NHibernateUtil.Initialize(serviceMethod.MappedToAction);
            }

            return serviceMethod;
        }

        [Transaction(ReadOnly = false)]
        public Schema GetSchemaByApplicationId(Guid applicationId)
        {
            return SchemaDao.FindByApplicationId(applicationId);
        }

        [Transaction(ReadOnly = false)]
        public Query GetQueryByIdWithProperties(Guid queryId)
        {
            Query query = QueryDao.FindById(queryId);

            if (query != null)
            {
                NHibernateUtil.Initialize(query.Properties);
                NHibernateUtil.Initialize(query.Schema);
            }

            return query;
        }
                        
        [Transaction(ReadOnly = false)]
        public PropertyMap SaveAndMergePropertyMap(PropertyMap map)
        {
            return PropertyMapDao.SaveOrUpdateMerge(map);
        }

        [Transaction(ReadOnly = true)]
        public IList<DeploymentGroup> GetAllDeploymentGroups()
        {
            IList<DeploymentGroup> deploymentGroups = DeploymentGroupDao.FindAll();

            foreach (DeploymentGroup deploymentGroup in deploymentGroups)
            {
                NHibernateUtil.Initialize( deploymentGroup);
                                 
                if (deploymentGroup.BackendApplication != null)
                    NHibernateUtil.Initialize(deploymentGroup.BackendApplication);

                if (deploymentGroup.FrontendApplication != null)
                    NHibernateUtil.Initialize(deploymentGroup.FrontendApplication);
            }

            return deploymentGroups;
        }

        [Transaction(ReadOnly = true)]
        public IList<string> FindAllDisplayFormatsUsed(Type displayFormatDataType)
        {
            return MappedPropertyDao.FindAllDisplayFormatsUsed(displayFormatDataType);
        }

        [Transaction(ReadOnly = true)]
        public Property GetPropertyByTableAndColumn(string tableName, string columnName, Guid applicationId)
        {
            return PropertyDao.FindAllByTableAndColumn(tableName, columnName, applicationId).FirstOrDefault();
        }
        
        public bool MMDbExsisting()
        {
            try
            {
                DeploymentGroupDao.FindAll();
            }
            catch (Spring.Data.BadSqlGrammarException e)
            {
                return false;
            }
            return true;
        }
    }
}

