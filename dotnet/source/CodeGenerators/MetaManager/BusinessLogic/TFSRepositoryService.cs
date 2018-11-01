using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.ComponentModel;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.Client;
using System.Configuration;

namespace Cdc.MetaManager.BusinessLogic
{
    public class TFSRepositoryService : IRepositoryService
    {
        private Workspace _workspace;
        private string _workspacePath;
        private VersionControlServer _versionControlServer;
        private Dictionary<DataAccess.IVersionControlled, string> _pendingCheckIns;
        private bool _isAtomicCheckInEnabled;
        
        public TFSRepositoryService()
        {
            _pendingCheckIns = new Dictionary<DataAccess.IVersionControlled, string>();

            AppSettingsReader appReader = new AppSettingsReader();

            _workspacePath = appReader.GetValue("RepositoryPath", typeof(string)).ToString();

            WorkspaceInfo sourceWorkspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(_workspacePath);
            TfsTeamProjectCollection projectCollecion = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(sourceWorkspaceInfo.ServerUri);
            _versionControlServer = (VersionControlServer)projectCollecion.GetService(typeof(VersionControlServer));
            _workspace = _versionControlServer.GetWorkspace(sourceWorkspaceInfo);
        }

        #region IRepositoryService Members

        public bool IsAtomicCheckInSupported
        {
            get
            {
                return true;
            }
        }

        public void PrepareCheckIn(IList<DataAccess.IVersionControlled> domainObjects)
        { 
        }

        public void BeginCheckIn(IList<DataAccess.IVersionControlled> domainObjects)
        {
            if (_pendingCheckIns.Count > 0)
            {
                throw new InvalidOperationException("An atomic check in is already in progress.");
            }

            _isAtomicCheckInEnabled = true;
        }
                
        public IList<DataAccess.IVersionControlled> CommitCheckIn()
        {
            ExecuteCheckin(_pendingCheckIns.Values.ToArray());

            var comittedItems = (from p in _pendingCheckIns
                    where _workspace.GetPendingChanges(p.Value).Count() == 0
                    select p.Key).ToList();
            
            _pendingCheckIns.Clear();

            return comittedItems;
        }

        public void RollbackCheckIn()
        {
            var pendingChanges = from p in _workspace.GetPendingChangesEnumerable()
                    where _pendingCheckIns.Values.Contains(p.LocalItem.ToLower())
                    select p;

            _workspace.Undo(pendingChanges.ToArray());

            _pendingCheckIns.Clear();
        }
                
        public bool CheckOutFile(Cdc.MetaManager.DataAccess.IVersionControlled domainObject, Cdc.MetaManager.DataAccess.Domain.Application application, out string errorMessage)
        {
            string parentPath = System.IO.Path.Combine(_workspacePath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
            string filePath = System.IO.Path.Combine(parentPath, NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject).Name);
            string fileName = domainObject.RepositoryFileName + ".xml";
            string fullPath = System.IO.Path.Combine(filePath, fileName);
            
            errorMessage = string.Empty;

            try
            {
                _workspace.PendEdit(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }

        public bool CheckInFile(Cdc.MetaManager.DataAccess.IVersionControlled domainObject, Cdc.MetaManager.DataAccess.Domain.Application application, out string errorMessage)
        {
            string parentPath = System.IO.Path.Combine(_workspacePath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
            string filePath = System.IO.Path.Combine(parentPath, NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject).Name);
            string fileName = domainObject.RepositoryFileName + ".xml";
            string fullPath = System.IO.Path.Combine(filePath, fileName);

            errorMessage = string.Empty;

            try
            {
                if (!_versionControlServer.ServerItemExists(_workspace.GetServerItemForLocalItem(fullPath), ItemType.Any))
                {
                    _workspace.PendAdd(fullPath);
                }

                CheckIn(domainObject, fullPath);

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }

        private void ExecuteCheckin(string[] files)
        {
            System.Diagnostics.Process process = new Process();
            
            process.StartInfo.FileName = System.Environment.ExpandEnvironmentVariables("\"%VS100COMNTOOLS%..\\IDE\\tf.exe\"");
            process.StartInfo.CreateNoWindow = true;

            for (int i = 0; i <files.Length; i++)
            {
                files[i] = string.Format("\"{0}\"", files[i]); 
            }

            process.StartInfo.Arguments = string.Format("checkin " + string.Join(" ", files) + " /force");
            process.Start();
            process.WaitForExit();
                        
            if (process.HasExited)
            {
                if (process.ExitCode == 100)
                {
                    throw new Exception("Check in cancelled by user.");
                }
                else if (process.ExitCode != 1 && process.ExitCode != 0)
                {
                    throw new Exception("Error calling Team Foundation Server.");
                }
            }
        }
        
        public bool RemoveFile(DataAccess.IVersionControlled domainObject, DataAccess.Domain.Application application, out string errorMessage)
        {
            string parentPath = System.IO.Path.Combine(_workspacePath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
            string filePath = System.IO.Path.Combine(parentPath, NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject).Name);
            string fileName = domainObject.RepositoryFileName + ".xml";
            string fullPath = System.IO.Path.Combine(filePath, fileName);

            errorMessage = string.Empty;

            try
            {
                if (_versionControlServer.ServerItemExists(_workspace.GetServerItemForLocalItem(fullPath), ItemType.Any))
                {
                    _workspace.PendDelete(fullPath);

                    CheckIn(domainObject, fullPath);

                    return true;
                }
                else
                {
                    errorMessage = string.Format("File \"{0}\" is not under source control.", fullPath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }

        private void CheckIn(DataAccess.IVersionControlled domainObject, string fullPath)
        {
            if (_isAtomicCheckInEnabled)
            {
                _pendingCheckIns.Add(domainObject, fullPath.ToLower());
            }
            else
            {
                ExecuteCheckin(new string[1] { fullPath });
            }
        }

        public void DiffFiles(string baseFilePath, string currentFilePath)
        {
            Process.Start("cleardiffmrg", "-bas " + baseFilePath + " " + currentFilePath);
        }

        public string GetSpecificVersionOfFile(string filePath, string version, string repositoryPathForImport)
        {
            throw new NotImplementedException();
        }

        public string GetBranchName(string path)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
