using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.DataAccess.Dao
{
    public interface IIssueDao
    {
        Issue FindById(Guid issueId);
        IList<Issue> FindAllIssues(Guid applicationId);
        IList<Issue> FindAllIssuesByType(Guid applicationId, IssueObjectType type);
        IList<Issue> FindAllIssuesByTypeAndObjectId(Guid applicationId, IssueObjectType type, Guid objectId);
        Issue SaveOrUpdate(Issue issue);
        void Delete(Issue issue);
        Issue FindIssue(Guid applicationId, IssueObjectType type, Guid objectId, string title, string text);
        void DeleteIssues(Guid applicationId, IssueObjectType type, Guid objectId);
        void DeleteIssues(Guid applicationId, IssueObjectType type, Guid objectId, IList<Issue> dontDeleteIssueList);
    }
}
