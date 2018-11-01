using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IAnalyzeService
    {
        AnalyzeIssueTree Check(Guid backendApplicationId,
                               Guid frontendApplicationId,
                               bool checkStoredProcs,
                               string specDirectory,
                               bool checkSQL,
                               string connectionString,
                               bool checkMaps,
                               bool checkDialogs,
                               CallbackDelegate callback);

        AnalyzeIssueTree CheckSingleQuery(Guid backendApplicationId, Guid queryId);
        AnalyzeIssueTree CheckSingleServiceMethod(Guid applicationId, Guid serviceMethodId);
        AnalyzeIssueTree CheckSingleStoredProcedure(Guid backendApplicationId, Guid storedProcedureId, string specDirectory);
        AnalyzeIssueTree CheckSingleDialog(Guid applicationId, Guid dialogId);
        Issue FindIssue(Guid applicationId, IssueObjectType objectType, Guid objectId, string title, string text);
        IList<Issue> FindAllIssues(Guid applicationId);
        IList<Issue> FindAllIssuesByType(Guid applicationId, IssueObjectType type);
        IList<Issue> FindAllIssuesByTypeAndObjectId(Guid applicationId, IssueObjectType type, Guid objectId);
        void DeleteIssues(Guid applicationId, IssueObjectType objectType, Guid objectId);
        Issue SaveOrUpdateIssue(Issue issue);
    }
}
