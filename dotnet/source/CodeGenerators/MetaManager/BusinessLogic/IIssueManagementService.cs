using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.BusinessLogic
{
    public class IssueFileInformation
    {
        public IssueFileInformation(string filePath, string latestVersionForIssue, string versionPriorToIssue)
        {
            FilePath = filePath;
            LatestVersionForIssue = latestVersionForIssue;
            VersionPriorToIssue = versionPriorToIssue;
        }

        public string FilePath;
        public string LatestVersionForIssue;
        public string VersionPriorToIssue;
    }

    public interface IIssueManagementService
    {
        List<IssueFileInformation> GetFilesIncludedInIssue(string Issue, string branch, out string additionalInfo);
    }
}
