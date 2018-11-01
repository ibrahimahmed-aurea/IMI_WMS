using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess;
using System.Reflection;
using System.Workflow.Activities.Rules;

namespace Cdc.MetaManager.BusinessLogic.Helpers
{
    class ImportChangeHelper : IImportChangeHelper
    {
        private IModelService modelService { get; set; }
        private IConfigurationManagementService configurationManagementService { get; set; }
        private IIssueManagementService issueManagementService { get; set; }


        private Dictionary<Type, Dictionary<Guid, IDomainObject>> CreatedObjectsLookUp = null;

        #region IImportChangeHelper Members

        [Transaction(ReadOnly = true)]
        public List<DeltaListEntry> GetChange(string issueId, string repositoryPathForImport, out string additionalInfo)
        {
            additionalInfo = string.Empty;
            CreatedObjectsLookUp = new Dictionary<Type, Dictionary<Guid, IDomainObject>>();

            List<DeltaListEntry> returnList = new List<DeltaListEntry>();

            //Get issue information from configuration management service.
            List<IssueFileInformation> issueFileInformationList = new List<IssueFileInformation>();

            string branchName = configurationManagementService.GetBranch(System.IO.Path.Combine(repositoryPathForImport, "metadata"));
            
            if (string.IsNullOrEmpty(branchName))
            {
                throw new Exception("The branch of the selected import view can not be determined");
            }

            issueFileInformationList = issueManagementService.GetFilesIncludedInIssue(issueId, branchName, out additionalInfo);

            Dictionary<Type, Dictionary<Guid, IDomainObject>> DomainObjectLookUpNewVersions = new Dictionary<Type, Dictionary<Guid, IDomainObject>>();
            Dictionary<Type, Dictionary<Guid, IDomainObject>> DomainObjectLookUpOldVersions = new Dictionary<Type, Dictionary<Guid, IDomainObject>>();

            List<MissingReference> MissingReferencesNewVersions = new List<MissingReference>();
            List<MissingReference> MissingReferencesOldVersions = new List<MissingReference>();

            List<KeyValuePair<KeyValuePair<IVersionControlled, IVersionControlled>, KeyValuePair<Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>, Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>>>> objectGetDeltaList = new List<KeyValuePair<KeyValuePair<IVersionControlled, IVersionControlled>, KeyValuePair<Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>, Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>>>>();

            foreach (IssueFileInformation issueFileInfo in issueFileInformationList)
            {
                string filePath = issueFileInfo.FilePath;
                string changeVersion = issueFileInfo.LatestVersionForIssue;
                string previousVersion = issueFileInfo.VersionPriorToIssue;

                Dictionary<Guid, KeyValuePair<IDomainObject, Guid>> newVisualComponentRefObjects = new Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>();
                Dictionary<Guid, KeyValuePair<IDomainObject, Guid>> oldVisualComponentRefObjects = new Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>();
                IVersionControlled newObject = null;
                IVersionControlled oldObject = null;

                try
                {
                    //DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                    DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp = DomainObjectLookUpNewVersions;
                    DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                    DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                    DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = true;
                    DataAccess.DomainXmlSerializationHelper.UseFileCache = true;

                    newObject = (IVersionControlled)configurationManagementService.GetSpecificVersionOfDomainObjectFromConfMgn(filePath, changeVersion, repositoryPathForImport);

                    foreach (IDomainObject refObj in DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
                    {
                        newVisualComponentRefObjects.Add(refObj.Id, new KeyValuePair<IDomainObject, Guid>(refObj, newObject.Id));
                    }

                    MissingReferencesNewVersions.AddRange(DataAccess.DomainXmlSerializationHelper.MissingReferenses);
                    //foreach (MissingReference missingRef in DataAccess.DomainXmlSerializationHelper.MissingReferenses)
                    //{
                    //    if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(missingRef.RefObjectType) && DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[missingRef.RefObjectType].ContainsKey(missingRef.RefObjectId))
                    //    {
                    //        ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[missingRef.RefObjectType][missingRef.RefObjectId], null);
                    //    }
                    //    else
                    //    {
                    //        IDomainObject newobj = (IDomainObject)System.Reflection.Assembly.GetAssembly(missingRef.RefObjectType).CreateInstance(missingRef.RefObjectType.FullName);
                    //        newobj.Id = missingRef.RefObjectId;
                    //        ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, newobj, null);
                    //    }
                    //}
                }
                finally
                {
                    //DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                    DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                    DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                    DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = false;
                    DataAccess.DomainXmlSerializationHelper.UseFileCache = false;
                }


                try
                {
                    //DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                    DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp = DomainObjectLookUpOldVersions;
                    //DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                    //DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                    DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = true;
                    DataAccess.DomainXmlSerializationHelper.UseFileCache = true;

                    while (oldObject == null)
                    {
                        DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                        DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                        oldObject = (IVersionControlled)configurationManagementService.GetSpecificVersionOfDomainObjectFromConfMgn(filePath, previousVersion, repositoryPathForImport);

                        if (oldObject == null)
                        {
                            int versionNr = Convert.ToInt32(previousVersion.Substring(previousVersion.LastIndexOf(@"\") + 1));
                            if (versionNr == 0)
                            {
                                break;
                            }
                            versionNr--;

                            previousVersion = previousVersion.Substring(0, previousVersion.LastIndexOf(@"\") + 1) + versionNr.ToString();
                        }
                    }



                    foreach (IDomainObject refObj in DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
                    {
                        oldVisualComponentRefObjects.Add(refObj.Id, new KeyValuePair<IDomainObject, Guid>(refObj, oldObject.Id));
                    }

                    MissingReferencesOldVersions.AddRange(DataAccess.DomainXmlSerializationHelper.MissingReferenses);
                    //foreach (MissingReference missingRef in DataAccess.DomainXmlSerializationHelper.MissingReferenses)
                    //{
                    //    if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(missingRef.RefObjectType) && DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[missingRef.RefObjectType].ContainsKey(missingRef.RefObjectId))
                    //    {
                    //        if (typeof(PropertyInfo).IsAssignableFrom(missingRef.MethodOrPropertyInfo.GetType()))
                    //        {
                    //            ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[missingRef.RefObjectType][missingRef.RefObjectId], null);
                    //        }
                    //        else
                    //        {
                    //            ((MethodInfo)missingRef.MethodOrPropertyInfo).Invoke(missingRef.TargetObject, new object[] { DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[missingRef.RefObjectType][missingRef.RefObjectId] });
                    //        }
                    //    }
                    //    else
                    //    {
                    //        IDomainObject newobj = (IDomainObject)System.Reflection.Assembly.GetAssembly(missingRef.RefObjectType).CreateInstance(missingRef.RefObjectType.FullName);
                    //        newobj.Id = missingRef.RefObjectId;
                    //        if (typeof(PropertyInfo).IsAssignableFrom(missingRef.MethodOrPropertyInfo.GetType()))
                    //        {
                    //            ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, newobj, null);
                    //        }
                    //        else
                    //        {
                    //            ((MethodInfo)missingRef.MethodOrPropertyInfo).Invoke(missingRef.TargetObject, new object[] { newobj });
                    //        }
                    //    }
                    //}
                }
                finally
                {
                    //DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                    DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                    DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                    DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = false;
                    DataAccess.DomainXmlSerializationHelper.UseFileCache = false;
                }

                if (oldObject != null || newObject != null)
                {
                    objectGetDeltaList.Add(new KeyValuePair<KeyValuePair<IVersionControlled, IVersionControlled>, KeyValuePair<Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>, Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>>>(new KeyValuePair<IVersionControlled, IVersionControlled>(oldObject, newObject), new KeyValuePair<Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>, Dictionary<Guid, KeyValuePair<IDomainObject, Guid>>>(oldVisualComponentRefObjects, newVisualComponentRefObjects)));
                }

            }

            for (int r = 0; r < 2; r++)
            {
                List<MissingReference> MissingReferences = null;
                Dictionary<Type, Dictionary<Guid, IDomainObject>> DomainObjectLookUp = null;

                if (r == 0)
                {
                    MissingReferences = MissingReferencesNewVersions;
                    DomainObjectLookUp = DomainObjectLookUpNewVersions;
                }
                else
                {
                    MissingReferences = MissingReferencesOldVersions;
                    DomainObjectLookUp = DomainObjectLookUpOldVersions;
                }

                foreach (MissingReference missingRef in MissingReferences)
                {
                    if (DomainObjectLookUp.ContainsKey(missingRef.RefObjectType) && DomainObjectLookUp[missingRef.RefObjectType].ContainsKey(missingRef.RefObjectId))
                    {
                        if (typeof(PropertyInfo).IsAssignableFrom(missingRef.MethodOrPropertyInfo.GetType()))
                        {
                            ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, DomainObjectLookUp[missingRef.RefObjectType][missingRef.RefObjectId], null);
                        }
                        else
                        {
                            ((MethodInfo)missingRef.MethodOrPropertyInfo).Invoke(missingRef.TargetObject, new object[] { DomainObjectLookUp[missingRef.RefObjectType][missingRef.RefObjectId] });
                        }
                    }
                    else
                    {
                        IDomainObject newobj = (IDomainObject)System.Reflection.Assembly.GetAssembly(missingRef.RefObjectType).CreateInstance(missingRef.RefObjectType.FullName);
                        newobj.Id = missingRef.RefObjectId;
                        if (typeof(PropertyInfo).IsAssignableFrom(missingRef.MethodOrPropertyInfo.GetType()))
                        {
                            ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, newobj, null);
                        }
                        else
                        {
                            ((MethodInfo)missingRef.MethodOrPropertyInfo).Invoke(missingRef.TargetObject, new object[] { newobj });
                        }
                    }
                }
            }

            for (int i = 0; i < objectGetDeltaList.Count; i++)
            {
                Dictionary<Guid, KeyValuePair<IDomainObject, Guid>> newVisualComponentRefObjects = objectGetDeltaList[i].Value.Value;
                Dictionary<Guid, KeyValuePair<IDomainObject, Guid>> oldVisualComponentRefObjects = objectGetDeltaList[i].Value.Key;
                IVersionControlled newObject = objectGetDeltaList[i].Key.Value;
                IVersionControlled oldObject = objectGetDeltaList[i].Key.Key;

                DeltaListEntry newDelta = GetChangeDelta(oldObject, newObject);

                bool componentMapsAdded = false;
                foreach (KeyValuePair<Guid, KeyValuePair<IDomainObject, Guid>> newRefObj in newVisualComponentRefObjects)
                {
                    IDomainObject oldRefObj = null;
                    if (oldVisualComponentRefObjects.ContainsKey(newRefObj.Key))
                    {
                        oldRefObj = oldVisualComponentRefObjects[newRefObj.Key].Key;
                    }

                    DeltaListEntry componentRefObjectDelta = GetChangeDelta(oldRefObj, newRefObj.Value.Key);
                    componentRefObjectDelta.OwnerId = newRefObj.Value.Value;

                    if (componentRefObjectDelta.Changes.Count > 0 || componentRefObjectDelta.ChildObjects.Count > 0 || (componentRefObjectDelta.CurrentObject_NewVersion != null && componentRefObjectDelta.CurrentObject_OldVersion == null))
                    {
                        returnList.Add(componentRefObjectDelta);
                        componentMapsAdded = true;
                    }
                }


                foreach (KeyValuePair<Guid, KeyValuePair<IDomainObject, Guid>> oldRefObj in oldVisualComponentRefObjects)
                {
                    if (!newVisualComponentRefObjects.ContainsKey(oldRefObj.Key))
                    {
                        DeltaListEntry delMapDelta = GetChangeDelta(oldRefObj.Value.Key, null);
                        delMapDelta.OwnerId = oldRefObj.Value.Value;
                        returnList.Add(delMapDelta);
                        componentMapsAdded = true;
                    }
                }

                if (newDelta.Changes.Count > 0 || newDelta.ChildObjects.Count > 0 || (newDelta.CurrentObject_NewVersion != null && newDelta.CurrentObject_OldVersion == null) || (newDelta.CurrentObject_NewVersion == null && newDelta.CurrentObject_OldVersion != null) || componentMapsAdded)
                {
                    returnList.Add(newDelta);
                }
            }
            return returnList;
        }

        private Dictionary<Type, Dictionary<Guid, IDomainObject>> CurrentDomainObjectLookUp = null;

        [Transaction(ReadOnly = true)]
        public List<string> GetCurrentVersionOfObjects(List<DeltaListEntry> changeDelta, DataAccess.Domain.Application frontendApplication, DataAccess.Domain.Application backendApplication)
        {
            List<string> errorList = new List<string>();
            try
            {
                for (int preview = 0; preview < 2; preview++)
                {
                    if (preview == 1)
                    {
                        foreach (DeltaListEntry delta in changeDelta)
                        {
                            if (!delta.PreviewMode)
                            {
                                delta.SwitchCurrentOjects();
                            }
                        }
                    }

                    DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                    DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                    DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                    DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = true;
                    DataAccess.DomainXmlSerializationHelper.UseFileCache = true;

                    for (int i = 0; i < changeDelta.Count; i++)
                    {
                        DeltaListEntry delta = changeDelta[i];

                        if (delta.CurrentObject_NewVersion != null)
                        {
                            if (typeof(IVersionControlled).IsAssignableFrom(delta.CurrentObject_NewVersion.GetType()))
                            {
                                IDomainObject newObject = (IDomainObject)delta.CurrentObject_NewVersion;

                                Type classType = newObject.GetType();

                                DataAccess.Domain.Application app;

                                if (modelService.ClassTypeBelongToFrontend(classType).Value)
                                {
                                    app = frontendApplication;
                                }
                                else
                                {
                                    app = backendApplication;
                                }


                                object currenObject = configurationManagementService.GetDomainObjectFromConfMgn(newObject.Id, newObject.GetType(), app, true);

                                if (currenObject != null)
                                {
                                    delta.CurrentObject_CurrentVersion = currenObject;
                                }
                            }
                        }
                    }

                    foreach (MissingReference missingRef in DataAccess.DomainXmlSerializationHelper.MissingReferenses)
                    {
                        if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(missingRef.RefObjectType) && DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[missingRef.RefObjectType].ContainsKey(missingRef.RefObjectId))
                        {
                            ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[missingRef.RefObjectType][missingRef.RefObjectId], null);
                        }
                        else
                        {
                            IDomainObject newobj = (IDomainObject)System.Reflection.Assembly.GetAssembly(missingRef.RefObjectType).CreateInstance(missingRef.RefObjectType.FullName);
                            newobj.Id = missingRef.RefObjectId;
                            ((PropertyInfo)missingRef.MethodOrPropertyInfo).SetValue(missingRef.TargetObject, newobj, null);
                        }
                    }

                    for (int i = 0; i < changeDelta.Count; i++)
                    {
                        DeltaListEntry delta = changeDelta[i];
                        if (delta.CurrentObject_NewVersion != null)
                        {
                            if (typeof(DataAccess.Domain.PropertyMap).IsAssignableFrom(delta.CurrentObject_NewVersion.GetType()))
                            {
                                //Componenet Maps
                                DataAccess.Domain.PropertyMap map = (DataAccess.Domain.PropertyMap)delta.CurrentObject_NewVersion;

                                if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(typeof(DataAccess.Domain.PropertyMap)))
                                {
                                    if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[typeof(DataAccess.Domain.PropertyMap)].ContainsKey(map.Id))
                                    {
                                        delta.CurrentObject_CurrentVersion = DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[typeof(DataAccess.Domain.PropertyMap)][map.Id];
                                    }
                                }
                            }
                            else if (delta.CurrentObject_CurrentVersion != null)
                            {
                                if (typeof(DataAccess.Domain.View).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType())) //Special handling of Search Panels
                                {
                                    if (((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion).RequestMap.Id == ((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion).ResponseMap.Id)
                                    {
                                        Guid mapId = ((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion).RequestMap.Id;

                                        if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(typeof(DataAccess.Domain.PropertyMap)) && DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[typeof(DataAccess.Domain.PropertyMap)].ContainsKey(mapId))
                                        {
                                            ((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion).RequestMap = (DataAccess.Domain.PropertyMap)DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[typeof(DataAccess.Domain.PropertyMap)][mapId];
                                            ((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion).ResponseMap = (DataAccess.Domain.PropertyMap)DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[typeof(DataAccess.Domain.PropertyMap)][mapId];
                                        }
                                        else
                                        {
                                            DataAccess.Domain.PropertyMap mapFromDB = modelService.GetInitializedDomainObject<DataAccess.Domain.PropertyMap>(mapId);

                                            ((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion).RequestMap = mapFromDB;
                                            ((DataAccess.Domain.View)delta.CurrentObject_CurrentVersion).ResponseMap = mapFromDB;

                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Recursivly fill all child objects from DomainObjectLookUp
                    foreach (DeltaListEntry delta in changeDelta)
                    {
                        if (delta.CurrentObject_CurrentVersion != null)
                        {
                            for (int i = 0; i < delta.ChildObjects.Count; i++)
                            {
                                DeltaListEntry childDelta = delta.ChildObjects[i];

                                if (typeof(IDomainObject).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                                {
                                    GetCurrentVersionOfObjectsRecursion(childDelta);
                                }
                                else if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                                {
                                    DataAccess.Domain.VisualModel.UXComponent currentComponent = (DataAccess.Domain.VisualModel.UXComponent)childDelta.ParentPropery.GetValue(delta.CurrentObject_CurrentVersion, null);

                                    childDelta.CurrentObject_CurrentVersion = currentComponent;
                                    List<string> componentTypeChange = new List<string>();
                                    GetCurrentVersionOfComponentsRecursion(childDelta, componentTypeChange);

                                    if (preview == 0)
                                    {
                                        foreach (string error in componentTypeChange)
                                        {
                                            errorList.Add("In " + delta.CurrentObject_NewVersion.GetType().Name + " " + delta.CurrentObject_NewVersion.GetType().GetProperty("Name").GetValue(delta.CurrentObject_NewVersion, null).ToString() + " Component " + error + " in issue prior to this.");
                                        }
                                    }
                                }
                                else if (typeof(RuleSet).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                                {
                                    if (typeof(IRuledObject).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                                    {
                                        RuleSet currentRule = ((IRuledObject)delta.CurrentObject_CurrentVersion).RuleSet;
                                        childDelta.CurrentObject_CurrentVersion = currentRule;

                                        GetCurrentVersionOfRule(childDelta);
                                    }
                                }
                            }
                        }
                    }

                    if (preview == 0)
                    {
                        CurrentDomainObjectLookUp = new Dictionary<Type, Dictionary<Guid, IDomainObject>>();
                        foreach (Type type in DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Keys)
                        {
                            CurrentDomainObjectLookUp.Add(type, new Dictionary<Guid, IDomainObject>());
                            foreach (KeyValuePair<Guid, IDomainObject> idObj in DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[type])
                            {
                                CurrentDomainObjectLookUp[type].Add(idObj.Key, idObj.Value);
                            }
                        }
                    }
                }

                foreach (DeltaListEntry delta in changeDelta)
                {
                    if (delta.PreviewMode)
                    {
                        delta.SwitchCurrentOjects();
                    }
                }
            }
            finally
            {
                DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = false;
                DataAccess.DomainXmlSerializationHelper.UseFileCache = false;
            }

            return errorList;
        }



        [Transaction(ReadOnly = true)]
        public void GetConflicts(List<DeltaListEntry> changeDelta)
        {
            foreach (DeltaListEntry listEntry in changeDelta)
            {
                if (listEntry.CurrentObject_CurrentVersion != null) //Changed object
                {
                    GetConflictsWithDelta(listEntry);
                }
            }
        }

        [Transaction(ReadOnly = true)]
        public List<DeltaListEntry> GetSortedDeltaList(List<DeltaListEntry> originalDeltaList, out Dictionary<DeltaListEntry, Dictionary<Guid, Type>> changedObjectsWithMissingReferences)
        {
            List<DeltaListEntry> sortedList = new List<DeltaListEntry>();

            List<sortStruct> sortingList = new List<sortStruct>();

            changedObjectsWithMissingReferences = new Dictionary<DeltaListEntry, Dictionary<Guid, Type>>();

            foreach (DeltaListEntry delta in originalDeltaList)
            {
                sortStruct newStruct = new sortStruct();

                newStruct.listEntry = delta;
                newStruct.createdAndDeletedIds = GetObjectIdsCreatedAndDeletedInChange(delta, CreatedObjectsLookUp); // GetObjectIdsCreatedForDomainObject((IDomainObject)delta.CurrentObject_NewVersion);
                newStruct.references = GetObjectIdsReferencedInChange(delta);  //GetObjectIdsReferencedByDomainObject((IDomainObject)delta.CurrentObject_NewVersion);

                sortingList.Add(newStruct);
            }

            int savedSortedListCount = -1;

            while (savedSortedListCount != sortedList.Count)
            {
                savedSortedListCount = sortedList.Count;

                for (int i = 0; i < sortingList.Count; i++)
                {
                    sortStruct currentItem = sortingList[i];

                    if (!currentItem.sorted)
                    {
                        bool okToSort = true;

                        foreach (KeyValuePair<Guid, Type> reference in currentItem.references)
                        {
                            bool createdByObjectInChange = false;
                            bool objectThatCreatesIsSorted = false;
                            bool existsInCurrentTrack = false;

                            if (!currentItem.createdAndDeletedIds.Contains(reference.Key))
                            {
                                foreach (sortStruct checkItem in sortingList)
                                {
                                    if (checkItem.createdAndDeletedIds.Contains(reference.Key))
                                    {
                                        createdByObjectInChange = true;

                                        if (checkItem.sorted || checkItem.listEntry.OwnerId == ((IDomainObject)currentItem.listEntry.CurrentObject_NewVersion).Id)
                                        {
                                            objectThatCreatesIsSorted = true;
                                        }

                                        break;
                                    }
                                }

                                IDomainObject checkObj = modelService.GetDomainObject(reference.Key, reference.Value);

                                if (checkObj != null)
                                {
                                    existsInCurrentTrack = true;
                                }


                                //Detecting missing references
                                if (!createdByObjectInChange && !existsInCurrentTrack)
                                {
                                    if (!changedObjectsWithMissingReferences.ContainsKey(currentItem.listEntry))
                                    {
                                        changedObjectsWithMissingReferences.Add(currentItem.listEntry, new Dictionary<Guid, Type>());
                                    }

                                    if (!changedObjectsWithMissingReferences[currentItem.listEntry].ContainsKey(reference.Key))
                                    {
                                        changedObjectsWithMissingReferences[currentItem.listEntry].Add(reference.Key, reference.Value);
                                    }
                                }

                                if (createdByObjectInChange)
                                {
                                    if (!objectThatCreatesIsSorted)
                                    {
                                        okToSort = false;
                                    }
                                }
                                else
                                {
                                    if (!existsInCurrentTrack)
                                    {
                                        okToSort = false;
                                    }
                                }
                            }
                        }

                        if (okToSort)
                        {
                            sortedList.Add(currentItem.listEntry);
                            currentItem.sorted = true;
                        }

                    }
                }
            }



            if (sortedList.Count != sortingList.Count) //Errors unable to import change.
            {


                return null;
            }
            else
            {
                return sortedList;
            }
        }

        [Transaction(ReadOnly = false)]
        public void ApplyChange(List<DeltaListEntry> sortedChangeDelta, bool save)
        {
            //apply/save changed, added and moved. Top to bottom

            for (int i = 0; i < sortedChangeDelta.Count; i++)
            {
                ApplyDeltaNewChangeRecursivly(sortedChangeDelta[i]);
            }

            if (save)
            {
                for (int i = 0; i < sortedChangeDelta.Count; i++)
                {
                    if (sortedChangeDelta[i].CurrentObject_CurrentVersion != null)
                    {
                        if (sortedChangeDelta[i].CurrentObject_NewVersion != null)
                        {
                            FixSearchPanel((IDomainObject)sortedChangeDelta[i].CurrentObject_CurrentVersion);
                            //Load session object to get merge to work.
                            IDomainObject chkobj = modelService.GetDomainObject(((IDomainObject)sortedChangeDelta[i].CurrentObject_NewVersion).Id, sortedChangeDelta[i].CurrentObject_NewVersion.GetType());

                            if (typeof(IVersionControlled).IsAssignableFrom(sortedChangeDelta[i].CurrentObject_CurrentVersion.GetType()))
                            {
                                ((IVersionControlled)sortedChangeDelta[i].CurrentObject_CurrentVersion).IsLocked = true;
                                ((IVersionControlled)sortedChangeDelta[i].CurrentObject_CurrentVersion).LockedDate = DateTime.Now;
                                ((IVersionControlled)sortedChangeDelta[i].CurrentObject_CurrentVersion).LockedBy = Environment.UserName;
                            }


                            modelService.MergeSaveDomainObject((IDomainObject)sortedChangeDelta[i].CurrentObject_CurrentVersion);
                        }
                    }
                    else
                    {
                        if (sortedChangeDelta[i].CurrentObject_NewVersion != null)
                        {
                            FixSearchPanel((IDomainObject)sortedChangeDelta[i].CurrentObject_NewVersion);

                            IDomainObject chkobj = modelService.GetDomainObject(((IDomainObject)sortedChangeDelta[i].CurrentObject_NewVersion).Id, sortedChangeDelta[i].CurrentObject_NewVersion.GetType());
                            if (chkobj == null)
                            {
                                modelService.SaveDomainObject((IDomainObject)sortedChangeDelta[i].CurrentObject_NewVersion, true);
                            }
                            else
                            {
                                if (typeof(IVersionControlled).IsAssignableFrom(sortedChangeDelta[i].CurrentObject_NewVersion.GetType()))
                                {
                                    ((IVersionControlled)sortedChangeDelta[i].CurrentObject_NewVersion).IsLocked = true;
                                    ((IVersionControlled)sortedChangeDelta[i].CurrentObject_NewVersion).LockedDate = DateTime.Now;
                                    ((IVersionControlled)sortedChangeDelta[i].CurrentObject_NewVersion).LockedBy = Environment.UserName;
                                    ((IVersionControlled)sortedChangeDelta[i].CurrentObject_NewVersion).State = VersionControlledObjectStat.New;
                                }

                                modelService.MergeSaveDomainObject((IDomainObject)sortedChangeDelta[i].CurrentObject_NewVersion);
                            }
                        }
                    }
                }
            }

            if (save)
            {
                MetaManagerServices.GetCurrentSession().Flush();
            }
            //apply/save deletes. Bottom up

            for (int i = sortedChangeDelta.Count - 1; i >= 0; i--)
            {
                ApplyDeltaDeletesRecursivly(sortedChangeDelta[i]);
            }

            if (save)
            {
                for (int i = sortedChangeDelta.Count - 1; i >= 0; i--)
                {

                    if (sortedChangeDelta[i].CurrentObject_NewVersion != null)
                    {
                        if (sortedChangeDelta[i].CurrentObject_CurrentVersion != null)
                        {
                            FixSearchPanel((IDomainObject)sortedChangeDelta[i].CurrentObject_CurrentVersion);

                            if (typeof(IVersionControlled).IsAssignableFrom(sortedChangeDelta[i].CurrentObject_CurrentVersion.GetType()))
                            {
                                ((IVersionControlled)sortedChangeDelta[i].CurrentObject_CurrentVersion).IsLocked = true;
                                ((IVersionControlled)sortedChangeDelta[i].CurrentObject_CurrentVersion).LockedDate = DateTime.Now;
                                ((IVersionControlled)sortedChangeDelta[i].CurrentObject_CurrentVersion).LockedBy = Environment.UserName;
                            }

                            modelService.MergeSaveDomainObject((IDomainObject)sortedChangeDelta[i].CurrentObject_CurrentVersion);
                        }
                    }
                    else
                    {
                        if (sortedChangeDelta[i].CurrentObject_OldVersion != null)
                        {
                            IDomainObject chkObj = modelService.GetDomainObject(((IDomainObject)sortedChangeDelta[i].CurrentObject_OldVersion).Id, sortedChangeDelta[i].CurrentObject_OldVersion.GetType());

                            if (chkObj != null)
                            {
                                modelService.DeleteDomainObjectWithoutChecksAndCheckOut((IDomainObject)sortedChangeDelta[i].CurrentObject_OldVersion);

                            }
                        }
                    }

                }
            }
        }

        #endregion


        #region Private Members

        private void FixSearchPanel(IDomainObject objectToSave)
        {
            //Fix for searchpanel
            //=========================================================================================================================================================
            if (objectToSave is DataAccess.Domain.View)
            {
                if (((DataAccess.Domain.View)objectToSave).RequestMap != null && ((DataAccess.Domain.View)objectToSave).ResponseMap != null)
                {
                    DataAccess.Domain.PropertyMap tmpmap = modelService.GetDomainObject<DataAccess.Domain.PropertyMap>(((DataAccess.Domain.View)objectToSave).RequestMap.Id);

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
        }

        private DeltaListEntry GetChangeDelta(IDomainObject oldObject, IDomainObject newObject)
        {
            DeltaListEntry rootNode = new DeltaListEntry(newObject, oldObject, null);
            if (oldObject != null && newObject != null)
            {
                if (oldObject.GetType() == newObject.GetType())
                {
                    if (oldObject.Id == newObject.Id)
                    {
                        deltaSerachRecursion(oldObject, newObject, rootNode);
                    }
                }
            }

            return rootNode;
        }

        private void deltaSerachRecursion(IDomainObject oldObject, IDomainObject newObject, DeltaListEntry delta)
        {
            Type classType = newObject.GetType();
            Dictionary<PropertyInfo, Dictionary<IDomainObject, IDomainObject>> childObjectsToSearch = new Dictionary<PropertyInfo, Dictionary<IDomainObject, IDomainObject>>();

            foreach (PropertyInfo pi in classType.GetProperties())
            {
                Object[] ignoreAttributes = pi.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true);

                if (ignoreAttributes.Length == 0)
                {
                    bool outsideFileReference = false;
                    object[] serItemIdAttributes = pi.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);
                    if (serItemIdAttributes.Length > 0)
                    {
                        outsideFileReference = true;
                    }

                    if (pi.PropertyType.IsPrimitive || pi.PropertyType.IsEnum || pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Guid" || pi.PropertyType.Name == typeof(System.Nullable<int>).Name)
                    {
                        //Serach rules
                        if (typeof(IRuledObject).IsAssignableFrom(classType) && pi.Name == "RuleSetXml")
                        {
                            RuleSet oldRule = ((IRuledObject)oldObject).RuleSet;
                            RuleSet newRule = ((IRuledObject)newObject).RuleSet;

                            if (oldRule != null || newRule != null)
                            {
                                if (oldRule == null && newRule != null)
                                {
                                    delta.Changes.Add(new ObjectChangeDescription(newObject.GetType().GetProperty("RuleSet"), ChangeTypes.Changed, newRule, null));
                                }
                                else if (oldRule != null && newRule == null)
                                {
                                    delta.Changes.Add(new ObjectChangeDescription(oldObject.GetType().GetProperty("RuleSet"), ChangeTypes.Changed, null, oldRule));
                                }
                                else if (!oldRule.Equals(newRule))
                                {
                                    DeltaListEntry ruleDelta = new DeltaListEntry(newRule, oldRule, newObject.GetType().GetProperty("RuleSet"));
                                    deltaSearchRuleSet(oldRule, newRule, ruleDelta);

                                    if (ruleDelta.Changes.Count > 0 || ruleDelta.ChildObjects.Count > 0)
                                    {
                                        delta.ChildObjects.Add(ruleDelta);
                                    }
                                }

                            }
                        }
                        else
                        {
                            object oldValue = pi.GetValue(oldObject, null);
                            object newValue = pi.GetValue(newObject, null);

                            if ((oldValue == null && newValue != null) || (oldValue != null && newValue == null) || (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString()))
                            {
                                //Add change to delta list for changed value property
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newValue, oldValue));
                                //delta.Add("PropertyName: " + pi.Name + "\r\nOldValue: " + oldValue == null ? "<NULL>" : oldValue.ToString() + "\r\nNewValue: " + newValue == null ? "<NULL>" : newValue.ToString()); 
                            }
                        }
                    }
                    else if (pi.PropertyType.Name == typeof(IList<object>).Name)
                    {


                        Dictionary<Guid, object> oldListObjects = new Dictionary<Guid, object>();
                        Dictionary<Guid, object> newListObjects = new Dictionary<Guid, object>();

                        foreach (object subObj in ((System.Collections.ICollection)pi.GetValue(oldObject, null)))
                        {
                            oldListObjects.Add(((Guid)subObj.GetType().GetProperty("Id").GetValue(subObj, null)), subObj);
                        }

                        foreach (object subObj in ((System.Collections.ICollection)pi.GetValue(newObject, null)))
                        {
                            newListObjects.Add(((Guid)subObj.GetType().GetProperty("Id").GetValue(subObj, null)), subObj);
                        }

                        Dictionary<IDomainObject, IDomainObject> existingEntrysToSearchForDelta = new Dictionary<IDomainObject, IDomainObject>();

                        //Detecting new and existing enteries
                        foreach (KeyValuePair<Guid, object> listEntryNew in newListObjects)
                        {
                            if (!outsideFileReference)
                            {
                                if (oldListObjects.ContainsKey(listEntryNew.Key)) //Existing
                                {
                                    existingEntrysToSearchForDelta.Add((IDomainObject)oldListObjects[listEntryNew.Key], (IDomainObject)listEntryNew.Value);
                                }
                            }

                            if (!oldListObjects.ContainsKey(listEntryNew.Key)) //New
                            {
                                if (outsideFileReference) //Could this happen.
                                {
                                    //Add change to delta list for a new Id only
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.New, listEntryNew.Key, null));
                                    //delta.Add("PropertyName: " + pi.Name + "\r\nNew Id: " + listEntryNew.Key.ToString());
                                }
                                else
                                {
                                    //Add change to delta list for a new object
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.New, listEntryNew.Value, null));
                                    //delta.Add("PropertyName: " + pi.Name + "\r\nNew Object: " + listEntryNew.Key.ToString());
                                }
                            }
                        }

                        //Detecting deleted enteries 
                        foreach (KeyValuePair<Guid, object> listEntryOld in oldListObjects)
                        {
                            if (!newListObjects.ContainsKey(listEntryOld.Key)) //Deleted
                            {
                                if (outsideFileReference) //Could this happen.
                                {
                                    //Add change to delta list for a deleted Id only
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Deleted, null, listEntryOld.Key));
                                    //delta.Add("PropertyName: " + pi.Name + "\r\nDeleted Id: " + listEntryOld.Key.ToString());
                                }
                                else
                                {
                                    //Add change to delta list for a deleted object
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Deleted, null, listEntryOld.Value));
                                    //delta.Add("PropertyName: " + pi.Name + "\r\nDeleted Object: " + listEntryOld.Key.ToString());
                                }
                            }
                        }

                        if (existingEntrysToSearchForDelta.Count > 0)
                        {
                            childObjectsToSearch.Add(pi, existingEntrysToSearchForDelta);
                        }
                    }
                    else
                    {
                        if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType))
                        {
                            IDomainObject oldRefObj = (IDomainObject)pi.GetValue(oldObject, null);
                            IDomainObject newRefObj = (IDomainObject)pi.GetValue(newObject, null);

                            if (oldRefObj == null && newRefObj != null)
                            {
                                //Add change to delta list for a added object refenrence
                                if (outsideFileReference)
                                {
                                    // only id
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newRefObj.Id, null));
                                    //delta.Add("PropertyName: " + pi.Name + "\r\nAdded Id: " + newRefObj.Id.ToString());
                                }
                                else
                                {
                                    //with object
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newRefObj, null));
                                    //delta.Add("PropertyName: " + pi.Name + "\r\nAdded Object: " + newRefObj.Id.ToString());
                                }
                            }
                            else if (oldRefObj != null && newRefObj == null)
                            {
                                //Add change to delta list for a removed object reference
                                if (outsideFileReference)
                                {
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, null, oldRefObj.Id));
                                }
                                else
                                {
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, null, oldRefObj));
                                }
                                //delta.Add("PropertyName: " + pi.Name + "\r\nRemoved Ref: " + oldRefObj.Id.ToString());
                            }
                            else if (oldRefObj != null && newRefObj != null)
                            {

                                Guid oldId = oldRefObj.Id;
                                Guid newId = newRefObj.Id;

                                if (outsideFileReference)
                                {
                                    if (oldId != newId)
                                    {
                                        //Add change to delta list for a changed object refenrence with type
                                        delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newId, oldId));
                                        //delta.Add("PropertyName: " + pi.Name + "\r\nChanged Ref:\r\nOld Id: " + oldId.ToString() + "\r\nNew Id: " + newId.ToString());
                                    }
                                }
                                else
                                {
                                    if (oldId == newId)
                                    {
                                        Dictionary<IDomainObject, IDomainObject> tmpList = new Dictionary<IDomainObject, IDomainObject>();
                                        tmpList.Add(oldRefObj, newRefObj);
                                        childObjectsToSearch.Add(pi, tmpList);
                                    }
                                    else
                                    {
                                        //Add change to delta list for a changed object refenrence with object
                                        delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newRefObj, oldRefObj));
                                        //delta.Add("PropertyName: " + pi.Name + "\r\nChanged Child:\r\nOld Id: " + oldId.ToString() + "\r\nNew Id: " + newId.ToString());
                                    }
                                }
                            }
                        }
                        else if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(pi.PropertyType))
                        {
                            DataAccess.Domain.VisualModel.UXComponent oldRefObj = (DataAccess.Domain.VisualModel.UXComponent)pi.GetValue(oldObject, null);
                            DataAccess.Domain.VisualModel.UXComponent newRefObj = (DataAccess.Domain.VisualModel.UXComponent)pi.GetValue(newObject, null);

                            if (oldRefObj == null && newRefObj != null)
                            {
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.New, newRefObj, null));
                            }
                            else if (oldRefObj != null && newRefObj == null)
                            {
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Deleted, null, oldRefObj));
                            }
                            else if (oldRefObj != null && newRefObj != null)
                            {
                                DeltaListEntry childEntry = new DeltaListEntry(newRefObj, oldRefObj, pi);

                                deltaSearchVisualTreeRecursive(oldRefObj, newRefObj, childEntry);

                                if (childEntry.Changes.Count > 0 || childEntry.ChildObjects.Count > 0)
                                {
                                    delta.ChildObjects.Add(childEntry);
                                }
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<PropertyInfo, Dictionary<IDomainObject, IDomainObject>> existingEntrysToSearchForDelta in childObjectsToSearch)
            {
                foreach (KeyValuePair<IDomainObject, IDomainObject> oldNewObject in existingEntrysToSearchForDelta.Value)
                {
                    DeltaListEntry childEntry = new DeltaListEntry(oldNewObject.Value, oldNewObject.Key, existingEntrysToSearchForDelta.Key);
                    deltaSerachRecursion(oldNewObject.Key, oldNewObject.Value, childEntry);

                    if (childEntry.Changes.Count > 0 || childEntry.ChildObjects.Count > 0)
                    {
                        delta.ChildObjects.Add(childEntry);
                    }
                }
            }
        }

        private void deltaSearchVisualTreeRecursive(DataAccess.Domain.VisualModel.UXComponent oldComponent, DataAccess.Domain.VisualModel.UXComponent newComponent, DeltaListEntry delta)
        {
            if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(newComponent.GetType()) || typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(newComponent.GetType()))
            {
                Dictionary<string, DataAccess.Domain.VisualModel.UXComponent> childComponentsNew = new Dictionary<string, DataAccess.Domain.VisualModel.UXComponent>();
                Dictionary<string, DataAccess.Domain.VisualModel.UXComponent> childComponentsOld = new Dictionary<string, DataAccess.Domain.VisualModel.UXComponent>();


                if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(newComponent.GetType()))
                {
                    foreach (DataAccess.Domain.VisualModel.UXLayoutGridCell cell in ((DataAccess.Domain.VisualModel.UXLayoutGrid)((DataAccess.Domain.VisualModel.UXGroupBox)newComponent).Container).Cells)
                    {
                        string compIndex = cell.Row.ToString() + ":" + cell.Column.ToString();
                        DataAccess.Domain.VisualModel.UXComponent comp = cell.Component;

                        if (typeof(DataAccess.Domain.VisualModel.UXStackPanel).IsAssignableFrom(comp.GetType()))
                        {
                            foreach (DataAccess.Domain.VisualModel.UXComponent childComp in ((DataAccess.Domain.VisualModel.UXStackPanel)comp).Children)
                            {
                                string compId = (childComp.GetType().GetProperty("MetaId") != null ? childComp.GetType().GetProperty("MetaId").GetValue(childComp, null).ToString() : childComp.Name);
                                childComponentsNew.Add(compIndex + "@" + compId, childComp);
                            }
                        }
                        else
                        {
                            string compId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);
                            childComponentsNew.Add(compIndex + "@" + compId, comp);
                        }
                    }

                    foreach (DataAccess.Domain.VisualModel.UXLayoutGridCell cell in ((DataAccess.Domain.VisualModel.UXLayoutGrid)((DataAccess.Domain.VisualModel.UXGroupBox)oldComponent).Container).Cells)
                    {
                        string compIndex = cell.Row.ToString() + ":" + cell.Column.ToString();
                        DataAccess.Domain.VisualModel.UXComponent comp = cell.Component;

                        if (typeof(DataAccess.Domain.VisualModel.UXStackPanel).IsAssignableFrom(comp.GetType()))
                        {
                            foreach (DataAccess.Domain.VisualModel.UXComponent childComp in ((DataAccess.Domain.VisualModel.UXStackPanel)comp).Children)
                            {
                                string compId = (childComp.GetType().GetProperty("MetaId") != null ? childComp.GetType().GetProperty("MetaId").GetValue(childComp, null).ToString() : childComp.Name);
                                childComponentsOld.Add(compIndex + "@" + compId, childComp);
                            }
                        }
                        else
                        {
                            string compId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);
                            childComponentsOld.Add(compIndex + "@" + compId, comp);
                        }
                    }

                    //foreach (DataAccess.Domain.VisualModel.UXComponent comp in ((DataAccess.Domain.VisualModel.UXGroupBox)newComponent).Container.Children)
                    //{
                    //    string compIndex = ((DataAccess.Domain.VisualModel.UXGroupBox)newComponent).Container.Children.IndexOf(comp).ToString();
                    //    string compId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);
                    //    childComponentsNew.Add(compIndex + "@" + compId, comp);
                    //}

                    //foreach (DataAccess.Domain.VisualModel.UXComponent comp in ((DataAccess.Domain.VisualModel.UXGroupBox)oldComponent).Container.Children)
                    //{
                    //    string compIndex = ((DataAccess.Domain.VisualModel.UXGroupBox)oldComponent).Container.Children.IndexOf(comp).ToString();
                    //    string compId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);
                    //    childComponentsOld.Add(compIndex + "@" + compId, comp);
                    //}
                }
                else
                {
                    foreach (DataAccess.Domain.VisualModel.UXComponent comp in ((DataAccess.Domain.VisualModel.UXContainer)newComponent).Children)
                    {
                        string compIndex = ((DataAccess.Domain.VisualModel.UXContainer)newComponent).Children.IndexOf(comp).ToString();
                        string compId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);
                        childComponentsNew.Add(compIndex + "@" + compId, comp);
                    }

                    foreach (DataAccess.Domain.VisualModel.UXComponent comp in ((DataAccess.Domain.VisualModel.UXContainer)oldComponent).Children)
                    {
                        string compIndex = ((DataAccess.Domain.VisualModel.UXContainer)oldComponent).Children.IndexOf(comp).ToString();
                        string compId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);
                        childComponentsOld.Add(compIndex + "@" + compId, comp);
                    }
                }

                Dictionary<DataAccess.Domain.VisualModel.UXComponent, DataAccess.Domain.VisualModel.UXComponent> childrenToSearch = new Dictionary<DataAccess.Domain.VisualModel.UXComponent, DataAccess.Domain.VisualModel.UXComponent>();

                Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string>> newComponents = new Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string>>();
                Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string>> deletedComponents = new Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string>>();

                //Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, int>> newComponents = new Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, int>>();
                //Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, int>> deletedComponents = new Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, int>>();

                //Find new components
                foreach (KeyValuePair<string, DataAccess.Domain.VisualModel.UXComponent> item in childComponentsNew)
                {
                    if (!childComponentsOld.ContainsKey(item.Key))
                    {
                        string compIndex = item.Key.Split('@')[0];
                        //int compIndex = Convert.ToInt32(item.Key.Split('@')[0]);
                        string compId = item.Key.Split('@')[1];

                        newComponents.Add(compId, new KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string>(item.Value, compIndex));
                        //New component or changed name?
                    }
                    else
                    {
                        childrenToSearch.Add(childComponentsOld[item.Key], item.Value);
                    }
                }

                //Find deleted components
                foreach (KeyValuePair<string, DataAccess.Domain.VisualModel.UXComponent> item in childComponentsOld)
                {
                    if (!childComponentsNew.ContainsKey(item.Key))
                    {
                        string compIndex = item.Key.Split('@')[0];
                        //int compIndex = Convert.ToInt32(item.Key.Split('@')[0]);
                        string compId = item.Key.Split('@')[1];

                        deletedComponents.Add(compId, new KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string>(item.Value, compIndex));
                        //Deleted component or changed name?
                    }
                }

                //Find possible moved components
                int i = 0;
                while (i < newComponents.Count)
                {
                    DataAccess.Domain.VisualModel.UXComponent newComp = newComponents.ElementAt(i).Value.Key;
                    string newCompIndex = newComponents.ElementAt(i).Value.Value;
                    string newCompId = newComponents.ElementAt(i).Key;

                    Dictionary<string, KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string>> possibleDelMatch = deletedComponents.Where(n => n.Key == newCompId).ToDictionary(k => k.Key, e => e.Value);
                    bool found = false;
                    int j = 0;
                    while (j < possibleDelMatch.Count)
                    {
                        DataAccess.Domain.VisualModel.UXComponent delComp = possibleDelMatch.ElementAt(j).Value.Key;
                        string delCompIndex = possibleDelMatch.ElementAt(j).Value.Value;
                        string delCompId = possibleDelMatch.ElementAt(j).Key;

                        if (delComp.GetType() == newComp.GetType())
                        {
                            if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(newComp.GetType()))
                            {

                                if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(newComp.GetType()))
                                {
                                    foreach (DataAccess.Domain.VisualModel.UXComponent child in ((DataAccess.Domain.VisualModel.UXGroupBox)newComp).Container.Children)
                                    {
                                        if (((DataAccess.Domain.VisualModel.UXGroupBox)delComp).Container.Children.Where(c => c.Name == child.Name).Count() > 0)
                                        {
                                            //Moved component;
                                            delta.Changes.Add(new ObjectChangeDescription(null, ChangeTypes.Moved, newComponents[newCompId].Value, deletedComponents[delCompId].Value, newComp));
                                            newComponents.Remove(newCompId);
                                            deletedComponents.Remove(delCompId);
                                            childrenToSearch.Add(delComp, newComp);
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (DataAccess.Domain.VisualModel.UXComponent child in ((DataAccess.Domain.VisualModel.UXContainer)newComp).Children)
                                    {
                                        if (((DataAccess.Domain.VisualModel.UXContainer)delComp).Children.Where(c => c.Name == child.Name).Count() > 0)
                                        {
                                            //Moved component;
                                            delta.Changes.Add(new ObjectChangeDescription(null, ChangeTypes.Moved, newComponents[newCompId].Value, deletedComponents[delCompId].Value, newComp));
                                            newComponents.Remove(newCompId);
                                            deletedComponents.Remove(delCompId);
                                            childrenToSearch.Add(delComp, newComp);
                                            found = true;
                                            break;
                                        }
                                    }
                                }

                                if (found)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                //Moved component;
                                delta.Changes.Add(new ObjectChangeDescription(null, ChangeTypes.Moved, newComponents[newCompId].Value, deletedComponents[delCompId].Value, newComp));
                                newComponents.Remove(newCompId);
                                deletedComponents.Remove(delCompId);
                                found = true;
                                childrenToSearch.Add(delComp, newComp);
                                break;
                            }
                        }

                        j++;
                    }

                    if (!found)
                    {
                        i++;
                    }
                }

                foreach (KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, string> newComp in newComponents.Values)
                {
                    delta.Changes.Add(new ObjectChangeDescription(null, ChangeTypes.New, newComp.Key, null, null, newComp.Value));
                }

                foreach (DataAccess.Domain.VisualModel.UXComponent delComp in deletedComponents.Values.Select(c => c.Key))
                {
                    delta.Changes.Add(new ObjectChangeDescription(null, ChangeTypes.Deleted, null, delComp));
                }


                foreach (KeyValuePair<DataAccess.Domain.VisualModel.UXComponent, DataAccess.Domain.VisualModel.UXComponent> children in childrenToSearch)
                {
                    DeltaListEntry childEntry = new DeltaListEntry(children.Value, children.Key, null);

                    deltaSearchVisualTreeRecursive(children.Key, children.Value, childEntry);

                    if (childEntry.Changes.Count > 0 || childEntry.ChildObjects.Count > 0)
                    {
                        delta.ChildObjects.Add(childEntry);
                    }
                }

            }
            //else
            //{
            //Check if type of component changed
            if (oldComponent.GetType() != newComponent.GetType())
            {
                //Type changed
                delta.Changes.Add(new ObjectChangeDescription(null, ChangeTypes.Changed, newComponent.GetType(), oldComponent.GetType()));
            }
            else
            {
                foreach (PropertyInfo pi in newComponent.GetType().GetProperties())
                {
                    if (pi.CanWrite)
                    {
                        if (pi.PropertyType.IsPrimitive || pi.PropertyType.IsEnum || pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Guid" || pi.PropertyType.Name == typeof(System.Nullable<int>).Name)
                        {
                            object oldPropertyValue = pi.GetValue(oldComponent, null);
                            object newPropertyValue = pi.GetValue(newComponent, null);

                            if ((oldPropertyValue == null && newPropertyValue != null) || (oldPropertyValue != null && newPropertyValue == null))
                            {
                                //Property changed
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newPropertyValue, oldPropertyValue));
                            }
                            else if (oldPropertyValue != null && newPropertyValue != null)
                            {
                                if (oldPropertyValue.ToString() != newPropertyValue.ToString())
                                {
                                    //Property changed
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newPropertyValue, oldPropertyValue));
                                }
                            }
                        }
                        else if (typeof(RuleSetWrapper).IsAssignableFrom(pi.PropertyType))
                        {
                            RuleSetWrapper oldRule = (RuleSetWrapper)pi.GetValue(oldComponent, null);
                            RuleSetWrapper newRule = (RuleSetWrapper)pi.GetValue(newComponent, null);

                            if (oldRule == null && newRule != null)
                            {
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.New, newRule, null));
                            }
                            else if (oldRule != null && newRule == null)
                            {
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Deleted, null, oldRule));
                            }
                            else
                            {
                                if (oldRule.RuleSet != null || newRule.RuleSet != null)
                                {
                                    if (oldRule.RuleSet == null && newRule.RuleSet != null)
                                    {
                                        DeltaListEntry childDelta = new DeltaListEntry(newRule, oldRule, pi);
                                        childDelta.Changes.Add(new ObjectChangeDescription(typeof(RuleSetWrapper).GetProperty("RuleSet"), ChangeTypes.Changed, newRule.RuleSet, null));
                                        delta.ChildObjects.Add(childDelta);
                                    }
                                    else if (oldRule.RuleSet != null && newRule.RuleSet == null)
                                    {
                                        DeltaListEntry childDelta = new DeltaListEntry(newRule, oldRule, pi);
                                        childDelta.Changes.Add(new ObjectChangeDescription(typeof(RuleSetWrapper).GetProperty("RuleSet"), ChangeTypes.Changed, null, oldRule.RuleSet));
                                        delta.ChildObjects.Add(childDelta);
                                    }
                                    else if (!oldRule.RuleSet.Equals(newRule.RuleSet))
                                    {
                                        DeltaListEntry ruleDelta = new DeltaListEntry(newRule.RuleSet, oldRule.RuleSet, typeof(RuleSetWrapper).GetProperty("RuleSet"));
                                        deltaSearchRuleSet(oldRule.RuleSet, newRule.RuleSet, ruleDelta);

                                        if (ruleDelta.Changes.Count > 0 || ruleDelta.ChildObjects.Count > 0)
                                        {
                                            DeltaListEntry childDelta = new DeltaListEntry(newRule, oldRule, pi);
                                            childDelta.ChildObjects.Add(ruleDelta);
                                            delta.ChildObjects.Add(childDelta);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            //}

        }

        private void deltaSearchRuleSet(System.Workflow.Activities.Rules.RuleSet oldRuleSet, System.Workflow.Activities.Rules.RuleSet newRuleSet, DeltaListEntry delta)
        {
            if (newRuleSet.ChainingBehavior != oldRuleSet.ChainingBehavior)
            {
                delta.Changes.Add(new ObjectChangeDescription(typeof(System.Workflow.Activities.Rules.RuleSet).GetProperty("ChainingBehavior"), ChangeTypes.Changed, newRuleSet.ChainingBehavior, oldRuleSet.ChainingBehavior));
            }

            if (newRuleSet.Description != oldRuleSet.Description)
            {
                delta.Changes.Add(new ObjectChangeDescription(typeof(System.Workflow.Activities.Rules.RuleSet).GetProperty("ChainingBehavior"), ChangeTypes.Changed, newRuleSet.Description, oldRuleSet.Description));
            }

            if (newRuleSet.Name != oldRuleSet.Name)
            {
                delta.Changes.Add(new ObjectChangeDescription(typeof(System.Workflow.Activities.Rules.RuleSet).GetProperty("ChainingBehavior"), ChangeTypes.Changed, newRuleSet.Name, oldRuleSet.Name));
            }

            Dictionary<string, System.Workflow.Activities.Rules.Rule> newRulesDict = new Dictionary<string, System.Workflow.Activities.Rules.Rule>();
            Dictionary<string, System.Workflow.Activities.Rules.Rule> oldRulesDict = new Dictionary<string, System.Workflow.Activities.Rules.Rule>();


            for (int i = 0; i < newRuleSet.Rules.Count; i++)
            {
                newRulesDict.Add(newRuleSet.Rules.ElementAt(i).Name, newRuleSet.Rules.ElementAt(i));
            }

            for (int i = 0; i < oldRuleSet.Rules.Count; i++)
            {
                oldRulesDict.Add(oldRuleSet.Rules.ElementAt(i).Name, oldRuleSet.Rules.ElementAt(i));
            }

            Dictionary<System.Workflow.Activities.Rules.Rule, System.Workflow.Activities.Rules.Rule> rulesToSearch = new Dictionary<System.Workflow.Activities.Rules.Rule, System.Workflow.Activities.Rules.Rule>();

            foreach (string newKey in newRulesDict.Keys)
            {
                if (!oldRulesDict.ContainsKey(newKey))
                {
                    delta.Changes.Add(new ObjectChangeDescription(typeof(System.Workflow.Activities.Rules.RuleSet).GetProperty("Rules"), ChangeTypes.New, newRulesDict[newKey], null));
                }
                else
                {
                    rulesToSearch.Add(newRulesDict[newKey], oldRulesDict[newKey]);
                }
            }

            foreach (string oldKey in oldRulesDict.Keys)
            {
                if (!newRulesDict.ContainsKey(oldKey))
                {
                    delta.Changes.Add(new ObjectChangeDescription(typeof(System.Workflow.Activities.Rules.RuleSet).GetProperty("Rules"), ChangeTypes.Deleted, null, oldRulesDict[oldKey]));
                }
            }


            foreach (KeyValuePair<System.Workflow.Activities.Rules.Rule, System.Workflow.Activities.Rules.Rule> newOldRule in rulesToSearch)
            {
                DeltaListEntry childDelta = new DeltaListEntry(newOldRule.Key, newOldRule.Value, typeof(System.Workflow.Activities.Rules.RuleSet).GetProperty("Rules"));

                deltaSearchRule(newOldRule.Value, newOldRule.Key, childDelta);

                if (childDelta.Changes.Count > 0 || childDelta.ChildObjects.Count > 0)
                {
                    delta.ChildObjects.Add(childDelta);
                }
            }

        }

        private void deltaSearchRule(System.Workflow.Activities.Rules.Rule oldRule, System.Workflow.Activities.Rules.Rule newRule, DeltaListEntry delta)
        {
            foreach (PropertyInfo pi in newRule.GetType().GetProperties())
            {
                if (pi.PropertyType.IsPrimitive || pi.PropertyType.IsEnum || pi.PropertyType.Name == "String" || pi.PropertyType.Name == typeof(System.Nullable<int>).Name)
                {
                    object oldValue = pi.GetValue(oldRule, null);
                    object newValue = pi.GetValue(newRule, null);

                    if ((oldValue == null && newValue != null) || (oldValue != null && newValue == null) || (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString()))
                    {
                        delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newValue, oldValue));
                    }
                }
                else if (typeof(System.Workflow.Activities.Rules.RuleCondition).IsAssignableFrom(pi.PropertyType))
                {
                    object oldValue = pi.GetValue(oldRule, null);
                    object newValue = pi.GetValue(newRule, null);

                    if ((oldValue == null && newValue != null) || (oldValue != null && newValue == null) || (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString()))
                    {
                        delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newValue, oldValue));
                    }
                }
                else if (pi.PropertyType.Name == typeof(IList<object>).Name)
                {
                    if (typeof(System.Workflow.Activities.Rules.RuleAction).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]))
                    {
                        IList<System.Workflow.Activities.Rules.RuleAction> newActions = (IList<System.Workflow.Activities.Rules.RuleAction>)pi.GetValue(newRule, null);
                        IList<System.Workflow.Activities.Rules.RuleAction> oldActions = (IList<System.Workflow.Activities.Rules.RuleAction>)pi.GetValue(oldRule, null);

                        for (int i = 0; i < newActions.Count; i++)
                        {
                            if (oldActions.Count > i)
                            {
                                if (newActions[i].ToString() != oldActions[i].ToString())
                                {
                                    delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Changed, newActions[i], oldActions[i]));
                                }
                            }
                            else
                            {
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.New, newActions[i], null));
                            }
                        }

                        if (oldActions.Count > newActions.Count)
                        {
                            for (int i = newActions.Count; i < oldActions.Count; i++)
                            {
                                delta.Changes.Add(new ObjectChangeDescription(pi, ChangeTypes.Deleted, null, oldActions[i]));
                            }
                        }

                    }
                }
            }
        }

        private void GetConflictsWithDelta(DeltaListEntry delta)
        {
            if (typeof(IDomainObject).IsAssignableFrom(delta.CurrentObject_NewVersion.GetType()))
            {
                if (delta.CurrentObject_CurrentVersion != null)
                {
                    Type classType = delta.CurrentObject_NewVersion.GetType();

                    IDomainObject currentVersion = (IDomainObject)delta.CurrentObject_CurrentVersion;

                    foreach (ObjectChangeDescription change in delta.Changes)
                    {
                        if (change.ChangedProperty != null)
                        {
                            if ((change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name && typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType.GetGenericArguments()[0])) || typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType))
                            {

                                if (change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name)
                                {

                                }
                                else //DomainObject
                                {
                                    if (change.ChangeType == ChangeTypes.New)
                                    {
                                        if (change.ChangedProperty.GetValue(currentVersion, null) != null)
                                        {
                                            Guid currentId = ((IDomainObject)change.ChangedProperty.GetValue(currentVersion, null)).Id;
                                            Guid newId = (change.NewValue is Guid ? (Guid)change.NewValue : ((IDomainObject)change.NewValue).Id);

                                            if (currentId != newId)
                                            {
                                                change.conflict = true;
                                                change.CurrentValue = (change.NewValue is Guid ? currentId : change.ChangedProperty.GetValue(currentVersion, null));
                                            }
                                        }

                                    }
                                    else if (change.ChangeType == ChangeTypes.Deleted)
                                    {
                                        if (change.ChangedProperty.GetValue(currentVersion, null) != null)
                                        {
                                            if (((IDomainObject)change.ChangedProperty.GetValue(currentVersion, null)).Id != (change.OldValue is Guid ? (Guid)change.OldValue : ((IDomainObject)change.OldValue).Id))
                                            {
                                                IDomainObject currentObject = ((IDomainObject)change.ChangedProperty.GetValue(currentVersion, null));
                                                change.conflict = true;
                                                change.CurrentValue = (change.OldValue is Guid ? currentObject.Id : (object)currentObject);
                                            }
                                        }

                                        change.ChangedProperty.SetValue(currentVersion, null, null);
                                    }
                                    else if (change.ChangeType == ChangeTypes.Changed)
                                    {


                                        object currentId = (change.ChangedProperty.GetValue(currentVersion, null) == null ? null : (object)((IDomainObject)change.ChangedProperty.GetValue(currentVersion, null)).Id);


                                        object oldId = null;
                                        if (change.OldValue != null)
                                        {
                                            oldId = (change.OldValue is Guid ? change.OldValue : ((IDomainObject)change.OldValue).Id);
                                        }

                                        object newId = null;
                                        if (change.NewValue != null)
                                        {
                                            newId = (change.NewValue is Guid ? change.NewValue : ((IDomainObject)change.NewValue).Id);
                                        }

                                        if (Convert.ToString(currentId) != Convert.ToString(oldId) && Convert.ToString(currentId) != Convert.ToString(newId))
                                        {
                                            change.conflict = true;
                                            change.CurrentValue = (change.NewValue is Guid ? currentId : change.ChangedProperty.GetValue(currentVersion, null));
                                        }
                                    }
                                }
                            }
                            else if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(change.ChangedProperty.PropertyType))
                            {
                                if (change.ChangeType == ChangeTypes.New)
                                {
                                    object currentValue = change.ChangedProperty.GetValue(currentVersion, null);

                                    if (currentValue != null)
                                    {
                                        change.conflict = true;
                                        change.CurrentValue = currentValue;
                                    }
                                }
                            }
                            else
                            {
                                object currentValue = change.ChangedProperty.GetValue(currentVersion, null);

                                string baseValueStr = Convert.ToString(change.OldValue);
                                string newValueStr = Convert.ToString(change.NewValue);
                                string currentValueStr = Convert.ToString(currentValue);


                                if (baseValueStr != currentValueStr && currentValueStr != newValueStr)
                                {
                                    change.conflict = true;
                                    change.CurrentValue = currentValue;
                                }
                                else if (currentValueStr == newValueStr)
                                {
                                    change.possibleDoubleImplementation = true;
                                    change.CurrentValue = currentValue;
                                }
                            }
                        }

                    }

                    foreach (DeltaListEntry childDelta in delta.ChildObjects)
                    {
                        if (typeof(IDomainObject).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                        {
                            GetConflictsWithDelta(childDelta);
                        }
                        else if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                        {
                            GetConflictsWithDeltaVisualTree(childDelta);
                        }
                        else if (typeof(RuleSet).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                        {
                            GetConflictsWithDeltaRules(childDelta);
                        }
                    }
                }
            }
        }

        private void GetConflictsWithDeltaRules(DeltaListEntry delta)
        {
            if (delta.CurrentObject_CurrentVersion != null)
            {
                foreach (ObjectChangeDescription change in delta.Changes)
                {
                    if (change.ChangedProperty.Name == "Rules")
                    {
                        if (change.ChangeType == ChangeTypes.New)
                        {
                            if (((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Where(r => r.Name == ((Rule)change.NewValue).Name).Count() > 0)
                            {
                                Rule currentRule = (Rule)((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Where(r => r.Name == ((Rule)change.NewValue).Name).First();

                                if (!currentRule.Equals(change.NewValue))
                                {
                                    change.conflict = true;
                                    change.CurrentValue = currentRule;
                                }
                            }
                        }
                    }
                    else
                    {
                        object currentValue = change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                        string baseValueStr = Convert.ToString(change.OldValue);
                        string newValueStr = Convert.ToString(change.NewValue);
                        string currentValueStr = Convert.ToString(currentValue);


                        if (baseValueStr != currentValueStr && currentValueStr != newValueStr)
                        {
                            change.conflict = true;
                            change.CurrentValue = currentValue;
                        }
                        else if (currentValueStr == newValueStr)
                        {
                            change.possibleDoubleImplementation = true;
                            change.CurrentValue = currentValue;
                        }
                    }
                }

                foreach (DeltaListEntry childDelta in delta.ChildObjects)
                {
                    GetConflictsWithDeltaRules(childDelta);
                }
            }
        }

        private void GetConflictsWithDeltaVisualTree(DeltaListEntry delta)
        {
            if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(delta.CurrentObject_NewVersion.GetType()))
            {
                if (delta.CurrentObject_CurrentVersion != null)
                {
                    DataAccess.Domain.VisualModel.UXComponent currentVersion = (DataAccess.Domain.VisualModel.UXComponent)delta.CurrentObject_CurrentVersion;

                    foreach (ObjectChangeDescription change in delta.Changes)
                    {
                        if (change.ChangedProperty != null)
                        {
                            PropertyInfo pi = change.ChangedProperty;

                            if (pi.PropertyType.IsPrimitive || pi.PropertyType.IsEnum || pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Guid" || pi.PropertyType.Name == typeof(System.Nullable<int>).Name)
                            {
                                object currentValue = pi.GetValue(currentVersion, null);

                                if (currentValue.ToString() != change.OldValue.ToString() && currentValue.ToString() != change.NewValue.ToString())
                                {
                                    change.conflict = true;
                                    change.CurrentValue = currentValue;
                                }
                            }
                            else if (pi.PropertyType == typeof(RuleSetWrapper))
                            {
                                object currentValue = pi.GetValue(currentVersion, null);

                                if (change.OldValue == null && currentValue != null)
                                {
                                    change.conflict = true;
                                    change.CurrentValue = currentValue;
                                }
                            }
                        }
                        else
                        {
                            if (change.ChangeType == ChangeTypes.Changed)
                            {
                                if (delta.CurrentObject_CurrentVersion != null)
                                {
                                    Type oldType = (Type)change.OldValue;
                                    Type newType = (Type)change.NewValue;
                                    Type currentType = delta.CurrentObject_CurrentVersion.GetType();

                                    if (oldType != currentType && currentType != newType)
                                    {
                                        change.conflict = true;
                                        change.CurrentValue = currentType;
                                    }
                                }
                            }
                            else if (change.ChangeType == ChangeTypes.Moved)
                            {
                                if (delta.CurrentObject_CurrentVersion != null)
                                {
                                    DataAccess.Domain.VisualModel.UXChildCollection currentChildCollection = null;
                                    DataAccess.Domain.VisualModel.UXLayoutGrid layoutGrid = null;

                                    if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                                    {
                                        currentChildCollection = ((DataAccess.Domain.VisualModel.UXGroupBox)delta.CurrentObject_CurrentVersion).Container.Children;
                                        layoutGrid = (DataAccess.Domain.VisualModel.UXLayoutGrid)((DataAccess.Domain.VisualModel.UXGroupBox)delta.CurrentObject_CurrentVersion).Container;
                                    }
                                    else if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                                    {
                                        currentChildCollection = ((DataAccess.Domain.VisualModel.UXContainer)delta.CurrentObject_CurrentVersion).Children;
                                    }

                                    string movedCompId = (change.MovedComponent.GetType().GetProperty("MetaId") != null ? change.MovedComponent.GetType().GetProperty("MetaId").GetValue(change.MovedComponent, null).ToString() : change.MovedComponent.Name);

                                    if (layoutGrid != null)
                                    {
                                        foreach (DataAccess.Domain.VisualModel.UXLayoutGridCell cell in layoutGrid.Cells)
                                        {
                                            if (cell.Component != null)
                                            {
                                                string currentChildId = (cell.Component.GetType().GetProperty("MetaId") != null ? cell.Component.GetType().GetProperty("MetaId").GetValue(cell.Component, null).ToString() : cell.Component.Name);

                                                if (currentChildId == movedCompId)
                                                {
                                                    if (change.OldValue.ToString() != (cell.Row + ":" + cell.Column))
                                                    {
                                                        change.conflict = true;
                                                        change.CurrentValue = (cell.Row + ":" + cell.Column);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < currentChildCollection.Count; i++)
                                        {
                                            string currentChildId = (currentChildCollection[i].GetType().GetProperty("MetaId") != null ? currentChildCollection[i].GetType().GetProperty("MetaId").GetValue(currentChildCollection[i], null).ToString() : currentChildCollection[i].Name);

                                            if (currentChildId == movedCompId)
                                            {
                                                if (Convert.ToInt32(change.OldValue) != i)
                                                {
                                                    change.conflict = true;
                                                    change.CurrentValue = i;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (DeltaListEntry childDelta in delta.ChildObjects)
                    {
                        if (typeof(RuleSetWrapper).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                        {
                            GetConflictsWithDeltaRules(childDelta);
                        }
                        else
                        {
                            GetConflictsWithDeltaVisualTree(childDelta);
                        }
                    }

                }
            }
        }

        private List<Guid> GetObjectIdsCreatedAndDeletedInChange(DeltaListEntry delta, Dictionary<Type, Dictionary<Guid, IDomainObject>> createdObjectsLookUp)
        {
            List<Guid> returnlist = new List<Guid>();

            if (delta.CurrentObject_OldVersion == null && delta.CurrentObject_NewVersion != null)
            {
                returnlist.AddRange(GetObjectIdsCreatedForDomainObject(((IDomainObject)delta.CurrentObject_NewVersion), createdObjectsLookUp));
            }
            else if (delta.CurrentObject_OldVersion != null && delta.CurrentObject_NewVersion == null)
            {
                returnlist.AddRange(GetObjectIdsCreatedForDomainObject(((IDomainObject)delta.CurrentObject_OldVersion), null));
            }
            else
            {
                foreach (ObjectChangeDescription change in delta.Changes)
                {
                    if (change.ChangeType == ChangeTypes.New || change.ChangeType == ChangeTypes.Changed)
                    {
                        if (change.NewValue != null)
                        {
                            if (typeof(IDomainObject).IsAssignableFrom(change.NewValue.GetType()))
                            {
                                returnlist.AddRange(GetObjectIdsCreatedForDomainObject((IDomainObject)change.NewValue, createdObjectsLookUp));
                            }
                        }
                    }
                    else if (change.ChangeType == ChangeTypes.Deleted)
                    {
                        if (change.OldValue != null)
                        {
                            if (typeof(IDomainObject).IsAssignableFrom(change.OldValue.GetType()))
                            {
                                returnlist.AddRange(GetObjectIdsCreatedForDomainObject((IDomainObject)change.OldValue, null));
                            }
                        }
                    }
                }

                foreach (DeltaListEntry childDelta in delta.ChildObjects)
                {
                    if ((childDelta.CurrentObject_NewVersion != null & typeof(IDomainObject).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType())) || (childDelta.CurrentObject_OldVersion != null & typeof(IDomainObject).IsAssignableFrom(childDelta.CurrentObject_OldVersion.GetType())))
                    {
                        returnlist.AddRange(GetObjectIdsCreatedAndDeletedInChange(childDelta, createdObjectsLookUp));
                    }
                }
            }

            return returnlist;
        }

        private List<Guid> GetObjectIdsCreatedForDomainObject(IDomainObject domainObject, Dictionary<Type, Dictionary<Guid, IDomainObject>> createdObjectsLookUp)
        {

            List<Guid> returnList = new List<Guid>();
            if (domainObject != null)
            {
                Dictionary<Guid, IDomainObject> childObjects = new Dictionary<Guid, IDomainObject>();

                returnList.Add(domainObject.Id);

                if (createdObjectsLookUp != null)
                {
                    if (!createdObjectsLookUp.ContainsKey(domainObject.GetType()))
                    {
                        createdObjectsLookUp.Add(domainObject.GetType(), new Dictionary<Guid, IDomainObject>());
                    }

                    if (!createdObjectsLookUp[domainObject.GetType()].ContainsKey(domainObject.Id))
                    {
                        createdObjectsLookUp[domainObject.GetType()].Add(domainObject.Id, domainObject);
                    }
                }

                foreach (PropertyInfo pi in domainObject.GetType().GetProperties())
                {
                    if (pi.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true).Length == 0)
                    {
                        if (pi.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length == 0)
                        {
                            if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType))
                            {
                                IDomainObject child = (IDomainObject)pi.GetValue(domainObject, null);
                                if (child != null)
                                {
                                    if (!childObjects.ContainsKey(child.Id))
                                    {
                                        childObjects.Add(child.Id, child);
                                    }
                                }

                            }
                            else if (pi.PropertyType.Name == typeof(IList<object>).Name)
                            {
                                if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]))
                                {
                                    foreach (object subObj in ((System.Collections.ICollection)pi.GetValue(domainObject, null)))
                                    {
                                        IDomainObject child = (IDomainObject)subObj;

                                        if (!childObjects.ContainsKey(child.Id))
                                        {
                                            childObjects.Add(child.Id, child);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                foreach (IDomainObject child in childObjects.Values)
                {
                    List<Guid> guidsFromChild = GetObjectIdsCreatedForDomainObject(child, createdObjectsLookUp);

                    returnList.AddRange(guidsFromChild);
                }

            }
            return returnList;
        }


        private Dictionary<Guid, Type> GetObjectIdsReferencedInChange(DeltaListEntry delta)
        {
            Dictionary<Guid, Type> returnList = new Dictionary<Guid, Type>();

            if (delta.CurrentObject_OldVersion == null && delta.CurrentObject_NewVersion != null)
            {
                returnList = GetObjectIdsReferencedByDomainObject((IDomainObject)delta.CurrentObject_NewVersion);
            }
            else if (delta.CurrentObject_OldVersion != null && delta.CurrentObject_NewVersion == null)
            {
                returnList = GetObjectIdsReferencedByDomainObject((IDomainObject)delta.CurrentObject_OldVersion);
            }
            else
            {
                foreach (ObjectChangeDescription change in delta.Changes)
                {
                    if (change.ChangeType == ChangeTypes.New || change.ChangeType == ChangeTypes.Changed)
                    {
                        if (change.NewValue != null)
                        {
                            if (typeof(IDomainObject).IsAssignableFrom(change.NewValue.GetType()))
                            {
                                Dictionary<Guid, Type> tempList = GetObjectIdsReferencedByDomainObject((IDomainObject)change.NewValue);

                                foreach (KeyValuePair<Guid, Type> reference in tempList)
                                {
                                    if (!returnList.ContainsKey(reference.Key))
                                    {
                                        returnList.Add(reference.Key, reference.Value);
                                    }
                                }
                            }
                            else if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(change.NewValue.GetType()))
                            {
                                Dictionary<Guid, Type> tempList = GetObjectIdsReferencedByComponent((DataAccess.Domain.VisualModel.UXComponent)change.NewValue);

                                foreach (KeyValuePair<Guid, Type> reference in tempList)
                                {
                                    if (!returnList.ContainsKey(reference.Key))
                                    {
                                        returnList.Add(reference.Key, reference.Value);
                                    }
                                }
                            }
                            else if (change.NewValue is Guid)
                            {
                                if (change.ChangedProperty != null && ((Guid)change.NewValue) != Guid.Empty)
                                {
                                    Type propertyType = null;

                                    if (typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType))
                                    {
                                        propertyType = change.ChangedProperty.PropertyType;
                                    }
                                    else if (change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name && typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType.GetGenericArguments()[0]))
                                    {
                                        propertyType = change.ChangedProperty.PropertyType.GetGenericArguments()[0];
                                    }


                                    if (propertyType != null)
                                    {
                                        if (!returnList.ContainsKey((Guid)change.NewValue))
                                        {
                                            returnList.Add((Guid)change.NewValue, propertyType);
                                        }
                                    }
                                    else //References from components.
                                    {
                                        if (!returnList.ContainsKey((Guid)change.NewValue))
                                        {
                                            Type referenceType = null;

                                            if (delta.CurrentObject_NewVersion.GetType().GetProperty(change.ChangedProperty.Name.Substring(0, change.ChangedProperty.Name.Length - 2)) != null)
                                            {
                                                referenceType = delta.CurrentObject_NewVersion.GetType().GetProperty(change.ChangedProperty.Name.Substring(0, change.ChangedProperty.Name.Length - 2)).PropertyType;
                                            }

                                            if (referenceType != null)
                                            {
                                                returnList.Add((Guid)change.NewValue, referenceType);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                foreach (DeltaListEntry childDelta in delta.ChildObjects)
                {
                    Dictionary<Guid, Type> tempList = GetObjectIdsReferencedInChange(childDelta);

                    foreach (KeyValuePair<Guid, Type> reference in tempList)
                    {
                        if (!returnList.ContainsKey(reference.Key))
                        {
                            returnList.Add(reference.Key, reference.Value);
                        }
                    }
                }
            }

            return returnList;
        }

        private Dictionary<Guid, Type> GetObjectIdsReferencedByComponent(DataAccess.Domain.VisualModel.UXComponent component)
        {
            Dictionary<Guid, Type> returnList = new Dictionary<Guid, Type>();

            DataAccess.Domain.VisualModel.UXChildCollection currentChildCollection = null;

            if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(component.GetType()))
            {
                if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(component.GetType()))
                {
                    currentChildCollection = ((DataAccess.Domain.VisualModel.UXGroupBox)component).Container.Children;
                }
                else if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(component.GetType()))
                {
                    currentChildCollection = ((DataAccess.Domain.VisualModel.UXContainer)component).Children;
                }
            }

            foreach (PropertyInfo pi in component.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(Guid))
                {
                    Guid refValue = (Guid)pi.GetValue(component, null);

                    if (refValue != Guid.Empty)
                    {
                        if (!returnList.ContainsKey(refValue))
                        {
                            Type referenceType = null;

                            if (component.GetType().GetProperty(pi.Name.Substring(0, pi.Name.Length - 2)) != null)
                            {
                                referenceType = component.GetType().GetProperty(pi.Name.Substring(0, pi.Name.Length - 2)).PropertyType;
                            }

                            if (referenceType != null)
                            {
                                returnList.Add(refValue, referenceType);
                            }
                        }
                    }
                }
            }

            if (currentChildCollection != null)
            {
                foreach (DataAccess.Domain.VisualModel.UXComponent childComp in currentChildCollection)
                {
                    Dictionary<Guid, Type> tempList = GetObjectIdsReferencedByComponent(childComp);

                    foreach (KeyValuePair<Guid, Type> reference in tempList)
                    {
                        if (!returnList.ContainsKey(reference.Key))
                        {
                            returnList.Add(reference.Key, reference.Value);
                        }
                    }
                }
            }

            return returnList;
        }

        private Dictionary<Guid, Type> GetObjectIdsReferencedByDomainObject(IDomainObject domainObject)
        {
            Dictionary<Guid, Type> returnList = new Dictionary<Guid, Type>();
            if (domainObject != null)
            {
                Dictionary<Guid, IDomainObject> childObjects = new Dictionary<Guid, IDomainObject>();


                foreach (PropertyInfo pi in domainObject.GetType().GetProperties())
                {
                    if (pi.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true).Length == 0)
                    {
                        if (pi.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length == 0)
                        {
                            if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType))
                            {
                                IDomainObject child = (IDomainObject)pi.GetValue(domainObject, null);

                                if (child != null)
                                {
                                    if (!childObjects.ContainsKey(child.Id))
                                    {
                                        childObjects.Add(child.Id, child);
                                    }
                                }

                            }
                            else if (pi.PropertyType.Name == typeof(IList<object>).Name)
                            {
                                if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]))
                                {
                                    foreach (object subObj in ((System.Collections.ICollection)pi.GetValue(domainObject, null)))
                                    {
                                        IDomainObject child = (IDomainObject)subObj;

                                        if (!childObjects.ContainsKey(child.Id))
                                        {
                                            childObjects.Add(child.Id, child);
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType))
                            {
                                IDomainObject child = (IDomainObject)pi.GetValue(domainObject, null);

                                if (child != null)
                                {
                                    if (!returnList.ContainsKey(child.Id))
                                    {
                                        returnList.Add(child.Id, modelService.GetDomainObjectType(child));
                                    }
                                }
                            }
                            else if (pi.PropertyType.Name == typeof(IList<object>).Name)
                            {
                                if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]))
                                {
                                    foreach (object subObj in ((System.Collections.ICollection)pi.GetValue(domainObject, null)))
                                    {
                                        IDomainObject child = (IDomainObject)subObj;

                                        if (!returnList.ContainsKey(child.Id))
                                        {
                                            returnList.Add(child.Id, modelService.GetDomainObjectType(child));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (IDomainObject child in childObjects.Values)
                {
                    Dictionary<Guid, Type> referencesFromChild = GetObjectIdsReferencedByDomainObject(child);

                    foreach (KeyValuePair<Guid, Type> reference in referencesFromChild)
                    {
                        if (!returnList.ContainsKey(reference.Key))
                        {
                            returnList.Add(reference.Key, reference.Value);
                        }
                    }
                }
            }

            return returnList;
        }

        private class sortStruct
        {
            public DeltaListEntry listEntry;
            public List<Guid> createdAndDeletedIds;
            public Dictionary<Guid, Type> references;
            public bool sorted;
        }

        private void ApplyDeltaNewChangeRecursivly(DeltaListEntry delta)
        {
            if (delta.CurrentObject_CurrentVersion != null)
            {
                foreach (ObjectChangeDescription change in delta.Changes)
                {
                    if (change.ChangedProperty != null)
                    {
                        if ((change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name && typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType.GetGenericArguments()[0])) || typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType))
                        {

                            if (change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name)
                            {
                                object currentListObject = change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                                Dictionary<Guid, object> currentListObjects = new Dictionary<Guid, object>();

                                foreach (object subObj in ((System.Collections.ICollection)currentListObject))
                                {
                                    currentListObjects.Add(((Guid)subObj.GetType().GetProperty("Id").GetValue(subObj, null)), subObj);
                                }


                                if (change.ChangeType == ChangeTypes.New)
                                {
                                    Guid newid = (change.NewValue is Guid ? (Guid)change.NewValue : ((IDomainObject)change.NewValue).Id);

                                    if (!currentListObjects.ContainsKey(newid))
                                    {
                                        IDomainObject newObj = null;

                                        if (change.NewValue is Guid)
                                        {
                                            newObj = GetDomainObjectFromChangeLookUpOrDB(newid, change.ChangedProperty.PropertyType.GetGenericArguments()[0]);
                                        }
                                        else
                                        {
                                            newObj = (IDomainObject)change.NewValue;
                                            if (!delta.PreviewMode)
                                            {
                                                FixReferencesForApplyObject(((IDomainObject)newObj));
                                            }
                                        }

                                        currentListObject.GetType().GetMethod("Add").Invoke(currentListObject, new object[] { newObj });
                                    }
                                }
                            }
                            else //DomainObject
                            {
                                if (change.ChangeType == ChangeTypes.New)
                                {
                                    IDomainObject newobj = null;
                                    if (change.NewValue is Guid)
                                    {
                                        newobj = GetDomainObjectFromChangeLookUpOrDB((Guid)change.NewValue, change.ChangedProperty.PropertyType);
                                    }
                                    else
                                    {
                                        newobj = ((IDomainObject)change.NewValue);
                                        if (!delta.PreviewMode)
                                        {
                                            FixReferencesForApplyObject(((IDomainObject)newobj));
                                        }
                                    }

                                    if (newobj != null)
                                    {
                                        change.ChangedProperty.SetValue(delta.CurrentObject_CurrentVersion, newobj, null);
                                    }
                                }
                                else if (change.ChangeType == ChangeTypes.Changed)
                                {
                                    object changeObj = null;

                                    if (change.NewValue is Guid)
                                    {
                                        Guid newId = (change.NewValue is Guid ? (Guid)change.NewValue : ((IDomainObject)change.NewValue).Id);
                                        changeObj = GetDomainObjectFromChangeLookUpOrDB((Guid)newId, change.ChangedProperty.PropertyType);
                                    }
                                    else
                                    {
                                        changeObj = change.NewValue;

                                        if (!delta.PreviewMode)
                                        {
                                            if (changeObj != null)
                                            {
                                                FixReferencesForApplyObject(((IDomainObject)changeObj));
                                            }
                                        }
                                    }

                                    change.ChangedProperty.SetValue(delta.CurrentObject_CurrentVersion, changeObj, null);
                                }
                            }
                        }
                        else
                        {
                            object baseValue = change.OldValue;
                            object currentValue = change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                            change.ChangedProperty.SetValue(delta.CurrentObject_CurrentVersion, change.NewValue, null);
                        }
                    }
                }

                foreach (DeltaListEntry childDelta in delta.ChildObjects)
                {
                    if (typeof(IDomainObject).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                    {
                        ApplyDeltaNewChangeRecursivly(childDelta);
                    }
                    else if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                    {
                        ApplyDeltaRecursivly_VisualTree(childDelta, null);
                        //childDelta.ParentPropery.SetValue(delta.CurrentObject_CurrentVersion, childDelta.CurrentObject_CurrentVersion, null);
                    }
                    else if (typeof(RuleSet).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                    {
                        ApplyDeltaRecursivly_RuleSetAndRules(childDelta);
                    }
                }

                //modelService.MergeSaveDomainObject((IDomainObject)delta.CurrentObject_CurrentVersion);
            }
            else
            {
                if (delta.CurrentObject_NewVersion != null)
                {
                    //modelService.SaveDomainObject((IDomainObject)delta.CurrentObject_NewVersion);
                }
            }
        }

        private void FixReferencesForApplyObject(IDomainObject newObject)
        {
            foreach (PropertyInfo pi in newObject.GetType().GetProperties())
            {
                if ((pi.PropertyType.Name == typeof(IList<object>).Name && typeof(IDomainObject).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0])) || typeof(IDomainObject).IsAssignableFrom(pi.PropertyType))
                {
                    if (pi.PropertyType.Name == typeof(IList<object>).Name)
                    {
                        Type listMemberType = pi.PropertyType.GetGenericArguments()[0];
                        Dictionary<int, object> indexList = new Dictionary<int, object>();
                        object currentListObject = pi.GetValue(newObject, null);

                        foreach (object subObj in ((System.Collections.ICollection)currentListObject))
                        {
                            int index = (int)pi.PropertyType.GetMethod("IndexOf").Invoke(currentListObject, new object[] { subObj });
                            indexList.Add(index, subObj);
                        }

                        bool idRefernece = false;
                        if (pi.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length > 0)
                        {
                            idRefernece = true;
                        }

                        foreach (KeyValuePair<int, object> indexObj in indexList)
                        {
                            if (idRefernece)
                            {
                                object currentRefObj = null;

                                if (CurrentDomainObjectLookUp.ContainsKey(indexObj.Value.GetType()) && CurrentDomainObjectLookUp[indexObj.Value.GetType()].ContainsKey(((IDomainObject)indexObj.Value).Id))
                                {
                                    currentRefObj = CurrentDomainObjectLookUp[indexObj.Value.GetType()][((IDomainObject)indexObj.Value).Id];
                                }
                                else
                                {
                                    currentRefObj = modelService.GetDomainObject(((IDomainObject)indexObj.Value).Id, indexObj.Value.GetType());
                                }

                                if (currentRefObj != null)
                                {
                                    currentListObject.GetType().GetMethod("RemoveAt").Invoke(currentListObject, new object[] { indexObj.Key });

                                    if (indexObj.Key < (int)currentListObject.GetType().GetProperty("Count").GetValue(currentListObject, null))
                                    {
                                        currentListObject.GetType().GetMethod("Insert").Invoke(currentListObject, new object[] { indexObj.Key, currentRefObj });
                                    }
                                    else
                                    {
                                        currentListObject.GetType().GetMethod("Add").Invoke(currentListObject, new object[] { currentRefObj });
                                    }
                                }
                            }
                            else
                            {
                                FixReferencesForApplyObject(((IDomainObject)indexObj.Value));
                            }
                        }
                    }
                    else
                    {
                        if (pi.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true).Length > 0)
                        {
                            object refObj = pi.GetValue(newObject, null);
                            if (refObj != null)
                            {
                                if (CurrentDomainObjectLookUp.ContainsKey(refObj.GetType()) && CurrentDomainObjectLookUp[refObj.GetType()].ContainsKey(((IDomainObject)refObj).Id))
                                {
                                    object currentRefObj = CurrentDomainObjectLookUp[refObj.GetType()][((IDomainObject)refObj).Id];
                                    pi.SetValue(newObject, currentRefObj, null);
                                }
                                else
                                {
                                    object currentRefObj = modelService.GetDomainObject(((IDomainObject)refObj).Id, refObj.GetType());
                                    if (currentRefObj != null)
                                    {
                                        pi.SetValue(newObject, currentRefObj, null);
                                    }
                                }
                            }
                        }
                        else
                        {
                            object childObject = pi.GetValue(newObject, null);

                            if (childObject != null)
                            {
                                FixReferencesForApplyObject(((IDomainObject)childObject));
                            }
                        }
                    }
                }
            }
        }

        private void ApplyDeltaDeletesRecursivly(DeltaListEntry delta)
        {
            //Delete of hole object
            if (delta.CurrentObject_NewVersion == null & delta.CurrentObject_OldVersion != null)
            {
                //if (typeof(IDomainObject).IsAssignableFrom(delta.CurrentObject_OldVersion.GetType()))
                //{
                //    IDomainObject chkObj = modelService.GetDomainObject(((IDomainObject)delta.CurrentObject_OldVersion).Id, delta.CurrentObject_OldVersion.GetType());

                //    if (chkObj != null)
                //    {
                //        modelService.DeleteDomainObject((IDomainObject)delta.CurrentObject_OldVersion);

                //    }
                //}
                return;
            }

            if (delta.CurrentObject_CurrentVersion != null)
            {
                foreach (ObjectChangeDescription change in delta.Changes)
                {
                    if (change.ChangedProperty != null)
                    {
                        if ((change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name && typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType.GetGenericArguments()[0])) || typeof(IDomainObject).IsAssignableFrom(change.ChangedProperty.PropertyType))
                        {

                            if (change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name)
                            {
                                if (change.ChangeType == ChangeTypes.Deleted)
                                {
                                    object currentListObject = change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                                    Dictionary<Guid, object> currentListObjects = new Dictionary<Guid, object>();

                                    foreach (object subObj in ((System.Collections.ICollection)currentListObject))
                                    {
                                        currentListObjects.Add(((Guid)subObj.GetType().GetProperty("Id").GetValue(subObj, null)), subObj);
                                    }

                                    Guid delid = (change.OldValue is Guid ? (Guid)change.OldValue : ((IDomainObject)change.OldValue).Id);

                                    if (currentListObjects.ContainsKey(delid))
                                    {
                                        currentListObject.GetType().GetMethod("Remove").Invoke(currentListObject, new object[] { currentListObjects[delid] });
                                    }
                                }
                            }
                            else //DomainObject
                            {
                                if (change.ChangeType == ChangeTypes.Deleted)
                                {
                                    change.ChangedProperty.SetValue(delta.CurrentObject_CurrentVersion, null, null);
                                }
                            }
                        }
                    }
                }


                //????? kanske inte behövs här. Kanske bara i change
                //foreach (DeltaListEntry childDelta in delta.ChildObjects)
                //{
                //    if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                //    {
                //        //ApplyDeltaDeletesRecursivly_VisualTree(childDelta);
                //    }
                //}
                //--------------------------------------------------------

                //modelService.MergeSaveDomainObject((IDomainObject)delta.CurrentObject_CurrentVersion);

                foreach (DeltaListEntry childDelta in delta.ChildObjects)
                {
                    if (typeof(IDomainObject).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                    {
                        ApplyDeltaDeletesRecursivly(childDelta);
                    }
                }
            }
        }

        private void ApplyDeltaRecursivly_VisualTree(DeltaListEntry delta, DataAccess.Domain.VisualModel.UXComponent parentComponent)
        {
            if (delta.CurrentObject_CurrentVersion != null)
            {
                foreach (ObjectChangeDescription change in delta.Changes.OrderBy(c => c.ChangeType))
                {
                    if (change.ChangeType == ChangeTypes.New || change.ChangeType == ChangeTypes.Deleted || change.ChangeType == ChangeTypes.Moved)
                    {
                        DataAccess.Domain.VisualModel.UXChildCollection currentChildCollection = null;
                        List<DataAccess.Domain.VisualModel.UXComponent> currentChildCompList = new List<DataAccess.Domain.VisualModel.UXComponent>();
                        DataAccess.Domain.VisualModel.UXLayoutGrid layoutGrid = null;

                        if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                        {
                            currentChildCollection = ((DataAccess.Domain.VisualModel.UXGroupBox)delta.CurrentObject_CurrentVersion).Container.Children;

                            foreach (DataAccess.Domain.VisualModel.UXComponent comp in currentChildCollection)
                            {
                                if (typeof(DataAccess.Domain.VisualModel.UXStackPanel).IsAssignableFrom(comp.GetType()))
                                {
                                    foreach (DataAccess.Domain.VisualModel.UXComponent childComp in ((DataAccess.Domain.VisualModel.UXStackPanel)comp).Children)
                                    {
                                        currentChildCompList.Add(childComp);
                                    }
                                }
                                else
                                {
                                    currentChildCompList.Add(comp);
                                }
                            }

                            layoutGrid = (DataAccess.Domain.VisualModel.UXLayoutGrid)((DataAccess.Domain.VisualModel.UXGroupBox)delta.CurrentObject_CurrentVersion).Container;
                        }
                        else if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                        {
                            currentChildCollection = ((DataAccess.Domain.VisualModel.UXContainer)delta.CurrentObject_CurrentVersion).Children;
                            currentChildCompList = currentChildCollection.ToList();
                        }

                        if (currentChildCollection != null)
                        {
                            if (change.ChangeType == ChangeTypes.New)
                            {
                                bool found = false;

                                string newCompId = (change.NewValue.GetType().GetProperty("MetaId") != null ? change.NewValue.GetType().GetProperty("MetaId").GetValue(change.NewValue, null).ToString() : ((DataAccess.Domain.VisualModel.UXComponent)change.NewValue).Name);

                                foreach (DataAccess.Domain.VisualModel.UXComponent comp in currentChildCompList)
                                {
                                    string currentCompId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);

                                    if (currentCompId == newCompId)
                                    {
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found)
                                {
                                    if (layoutGrid != null)
                                    {
                                        int row = Convert.ToInt32(change.NewComponentOrder.Split(':')[0]);
                                        int col = Convert.ToInt32(change.NewComponentOrder.Split(':')[1]);

                                        layoutGrid.AddComponent(col, row, (DataAccess.Domain.VisualModel.UXComponent)change.NewValue);
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(change.NewComponentOrder) > -1 && Convert.ToInt32(change.NewComponentOrder) < currentChildCollection.Count)
                                        {
                                            currentChildCollection.Insert(Convert.ToInt32(change.NewComponentOrder), (DataAccess.Domain.VisualModel.UXComponent)change.NewValue);
                                        }
                                        else
                                        {
                                            currentChildCollection.Add((DataAccess.Domain.VisualModel.UXComponent)change.NewValue);
                                        }
                                    }
                                }
                            }
                            else if (change.ChangeType == ChangeTypes.Deleted)
                            {
                                DataAccess.Domain.VisualModel.UXComponent found = null;

                                string newCompId = (change.OldValue.GetType().GetProperty("MetaId") != null ? change.OldValue.GetType().GetProperty("MetaId").GetValue(change.OldValue, null).ToString() : ((DataAccess.Domain.VisualModel.UXComponent)change.OldValue).Name);

                                foreach (DataAccess.Domain.VisualModel.UXComponent comp in currentChildCompList)
                                {
                                    string currentCompId = (comp.GetType().GetProperty("MetaId") != null ? comp.GetType().GetProperty("MetaId").GetValue(comp, null).ToString() : comp.Name);

                                    if (currentCompId == newCompId)
                                    {
                                        found = comp;
                                        break;
                                    }
                                }

                                if (found != null)
                                {
                                    if (layoutGrid != null)
                                    {
                                        layoutGrid.RemoveComponent(found);
                                    }
                                    else
                                    {
                                        currentChildCollection.Remove(found);
                                    }
                                }
                            }
                            else if (change.ChangeType == ChangeTypes.Moved)
                            {
                                int foundAt = -1;

                                string movedCompId = (change.MovedComponent.GetType().GetProperty("MetaId") != null ? change.MovedComponent.GetType().GetProperty("MetaId").GetValue(change.MovedComponent, null).ToString() : change.MovedComponent.Name);

                                for (int i = 0; i < currentChildCompList.Count; i++)
                                {
                                    string currentChildId = (currentChildCompList[i].GetType().GetProperty("MetaId") != null ? currentChildCompList[i].GetType().GetProperty("MetaId").GetValue(currentChildCompList[i], null).ToString() : currentChildCompList[i].Name);

                                    if (currentChildId == movedCompId)
                                    {
                                        foundAt = i;
                                        break;
                                    }
                                }

                                if (foundAt > -1)
                                {
                                    DataAccess.Domain.VisualModel.UXComponent comp = currentChildCompList[foundAt];

                                    if (layoutGrid != null)
                                    {
                                        int row = Convert.ToInt32(change.NewValue.ToString().Split(':')[0]);
                                        int col = Convert.ToInt32(change.NewValue.ToString().Split(':')[1]);

                                        DataAccess.Domain.VisualModel.UXComponent currentCellComp = layoutGrid.GetComponent(row, col);
                                        string currentCellCompId = string.Empty;

                                        if (currentCellComp != null)
                                        {
                                            currentCellCompId = (currentCellComp.GetType().GetProperty("MetaId") != null ? currentCellComp.GetType().GetProperty("MetaId").GetValue(currentCellComp, null).ToString() : currentCellComp.Name);
                                        }

                                        if (string.IsNullOrEmpty(currentCellCompId) || currentCellCompId != movedCompId)
                                        {
                                            layoutGrid.RemoveComponent(comp);
                                            layoutGrid.AddComponent(col, row, comp);
                                        }

                                    }
                                    else
                                    {
                                        if (foundAt != Convert.ToInt32(change.NewValue))
                                        {
                                            currentChildCollection.RemoveAt(foundAt);

                                            if (currentChildCollection.Count > Convert.ToInt32(change.NewValue))
                                            {
                                                currentChildCollection.Insert(Convert.ToInt32(change.NewValue), comp);
                                            }
                                            else
                                            {
                                                currentChildCollection.Add(comp);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        if (change.ChangedProperty != null)
                        {
                            change.ChangedProperty.SetValue(delta.CurrentObject_CurrentVersion, change.NewValue, null);
                        }
                        else
                        {
                            //Changed type of componenet. Parent list update needed
                            if (parentComponent != null)
                            {
                                if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(parentComponent.GetType()) || typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(parentComponent.GetType()))
                                {
                                    DataAccess.Domain.VisualModel.UXChildCollection parentChildCollection = null;
                                    DataAccess.Domain.VisualModel.UXLayoutGrid parentLayoutGrid = null;

                                    if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(parentComponent.GetType()))
                                    {
                                        parentChildCollection = ((DataAccess.Domain.VisualModel.UXGroupBox)parentComponent).Container.Children;
                                        parentLayoutGrid = (DataAccess.Domain.VisualModel.UXLayoutGrid)((DataAccess.Domain.VisualModel.UXGroupBox)parentComponent).Container;
                                    }
                                    else if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(parentComponent.GetType()))
                                    {
                                        parentChildCollection = ((DataAccess.Domain.VisualModel.UXContainer)parentComponent).Children;
                                    }

                                    if (parentChildCollection != null)
                                    {
                                        int foundAt = -1;
                                        for (int i = 0; i < parentChildCollection.Count; i++)
                                        {
                                            if (parentChildCollection[i].Name == ((DataAccess.Domain.VisualModel.UXComponent)delta.CurrentObject_CurrentVersion).Name)
                                            {
                                                foundAt = i;
                                                break;
                                            }
                                        }

                                        if (foundAt > -1)
                                        {

                                            if (parentLayoutGrid != null)
                                            {
                                                int col = -1;
                                                int row = -1;
                                                DataAccess.Domain.VisualModel.UXComponent foundComp = null;

                                                foreach (DataAccess.Domain.VisualModel.UXLayoutGridCell cell in parentLayoutGrid.Cells)
                                                {
                                                    if (cell.Component == parentChildCollection[foundAt])
                                                    {
                                                        col = cell.Column;
                                                        row = cell.Row;
                                                        foundComp = cell.Component;
                                                        break;
                                                    }
                                                }

                                                if (col > -1 & row > -1)
                                                {
                                                    parentLayoutGrid.RemoveComponent(foundComp);

                                                    parentLayoutGrid.AddComponent(col, row, (DataAccess.Domain.VisualModel.UXComponent)delta.CurrentObject_NewVersion);
                                                }
                                            }

                                            parentChildCollection.RemoveAt(foundAt);
                                            parentChildCollection.Insert(foundAt, (DataAccess.Domain.VisualModel.UXComponent)delta.CurrentObject_NewVersion);

                                            delta.CurrentObject_CurrentVersion = delta.CurrentObject_NewVersion;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (DeltaListEntry childDelta in delta.ChildObjects)
                {
                    if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                    {
                        ApplyDeltaRecursivly_VisualTree(childDelta, (DataAccess.Domain.VisualModel.UXComponent)delta.CurrentObject_CurrentVersion);
                    }
                    else if (typeof(RuleSetWrapper).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                    {
                        ApplyDeltaRecursivly_RuleSetAndRules(childDelta);
                    }
                }
            }
        }

        private void ApplyDeltaRecursivly_RuleSetAndRules(DeltaListEntry delta)
        {
            if (delta.CurrentObject_CurrentVersion != null)
            {
                foreach (ObjectChangeDescription change in delta.Changes)
                {
                    if (change.ChangedProperty != null)
                    {
                        if (typeof(RuleSet).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()) && change.ChangedProperty.Name == "Rules")
                        {
                            if (change.ChangeType == ChangeTypes.New)
                            {
                                if (((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Where(r => r.Name == ((Rule)change.NewValue).Name).Count() == 0)
                                {
                                    ((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Add((Rule)change.NewValue);
                                }
                            }
                            else if (change.ChangeType == ChangeTypes.Deleted)
                            {
                                Rule existingRule = ((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Where(r => r.Name == ((Rule)change.OldValue).Name).FirstOrDefault();

                                if (existingRule != null)
                                {
                                    ((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Remove(existingRule);
                                }
                            }
                        }
                        else if (typeof(Rule).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()) && change.ChangedProperty.PropertyType.Name == typeof(IList<object>).Name)
                        {
                            if (typeof(System.Workflow.Activities.Rules.RuleAction).IsAssignableFrom(change.ChangedProperty.PropertyType.GetGenericArguments()[0]))
                            {
                                IList<System.Workflow.Activities.Rules.RuleAction> currentActions = (IList<System.Workflow.Activities.Rules.RuleAction>)change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                                bool found = false;
                                for (int i = 0; i < currentActions.Count; i++)
                                {
                                    if (change.ChangeType == ChangeTypes.Changed)
                                    {
                                        if (currentActions[i].ToString() == change.OldValue.ToString())
                                        {
                                            currentActions[i] = (System.Workflow.Activities.Rules.RuleAction)change.NewValue;
                                            break;
                                        }
                                    }
                                    else if (change.ChangeType == ChangeTypes.New)
                                    {
                                        if (currentActions[i].ToString() == change.NewValue.ToString())
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    else if (change.ChangeType == ChangeTypes.Deleted)
                                    {
                                        if (currentActions[i].ToString() == change.OldValue.ToString())
                                        {
                                            currentActions.RemoveAt(i);
                                            break;
                                        }
                                    }
                                }

                                if (change.ChangeType == ChangeTypes.New && !found)
                                {
                                    currentActions.Add((System.Workflow.Activities.Rules.RuleAction)change.NewValue);
                                }
                            }
                        }
                        else
                        {
                            object baseValue = change.OldValue;
                            object currentValue = change.ChangedProperty.GetValue(delta.CurrentObject_CurrentVersion, null);

                            change.ChangedProperty.SetValue(delta.CurrentObject_CurrentVersion, change.NewValue, null);
                        }
                    }
                }

                foreach (DeltaListEntry childDelta in delta.ChildObjects)
                {
                    ApplyDeltaRecursivly_RuleSetAndRules(childDelta);
                }
            }
        }

        private void GetCurrentVersionOfObjectsRecursion(DeltaListEntry delta)
        {
            if (typeof(IDomainObject).IsAssignableFrom(delta.CurrentObject_NewVersion.GetType()))
            {
                Type classType = delta.CurrentObject_NewVersion.GetType();
                Guid id = ((IDomainObject)delta.CurrentObject_NewVersion).Id;

                if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.ContainsKey(classType))
                {
                    if (DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[classType].ContainsKey(id))
                    {
                        delta.CurrentObject_CurrentVersion = DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[classType][id];
                    }
                }


                if (delta.CurrentObject_CurrentVersion != null)
                {
                    for (int i = 0; i < delta.ChildObjects.Count; i++)
                    {
                        DeltaListEntry childDelta = delta.ChildObjects[i];

                        GetCurrentVersionOfObjectsRecursion(childDelta);
                    }
                }
            }
        }

        private void GetCurrentVersionOfComponentsRecursion(DeltaListEntry delta, List<string> componentTypeChangedInOtherIssue)
        {
            for (int i = 0; i < delta.ChildObjects.Count; i++)
            {
                DeltaListEntry childDelta = delta.ChildObjects[i];
                if (typeof(DataAccess.Domain.VisualModel.UXComponent).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                {

                    DataAccess.Domain.VisualModel.UXComponent childComponent = (DataAccess.Domain.VisualModel.UXComponent)childDelta.CurrentObject_NewVersion;

                    DataAccess.Domain.VisualModel.UXChildCollection currentChildCollection = null;
                    List<DataAccess.Domain.VisualModel.UXComponent> currentChildCompList = new List<DataAccess.Domain.VisualModel.UXComponent>();

                    if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                    {
                        currentChildCollection = ((DataAccess.Domain.VisualModel.UXGroupBox)delta.CurrentObject_CurrentVersion).Container.Children;

                        foreach (DataAccess.Domain.VisualModel.UXComponent comp in currentChildCollection)
                        {
                            if (typeof(DataAccess.Domain.VisualModel.UXStackPanel).IsAssignableFrom(comp.GetType()))
                            {
                                foreach (DataAccess.Domain.VisualModel.UXComponent childComp in ((DataAccess.Domain.VisualModel.UXStackPanel)comp).Children)
                                {
                                    currentChildCompList.Add(childComp);
                                }
                            }
                            else
                            {
                                currentChildCompList.Add(comp);
                            }
                        }

                        //layoutGrid = (DataAccess.Domain.VisualModel.UXLayoutGrid)((DataAccess.Domain.VisualModel.UXGroupBox)delta.CurrentObject_CurrentVersion).Container;
                    }
                    else if (typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(delta.CurrentObject_CurrentVersion.GetType()))
                    {
                        currentChildCollection = ((DataAccess.Domain.VisualModel.UXContainer)delta.CurrentObject_CurrentVersion).Children;
                        currentChildCompList = currentChildCollection.ToList();
                    }

                    foreach (DataAccess.Domain.VisualModel.UXComponent currentChildComp in currentChildCompList)
                    {
                        string currentChildCompId = (currentChildComp.GetType().GetProperty("MetaId") != null ? currentChildComp.GetType().GetProperty("MetaId").GetValue(currentChildComp, null).ToString() : currentChildComp.Name);
                        string childCompId = (childComponent.GetType().GetProperty("MetaId") != null ? childComponent.GetType().GetProperty("MetaId").GetValue(childComponent, null).ToString() : childComponent.Name);


                        if (childCompId == currentChildCompId)
                        {
                            childDelta.CurrentObject_CurrentVersion = currentChildComp;

                            if (childDelta.CurrentObject_NewVersion.GetType() != childDelta.CurrentObject_CurrentVersion.GetType())
                            {
                                if (childDelta.Changes.Count > 1 || (childDelta.Changes.Count == 1 && childDelta.Changes[0].ChangedProperty != null))
                                {
                                    componentTypeChangedInOtherIssue.Add(currentChildComp.Name + " changed from " + childDelta.CurrentObject_CurrentVersion.GetType().Name + " to " + childDelta.CurrentObject_NewVersion.GetType().Name);
                                }

                            }

                            break;
                        }
                    }

                    if (childDelta.CurrentObject_CurrentVersion != null)
                    {
                        //if (typeof(DataAccess.Domain.VisualModel.UXGroupBox).IsAssignableFrom(childComponent.GetType()) || typeof(DataAccess.Domain.VisualModel.UXContainer).IsAssignableFrom(childComponent.GetType()))
                        //{
                        GetCurrentVersionOfComponentsRecursion(childDelta, componentTypeChangedInOtherIssue);
                        //}
                    }
                }
                else if (typeof(RuleSetWrapper).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                {
                    childDelta.CurrentObject_CurrentVersion = ((DataAccess.Domain.VisualModel.UXComponent)delta.CurrentObject_CurrentVersion).RuleSetWrapper;

                    if (childDelta.CurrentObject_CurrentVersion != null && ((RuleSetWrapper)childDelta.CurrentObject_CurrentVersion).RuleSet != null)
                    {
                        for (int r = 0; r < childDelta.ChildObjects.Count; r++)
                        {
                            DeltaListEntry ruleDelta = childDelta.ChildObjects[r];

                            if (typeof(RuleSet).IsAssignableFrom(ruleDelta.CurrentObject_NewVersion.GetType()) || typeof(RuleSet).IsAssignableFrom(ruleDelta.CurrentObject_OldVersion.GetType()))
                            {
                                RuleSet currentRule = ((RuleSetWrapper)childDelta.CurrentObject_CurrentVersion).RuleSet;
                                ruleDelta.CurrentObject_CurrentVersion = currentRule;

                                GetCurrentVersionOfRule(ruleDelta);
                            }
                        }
                    }
                }
            }
        }

        private void GetCurrentVersionOfRule(DeltaListEntry delta)
        {
            for (int i = 0; i < delta.ChildObjects.Count; i++)
            {
                DeltaListEntry childDelta = delta.ChildObjects[i];

                Rule compareRule = null;
                if (childDelta.CurrentObject_NewVersion != null)
                {
                    compareRule = (Rule)childDelta.CurrentObject_NewVersion;
                }
                else if (childDelta.CurrentObject_OldVersion != null)
                {
                    compareRule = (Rule)childDelta.CurrentObject_OldVersion;
                }

                if (compareRule != null)
                {
                    if (typeof(Rule).IsAssignableFrom(childDelta.CurrentObject_NewVersion.GetType()))
                    {
                        if (((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Where(r => r.Name == compareRule.Name).Count() > 0)
                        {
                            childDelta.CurrentObject_CurrentVersion = ((RuleSet)delta.CurrentObject_CurrentVersion).Rules.Where(r => r.Name == compareRule.Name).FirstOrDefault();
                        }
                    }
                }
            }
        }


        private IDomainObject GetDomainObjectFromChangeLookUpOrDB(Guid id, Type classType)
        {
            if (CreatedObjectsLookUp != null)
            {
                if (CreatedObjectsLookUp.ContainsKey(classType))
                {
                    if (CreatedObjectsLookUp[classType].ContainsKey(id))
                    {
                        IDomainObject newObject = CreatedObjectsLookUp[classType][id];
                        FixReferencesForApplyObject(newObject);
                        return newObject;
                    }
                }
            }

            return modelService.GetDomainObject(id, classType);
        }

        #endregion
    }
}
