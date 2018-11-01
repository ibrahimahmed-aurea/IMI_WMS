using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IRepositoryService
    {
        bool CheckOutFile(DataAccess.IVersionControlled domainObject, DataAccess.Domain.Application application, out string errorMessage);
        bool CheckInFile(DataAccess.IVersionControlled domainObject, DataAccess.Domain.Application application, out string errorMessage);
        bool RemoveFile(DataAccess.IVersionControlled domainObject, DataAccess.Domain.Application application, out string errorMessage);
        void DiffFiles(string baseFilePath, string currentFilePath);
        void BeginCheckIn(IList<DataAccess.IVersionControlled> domainObjects);
        IList<DataAccess.IVersionControlled> CommitCheckIn();
        void RollbackCheckIn();
        bool IsAtomicCheckInSupported { get; }
        string GetSpecificVersionOfFile(string filePath, string version, string repositoryPathForImport);
        string GetBranchName(string path);
    }
}
