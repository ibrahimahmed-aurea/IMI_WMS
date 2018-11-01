using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using Spring.Context.Support;
using NHibernate;
using Spring.Transaction.Interceptor;
using NHibernate.Proxy;



namespace Cdc.MetaManager.DataAccess
{
    public struct MissingReference
    {
        public object MethodOrPropertyInfo;
        public object TargetObject;
        public Guid RefObjectId;
        public Type RefObjectType;
    }

    public static class DomainXmlSerializationHelper
    {
        //File Cache
        public static Dictionary<string, string> XmlFileCache = new Dictionary<string, string>();
        public static bool UseFileCache = false;
        public static bool FileCacheUpdating = false;
        public static bool WaitingForCache = false;


        public static bool DontUseDBorSession = false;
        public static Dictionary<Type, Dictionary<Guid, IDomainObject>> DomainObjectLookUp = new Dictionary<Type, Dictionary<Guid, IDomainObject>>();
        public static List<MissingReference> MissingReferenses = new List<MissingReference>();

        public static Dictionary<Type, Dictionary<Guid, Dictionary<Type, List<Guid>>>> PossibleParentReferences = new Dictionary<Type, Dictionary<Guid, Dictionary<Type, List<Guid>>>>();

        public static List<IDomainObject> VisualComponentRefObjects = new List<IDomainObject>();

        private static Dictionary<Type, Dictionary<Guid, object>> referenceDictionary = new Dictionary<Type, Dictionary<Guid, object>>();
        private static object rootObj = null;

        public static object GetObjectFromRef(Guid id, Type type)
        {
            if (referenceDictionary.ContainsKey(type))
            {
                if (referenceDictionary[type].ContainsKey(id))
                {
                    return referenceDictionary[type][id];
                }
            }

            return null;
        }

