using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.IO;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
using Spring.Context.Support;
using Cdc.MetaManager.DataAccess;
using System.Reflection;
using Imi.HbmGenerator.Attributes;
using Spring.Data.NHibernate.Support;
using NHibernate;

namespace Cdc.MetaManager.BusinessLogic
{
    public class ConfigurationManagementService : IConfigurationManagementService
    {

        public IRepositoryService RepositoryService { get; set; }
        private IModelService ModelService { get; set; }

        #region IConfMgnService Members
        public event StatusChangedDelegate StatusChanged;
        public event DomainObjectChangedDelegate ObjectChanged;
        public event DomainObjectAddedDelegate ObjectAdded;

        private System.IO.FileSystemWatcher _fileSystemWatcher;

        public ConfigurationManagementService()
        {
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();

            string repositoryTypeName = appReader.GetValue("RepositoryType", typeof(System.String)).ToString();

            Type repositoryType = Assembly.GetExecutingAssembly().GetType(string.Format("{0}.{1}RepositoryService", typeof(ConfigurationManagementService).Namespace, repositoryTypeName));

            RepositoryService = (IRepositoryService)Activator.CreateInstance(repositoryType);

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(InitialCacheLoaderWorker), null);
        }

        private void InitialCacheLoaderWorker(object State)
        {
            DataAccess.DomainXmlSerializationHelper.FileCacheUpdating = true;
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();

            _fileSystemWatcher = new FileSystemWatcher(rootPath);
            _fileSystemWatcher.IncludeSubdirectories = true;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileSystemWatcher.Filter = "*.xml";
            _fileSystemWatcher.Changed += new FileSystemEventHandler(FileSystemWatcher_Changed);
            _fileSystemWatcher.EnableRaisingEvents = true;

            Dictionary<Type, List<string>> filesToRead = new Dictionary<Type, List<string>>();

            GetAllFilesRecursive(rootPath, ref filesToRead, false);

            int totalNumberOfFiles = 0;
            int progress = 0;
            object lockObj = new object();

            foreach (Type typ in filesToRead.Keys)
            {
                totalNumberOfFiles += filesToRead[typ].Count;
            }

            System.Threading.Tasks.Parallel.ForEach(filesToRead.Keys, t =>
            {
                System.Threading.Tasks.Parallel.ForEach(filesToRead[t], p =>
                {
                    lock (lockObj)
                    {
                        progress++;

                        if (DataAccess.DomainXmlSerializationHelper.WaitingForCache)
                        {
                            StatusChanged("Getting file: " + p, progress, 0, totalNumberOfFiles);
                        }
                    }

                    System.IO.FileInfo fi = new System.IO.FileInfo(p);

                    string fileContent = string.Empty;
                    bool fileReadOK = false;

                    while (!fileReadOK)
                    {
                        System.Threading.Thread.Sleep(500);

                        try
                        {
                            fileContent = fi.OpenText().ReadToEnd();
                            fileReadOK = true;
                        }
                        catch (IOException)
                        {
                            fileReadOK = false;
                        }
                    }

                    lock (DataAccess.DomainXmlSerializationHelper.XmlFileCache)
                    {
                        if (DataAccess.DomainXmlSerializationHelper.XmlFileCache.ContainsKey(p))
                        {
                            DataAccess.DomainXmlSerializationHelper.XmlFileCache[p] = fileContent;
                        }
                        else
                        {
                            DataAccess.DomainXmlSerializationHelper.XmlFileCache.Add(p, fileContent);
                        }
                    }
                });
            });

            DataAccess.DomainXmlSerializationHelper.FileCacheUpdating = false;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(e.FullPath);

            string fileContent = string.Empty;
            bool fileReadOK = false;

            while (!fileReadOK)
            {
                System.Threading.Thread.Sleep(500);

                try
                {
                    fileContent = fi.OpenText().ReadToEnd();
                    fileReadOK = true;
                }
                catch (IOException)
                {
                    fileReadOK = false;
                }
            }

            lock (DataAccess.DomainXmlSerializationHelper.XmlFileCache)
            {

                if (DataAccess.DomainXmlSerializationHelper.XmlFileCache.ContainsKey(e.FullPath))
                {
                    DataAccess.DomainXmlSerializationHelper.XmlFileCache[e.FullPath] = fileContent;
                }
                else
                {
                    DataAccess.DomainXmlSerializationHelper.XmlFileCache.Add(e.FullPath, fileContent);
                }
            }
        }

