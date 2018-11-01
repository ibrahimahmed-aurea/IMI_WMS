using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace Cdc.MetaManager.BusinessLogic
{
    public class ClearCaseRepositoryService : IRepositoryService
    {
        private int _totalCheckIns;
        private int _checkInCount;
        
        private class OutputErrorPair
        {
            public OutputErrorPair(string output, string error)
            {
                this.Output = output;
                this.Error = error;
            }

            public string Output { get; set; }
            public string Error { get; set; }
        }

        private struct ClearCaseCommands
        {
            public static string CHECK_OUT = "co -nc \"{0}\"";
            public static string CHECK_IN = "ci -c \"{1}\" -ide \"{0}\"";
            public static string MAKE_DIR = "mkdir -nc \"{0}\"";
            public static string MAKE_DIR_NO_CHECK_OUT = "mkdir -nco -nc \"{0}\"";
            public static string MAKE_ELEMENT = "mkelem -ci -nc \"{0}\"";
            public static string MAKE_ELEMENT_NO_CHECK_OUT = "mkelem -nco -nc \"{0}\"";
            public static string GET_ELEMENT_IN_PRIVATE_VIEW = "ls -view_only \"{0}\"";
            public static string GET_ELEMENT_IN_REPOSITORY = "ls \"{0}\"";
            public static string GET_DIR_IN_REPOSITORY = "ls -d \"{0}\"";
            public static string GET_CHECKED_OUT_BY_ME = "lscheckout -me -cview \"{0}\"";
            public static string CHANGE_TYPE_TO_COMPRESSED_FILE = "chtype -force compressed_file \"{0}\"";
            public static string REMOVE_FILE = "rm \"{0}\"";
            public static string GET = "get -to \"{0}\" \"{1}\"";
            public static string GET_CONFIG_SPEC = "catcs";
        }

        public ClearCaseRepositoryService()
        {
        }

        private OutputErrorPair ExecuteClearCaseCommand(string command, string path, string pname = "", string viewPath = "")
        {
            if (string.IsNullOrEmpty(viewPath))
            {
                System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
                viewPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
                viewPath = viewPath.Replace(@"\metadata", "");
            }

            string outputtext = string.Empty;
            string errortext = string.Empty;
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(@"cleartool");
            try
            {
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WorkingDirectory = viewPath;

                if (!string.IsNullOrEmpty(path) || !string.IsNullOrEmpty(pname))
                {
                    if (!string.IsNullOrEmpty(pname))
                    {
                        psi.Arguments = string.Format(command, path, pname);
                    }
                    else
                    {
                        psi.Arguments = string.Format(command, path);
                    }
                }
                else
                {
                    psi.Arguments = command;
                }

                System.Diagnostics.Process process;
                process = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader outputStream = process.StandardOutput;
                System.IO.StreamReader errorStream = process.StandardError;
                
                //Fix to handle long outputs from clearcase at check in.
                outputtext = outputStream.ReadToEnd();
                errortext = errorStream.ReadToEnd();

                process.WaitForExit();

                if (process.HasExited)
                {
                    errortext += errorStream.ReadToEnd();
                    outputtext += outputStream.ReadToEnd();

                    return new OutputErrorPair(outputtext, errortext);
                }
            }
            catch
            {
                return new OutputErrorPair("", "System Error");
            }

            return new OutputErrorPair("", "System Error");
        }
        
        // It necessary to have a seperate routine for checkin depending on the trigger functions in ClearCase
        // The checkin command will stop when using the other routine if Deploy is activated for the vob. 
        private OutputErrorPair ExecuteClearCaseCommandCheckin(string command, string path)
        {
            string outputtext = string.Empty;
            string errortext = string.Empty;
            System.Diagnostics.Process process = new Process();
            string comment = string.Empty;

            try
            {
                process.StartInfo.FileName = "ClearTool.exe";
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                // It is necessary to set UseShellExecute to true
                // This is because of the Deploy application that is triggered from ClearCase at checkins
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Arguments = string.Format(command, path, comment);
                process.Start();
                process.WaitForExit();

                if (process.HasExited)
                {
                    if (process.ExitCode != 0)
                    {

                        errortext += "Error calling Clear Case.";
                    }

                    outputtext = string.Empty;

                    return new OutputErrorPair(outputtext, errortext);
                }
            }
            catch
            {
                return new OutputErrorPair("", "System Error");
            }

            return new OutputErrorPair("", "System Error");
        }

        private bool IsDirectoryInClearCase(string path)
        {
            return string.IsNullOrEmpty(ExecuteClearCaseCommand(ClearCaseCommands.GET_DIR_IN_REPOSITORY, path).Output) ? false : true;
        }

        private bool IsCheckedOutByMe(string path)
        {
            return string.IsNullOrEmpty(ExecuteClearCaseCommand(ClearCaseCommands.GET_CHECKED_OUT_BY_ME, path).Output) ? false : true;
        }

        private bool CreateDirectoryInClearCase(string path, string parent_path, out string errorMessage)
        {
            errorMessage = ExecuteClearCaseCommand(ClearCaseCommands.CHECK_OUT, parent_path).Error;
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = ExecuteClearCaseCommand(ClearCaseCommands.MAKE_DIR_NO_CHECK_OUT, path).Error;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = ExecuteClearCaseCommandCheckin(ClearCaseCommands.CHECK_IN, parent_path).Error;
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsFileInClearCase(string path)
        {
            return string.IsNullOrEmpty(ExecuteClearCaseCommand(ClearCaseCommands.GET_ELEMENT_IN_REPOSITORY, path).Output) ? false : true;
        }

        private bool IsFileInPrivateView(string path)
        {
            string output = ExecuteClearCaseCommand(ClearCaseCommands.GET_ELEMENT_IN_PRIVATE_VIEW, path).Output;

            if (string.IsNullOrEmpty(output))
            {
                return false;
            }
            else
            {
                string[] outputlines = output.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (outputlines.Where(s => s.Contains("CHECKEDOUT") == false).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        #region IRepositoryService Members

        public bool IsAtomicCheckInSupported
        {
            get
            {
                return false;
            }
        }
                        
        private static void UpdateDeployInfo(int checkInCount, int totalCheckIns)
        {
            string fileName = Path.Combine(Path.GetTempPath(), "DeployMetamanagerCheckin.dat");

            if (totalCheckIns > 1)
            {
                File.WriteAllText(fileName, string.Format("{0}\r\n{1}", checkInCount, totalCheckIns));
            }
            else
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                { 
                }
            }
        }

        public void BeginCheckIn(IList<DataAccess.IVersionControlled> domainObjects)
        {
            _checkInCount = 0;
            _totalCheckIns = domainObjects.Count;
                        
            UpdateDeployInfo(_checkInCount, _totalCheckIns);
        }

        public void RollbackCheckIn()
        {
            _checkInCount = 0;
            _totalCheckIns = 0;

            UpdateDeployInfo(_checkInCount, _totalCheckIns);
        }

        public IList<Cdc.MetaManager.DataAccess.IVersionControlled> CommitCheckIn()
        {
            _checkInCount = 0;
            _totalCheckIns = 0;

            UpdateDeployInfo(_checkInCount, _totalCheckIns);

            return null;
        }

        public bool CheckOutFile(Cdc.MetaManager.DataAccess.IVersionControlled domainObject, Cdc.MetaManager.DataAccess.Domain.Application application, out string errorMessage)
        {
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();

            string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
            string parentPath = System.IO.Path.Combine(rootPath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
            string filePath = System.IO.Path.Combine(parentPath, NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject).Name);
            string fileName = domainObject.RepositoryFileName + ".xml";
            string fullPath = System.IO.Path.Combine(filePath, fileName);

            bool success = true;

            errorMessage = string.Empty;

            if (IsFileInClearCase(fullPath))
            {
                if (!IsCheckedOutByMe(fullPath))
                {
                    errorMessage = ExecuteClearCaseCommand(ClearCaseCommands.CHECK_OUT, fullPath).Error;
                    success = string.IsNullOrEmpty(errorMessage);
                }
            }
            
            return success;
        }

        public bool CheckInFile(DataAccess.IVersionControlled domainObject, DataAccess.Domain.Application application, out string errorMessage)
        {
            _checkInCount++;
            UpdateDeployInfo(_checkInCount, _totalCheckIns);
            
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();

            string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
            string parentPath = System.IO.Path.Combine(rootPath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
            string filePath = System.IO.Path.Combine(parentPath, NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject).Name);
            string fileName = domainObject.RepositoryFileName + ".xml";
            string fullPath = System.IO.Path.Combine(filePath, fileName);

            bool success = false;

            errorMessage = string.Empty;

            if (IsFileInPrivateView(fullPath))
            {
                errorMessage = ExecuteClearCaseCommand(ClearCaseCommands.CHECK_OUT, filePath).Error;

                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = ExecuteClearCaseCommandCheckin(ClearCaseCommands.MAKE_ELEMENT, fullPath).Error;                    
                    
                    // As long as filesize is zero, sleep 100 ms. If sleeping, log this to file
                    int n;
                    FileInfo fi = new FileInfo(fullPath);                    
                    for (n = 0; n < 100; n++)
                    {
                        if (fi.Length > 0)
                        {
                            break;
                        }                    
                        Thread.Sleep(100);
                        
                        // Logging this to file
                        String uriPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\TmpLogfile.txt";
                        string localPath = new Uri(uriPath).LocalPath;
                        using (StreamWriter sw = File.AppendText(localPath))
                        {
                            sw.WriteLine(DateTime.Now.ToString() + ": CheckInFile(): Slept for 100 ms. n = " + n.ToString());
                        }
                    }

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ExecuteClearCaseCommandCheckin(ClearCaseCommands.CHECK_IN, filePath).Error;
                        success = string.IsNullOrEmpty(errorMessage);                        
                    }
                }
            }
            else if (IsFileInClearCase(fullPath))
            {
                errorMessage = ExecuteClearCaseCommandCheckin(ClearCaseCommands.CHECK_IN, fullPath).Error;
                success = string.IsNullOrEmpty(errorMessage);
            }
            else
            {
                errorMessage = string.Format("File \"{0}\" is not under source control.", fullPath);
            }
                        
            return success;
        }

        public bool RemoveFile(DataAccess.IVersionControlled domainObject, DataAccess.Domain.Application application, out string errorMessage)
        {
            _checkInCount++;
            UpdateDeployInfo(_checkInCount, _totalCheckIns);
                        
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();

            string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
            string parentPath = System.IO.Path.Combine(rootPath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
            string filePath = System.IO.Path.Combine(parentPath, NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject).Name);
            string fileName = domainObject.RepositoryFileName + ".xml";
            string fullPath = System.IO.Path.Combine(filePath, fileName);

            bool success = false;

            errorMessage = string.Empty;

            if (IsFileInClearCase(fullPath))
            {
                errorMessage = ExecuteClearCaseCommand(ClearCaseCommands.CHECK_OUT, filePath).Error;

                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = ExecuteClearCaseCommand(ClearCaseCommands.REMOVE_FILE, fullPath).Error;

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = ExecuteClearCaseCommandCheckin(ClearCaseCommands.CHECK_IN, filePath).Error;
                        success = string.IsNullOrEmpty(errorMessage);
                    }
                }
            }
            else
            {
                errorMessage = string.Format("File \"{0}\" is not under source control.", fullPath);
            }
                        
            return success;
        }

        public void DiffFiles(string baseFilePath, string currentFilePath)
        {
            Process.Start("cleardiffmrg", "-bas " + baseFilePath + " " + currentFilePath);
        }

        public string GetSpecificVersionOfFile(string filePath, string version, string repositoryPathForImport)
        {
            string tempFileName = Guid.NewGuid().ToString() + ".xml";

            string tempPath = Path.Combine(Path.GetTempPath(), tempFileName);
            if (System.IO.File.Exists(tempPath))
            {
                System.IO.File.Delete(tempPath);
            }

            string pname = filePath + "@@" + version;

            ExecuteClearCaseCommand(ClearCaseCommands.GET, tempPath, pname, repositoryPathForImport);

            string fileContent = string.Empty;

            if (File.Exists(tempPath))
            {
                StreamReader reader = File.OpenText(tempPath);
                fileContent = reader.ReadToEnd();
            }

            return fileContent;
        }

        public string GetBranchName(string path)
        {
            OutputErrorPair result = ExecuteClearCaseCommand(ClearCaseCommands.GET_CONFIG_SPEC, "", "", path);

            if (!string.IsNullOrEmpty(result.Output))
            {
                string viewInfo = result.Output;

                viewInfo = result.Output.Substring(result.Output.IndexOf("element *"));

                viewInfo = viewInfo.Replace("/r", "");

                string[] rows = viewInfo.Split('\n');

                for (int i = 0; i < rows.Length; i++) 
                {
                    if (rows[i].StartsWith("element * "))
                    {
                        rows[i] = rows[i].Substring(10);
                        rows[i] = rows[i].Replace("\r", "");
                    }
                    else
                    {
                        rows[i] = string.Empty;
                    }

                    if (rows[i] != string.Empty)
                    {
                        string[] ruleParts = rows[i].Split(' ');

                        if (ruleParts.Length > 0)
                        {
                            if (ruleParts.Length == 1)
                            {
                                string[] branchDescr = ruleParts[0].Split('\\');

                                if (branchDescr.Last() == "LATEST")
                                {
                                    return branchDescr[branchDescr.Length - 2];
                                }
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