        public static void ReadXML(object thisobject, XmlReader reader)
        {
            Dictionary<string, PropertyInfo> properties = thisobject.GetType().GetProperties().ToDictionary(v => v.Name, v => v);
            PropertyInfo property;
            XmlDocument doc = new XmlDocument();
            XmlDocument tmpdoc;
            bool skipMaps = false;
            Guid tempId = Guid.Empty;


            if (DontUseDBorSession)
            {
                if (!DomainObjectLookUp.ContainsKey(thisobject.GetType()))
                {
                    lock (DomainObjectLookUp)
                    {
                        if (!DomainObjectLookUp.ContainsKey(thisobject.GetType()))
                        {
                            DomainObjectLookUp.Add(thisobject.GetType(), new Dictionary<Guid, IDomainObject>());
                        }
                    }
                }
            }
            else
            {

                if (rootObj == null)
                {
                    rootObj = thisobject;
                    referenceDictionary.Clear();
                }

                if (!referenceDictionary.ContainsKey(NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(thisobject)))
                {
                    referenceDictionary.Add(NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(thisobject), new Dictionary<Guid, object>());
                }
            }



            doc.Load(reader);

            if (doc.FirstChild.Name == thisobject.GetType().Name)
            {
                if (DontUseDBorSession)
                {
                    if (thisobject is Domain.View)
                    {
                        if (doc.ChildNodes[0].SelectSingleNode("RequestMap").ChildNodes.Count > 0 & doc.ChildNodes[0].SelectSingleNode("ResponseMap").ChildNodes.Count > 0)
                        {
                            if (doc.ChildNodes[0].SelectSingleNode("RequestMap").ChildNodes[0].ChildNodes[0].InnerText == doc.ChildNodes[0].SelectSingleNode("ResponseMap").ChildNodes[0].ChildNodes[0].InnerText)
                            {
                                skipMaps = true;
                            }
                        }
                    }
                }

                foreach (XmlNode node in doc.FirstChild.ChildNodes)
                {
                    if (properties.ContainsKey(node.Name) && node.Attributes.GetNamedItem("uxComponentProperty") == null)
                    {
                        property = properties[node.Name];

                        object[] ignoreAttributes = property.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true);

                        if (ignoreAttributes.Length == 0)
                        {
                            if (property.PropertyType.IsPrimitive || property.PropertyType.Name == "String")
                            {
                                property.SetValue(thisobject, Convert.ChangeType(node.InnerText, property.PropertyType), null);
                            }
                            else if (property.PropertyType.Name == typeof(System.Nullable<int>).Name)
                            {
                                property.SetValue(thisobject, string.IsNullOrEmpty(node.InnerText) ? null : Convert.ChangeType(node.InnerText, property.PropertyType.GetGenericArguments()[0]), null);
                            }
                            else if (property.PropertyType.Name == "Guid")
                            {
                                Guid id = new Guid(node.InnerText);
                                property.SetValue(thisobject, id, null);

                                if (property.Name == "Id")
                                {
                                    if (DontUseDBorSession)
                                    {
                                        lock (DomainObjectLookUp)
                                        {
                                            if (!DomainObjectLookUp[thisobject.GetType()].ContainsKey(id))
                                            {
                                                DomainObjectLookUp[thisobject.GetType()].Add(id, (IDomainObject)thisobject);
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!referenceDictionary[NHibernateProxyHelper.GetClassWithoutInitializingProxy(thisobject)].ContainsKey(id))
                                        {
                                            //Add object to reference dictionary
                                            referenceDictionary[NHibernateProxyHelper.GetClassWithoutInitializingProxy(thisobject)].Add(id, thisobject);
                                        }
                                    }


                                    //Correct possibleParentRef
                                    if (tempId != Guid.Empty)
                                    {
                                        lock (PossibleParentReferences)
                                        {
                                            foreach (Type parentType in PossibleParentReferences.Keys)
                                            {
                                                foreach (Guid parentId in PossibleParentReferences[parentType].Keys)
                                                {
                                                    if (PossibleParentReferences[parentType][parentId].ContainsKey(thisobject.GetType()))
                                                    {
                                                        if (PossibleParentReferences[parentType][parentId][thisobject.GetType()].Contains(tempId))
                                                        {
                                                            PossibleParentReferences[parentType][parentId][thisobject.GetType()].Add(id);
                                                            PossibleParentReferences[parentType][parentId][thisobject.GetType()].Remove(tempId);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    tempId = id;
                                }
                            }
                            else if (property.PropertyType.IsEnum)
                            {
                                property.SetValue(thisobject, Enum.Parse(property.PropertyType, node.InnerText), null);
                            }
                            else if (property.PropertyType.Name == typeof(IList<object>).Name)
                            {
                                object[] serItemIdAttributes = property.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);

                                foreach (XmlNode chnode in node.ChildNodes)
                                {
                                    if (serItemIdAttributes.Length > 0)
                                    {
                                        tmpdoc = new XmlDocument();
                                        tmpdoc.LoadXml(chnode.OuterXml);
                                        object listobj = property.GetValue(thisobject, null);

                                        Type refClassType = listobj.GetType().GetGenericArguments()[0];
                                        Guid refClassId = new Guid(tmpdoc.GetElementsByTagName("Id")[0].InnerText);

                                        if (DontUseDBorSession)
                                        {
                                            if (DomainObjectLookUp.ContainsKey(refClassType) && DomainObjectLookUp[refClassType].ContainsKey(refClassId))
                                            {
                                                listobj.GetType().GetMethod("Add").Invoke(listobj, new object[] { DomainObjectLookUp[refClassType][refClassId] });
                                            }
                                            else
                                            {
                                                lock (MissingReferenses)
                                                {
                                                    MissingReferenses.Add(new MissingReference() { MethodOrPropertyInfo = listobj.GetType().GetMethod("Add"), TargetObject = listobj, RefObjectId = refClassId, RefObjectType = refClassType });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            listobj.GetType().GetMethod("Add").Invoke(listobj, new object[] { GetClassInstanceFromReferenceOrDB(refClassId, refClassType) });
                                        }

                                    }
                                    else
                                    {
                                        XmlSerializer serier = new XmlSerializer(property.PropertyType.GetGenericArguments()[0]);
                                        object listobj = property.GetValue(thisobject, null);
                                        object childObj = serier.Deserialize(XmlReader.Create(new System.IO.StringReader(chnode.OuterXml)));
                                        listobj.GetType().GetMethod("Add").Invoke(listobj, new object[] { childObj });
                                    }
                                }
                            }
                            else
                            {
                                if (node.ChildNodes.Count > 0)
                                {
                                    object[] serItemIdAttributes = property.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);

                                    if (serItemIdAttributes.Length > 0 || (DontUseDBorSession & skipMaps & property.PropertyType == typeof(Domain.PropertyMap)))
                                    {
                                        tmpdoc = new XmlDocument();
                                        tmpdoc.LoadXml(node.FirstChild.OuterXml);

                                        Type refClassType = property.PropertyType;
                                        Guid refClassId = new Guid(tmpdoc.GetElementsByTagName("Id")[0].InnerText);

                                        if (DontUseDBorSession)
                                        {
                                            if (DomainObjectLookUp.ContainsKey(refClassType) && DomainObjectLookUp[refClassType].ContainsKey(refClassId))
                                            {
                                                property.SetValue(thisobject, DomainObjectLookUp[refClassType][refClassId], null);
                                            }
                                            else
                                            {
                                                lock (MissingReferenses)
                                                {
                                                    MissingReferenses.Add(new MissingReference() { MethodOrPropertyInfo = property, TargetObject = thisobject, RefObjectId = refClassId, RefObjectType = refClassType });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            property.SetValue(thisobject, GetClassInstanceFromReferenceOrDB(refClassId, refClassType), null);
                                        }


                                        //Add parent reference
                                        //if (typeof(IVersionControlled).IsAssignableFrom(thisobject.GetType()))
                                        //{
                                            if (typeof(IVersionControlled).IsAssignableFrom(property.PropertyType))
                                            {
                                                if (property.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == thisobject.GetType()).Count() > 0)
                                                {
                                                    lock (PossibleParentReferences)
                                                    {
                                                        if (!PossibleParentReferences.ContainsKey(refClassType))
                                                        {
                                                            PossibleParentReferences.Add(refClassType, new Dictionary<Guid, Dictionary<Type, List<Guid>>>());
                                                        }
                                                        if (!PossibleParentReferences[refClassType].ContainsKey(refClassId))
                                                        {
                                                            PossibleParentReferences[refClassType].Add(refClassId, new Dictionary<Type, List<Guid>>());
                                                        }
                                                        if (!PossibleParentReferences[refClassType][refClassId].ContainsKey(thisobject.GetType()))
                                                        {
                                                            PossibleParentReferences[refClassType][refClassId].Add(thisobject.GetType(), new List<Guid>());
                                                        }

                                                        if (tempId == Guid.Empty)
                                                        {
                                                            tempId = Guid.NewGuid();
                                                        }

                                                        if (!PossibleParentReferences[refClassType][refClassId][thisobject.GetType()].Contains(tempId))
                                                        {
                                                            PossibleParentReferences[refClassType][refClassId][thisobject.GetType()].Add(tempId);
                                                        }
                                                    }
                                                }
                                            }
                                        //}
                                    }
                                    else
                                    {
                                        XmlSerializer serier = new XmlSerializer(property.PropertyType);
                                        object childObj = serier.Deserialize(XmlReader.Create(new System.IO.StringReader(node.FirstChild.OuterXml)));
                                        property.SetValue(thisobject, childObj, null);
                                    }
                                }
                            }
                        }
                    }
                    else if (node.Attributes.GetNamedItem("uxComponentProperty") != null)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            XmlNode chnode = node.ChildNodes[i];
                            if (chnode.Name == "PropertyMap")
                            {
                                XmlSerializer serier = new XmlSerializer(typeof(Domain.PropertyMap));
                                object childObj = serier.Deserialize(XmlReader.Create(new System.IO.StringReader(chnode.OuterXml)));

                                //if (!DontUseDBorSession)
                                //{
                                    if (((IDomainObject)childObj).Id != Guid.Empty)
                                    {
                                        lock (VisualComponentRefObjects)
                                        {
                                            if (!VisualComponentRefObjects.Contains(childObj))
                                            {
                                                VisualComponentRefObjects.Add((IDomainObject)childObj);
                                            }
                                        }

                                    }
                                //}
                            }
                        }
                    }
                }
            }

            if (!DontUseDBorSession)
            {
                if (rootObj == thisobject)
                {
                    rootObj = null;
                }
            }
        }

        private struct UXCompInfo
        {
            public bool Empty;
            public string TypeName;
            public string Id;
            public string Name;
        }

        [Transaction(ReadOnly = false)]
        public static void WriteXML(Object thisobject, XmlWriter writer)
        {
            XmlSerializer SubSerializer;
            Dictionary<UXCompInfo, List<Guid>> UXComponentMapsLookUp = new Dictionary<UXCompInfo, List<Guid>>();

            writer.WriteAttributeString("typeName", thisobject.GetType().FullName);

            PropertyInfo[] properties = thisobject.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Object[] ignoreAttributes = property.GetCustomAttributes(typeof(DomainXmlIgnoreAttribute), true);

                if (ignoreAttributes.Length == 0)
                {
                    if (property.PropertyType.IsPrimitive || property.PropertyType.IsEnum || property.PropertyType.Name == "String" || property.PropertyType.Name == "Guid" || property.PropertyType.Name == typeof(System.Nullable<int>).Name)
                    {
                        writer.WriteElementString(property.Name, property.GetValue(thisobject, null) == null ? null : property.GetValue(thisobject, null).ToString());

                    }
                    else if (property.PropertyType.Name == typeof(IList<object>).Name)
                    {
                        writer.WriteStartElement(property.Name);

                        object[] serItemIdAttributes = property.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);


                        foreach (object subObj in ((System.Collections.ICollection)property.GetValue(thisobject, null)))
                        {
                            if (serItemIdAttributes.Length > 0)
                            {
                                if (typeof(IVersionControlled).IsAssignableFrom(property.PropertyType.GetGenericArguments()[0]))
                                {
                                    if (((IVersionControlled)subObj).State == VersionControlledObjectStat.New)
                                    {
                                        continue;
                                    }
                                }

                                writer.WriteStartElement(property.PropertyType.GetGenericArguments()[0].Name);
                                writer.WriteAttributeString("typeName", property.PropertyType.GetGenericArguments()[0].FullName);
                                writer.WriteElementString("Id", subObj.GetType().GetProperty("Id") == null ? Guid.Empty.ToString() : subObj.GetType().GetProperty("Id").GetValue(subObj, null).ToString());
                                writer.WriteEndElement();
                            }
                            else
                            {
                                SubSerializer = new XmlSerializer(property.PropertyType.GetGenericArguments()[0]);
                                SubSerializer.Serialize(writer, subObj);
                            }
                        }


                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteStartElement(property.Name);

                        object[] serItemIdAttributes = property.GetCustomAttributes(typeof(DomainXmlByIdAttribute), true);

                        object subObj = property.GetValue(thisobject, null);

                        if (subObj != null)
                        {
                            if (serItemIdAttributes.Length > 0)
                            {
                                writer.WriteStartElement(property.PropertyType.Name);
                                writer.WriteAttributeString("namespace", property.PropertyType.Namespace);
                                writer.WriteElementString("Id", subObj.GetType().GetProperty("Id") == null ? Guid.Empty.ToString() : subObj.GetType().GetProperty("Id").GetValue(subObj, null).ToString());
                                writer.WriteEndElement();
                            }
                            else
                            {
                                SubSerializer = new XmlSerializer(property.PropertyType);
                                SubSerializer.Serialize(writer, subObj);
                            }
                        }
                        writer.WriteEndElement();

                        //Take care of PropertyMaps in XML
                        if (subObj != null)
                        {
                            if (typeof(Domain.VisualModel.UXContainer).IsAssignableFrom(property.PropertyType))
                            {
                                UXComponentMapsLookUp.Clear();
                                FindPropertyMapsInUXComponent((Domain.VisualModel.UXContainer)subObj, UXComponentMapsLookUp);
                                foreach (UXCompInfo compInfo in UXComponentMapsLookUp.Keys)
                                {
                                    writer.WriteStartElement("UXComponentProperties");
                                    writer.WriteAttributeString("uxComponentProperty", "True");
                                    writer.WriteAttributeString("componentType", compInfo.TypeName);
                                    writer.WriteAttributeString("componentId", compInfo.Id);
                                    writer.WriteAttributeString("componentName", compInfo.Name);

                                    SubSerializer = new XmlSerializer(typeof(Domain.PropertyMap));
                                    foreach (Guid mapId in UXComponentMapsLookUp[compInfo])
                                    {
                                        subObj = GetClassInstance(mapId, typeof(Domain.PropertyMap), false);
                                        SubSerializer.Serialize(writer, subObj);
                                    }

                                    writer.WriteEndElement();
                                }
                            }
                        }

                    }
                }
            }
        }

        private static void FindPropertyMapsInUXComponent(Domain.VisualModel.UXComponent uxComp, Dictionary<UXCompInfo, List<Guid>> UXComponentMapsLookUp)
        {
            if (uxComp == null) { return; }
            Guid Id;
            UXCompInfo compInfo = new UXCompInfo();
            compInfo.Empty = true;
            compInfo.TypeName = string.Empty;
            compInfo.Id = string.Empty;
            compInfo.Name = string.Empty;

            foreach (PropertyInfo pi in uxComp.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(Domain.PropertyMap))
                {
                    if (compInfo.Empty)
                    {
                        compInfo.Empty = false;
                        compInfo.TypeName = uxComp.GetType().Name;
                        //compInfo.Id = uxComp.ViewComponentId == Guid.Empty ? "" : uxComp.ViewComponentId.ToString();
                        compInfo.Name = string.IsNullOrEmpty(uxComp.Name) ? "" : uxComp.Name;

                        UXComponentMapsLookUp.Add(compInfo, new List<Guid>());
                    }

                    Id = ((Guid)uxComp.GetType().GetProperty(pi.Name + "Id").GetValue(uxComp, null));

                    UXComponentMapsLookUp[compInfo].Add(Id);
                }
                else if (pi.PropertyType == typeof(Domain.VisualModel.UXContainer) && pi.Name != "Parent")
                {
                    FindPropertyMapsInUXComponent((Domain.VisualModel.UXContainer)pi.GetValue(uxComp, null), UXComponentMapsLookUp);
                }
            }

            if (uxComp is Domain.VisualModel.UXContainer)
            {
                if (((Domain.VisualModel.UXContainer)uxComp).Children != null)
                {
                    foreach (Domain.VisualModel.UXComponent childComp in ((Domain.VisualModel.UXContainer)uxComp).Children)
                    {
                        FindPropertyMapsInUXComponent(childComp, UXComponentMapsLookUp);
                    }
                }
            }
        }

        private static object GetClassInstanceFromReferenceOrDB(Guid Id, Type classType)
        {
            Spring.Context.IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            NHibernate.ISessionFactory sessionFactory = ((NHibernate.ISessionFactory)ctx["SessionFactory"]);
            NHibernate.ISession session = Spring.Data.NHibernate.SessionFactoryUtils.GetSession(sessionFactory, false);



            object sessionObj = null;

            if (typeof(IVersionControlled).IsAssignableFrom(classType))
            {
                sessionObj = session.Get(classType, Id);
            }



            if (referenceDictionary.ContainsKey(classType))
            {
                if (referenceDictionary[classType].ContainsKey(Id))
                {
                    if (sessionObj != null)
                    {
                        if (sessionObj != referenceDictionary[classType][Id])
                        {
                            return sessionObj;
                        }
                    }

                    return referenceDictionary[classType][Id];
                }
            }


            object objtoreturn = GetClassInstance(Id, classType, false);

            if (objtoreturn == null)
            {
                IDomainObject newobj = (IDomainObject)System.Reflection.Assembly.GetAssembly(classType).CreateInstance(classType.FullName);
                newobj.Id = Id;
                //newobj.IsTransient = false;
                return newobj;
            }

            return objtoreturn;
        }

        public static object GetClassInstance(Guid Id, Type classType, bool recursive)
        {
            if (classType == typeof(Domain.Action))
            {
                return ((Dao.IActionDao)ContextRegistry.GetContext()["ActionDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Application))
            {
                return ((Dao.IApplicationDao)ContextRegistry.GetContext()["ApplicationDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.BusinessEntity))
            {
                return ((Dao.IBusinessEntityDao)ContextRegistry.GetContext()["BusinessEntityDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.CustomDialog))
            {
                return ((Dao.ICustomDialogDao)ContextRegistry.GetContext()["CustomDialogDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.DataSource))
            {
                return ((Dao.IDataSourceDao)ContextRegistry.GetContext()["DataSourceDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.DeploymentGroup))
            {
                return ((Dao.IDeploymentGroupDao)ContextRegistry.GetContext()["DeploymentGroupDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Dialog))
            {
                return ((Dao.IDialogDao)ContextRegistry.GetContext()["DialogDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Hint))
            {
                return ((Dao.IHintDao)ContextRegistry.GetContext()["HintDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.HintCollection))
            {
                return ((Dao.IHintCollectionDao)ContextRegistry.GetContext()["HintCollectionDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Issue))
            {
                return ((Dao.IIssueDao)ContextRegistry.GetContext()["IssueDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.MappedProperty))
            {
                return ((Dao.IMappedPropertyDao)ContextRegistry.GetContext()["MappedPropertyDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Menu))
            {
                return ((Dao.IMenuDao)ContextRegistry.GetContext()["MenuDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.MenuItem))
            {
                return ((Dao.IMenuItemDao)ContextRegistry.GetContext()["MenuItemDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Module))
            {
                return ((Dao.IModuleDao)ContextRegistry.GetContext()["ModuleDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Package))
            {
                return ((Dao.IPackageDao)ContextRegistry.GetContext()["PackageDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.ProcedureProperty))
            {
                return ((Dao.IStoredProcedurePropertyDao)ContextRegistry.GetContext()["StoredProcedurePropertyDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Property))
            {
                return ((Dao.IPropertyDao)ContextRegistry.GetContext()["PropertyDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.PropertyCaption))
            {
                return ((Dao.IPropertyCaptionDao)ContextRegistry.GetContext()["PropertyCaptionDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.PropertyCode))
            {
                return ((Dao.IPropertyCodeDao)ContextRegistry.GetContext()["PropertyCodeDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.PropertyMap))
            {
                return ((Dao.IPropertyMapDao)ContextRegistry.GetContext()["PropertyMapDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.PropertyStorageInfo))
            {
                return ((Dao.IPropertyStorageInfoDao)ContextRegistry.GetContext()["PropertyStorageInfoDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Query))
            {
                return ((Dao.IQueryDao)ContextRegistry.GetContext()["QueryDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.QueryProperty))
            {
                return ((Dao.IQueryPropertyDao)ContextRegistry.GetContext()["QueryPropertyDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Report))
            {
                return ((Dao.IReportDao)ContextRegistry.GetContext()["ReportDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.ReportQuery))
            {
                return ((Dao.IReportQueryDao)ContextRegistry.GetContext()["ReportQueryDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Schema))
            {
                return ((Dao.ISchemaDao)ContextRegistry.GetContext()["SchemaDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Service))
            {
                return ((Dao.IServiceDao)ContextRegistry.GetContext()["ServiceDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.ServiceMethod))
            {
                return ((Dao.IServiceMethodDao)ContextRegistry.GetContext()["ServiceMethodDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.StoredProcedure))
            {
                return ((Dao.IStoredProcedureDao)ContextRegistry.GetContext()["StoredProcedureDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.UXAction))
            {
                return ((Dao.IUXActionDao)ContextRegistry.GetContext()["UXActionDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.UXSession))
            {
                return ((Dao.IUXSessionDao)ContextRegistry.GetContext()["UXSessionDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.UXSessionProperty))
            {
                return ((Dao.IUXSessionPropertyDao)ContextRegistry.GetContext()["UXSessionPropertyDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.ViewAction))
            {
                return ((Dao.IViewActionDao)ContextRegistry.GetContext()["ViewActionDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.View))
            {
                return ((Dao.IViewDao)ContextRegistry.GetContext()["ViewDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.ViewNode))
            {
                return ((Dao.IViewNodeDao)ContextRegistry.GetContext()["ViewNodeDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.Workflow))
            {
                return ((Dao.IWorkflowDao)ContextRegistry.GetContext()["WorkflowDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.WorkflowDialog))
            {
                return ((Dao.IWorkflowDialogDao)ContextRegistry.GetContext()["WorkflowDialogDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.WorkflowServiceMethod))
            {
                return ((Dao.IWorkflowServiceMethodDao)ContextRegistry.GetContext()["WorkflowServiceMethodDao"]).FindById(Id);
            }
            else if (classType == typeof(Domain.WorkflowSubworkflow))
            {
                return ((Dao.IWorkflowSubworkflowDao)ContextRegistry.GetContext()["WorkflowSubworkflowDao"]).FindById(Id);
            }
            else
            {
                if (!recursive)
                {
                    return GetClassInstance(Id, classType.BaseType, true);
                }
                else
                {
                    return null;
                }
            }
        }

        public static object SaveClassInstance(object classInstance, Type classType, bool recursive)
        {
            if (classType == typeof(Domain.Action))
            {
                return ((Dao.IActionDao)ContextRegistry.GetContext()["ActionDao"]).SaveOrUpdate((Domain.Action)classInstance);
            }
            else if (classType == typeof(Domain.Application))
            {
                return ((Dao.IApplicationDao)ContextRegistry.GetContext()["ApplicationDao"]).SaveOrUpdate((Domain.Application)classInstance);
            }
            else if (classType == typeof(Domain.BusinessEntity))
            {
                return ((Dao.IBusinessEntityDao)ContextRegistry.GetContext()["BusinessEntityDao"]).SaveOrUpdate((Domain.BusinessEntity)classInstance);
            }
            else if (classType == typeof(Domain.CustomDialog))
            {
                return ((Dao.ICustomDialogDao)ContextRegistry.GetContext()["CustomDialogDao"]).SaveOrUpdate((Domain.CustomDialog)classInstance);
            }
            else if (classType == typeof(Domain.DataSource))
            {
                return ((Dao.IDataSourceDao)ContextRegistry.GetContext()["DataSourceDao"]).SaveOrUpdate((Domain.DataSource)classInstance);
            }
            else if (classType == typeof(Domain.DeploymentGroup))
            {
                return ((Dao.IDeploymentGroupDao)ContextRegistry.GetContext()["DeploymentGroupDao"]).SaveOrUpdate((Domain.DeploymentGroup)classInstance);
            }
            else if (classType == typeof(Domain.Dialog))
            {
                return ((Dao.IDialogDao)ContextRegistry.GetContext()["DialogDao"]).SaveOrUpdate((Domain.Dialog)classInstance);
            }
            else if (classType == typeof(Domain.Hint))
            {
                return ((Dao.IHintDao)ContextRegistry.GetContext()["HintDao"]).SaveOrUpdate((Domain.Hint)classInstance);
            }
            else if (classType == typeof(Domain.HintCollection))
            {
                return ((Dao.IHintCollectionDao)ContextRegistry.GetContext()["HintCollectionDao"]).SaveOrUpdate((Domain.HintCollection)classInstance);
            }
            else if (classType == typeof(Domain.Issue))
            {
                return ((Dao.IIssueDao)ContextRegistry.GetContext()["IssueDao"]).SaveOrUpdate((Domain.Issue)classInstance);
            }
            else if (classType == typeof(Domain.MappedProperty))
            {
                return ((Dao.IMappedPropertyDao)ContextRegistry.GetContext()["MappedPropertyDao"]).SaveOrUpdate((Domain.MappedProperty)classInstance);
            }
            else if (classType == typeof(Domain.Menu))
            {
                return ((Dao.IMenuDao)ContextRegistry.GetContext()["MenuDao"]).SaveOrUpdate((Domain.Menu)classInstance);
            }
            else if (classType == typeof(Domain.MenuItem))
            {
                return ((Dao.IMenuItemDao)ContextRegistry.GetContext()["MenuItemDao"]).SaveOrUpdate((Domain.MenuItem)classInstance);
            }
            else if (classType == typeof(Domain.Module))
            {
                return ((Dao.IModuleDao)ContextRegistry.GetContext()["ModuleDao"]).SaveOrUpdate((Domain.Module)classInstance);
            }
            else if (classType == typeof(Domain.Package))
            {
                return ((Dao.IPackageDao)ContextRegistry.GetContext()["PackageDao"]).SaveOrUpdate((Domain.Package)classInstance);
            }
            else if (classType == typeof(Domain.Property))
            {
                return ((Dao.IPropertyDao)ContextRegistry.GetContext()["PropertyDao"]).SaveOrUpdate((Domain.Property)classInstance);
            }
            else if (classType == typeof(Domain.PropertyMap))
            {
                return ((Dao.IPropertyMapDao)ContextRegistry.GetContext()["PropertyMapDao"]).SaveOrUpdate((Domain.PropertyMap)classInstance);
            }
            else if (classType == typeof(Domain.PropertyStorageInfo))
            {
                return ((Dao.IPropertyStorageInfoDao)ContextRegistry.GetContext()["PropertyStorageInfoDao"]).SaveOrUpdate((Domain.PropertyStorageInfo)classInstance);
            }
            else if (classType == typeof(Domain.Query))
            {
                return ((Dao.IQueryDao)ContextRegistry.GetContext()["QueryDao"]).SaveOrUpdate((Domain.Query)classInstance);
            }
            else if (classType == typeof(Domain.QueryProperty))
            {
                return ((Dao.IQueryPropertyDao)ContextRegistry.GetContext()["QueryPropertyDao"]).SaveOrUpdate((Domain.QueryProperty)classInstance);
            }
            else if (classType == typeof(Domain.Report))
            {
                return ((Dao.IReportDao)ContextRegistry.GetContext()["ReportDao"]).SaveOrUpdate((Domain.Report)classInstance);
            }
            else if (classType == typeof(Domain.ReportQuery))
            {
                return ((Dao.IReportQueryDao)ContextRegistry.GetContext()["ReportQueryDao"]).SaveOrUpdate((Domain.ReportQuery)classInstance);
            }
            else if (classType == typeof(Domain.Schema))
            {
                return ((Dao.ISchemaDao)ContextRegistry.GetContext()["SchemaDao"]).SaveOrUpdate((Domain.Schema)classInstance);
            }
            else if (classType == typeof(Domain.Service))
            {
                return ((Dao.IServiceDao)ContextRegistry.GetContext()["ServiceDao"]).SaveOrUpdate((Domain.Service)classInstance);
            }
            else if (classType == typeof(Domain.ServiceMethod))
            {
                return ((Dao.IServiceMethodDao)ContextRegistry.GetContext()["ServiceMethodDao"]).SaveOrUpdate((Domain.ServiceMethod)classInstance);
            }
            else if (classType == typeof(Domain.StoredProcedure))
            {
                return ((Dao.IStoredProcedureDao)ContextRegistry.GetContext()["StoredProcedureDao"]).SaveOrUpdate((Domain.StoredProcedure)classInstance);
            }
            else if (classType == typeof(Domain.ProcedureProperty))
            {
                return ((Dao.IStoredProcedurePropertyDao)ContextRegistry.GetContext()["StoredProcedurePropertyDao"]).SaveOrUpdate((Domain.ProcedureProperty)classInstance);
            }
            else if (classType == typeof(Domain.UXAction))
            {
                return ((Dao.IUXActionDao)ContextRegistry.GetContext()["UXActionDao"]).SaveOrUpdate((Domain.UXAction)classInstance);
            }
            else if (classType == typeof(Domain.UXSession))
            {
                return ((Dao.IUXSessionDao)ContextRegistry.GetContext()["UXSessionDao"]).SaveOrUpdate((Domain.UXSession)classInstance);
            }
            else if (classType == typeof(Domain.ViewAction))
            {
                return ((Dao.IViewActionDao)ContextRegistry.GetContext()["ViewActionDao"]).SaveOrUpdate((Domain.ViewAction)classInstance);
            }
            else if (classType == typeof(Domain.View))
            {
                return ((Dao.IViewDao)ContextRegistry.GetContext()["ViewDao"]).SaveOrUpdate((Domain.View)classInstance);
            }
            else if (classType == typeof(Domain.ViewNode))
            {
                return ((Dao.IViewNodeDao)ContextRegistry.GetContext()["ViewNodeDao"]).SaveOrUpdate((Domain.ViewNode)classInstance);
            }
            else if (classType == typeof(Domain.Workflow))
            {
                return ((Dao.IWorkflowDao)ContextRegistry.GetContext()["WorkflowDao"]).SaveOrUpdate((Domain.Workflow)classInstance);
            }
            //else if (classType == typeof(Domain.WorkflowDialog))
            //{
            //    return ((Dao.IWorkflowDialogDao)ContextRegistry.GetContext()["WorkflowDialogDao"]).SaveOrUpdate((Domain.WorkflowDialog)classInstance);
            //}
            //else if (classType == typeof(Domain.WorkflowSubworkflow))
            //{
            //    return ((Dao.IWorkflowSubworkflowDao)ContextRegistry.GetContext()["WorkflowSubworkflowDao"]).SaveOrUpdate((Domain.WorkflowSubworkflow)classInstance);
            //}
            else
            {
                if (!recursive)
                {
                    return SaveClassInstance(classInstance, classType.BaseType, true);
                }
                else
                {
                    return null;
                }
            }
        }

        public static object MergeSaveClassInstance(object classInstance, Type classType, bool recursive)
        {
            if (classType == typeof(Domain.Action))
            {
                return ((Dao.IActionDao)ContextRegistry.GetContext()["ActionDao"]).SaveOrUpdateMerge((Domain.Action)classInstance);
            }
            else if (classType == typeof(Domain.Application))
            {
                return ((Dao.IApplicationDao)ContextRegistry.GetContext()["ApplicationDao"]).SaveOrUpdateMerge((Domain.Application)classInstance);
            }
            else if (classType == typeof(Domain.BusinessEntity))
            {
                return ((Dao.IBusinessEntityDao)ContextRegistry.GetContext()["BusinessEntityDao"]).SaveOrUpdateMerge((Domain.BusinessEntity)classInstance);
            }
            else if (classType == typeof(Domain.CustomDialog))
            {
                return ((Dao.ICustomDialogDao)ContextRegistry.GetContext()["CustomDialogDao"]).SaveOrUpdateMerge((Domain.CustomDialog)classInstance);
            }
            else if (classType == typeof(Domain.DataSource))
            {
                return ((Dao.IDataSourceDao)ContextRegistry.GetContext()["DataSourceDao"]).SaveOrUpdateMerge((Domain.DataSource)classInstance);
            }
            else if (classType == typeof(Domain.DeploymentGroup))
            {
                return ((Dao.IDeploymentGroupDao)ContextRegistry.GetContext()["DeploymentGroupDao"]).SaveOrUpdateMerge((Domain.DeploymentGroup)classInstance);
            }
            else if (classType == typeof(Domain.Dialog))
            {
                return ((Dao.IDialogDao)ContextRegistry.GetContext()["DialogDao"]).SaveOrUpdateMerge((Domain.Dialog)classInstance);
            }
            else if (classType == typeof(Domain.Hint))
            {
                return ((Dao.IHintDao)ContextRegistry.GetContext()["HintDao"]).SaveOrUpdateMerge((Domain.Hint)classInstance);
            }
            else if (classType == typeof(Domain.HintCollection))
            {
                return ((Dao.IHintCollectionDao)ContextRegistry.GetContext()["HintCollectionDao"]).SaveOrUpdateMerge((Domain.HintCollection)classInstance);
            }
            //else if (classType == typeof(Domain.Issue))
            //{
            //    return ((Dao.IIssueDao)ContextRegistry.GetContext()["IssueDao"]).SaveOrUpdateMerge((Domain.Issue)classInstance);
            //}
            else if (classType == typeof(Domain.MappedProperty))
            {
                return ((Dao.IMappedPropertyDao)ContextRegistry.GetContext()["MappedPropertyDao"]).SaveOrUpdateMerge((Domain.MappedProperty)classInstance);
            }
            else if (classType == typeof(Domain.Menu))
            {
                return ((Dao.IMenuDao)ContextRegistry.GetContext()["MenuDao"]).SaveOrUpdateMerge((Domain.Menu)classInstance);
            }
            else if (classType == typeof(Domain.MenuItem))
            {
                return ((Dao.IMenuItemDao)ContextRegistry.GetContext()["MenuItemDao"]).SaveOrUpdateMerge((Domain.MenuItem)classInstance);
            }
            else if (classType == typeof(Domain.Module))
            {
                return ((Dao.IModuleDao)ContextRegistry.GetContext()["ModuleDao"]).SaveOrUpdateMerge((Domain.Module)classInstance);
            }
            else if (classType == typeof(Domain.Package))
            {
                return ((Dao.IPackageDao)ContextRegistry.GetContext()["PackageDao"]).SaveOrUpdateMerge((Domain.Package)classInstance);
            }
            else if (classType == typeof(Domain.Property))
            {
                return ((Dao.IPropertyDao)ContextRegistry.GetContext()["PropertyDao"]).SaveOrUpdateMerge((Domain.Property)classInstance);
            }
            else if (classType == typeof(Domain.PropertyMap))
            {
                return ((Dao.IPropertyMapDao)ContextRegistry.GetContext()["PropertyMapDao"]).SaveOrUpdateMerge((Domain.PropertyMap)classInstance);
            }
            else if (classType == typeof(Domain.PropertyStorageInfo))
            {
                return ((Dao.IPropertyStorageInfoDao)ContextRegistry.GetContext()["PropertyStorageInfoDao"]).SaveOrUpdateMerge((Domain.PropertyStorageInfo)classInstance);
            }
            else if (classType == typeof(Domain.Query))
            {
                return ((Dao.IQueryDao)ContextRegistry.GetContext()["QueryDao"]).SaveOrUpdateMerge((Domain.Query)classInstance);
            }
            else if (classType == typeof(Domain.QueryProperty))
            {
                return ((Dao.IQueryPropertyDao)ContextRegistry.GetContext()["QueryPropertyDao"]).SaveOrUpdateMerge((Domain.QueryProperty)classInstance);
            }
            else if (classType == typeof(Domain.Report))
            {
                return ((Dao.IReportDao)ContextRegistry.GetContext()["ReportDao"]).SaveOrUpdateMerge((Domain.Report)classInstance);
            }
            else if (classType == typeof(Domain.ReportQuery))
            {
                return ((Dao.IReportQueryDao)ContextRegistry.GetContext()["ReportQueryDao"]).SaveOrUpdateMerge((Domain.ReportQuery)classInstance);
            }
            else if (classType == typeof(Domain.Schema))
            {
                return ((Dao.ISchemaDao)ContextRegistry.GetContext()["SchemaDao"]).SaveOrUpdateMerge((Domain.Schema)classInstance);
            }
            else if (classType == typeof(Domain.Service))
            {
                return ((Dao.IServiceDao)ContextRegistry.GetContext()["ServiceDao"]).SaveOrUpdateMerge((Domain.Service)classInstance);
            }
            else if (classType == typeof(Domain.ServiceMethod))
            {
                return ((Dao.IServiceMethodDao)ContextRegistry.GetContext()["ServiceMethodDao"]).SaveOrUpdateMerge((Domain.ServiceMethod)classInstance);
            }
            else if (classType == typeof(Domain.StoredProcedure))
            {
                return ((Dao.IStoredProcedureDao)ContextRegistry.GetContext()["StoredProcedureDao"]).SaveOrUpdateMerge((Domain.StoredProcedure)classInstance);
            }
            else if (classType == typeof(Domain.ProcedureProperty))
            {
                return ((Dao.IStoredProcedurePropertyDao)ContextRegistry.GetContext()["StoredProcedurePropertyDao"]).SaveOrUpdateMerge((Domain.ProcedureProperty)classInstance);
            }
            else if (classType == typeof(Domain.UXAction))
            {
                return ((Dao.IUXActionDao)ContextRegistry.GetContext()["UXActionDao"]).SaveOrUpdateMerge((Domain.UXAction)classInstance);
            }
            else if (classType == typeof(Domain.UXSession))
            {
                return ((Dao.IUXSessionDao)ContextRegistry.GetContext()["UXSessionDao"]).SaveOrUpdateMerge((Domain.UXSession)classInstance);
            }
            else if (classType == typeof(Domain.ViewAction))
            {
                return ((Dao.IViewActionDao)ContextRegistry.GetContext()["ViewActionDao"]).SaveOrUpdateMerge((Domain.ViewAction)classInstance);
            }
            else if (classType == typeof(Domain.View))
            {
                return ((Dao.IViewDao)ContextRegistry.GetContext()["ViewDao"]).SaveOrUpdateMerge((Domain.View)classInstance);
            }
            else if (classType == typeof(Domain.ViewNode))
            {
                return ((Dao.IViewNodeDao)ContextRegistry.GetContext()["ViewNodeDao"]).SaveOrUpdateMerge((Domain.ViewNode)classInstance);
            }
            else if (classType == typeof(Domain.Workflow))
            {
                return ((Dao.IWorkflowDao)ContextRegistry.GetContext()["WorkflowDao"]).SaveOrUpdateMerge((Domain.Workflow)classInstance);
            }
            else if (classType == typeof(Domain.WorkflowDialog))
            {
                return ((Dao.IWorkflowDialogDao)ContextRegistry.GetContext()["WorkflowDialogDao"]).SaveOrUpdateMerge((Domain.WorkflowDialog)classInstance);
            }
            else if (classType == typeof(Domain.WorkflowSubworkflow))
            {
                return ((Dao.IWorkflowSubworkflowDao)ContextRegistry.GetContext()["WorkflowSubworkflowDao"]).SaveOrUpdateMerge((Domain.WorkflowSubworkflow)classInstance);
            }
            else
            {
                if (!recursive)
                {
                    return MergeSaveClassInstance(classInstance, classType.BaseType, true);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
