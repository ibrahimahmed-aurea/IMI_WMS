using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IModelService
    {
        event StatusChangedDelegate StatusChanged;
        event DomainObjectDeletedDelegate DomainObjectDeleted;
        event DomainObjectAddedDelegate DomainObjectAdded;
        event DomainObjectChangedDelegate DomainObjectChanged;

        IEnumerable<DataAccess.IVersionControlled> GetVersionControlledDomainObjectsForParent(Type domainObjectType, Guid parentId);
        IList<IVersionControlled> GetVersionControlledParent(IDomainObject domainObject, out List<IDomainObject> parents);
        Type GetDomainObjectType(IDomainObject domainObject);
        IList<T> GetAllDomainObjectsByApplicationId<T>(Guid ApplicationId);
        IList<IDomainObject> GetAllDomainObjectsByApplicationId(Guid ApplicationId, Type classType);
        IList<IVersionControlled> GetAllVersionControlledObjectsInApplication(Guid applicationId);
        IList<T> GetAllDomainObjectsByPropertyValues<T>(Dictionary<string, object> namedPropertyAndValue);
        IList<T> GetAllDomainObjectsByPropertyValues<T>(Dictionary<string, object> namedPropertyAndValue, bool initalize);
        IList<T> GetAllDomainObjectsByPropertyValues<T>(Dictionary<string, object> namedPropertyAndValue, bool initalize, bool useWildcards);
        IDomainObject GetDomainObject(Guid domainObjectId, Type classType);
        T GetDomainObject<T>(Guid domainObjectId);
        IDomainObject GetInitializedDomainObject(Guid domainObjectId, Type classType);
        T GetInitializedDomainObject<T>(Guid domainObjectId);
        DataAccess.Domain.Application GetApplicationForDomainObject(IDomainObject domainObject);
        IDomainObject SaveDomainObject(IDomainObject domainObject, bool newObj = false);
        IDomainObject MergeSaveDomainObject(IDomainObject domainObject);
        void DeleteDomainObject(IDomainObject domainObject);
        void DeleteDomainObjectAtCheckIn(IDomainObject domainObject);
        void DeleteDomainObjectWithoutChecksAndCheckOut(IDomainObject domainObject);
        bool? ClassTypeBelongToFrontend(Type classType);
        Type FindParentType(Type classType, bool versionControlled);
        void CreateAndSynchronizePropertyMaps(IDomainObject sourceObject, IDomainObject targetObject);
        void StartSynchronizePropertyMapsInObjects(IDomainObject domainObject, List<IDomainObject> objectsToSave = null, List<IDomainObject> objectsToDelete = null);
        void SynchronizePropertyMapChain(DataAccess.Domain.PropertyMap SourceMaps, List<IDomainObject> objectsToDelete);
        IDomainObject GetDynamicInitializedDomainObject(Guid domainObjectId, Type classType, List<string> namedPropertyWithDomainObjectToInitialize);
        T GetDynamicInitializedDomainObject<T>(Guid domainObjectId, List<string> namedPropertyWithDomainObjectToInitialize);
        IList<IDomainObject> GetReferencingObjects(IDomainObject domainObject);
        IList<Type> GetAllVersionControlledTypes();
        IList<T> GetDomainObjectsByQuery<T>(string query);
        IList<IDomainObject> GetDomainObjectsByQuery(string query);
        IVersionControlled MoveDomainObject(IVersionControlled objectToMove, IVersionControlled newParentObject);
        void GenerateFrontendCode(List<Cdc.MetaManager.DataAccess.Domain.Module> selectedModules, DataAccess.Domain.Application frontendApplication, string solutionFileNameAndPath, string referencePath);
        void GenerateApplication(List<string> frontendApplications, List<string> backendApplications, bool ignoreCheckOuts = false);
        void GenerateApplication(List<Cdc.MetaManager.DataAccess.Domain.Application> applications, bool ignoreCheckOuts = false, Dictionary<Guid, List<Guid>> SelectedModulesOrServicesPerApplication = null);
        IList<IVersionControlled> GetReferencedVersionControlled(IDomainObject domainObject);
    }
}
