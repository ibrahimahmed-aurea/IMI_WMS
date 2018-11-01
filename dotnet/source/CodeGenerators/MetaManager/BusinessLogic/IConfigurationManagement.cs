using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic
{
    public delegate void StatusChangedDelegate(string message, int value, int min, int max);

    public interface IConfigurationManagementService
    {
        event StatusChangedDelegate StatusChanged;
        event DomainObjectChangedDelegate ObjectChanged;
        event DomainObjectAddedDelegate ObjectAdded;
        void CheckOutDomainObject(Guid id, Type classType);
        void CheckInDomainObject(Guid id, Type classType, DataAccess.Domain.Application application, bool doCheckIn = true, bool doUnlock = true);
        void CheckInDomainObjects(IList<IVersionControlled> domainObjects);
        void RemoveDomainObject(Guid id, Type classType, DataAccess.Domain.Application application);
        void UndoCheckOutDomainObject(Guid id, Type classType, DataAccess.Domain.Application application);
        void ImportDomainObjects(List<string> paths, bool checkout, bool excludeZeroSizeFiles);
        void ImportDomainObject(string FileName);
        object GetDomainObjectFromConfMgn(Guid id, Type classType, DataAccess.Domain.Application application, bool dontsave = false);
        object GetDomainObjectFromConfMgn(string path, Type classType, bool dontsave = false);
        object GetDomainObjectFromConfMgn(Type classType, string fileContent, bool dontsave = false);
        object GetSpecificVersionOfDomainObjectFromConfMgn(string path, string version, string repositoryPathForImport);
        void GetDomainFromConfMgn(string application);
        void AddDomainToConfMgn(DataAccess.Domain.DeploymentGroup deploymentGroup, bool doCheckIn, bool frontend, bool backend);
        Dictionary<Guid, IDomainObject> GetLatestVersionOfObjects(List<IDomainObject> objects, DataAccess.Domain.Application application, bool getAllFromCM = false, bool onlyResolveChildren = false, bool statusEvent = true);
        KeyValuePair<Type, Guid> GetClassTypeAndIdForXML(string xmlfilename);
        void DiffWithPreviousVersion(IVersionControlled versionControledObject, DataAccess.Domain.Application application);
        string GetCurrentBranch();
        string GetBranch(string repositoryPath);
    }
}
