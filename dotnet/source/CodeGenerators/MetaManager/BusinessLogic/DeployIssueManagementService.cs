using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

namespace Cdc.MetaManager.BusinessLogic
{
    public class DeployIssueManagementService : IIssueManagementService
    {

        #region IIssueManagementService Members

        public List<IssueFileInformation> GetFilesIncludedInIssue(string Issue, string branch, out string additionalInfo)
        {
            additionalInfo = string.Empty;
            List<IssueFileInformation> returnList = new List<IssueFileInformation>();

            if (!string.IsNullOrEmpty(Issue))
            {
                using (OracleConnection connection = new OracleConnection("data source=DEPLOY;user id=DEPLOY;password=DEPLOY;pooling=true;enlist=false"))
                {
                    string projectId;
                    string branch_path;

                    Dictionary<string, KeyValuePair<string, string>> fileInfo = new Dictionary<string, KeyValuePair<string, string>>();

                    connection.Open();
                    
                    //Get ProjectId
                    //----------------------------------------------------------------------------------------------------------------------------
                    string sql = "SELECT Project_Id, Branch_Path FROM Dev_Path WHERE Branch_Name = '" + branch + "'";

                    OracleCommand cmd = new OracleCommand(sql, connection);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);

                    System.Data.DataTable ProjectIdDT = new System.Data.DataTable();

                    da.Fill(ProjectIdDT);

                    if (ProjectIdDT.Rows.Count == 1)
                    {
                        projectId = ProjectIdDT.Rows[0][0].ToString();
                        branch_path = ProjectIdDT.Rows[0][1].ToString();
                        additionalInfo += "ProjectId: " + projectId + " Branch Path: " + branch_path;
                    }
                    else
                    {
                        throw new Exception("No or multiple project Ids found for the given branch, DeployIssueManagement, Branch: " + branch);
                    }

                    //-----------------------------------------------------------------------------------------------------------------------------

                    sql = "SELECT Element_Name, Element_Revision FROM Element_Revision WHERE Issue_Id = '" + Issue + "' AND Project_Id = '" + projectId + "' AND Branch_Path = '" + branch_path + @"' AND Element_Name like '\metadata\%'";

                    cmd.CommandText = sql;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                string filepath = reader.GetString(0).Substring(1);
                                string fileVersion = reader.GetString(1);

                                if (!fileInfo.ContainsKey(filepath))
                                {
                                    fileInfo.Add(filepath, new KeyValuePair<string, string>(fileVersion, fileVersion));
                                }
                                else
                                {
                                    string latestVersion = fileInfo[filepath].Key;
                                    string firstVersion = fileInfo[filepath].Value;

                                    int latest = Convert.ToInt32(latestVersion.Substring(latestVersion.LastIndexOf(@"\")+1));
                                    int first = Convert.ToInt32(firstVersion.Substring(firstVersion.LastIndexOf(@"\") + 1));
                                    int current = Convert.ToInt32(fileVersion.Substring(fileVersion.LastIndexOf(@"\") + 1));

                                    bool change = false;

                                    if (current > latest)
                                    {
                                        latestVersion = fileVersion;
                                        change = true;
                                    }

                                    if (current < first)
                                    {
                                        firstVersion = fileVersion;
                                        change = true;
                                    }

                                    if (change)
                                    {
                                        fileInfo[filepath] = new KeyValuePair<string, string>(latestVersion, firstVersion);
                                    }
                                }
                            }
                        }
                    }

                    foreach (KeyValuePair<string, KeyValuePair<string, string>> fileInfoElement in fileInfo)
                    {
                        string priorVersion = string.Empty;

                        int firstVersionNr = Convert.ToInt32(fileInfoElement.Value.Value.Substring(fileInfoElement.Value.Value.LastIndexOf(@"\")+1));

                        priorVersion = fileInfoElement.Value.Value.Substring(0, fileInfoElement.Value.Value.LastIndexOf(@"\") + 1) + (firstVersionNr - 1).ToString();

                        returnList.Add(new IssueFileInformation(fileInfoElement.Key, fileInfoElement.Value.Key, priorVersion));
                    }

                }
            }

            return returnList;
        }

        #endregion
    }
}
