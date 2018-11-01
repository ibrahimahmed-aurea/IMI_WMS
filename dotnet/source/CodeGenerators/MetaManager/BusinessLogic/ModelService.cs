using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess;
using Spring.Context;
using Spring.Context.Support;
using Spring.Transaction.Interceptor;
using System.Data;
using System.Reflection;
using Spring.Data.NHibernate.Generic.Support;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Diagnostics;
using System.Configuration;


namespace Cdc.MetaManager.BusinessLogic
{
    public class ModelService : IModelService
    {
        private IConfigurationManagementService ConfigurationManagementService { get; set; }

        private IApplicationContext ctx = null;

        private void GetContext()
        {
            // Get application service context
            if (ctx == null)
            {
                ctx = ContextRegistry.GetContext();
            }
        }

        #region IDomainModelService Members

        public event StatusChangedDelegate StatusChanged;

        public event DomainObjectDeletedDelegate DomainObjectDeleted;

        public event DomainObjectAddedDelegate DomainObjectAdded;

        public event DomainObjectChangedDelegate DomainObjectChanged;

        [Transaction(ReadOnly = true)]
        public IList<IVersionControlled> GetAllVersionControlledObjectsInApplication(Guid applicationId)
        {
            List<IVersionControlled> returnList = new List<IVersionControlled>();
            IEnumerable<Type> domainObjectTypes = Assembly.GetAssembly(typeof(IVersionControlled)).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IVersionControlled)));

            foreach (Type domainObjectType in domainObjectTypes)
            {
                returnList.AddRange(GetAllDomainObjectsByApplicationId(applicationId, domainObjectType).Cast<IVersionControlled>());
            }

            return returnList;

        }

        private Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>> GetReferencingMaps(IDomainObject domainObject, List<PropertyMap> sourceMaps, List<Type> allDomainClasses, List<Type> classesReferencedByComponents, List<Type> allClassesWithUXContainer, Dictionary<PropertyMap, List<MappedProperty>> deletedMPsInMaps, Dictionary<Guid, Dictionary<IDomainObject, List<PropertyInfo>>> xmlLookup)
        {
            Type classType = GetDomainObjectType(domainObject);

            List<Type> allReferensingClasses = allDomainClasses.Where(t => t.GetProperties().Where(p => p.PropertyType == classType).Count() > 0).ToList();

            List<Type> allReferensingClassesToDialog = allDomainClasses.Where(t => t.GetProperties().Where(p => p.PropertyType == typeof(Dialog)).Count() > 0).ToList();
            List<Dialog> dialogsForIntefaceView = new List<Dialog>();
            //Special treatment if classType is View
            if (classType == typeof(View))
            {
                string query = "from Dialog d where d.InterfaceView.Id = '" + domainObject.Id.ToString() + "'";
                dialogsForIntefaceView = GetDomainObjectsByQuery<Dialog>(query).ToList();
            }

            Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>> sourceMapsWithRefMaps = new Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>>();

            if (domainObject is IMappedObject)
            {

                //Get the maps from object
                SetTargetChoice setTarget = SetTargetChoice.Yes;
                Guid requestMapId = ((IMappedObject)domainObject).GetRequestMapId(null, out setTarget);
                Guid responseMapId = ((IMappedObject)domainObject).GetResponseMapId(null, out setTarget);
                PropertyMap requestMap = null;
                PropertyMap responseMap = null;

                if (requestMapId != Guid.Empty)
                {
                    requestMap = sourceMaps.Where(m => m.Id == requestMapId).FirstOrDefault();
                    if (requestMap != null)
                    {
                        sourceMapsWithRefMaps.Add(requestMap, new Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>());
                    }
                }

                if (responseMapId != Guid.Empty)
                {
                    responseMap = sourceMaps.Where(m => m.Id == responseMapId).FirstOrDefault();
                    if (responseMap != null)
                    {
                        sourceMapsWithRefMaps.Add(responseMap, new Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>());
                    }
                }


                //Get Source Mappings
                //==================================================================================================================================================================
                //From referensing objects
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                foreach (Type typ in allReferensingClasses)
                {
                    string qry;
                    string propertyName = typ.GetProperties().Where(p => p.PropertyType == classType).ToList()[0].Name;

                    qry = "from " + typ.Name + " o ";
                    qry += "where o." + propertyName + ".Id = '" + domainObject.Id.ToString() + "'";

                    IList<IDomainObject> objList = GetDomainObjectsByQuery(qry);

                    foreach (IDomainObject refDomainObject in objList)
                    {
                        if (refDomainObject is IMappedObject)
                        {

                            GetMapsFromObjectForSyncronisation(requestMap, responseMap, domainObject, refDomainObject, ((IMappedObject)refDomainObject), sourceMapsWithRefMaps, MapSyncOption.Source);
                        }
                    }
                }
                foreach (Dialog dialog in dialogsForIntefaceView)
                {
                    foreach (Type typ in allReferensingClassesToDialog)
                    {
                        string qry;
                        string propertyName = typ.GetProperties().Where(p => p.PropertyType == typeof(Dialog)).ToList()[0].Name;

                        qry = "from " + typ.Name + " o ";
                        qry += "where o." + propertyName + ".Id = '" + dialog.Id.ToString() + "'";

                        IList<IDomainObject> objList = GetDomainObjectsByQuery(qry);

                        foreach (IDomainObject refDomainObject in objList)
                        {
                            if (refDomainObject is IMappedObject)
                            {
                                GetMapsFromObjectForSyncronisation(requestMap, responseMap, dialog, refDomainObject, ((IMappedObject)refDomainObject), sourceMapsWithRefMaps, MapSyncOption.Source);
                            }
                        }
                    }
                }
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------

                //From Component reference
                //------------------------------------------------------------------------------------------------------------------------------------------------------------------
                Dictionary<IDomainObject, Type> tempDomainObjectAndType = new Dictionary<IDomainObject, Type>();
                //Special treatment if classType is View
                //if (classType == typeof(View))
                //{
                //    string query = "from Dialog d where d.InterfaceView.Id = '" + domainObject.Id.ToString() + "'";
                //    IList<Dialog> dialogs = GetDomainObjectsByQuery<Dialog>(query);
                //    foreach (Dialog dialog in dialogs)
                //    {
                //        tempDomainObjectAndType.Add(dialog, typeof(Dialog));
                //    }
                //}
                foreach (Dialog dialog in dialogsForIntefaceView)
                {
                    tempDomainObjectAndType.Add(dialog, typeof(Dialog));
                }

                tempDomainObjectAndType.Add(domainObject, classType);

                foreach (KeyValuePair<IDomainObject, Type> objectAndType in tempDomainObjectAndType)
                {
                    if (classesReferencedByComponents.Contains(objectAndType.Value))
                    {
                        if (xmlLookup.ContainsKey(objectAndType.Key.Id))
                        {
                            foreach (IDomainObject refObj in xmlLookup[objectAndType.Key.Id].Keys)
                            {
                                foreach (PropertyInfo pi in xmlLookup[objectAndType.Key.Id][refObj])
                                {
                                    //Get all componenets referensing the current domain object
                                    List<IMappedObject> refComponenets = GetAllMappedComponentsInVisualTreeReferingObject(((UXComponent)pi.GetValue(refObj, null)), objectAndType.Key.Id);

                                    foreach (IMappedObject mappedObj in refComponenets)
                                    {
                                        GetMapsFromObjectForSyncronisation(requestMap, responseMap, objectAndType.Key, refObj, mappedObj, sourceMapsWithRefMaps, MapSyncOption.Source);
                                    }
                                }
                            }
                        }
                    }
                }

                //Get componenet maps referensing parent object
                if (allClassesWithUXContainer.Contains(classType))
                {
                    tempDomainObjectAndType.Clear();

                    foreach (Dialog dialog in dialogsForIntefaceView)
                    {
                        if (dialog.SearchPanelView != null)
                        {
                            tempDomainObjectAndType.Add(dialog.SearchPanelView, typeof(View));
                        }
                    }

                    tempDomainObjectAndType.Add(domainObject, classType);

                    foreach (KeyValuePair<IDomainObject, Type> objectAndType in tempDomainObjectAndType)
                    {
                        PropertyMap tempRequestMap = requestMap;
                        PropertyMap tempResponseMap = responseMap;
                        SetTargetChoice tmpTC;
                        if (((IMappedObject)objectAndType.Key).GetRequestMapId(null, out tmpTC) == ((IMappedObject)objectAndType.Key).GetResponseMapId(null, out tmpTC))
                        {
                            tempRequestMap = requestMap;
                            tempResponseMap = requestMap;
                        }

                        if (allClassesWithUXContainer.Contains(objectAndType.Value))
                        {
                            //Get the visual tree propertys
                            foreach (PropertyInfo pi in objectAndType.Value.GetProperties().Where(p => p.PropertyType == typeof(UXContainer)))
                            {
                                //Get all components with responsemap referensing parent i.e. no referensing object
                                List<IMappedObject> refComponenets = GetAllMappedComponentsInVisualTreeReferingObject(((UXComponent)pi.GetValue(objectAndType.Key, null)), Guid.Empty, true);

                                foreach (IMappedObject mappedObj in refComponenets)
                                {
                                    GetMapsFromObjectForSyncronisation(tempRequestMap, tempResponseMap, objectAndType.Key, objectAndType.Key, mappedObj, sourceMapsWithRefMaps, MapSyncOption.Source);
                                }
                            }
                        }
                    }
                }

                //Get Target Mappings
                //==================================================================================================================================================================
                string ids = string.Empty;
                string trgqry = string.Empty;
                PropertyMap sourceMap = requestMap;

                for (int i = 0; i < 2; i++)
                {
                    ids = string.Empty;
                    trgqry = string.Empty;

                    if (sourceMap != null)
                    {
                        if (deletedMPsInMaps.ContainsKey(sourceMap))
                        {
                            if (deletedMPsInMaps[sourceMap].Count > 0)
                            {
                                foreach (MappedProperty mp in deletedMPsInMaps[sourceMap])
                                {
                                    if (ids.EndsWith("'")) { ids += ","; }

                                    ids += "'" + mp.Id.ToString() + "'";
                                }

                                trgqry = "from " + typeof(MappedProperty).Name + " o where o.TargetMappedProperty.Id IN (" + ids + ")";

                                IList<MappedProperty> trgMPs = GetDomainObjectsByQuery<MappedProperty>(trgqry);

                                List<Guid> trgMapIds = trgMPs.Select(m => m.PropertyMap.Id).Distinct().ToList();

                                foreach (Guid trgMapId in trgMapIds)
                                {
                                    PropertyMap trgMap = GetDomainObject<PropertyMap>(trgMapId);


                                    InsertReferenseMapInSyncronisationLookup(sourceMap, trgMap, trgMap, MapSyncOption.Target, SetTargetChoice.No, sourceMapsWithRefMaps);

                                }
                            }
                        }
                    }

                    sourceMap = responseMap;
                }
                //==================================================================================================================================================================

            }

            return sourceMapsWithRefMaps;
        }

        private void GetMapsFromObjectForSyncronisation(PropertyMap requestMap, PropertyMap responseMap, IDomainObject domainObject, IDomainObject refDomainObject, IMappedObject refMappedObject, Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>> sourceMapsWithRefMaps, MapSyncOption mapSyncOption)
        {
            SetTargetChoice setTarget = SetTargetChoice.No;
            IDomainObject vcParent = null;

            if (requestMap != null)
            {
                Guid refRequestMapId = refMappedObject.GetRequestMapId(domainObject, out setTarget);
                if (refRequestMapId != Guid.Empty)
                {
                    PropertyMap refRequestMap = GetDomainObject<PropertyMap>(refRequestMapId);

                    vcParent = InsertReferenseMapInSyncronisationLookup(requestMap, refRequestMap, refDomainObject, mapSyncOption, setTarget, sourceMapsWithRefMaps);
                }
            }

            if (responseMap != null)
            {
                Guid refResponseMapId = refMappedObject.GetResponseMapId(domainObject, out setTarget);
                if (refResponseMapId != Guid.Empty)
                {
                    PropertyMap refResponseMap = GetDomainObject<PropertyMap>(refResponseMapId);

                    if (vcParent == null)
                    {
                        vcParent = refDomainObject;
                    }

                    InsertReferenseMapInSyncronisationLookup(responseMap, refResponseMap, vcParent, mapSyncOption, setTarget, sourceMapsWithRefMaps);
                }
            }
        }

        private IVersionControlled InsertReferenseMapInSyncronisationLookup(PropertyMap sourceMap, PropertyMap refMap, IDomainObject refDomainObject, MapSyncOption mapSyncOption, SetTargetChoice setTarget, Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>> sourceMapsWithRefMaps)
        {
            List<IDomainObject> parents = new List<IDomainObject>();
            IVersionControlled vcParent = null;
            if (typeof(IVersionControlled).IsAssignableFrom(GetDomainObjectType(refDomainObject)))
            {
                vcParent = (IVersionControlled)refDomainObject;
            }
            else
            {
                vcParent = GetVersionControlledParent(refDomainObject, out parents)[0];
            }

            if (vcParent != null)
            {
                if (!sourceMapsWithRefMaps[sourceMap].ContainsKey(vcParent))
                {
                    sourceMapsWithRefMaps[sourceMap].Add(vcParent, new Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>());
                }

                if (!sourceMapsWithRefMaps[sourceMap][vcParent].ContainsKey(refMap))
                {
                    sourceMapsWithRefMaps[sourceMap][vcParent].Add(refMap, new KeyValuePair<MapSyncOption, SetTargetChoice>(mapSyncOption, setTarget));
                }
            }
            else
            {
                //Städrutin??
            }

            return vcParent;
        }

        private List<IMappedObject> GetAllMappedComponentsInVisualTreeReferingObject(UXComponent component, Guid referencedObjectId, bool noReferencedObject = false)
        {
            Type componenetType = NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(component);
            List<IMappedObject> returnList = new List<IMappedObject>();

            if (component is UXGroupBox)
            {
                if (((UXGroupBox)component).Children != null)
                {
                    foreach (UXComponent childComp in ((UXGroupBox)component).Children)
                    {
                        returnList.AddRange(GetAllMappedComponentsInVisualTreeReferingObject(childComp, referencedObjectId, noReferencedObject));
                    }
                }

                if (((UXGroupBox)component).Container != null && ((UXGroupBox)component).Container.Children != null)
                {
                    foreach (UXComponent childComp in ((UXGroupBox)component).Container.Children)
                    {
                        returnList.AddRange(GetAllMappedComponentsInVisualTreeReferingObject(childComp, referencedObjectId, noReferencedObject));
                    }
                }
            }
            else if (component is UXContainer)
            {
                if (((UXContainer)component).Children != null)
                {
                    foreach (UXComponent childComp in ((UXContainer)component).Children)
                    {
                        returnList.AddRange(GetAllMappedComponentsInVisualTreeReferingObject(childComp, referencedObjectId, noReferencedObject));
                    }
                }
            }
            else if (typeof(IMappedObject).IsAssignableFrom(componenetType))
            {
                if (noReferencedObject || (componenetType.GetProperties().Where(p => p.PropertyType == typeof(Guid) && ((Guid)p.GetValue(component, null)) == referencedObjectId).Count() > 0))
                {
                    returnList.Add(((IMappedObject)component));
                }
            }

            return returnList;
        }

        [Transaction(ReadOnly = false)]
        public void StartSynchronizePropertyMapsInObjects(IDomainObject domainObject, List<IDomainObject> objectsToSave = null, List<IDomainObject> objectsToDelete = null)
        {
            List<PropertyInfo> mapsPi = GetDomainObjectType(domainObject).GetProperties().Where(p => p.PropertyType == typeof(PropertyMap)).ToList();

            List<PropertyMap> maps = new List<PropertyMap>();

            foreach (PropertyInfo pi in mapsPi)
            {
                PropertyMap tmpMap = (PropertyMap)pi.GetValue(domainObject, null);
                if (tmpMap != null)
                {
                    maps.Add(tmpMap);
                }
            }

            if (maps.Count == 0) { return; }

            StartSyncronisationRecursion(domainObject, maps, objectsToSave, objectsToDelete);

        }


        [Transaction(ReadOnly = false)]
        public void SynchronizePropertyMapChain(PropertyMap sourceMap, List<IDomainObject> objectsToDelete)
        {
            if (sourceMap != null)
            {
                List<IDomainObject> parents = new List<IDomainObject>();
                IVersionControlled vcParent = null;

                vcParent = GetVersionControlledParent(sourceMap, out parents)[0];

                StartSyncronisationRecursion(vcParent, new List<PropertyMap> { sourceMap }, null, objectsToDelete);
            }
        }

        private void StartSyncronisationRecursion(IDomainObject domainObject, List<PropertyMap> sourceMaps, List<IDomainObject> objectsToSave = null, List<IDomainObject> objectsToDelete = null)
        {
            if (objectsToDelete == null)
            {
                objectsToDelete = new List<IDomainObject>();
            }

            if (objectsToSave != null)
            {
                foreach (PropertyMap pm in sourceMaps)
                {
                    foreach (MappedProperty mp in pm.MappedProperties)
                    {
                        if (mp.Id == Guid.Empty)
                        {
                            mp.Id = Guid.NewGuid();
                            objectsToSave.Add(mp);
                        }
                    }
                }
            }

            Dictionary<PropertyMap, List<MappedProperty>> deletedMappedPropertiesInSourceMaps = new Dictionary<PropertyMap, List<MappedProperty>>();

            foreach (IDomainObject deleteObj in objectsToDelete)
            {
                if (GetDomainObjectType(deleteObj) == typeof(MappedProperty))
                {
                    if (sourceMaps.Where(m => m.Id == ((MappedProperty)deleteObj).PropertyMap.Id).Count() > 0)
                    {
                        PropertyMap sourceMap = sourceMaps.Where(m => m.Id == ((MappedProperty)deleteObj).PropertyMap.Id).First();
                        if (!deletedMappedPropertiesInSourceMaps.ContainsKey(sourceMap))
                        {
                            deletedMappedPropertiesInSourceMaps.Add(sourceMap, new List<MappedProperty>());
                        }

                        if (!deletedMappedPropertiesInSourceMaps[sourceMap].Contains(((MappedProperty)deleteObj)))
                        {
                            deletedMappedPropertiesInSourceMaps[sourceMap].Add(((MappedProperty)deleteObj));
                        }
                    }
                }
            }


            List<Type> allDomainClasses = Assembly.GetAssembly(typeof(IDomainObject)).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDomainObject)) && !t.IsInterface && t.GetProperties().Where(p => p.PropertyType == typeof(PropertyMap)).Count() > 0).ToList();

            List<Type> allClassesWithUXContainer = allDomainClasses.Where(t => t.GetProperties().Where(p => p.PropertyType == typeof(UXContainer)).Count() > 0).ToList();

            //Get all DomainObject types referenced by UXComponenet 
            List<Type> classesReferencedByComponents = new List<Type>();
            List<Type> allComponenetsReferencingDomainObject = Assembly.GetAssembly(typeof(UXComponent)).GetTypes().Where(t => t.BaseType == typeof(UXComponent) && !t.IsInterface && t.GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType) && (typeof(IMappedObject).IsAssignableFrom(p.PropertyType) || typeof(IVersionControlled).IsAssignableFrom(p.PropertyType))).Count() > 0).ToList();
            foreach (Type typ in allComponenetsReferencingDomainObject)
            {
                List<PropertyInfo> tempProperties = typ.GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType) && (typeof(IMappedObject).IsAssignableFrom(p.PropertyType) || typeof(IVersionControlled).IsAssignableFrom(p.PropertyType))).ToList();

                foreach (PropertyInfo pi in tempProperties)
                {
                    if (!classesReferencedByComponents.Contains(pi.PropertyType))
                    {
                        classesReferencedByComponents.Add(pi.PropertyType);
                    }
                }
            }


            //Build Lookup for all guids in visual trees.
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[0-9ABCDEFabcdef]{8}-[0-9ABCDEFabcdef]{4}-[0-9ABCDEFabcdef]{4}-[0-9ABCDEFabcdef]{4}-[0-9ABCDEFabcdef]{12}");
            Dictionary<Guid, Dictionary<IDomainObject, List<PropertyInfo>>> xmlLookup = new Dictionary<Guid, Dictionary<IDomainObject, List<PropertyInfo>>>();

            foreach (Type typ in allClassesWithUXContainer)
            {
                IList<IDomainObject> objects = GetDomainObjectsByQuery("from  " + typ.Name + " o");

                foreach (IDomainObject obj in objects)
                {

                    foreach (PropertyInfo pi in typ.GetProperties().Where(p => p.PropertyType == typeof(UXContainer)))
                    {
                        object xmlValue = typ.GetProperty(pi.Name + "Xml").GetValue(obj, null);

                        if (xmlValue != null)
                        {
                            string tmptext = xmlValue.ToString();
                            System.Text.RegularExpressions.MatchCollection matches = regex.Matches(tmptext);
                            foreach (System.Text.RegularExpressions.Match match in matches)
                            {
                                if (match.Value != Guid.Empty.ToString())
                                {
                                    Guid tmpGuid = new Guid(match.Value);
                                    if (!xmlLookup.ContainsKey(tmpGuid))
                                    {
                                        xmlLookup.Add(tmpGuid, new Dictionary<IDomainObject, List<PropertyInfo>>());
                                    }

                                    if (!xmlLookup[tmpGuid].ContainsKey(obj))
                                    {
                                        xmlLookup[tmpGuid].Add(obj, new List<PropertyInfo>());
                                    }

                                    if (!xmlLookup[tmpGuid][obj].Contains(pi))
                                    {
                                        xmlLookup[tmpGuid][obj].Add(pi);
                                    }
                                }
                            }
                        }
                    }

                    try
                    {
                        MetaManagerServices.GetCurrentSession().Evict(obj);
                    }
                    catch (KeyNotFoundException)
                    {
                    }

                }
            }

            SynchronizePropertyMapsChain(domainObject, sourceMaps, objectsToDelete, objectsToSave, null, null, deletedMappedPropertiesInSourceMaps, allDomainClasses, classesReferencedByComponents, allClassesWithUXContainer, xmlLookup);
        }

        [Transaction(ReadOnly = false)]
        private void SynchronizePropertyMapsChain(IDomainObject domainObject, List<PropertyMap> sourceMaps, List<IDomainObject> propertyToDelete, List<IDomainObject> mapsToSave, List<IDomainObject> refObjectsToSave, List<IVersionControlled> versionControlledParents, Dictionary<PropertyMap, List<MappedProperty>> deletedMappedPropertiesInSourceMaps, List<Type> allDomainClasses, List<Type> classesReferencedByComponents, List<Type> allClassesWithUXContainer, Dictionary<Guid, Dictionary<IDomainObject, List<PropertyInfo>>> xmlLookup)
        {
            bool firstCall = false;

            if (versionControlledParents == null)
            {
                if (mapsToSave == null)
                {
                    mapsToSave = new List<IDomainObject>();
                }

                if (propertyToDelete == null)
                {
                    propertyToDelete = new List<IDomainObject>();
                }


                refObjectsToSave = new List<IDomainObject>();
                versionControlledParents = new List<IVersionControlled>();
                firstCall = true;
            }

            Dictionary<IVersionControlled, Dictionary<PropertyMap, List<MappedProperty>>> mapsToSyncronize = new Dictionary<IVersionControlled, Dictionary<PropertyMap, List<MappedProperty>>>();

            Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>> sourceMapsWithRefMaps;

            sourceMapsWithRefMaps = GetReferencingMaps(domainObject, sourceMaps, allDomainClasses, classesReferencedByComponents, allClassesWithUXContainer, deletedMappedPropertiesInSourceMaps, xmlLookup);

            foreach (PropertyMap sourceMap in sourceMapsWithRefMaps.Keys)
            {
                foreach (IVersionControlled vcObject in sourceMapsWithRefMaps[sourceMap].Keys)
                {
                    foreach (KeyValuePair<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>> mapWithProps in sourceMapsWithRefMaps[sourceMap][vcObject])
                    {
                        PropertyMap fromMap = sourceMap;
                        PropertyMap toMap = mapWithProps.Key;

                        if (fromMap.Id != toMap.Id)
                        {
                            List<MappedProperty> mappedPropertysToDeleteInToMap = new List<MappedProperty>();
                            int savedMapsToSaveCount = mapsToSave.Count;

                            if (SynchronizePropertyMaps(fromMap, ref toMap, propertyToDelete, mapWithProps.Value.Value, mapWithProps.Value.Key, mapsToSave, refObjectsToSave, versionControlledParents, mappedPropertysToDeleteInToMap))
                            {
                                if (!mapsToSyncronize.ContainsKey(vcObject))
                                {
                                    mapsToSyncronize.Add(vcObject, new Dictionary<PropertyMap, List<MappedProperty>>());
                                }
                                if (!mapsToSyncronize[vcObject].ContainsKey(toMap))
                                {
                                    mapsToSyncronize[vcObject].Add(toMap, new List<MappedProperty>());
                                }

                                mapsToSyncronize[vcObject][toMap].AddRange(mappedPropertysToDeleteInToMap);
                            }

                            if (mapsToSave.Count > savedMapsToSaveCount)
                            {
                                if (!versionControlledParents.Contains(vcObject))
                                {
                                    versionControlledParents.Add(vcObject);
                                }
                            }
                        }
                    }
                }
            }

            if (mapsToSyncronize.Count > 0)
            {
                foreach (IVersionControlled vcObject in mapsToSyncronize.Keys)
                {
                    SynchronizePropertyMapsChain(vcObject, mapsToSyncronize[vcObject].Keys.ToList(), propertyToDelete, mapsToSave, refObjectsToSave, versionControlledParents, mapsToSyncronize[vcObject], allDomainClasses, classesReferencedByComponents, allClassesWithUXContainer, xmlLookup);
                }
            }

            if (firstCall)
            {

                foreach (IVersionControlled versionControlledParent in versionControlledParents)
                {
                    if (versionControlledParent.LockedBy != Environment.UserName)
                    {
                        ConfigurationManagementService.CheckOutDomainObject(versionControlledParent.Id, GetDomainObjectType(versionControlledParent));
                        MetaManagerServices.GetCurrentSession().Flush();
                    }
                }

                refObjectsToSave.Reverse();
                foreach (IDomainObject refObj in refObjectsToSave)
                {
                    SaveDomainObject(refObj);
                }

                foreach (IDomainObject map in mapsToSave)
                {
                    if (map is QueryProperty || map is ProcedureProperty)
                    {
                        SaveDomainObject(map);
                    }
                    else
                    {
                        MergeSaveDomainObject(map);
                    }

                    if (map.IsTransient)
                    {
                        map.IsTransient = false;
                    }
                }

                propertyToDelete.Reverse();
                foreach (IDomainObject obj in propertyToDelete)
                {
                    List<IDomainObject> parents = new List<IDomainObject>();
                    if (GetDomainObjectType(obj) == typeof(MappedProperty))
                    {
                        parents.Add(((MappedProperty)obj).PropertyMap);
                    }

                    DeleteDomainObject(obj, false, parents, true);
                }
            }
        }

        [Transaction(ReadOnly = false)]
        public void CreateAndSynchronizePropertyMaps(IDomainObject fromObject, IDomainObject toObject)
        {

            Type sourceType = GetDomainObjectType(fromObject);
            Type targetType = GetDomainObjectType(toObject);

            Dictionary<PropertyInfo, PropertyMapType> sourceMaps = sourceType.GetProperties().Where(pi => pi.PropertyType == typeof(PropertyMap)).ToDictionary(k => k, v => ((PropertyMapAttribute)v.GetCustomAttributes(typeof(PropertyMapAttribute), true)[0]).Type);
            Dictionary<PropertyInfo, PropertyMapType> targetMaps = targetType.GetProperties().Where(pi => pi.PropertyType == typeof(PropertyMap)).ToDictionary(k => k, v => ((PropertyMapAttribute)v.GetCustomAttributes(typeof(PropertyMapAttribute), true)[0]).Type);

            foreach (var entry in sourceMaps)
            {
                if (entry.Value == PropertyMapType.Request || entry.Value == PropertyMapType.Response)
                {

                    PropertyMap sourceMap = entry.Key.GetValue(fromObject, null) as PropertyMap;
                    sourceMap = GetInitializedDomainObject<PropertyMap>(sourceMap.Id);

                    var target = from t in targetMaps
                                 where t.Value == entry.Value
                                 select t.Key;

                    if (target.Count() > 1)
                    {
                        throw new Exception("Failed to synchronize property maps, multiple maps found.");
                    }
                    else if (target.Count() == 1)
                    {


                        PropertyMap targetMap = target.First().GetValue(toObject, null) as PropertyMap;

                        if (targetMap == null)
                        {
                            targetMap = new PropertyMap();
                        }

                        if (targetMap.Id == Guid.Empty)
                        {
                            SaveDomainObject(targetMap);
                        }

                        SynchronizePropertyMaps(sourceMap, ref targetMap, new List<IDomainObject>(), SetTargetChoice.Yes, MapSyncOption.Source, new List<IDomainObject>(), new List<IDomainObject>(), null, new List<MappedProperty>());

                        target.First().SetValue(toObject, targetMap, null);
                    }
                }
            }

            MergeSaveDomainObject(toObject);
            StartSynchronizePropertyMapsInObjects(GetDomainObject(toObject.Id, GetDomainObjectType(toObject)));
        }

        [Transaction(ReadOnly = false)]
        private bool SynchronizePropertyMaps(PropertyMap fromMap, ref PropertyMap toMap, List<IDomainObject> propertyToDelete, SetTargetChoice setTarget, MapSyncOption mapSyncOption, List<IDomainObject> mapsToSave, List<IDomainObject> refObjectsToSave, List<IVersionControlled> versionControlledParents, List<MappedProperty> mappedPropertysToDeleteInToMap)
        {
            bool changesMade = false;
            bool onlySaveMap = false;

            PropertyMap tmptoMap = GetInitializedDomainObject<PropertyMap>(toMap.Id);

            //Create an off session copy of the map
            toMap = new PropertyMap();
            toMap.Id = tmptoMap.Id;
            toMap.IsCollection = tmptoMap.IsCollection;
            toMap.MappedProperties = new List<MappedProperty>(tmptoMap.MappedProperties);
            toMap.IsTransient = false;

            //Add missing mapped properties in toMap
            if (mapSyncOption == MapSyncOption.Source)
            {
                foreach (MappedProperty fromProperty in fromMap.MappedProperties.Where(m => !propertyToDelete.Contains(m)))
                {
                    if (fromProperty.Id != Guid.Empty)
                    {
                        IEnumerable<MappedProperty> toMapProperties = toMap.MappedProperties.Where(p => p.Source != null && p.Source.Id == fromProperty.Id);

                        MappedProperty mappedProperty = null;

                        if (toMapProperties.Count() > 0)
                        {
                            if (setTarget == SetTargetChoice.Yes && (toMapProperties.ElementAt(0).Name != fromProperty.Name || toMapProperties.ElementAt(0).Target.Id != fromProperty.Target.Id))
                            {
                                mappedProperty = toMapProperties.ElementAt(0);

                                toMap.MappedProperties.Remove(mappedProperty);
                                MappedProperty tmpMP = new MappedProperty();
                                tmpMP = (MappedProperty)mappedProperty.Clone();

                                mappedProperty = tmpMP;
                                toMap.MappedProperties.Add(mappedProperty);
                            }

                        }
                        else
                        {
                            mappedProperty = new MappedProperty();
                            mappedProperty.Id = Guid.NewGuid();
                            toMap.MappedProperties.Add(mappedProperty);
                            mappedProperty.PropertyMap = toMap;
                            mappedProperty.Source = fromProperty;

                            mappedProperty.IsSearchable = false;
                        }

                        if (mappedProperty != null)
                        {
                            if (setTarget == SetTargetChoice.Yes)
                            {
                                mappedProperty.Target = fromProperty.Target;
                                mappedProperty.Name = fromProperty.Name;
                            }
                            else
                            {
                                mappedProperty.IsEnabled = false;
                            }

                            mappedProperty.Sequence = fromProperty.Sequence;
                            mappedProperty.IsMandatory = fromProperty.IsMandatory;

                            changesMade = true;
                        }

                    }
                }
            }

            //Remove mapped properties from toMap that is not in fromMap
            foreach (MappedProperty toProperty in toMap.MappedProperties.ToArray())
            {
                bool existsInFromMap = false;

                foreach (MappedProperty fromProperty in fromMap.MappedProperties.Where(m => !propertyToDelete.Contains(m)).ToArray())
                {
                    if (mapSyncOption == MapSyncOption.Source)
                    {
                        if (toProperty.Source != null && toProperty.Source.Id == fromProperty.Id)
                        {
                            existsInFromMap = true;
                            break;
                        }
                    }
                    else
                    {
                        if (toProperty.Target == null || (toProperty.Target != null && toProperty.Target.Id == fromProperty.Id))
                        {
                            existsInFromMap = true;
                            break;
                        }
                    }
                }

                if ((!existsInFromMap) && (!toProperty.IsCustom))
                {
                    if (mapSyncOption == MapSyncOption.Source)
                    {
                        toProperty.Source = null;

                        propertyToDelete.Add(toProperty);
                        mappedPropertysToDeleteInToMap.Add(toProperty);

                        changesMade = true;


                        if (versionControlledParents != null)
                        {
                            //Find all referencing properties and null them
                            Dictionary<Type, List<IDomainObject>> mpReferences = GetReferencingObjects(toProperty, new List<Type> { typeof(PropertyMap), typeof(MappedProperty) }, new List<Type>());

                            Dictionary<string, object> criteria = new Dictionary<string, object>();
                            criteria.Add("RequestMappedProperty.Id", toProperty.Id);

                            IList<MappedProperty> requestMappedProperties = GetAllDomainObjectsByPropertyValues<MappedProperty>(criteria);
                            mpReferences.Add(typeof(MappedProperty), requestMappedProperties.ToList<IDomainObject>());

                            foreach (Type refType in mpReferences.Keys)
                            {
                                foreach (IDomainObject refObj in mpReferences[refType])
                                {
                                    List<IDomainObject> parents;
                                    IVersionControlled vcParent = refObj as IVersionControlled;

                                    if (vcParent == null)
                                    {
                                        vcParent = GetVersionControlledParent(refObj, out parents).First();
                                    }

                                    if (!versionControlledParents.Contains(vcParent))
                                    {
                                        versionControlledParents.Add(vcParent);
                                    }

                                    List<PropertyInfo> props = refType.GetProperties().Where(p => p.PropertyType == typeof(MappedProperty) && p.GetValue(refObj, null) != null && ((MappedProperty)p.GetValue(refObj, null)).Id == toProperty.Id).ToList();

                                    foreach (PropertyInfo pi in props)
                                    {
                                        pi.SetValue(refObj, null, null);
                                    }

                                    refObjectsToSave.Add(refObj);
                                }
                            }
                        }

                    }
                    else
                    {
                        toProperty.Target = null;
                        toProperty.IsEnabled = false;

                        onlySaveMap = true;
                    }
                }
            }

            if (mapSyncOption == MapSyncOption.Source)
            {
                if (toMap.IsCollection != fromMap.IsCollection && setTarget == SetTargetChoice.Yes)
                {
                    toMap.IsCollection = fromMap.IsCollection;
                    changesMade = true;
                }
            }

            if (changesMade || onlySaveMap)
            {
                mapsToSave.Add(toMap);
            }

            return changesMade;
        }


        [Transaction(ReadOnly = true)]
        public IList<T> GetDomainObjectsByQuery<T>(string query)
        {
            List<T> result = GetDomainObjectsByQuery(query).Cast<T>().ToList();

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<IDomainObject> GetDomainObjectsByQuery(string query)
        {
            List<IDomainObject> result = ((DataAccess.Dao.IDynamicDao)ctx["DynamicDao"]).FindByQuery(query).Cast<IDomainObject>().ToList();

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IList<T> GetAllDomainObjectsByPropertyValues<T>(Dictionary<string, object> namedPropertyAndValue)
        {
            return GetAllDomainObjectsByPropertyValues<T>(namedPropertyAndValue, false, false);
        }

        [Transaction(ReadOnly = true)]
        public IList<T> GetAllDomainObjectsByPropertyValues<T>(Dictionary<string, object> namedPropertyAndValue, bool initialize)
        {
            return GetAllDomainObjectsByPropertyValues<T>(namedPropertyAndValue, initialize, false);
        }

        [Transaction(ReadOnly = true)]
        public IList<T> GetAllDomainObjectsByPropertyValues<T>(Dictionary<string, object> namedPropertyAndValue, bool initialize, bool useWildcards)
        {
            return GetAllDomainObjectsByPropertyValues(typeof(T), namedPropertyAndValue, initialize, useWildcards).Cast<T>().ToList();
        }

        [Transaction(ReadOnly = true)]
        private IList<DataAccess.IDomainObject> GetAllDomainObjectsByPropertyValues(Type classType, Dictionary<string, object> namedPropertyAndValue, bool initialize, bool useWildcards)
        {
            string domainObjectTypeName = classType.Name;
            string query = "select o from " + domainObjectTypeName + " o ";

            if (namedPropertyAndValue.Count > 0)
            {
                query += "where ";

                foreach (string prop in namedPropertyAndValue.Keys)
                {
                    if (!query.EndsWith("where "))
                    {
                        query += "and ";
                    }

                    query += "(o." + prop;

                    if (namedPropertyAndValue[prop] is Guid)
                    {
                        query += " = '" + ((Guid)namedPropertyAndValue[prop]).ToString() + "' ";
                    }
                    else if (namedPropertyAndValue[prop] is bool)
                    {
                        query += " = " + ((bool)namedPropertyAndValue[prop] ? "true" : "false");
                    }
                    else
                    {
                        if (useWildcards)
                        {
                            query += " LIKE '%" + (string)namedPropertyAndValue[prop] + "%' ";

                            if (string.IsNullOrEmpty((string)namedPropertyAndValue[prop]))
                            {
                                query += "OR " + prop + " = Null ";
                            }
                        }
                        else
                        {
                            query += " = '" + (string)namedPropertyAndValue[prop] + "' ";
                        }
                    }

                    query += ") ";
                }
            }

            GetContext();

            List<IDomainObject> result = ((DataAccess.Dao.IDynamicDao)ctx["DynamicDao"]).FindByQuery(query).ToList();

            if (initialize)
            {
                foreach (IDomainObject o in result)
                {
                    InitializeDomainObject(o, namedPropertyAndValue.Keys.ToList());
                }
            }

            return result;
        }

        [Transaction(ReadOnly = true)]
        public IEnumerable<DataAccess.IVersionControlled> GetVersionControlledDomainObjectsForParent(Type domainObjectType, Guid parentId)
        {
            GetContext();
            if (ctx != null)
            {
                if (domainObjectType == typeof(DataAccess.Domain.Application))
                {
                    if (parentId != Guid.Empty)
                    {
                        DataAccess.Domain.DeploymentGroup dG = ((DataAccess.Domain.DeploymentGroup)GetDomainObject(parentId, typeof(DataAccess.Domain.DeploymentGroup)));
                        List<DataAccess.IVersionControlled> toReturn = new List<DataAccess.IVersionControlled>();
                        NHibernateUtil.Initialize(dG.FrontendApplication);
                        NHibernateUtil.Initialize(dG.BackendApplication);
                        toReturn.Add(dG.FrontendApplication);
                        toReturn.Add(dG.BackendApplication);
                        return toReturn;
                    }
                    else
                    {
                        return ((DataAccess.Dao.IApplicationDao)ctx["ApplicationDao"]).FindAll().Cast<DataAccess.IVersionControlled>();
                    }
                }
                else if (domainObjectType == typeof(DataAccess.Domain.Module))
                {
                    return ((DataAccess.Dao.IModuleDao)ctx["ModuleDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.Dialog))
                {
                    return ((DataAccess.Dao.IDialogDao)ctx["DialogDao"]).FindAllByModule(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.Workflow))
                {
                    return ((DataAccess.Dao.IWorkflowDao)ctx["WorkflowDao"]).FindAllByModule(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.Service))
                {
                    return ((DataAccess.Dao.IServiceDao)ctx["ServiceDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.ServiceMethod))
                {
                    return ((DataAccess.Dao.IServiceMethodDao)ctx["ServiceMethodDao"]).FindAllByService(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.Menu))
                {
                    return ((DataAccess.Dao.IMenuDao)ctx["MenuDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.CustomDialog))
                {
                    return ((DataAccess.Dao.ICustomDialogDao)ctx["CustomDialogDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.UXAction))
                {
                    return ((DataAccess.Dao.IUXActionDao)ctx["UXActionDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.View))
                {
                    return ((DataAccess.Dao.IViewDao)ctx["ViewDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.BusinessEntity))
                {
                    return ((DataAccess.Dao.IBusinessEntityDao)ctx["BusinessEntityDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.HintCollection))
                {
                    return ((DataAccess.Dao.IHintCollectionDao)ctx["HintCollectionDao"]).FindAll(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.Action))
                {
                    return ((DataAccess.Dao.IActionDao)ctx["ActionDao"]).FindByBusinessEntityId(parentId).Cast<DataAccess.IVersionControlled>();
                }
                else if (domainObjectType == typeof(DataAccess.Domain.Report))
                {
                    return ((DataAccess.Dao.IReportDao)ctx["ReportDao"]).FindAllReports(parentId).Cast<DataAccess.IVersionControlled>();
                }
            }

            return null;
        }

        public Type GetDomainObjectType(IDomainObject domainObject)
        {
            return NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject);
        }

        public bool? ClassTypeBelongToFrontend(Type classType)
        {
            if (classType == typeof(DataAccess.Domain.Application))
            {
                return null;
            }

            List<PropertyInfo> api = classType.GetProperties().Where(p => p.PropertyType == typeof(DataAccess.Domain.Application)).ToList();

            if (api.Count() > 0)
            {
                List<PropertyInfo> appChildPropList = typeof(DataAccess.Domain.Application).GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == classType).ToList();
                if (appChildPropList.Count > 0)
                {
                    object[] onlyFrontendAttributes = appChildPropList[0].GetCustomAttributes(typeof(ApplicationOnlyFrontendAttribute), true);

                    if (onlyFrontendAttributes.Length > 0)
                    {
                        return true;
                    }

                    object[] onlyBackendAttributes = appChildPropList[0].GetCustomAttributes(typeof(ApplicationOnlyBackendAttribute), true);

                    if (onlyBackendAttributes.Length > 0)
                    {
                        return false;
                    }
                }
                else
                {
                    List<PropertyInfo> appChildProp = typeof(DataAccess.Domain.Application).GetProperties().Where(p => p.PropertyType == classType).ToList();
                    if (appChildProp.Count > 0)
                    {
                        object[] onlyFrontendAttributes = appChildProp[0].GetCustomAttributes(typeof(ApplicationOnlyFrontendAttribute), true);

                        if (onlyFrontendAttributes.Length > 0)
                        {
                            return true;
                        }

                        object[] onlyBackendAttributes = appChildProp[0].GetCustomAttributes(typeof(ApplicationOnlyBackendAttribute), true);

                        if (onlyBackendAttributes.Length > 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //Special for fixing problem with package och schema objects
                        return false;
                    }

                }
                return null;

            }
            else
            {
                Type parentType = FindParentType(classType, false);
                if (parentType != null)
                {
                    return ClassTypeBelongToFrontend(parentType);
                }
            }

            return null;
        }

        public Type FindParentType(Type classType, bool versionControlled)
        {
            IEnumerable<PropertyInfo> pis = classType.GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType) && p.PropertyType != classType);

            foreach (PropertyInfo pi in pis)
            {
                if (pi.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == classType).ToList().Count > 0)
                {
                    if (versionControlled)
                    {
                        if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(pi.PropertyType))
                        {
                            return pi.PropertyType;
                        }
                        else
                        {
                            return FindParentType(pi.PropertyType, versionControlled);
                        }
                    }
                    else
                    {
                        return pi.PropertyType;
                    }
                }
            }

            return null;
        }

        [Transaction(ReadOnly = true)]
        public DataAccess.Domain.Application GetApplicationForDomainObject(IDomainObject domainObject)
        {
            Type classType = GetDomainObjectType(domainObject);
            IDomainObject theObject = GetDomainObject(domainObject.Id, classType);

            if (theObject is DataAccess.Domain.Application) { return (DataAccess.Domain.Application)theObject; }

            List<PropertyInfo> api = classType.GetProperties().Where(p => p.PropertyType == typeof(DataAccess.Domain.Application)).ToList();

            if (api.Count() > 0)
            {
                return ((DataAccess.Domain.Application)api[0].GetValue(theObject, null));
            }

            IEnumerable<PropertyInfo> pis = classType.GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType));

            DataAccess.Domain.Application parentApp = null;
            foreach (PropertyInfo pi in pis)
            {
                if (pi.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == classType).ToList().Count > 0)
                {
                    IDomainObject parent = (IDomainObject)pi.GetValue(domainObject, null);

                    if (parent != null)
                    {
                        parentApp = GetApplicationForDomainObject(parent);
                    }
                }

                if (parentApp != null)
                {
                    return parentApp;
                }
            }

            return null;

        }

        [Transaction(ReadOnly = true)]
        public IList<T> GetAllDomainObjectsByApplicationId<T>(Guid ApplicationId)
        {
            return GetAllDomainObjectsByApplicationId(ApplicationId, typeof(T)).Cast<T>().ToList();
        }

        [Transaction(ReadOnly = true)]
        public IList<IDomainObject> GetAllDomainObjectsByApplicationId(Guid ApplicationId, Type classType)
        {
            GetContext();

            if (classType == typeof(DataAccess.Domain.Application))
            {
                IList<DataAccess.Domain.Application> tmpapplist = new List<DataAccess.Domain.Application>();
                tmpapplist.Add(((DataAccess.Dao.IApplicationDao)ctx["ApplicationDao"]).FindById(ApplicationId));
                return tmpapplist.ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.CustomDialog))
            {
                return ((DataAccess.Dao.ICustomDialogDao)ctx["CustomDialogDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.DataSource))
            {
                return ((DataAccess.Dao.IDataSourceDao)ctx["DataSourceDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Dialog))
            {
                return ((DataAccess.Dao.IDialogDao)ctx["DialogDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Hint))
            {
                return ((DataAccess.Dao.IHintDao)ctx["HintDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.HintCollection))
            {
                return ((DataAccess.Dao.IHintCollectionDao)ctx["HintCollectionDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Issue))
            {
                return ((DataAccess.Dao.IIssueDao)ctx["IssueDao"]).FindAllIssues(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.MappedProperty))
            {
                //Special
                return ((DataAccess.Dao.IMappedPropertyDao)ctx["MappedPropertyDao"]).FindAll().ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Menu))
            {
                return ((DataAccess.Dao.IMenuDao)ctx["MenuDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.MenuItem))
            {
                return ((DataAccess.Dao.IMenuItemDao)ctx["MenuItemDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Module))
            {
                return ((DataAccess.Dao.IModuleDao)ctx["ModuleDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Report))
            {
                return ((DataAccess.Dao.IReportDao)ctx["ReportDao"]).FindAllReports(ApplicationId).ToList<IDomainObject>();
            }
            //else if (classType == typeof(DataAccess.Domain.ReportQuery))
            //{
            //    return ((IList<IDomainObject>) ((object) ((DataAccess.Dao.IReportQueryDao)ctx["ReportQueryDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            //}
            else if (classType == typeof(DataAccess.Domain.UXAction))
            {
                return ((DataAccess.Dao.IUXActionDao)ctx["UXActionDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.UXSession))
            {
                IList<IDomainObject> tmpsessionList = new List<IDomainObject>();
                tmpsessionList.Add(((DataAccess.Dao.IUXSessionDao)ctx["UXSessionDao"]).FindByApplicationId(ApplicationId));
                return tmpsessionList;
            }
            else if (classType == typeof(DataAccess.Domain.ViewAction))
            {
                return ((DataAccess.Dao.IViewActionDao)ctx["ViewActionDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.View))
            {
                return ((DataAccess.Dao.IViewDao)ctx["ViewDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.ViewNode))
            {
                return ((DataAccess.Dao.IViewNodeDao)ctx["ViewNodeDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Workflow))
            {
                return ((DataAccess.Dao.IWorkflowDao)ctx["WorkflowDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.WorkflowDialog))
            {
                return ((DataAccess.Dao.IWorkflowDialogDao)ctx["WorkflowDialogDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.WorkflowServiceMethod))
            {
                return ((DataAccess.Dao.IWorkflowServiceMethodDao)ctx["WorkflowServiceMethodDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            //else if (classType == typeof(DataAccess.Domain.WorkflowSubworkflow))
            //{
            //    return ((IList<IDomainObject>) ((object) ((DataAccess.Dao.IWorkflowSubworkflowDao)ctx["WorkflowSubworkflowDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            //}
            else if (classType == typeof(DataAccess.Domain.BusinessEntity))
            {
                return ((DataAccess.Dao.IBusinessEntityDao)ctx["BusinessEntityDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Package))
            {
                return ((DataAccess.Dao.IPackageDao)ctx["PackageDao"]).FindAllByApplicationId(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Property))
            {
                return ((DataAccess.Dao.IPropertyDao)ctx["PropertyDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.PropertyMap))
            {
                //Special
                return ((DataAccess.Dao.IPropertyMapDao)ctx["PropertyMapDao"]).FindAll().ToList<IDomainObject>();
            }
            //else if (classType == typeof(DataAccess.Domain.PropertyStorageInfo))
            //{
            //    return ((IList<IDomainObject>) ((object) ((DataAccess.Dao.IPropertyStorageInfoDao)ctx["PropertyStorageInfoDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            //}
            else if (classType == typeof(DataAccess.Domain.Action))
            {
                return ((DataAccess.Dao.IActionDao)ctx["ActionDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.Query))
            {
                return ((DataAccess.Dao.IQueryDao)ctx["QueryDao"]).FindAllByApplicationId(ApplicationId).ToList<IDomainObject>();
            }
            //else if (classType == typeof(DataAccess.Domain.QueryProperty))
            //{
            //    return ((IList<IDomainObject>) ((object) ((DataAccess.Dao.IQueryPropertyDao)ctx["QueryPropertyDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            //}
            else if (classType == typeof(DataAccess.Domain.Schema))
            {
                IList<IDomainObject> tmpschemaList = new List<IDomainObject>();
                tmpschemaList.Add(((DataAccess.Dao.ISchemaDao)ctx["SchemaDao"]).FindByApplicationId(ApplicationId));
                return tmpschemaList;
            }
            else if (classType == typeof(DataAccess.Domain.Service))
            {
                return ((DataAccess.Dao.IServiceDao)ctx["ServiceDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            }
            else if (classType == typeof(DataAccess.Domain.ServiceMethod))
            {
                return ((DataAccess.Dao.IServiceMethodDao)ctx["ServiceMethodDao"]).FindAllByApplicationId(ApplicationId).ToList<IDomainObject>();
            }
            //else if (classType == typeof(DataAccess.Domain.StoredProcedure))
            //{
            //    return ((IList<IDomainObject>) ((object) ((DataAccess.Dao.IStoredProcedureDao)ctx["StoredProcedureDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            //}
            //else if (classType == typeof(DataAccess.Domain.StoredProcedureProperty))
            //{
            //    return ((IList<IDomainObject>) ((object) ((DataAccess.Dao.IStoredProcedurePropertyDao)ctx["StoredProcedurePropertyDao"]).FindAll(ApplicationId).ToList<IDomainObject>();
            //}
            else if (classType == typeof(DataAccess.Domain.DeploymentGroup))
            {
                IList<IDomainObject> list = new List<IDomainObject>();
                return list;
            }

            throw new Exception("GetAllDomainObjectsByApplicationId not implemented for type: " + classType.Name);
        }

        [Transaction(ReadOnly = true)]
        public T GetInitializedDomainObject<T>(Guid domainObjectId)
        {
            return (T)GetInitializedDomainObject(domainObjectId, typeof(T));
        }

        [Transaction(ReadOnly = true)]
        public IDomainObject GetInitializedDomainObject(Guid domainObjectId, Type classType)
        {
            IDomainObject theObject = null;

            theObject = GetDomainObject(domainObjectId, classType);

            InitializeDomainObject(theObject, new List<string>());

            return theObject;
        }

        [Transaction(ReadOnly = true)]
        public T GetDomainObject<T>(Guid domainObjectId)
        {
            return (T)GetDomainObject(domainObjectId, typeof(T), false);
        }

        [Transaction(ReadOnly = true)]
        public IDomainObject GetDomainObject(Guid domainObjectId, Type classType)
        {
            return GetDomainObject(domainObjectId, classType, false);
        }

        [Transaction(ReadOnly = true)]
        private IDomainObject GetDomainObject(Guid domainObjectId, Type classType, bool recursive)
        {
            return (IDomainObject)DataAccess.DomainXmlSerializationHelper.GetClassInstance(domainObjectId, classType, recursive);
        }

        [Transaction(ReadOnly = false)]
        public IDomainObject SaveDomainObject(IDomainObject domainObject, bool newObj = false)
        {
            if (domainObject.Id == Guid.Empty)
            {
                newObj = true;
            }

            Type classType = GetDomainObjectType(domainObject);

            if (newObj && typeof(IVersionControlled).IsAssignableFrom(classType))
            {
                ((IVersionControlled)domainObject).State = VersionControlledObjectStat.New;
                ((IVersionControlled)domainObject).IsLocked = true;
                ((IVersionControlled)domainObject).LockedBy = Environment.UserName;
                ((IVersionControlled)domainObject).LockedDate = DateTime.Now;
            }

            IDomainObject theObj = (IDomainObject)DataAccess.DomainXmlSerializationHelper.SaveClassInstance(domainObject, classType, false);

            if (newObj && typeof(IVersionControlled).IsAssignableFrom(classType))
            {
                DomainObjectAdded(theObj.Id, classType);
            }

            return theObj;
        }

        [Transaction(ReadOnly = false)]
        public IDomainObject MergeSaveDomainObject(IDomainObject domainObject)
        {
            bool newObj = domainObject.Id == Guid.Empty;
            Type classType = GetDomainObjectType(domainObject);

            if (newObj && typeof(IVersionControlled).IsAssignableFrom(classType))
            {
                ((IVersionControlled)domainObject).State = VersionControlledObjectStat.New;
            }

            IDomainObject theObj = (IDomainObject)DataAccess.DomainXmlSerializationHelper.MergeSaveClassInstance(domainObject, classType, false);

            if (newObj)
            {
                DomainObjectAdded(theObj.Id, classType);
            }

            return theObj;
        }

        [Transaction(ReadOnly = true)]
        public IList<IDomainObject> GetReferencingObjects(IDomainObject domainObject)
        {
            Type classType = GetDomainObjectType(domainObject);
            domainObject = GetDomainObject(domainObject.Id, classType);
            List<IDomainObject> parents = new List<IDomainObject>();
            List<Type> parentTypes = new List<Type>();
            List<Type> childTypes = new List<Type>();

            GetVersionControlledParent(domainObject, out parents);

            foreach (IDomainObject parent in parents)
            {
                if (!parentTypes.Contains(GetDomainObjectType(parent)))
                {
                    parentTypes.Add(GetDomainObjectType(parent));
                }
            }

            GetChildrenWithParents(domainObject, parentTypes, out childTypes);

            Dictionary<Type, List<IDomainObject>> referencingObjects = GetReferencingObjects(domainObject, parentTypes, childTypes);

            return referencingObjects.Values.SelectMany(l => l).ToList();
        }

        [Transaction(ReadOnly = false)]
        public void DeleteDomainObject(IDomainObject domainObject)
        {
            DeleteDomainObject(domainObject, true, null, false);
        }

        [Transaction(ReadOnly = false)]
        public void DeleteDomainObjectAtCheckIn(IDomainObject domainObject)
        {
            DeleteDomainObject(domainObject, true, null, true);
        }

        [Transaction(ReadOnly = false)]
        public void DeleteDomainObjectWithoutChecksAndCheckOut(IDomainObject domainObject)
        {
            DeleteDomainObject(domainObject, false, new List<IDomainObject>(), true);
        }

        [Transaction(ReadOnly = false)]
        private void DeleteDomainObject(IDomainObject domainObject, bool doChecksAndCheckOut, List<IDomainObject> objectParents, bool doDelete)
        {
            IList<IVersionControlled> versionControlledParents = null;
            IVersionControlled versionControlledObject = null;
            Type vParentClassType = null;
            Type classType = null;
            bool removeFromConfMgn = true;

            List<IDomainObject> parents = null;

            List<IVersionControlled> autoCheckedOutParents = new List<IVersionControlled>();

            if (domainObject == null) { return; }

            if (domainObject is IVersionControlled)
            {
                versionControlledObject = ((IVersionControlled)domainObject);
            }


            if (versionControlledObject != null)
            {
                if (versionControlledObject.State == VersionControlledObjectStat.Default)
                {
                    doDelete = false;
                }
                else if (versionControlledObject.State == VersionControlledObjectStat.New)
                {
                    doDelete = true;
                    removeFromConfMgn = false;
                }
            }
            else
            {
                doDelete = true;
                removeFromConfMgn = false;
            }

            if (doChecksAndCheckOut)
            {
                if (versionControlledObject != null)
                {
                    classType = GetDomainObjectType(versionControlledObject);

                    ConfigurationManagementService.CheckOutDomainObject(versionControlledObject.Id, classType);
                }

                versionControlledParents = GetVersionControlledParent(domainObject, out parents);

                CheckAndBreakReference(domainObject, parents, true, true, true);

                if (doDelete && removeFromConfMgn)
                {
                    if (versionControlledObject == null)
                    {
                        for (int index = 0; index < versionControlledParents.Count; index++)
                        {
                            IVersionControlled versionControlledParent = versionControlledParents[index];

                            if (versionControlledParent.LockedBy != Environment.UserName)
                            {
                                vParentClassType = GetDomainObjectType(versionControlledParent);

                                ConfigurationManagementService.CheckOutDomainObject(versionControlledParent.Id, vParentClassType);

                                autoCheckedOutParents.Add(versionControlledParent);
                            }
                        }
                    }
                }
            }
            else
            {
                parents = objectParents;
            }

            if (doDelete)
            {
                
                //Remove the object from it's parents.
                foreach (IDomainObject parent in parents)
                {
                    RemoveDomainObjectFromParent(domainObject, parent);
                }

                domainObject = GetDomainObject(domainObject.Id, GetDomainObjectType(domainObject));


                if (removeFromConfMgn)
                {
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["DeleteRemovedFilesFromRepo"]))
                    {
                        ConfigurationManagementService.RemoveDomainObject(domainObject.Id, GetDomainObjectType(domainObject), GetApplicationForDomainObject(domainObject));
                    }
                }

                if (domainObject is DataAccess.Domain.Action)
                {
                    ((DataAccess.Dao.IActionDao)ctx["ActionDao"]).Delete((DataAccess.Domain.Action)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Application)
                {
                    ((DataAccess.Dao.IApplicationDao)ctx["ApplicationDao"]).Delete((DataAccess.Domain.Application)domainObject);
                }
                else if (domainObject is DataAccess.Domain.BusinessEntity)
                {
                    ((DataAccess.Dao.IBusinessEntityDao)ctx["BusinessEntityDao"]).Delete((DataAccess.Domain.BusinessEntity)domainObject);
                }
                else if (domainObject is DataAccess.Domain.CustomDialog) //OK
                {
                    ((DataAccess.Dao.ICustomDialogDao)ctx["CustomDialogDao"]).Delete((DataAccess.Domain.CustomDialog)domainObject);
                }
                else if (domainObject is DataAccess.Domain.DataSource) //OK
                {
                    ((DataAccess.Dao.IDataSourceDao)ctx["DataSourceDao"]).Delete((DataAccess.Domain.DataSource)domainObject);
                }
                else if (domainObject is DataAccess.Domain.DeploymentGroup)
                {
                    ((DataAccess.Dao.IDeploymentGroupDao)ctx["DeploymentGroupDao"]).Delete((DataAccess.Domain.DeploymentGroup)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Dialog)
                {
                    ((DataAccess.Dao.IDialogDao)ctx["DialogDao"]).Delete((DataAccess.Domain.Dialog)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Hint)
                {
                    ((DataAccess.Dao.IHintDao)ctx["HintDao"]).Delete((DataAccess.Domain.Hint)domainObject);
                }
                else if (domainObject is DataAccess.Domain.HintCollection)
                {
                    ((DataAccess.Dao.IHintCollectionDao)ctx["HintCollectionDao"]).Delete((DataAccess.Domain.HintCollection)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Issue)
                {
                    ((DataAccess.Dao.IIssueDao)ctx["IssueDao"]).Delete((DataAccess.Domain.Issue)domainObject);
                }
                else if (domainObject is DataAccess.Domain.MappedProperty)
                {
                    ((DataAccess.Dao.IMappedPropertyDao)ctx["MappedPropertyDao"]).Delete((DataAccess.Domain.MappedProperty)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Menu)
                {
                    ((DataAccess.Dao.IMenuDao)ctx["MenuDao"]).Delete((DataAccess.Domain.Menu)domainObject);
                }
                else if (domainObject is DataAccess.Domain.MenuItem)
                {
                    ((DataAccess.Dao.IMenuItemDao)ctx["MenuItemDao"]).Delete((DataAccess.Domain.MenuItem)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Module)
                {
                    ((DataAccess.Dao.IModuleDao)ctx["ModuleDao"]).Delete((DataAccess.Domain.Module)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Package)
                {
                    ((DataAccess.Dao.IPackageDao)ctx["PackageDao"]).Delete((DataAccess.Domain.Package)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Property)
                {
                    ((DataAccess.Dao.IPropertyDao)ctx["PropertyDao"]).Delete((DataAccess.Domain.Property)domainObject);
                }
                else if (domainObject is DataAccess.Domain.PropertyMap) //OK
                {
                    ((DataAccess.Dao.IPropertyMapDao)ctx["PropertyMapDao"]).Delete((DataAccess.Domain.PropertyMap)domainObject);
                }
                else if (domainObject is DataAccess.Domain.PropertyStorageInfo)
                {
                    ((DataAccess.Dao.IPropertyStorageInfoDao)ctx["PropertyStorageInfoDao"]).Delete((DataAccess.Domain.PropertyStorageInfo)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Query)
                {
                    ((DataAccess.Dao.IQueryDao)ctx["QueryDao"]).Delete((DataAccess.Domain.Query)domainObject);
                }
                else if (domainObject is DataAccess.Domain.QueryProperty)
                {
                    ((DataAccess.Dao.IQueryPropertyDao)ctx["QueryPropertyDao"]).Delete((DataAccess.Domain.QueryProperty)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Report)
                {
                    ((DataAccess.Dao.IReportDao)ctx["ReportDao"]).Delete((DataAccess.Domain.Report)domainObject);
                }
                else if (domainObject is DataAccess.Domain.ReportQuery)
                {
                    ((DataAccess.Dao.IReportQueryDao)ctx["ReportQueryDao"]).Delete((DataAccess.Domain.ReportQuery)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Schema)
                {
                    ((DataAccess.Dao.ISchemaDao)ctx["SchemaDao"]).Delete((DataAccess.Domain.Schema)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Service)
                {
                    ((DataAccess.Dao.IServiceDao)ctx["ServiceDao"]).Delete((DataAccess.Domain.Service)domainObject);
                }
                else if (domainObject is DataAccess.Domain.ServiceMethod)
                {
                    ((DataAccess.Dao.IServiceMethodDao)ctx["ServiceMethodDao"]).Delete((DataAccess.Domain.ServiceMethod)domainObject);
                }
                else if (domainObject is DataAccess.Domain.StoredProcedure)
                {
                    ((DataAccess.Dao.IStoredProcedureDao)ctx["StoredProcedureDao"]).Delete((DataAccess.Domain.StoredProcedure)domainObject);
                }
                else if (domainObject is DataAccess.Domain.ProcedureProperty)
                {
                    ((DataAccess.Dao.IStoredProcedurePropertyDao)ctx["StoredProcedurePropertyDao"]).Delete((DataAccess.Domain.ProcedureProperty)domainObject);
                }
                else if (domainObject is DataAccess.Domain.UXAction)
                {
                    ((DataAccess.Dao.IUXActionDao)ctx["UXActionDao"]).Delete((DataAccess.Domain.UXAction)domainObject);
                }
                else if (domainObject is DataAccess.Domain.UXSession)
                {
                    ((DataAccess.Dao.IUXSessionDao)ctx["UXSessionDao"]).Delete((DataAccess.Domain.UXSession)domainObject);
                }
                else if (domainObject is DataAccess.Domain.ViewAction)
                {
                    ((DataAccess.Dao.IViewActionDao)ctx["ViewActionDao"]).Delete((DataAccess.Domain.ViewAction)domainObject);
                }
                else if (domainObject is DataAccess.Domain.View) //OK
                {
                    DeleteMapsFromUXComponent(((DataAccess.Domain.View)domainObject).VisualTree);
                    ((DataAccess.Dao.IViewDao)ctx["ViewDao"]).Delete((DataAccess.Domain.View)domainObject);
                }
                else if (domainObject is DataAccess.Domain.ViewNode)
                {
                    ((DataAccess.Dao.IViewNodeDao)ctx["ViewNodeDao"]).Delete((DataAccess.Domain.ViewNode)domainObject);
                }
                else if (domainObject is DataAccess.Domain.Workflow) //OK
                {
                    ((DataAccess.Dao.IWorkflowDao)ctx["WorkflowDao"]).Delete((DataAccess.Domain.Workflow)domainObject);
                }

                if (domainObject is IVersionControlled)
                {
                    DomainObjectDeleted(domainObject.Id, GetDomainObjectType(domainObject));
                }
            }
            else
            {
                versionControlledObject = (IVersionControlled)GetDomainObject(versionControlledObject.Id, GetDomainObjectType(versionControlledObject));
                versionControlledObject.State = VersionControlledObjectStat.Deleted;
                SaveDomainObject(versionControlledObject);
                DomainObjectChanged(versionControlledObject.Id, GetDomainObjectType(versionControlledObject));
            }
        }

        [Transaction(ReadOnly = true)]
        public IList<Type> GetAllVersionControlledTypes()
        {
            IList<Type> VersionControlledTypes = Assembly.GetAssembly(typeof(IVersionControlled)).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IVersionControlled))).ToList();

            return VersionControlledTypes;
        }

        [Transaction(ReadOnly = true)]
        public IList<IVersionControlled> GetVersionControlledParent(IDomainObject domainObject, out List<IDomainObject> parents)
        {
            List<IDomainObject> parentParent = new List<IDomainObject>();
            parents = new List<IDomainObject>();
            List<IVersionControlled> vParents = new List<IVersionControlled>();
            Type classType = GetDomainObjectType(domainObject);

            domainObject = GetDomainObject(domainObject.Id, classType);

            IEnumerable<PropertyInfo> pis = classType.GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType));

            foreach (PropertyInfo pi in pis)
            {
                if (pi.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == classType).ToList().Count > 0)
                {
                    IDomainObject parent = (IDomainObject)pi.GetValue(domainObject, null);
                    if (parent != null)
                    {
                        parents.Add(parent);

                        if (typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType))
                        {
                            vParents.Add((IVersionControlled)parent);
                        }
                    }
                }
            }

            if (parents.Count == 0 && classType != typeof(Application))
            {
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.PropertyType.GetProperties().Where(p => p.PropertyType == classType).ToList().Count > 0)
                    {
                        if (classType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == pi.PropertyType).ToList().Count == 0)
                        {
                            IDomainObject parent = (IDomainObject)pi.GetValue(domainObject, null);
                            if (parent != null)
                            {
                                parents.Add(parent);

                                if (typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType))
                                {
                                    vParents.Add((IVersionControlled)parent);
                                }
                            }
                        }
                    }
                }
            }

            if (vParents.Count == 0)
            {
                foreach (IDomainObject parent in parents)
                {
                    vParents.AddRange(GetVersionControlledParent(parent, out parentParent));

                    if (vParents.Count > 0)
                    {
                        break;
                    }
                }
            }

            if (vParents.Count == 0 && classType == typeof(PropertyMap))
            {
                Dictionary<Type, List<IDomainObject>> mapParents = GetReferencingObjects(domainObject, new List<Type>(), new List<Type> { typeof(MappedProperty) });

                if (mapParents.Count == 1)
                {
                    if (mapParents.First().Value.Count > 0)
                    {
                        IDomainObject mapParent = mapParents.First().Value[0];

                        if (mapParents.First().Value.Count > 1 && mapParents.First().Key == typeof(View))
                        {
                            foreach (View viewParent in mapParents.First().Value.Cast<View>().ToList())
                            {
                                if (viewParent.RequestMap.Id != viewParent.ResponseMap.Id)
                                {
                                    mapParent = viewParent;
                                    break;
                                }
                            }
                        }

                        if (typeof(IVersionControlled).IsAssignableFrom(GetDomainObjectType(mapParent)))
                        {
                            vParents.Add((IVersionControlled)mapParent);
                            parents.Add(mapParent);
                        }
                        else
                        {
                            parents.Add(mapParent);
                            vParents.AddRange(GetVersionControlledParent(mapParent, out parentParent));
                        }
                    }
                }
                else if (mapParents.Count == 0)
                {
                    #region Get references in XML properties

                    IEnumerable<Type> domainObjectTypes = Assembly.GetAssembly(typeof(IDomainObject)).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDomainObject)) && t.GetProperties().Where(p => p.PropertyType == typeof(string) && p.Name.ToUpper().Contains("XML")).Count() > 0);

                    foreach (Type domainObjectType in domainObjectTypes)
                    {
                        foreach (PropertyInfo pi in domainObjectType.GetProperties().Where(p => p.PropertyType == typeof(string) && p.Name.ToUpper().Contains("XML")))
                        {
                            string query = "select o from " + domainObjectType.Name + " o where o." + pi.Name + " LIKE '%" + domainObject.Id + "%'";
                            IList<IDomainObject> xmlReferences = GetDomainObjectsByQuery<IDomainObject>(query);

                            if (xmlReferences.Count == 1)
                            {

                                if (typeof(IVersionControlled).IsAssignableFrom(GetDomainObjectType(xmlReferences.First())))
                                {
                                    vParents.Add((IVersionControlled)xmlReferences.First());
                                    parents.Add(xmlReferences.First());
                                }
                                else
                                {
                                    parents.Add(xmlReferences.First());
                                    vParents.AddRange(GetVersionControlledParent(xmlReferences.First(), out parentParent));
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            if (vParents.Count == 0)
            {
                if (classType.GetProperties().Where(p => p.PropertyType == typeof(Application)).Count() > 0)
                {
                    vParents.Add((IVersionControlled)classType.GetProperties().Where(p => p.PropertyType == typeof(Application)).FirstOrDefault().GetValue(domainObject, null));
                    parents.Add(vParents[0]);
                }
            }

            return vParents;
        }

        [Transaction(ReadOnly = true)]
        public IDomainObject GetDynamicInitializedDomainObject(Guid domainObjectId, Type classType, List<string> namedPropertyWithDomainObjectToInitialize)
        {
            IDomainObject theObject = null;

            theObject = GetDomainObject(domainObjectId, classType);

            InitializeDomainObject(theObject, namedPropertyWithDomainObjectToInitialize);

            return theObject;
        }

        [Transaction(ReadOnly = true)]
        public T GetDynamicInitializedDomainObject<T>(Guid domainObjectId, List<string> namedPropertyWithDomainObjectToInitialize)
        {
            return (T)GetDynamicInitializedDomainObject(domainObjectId, typeof(T), namedPropertyWithDomainObjectToInitialize);
        }

        [Transaction(ReadOnly = false)]
        public IVersionControlled MoveDomainObject(IVersionControlled vcObjectToMove, IVersionControlled vcNewParentObject)
        {
            Type classType = GetDomainObjectType(vcObjectToMove);
            vcObjectToMove = GetDomainObject(vcObjectToMove.Id, classType) as IVersionControlled;

            Type classTypeNewParrent = GetDomainObjectType(vcNewParentObject);
            vcNewParentObject = GetDomainObject(vcNewParentObject.Id, classTypeNewParrent) as IVersionControlled;

            List<IDomainObject> parents;
            IVersionControlled vcOldParent = GetVersionControlledParent(vcObjectToMove, out parents).First();

            List<PropertyInfo> propertyList = classType.GetProperties().ToList();
            PropertyInfo parentProperty = null;
            foreach (PropertyInfo pi in propertyList)
            {
                if (pi.GetValue(vcObjectToMove, null).Equals(vcOldParent))
                {
                    parentProperty = pi;
                }
            }

            IVersionControlled vcOldParent2 = parentProperty.GetValue(vcObjectToMove, null) as IVersionControlled;
            if (GetDomainObjectType(vcOldParent2) == GetDomainObjectType(vcNewParentObject))
            {
                ConfigurationManagementService.CheckOutDomainObject(vcObjectToMove.Id, vcObjectToMove.GetType());
                ConfigurationManagementService.CheckOutDomainObject(vcOldParent2.Id, vcOldParent2.GetType());
                ConfigurationManagementService.CheckOutDomainObject(vcNewParentObject.Id, vcNewParentObject.GetType());

                parentProperty.SetValue(vcObjectToMove, vcNewParentObject, null);
                SaveDomainObject(vcObjectToMove);
                DomainObjectDeleted(vcObjectToMove.Id, GetDomainObjectType(vcObjectToMove));
                DomainObjectAdded(vcObjectToMove.Id, GetDomainObjectType(vcObjectToMove));


            }
            else
            {
                throw new ModelException("Parent is of wrong type.");
            }


            return vcObjectToMove;
        }

        #endregion

        #region Helper functions

        private void InitializeDomainObject(IDomainObject theObject, List<string> namedPropertyWithDomainObjectToInitialize)
        {
            if (theObject != null)
            {
                foreach (PropertyInfo pi in theObject.GetType().GetProperties())
                {
                    object proxy = pi.GetGetMethod().Invoke(theObject, new object[0]);
                    if (proxy != null)
                    {
                        Type[] interfaces = proxy.GetType().GetInterfaces();
                        foreach (Type iface in interfaces)
                        {
                            if (iface == typeof(NHibernate.Proxy.INHibernateProxy) ||
                                iface == typeof(NHibernate.Collection.IPersistentCollection))
                            {
                                if (!NHibernate.NHibernateUtil.IsInitialized(proxy))
                                {
                                    NHibernate.NHibernateUtil.Initialize(proxy);
                                }

                                break;
                            }
                        }

                    }

                    if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType) || (pi.PropertyType.Name == typeof(IList<object>).Name && typeof(IDomainObject).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0])))
                    {
                        List<string> childDomainObjectToInitializePropertyNames = namedPropertyWithDomainObjectToInitialize.Where(s => s.StartsWith(pi.Name)).ToList();
                        if (childDomainObjectToInitializePropertyNames.Count > 0)
                        {
                            int i = 0;
                            while (i < childDomainObjectToInitializePropertyNames.Count)
                            {
                                if (childDomainObjectToInitializePropertyNames[i] == pi.Name)
                                {
                                    childDomainObjectToInitializePropertyNames.RemoveAt(i);
                                }
                                else
                                {
                                    childDomainObjectToInitializePropertyNames[i] = childDomainObjectToInitializePropertyNames[i].Replace(pi.Name + ".", "");
                                    i++;
                                }
                            }

                            if (typeof(IDomainObject).IsAssignableFrom(pi.PropertyType))
                            {
                                InitializeDomainObject((IDomainObject)pi.GetValue(theObject, null), childDomainObjectToInitializePropertyNames);
                            }
                            else
                            {
                                foreach (IDomainObject dobj in ((IEnumerable<IDomainObject>)pi.GetValue(theObject, null)))
                                {
                                    InitializeDomainObject(dobj, childDomainObjectToInitializePropertyNames);
                                }
                            }
                        }
                    }

                }
            }
        }

        private void DeleteMapsFromUXComponent(DataAccess.Domain.VisualModel.UXComponent domainComponent)
        {
            if (domainComponent != null)
            {
                foreach (PropertyInfo info in domainComponent.GetType().GetProperties())
                {
                    if (info.GetCustomAttributes(typeof(DomainReferenceAttribute), false).Count() > 0)
                    {
                        PropertyInfo idProperty = domainComponent.GetType().GetProperty(info.Name + "Id");

                        if (idProperty == null)
                            throw new Exception("Id property not found.");

                        Guid id = (Guid)idProperty.GetValue(domainComponent, null);

                        if (id != Guid.Empty)
                        {
                            if (info.PropertyType == typeof(DataAccess.Domain.PropertyMap))
                            {
                                DataAccess.Domain.PropertyMap map = GetDomainObject<DataAccess.Domain.PropertyMap>(id);

                                if (map != null)
                                {
                                    DeleteDomainObject(map);
                                }
                            }
                        }
                    }
                }

                DataAccess.Domain.VisualModel.UXContainer container = null;

                if (domainComponent is DataAccess.Domain.VisualModel.UXGroupBox)
                    container = (domainComponent as DataAccess.Domain.VisualModel.UXGroupBox).Container;
                else
                    container = domainComponent as DataAccess.Domain.VisualModel.UXContainer;

                if (container != null)
                {
                    foreach (DataAccess.Domain.VisualModel.UXComponent child in container.Children)
                        DeleteMapsFromUXComponent(child);
                }
            }
        }

        private void RemoveDomainObjectFromParent(IDomainObject domainObject, IDomainObject parent)
        {
            Type classType = GetDomainObjectType(domainObject);

            Type parentType = GetDomainObjectType(parent);
            parent = GetDomainObject(parent.Id, parentType);

            List<PropertyInfo> pil = parentType.GetProperties().Where(p => p.PropertyType == classType || (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == classType)).ToList();

            foreach (PropertyInfo pi in pil)
            {
                if (pi.PropertyType == classType)
                {
                    if (((IDomainObject)pi.GetValue(parent, null)) != null)
                    {
                        if (((IDomainObject)pi.GetValue(parent, null)).Id == domainObject.Id)
                        {
                            pi.SetValue(parent, null, null);
                        }
                    }
                }
                else if (pi.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && pi.PropertyType.GetGenericArguments()[0] == classType)
                {
                    IList<IDomainObject> domainObjects = ((IEnumerable<IDomainObject>)pi.GetValue(parent, null)).Where(o => o.Id == domainObject.Id).ToList();
                    if (domainObjects.Count > 0)
                    {
                        int index = (int)pi.PropertyType.GetMethod("IndexOf").Invoke(pi.GetValue(parent, null), new object[] { domainObjects[0] });
                        pi.PropertyType.GetMethod("RemoveAt").Invoke(pi.GetValue(parent, null), new object[] { index });
                    }
                }
            }
        }

        private Dictionary<IDomainObject, List<IDomainObject>> GetChildrenWithParents(IDomainObject domainObject, List<Type> parentTypes, out List<Type> childTypes)
        {
            Type classType = GetDomainObjectType(domainObject);
            Dictionary<IDomainObject, List<IDomainObject>> childrenWithParents = new Dictionary<IDomainObject, List<IDomainObject>>();

            childTypes = new List<Type>();

            foreach (PropertyInfo pi in classType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && typeof(IDomainObject).IsAssignableFrom(p.PropertyType.GetGenericArguments()[0])))
            {
                bool ignore = pi.GetCustomAttributes(typeof(DataAccess.DomainXmlIgnoreAttribute), true).Count() > 0;
                if (!ignore)
                {
                    if (!typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType.GetGenericArguments()[0]))
                    {
                        if (!childTypes.Contains(pi.PropertyType.GetGenericArguments()[0]))
                        {
                            childTypes.Add(pi.PropertyType.GetGenericArguments()[0]);

                            foreach (IDomainObject child in ((IEnumerable<IDomainObject>)pi.GetValue(domainObject, null)))
                            {
                                List<IDomainObject> childParents = new List<IDomainObject>();
                                GetVersionControlledParent(child, out childParents);
                                if (childParents.Where(p => p.Id == domainObject.Id && GetDomainObjectType(p) == GetDomainObjectType(domainObject)).Count() == 0)
                                {
                                    childParents.Add(domainObject);
                                }
                                childrenWithParents.Add(child, childParents);
                            }
                        }
                    }
                }
            }

            foreach (PropertyInfo pi in classType.GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType) && !typeof(IVersionControlled).IsAssignableFrom(p.PropertyType) && !parentTypes.Contains(p.PropertyType)))
            {
                bool ignore = pi.GetCustomAttributes(typeof(DataAccess.DomainXmlIgnoreAttribute), true).Count() > 0;
                if (!ignore)
                {
                    bool notchild = pi.GetCustomAttributes(typeof(DataAccess.DomainXmlByIdAttribute), true).Count() > 0;
                    if (!notchild)
                    {
                        if (!childTypes.Contains(pi.PropertyType))
                        {
                            childTypes.Add(pi.PropertyType);
                        }
                        IDomainObject child = (IDomainObject)pi.GetValue(domainObject, null);
                        if (child != null)
                        {
                            if (!childrenWithParents.ContainsKey(child))
                            {
                                List<IDomainObject> childParents = new List<IDomainObject>();
                                GetVersionControlledParent(child, out childParents);
                                if (childParents.Where(p => p.Id == domainObject.Id && GetDomainObjectType(p) == GetDomainObjectType(domainObject)).Count() == 0)
                                {
                                    childParents.Add(domainObject);
                                }
                                childrenWithParents.Add(child, childParents);
                            }
                        }
                    }
                }
            }

            return childrenWithParents;
        }

        private Dictionary<Type, List<IDomainObject>> GetReferencingObjects(IDomainObject domainObject, List<Type> parentTypes, List<Type> childTypes)
        {
            Dictionary<Type, List<IDomainObject>> returnList = new Dictionary<Type, List<IDomainObject>>();

            //Remove all child types that reference the current type, i.e. WorkflowSubworkflow
            childTypes.RemoveAll(t => t.GetProperties().Where(p => p.PropertyType == GetDomainObjectType(domainObject)).Count() > 1);
            
            IEnumerable<Type> domainObjectTypes = Assembly.GetAssembly(typeof(IDomainObject)).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDomainObject)) && !parentTypes.Contains(t) && !childTypes.Contains(t));

            foreach (Type domainClassType in domainObjectTypes)
            {
                if (!returnList.ContainsKey(domainClassType))
                {
                    if (domainClassType.GetProperties().Where(p => p.PropertyType == GetDomainObjectType(domainObject)).Count() > 0)
                    {
                        string query = string.Empty;
                        query = "select o from " + domainClassType.Name + " o where ";

                        string whereClause = string.Empty;

                        List<IDomainObject> objects = new List<IDomainObject>();
                        foreach (PropertyInfo pi in domainClassType.GetProperties().Where(p => p.PropertyType == GetDomainObjectType(domainObject)))
                        {
                            if (whereClause != string.Empty)
                            {
                                whereClause += " or ";
                            }

                            whereClause += "o." + pi.Name + ".Id = '" + domainObject.Id.ToString() + "'";


                        }

                        query += whereClause;

                        objects.AddRange(((DataAccess.Dao.IDynamicDao)ctx["DynamicDao"]).FindByQuery(query));

                        if (objects.Count > 0)
                        {
                            returnList.Add(domainClassType, objects);
                        }
                    }
                }
            }

            return returnList;
        }

        private enum MapSyncOption
        {
            Source,
            Target
        }

        private Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>> GetReferencingPropertyMapsWithOwner(List<PropertyMap> sourceMaps, List<IDomainObject> objectsToDelete, Dictionary<string, List<MappedProperty>> mappedPropertyLoockup)
        {
            Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>> returnReferences = new Dictionary<PropertyMap, Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>>();
            List<IDomainObject> parents = new List<IDomainObject>();
            IList<IVersionControlled> versionControlledParents;
            Dictionary<Guid, IVersionControlled> parentDictionary = new Dictionary<Guid, IVersionControlled>();


            foreach (PropertyMap sourceMap in sourceMaps)
            {
                if (!returnReferences.ContainsKey(sourceMap))
                {
                    returnReferences.Add(sourceMap, new Dictionary<IVersionControlled, Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>>());

                    foreach (MappedProperty mappedProperty in sourceMap.MappedProperties)
                    {
                        Dictionary<Type, List<IDomainObject>> mpReferences = new Dictionary<Type, List<IDomainObject>>();
                        if (mappedPropertyLoockup != null)
                        {
                            if (mappedPropertyLoockup.ContainsKey(mappedProperty.Id.ToString()))
                            {
                                mpReferences.Add(typeof(MappedProperty), mappedPropertyLoockup[mappedProperty.Id.ToString()].Cast<IDomainObject>().ToList());
                            }
                        }
                        else
                        {
                            mpReferences = GetReferencingObjects(mappedProperty, new List<Type> { typeof(PropertyMap) }, new List<Type>());
                        }

                        foreach (Type refType in mpReferences.Keys)
                        {
                            foreach (IDomainObject refObj in mpReferences[refType])
                            {
                                if (GetDomainObjectType(refObj) == typeof(MappedProperty))
                                {
                                    MapSyncOption mapSyncOption = MapSyncOption.Source;

                                    if (((MappedProperty)refObj).Target != null && ((MappedProperty)refObj).Target.Id == mappedProperty.Id)
                                    {
                                        mapSyncOption = MapSyncOption.Target;
                                    }
                                    else if (((MappedProperty)refObj).Source != null && ((MappedProperty)refObj).Source.Id == mappedProperty.Id)
                                    {
                                        mapSyncOption = MapSyncOption.Source;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                    if (parentDictionary.ContainsKey(((MappedProperty)refObj).PropertyMap.Id))
                                    {
                                        versionControlledParents = new List<IVersionControlled>();

                                        parents = new List<IDomainObject>();
                                        parents.Add(((MappedProperty)refObj).PropertyMap);

                                        if (parentDictionary[((PropertyMap)parents[0]).Id] != null)
                                        {
                                            versionControlledParents.Add(parentDictionary[((PropertyMap)parents[0]).Id]);
                                        }
                                    }
                                    else
                                    {
                                        versionControlledParents = GetVersionControlledParent(refObj, out parents);

                                        //Add Unreferenced map to deletelist
                                        if (versionControlledParents.Count == 0 && parents.Count > 0 && GetDomainObjectType(parents[0]) == typeof(PropertyMap))
                                        {
                                            objectsToDelete.Add(parents[0]);
                                        }
                                    }

                                    if (parents.Count > 0 && parents.Where(p => GetDomainObjectType(p) == typeof(PropertyMap)).Count() > 0)
                                    {
                                        PropertyMap ownerMap = (PropertyMap)parents.Where(p => GetDomainObjectType(p) == typeof(PropertyMap)).First();

                                        if (!parentDictionary.ContainsKey(ownerMap.Id))
                                        {
                                            if (versionControlledParents.Count > 0)
                                            {
                                                parentDictionary.Add(ownerMap.Id, versionControlledParents[0]);
                                            }
                                            else
                                            {
                                                parentDictionary.Add(ownerMap.Id, null);
                                            }
                                        }

                                        if (versionControlledParents.Count > 0)
                                        {
                                            if (!returnReferences[sourceMap].ContainsKey(versionControlledParents[0]) || returnReferences[sourceMap][versionControlledParents[0]].Keys.Where(k => k.Id == ownerMap.Id).Count() == 0)
                                            {
                                                SetTargetChoice setTarget = SetTargetChoice.No;
                                                Dictionary<Type, List<IDomainObject>> ownerMapOwners = GetReferencingObjects(ownerMap, new List<Type>(), new List<Type> { typeof(MappedProperty) });

                                                if (ownerMapOwners.Count > 0)
                                                {
                                                    IDomainObject ownerMapOwner = ownerMapOwners.First().Value.First();

                                                    if (ownerMapOwners.First().Value.Count > 1)
                                                    {
                                                        if (ownerMapOwners.First().Key == typeof(View))
                                                        {
                                                            foreach (View ownerView in ownerMapOwners.First().Value.Cast<View>().ToList())
                                                            {
                                                                if (ownerView.RequestMap.Id != ownerView.ResponseMap.Id)
                                                                {
                                                                    ownerMapOwner = ownerView;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (GetDomainObjectType(ownerMapOwner) == typeof(View))
                                                    {
                                                        if (((View)ownerMapOwner).RequestMap.Id == ((View)ownerMapOwner).ResponseMap.Id)
                                                        {
                                                            continue;
                                                        }
                                                    }


                                                    List<PropertyInfo> ownerMapPis = GetDomainObjectType(ownerMapOwner).GetProperties().Where(p => p.PropertyType == typeof(PropertyMap) && ((IDomainObject)p.GetValue(ownerMapOwner, null)).Id == ownerMap.Id).ToList();
                                                    if (ownerMapPis.Count > 0)
                                                    {
                                                        setTarget = ((PropertyMapAttribute)ownerMapPis[0].GetCustomAttributes(typeof(PropertyMapAttribute), true)[0]).SetTarget;
                                                    }
                                                }

                                                if (!returnReferences[sourceMap].ContainsKey(versionControlledParents[0]))
                                                {
                                                    returnReferences[sourceMap].Add(versionControlledParents[0], new Dictionary<PropertyMap, KeyValuePair<MapSyncOption, SetTargetChoice>>());
                                                }

                                                ownerMap = GetDomainObject<PropertyMap>(ownerMap.Id);
                                                if (!returnReferences[sourceMap][versionControlledParents[0]].ContainsKey(ownerMap))
                                                {
                                                    returnReferences[sourceMap][versionControlledParents[0]].Add(ownerMap, new KeyValuePair<MapSyncOption, SetTargetChoice>(mapSyncOption, setTarget));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return returnReferences;
        }

        private List<IDomainObject> CheckAndBreakReference(IDomainObject domainObject, List<IDomainObject> parents, bool throwExceptions, bool checkChildren, bool breakReference)
        {
            List<IDomainObject> returnList = new List<IDomainObject>();

            if (domainObject == null) { return returnList; }

            domainObject = GetDomainObject(domainObject.Id, GetDomainObjectType(domainObject));
            string idText = string.Empty;
            Type classType = GetDomainObjectType(domainObject);
            List<Type> parentTypes = new List<Type>();
            Dictionary<IDomainObject, List<IDomainObject>> childrenWithParents = new Dictionary<IDomainObject, List<IDomainObject>>();



            foreach (IDomainObject parent in parents)
            {
                if (!parentTypes.Contains(GetDomainObjectType(parent)))
                {
                    parentTypes.Add(GetDomainObjectType(parent));
                }
            }

            List<Type> childTypes = new List<Type>();

            childrenWithParents = GetChildrenWithParents(domainObject, parentTypes, out childTypes);


            Dictionary<Type, List<string>> referenceTypeAndIds = new Dictionary<Type, List<string>>();
            Dictionary<Type, List<IDomainObject>> referencingObjects = GetReferencingObjects(domainObject, parentTypes, childTypes);

            foreach (List<IDomainObject> objects in referencingObjects.Values)
            {
                foreach (IDomainObject obj in objects)
                {
                    returnList.Add(obj);


                    IList<IVersionControlled> vParents = new List<IVersionControlled>();

                    if (obj is IVersionControlled)
                    {
                        vParents.Add((IVersionControlled)obj);
                    }
                    else
                    {
                        List<IDomainObject> objParents = new List<IDomainObject>();
                        vParents = GetVersionControlledParent(obj, out objParents);
                    }

                    if (vParents.Count > 0)
                    {
                        if (!parentTypes.Contains(GetDomainObjectType(vParents[0])))
                        {
                            if (!referenceTypeAndIds.ContainsKey(GetDomainObjectType(vParents[0])))
                            {
                                referenceTypeAndIds.Add(GetDomainObjectType(vParents[0]), new List<string>());
                            }

                            referenceTypeAndIds[GetDomainObjectType(vParents[0])].Add(vParents[0].Id.ToString());
                        }

                        if (!returnList.Contains(vParents[0]))
                        {
                            returnList.Add(vParents[0]);
                        }
                    }
                    else
                    {
                        bool refbroken = false;
                        IDomainObject owner = obj;
                        if (obj is DataAccess.Domain.MappedProperty)
                        {
                            owner = GetOwnerForMappedProperty((DataAccess.Domain.MappedProperty)obj);
                            if (owner == null) { owner = obj; }


                            //Brerak reference
                            if (((MappedProperty)obj).Target != null)
                            {
                                if (((MappedProperty)obj).Target.Id == domainObject.Id)
                                {
                                    List<IDomainObject> tmpparents = new List<IDomainObject>();
                                    IList<IVersionControlled> tmpVParents = GetVersionControlledParent(owner, out tmpparents);
                                    if (tmpVParents.Count == 1)
                                    {
                                        ConfigurationManagementService.CheckOutDomainObject(tmpVParents[0].Id, GetDomainObjectType(tmpVParents[0]));
                                        ((MappedProperty)obj).Target = null;
                                        SaveDomainObject(obj);
                                        refbroken = true;
                                    }
                                }
                            }
                        }

                        if (!refbroken)
                        {

                            if (!returnList.Contains(owner))
                            {
                                returnList.Add(owner);
                            }

                            if (!parentTypes.Contains(GetDomainObjectType(owner)))
                            {
                                if (!referenceTypeAndIds.ContainsKey(GetDomainObjectType(owner)))
                                {
                                    referenceTypeAndIds.Add(GetDomainObjectType(owner), new List<string>());
                                }

                                referenceTypeAndIds[GetDomainObjectType(owner)].Add(owner.Id.ToString());
                            }
                        }
                    }

                }
            }


            if (throwExceptions)
            {
                if (referenceTypeAndIds.Count > 0)
                {
                    List<string> ids = new List<string>();
                    string message = "There are references from this " + classType.Name + " to the following types of objects with the corresponding Ids. The references need to be removed or changed.\r\n";

                    foreach (Type t in referenceTypeAndIds.Keys)
                    {
                        message += "\r\n" + t.Name;

                        foreach (string s in referenceTypeAndIds[t])
                        {
                            message += "\r\n" + s;
                            ids.Add(s);
                        }

                        message += "\r\n";
                    }

                    throw new ModelAggregatedException(ids, message);
                }
            }

            if (breakReference)
            {
                if (domainObject is DataAccess.Domain.Dialog)
                {
                    if (((DataAccess.Domain.Dialog)domainObject).SearchPanelView != null)
                    {
                        ((DataAccess.Domain.Dialog)domainObject).SearchPanelView.RequestMap = null;
                        ((DataAccess.Domain.Dialog)domainObject).SearchPanelView.ResponseMap = null;
                        SaveDomainObject(((DataAccess.Domain.Dialog)domainObject).SearchPanelView);
                    }
                }
            }

            if (checkChildren)
            {
                foreach (IDomainObject child in childrenWithParents.Keys)
                {
                    foreach (IDomainObject parent in parents)
                    {
                        if (!childrenWithParents[child].Contains(parent))
                        {
                            childrenWithParents[child].Add(parent);
                        }
                    }
                    returnList.AddRange(CheckAndBreakReference(child, childrenWithParents[child], throwExceptions, checkChildren, breakReference));
                }
            }

            return returnList;
        }

        private IDomainObject GetOwnerForMappedProperty(DataAccess.Domain.MappedProperty map)
        {
            GetContext();
            DataAccess.Domain.MappedProperty mp = GetDomainObject<DataAccess.Domain.MappedProperty>(map.Id);
            string propertyMapId = mp.PropertyMap.Id.ToString();

            IEnumerable<Type> domainObjectTypes = Assembly.GetAssembly(typeof(IDomainObject)).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDomainObject)) && t.GetProperties().Where(p => p.PropertyType == typeof(DataAccess.Domain.PropertyMap)).Count() > 0 && t != typeof(DataAccess.Domain.MappedProperty));

            foreach (Type domainObjectType in domainObjectTypes)
            {
                foreach (PropertyInfo pi in domainObjectType.GetProperties().Where(p => p.PropertyType == typeof(DataAccess.Domain.PropertyMap)))
                {
                    string query = "select o from " + domainObjectType.Name + " o where o." + pi.Name + ".Id = '" + propertyMapId + "'";
                    IList<IDomainObject> owners = ((DataAccess.Dao.IDynamicDao)ctx["DynamicDao"]).FindByQuery(query);
                    if (owners.Count > 0)
                    {
                        return owners[0];
                    }
                }
            }

            domainObjectTypes = Assembly.GetAssembly(typeof(IDomainObject)).GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDomainObject)) && t.GetProperties().Where(p => p.PropertyType == typeof(string) && p.Name.ToUpper().Contains("XML")).Count() > 0);

            foreach (Type domainObjectType in domainObjectTypes)
            {
                foreach (PropertyInfo pi in domainObjectType.GetProperties().Where(p => p.PropertyType == typeof(string) && p.Name.ToUpper().Contains("XML")))
                {
                    string query = "select o from " + domainObjectType.Name + " o where o." + pi.Name + " LIKE '%" + propertyMapId + "%'";
                    IList<IDomainObject> owners = ((DataAccess.Dao.IDynamicDao)ctx["DynamicDao"]).FindByQuery(query);
                    if (owners.Count > 0)
                    {
                        return owners[0];
                    }
                }
            }


            return null;
        }



        [Transaction(ReadOnly = true)]
        public void GenerateApplication(List<string> frontendApplications, List<string> backendApplications, bool ignoreCheckOuts = false)
        {
            List<Application> applicationsToGenerate = new List<Application>();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (!frontendApplications.Contains("All"))
            {
                foreach (string appName in frontendApplications)
                {
                    parameters.Add("Name", appName);
                }
            }

            parameters.Add("IsFrontend", true);

            applicationsToGenerate.AddRange(GetAllDomainObjectsByPropertyValues<Application>(parameters));

            parameters.Clear();

            if (!backendApplications.Contains("All"))
            {
                foreach (string appName in backendApplications)
                {
                    parameters.Add("Name", appName);
                }
            }

            parameters.Add("IsFrontend", false);

            applicationsToGenerate.AddRange(GetAllDomainObjectsByPropertyValues<Application>(parameters));

            GenerateApplication(applicationsToGenerate, ignoreCheckOuts);
        }

        [Transaction(ReadOnly = true)]
        public void GenerateApplication(List<Application> applications, bool ignoreCheckOuts = false, Dictionary<Guid, List<Guid>> SelectedModulesOrServicesPerApplication = null)
        {
            try
            {
                DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = true;
                DataAccess.DomainXmlSerializationHelper.UseFileCache = true;

                List<string> appPaths = new List<string>();

                System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
                string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();

                foreach (Application app in applications)
                {
                    appPaths.Add(System.IO.Path.Combine(rootPath, app.Name + (app.IsFrontend.Value ? "_Frontend" : "_Backend")));
                }

                if (!ignoreCheckOuts)
                {
                    onStatusChanged("[OVERALL,1,START]", 0, 0, 0);
                    GetCheckedOutObjectsToDomainObjectLookUp(applications.Select(v => v.Id).ToList());
                    onStatusChanged("[OVERALL,1,END]", 0, 0, 0);
                }

                onStatusChanged("[OVERALL,2,START]", 0, 0, 0);

                //Wait while cache is loading
                DataAccess.DomainXmlSerializationHelper.WaitingForCache = true;
                while (DataAccess.DomainXmlSerializationHelper.FileCacheUpdating)
                {
                    System.Threading.Thread.Sleep(50);
                }
                DataAccess.DomainXmlSerializationHelper.WaitingForCache = false;

                ConfigurationManagementService.ImportDomainObjects(appPaths, false, false);
                onStatusChanged("[OVERALL,2,END]", 0, 0, 0);


                onStatusChanged("[OVERALL,3,START]", 0, 0, 0);
                string genrootPath = appReader.GetValue("CodeGeneratingOutputRootFolder", typeof(System.String)).ToString();

                foreach (Application applicationToGenerate in DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp[typeof(Application)].Values)
                {

                    List<IDomainObject> selectedModulesOrService = (applicationToGenerate.IsFrontend.Value ? applicationToGenerate.Modules.ToList<IDomainObject>() : applicationToGenerate.Services.ToList<IDomainObject>());

                    if (SelectedModulesOrServicesPerApplication != null)
                    {
                        if (SelectedModulesOrServicesPerApplication.ContainsKey(applicationToGenerate.Id))
                        {
                            if (SelectedModulesOrServicesPerApplication[applicationToGenerate.Id].Count > 0)
                            {
                                selectedModulesOrService.Clear();
                                foreach (Guid id in SelectedModulesOrServicesPerApplication[applicationToGenerate.Id])
                                {
                                    Type typeForList = (applicationToGenerate.IsFrontend.Value ? typeof(Cdc.MetaManager.DataAccess.Domain.Module) : typeof(Service));
                                    if (DomainXmlSerializationHelper.DomainObjectLookUp[typeForList].ContainsKey(id))
                                    {
                                        selectedModulesOrService.Add(DomainXmlSerializationHelper.DomainObjectLookUp[typeForList][id]);
                                    }
                                }
                            }
                            else
                            {
                                selectedModulesOrService.Clear();
                            }
                        }
                    }

                    if (selectedModulesOrService.Count > 0)
                    {
                        if (applicationToGenerate.IsFrontend.Value)
                        {
                            onStatusChanged("Start Generating Frontend", 0, 0, 0);
                            string applicationRootPath = System.IO.Path.Combine(genrootPath, @"FE\", applicationToGenerate.Name);
                            GenerateFrontendCode(selectedModulesOrService.Cast<Cdc.MetaManager.DataAccess.Domain.Module>().ToList(), applicationToGenerate, System.IO.Path.Combine(applicationRootPath, "Auto.sln"), System.IO.Path.Combine(applicationRootPath, "temp"));
                        }
                        else
                        {
                            onStatusChanged("Start Generating Backend", 0, 0, 0);
                            string applicationRootPath = System.IO.Path.Combine(genrootPath, @"BE\", applicationToGenerate.Name);
                            GenerateBackendCode(selectedModulesOrService.Cast<Cdc.MetaManager.DataAccess.Domain.Service>().ToList(), System.IO.Path.Combine(applicationRootPath, "Auto.sln"), System.IO.Path.Combine(applicationRootPath, "temp"));
                        }
                    }
                }
                onStatusChanged("[OVERALL,3,END]", 0, 0, 0);



            }
            finally
            {
                DataAccess.DomainXmlSerializationHelper.DomainObjectLookUp.Clear();
                DataAccess.DomainXmlSerializationHelper.MissingReferenses.Clear();
                DataAccess.DomainXmlSerializationHelper.PossibleParentReferences.Clear();
                DataAccess.DomainXmlSerializationHelper.DontUseDBorSession = false;
                DataAccess.DomainXmlSerializationHelper.UseFileCache = false;
            }
        }

        private void GetCheckedOutObjectsToDomainObjectLookUp(List<Guid> applicationIds)
        {
            if (DataAccess.DomainXmlSerializationHelper.DontUseDBorSession)
            {
                onStatusChanged("Getting checked out objects", 0, 0, 0);

                List<IVersionControlled> checkedOutObjects = new List<IVersionControlled>();
                IList<Type> allVersionControlledTypes = GetAllVersionControlledTypes();

                Dictionary<string, object> searchProps = new Dictionary<string, object>();
                searchProps.Add("LockedBy", Environment.UserName);
                searchProps.Add("IsLocked", true);

                foreach (Type versionControlledType in allVersionControlledTypes)
                {
                    IList<IDomainObject> foundCheckouts = GetAllDomainObjectsByPropertyValues(versionControlledType, searchProps, false, false);

                    foreach (IVersionControlled foundCheckOut in foundCheckouts)
                    {
                        IVersionControlled parentApplication = null;

                        if (versionControlledType == typeof(Application))
                        {
                            parentApplication = foundCheckOut;
                        }
                        else
                        {
                            List<IDomainObject> parents = new List<IDomainObject>();
                            IList<IVersionControlled> vParents = GetVersionControlledParent(foundCheckOut, out parents);

                            parentApplication = vParents.Where(o => GetDomainObjectType(o) == typeof(Application)).FirstOrDefault();

                            while (parentApplication == null)
                            {
                                vParents = GetVersionControlledParent(vParents[0], out parents);
                                parentApplication = vParents.Where(o => GetDomainObjectType(o) == typeof(Application)).FirstOrDefault();
                            }
                        }

                        if (applicationIds.Contains(parentApplication.Id))
                        {
                            checkedOutObjects.Add(foundCheckOut);
                        }
                    }
                }

                int index = 0;
                foreach (IVersionControlled checkedOutObj in checkedOutObjects)
                {
                    onStatusChanged("Getting checked out " + checkedOutObj.GetType().Name + ": " + checkedOutObj.Id.ToString(), index, 0, checkedOutObjects.Count);
                    index++;

                    Type classType = GetDomainObjectType(checkedOutObj);


                    //System.IO.TextWriter textWriter = new System.IO.StringWriter();
                    //System.Xml.XmlWriter writer = new System.Xml.XmlTextWriter(textWriter);
                    //((System.Xml.Serialization.IXmlSerializable)checkedOutObj).WriteXml(writer);

                    System.Xml.XmlWriterSettings xmlSettings = new System.Xml.XmlWriterSettings();
                    xmlSettings.Indent = true;
                    xmlSettings.NewLineHandling = System.Xml.NewLineHandling.Entitize;

                    StringBuilder stringBuilder = new StringBuilder();
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(classType);
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stringBuilder, xmlSettings);
                    xmlSerializer.Serialize(writer, checkedOutObj);
                    writer.Close();


                    ConfigurationManagementService.GetDomainObjectFromConfMgn(classType, stringBuilder.ToString(), true);
                }
            }

        }

        [Transaction(ReadOnly = true)]
        public void GenerateBackendCode(List<Cdc.MetaManager.DataAccess.Domain.Service> selectedServices, string solutionFileNameAndPath, string referencePath)
        {
            try
            {
                List<DataAccess.Domain.Service> loadedServices = new List<DataAccess.Domain.Service>();

                if (DataAccess.DomainXmlSerializationHelper.DontUseDBorSession)
                {
                    loadedServices = selectedServices;
                }
                else
                {
                    foreach (DataAccess.Domain.Service selectedService in selectedServices)
                    {
                        loadedServices.Add(GetDomainObject<DataAccess.Domain.Service>(selectedService.Id));
                    }
                }

                // Get which business entities we need to generate depending on the selected services.
                Dictionary<Guid, BusinessEntity> entityDictionary = new Dictionary<Guid, BusinessEntity>();

                foreach (Service service in loadedServices)
                {
                    foreach (ServiceMethod method in service.ServiceMethods)
                    {
                        if (method != null)
                        {
                            if (!entityDictionary.ContainsKey(method.MappedToAction.BusinessEntity.Id))
                            {
                                entityDictionary.Add(method.MappedToAction.BusinessEntity.Id, method.MappedToAction.BusinessEntity);
                            }
                        }
                    }
                }

                Cdc.MetaManager.CodeGeneration.CodeSmithTemplates.ApplicationTemplate template = new Cdc.MetaManager.CodeGeneration.CodeSmithTemplates.ApplicationTemplate();
                template.entities = entityDictionary.Values.ToList<BusinessEntity>();
                template.services = loadedServices;
                template.solutionFileName = solutionFileNameAndPath;
                template.referenceDirectory = referencePath;
                template.templateCallback = CodeSmithTemplateCallback;

                template.Render(System.IO.TextWriter.Null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Codegeneration", ex);
            }
        }

        [Transaction(ReadOnly = true)]
        public void GenerateFrontendCode(List<Cdc.MetaManager.DataAccess.Domain.Module> selectedModules, DataAccess.Domain.Application frontendApplication, string solutionFileNameAndPath, string referencePath)
        {
            try
            {
                //Ta bort?
                Dictionary<Guid, IDomainObject> loadedObjects = new Dictionary<Guid, IDomainObject>();

                Dictionary<Guid, Service> serviceDictionary = new Dictionary<Guid, Service>();
                List<DataAccess.Domain.Module> loadedModules = new List<DataAccess.Domain.Module>();

                if (DataAccess.DomainXmlSerializationHelper.DontUseDBorSession)
                {
                    loadedModules = selectedModules;
                }
                else
                {
                    foreach (DataAccess.Domain.Module selectedModule in selectedModules)
                    {
                        loadedModules.Add(GetDomainObject<DataAccess.Domain.Module>(selectedModule.Id));
                    }
                }

                BusinessLogic.Helpers.ModuleHelper.InitializeUXComponentForAllViews(loadedModules, loadedObjects);

                onStatusChanged("Start service reference search", 0, 0, 0);
                int i = 0;
                foreach (DataAccess.Domain.Module module in loadedModules)
                {
                    i++;
                    foreach (Dialog dialog in module.Dialogs)
                    {
                        if (dialog.SearchPanelView != null)
                        {
                            onStatusChanged("Finding service references in View: " + dialog.SearchPanelView.Id.ToString(), i, 0, loadedModules.Count);
                            MetaManagerServices.Helpers.ViewHelper.FindComponentServiceReferences(dialog.SearchPanelView.VisualTree, serviceDictionary);
                        }

                        foreach (ViewNode vn in dialog.ViewNodes)
                        {
                            if (vn.View != null)
                            {
                                onStatusChanged("Finding service references in View: " + vn.View.Id.ToString(), i, 0, loadedModules.Count);

                                DataAccess.Domain.View view = vn.View;

                                foreach (DataSource ds in view.DataSources)
                                {
                                    if (!serviceDictionary.ContainsKey(ds.ServiceMethod.Service.Id))
                                        serviceDictionary.Add(ds.ServiceMethod.Service.Id, ds.ServiceMethod.Service);
                                }

                                if ((view.ServiceMethod != null) && (view.ServiceMethod.Service != null))
                                {
                                    if (!serviceDictionary.ContainsKey(view.ServiceMethod.Service.Id))
                                        serviceDictionary.Add(view.ServiceMethod.Service.Id, view.ServiceMethod.Service);
                                }

                                foreach (ViewAction viewAction in vn.ViewActions)
                                {
                                    if ((viewAction.Action.ServiceMethod != null) && (viewAction.Action.ServiceMethod.Service != null))
                                    {
                                        if (!serviceDictionary.ContainsKey(viewAction.Action.ServiceMethod.Service.Id))
                                            serviceDictionary.Add(viewAction.Action.ServiceMethod.Service.Id, viewAction.Action.ServiceMethod.Service);
                                    }
                                }

                                MetaManagerServices.Helpers.ViewHelper.FindComponentServiceReferences(view.VisualTree, serviceDictionary);
                            }
                        }
                    }

                    foreach (DataAccess.Domain.Workflow workflow in module.Workflows)
                    {
                        onStatusChanged("Finding service references in Workflow: " + workflow.Id.ToString(), i, 0, loadedModules.Count);

                        foreach (WorkflowServiceMethod workflowServiceMethod in workflow.ServiceMethods)
                        {
                            if (!serviceDictionary.ContainsKey(workflowServiceMethod.ServiceMethod.Service.Id))
                                serviceDictionary.Add(workflowServiceMethod.ServiceMethod.Service.Id, workflowServiceMethod.ServiceMethod.Service);
                        }
                    }
                }

                List<Service> serviceList = new List<Service>(serviceDictionary.Values);
                Cdc.MetaManager.DataAccess.Domain.Menu menu;


                menu = frontendApplication.Menu;


                Cdc.MetaManager.CodeGeneration.CodeSmithTemplates.UXTemplate template = new Cdc.MetaManager.CodeGeneration.CodeSmithTemplates.UXTemplate();

                template.modules = loadedModules;
                template.referenceDirectory = referencePath;
                template.solutionFileName = solutionFileNameAndPath;
                template.services = serviceList;
                template.menu = menu;
                template.templateCallback = CodeSmithTemplateCallback;

                template.Render(System.IO.TextWriter.Null);

            }
            catch (Exception ex)
            {
                throw new Exception("Error in Codegeneration", ex);
            }

        }

        [Transaction(ReadOnly = true)]
        public IList<IVersionControlled> GetReferencedVersionControlled(IDomainObject domainObject)
        {
            List<IVersionControlled> returnList = new List<IVersionControlled>();
            Type classType = GetDomainObjectType(domainObject);
            domainObject = GetDomainObject(domainObject.Id, classType);

            if (domainObject != null)
            {
                foreach (PropertyInfo pi in classType.GetProperties())
                {
                    if (typeof(IVersionControlled).IsAssignableFrom(pi.PropertyType))
                    {
                        object obj = pi.GetValue(domainObject, null);
                        if (obj != null)
                        {
                            returnList.Add((IVersionControlled)GetDomainObject(((IDomainObject)obj).Id, pi.PropertyType));
                        }
                    }
                }
            }
            return returnList;
        }

        private void CodeSmithTemplateCallback(string text, int percentageDone)
        {
            onStatusChanged(text, percentageDone, 0, 100);
        }

        private void onStatusChanged(string message, int value, int min, int max)
        {
            if (StatusChanged != null)
            {
                StatusChanged(message, value, min, max);
            }
        }

        #endregion
    }
}
