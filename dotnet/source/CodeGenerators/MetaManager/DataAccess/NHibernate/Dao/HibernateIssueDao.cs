using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Domain;


namespace Cdc.MetaManager.DataAccess.NHibernate.Dao
{
    public class HibernateIssueDao : HibernateDaoSupport, IIssueDao
    {
        #region IIssueDao Members

        [Transaction(ReadOnly = true)]
        public Issue FindById(Guid issueId)
        {
            return HibernateTemplate.Get<Issue>(issueId);
        }

        [Transaction(ReadOnly = true)]
        public IList<Issue> FindAllIssues(Guid applicationId)
        {
            string[] paramNames = { "applicationId" };
            object[] paramValues = { applicationId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Issue>("Issue.FindAllIssues", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Issue> FindAllIssuesByType(Guid applicationId, IssueObjectType type)
        {
            string[] paramNames = { "applicationId", "objectType" };
            object[] paramValues = { applicationId, (int)type };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Issue>("Issue.FindAllIssuesByType", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public IList<Issue> FindAllIssuesByTypeAndObjectId(Guid applicationId, IssueObjectType type, Guid objectId)
        {
            string[] paramNames = { "applicationId", "objectType", "objectId" };
            object[] paramValues = { applicationId, (int)type, objectId };

            return HibernateTemplate.FindByNamedQueryAndNamedParam<Issue>("Issue.FindAllIssuesByTypeAndObjectId", paramNames, paramValues);
        }

        [Transaction(ReadOnly = true)]
        public Issue FindIssue(Guid applicationId, IssueObjectType type, Guid objectId, string title, string text)
        {
            string[] paramNames = { "applicationId", "objectType", "objectId", "title", "text" };
            object[] paramValues = { applicationId, (int)type, objectId, title, text };

            IList<Issue> foundIssues = HibernateTemplate.FindByNamedQueryAndNamedParam<Issue>("Issue.FindIssue", paramNames, paramValues);

            if (foundIssues.Count == 1)
                return foundIssues[0];
            else
                return null;
        }

        [Transaction(ReadOnly = false)]
        public void DeleteIssues(Guid applicationId, IssueObjectType type, Guid objectId)
        {
            DeleteIssues(applicationId, type, objectId, null);
        }

        [Transaction(ReadOnly = false)]
        public void DeleteIssues(Guid applicationId, IssueObjectType type, Guid objectId, IList<Issue> dontDeleteIssueList)
        {
            if (objectId != Guid.Empty)
            {
                DeleteIssuesForObjectId(applicationId, type, objectId, dontDeleteIssueList);
            }
            else
            {
                DeleteIssuesForObjectType(applicationId, type, dontDeleteIssueList);
            }
        }

        [Transaction(ReadOnly = false)]
        private void DeleteIssuesForObjectId(Guid applicationId, IssueObjectType type, Guid objectId, IList<Issue> dontDeleteIssueList)
        {
            IList<Issue> foundIssues = null;

            Guid[] varIdList = null;

            if (dontDeleteIssueList == null ||
                dontDeleteIssueList.Count == 0)
            {
                varIdList = new List<Guid>{ Guid.Empty }.ToArray();
            }
            else
            {
                // Get the id's from the Issuelist
                varIdList = dontDeleteIssueList.Select(x => x.Id).ToArray();
            }

            string[] paramNames = { "applicationId", "objectType", "objectId", "exceptionIdList" };
            object[] paramValues = { applicationId, (int)type, objectId, varIdList };

            foundIssues = HibernateTemplate.FindByNamedQueryAndNamedParam<Issue>("Issue.SelectToDeleteIssuesForObjectId", paramNames, paramValues);

            if (foundIssues != null && foundIssues.Count > 0)
            {
                foreach (Issue issue in foundIssues)
                {
                    Delete(issue);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        private void DeleteIssuesForObjectType(Guid applicationId, IssueObjectType type, IList<Issue> dontDeleteIssueList)
        {
            IList<Issue> foundIssues = null;

            Guid[] varIdList = null;

            if (dontDeleteIssueList == null ||
                dontDeleteIssueList.Count == 0)
            {
                varIdList = new List<Guid>().ToArray();
            }
            else
            {
                // Get the id's from the Issuelist
                varIdList = dontDeleteIssueList.Select(x => x.Id).ToArray();
            }

            string[] paramNames = { "applicationId", "objectType", "exceptionIdList" };
            object[] paramValues = { applicationId, (int)type, varIdList };

            foundIssues = HibernateTemplate.FindByNamedQueryAndNamedParam<Issue>("Issue.SelectToDeleteIssuesForObjectType", paramNames, paramValues);

            if (foundIssues != null && foundIssues.Count > 0)
            {
                foreach(Issue issue in foundIssues)
                {
                    Delete(issue);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public Issue SaveOrUpdate(Issue issue)
        {
            HibernateTemplate.SaveOrUpdate(issue);
            return issue;
        }

        [Transaction(ReadOnly = false)]
        public void Delete(Issue issue)
        {
            HibernateTemplate.Delete(issue);
        }

        #endregion
    }
}
