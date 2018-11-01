using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Interop.Security.AzRoles;
using Imi.SupplyChain.Authorization.BusinessEntities;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Imi.SupplyChain.Authorization.DataAccess
{
    public class AzManAuthorizationDao : IAuthorizationDao
    {
        private Dictionary<string, AzAuthorizationStore> _storeDictionary;
        private Dictionary<string, IAzApplication2> _applicationDictionary;
        private Dictionary<string, Dictionary<string, int>> _operationDictionary;
        private Dictionary<string, FileSystemWatcher> _watcherDictionary;
        private ReaderWriterLockSlim _syncLock;
        private const int AZ_AZSTORE_FLAG_BATCH_UPDATE = 4;
        private const string AZ_MERGE_FOLDER = "AuthorizationMerge";

        static AzManAuthorizationDao()
        {
            //Merge authorization files on startup
            CheckAndMergeStores();
        }

        private static void CheckAndMergeStores()
        {
            SecuritySettings securitySettings = (SecuritySettings)ConfigurationManager.GetSection(SecuritySettings.SectionName);

            NameTypeConfigurationElementCollection<AuthorizationProviderData, CustomAuthorizationProviderData> providerElementCollection = securitySettings.AuthorizationProviders;
            
            LogEntry entry = new LogEntry();
            entry.Severity = TraceEventType.Verbose;
            entry.Priority = -1;

            if (Logger.ShouldLog(entry))
            {
                entry.Message = "Checking for authorization store merge files...";
                Logger.Write(entry);
            }

            for (int i = 0; i < providerElementCollection.Count; i++)
            {
                AuthorizationProviderData providerData = providerElementCollection.Get(i);

                string toStoreLocation = providerData.ElementInformation.Properties["storeLocation"].Value as string;
                toStoreLocation = toStoreLocation.Replace("{currentPath}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                string toFileName = toStoreLocation.Replace("msxml://", "");

                string fromFileName = Path.Combine(Path.Combine(Path.GetDirectoryName(toFileName), AZ_MERGE_FOLDER), Path.GetFileName(toFileName));
                string fromStoreLocation = string.Format("msxml://{0}", fromFileName);
                                                                
                if (File.Exists(fromFileName))
                {
                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = string.Format("Found merge file \"{0}\".", fromFileName);
                        Logger.Write(entry);
                    }

                    string backupFileName = Path.ChangeExtension(toFileName, "bak");

                    int counter = 0;

                    while (File.Exists(backupFileName))
                    {
                        counter++;
                        backupFileName = Path.ChangeExtension(backupFileName, string.Format("bak{0}", counter));
                    }

                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = string.Format("Creating backup \"{0}\"...", backupFileName);
                        Logger.Write(entry);
                    }

                    File.Copy(toFileName, backupFileName);

                    FileInfo fi = new FileInfo(toFileName);
                    fi.IsReadOnly = false;
                    
                    MergeStores(fromStoreLocation, toStoreLocation);

                    string mergedFileName = Path.ChangeExtension(fromFileName, "merged");

                    counter = 0;

                    while (File.Exists(mergedFileName))
                    {
                        counter++;
                        mergedFileName = Path.ChangeExtension(mergedFileName, string.Format("merged{0}", counter));
                    }

                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = string.Format("Renaming merge file to \"{0}\"...", mergedFileName);
                        Logger.Write(entry);
                    }
                    
                    File.Move(fromFileName, mergedFileName);

                    if (Logger.ShouldLog(entry))
                    {
                        entry.Message = string.Format("Merge complete.");
                        Logger.Write(entry);
                    }
                }
            }
        }

        public AzManAuthorizationDao()
        {
            _storeDictionary = new Dictionary<string, AzAuthorizationStore>();
            _applicationDictionary = new Dictionary<string, IAzApplication2>();
            _operationDictionary = new Dictionary<string, Dictionary<string, int>>();
            _watcherDictionary = new Dictionary<string, FileSystemWatcher>();
            _syncLock = new ReaderWriterLockSlim();

            SecuritySettings securitySettings = (SecuritySettings)ConfigurationManager.GetSection(SecuritySettings.SectionName);

            NameTypeConfigurationElementCollection<AuthorizationProviderData, CustomAuthorizationProviderData> providerElementCollection = securitySettings.AuthorizationProviders;

            for (int i = 0; i < providerElementCollection.Count; i++)
            {
                AuthorizationProviderData providerData = providerElementCollection.Get(i);

                string storeLocation = providerData.ElementInformation.Properties["storeLocation"].Value as string;
                storeLocation = storeLocation.Replace("{currentPath}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                string providerName = providerData.Name;
                string applicationName = providerData.ElementInformation.Properties["application"].Value as string;

                InitializeProvider(providerName, applicationName, storeLocation);
            }

            foreach (var entry in _watcherDictionary)
            {
                entry.Value.EnableRaisingEvents = true;
            }
        }

        private static void MergeStores(string fromStoreLocation, string toStoreLocation)
        {
            LogEntry entry = new LogEntry();
            entry.Severity = TraceEventType.Verbose;
            entry.Priority = -1;

            if (Logger.ShouldLog(entry))
            {
                entry.Message = string.Format("Merging authorization stores.\nFrom Store = \"{0}\"\nTo Store = \"{1}\"...", fromStoreLocation, toStoreLocation);
                Logger.Write(entry);
            }

            try
            {
                AzAuthorizationStore fromStore = new AzAuthorizationStore();
                fromStore.Initialize(0, fromStoreLocation, null);
                
                AzAuthorizationStore toStore = new AzAuthorizationStore();
                toStore.Initialize(AZ_AZSTORE_FLAG_BATCH_UPDATE, toStoreLocation, null);
                
                foreach (IAzApplication3 fromApplication in fromStore.Applications)
                {
                    IAzApplication3 toApplication = (IAzApplication3)((IAzAuthorizationStore3)toStore).OpenApplication2(fromApplication.Name, null);
                    
                    var operationsDictionary = new Dictionary<string, IAzOperation>();
                    
                    int nextOperationId = 0;

                    foreach (IAzOperation toOperation in toApplication.Operations)
                    {
                        operationsDictionary.Add(toOperation.Name, toOperation);
                        nextOperationId = Math.Max(nextOperationId, toOperation.OperationID);
                    }
                                        
                    foreach (IAzOperation fromOperation in fromApplication.Operations)
                    {
                        IAzOperation toOperation = null;

                        if (operationsDictionary.ContainsKey(fromOperation.Name))
                        {
                            toOperation = operationsDictionary[fromOperation.Name];
                        }
                        else
                        {
                            if (Logger.ShouldLog(entry))
                            {
                                entry.Message = string.Format("Adding new Operation \"{0}\"...", fromOperation.Name);
                                Logger.Write(entry);
                            }

                            toOperation = toApplication.CreateOperation(fromOperation.Name);
                            nextOperationId++;
                            toOperation.OperationID = nextOperationId;
                        }
                                                
                        toOperation.Description = fromOperation.Description;
                        toOperation.Submit();
                    }
                                        
                    var tasksDictionary = new Dictionary<string, IAzTask>();

                    foreach (IAzTask toTask in toApplication.Tasks)
                    {
                        tasksDictionary.Add(toTask.Name, toTask);
                    }

                    foreach (IAzTask fromTask in fromApplication.Tasks)
                    {
                        IAzTask toTask = null;
                        
                        if (tasksDictionary.ContainsKey(fromTask.Name))
                        {
                            toTask = tasksDictionary[fromTask.Name];
                        }
                        else
                        {
                            if (Logger.ShouldLog(entry))
                            {
                                entry.Message = string.Format("Adding new Task \"{0}\"...", fromTask.Name);
                                Logger.Write(entry);
                            }

                            toTask = toApplication.CreateTask(fromTask.Name);
                        }
                                                
                        toTask.IsRoleDefinition = fromTask.IsRoleDefinition;
                        toTask.Description = fromTask.Description;

                        foreach (string taskOperation in fromTask.Operations)
                        {
                            if (!((object[])toTask.Operations).Contains(taskOperation))
                            {
                                if (Logger.ShouldLog(entry))
                                {
                                    entry.Message = string.Format("Adding Operation \"{0}\" to Task \"{1}\"...", taskOperation, toTask.Name);
                                    Logger.Write(entry);
                                }

                                toTask.AddOperation(taskOperation);
                            }
                        }

                        toTask.Submit();
                    }
                                        
                    var rolesDictionary = new Dictionary<string, IAzRoleDefinition>();

                    foreach (IAzRoleDefinition toRole in toApplication.RoleDefinitions)
                    {
                        rolesDictionary.Add(toRole.Name, toRole);
                    }

                    foreach (IAzRoleDefinition fromRole in fromApplication.RoleDefinitions)
                    {
                        IAzRoleDefinition toRole = null;
                        
                        if (rolesDictionary.ContainsKey(fromRole.Name))
                        {
                            toRole = rolesDictionary[fromRole.Name];
                        }
                        else
                        {
                            if (Logger.ShouldLog(entry))
                            {
                                entry.Message = string.Format("Adding new Role Definition \"{0}\"...", fromRole.Name);
                                Logger.Write(entry);
                            }

                            toRole = toApplication.CreateRoleDefinition(fromRole.Name);
                        }

                        toRole.Description = toRole.Description;

                        foreach (string roleOperation in fromRole.Operations)
                        {
                            if (!((object[])toRole.Operations).Contains(roleOperation))
                            {
                                if (Logger.ShouldLog(entry))
                                {
                                    entry.Message = string.Format("Adding Operation \"{0}\" to Role Definition \"{1}\"...", roleOperation, toRole.Name);
                                    Logger.Write(entry);
                                }

                                toRole.AddOperation(roleOperation);
                            }
                        }

                        foreach (string roleTask in fromRole.Tasks)
                        {
                            if (!((object[])toRole.Tasks).Contains(roleTask))
                            {
                                if (Logger.ShouldLog(entry))
                                {
                                    entry.Message = string.Format("Adding Task \"{0}\" to Role Definition \"{1}\"...", roleTask, toRole.Name);
                                    Logger.Write(entry);
                                }

                                toRole.AddTask(roleTask);
                            }
                        }

                        toRole.Submit();
                    }
                }

                if (Logger.ShouldLog(entry))
                {
                    entry.Message = string.Format("Submitting changes...");
                    Logger.Write(entry);
                }

                toStore.Submit();
            }
            catch (Exception ex)
            {
                AuthorizationException authException = new AuthorizationException(string.Format("Failed to merge authorization stores.\nFrom Store = \"{0}\"\nTo Store = \"{1}\"\n", fromStoreLocation, toStoreLocation), ex);

                entry.Severity = TraceEventType.Error;

                if (Logger.ShouldLog(entry))
                {
                    entry.Message = authException.ToString();
                    Logger.Write(entry);
                }

                throw authException;
            }
        }

        private void InitializeProvider(string providerName, string applicationName, string storeLocation)
        {
            if (!_storeDictionary.ContainsKey(storeLocation))
            {
                try
                {
                    AzAuthorizationStore store = new AzAuthorizationStore();

                    store.Initialize(0, storeLocation, null);
                                        
                    _storeDictionary.Add(storeLocation, store);

                    AddFileSystemWatcher(storeLocation);
                }
                catch (Exception ex)
                {
                    throw new AuthorizationException(string.Format("Failed to open authorization store. ProviderName = {0}, ApplicationName = {1}, StoreLocation = {2}.", providerName, applicationName, storeLocation), ex);
                }
            }

            if (!_applicationDictionary.ContainsKey(providerName))
            {
                try
                {
                    IAzApplication2 application = ((IAzAuthorizationStore2)_storeDictionary[storeLocation]).OpenApplication2(applicationName, null);
                    _applicationDictionary.Add(providerName, application);
                    
                    _operationDictionary.Add(providerName, new Dictionary<string, int>());

                    foreach (IAzOperation o in application.Operations)
                    {
                        _operationDictionary[providerName].Add(o.Name, o.OperationID);
                    }
                }
                catch (Exception ex)
                {
                    throw new AuthorizationException(string.Format("Failed to open application. ProviderName = {0}, ApplicationName = {1}, StoreLocation = {2}.", providerName, applicationName, storeLocation), ex);
                }
            }
        }

        private void AddFileSystemWatcher(string storeLocation)
        {
            string storeFileName = storeLocation.Replace("msxml://", "");

            if (File.Exists(storeFileName))
            {
                string storeDirectory = Path.GetDirectoryName(storeFileName);

                if (!_watcherDictionary.ContainsKey(storeDirectory))
                {
                    FileSystemWatcher watcher = new FileSystemWatcher(storeDirectory, string.Format("*{0}", Path.GetExtension(storeFileName)));
                    watcher.NotifyFilter = NotifyFilters.LastWrite;
                    watcher.Changed += StoreChangedEventHandler;

                    _watcherDictionary.Add(storeDirectory, watcher);
                }
            }
        }

        private void StoreChangedEventHandler(object sender, FileSystemEventArgs e)
        {
            FileSystemWatcher watcher = sender as FileSystemWatcher;

            watcher.EnableRaisingEvents = false;

            try
            {
                var store = (from entry in _storeDictionary
                             where entry.Key.EndsWith(e.FullPath)
                             select entry.Value).LastOrDefault();

                if (store != null)
                {
                    int tries = 0;

                    while (true)
                    {
                        tries++;

                        Thread.Sleep(5000);

                        try
                        {
                            _syncLock.TryEnterWriteLock(-1);

                            store.UpdateCache(null);

                            break;
                        }
                        catch (Exception)
                        {
                            if (tries > 3)
                            {
                                throw;
                            }
                        }
                        finally
                        {
                            _syncLock.ExitWriteLock();
                        }
                    }
                }
            }
            finally
            {
                watcher.EnableRaisingEvents = true;
            }
        }

        public void Authorize(IList<AuthOperation> operations, IList<string> roles, string authorizationProvider)
        {
            _syncLock.TryEnterReadLock(-1);

            try
            {
                if (!_applicationDictionary.ContainsKey(authorizationProvider))
                {
                    throw new AuthorizationException(string.Format("The authorization provider has not been initialized."));
                }

                IAzClientContext2 clientContext = _applicationDictionary[authorizationProvider].InitializeClientContext2(authorizationProvider, null);

                object[] roleArray = new object[roles.Count];

                for (int i = 0; i < roles.Count; i++)
                {
                    roleArray[i] = roles[i];
                }

                clientContext.AddStringSids(roleArray);

                var operationIds = new List<int>();
                var operationList = new List<AuthOperation>();

                foreach (AuthOperation operation in operations)
                {
                    if (_operationDictionary[authorizationProvider].ContainsKey(operation.Operation))
                    {
                        operationIds.Add(_operationDictionary[authorizationProvider][operation.Operation]);
                        operationList.Add(operation);
                    }
                    else
                    {
                        operation.IsAuthorized = true;
                    }
                }

                if (operationIds.Count > 0)
                {
                    var scope = new object[1] { "" };

                    object[] result = clientContext.AccessCheck(authorizationProvider, scope, operationIds.Cast<object>().ToArray(), null, null, null, null, null);

                    for (int i = 0; i < result.Length; i++)
                    {
                        operationList[i].IsAuthorized = (int)result[i] == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(string.Format("Authorization failed. ProviderName = {0}.", authorizationProvider), ex);
            }
            finally
            {
                _syncLock.ExitReadLock();
            }
        }
    }
}