        public void CheckInDomainObjects(IList<IVersionControlled> domainObjects)
        {
            if (RepositoryService.IsAtomicCheckInSupported)
            {
                CheckInAtomic(domainObjects);
            }
            else
            {
                int max = domainObjects.Count;
                int count = 0;

                RaiseStatusChange("Starting check in...", 0, 0, max);

                RepositoryService.BeginCheckIn(domainObjects);

                try
                {

                    foreach (IVersionControlled domainObject in domainObjects)
                    {
                        RaiseStatusChange(string.Format("Checking in \"{0}\"...", domainObject.ToString()), count, 0, max);

                        using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Auto, true))
                        {
                            CheckInDomainObject(domainObject.Id, domainObject.GetType(), ModelService.GetApplicationForDomainObject(domainObject), true, true);
                        }

                        count++;
                    }

                    RepositoryService.CommitCheckIn();

                    RaiseStatusChange("Check in completed successfully.", max, 0, max);
                }
                catch (Exception ex)
                {
                    RepositoryService.RollbackCheckIn();

                    throw ex;
                }
            }
        }

        private void CheckInAtomic(IList<IVersionControlled> domainObjects)
        {
            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Auto, true))
            {
                int max = domainObjects.Count + 2;

                RaiseStatusChange("Starting check in...", 0, 0, max);

                RepositoryService.BeginCheckIn(domainObjects);

                bool doRollback = true;

                try
                {
                    int count = 0;

                    foreach (IVersionControlled domainObject in domainObjects)
                    {
                        RaiseStatusChange(string.Format("Checking in \"{0}\"...", domainObject.ToString()), count, 0, max);
                        CheckInDomainObject(domainObject.Id, ModelService.GetDomainObjectType(domainObject), ModelService.GetApplicationForDomainObject(domainObject), true, false);
                        count++;
                    }

                    RaiseStatusChange("Committing check in...", count, 0, max);

                    IList<IVersionControlled> commitedItems = RepositoryService.CommitCheckIn();
                    doRollback = false;

                    count++;

                    foreach (IVersionControlled domainObject in commitedItems)
                    {
                        if (domainObject.State != VersionControlledObjectStat.Deleted)
                        {
                            RaiseStatusChange(string.Format("Unlocking \"{0}\"...", domainObject.ToString()), count, 0, max);
                            UnlockDomainObject((IVersionControlled)ModelService.GetDomainObject(domainObject.Id, domainObject.GetType()));
                        }
                    }

                    count++;

                    RaiseStatusChange("Check in completed successfully.", max, 0, max);
                }
                catch
                {
                    if (doRollback)
                    {
                        RaiseStatusChange("Error checking in, rolling back changes...", 0, 0, 1);
                        RepositoryService.RollbackCheckIn();
                    }

                    throw;
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public void CheckOutDomainObject(Guid id, Type classType)
        {
            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(classType))
            {
                DataAccess.IVersionControlled domainObject = (DataAccess.IVersionControlled)ModelService.GetDomainObject(id, classType);

                if (!domainObject.IsLocked || (domainObject.IsLocked && domainObject.LockedBy == Environment.UserName))
                {
                    if (!domainObject.IsLocked)
                    {
                        try
                        {
                            domainObject.IsLocked = true;
                            domainObject.LockedBy = Environment.UserName;
                            domainObject.LockedDate = DateTime.Now;

                            ModelService.SaveDomainObject(domainObject);

                            ObjectChanged(id, ModelService.GetDomainObjectType(domainObject));
                        }
                        catch (Exception ex)
                        {
                            throw new ConfigurationManagementException(string.Format("Unable to check out \"{0}\".", domainObject.ToString()), ex);
                        }
                    }

                    return;
                }

                string name = string.Empty;

                PropertyInfo pi = ModelService.GetDomainObjectType(domainObject).GetProperty("Name");

                if (pi != null)
                {
                    name = " - \"" + pi.GetValue(domainObject, null).ToString() + "\"";
                }

                throw new ConfigurationManagementException(string.Format("\"{0}\"{1} is already checked out by user: \"{2}\".", domainObject.ToString(), name, domainObject.LockedBy));
            }

            throw new ConfigurationManagementException(string.Format("Object type \"{0}\" is not under version control.", classType.Name));
        }

        [Transaction(ReadOnly = true)]
        public void RemoveDomainObject(Guid id, Type classType, DataAccess.Domain.Application application)
        {
            string errorMessage;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(classType))
            {
                DataAccess.IVersionControlled domainObject = (DataAccess.IVersionControlled)ModelService.GetInitializedDomainObject(id, classType);

                CheckAccess(domainObject);

                if (!RepositoryService.RemoveFile(domainObject, application, out errorMessage))
                {
                    throw new ConfigurationManagementException(errorMessage);
                }
            }
            else
            {
                throw new ConfigurationManagementException(string.Format("Object type \"{0}\" is not under version control.", classType.Name));
            }
        }

        public void PrepareCheckIn(int totalNumberOfFiles, int sequence)
        {
            string fileName = Path.GetTempPath() + "DeployMetamanagerCheckin.dat";

            if (!System.IO.File.Exists(fileName))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                {
                    fs.Close();
                }
            }
            else
            {
                System.IO.File.WriteAllText(fileName, string.Empty);
            }

            FileStream file_Stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            StreamWriter fileWriter = new StreamWriter(file_Stream);

            try
            {

                fileWriter.BaseStream.Seek(0, SeekOrigin.End);
                fileWriter.WriteLine(sequence);
                fileWriter.WriteLine(totalNumberOfFiles);

            }
            catch (IOException ex)
            {
                throw new ConfigurationManagementException(ex.Message, ex.InnerException);
            }
            finally
            {
                fileWriter.Close();
            }
        }

        [Transaction(ReadOnly = false)]
        public void CheckInDomainObject(Guid id, Type classType, DataAccess.Domain.Application application, bool doCheckIn = true, bool doUnlock = true)
        {
            string errorMessage;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(classType))
            {
                DataAccess.IVersionControlled domainObject = null;

                try
                {
                    domainObject = (DataAccess.IVersionControlled)ModelService.GetDomainObject(id, classType);

                    CheckAccess(domainObject);

                    //To avoid proxytype
                    classType = ModelService.GetDomainObjectType(domainObject);

                    if (domainObject.State == VersionControlledObjectStat.Deleted)
                    {
                        ModelService.DeleteDomainObjectAtCheckIn(domainObject);
                    }
                    else
                    {
                        if (domainObject.State == VersionControlledObjectStat.New && doCheckIn)
                        {
                            domainObject.State = VersionControlledObjectStat.Default;
                            ModelService.SaveDomainObject(domainObject);
                        }
                        else if (domainObject.State == VersionControlledObjectStat.Default && doCheckIn)
                        {
                            if (CheckForNewObjectReference(classType, domainObject))
                            {
                                throw new ConfigurationManagementException(string.Format("Unable to check in \"{0}\" . Reference exists to a new unchecked in object.", domainObject.ToString()));
                            }
                        }

                        //Check out file
                        System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
                        string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
                        string parentPath = System.IO.Path.Combine(rootPath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
                        string filePath = System.IO.Path.Combine(parentPath, classType.Name);
                        string fileName = domainObject.RepositoryFileName + ".xml";
                        string fullPath = System.IO.Path.Combine(filePath, fileName);

                        if (RepositoryService.CheckOutFile(domainObject, application, out errorMessage))
                        {
                            //Serialize 
                            XmlWriterSettings xmlSettings = new XmlWriterSettings();
                            xmlSettings.Indent = true;

                            XmlSerializer xmlSerializer = new XmlSerializer(classType);
                            XmlWriter writer = XmlWriter.Create(fullPath, xmlSettings);
                            xmlSerializer.Serialize(writer, domainObject);
                            writer.Close();

                            if (doCheckIn)
                            {
                                //Check in file
                                if (RepositoryService.CheckInFile(domainObject, application, out errorMessage))
                                {
                                    // If filesize is zero (clearcase checkin error), then checkout and checkin
                                    FileInfo fi = new FileInfo(fullPath);
                                    if (fi.Length == 0) 
                                    {
                                        CheckInDomainObject(id, classType, application, true, false);
                                    }

                                    if (doUnlock)
                                    {
                                        UnlockDomainObject(domainObject);
                                    }
                                }
                                else
                                {
                                    throw new ConfigurationManagementException(string.Format("Unable to check in \"{0}\".\r\n{1}", domainObject.ToString(), errorMessage));
                                }
                            }
                            else
                            {
                                if (doUnlock)
                                {
                                    UnlockDomainObject(domainObject);
                                }
                            }
                        }
                        else
                        {
                            throw new ConfigurationManagementException(string.Format("Unable to check in \"{0}\".\r\n{1}", domainObject.ToString(), errorMessage));
                        }
                    }
                }
                catch (ConfigurationManagementException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (domainObject == null)
                    {
                        throw new ConfigurationManagementException(string.Format("Error checking in object \"{0}\".", id), ex);
                    }
                    else
                    {
                        throw new ConfigurationManagementException(string.Format("Unable to check in \"{0}\".", domainObject.ToString()), ex);
                    }
                }
            }
            else
            {
                throw new ConfigurationManagementException(string.Format("Object type \"{0}\" is not under version control.", classType.Name));
            }
        }

        private void UnlockDomainObject(DataAccess.IVersionControlled domainObject)
        {
            domainObject = (IVersionControlled)ModelService.GetDomainObject(domainObject.Id, ModelService.GetDomainObjectType(domainObject));
            domainObject.IsLocked = false;
            domainObject.LockedBy = null;
            domainObject.LockedDate = null;

            ModelService.SaveDomainObject(domainObject);

            ObjectChanged(domainObject.Id, domainObject.GetType());
        }

        private bool CheckForNewObjectReference(Type classType, IDomainObject domainObject)
        {
            bool returnValue = false;

            foreach (PropertyInfo pi in classType.GetProperties())
            {
                if (typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType))
                {
                    object obj = pi.GetValue(domainObject, null);
                    if (obj != null)
                    {
                        if (((IVersionControlled)obj).State == VersionControlledObjectStat.New)
                        {
                            returnValue = true;
                            break;
                        }
                    }
                }
                else if (pi.PropertyType.Name == typeof(IList<object>).Name)
                {
                    if (!typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]) && typeof(IDomainObject).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]))
                    {
                        foreach (object subObj in ((System.Collections.ICollection)pi.GetValue(domainObject, null)))
                        {
                            if (subObj != null)
                            {
                                returnValue = CheckForNewObjectReference(ModelService.GetDomainObjectType(((IDomainObject)subObj)), ((IDomainObject)subObj));
                            }
                            if (returnValue) { break; }
                        }

                        if (returnValue) { break; }
                    }
                }
            }

            return returnValue;
        }

        [Transaction(ReadOnly = false)]
        public object GetDomainObjectFromConfMgn(Guid id, Type classType, DataAccess.Domain.Application application, bool dontsave = false)
        {
            bool? appFrontend = false;

            try
            {
                if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(classType))
                {
                    DataAccess.IVersionControlled domainObject = null;

                    if (!dontsave)
                    {
                        domainObject = (DataAccess.IVersionControlled)ModelService.GetInitializedDomainObject(id, classType);
                        //To avoid proxytype
                        classType = NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject);

                        appFrontend = application.IsFrontend;
                    }
                    else
                    {
                        domainObject = (IVersionControlled)System.Reflection.Assembly.GetAssembly(classType).CreateInstance(classType.FullName);
                        domainObject.Id = id;

                        if (classType == typeof(DataAccess.Domain.Application))
                        {
                            if (domainObject.Id == application.Id)
                            {
                                appFrontend = application.IsFrontend;
                            }
                            else
                            {
                                appFrontend = !application.IsFrontend;
                            }
                        }
                        else
                        {
                            appFrontend = ModelService.ClassTypeBelongToFrontend(classType);
                        }

                        if (appFrontend == null)
                        {
                            throw new ConfigurationManagementException("Unable to determine application type for object type " + classType.Name + " - GUID: " + id.ToString());
                        }
                    }

                    if (dontsave || (domainObject.IsLocked && domainObject.LockedBy == Environment.UserName))
                    {
                        System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
                        string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
                        string parentPath = System.IO.Path.Combine(rootPath, application.Name + (appFrontend.Value ? "_Frontend" : "_Backend"));
                        string filePath = System.IO.Path.Combine(parentPath, classType.Name);
                        string fileName = domainObject.RepositoryFileName + ".xml";
                        string fullPath = System.IO.Path.Combine(filePath, fileName);

                        return GetDomainObjectFromConfMgn(fullPath, classType, dontsave);
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        [Transaction(ReadOnly = false)]
        public object GetDomainObjectFromConfMgn(string path, Type classType, bool dontsave = false)
        {
            string fileContent = string.Empty;

            if (DataAccess.DomainXmlSerializationHelper.UseFileCache)
            {
                lock (DataAccess.DomainXmlSerializationHelper.XmlFileCache)
                {
                    if (DataAccess.DomainXmlSerializationHelper.XmlFileCache.ContainsKey(path))
                    {
                        fileContent = DataAccess.DomainXmlSerializationHelper.XmlFileCache[path];
                    }
                }
            }

            if (string.IsNullOrEmpty(fileContent))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                if (fi.Exists)
                {
                    fileContent = fi.OpenText().ReadToEnd();
                }
            }


            return GetDomainObjectFromConfMgn(classType, fileContent, dontsave);
        }

        [Transaction(ReadOnly = false)]
        public object GetDomainObjectFromConfMgn(Type classType, string fileContent, bool dontsave = false)
        {
            lock (DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
            {
                DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects.Clear();
            }

            if (!string.IsNullOrEmpty(fileContent))
            {
                System.Xml.Serialization.XmlSerializer xmlSerie = new System.Xml.Serialization.XmlSerializer(classType);

                System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new System.IO.StringReader(fileContent));

                object domainObject = xmlSerie.Deserialize(reader);

                if (!dontsave)
                {
                    if (ModelService.GetDomainObject(((IDomainObject)domainObject).Id, ModelService.GetDomainObjectType((IDomainObject)domainObject)) != null)
                    {
                        bool sync = true;
                        //fix for search panel
                        if (ModelService.GetDomainObjectType((IDomainObject)domainObject) == typeof(DataAccess.Domain.View))
                        {
                            if (((DataAccess.Domain.View)domainObject).RequestMap.Id == ((DataAccess.Domain.View)domainObject).ResponseMap.Id)
                            {
                                sync = false;
                                DataAccess.Domain.PropertyMap tmpMap = ModelService.GetInitializedDomainObject<DataAccess.Domain.PropertyMap>(((DataAccess.Domain.View)domainObject).RequestMap.Id);
                                ((DataAccess.Domain.View)domainObject).RequestMap = tmpMap;
                                ((DataAccess.Domain.View)domainObject).ResponseMap = tmpMap;
                            }
                        }

                        if (sync)
                        {
                            ModelService.StartSynchronizePropertyMapsInObjects((IDomainObject)domainObject);
                        }
                    }

                    ModelService.MergeSaveDomainObject((IDomainObject)domainObject);

                    foreach (IDomainObject obj in DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
                    {
                        ModelService.MergeSaveDomainObject(obj);
                    }

                }

                return domainObject;
            }

            return null;
        }


        [Transaction(ReadOnly = false)]
        public void UndoCheckOutDomainObject(Guid id, Type classType, Cdc.MetaManager.DataAccess.Domain.Application application)
        {
            IVersionControlled domainObject = null;

            try
            {
                domainObject = (DataAccess.IVersionControlled)ModelService.GetInitializedDomainObject(id, classType);

                CheckAccess(domainObject);

                if (domainObject.State == VersionControlledObjectStat.New)
                {
                    throw new ConfigurationManagementException(string.Format("Unable to undo check out for {0} since it's a new object. Use delete instead.", domainObject.ToString()));
                }

                GetDomainObjectFromConfMgn(id, classType, application);

                UnlockDomainObject(domainObject);
            }
            catch (ConfigurationManagementException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (domainObject == null)
                {
                    throw new ConfigurationManagementException(string.Format("Unable to undo check out for object \"{0}\".", id), ex);
                }
                else
                {
                    throw new ConfigurationManagementException(string.Format("Unable to undo check out for \"{0}\".", domainObject.ToString()), ex);
                }
            }
        }

        private static void CheckAccess(IVersionControlled domainObject)
        {
            if (!domainObject.IsLocked)
            {
                throw new ConfigurationManagementException(string.Format("\"{0}\" is not checked out.", domainObject.ToString()));
            }
            else if (domainObject.LockedBy != Environment.UserName)
            {
                throw new ConfigurationManagementException(string.Format("\"{0}\" is locked by another user: \"{1}\".", domainObject.ToString(), domainObject.LockedBy));
            }
        }

        [Transaction(ReadOnly = false)]
        public void ImportDomainObjects(List<string> paths, bool checkout, bool excludeZeroSizeFiles)
        {
            Dictionary<Type, Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>> importedObjects = null;
            Dictionary<Guid, Type> idsToImport = null;

            if (!DataAccess.DomainXmlSerializationHelper.DontUseDBorSession)
            {
                importedObjects = new Dictionary<Type, Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>>();
                idsToImport = new Dictionary<Guid, Type>();
            }

            DeserializeFilesForImport(paths, importedObjects, idsToImport, excludeZeroSizeFiles);

            if (DataAccess.DomainXmlSerializationHelper.DontUseDBorSession)
            {
                System.Threading.Tasks.Parallel.ForEach(DataAccess.DomainXmlSerializationHelper.MissingReferenses, m =>
                    {
                        if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(m.RefObjectType))
                        {
                            if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[m.RefObjectType].ContainsKey(m.RefObjectId))
                            {
                                IDomainObject refObj = DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[m.RefObjectType][m.RefObjectId];

                                if (m.MethodOrPropertyInfo is MethodInfo)
                                {
                                    if (((MethodInfo)m.MethodOrPropertyInfo).Name == "Add")
                                    {
                                        lock (m.TargetObject)
                                        {
                                            ((MethodInfo)m.MethodOrPropertyInfo).Invoke(m.TargetObject, new object[] { refObj });
                                        }
                                    }
                                }
                                else if (m.MethodOrPropertyInfo is PropertyInfo)
                                {
                                    ((PropertyInfo)m.MethodOrPropertyInfo).SetValue(m.TargetObject, refObj, null);
                                }
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                    });

                RaiseStatusChange("Filling parent child lists", 0, 0, 0);

                System.Threading.Tasks.Parallel.ForEach(ModelService.GetAllVersionControlledTypes(), t =>
                    {
                        if (DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.ContainsKey(t))
                        {
                            List<PropertyInfo> refListPropList = t.GetProperties().Where(p => p.PropertyType.IsGenericType
                                                                                           && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)
                                                                                           && p.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true).Length > 0).ToList();
                            if (refListPropList.Count > 0)
                            {
                                foreach (IVersionControlled parent in DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[t].Values)
                                {
                                    if (DataAccess.DomainXmlSerializationHelper.PossibleParentReferences[t].ContainsKey(parent.Id))
                                    {
                                        foreach (PropertyInfo refListProp in refListPropList)
                                        {
                                            Type refListGenericType = refListProp.PropertyType.GetGenericArguments()[0];
                                            if (DataAccess.DomainXmlSerializationHelper.PossibleParentReferences[t][parent.Id].ContainsKey(refListGenericType))
                                            {
                                                lock (DataAccess.DomainXmlSerializationHelper.PossibleParentReferences)
                                                {
                                                    object listobj = refListProp.GetValue(parent, null);

                                                    foreach (Guid refId in DataAccess.DomainXmlSerializationHelper.PossibleParentReferences[t][parent.Id][refListGenericType])
                                                    {
                                                        if (!((bool)listobj.GetType().GetMethod("Contains").Invoke(listobj, new object[] { DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[refListGenericType][refId] })))
                                                        {
                                                            listobj.GetType().GetMethod("Add").Invoke(listobj, new object[] { DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[refListGenericType][refId] });
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });

                RaiseStatusChange("Import done.", 0, 0, 0);
            }
            else
            {
                ProcessAndSaveObjectsForImport(checkout, importedObjects, idsToImport);

                RaiseStatusChange("Import done. Waiting for database commit.", 0, 0, 0);
            }
        }

        private void DeserializeFilesForImport(List<string> paths, Dictionary<Type, Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>> importedObjects, Dictionary<Guid, Type> IdsToImport, bool excludeZeroSizeFiles)
        {
            Dictionary<Type, List<string>> filesToImport = new Dictionary<Type, List<string>>();
            int numberOfFiles = 0;
            int progress = 0;

            RaiseStatusChange("Getting all files to import", 0, 0, 0);

            foreach (string filepath in paths)
            {
                FileInfo fi = new FileInfo(filepath);
                DirectoryInfo di = new DirectoryInfo(filepath);

                if (!fi.Exists && di.Exists)
                {
                    numberOfFiles += GetAllFilesRecursive(filepath, ref filesToImport, excludeZeroSizeFiles);
                }
                else
                {
                    if (!excludeZeroSizeFiles || (excludeZeroSizeFiles && fi.Length > 0))
                    {
                        Type classType = GetClassTypeAndIdForXML(filepath).Key;

                        if (!filesToImport.ContainsKey(classType))
                        {
                            filesToImport.Add(classType, new List<string>());
                        }

                        filesToImport[classType].Add(filepath);
                        numberOfFiles++;
                    }
                }
            }

            if (!DataAccess.DomainXmlSerializationHelper.DontUseDBorSession)
            {
                foreach (Type classType in filesToImport.Keys)
                {
                    if (!importedObjects.ContainsKey(classType))
                    {
                        importedObjects.Add(classType, new Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>());
                    }

                    foreach (string fileToImport in filesToImport[classType])
                    {
                        progress++;

                        if (progress % 50 == 0)
                        {
                            RaiseStatusChange("Deserializing file: " + Path.GetFileName(fileToImport), progress, 0, numberOfFiles);
                        }

                        // Get the domainobject from xml file
                        DataAccess.IDomainObject domainObject = ((DataAccess.IVersionControlled)GetDomainObjectFromConfMgn(fileToImport, classType, true));


                        importedObjects[classType].Add(domainObject.Id, new KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>(domainObject, new SortedList<Guid, IDomainObject>()));
                        IdsToImport.Add(domainObject.Id, classType);

                        foreach (IDomainObject visualCompRefObj in DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
                        {
                            if (!importedObjects.ContainsKey(visualCompRefObj.GetType()))
                            {
                                importedObjects.Add(visualCompRefObj.GetType(), new Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>());
                            }
                            if (!importedObjects[visualCompRefObj.GetType()].ContainsKey(visualCompRefObj.Id))
                            {
                                importedObjects[visualCompRefObj.GetType()].Add(visualCompRefObj.Id, new KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>(visualCompRefObj, new SortedList<Guid, IDomainObject>()));
                                IdsToImport.Add(visualCompRefObj.Id, visualCompRefObj.GetType());
                            }
                        }
                    }
                }
            }
            else
            {
                object lockObj = new object();

                System.Threading.Tasks.Parallel.ForEach(filesToImport.Keys, t =>
                {
                    System.Threading.Tasks.Parallel.ForEach(filesToImport[t], f =>
                    {
                        lock (lockObj)
                        {
                            progress++;

                            if (progress % 50 == 0)
                            {
                                RaiseStatusChange("Deserializing file: " + Path.GetFileName(f), progress, 0, numberOfFiles);
                            }
                        }

                        // Get the domainobject from xml file
                        GetDomainObjectFromConfMgn(f, t, true);
                    });
                });
            }
        }

        private void ProcessAndSaveObjectsForImport(bool checkout, Dictionary<Type, Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>> importedObjects, Dictionary<Guid, Type> idsToImport)
        {
            int progress = idsToImport.Count;
            int oldprogress = idsToImport.Count;
            bool objectNotSaved = true;

            Dictionary<KeyValuePair<Guid, Type>, Dictionary<PropertyInfo, KeyValuePair<Guid, Type>>> brokenReferences = new Dictionary<KeyValuePair<Guid, Type>, Dictionary<PropertyInfo, KeyValuePair<Guid, Type>>>();

            List<Guid> notSavedObjects = new List<Guid>();
            List<String> errorDescription = new List<string>();

            while (objectNotSaved)
            {
                notSavedObjects.Clear();
                errorDescription.Clear();
                objectNotSaved = false;
                oldprogress = progress;

                foreach (KeyValuePair<Guid, Type> IdType in idsToImport)
                {
                    if (importedObjects[IdType.Value][IdType.Key].Key != null)
                    {
                        RaiseStatusChange("Processing " + IdType.Value + " : " + IdType.Key, progress, 0, idsToImport.Count);

                        Dictionary<Guid, IDomainObject> recursionList = new Dictionary<Guid, IDomainObject>();

                        if (!TrySaveImportedObject(importedObjects[IdType.Value][IdType.Key].Key, importedObjects[IdType.Value][IdType.Key].Key, importedObjects[IdType.Value][IdType.Key].Key, importedObjects, recursionList, brokenReferences, checkout, errorDescription))
                        {
                            if (!notSavedObjects.Contains(IdType.Key))
                            {
                                notSavedObjects.Add(IdType.Key);
                            }
                        }
                        else
                        {
                            foreach (Guid id in recursionList.Keys)
                            {
                                if (notSavedObjects.Contains(id))
                                {
                                    notSavedObjects.Remove(id);
                                }
                            }
                        }

                        progress -= recursionList.Count;
                    }
                }

                if (notSavedObjects.Count > 0)
                {
                    objectNotSaved = true;
                }

                if (oldprogress == progress)
                {
                    throw new ConfigurationManagementException("Unable to import all objects.\r\n" + String.Join("\r\n", errorDescription));
                }
            }

            foreach (KeyValuePair<Guid, Type> brokenObjInfo in brokenReferences.Keys)
            {
                IDomainObject brokenObj = ModelService.GetDomainObject(brokenObjInfo.Key, brokenObjInfo.Value);

                if (brokenObj != null)
                {
                    foreach (KeyValuePair<PropertyInfo, KeyValuePair<Guid, Type>> propRefInfo in brokenReferences[brokenObjInfo])
                    {
                        IDomainObject refObj = ModelService.GetDomainObject(propRefInfo.Value.Key, propRefInfo.Value.Value);

                        if (refObj != null)
                        {
                            RaiseStatusChange(string.Format("Repairing reference from object \"{0}\" to \"{1} ({2})\".", brokenObj.ToString(), propRefInfo.Value.Value, propRefInfo.Value.Key), progress, 0, idsToImport.Count);
                            propRefInfo.Key.SetValue(brokenObj, refObj, null);
                        }
                        else
                        {
                            throw new ConfigurationManagementException(string.Format("Unable to find reference from object \"{0}\" to \"{1} ({2})\".", brokenObj.ToString(), propRefInfo.Value.Value, propRefInfo.Value.Key));
                        }
                    }
                }
                else
                {
                    throw new ConfigurationManagementException(string.Format("Unable to find object with id \"{0}\".", brokenObjInfo.Key));
                }
            }
        }


        [Transaction(ReadOnly = false)]
        public void ImportDomainObject(string fileName)
        {
            try
            {
                Type classType = GetClassTypeAndIdForXML(fileName).Key;
                // Get the domainobject from xml file
                DataAccess.IDomainObject domainObject = ((DataAccess.IVersionControlled)GetDomainObjectFromConfMgn(fileName, classType, true));

                // Check if the domain object already exists else it is a new object
                DataAccess.IDomainObject readDomainObjectfromDB = ((DataAccess.IVersionControlled)ModelService.GetDomainObject(domainObject.Id, classType));

                bool newObject = false;

                if (readDomainObjectfromDB == null)
                {
                    newObject = true;
                }

                if (!newObject)
                {
                    // check out domainobject
                    CheckOutDomainObject(domainObject.Id, domainObject.GetType());
                }

                ModelService.MergeSaveDomainObject((IDomainObject)domainObject);

                // check out domainobject

                CheckOutDomainObject(domainObject.Id, domainObject.GetType());

                if (newObject)
                {
                    ObjectAdded(domainObject.Id, classType);
                }
                else
                {
                    ObjectChanged(domainObject.Id, classType);
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationManagementException(string.Format("Unable to import object from file \"{0}\".", fileName), ex);
            }
        }

        public KeyValuePair<Type, Guid> GetClassTypeAndIdForXML(string xmlfilename)
        {
            Type classType;
            Guid id;

            string[] parts = Path.GetFileNameWithoutExtension(xmlfilename).Split('_');

            classType = Type.GetType("Cdc.MetaManager.DataAccess.Domain." + parts[0] + ",Cdc.MetaManager.DataAccess");
            id = new Guid(parts[1]);

            return new KeyValuePair<Type, Guid>(classType, id);
        }


        [Transaction(ReadOnly = false)]
        public void GetDomainFromConfMgn(string application)
        {
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
            string domainPath = System.IO.Path.Combine(rootPath, application);

            DirectoryInfo di = new DirectoryInfo(domainPath);

            if (di.Exists)
            {
                foreach (DirectoryInfo subDi in di.GetDirectories())
                {
                    //TODO
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public void AddDomainToConfMgn(DataAccess.Domain.DeploymentGroup deploymentGroup, bool doCheckIn, bool frontend, bool backend)
        {
            List<string> errorList = new List<string>();
            IList<IVersionControlled> allApplicationObjects = null;
            int progress;

            if (frontend)
            {
                allApplicationObjects = ModelService.GetAllVersionControlledObjectsInApplication(deploymentGroup.FrontendApplication.Id);
                RaiseStatusChange("Start checking out Frontend application objects", 0, 0, allApplicationObjects.Count);
                //Check out Frontend Application
                progress = 0;
                foreach (IVersionControlled vobj in allApplicationObjects)
                {
                    try
                    {
                        progress++;

                        if (vobj != null)
                        {
                            RaiseStatusChange("Checking out " + ModelService.GetDomainObjectType(vobj).Name + " : " + vobj.Id.ToString(), progress, 0, allApplicationObjects.Count);
                            CheckOutDomainObject(vobj.Id, ModelService.GetDomainObjectType(vobj));
                        }
                    }
                    catch (Exception ex)
                    {
                        errorList.Add(ex.Message);
                    }
                }

                if (errorList.Count == 0)
                {
                    RaiseStatusChange("Start checking in Frontend application objects", 0, 0, allApplicationObjects.Count);
                    progress = 0;
                    //Check in Frontend Application
                    foreach (IVersionControlled vobj in allApplicationObjects)
                    {
                        try
                        {
                            progress++;
                            if (vobj != null)
                            {
                                RaiseStatusChange("Checking in " + ModelService.GetDomainObjectType(vobj).Name + " : " + vobj.Id.ToString(), progress, 0, allApplicationObjects.Count);
                                CheckInDomainObject(vobj.Id, ModelService.GetDomainObjectType(vobj), deploymentGroup.FrontendApplication, doCheckIn);
                            }
                        }
                        catch (Exception ex)
                        {
                            errorList.Add(ex.Message);
                        }
                    }
                }
            }

            if (backend)
            {
                if (errorList.Count == 0)
                {
                    allApplicationObjects = ModelService.GetAllVersionControlledObjectsInApplication(deploymentGroup.BackendApplication.Id);
                    RaiseStatusChange("Start checking out Backend application objects", 0, 0, allApplicationObjects.Count);
                    progress = 0;

                    //check out Backend Application 

                    foreach (IVersionControlled vobj in allApplicationObjects)
                    {
                        try
                        {
                            progress++;
                            if (vobj != null)
                            {
                                RaiseStatusChange("Checking out " + ModelService.GetDomainObjectType(vobj).Name + " : " + vobj.Id.ToString(), progress, 0, allApplicationObjects.Count);
                                CheckOutDomainObject(vobj.Id, ModelService.GetDomainObjectType(vobj));
                            }
                        }
                        catch (Exception ex)
                        {
                            errorList.Add(ex.Message);
                        }
                    }
                }

                if (errorList.Count == 0)
                {
                    RaiseStatusChange("Start checking in Backend application objects", 0, 0, allApplicationObjects.Count);
                    progress = 0;
                    //check in Backend Application 
                    foreach (IVersionControlled vobj in allApplicationObjects)
                    {
                        try
                        {
                            progress++;
                            if (vobj != null)
                            {
                                RaiseStatusChange("Checking in " + ModelService.GetDomainObjectType(vobj).Name + " : " + vobj.Id.ToString(), progress, 0, allApplicationObjects.Count);
                                CheckInDomainObject(vobj.Id, ModelService.GetDomainObjectType(vobj), deploymentGroup.BackendApplication, doCheckIn);
                            }
                        }
                        catch (Exception ex)
                        {
                            errorList.Add(ex.Message);
                        }

                    }
                }
            }


            //Errors
            if (errorList.Count > 0)
            {
                string msg = string.Empty;

                foreach (string error in errorList)
                {
                    msg += error + "\r\n\r\n";
                }

                throw new ConfigurationManagementException(msg);
            }

            RaiseStatusChange("Add to source control done!", 100, 0, 100);
        }

        [Transaction(ReadOnly = true)]
        public Dictionary<Guid, IDomainObject> GetLatestVersionOfObjects(List<IDomainObject> objects, DataAccess.Domain.Application application, bool getAllFromCM = false, bool onlyResolveChildren = false, bool statusEvent = true)
        {
            Dictionary<Guid, IDomainObject> loadedObjects = new Dictionary<Guid, IDomainObject>();
            Dictionary<IDomainObject, Dictionary<PropertyInfo, Guid>> unSolvedReferences = new Dictionary<IDomainObject, Dictionary<PropertyInfo, Guid>>();

            int i = 0;

            foreach (IDomainObject obj in objects)
            {
                i++;

                if (statusEvent)
                {
                    RaiseStatusChange("Getting latest version of " + ModelService.GetDomainObjectType(obj).Name + " [" + obj.Id + "]", i, 0, objects.Count);
                }

                GetLatestVersionControlled(obj.Id, ModelService.GetDomainObjectType(obj), application, loadedObjects, getAllFromCM, unSolvedReferences, true, false);
            }

            if (application.IsFrontend.Value)
            {
                GetLatestVersionControlled(application.Menu.Id, typeof(DataAccess.Domain.Menu), application, loadedObjects, getAllFromCM, unSolvedReferences, true, false);
            }

            if (!onlyResolveChildren)
            {
                bool notSolvedNotFound = false;
                List<string> unSolvableReferences = new List<string>();

                i = 0;
                for (int index = 0; index < unSolvedReferences.Count; index++) //foreach (IDomainObject obj in unSolvedReferences.Keys)
                {
                    IDomainObject obj = unSolvedReferences.ElementAt(index).Key;
                    i++;
                    if (statusEvent)
                    {
                        RaiseStatusChange("Completing unsolved references in " + ModelService.GetDomainObjectType(obj).Name + " [" + obj.Id + "]", i, 0, unSolvedReferences.Count);
                    }

                    foreach (KeyValuePair<PropertyInfo, Guid> piId in unSolvedReferences[obj])
                    {

                        if (loadedObjects.ContainsKey(piId.Value))
                        {
                            piId.Key.SetValue(obj, loadedObjects[piId.Value], null);
                        }
                        else
                        {
                            IDomainObject refObj = ModelService.GetDomainObject(piId.Value, piId.Key.PropertyType);
                            IVersionControlled objectToGet = null;

                            if (typeof(IVersionControlled).IsAssignableFrom(piId.Key.PropertyType))
                            {
                                objectToGet = (IVersionControlled)refObj;
                            }
                            else
                            {
                                List<IDomainObject> parents = new List<IDomainObject>();
                                IList<IVersionControlled> vParents = ModelService.GetVersionControlledParent(refObj, out parents);
                                if (vParents.Count > 0)
                                {
                                    objectToGet = vParents[0];
                                }
                            }

                            if (objectToGet != null)
                            {
                                string objectToGetLockedBy = objectToGet.LockedBy;

                                if (!loadedObjects.ContainsKey(objectToGet.Id))
                                {
                                    GetLatestVersionControlled(objectToGet.Id, ModelService.GetDomainObjectType(objectToGet), application, loadedObjects, getAllFromCM, unSolvedReferences, false, false);
                                    //foreach (KeyValuePair<Guid, IDomainObject> kv in tmpObjects)
                                    //{
                                    //    if (!loadedObjects.ContainsKey(kv.Key))
                                    //    {
                                    //        loadedObjects.Add(kv.Key, kv.Value);
                                    //    }
                                    //}
                                }

                                if (loadedObjects.ContainsKey(refObj.Id))
                                {
                                    piId.Key.SetValue(obj, loadedObjects[refObj.Id], null);
                                }
                                else
                                {

                                    string refObjName = string.Empty;
                                    Type refObjType = ModelService.GetDomainObjectType(refObj);

                                    if (refObjType.GetProperty("Name") != null)
                                    {
                                        refObjName = refObjType.GetProperty("Name").GetValue(refObj, null) as string;
                                    }

                                    string infoText = "Can´t find " + refObjType.Name + " {" + (string.IsNullOrEmpty(refObjName) ? string.Empty : refObjName) + "} [" + refObj.Id.ToString() + "]";

                                    if (refObj.Id != objectToGet.Id)
                                    {
                                        string objectToGetName = string.Empty;
                                        Type objectToGetType = ModelService.GetDomainObjectType(objectToGet);
                                        if (objectToGetType.GetProperty("Name") != null)
                                        {
                                            objectToGetName = objectToGetType.GetProperty("Name").GetValue(objectToGet, null) as string;
                                        }

                                        infoText += " in " + objectToGetType.Name + " {" + (string.IsNullOrEmpty(objectToGetName) ? string.Empty : objectToGetName) + "} [" + objectToGet.Id.ToString() + "]";
                                    }

                                    if (!string.IsNullOrEmpty(objectToGetLockedBy))
                                    {
                                        infoText += " Checked out by: " + objectToGetLockedBy;
                                    }

                                    unSolvableReferences.Add(infoText);
                                    notSolvedNotFound = true;
                                }
                            }
                            else
                            {
                                string refObjName = string.Empty;
                                Type refObjType = ModelService.GetDomainObjectType(refObj);

                                if (refObjType.GetProperty("Name") != null)
                                {
                                    refObjName = refObjType.GetProperty("Name").GetValue(refObj, null) as string;
                                }

                                string infoText = "Can´t find owning object for " + refObjType.Name + " {" + (string.IsNullOrEmpty(refObjName) ? string.Empty : refObjName) + "} [" + refObj.Id.ToString() + "]";
                                unSolvableReferences.Add(infoText);
                                notSolvedNotFound = true;
                            }
                        }
                    }
                }

                if (notSolvedNotFound)
                {
                    string message = "Unable to resolve all references. The metadata is not in sync. \r\n";
                    foreach (string infoText in unSolvableReferences)
                    {
                        message += infoText + "\r\n";
                    }

                    throw new ConfigurationManagementException(message);
                }
            }

            return loadedObjects;
        }

        [Transaction(ReadOnly = false)]
        public void DiffWithPreviousVersion(IVersionControlled versionControledObject, DataAccess.Domain.Application application)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "MetaManagerDiffCompareFile.xml");
            if (System.IO.File.Exists(tempPath))
            {
                System.IO.File.Delete(tempPath);
            }

            Type classType = ModelService.GetDomainObjectType(versionControledObject);

            versionControledObject = (DataAccess.IVersionControlled)ModelService.GetInitializedDomainObject(versionControledObject.Id, classType);

            //Serialize 
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            //xmlSettings.NewLineHandling = NewLineHandling.None;

            XmlSerializer xmlSerializer = new XmlSerializer(classType);
            XmlWriter writer = XmlWriter.Create(tempPath, xmlSettings);
            xmlSerializer.Serialize(writer, versionControledObject);
            writer.Close();

            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
            string parentPath = System.IO.Path.Combine(rootPath, application.Name + (application.IsFrontend.Value ? "_Frontend" : "_Backend"));
            string filePath = System.IO.Path.Combine(parentPath, classType.Name);
            string fileName = versionControledObject.RepositoryFileName + ".xml";
            string fullPath = System.IO.Path.Combine(filePath, fileName);

            RepositoryService.DiffFiles(fullPath, tempPath);
        }

        [Transaction(ReadOnly = true)]
        public object GetSpecificVersionOfDomainObjectFromConfMgn(string path, string version, string repositoryPathForImport)
        {
            Type classType = GetClassTypeAndIdForXML(path).Key;

            object theObject = null;

            string fileContent = RepositoryService.GetSpecificVersionOfFile(path, version, repositoryPathForImport);

            theObject = GetDomainObjectFromConfMgn(classType, fileContent, true);

            return theObject;
        }

        [Transaction(ReadOnly = true)]
        public string GetCurrentBranch()
        {
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();

            return GetBranch(rootPath);
        }

        [Transaction(ReadOnly = true)]
        public string GetBranch(string repositoryPath)
        {
            return RepositoryService.GetBranchName(repositoryPath);
        }

        private IDomainObject GetLatestVersionControlled(Guid id, Type classType, DataAccess.Domain.Application application, Dictionary<Guid, IDomainObject> loadedObjects, bool getAllFromCM, Dictionary<IDomainObject, Dictionary<PropertyInfo, Guid>> unSolvedReferences, bool resolveReferenceLists, bool refFromMenuItem)
        {
            if (loadedObjects.ContainsKey(id))
            {
                return loadedObjects[id];
            }

            IDomainObject obj = null;

            if (!getAllFromCM)
            {
                obj = ModelService.GetDomainObject(id, classType);

                if (obj is IVersionControlled)
                {
                    if (((IVersionControlled)obj).IsLocked && ((IVersionControlled)obj).LockedBy != Environment.UserName)
                    {
                        //The object is checked out by someone else.
                        obj = (IDomainObject)GetDomainObjectFromConfMgn(id, classType, application, true);

                        foreach (IDomainObject UXcompObj in DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
                        {
                            GetLatestVersionOfReferences(UXcompObj, loadedObjects, application, getAllFromCM, unSolvedReferences, resolveReferenceLists, refFromMenuItem);
                        }
                    }
                }
            }
            else
            {
                obj = (IDomainObject)GetDomainObjectFromConfMgn(id, classType, application, true);
                foreach (IDomainObject UXcompObj in DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
                {
                    GetLatestVersionOfReferences(UXcompObj, loadedObjects, application, getAllFromCM, unSolvedReferences, resolveReferenceLists, refFromMenuItem);
                }
            }

            if (obj == null)
            {
                throw new ConfigurationManagementException("Can´t find " + classType.Name + " with id " + id.ToString());
            }

            return GetLatestVersionOfReferences(obj, loadedObjects, application, getAllFromCM, unSolvedReferences, resolveReferenceLists, refFromMenuItem);
        }

        private IDomainObject GetLatestVersionOfReferences(IDomainObject obj, Dictionary<Guid, IDomainObject> loadedObjects, DataAccess.Domain.Application application, bool getAllFromCM, Dictionary<IDomainObject, Dictionary<PropertyInfo, Guid>> unSolvedReferences, bool resolveReferenceLists, bool refFromMenuItem, List<Type> parentTypes = null)
        {
            List<IDomainObject> parentsToResetTransient = new List<IDomainObject>();

            if (obj == null)
            {
                return null;
            }

            if (loadedObjects.ContainsKey(obj.Id))
            {
                return loadedObjects[obj.Id];
            }

            loadedObjects.Add(obj.Id, obj);

            if (parentTypes == null)
            {
                parentTypes = new List<Type>();
                List<IDomainObject> parents = new List<IDomainObject>();
                ModelService.GetVersionControlledParent(obj, out parents);

                foreach (IDomainObject parent in parents)
                {
                    if (!parentTypes.Contains(ModelService.GetDomainObjectType(parent)))
                    {
                        parentTypes.Add(ModelService.GetDomainObjectType(parent));
                    }
                }
            }


            List<PropertyInfo> pis;
            IDomainObject childObj;

            foreach (PropertyInfo pi in ModelService.GetDomainObjectType(obj).GetProperties().Where(p => p.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length > 0))
            {
                if (typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType) && !parentTypes.Contains(pi.PropertyType))
                {
                    IVersionControlled child = (IVersionControlled)pi.GetValue(obj, null);
                    if (child != null)
                    {
                        //Check if its a menuitem then stop recursion from going deeper.
                        if (refFromMenuItem)
                        {
                            if (loadedObjects.ContainsKey(child.Id))
                            {
                                pi.SetValue(obj, loadedObjects[child.Id], null);
                            }
                            else
                            {
                                pi.SetValue(obj, ModelService.GetDomainObject(child.Id, ModelService.GetDomainObjectType(child)), null);
                            }
                        }
                        else
                        {
                            bool tmpRefFromMenuItem = refFromMenuItem;
                            if (ModelService.GetDomainObjectType(obj) == typeof(DataAccess.Domain.MenuItem))
                            {
                                tmpRefFromMenuItem = true;
                            }

                            pi.SetValue(obj, GetLatestVersionControlled(child.Id, ModelService.GetDomainObjectType(child), application, loadedObjects, getAllFromCM, unSolvedReferences, false, tmpRefFromMenuItem), null);

                        }
                    }

                }
                else if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]))
                {
                    if (resolveReferenceLists)
                    {
                        IList<IVersionControlled> newList = new List<IVersionControlled>();
                        IList<IVersionControlled> tmpList = ((IEnumerable<object>)pi.GetValue(obj, null)).Cast<IVersionControlled>().ToList();
                        if (tmpList != null)
                        {
                            foreach (IVersionControlled listChild in tmpList)
                            {
                                //Check if its a menuitem then stop recursion from going deeper.
                                if (refFromMenuItem)
                                {
                                    if (loadedObjects.ContainsKey(listChild.Id))
                                    {
                                        newList.Add((IVersionControlled)loadedObjects[listChild.Id]);
                                    }
                                    else
                                    {
                                        newList.Add((IVersionControlled)ModelService.GetDomainObject(listChild.Id, ModelService.GetDomainObjectType(listChild)));
                                    }
                                }
                                else
                                {
                                    bool tmpRefFromMenuItem = refFromMenuItem;
                                    if (ModelService.GetDomainObjectType(obj) == typeof(DataAccess.Domain.MenuItem))
                                    {
                                        tmpRefFromMenuItem = true;
                                    }

                                    newList.Add((IVersionControlled)GetLatestVersionControlled(listChild.Id, ModelService.GetDomainObjectType(listChild), application, loadedObjects, getAllFromCM, unSolvedReferences, false, tmpRefFromMenuItem));
                                }
                            }

                            object listobj = pi.GetValue(obj, null);
                            listobj.GetType().GetMethod("Clear").Invoke(listobj, null);

                            foreach (IVersionControlled listChild in newList)
                            {
                                listobj.GetType().GetMethod("Add").Invoke(listobj, new object[] { listChild });
                            }
                        }
                    }
                }
                else if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType) || (typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType) && parentTypes.Contains(pi.PropertyType)))
                {
                    childObj = (IDomainObject)pi.GetValue(obj, null);
                    if (childObj != null)
                    {
                        if (loadedObjects.ContainsKey(childObj.Id) && !loadedObjects[childObj.Id].IsTransient)
                        {
                            pi.SetValue(obj, loadedObjects[childObj.Id], null);
                        }
                        else
                        {
                            if (!unSolvedReferences.ContainsKey(obj))
                            {
                                unSolvedReferences.Add(obj, new Dictionary<PropertyInfo, Guid>());
                            }

                            if (!unSolvedReferences[obj].ContainsKey(pi))
                            {
                                unSolvedReferences[obj].Add(pi, childObj.Id);
                                if (parentTypes.Contains(pi.PropertyType))
                                {
                                    if (childObj.IsTransient)
                                    {
                                        childObj.IsTransient = false;
                                        parentsToResetTransient.Add(childObj);
                                    }
                                    pi.SetValue(obj, childObj, null);
                                }
                                else
                                {
                                    pi.SetValue(obj, null, null);
                                }
                            }
                        }
                    }
                }

            }


            //Children
            //=========================================================================================================================================
            bool thisAddedAsParent = false;
            if (!parentTypes.Contains(ModelService.GetDomainObjectType(obj)))
            {
                parentTypes.Add(ModelService.GetDomainObjectType(obj));
                thisAddedAsParent = true;
            }
            //Childproperties
            pis = ModelService.GetDomainObjectType(obj).GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType) &&
                                                      p.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true).Length == 0 &&
                                                      p.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length == 0).ToList();

            foreach (PropertyInfo pi in pis)
            {
                childObj = (IDomainObject)pi.GetValue(obj, null);
                childObj = GetLatestVersionOfReferences(childObj, loadedObjects, application, getAllFromCM, unSolvedReferences, resolveReferenceLists, refFromMenuItem, parentTypes);

                pi.SetValue(obj, childObj, null);
            }

            //Childlists
            pis = ModelService.GetDomainObjectType(obj).GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) &&
                                                      typeof(IDomainObject).IsAssignableFrom(p.PropertyType.GetGenericArguments()[0]) &&
                                                      p.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true).Length == 0 &&
                                                      p.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length == 0).ToList();

            foreach (PropertyInfo pi in pis)
            {

                object listobj = pi.GetValue(obj, null);

                if (listobj != null)
                {
                    IList<IDomainObject> tmpList = ((IEnumerable<object>)pi.GetValue(obj, null)).Cast<IDomainObject>().ToList();
                    listobj.GetType().GetMethod("Clear").Invoke(listobj, null);

                    foreach (object listObj in tmpList)
                    {
                        IDomainObject newRefObject = GetLatestVersionOfReferences((IDomainObject)listObj, loadedObjects, application, getAllFromCM, unSolvedReferences, resolveReferenceLists, refFromMenuItem, parentTypes);
                        if (newRefObject != null)
                        {
                            listobj.GetType().GetMethod("Add").Invoke(listobj, new object[] { newRefObject });
                        }
                    }


                }
            }

            if (thisAddedAsParent)
            {
                parentTypes.Remove(ModelService.GetDomainObjectType(obj));
            }

            //=========================================================================================================================================

            if (obj.IsTransient)
            {
                IDomainObject tmpobj = (IDomainObject)MetaManagerServices.GetCurrentSession().Merge(obj);
                if (tmpobj != obj)
                {
                    loadedObjects[obj.Id] = tmpobj;
                    if (unSolvedReferences.ContainsKey(obj))
                    {
                        unSolvedReferences.Add(tmpobj, unSolvedReferences[obj]);
                        unSolvedReferences.Remove(obj);
                    }

                    obj = tmpobj;
                }

            }

            foreach (IDomainObject transientParent in parentsToResetTransient)
            {
                transientParent.IsTransient = true;
            }

            return obj;
        }

        #endregion


        private bool TrySaveImportedObject(IDomainObject objectToSave, IDomainObject currentObject, IDomainObject parentObject, Dictionary<Type, Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>> importedObjects, Dictionary<Guid, IDomainObject> recursionList, Dictionary<KeyValuePair<Guid, Type>, Dictionary<PropertyInfo, KeyValuePair<Guid, Type>>> brokenReferences, bool checkout, List<String> ErrorDescriptions)
        {
            bool iOk = true;
            List<PropertyInfo> mappsToRerun = new List<PropertyInfo>();

            if (objectToSave == currentObject)
            {
                if (!recursionList.ContainsKey(objectToSave.Id))
                {
                    recursionList.Add(objectToSave.Id, objectToSave);
                }
            }

            List<PropertyInfo> pis = currentObject.GetType().GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType)).ToList();
            pis.Sort(OrderTypes);

            foreach (PropertyInfo pi in pis)
            {
                if (!iOk) { break; }

                object[] ignoreAttributes = pi.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true);

                if (ignoreAttributes.Length == 0)
                {

                    IDomainObject tmpObj = (IDomainObject)pi.GetValue(currentObject, null);

                    if (tmpObj != null && tmpObj != objectToSave && tmpObj != parentObject)
                    {


                        object[] serItemIdAttributes = pi.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);

                        if (serItemIdAttributes.Length > 0)
                        {
                            if (currentObject.GetType() != typeof(DataAccess.Domain.Application))
                            {
                                if (typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType))
                                {
                                    if (!recursionList.ContainsKey(tmpObj.Id) || recursionList[tmpObj.Id] == null)
                                    {
                                        if (importedObjects.ContainsKey(pi.PropertyType))
                                        {
                                            if (importedObjects[pi.PropertyType].ContainsKey(tmpObj.Id))
                                            {
                                                if (importedObjects[pi.PropertyType][tmpObj.Id].Key != null)
                                                {
                                                    iOk = iOk & TrySaveImportedObject(importedObjects[pi.PropertyType][tmpObj.Id].Key, importedObjects[pi.PropertyType][tmpObj.Id].Key, importedObjects[pi.PropertyType][tmpObj.Id].Key, importedObjects, recursionList, brokenReferences, checkout, ErrorDescriptions);
                                                }

                                                if (iOk)
                                                {
                                                    IDomainObject sessionObj = ModelService.GetDomainObject(tmpObj.Id, pi.PropertyType);
                                                    pi.SetValue(currentObject, sessionObj, null);
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                    else
                                    {

                                        object[] propertyStorageHintAttributes = pi.GetCustomAttributes(typeof(PropertyStorageHintAttribute), true);

                                        if ((propertyStorageHintAttributes.Length > 0 && !((PropertyStorageHintAttribute)propertyStorageHintAttributes[0]).IsMandatory) || propertyStorageHintAttributes.Length == 0)
                                        {

                                            KeyValuePair<Guid, Type> currObj = new KeyValuePair<Guid, Type>(currentObject.Id, currentObject.GetType());
                                            if (!brokenReferences.ContainsKey(currObj))
                                            {
                                                brokenReferences.Add(currObj, new Dictionary<PropertyInfo, KeyValuePair<Guid, Type>>());
                                            }
                                            if (!brokenReferences[currObj].ContainsKey(pi))
                                            {
                                                brokenReferences[currObj].Add(pi, new KeyValuePair<Guid, Type>(tmpObj.Id, pi.PropertyType));
                                            }

                                            pi.SetValue(currentObject, null, null);
                                        }
                                        else
                                        {
                                            if (objectToSave.Id == tmpObj.Id)
                                            {
                                                pi.SetValue(currentObject, objectToSave, null);
                                            }
                                            else
                                            {
                                                iOk = false;
                                                string text = "Mandatory versioncontrolled object missing. Object ID: " + tmpObj.Id + " , Type: " + tmpObj.GetType().ToString();
                                                if (!ErrorDescriptions.Contains(text))
                                                {
                                                    ErrorDescriptions.Add(text);
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if (!importedObjects[objectToSave.GetType()][objectToSave.Id].Value.ContainsKey(tmpObj.Id))
                                    {
                                        IDomainObject sessionObj = ModelService.GetDomainObject(tmpObj.Id, pi.PropertyType);
                                        if (sessionObj == null)
                                        {
                                            IDomainObject loadedObjEarlierInRecursion = GetLoadedObjectFromRecursionList(tmpObj.Id, pi.PropertyType, recursionList, importedObjects);
                                            if (loadedObjEarlierInRecursion != null)
                                            {
                                                object[] propertyStorageHintAttributes = pi.GetCustomAttributes(typeof(PropertyStorageHintAttribute), true);

                                                if ((propertyStorageHintAttributes.Length > 0 && !((PropertyStorageHintAttribute)propertyStorageHintAttributes[0]).IsMandatory) || propertyStorageHintAttributes.Length == 0)
                                                {
                                                    KeyValuePair<Guid, Type> currObj = new KeyValuePair<Guid, Type>(currentObject.Id, currentObject.GetType());
                                                    if (!brokenReferences.ContainsKey(currObj))
                                                    {
                                                        brokenReferences.Add(currObj, new Dictionary<PropertyInfo, KeyValuePair<Guid, Type>>());
                                                    }
                                                    if (!brokenReferences[currObj].ContainsKey(pi))
                                                    {
                                                        brokenReferences[currObj].Add(pi, new KeyValuePair<Guid, Type>(tmpObj.Id, pi.PropertyType));
                                                    }

                                                    pi.SetValue(currentObject, null, null);
                                                }
                                                else
                                                {
                                                    iOk = false;
                                                    string text = "Mandatory object missing. Object ID: " + tmpObj.Id + " , Type: " + tmpObj.GetType().ToString();
                                                    if (!ErrorDescriptions.Contains(text))
                                                    {
                                                        ErrorDescriptions.Add(text);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                iOk = false;
                                                string text = "Object not loaded earlier in recursion and missing from session. Object ID: " + tmpObj.Id + " , Type: " + tmpObj.GetType().ToString();
                                                if (!ErrorDescriptions.Contains(text))
                                                {
                                                    ErrorDescriptions.Add(text);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            pi.SetValue(currentObject, sessionObj, null);
                                        }
                                    }
                                    else
                                    {
                                        pi.SetValue(currentObject, importedObjects[objectToSave.GetType()][objectToSave.Id].Value[tmpObj.Id], null);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pi.PropertyType == typeof(DataAccess.Domain.PropertyMap))
                            {
                                if (!TrySaveImportedObject(objectToSave, tmpObj, currentObject, importedObjects, recursionList, brokenReferences, checkout, ErrorDescriptions))
                                {
                                    mappsToRerun.Add(pi);
                                }
                            }
                            else
                            {
                                iOk = iOk & TrySaveImportedObject(objectToSave, tmpObj, currentObject, importedObjects, recursionList, brokenReferences, checkout, ErrorDescriptions);
                            }
                        }
                    }
                }
            }

            if (iOk)
            {
                //Non reference child lists
                pis = currentObject.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) &&
                                                                                        typeof(IDomainObject).IsAssignableFrom(p.PropertyType.GetGenericArguments()[0]) &&
                                                                                        p.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true).Length == 0 &&
                                                                                        p.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length == 0).ToList();
                foreach (PropertyInfo pi in pis)
                {
                    foreach (object listObj in ((System.Collections.ICollection)pi.GetValue(currentObject, null)))
                    {
                        iOk = iOk & TrySaveImportedObject(objectToSave, (IDomainObject)listObj, currentObject, importedObjects, recursionList, brokenReferences, checkout, ErrorDescriptions);
                        if (!iOk) { break; }
                    }
                    if (!iOk) { break; }
                }
            }

            if (iOk)
            {
                foreach (PropertyInfo mappPi in mappsToRerun)
                {
                    IDomainObject tmpObj = (IDomainObject)mappPi.GetValue(currentObject, null);
                    iOk = iOk & TrySaveImportedObject(objectToSave, tmpObj, currentObject, importedObjects, recursionList, brokenReferences, checkout, ErrorDescriptions);
                }
            }


            if (iOk)
            {
                if (objectToSave == currentObject)
                {
                    //Fix for searchpanel
                    //=========================================================================================================================================================
                    if (objectToSave is DataAccess.Domain.View)
                    {
                        if (((DataAccess.Domain.View)objectToSave).RequestMap != null && ((DataAccess.Domain.View)objectToSave).ResponseMap != null)
                        {
                            DataAccess.Domain.PropertyMap tmpmap = ModelService.GetDomainObject<DataAccess.Domain.PropertyMap>(((DataAccess.Domain.View)objectToSave).RequestMap.Id);

                            if (((DataAccess.Domain.View)objectToSave).RequestMap.Id == ((DataAccess.Domain.View)objectToSave).ResponseMap.Id)
                            {
                                if (tmpmap != null)
                                {
                                    ((DataAccess.Domain.View)objectToSave).RequestMap = tmpmap;
                                    ((DataAccess.Domain.View)objectToSave).ResponseMap = tmpmap;
                                }
                                else
                                {
                                    ((DataAccess.Domain.View)objectToSave).ResponseMap = ((DataAccess.Domain.View)objectToSave).RequestMap;
                                }
                            }
                            else
                            {
                                if (tmpmap != null && tmpmap.IsTransient)
                                {
                                    ((DataAccess.Domain.View)objectToSave).RequestMap = tmpmap;
                                }
                            }
                        }
                    }
                    //=========================================================================================================================================================

                    //Fix UXAction
                    //=========================================================================================================================================================
                    if (objectToSave is DataAccess.Domain.UXAction)
                    {
                        if (((DataAccess.Domain.UXAction)objectToSave).RequestMap != null)
                        {
                            DataAccess.Domain.PropertyMap tmpmap = ModelService.GetDomainObject<DataAccess.Domain.PropertyMap>(((DataAccess.Domain.UXAction)objectToSave).RequestMap.Id);

                            if (tmpmap != null && tmpmap.IsTransient)
                            {
                                ((DataAccess.Domain.UXAction)objectToSave).RequestMap = tmpmap;
                            }
                        }
                    }
                    //=========================================================================================================================================================


                    IDomainObject testobj = ModelService.GetDomainObject(objectToSave.Id, objectToSave.GetType());

                    if (checkout)
                    {
                        if (typeof(IVersionControlled).IsAssignableFrom(objectToSave.GetType()))
                        {
                            ((IVersionControlled)objectToSave).IsLocked = true;
                            ((IVersionControlled)objectToSave).LockedBy = Environment.UserName;
                            ((IVersionControlled)objectToSave).LockedDate = DateTime.Now;

                            if (testobj == null)
                            {
                                ((IVersionControlled)objectToSave).State = VersionControlledObjectStat.New;
                            }
                        }
                    }

                    if (testobj != null)
                    {
                        ModelService.MergeSaveDomainObject(objectToSave);
                    }
                    else
                    {
                        ModelService.SaveDomainObject(objectToSave);
                    }

                    importedObjects[objectToSave.GetType()][objectToSave.Id] = new KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>(null, null);
                    recursionList[objectToSave.Id] = null;
                }
                else
                {
                    if (!importedObjects[objectToSave.GetType()][objectToSave.Id].Value.ContainsKey(currentObject.Id))
                    {
                        importedObjects[objectToSave.GetType()][objectToSave.Id].Value.Add(currentObject.Id, currentObject);
                    }
                }
            }
            else
            {
                if (objectToSave == currentObject)
                {
                    recursionList.Remove(objectToSave.Id);
                }
            }

            return iOk;
        }

        private int OrderTypes(PropertyInfo x, PropertyInfo y)
        {
            int xval = 0;
            int yval = 0;

            object[] xSerItemIdAttributes = x.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);
            object[] ySerItemIdAttributes = y.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);

            if (xSerItemIdAttributes.Length == 0)
            {
                xval = 1;
            }

            if (ySerItemIdAttributes.Length == 0)
            {
                yval = 1;
            }


            if (xval == yval)
            {
                if (x.PropertyType == typeof(DataAccess.Domain.PropertyMap))
                {
                    xval = -1;
                }
                if (y.PropertyType == typeof(DataAccess.Domain.PropertyMap))
                {
                    yval = -1;
                }
            }

            if (xval == yval)
            {
                xval = 0;
                yval = 0;
                object[] xPropertyMapAttributes = x.GetCustomAttributes(typeof(PropertyMapAttribute), true);
                object[] yPropertyMapAttributes = y.GetCustomAttributes(typeof(PropertyMapAttribute), true);

                if (xPropertyMapAttributes.Length > 0 && ((PropertyMapAttribute)xPropertyMapAttributes[0]).Type == PropertyMapType.Request)
                {
                    xval = 1;
                }
                if (yPropertyMapAttributes.Length > 0 && ((PropertyMapAttribute)yPropertyMapAttributes[0]).Type == PropertyMapType.Request)
                {
                    yval = 1;
                }

            }

            return yval - xval;

        }

        private IDomainObject GetLoadedObjectFromRecursionList(Guid Id, Type classType, Dictionary<Guid, IDomainObject> recursionList, Dictionary<Type, Dictionary<Guid, KeyValuePair<IDomainObject, SortedList<Guid, IDomainObject>>>> importedObjects)
        {
            foreach (KeyValuePair<Guid, IDomainObject> recListObj in recursionList)
            {
                if (recListObj.Key != recursionList.Last().Key)
                {
                    if (recListObj.Value != null)
                    {
                        if (importedObjects.ContainsKey(recListObj.Value.GetType()))
                        {
                            if (importedObjects[recListObj.Value.GetType()].ContainsKey(recListObj.Key))
                            {
                                if (importedObjects[recListObj.Value.GetType()][recListObj.Key].Value.ContainsKey(Id))
                                {
                                    if (importedObjects[recListObj.Value.GetType()][recListObj.Key].Value[Id].GetType() == classType)
                                    {
                                        return importedObjects[recListObj.Value.GetType()][recListObj.Key].Value[Id];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private int GetAllFilesRecursive(string path, ref Dictionary<Type, List<string>> filesToImport, bool excludeZeroSizeFiles)
        {
            DirectoryInfo di = new DirectoryInfo(Path.GetFullPath(path));
            FileInfo[] files = di.GetFiles("*.xml");
            int count = 0;

            foreach (FileInfo file in files)
            {
                if (!excludeZeroSizeFiles || (excludeZeroSizeFiles && file.Length > 0))
                {
                    Type classType = GetClassTypeAndIdForXML(file.FullName).Key;
                    if (!filesToImport.ContainsKey(classType))
                    {
                        filesToImport.Add(classType, new List<string>());
                    }
                    filesToImport[classType].Add(file.FullName);
                    count++;
                }
            }

            DirectoryInfo[] dirs = di.GetDirectories();

            foreach (DirectoryInfo dir in dirs)
            {
                count += GetAllFilesRecursive(dir.FullName, ref filesToImport, excludeZeroSizeFiles);
            }

            return count;
        }

        private void RaiseStatusChange(string message, int value, int min, int max)
        {
            if (StatusChanged != null)
            {
                StatusChanged(message, value, min, max);
            }
        }
    }
}
