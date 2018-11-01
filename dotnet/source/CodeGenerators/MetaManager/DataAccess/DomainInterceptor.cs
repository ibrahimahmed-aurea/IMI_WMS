using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public class DomainInterceptor : NHibernate.IInterceptor
    {
        public bool GetDataFromConfigurationManagement = false;

        public DomainInterceptor()
        {
        }
        #region IInterceptor Members

        public void AfterTransactionBegin(NHibernate.ITransaction tx)
        {

        }

        public void AfterTransactionCompletion(NHibernate.ITransaction tx)
        {

        }

        public void BeforeTransactionCompletion(NHibernate.ITransaction tx)
        {

        }

        public int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            return null;
        }

        public object GetEntity(string entityName, object id)
        {
            return null;
        }

        public string GetEntityName(object entity)
        {
            return null;
        }

        public object Instantiate(string entityName, NHibernate.EntityMode entityMode, object id)
        {
            return null;
        }

        public bool? IsTransient(object entity)
        {
            if (entity is IDomainObject)
            {
                if (((IDomainObject)entity).IsTransient)
                {
                    if (((IDomainObject)entity).Id == Guid.Empty)
                    {
                        ((IDomainObject)entity).Id = Guid.NewGuid();
                        return true;
                    }
                    else if (DomainXmlSerializationHelper.GetClassInstance(((IDomainObject)entity).Id, entity.GetType(), true) == null)
                    {

                        return true;

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return ((IDomainObject)entity).IsTransient;
                }
            }
            else
            {
                return null;
            }
        }

        public void OnCollectionRecreate(object collection, object key)
        {
        }

        public void OnCollectionRemove(object collection, object key)
        {
        }

        public void OnCollectionUpdate(object collection, object key)
        {
        }

        public void OnDelete(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
        }

        public bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            return true;
        }

        public bool OnLoad(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            if (entity is IDomainObject)
            {
                ((IDomainObject)entity).IsTransient = false;

                if (GetDataFromConfigurationManagement)
                {
                    return GetLatestStateFromConfigurationManagement(entity, state, propertyNames);
                }
            }

            return true;
        }

        public NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            if (GetDataFromConfigurationManagement)
            {
                sql = FilterSQLToAvoidOtherUsersNewObjects(sql);
            }
            return sql;
        }

        public bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            return true;
        }

        public void PostFlush(System.Collections.ICollection entities)
        {
        }

        public void PreFlush(System.Collections.ICollection entities)
        {
        }

        public void SetSession(NHibernate.ISession session)
        {
        }

        #endregion





        #region private methods

        private bool GetLatestStateFromConfigurationManagement(object entity, object[] state, string[] propertyNames)
        {
            bool? appFrontend = false;
            Type classType;
            Domain.Application application;

            classType = NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(entity);
            if (typeof(IVersionControlled).IsAssignableFrom(classType))
            {

                if (((bool)state[propertyNames.ToList().IndexOf("IsLocked")]) && ((string)state[propertyNames.ToList().IndexOf("LockedBy")]) != Environment.UserName)
                {

                    IVersionControlled dbobject = (IVersionControlled)GetObjectFromState(classType, state, propertyNames);
                    dbobject.Id = ((IVersionControlled)entity).Id;


                    application = GetApplicationForDomainObject(dbobject);
                    appFrontend = application.IsFrontend.Value;


                    if (appFrontend == null)
                    {
                        return false;
                    }


                    Spring.Context.IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
                    NHibernate.ISessionFactory sessionFactory = ((NHibernate.ISessionFactory)ctx["SessionFactory"]);
                    NHibernate.ISession session = Spring.Data.NHibernate.SessionFactoryUtils.GetSession(sessionFactory, false);

                    System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
                    string rootPath = appReader.GetValue("RepositoryPath", typeof(System.String)).ToString();
                    string parentPath = System.IO.Path.Combine(rootPath, application.Name + (appFrontend.Value ? "_Frontend" : "_Backend"));
                    string filePath = System.IO.Path.Combine(parentPath, classType.Name);
                    string fileName = ((IVersionControlled)dbobject).RepositoryFileName + ".xml";
                    string fullPath = System.IO.Path.Combine(filePath, fileName);

                    System.IO.FileInfo fi = new System.IO.FileInfo(fullPath);

                    if (fi.Exists)
                    {
                        System.Xml.Serialization.XmlSerializer xmlSerie = new System.Xml.Serialization.XmlSerializer(classType);

                        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new System.IO.StringReader(fi.OpenText().ReadToEnd()));

                        DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects.Clear();

                        object domainObject = xmlSerie.Deserialize(reader);

                        foreach (IDomainObject obj in DataAccess.DomainXmlSerializationHelper.VisualComponentRefObjects)
                        {
                            session.Merge(obj);
                        }

                        ((IVersionControlled)domainObject).IsLocked = true;
                        ((IVersionControlled)domainObject).LockedBy = dbobject.LockedBy;
                        ((IVersionControlled)domainObject).LockedDate = dbobject.LockedDate;
                        ((IDomainObject)domainObject).IsTransient = false;

                        SetStateFromObject(domainObject, classType, state, propertyNames);

                        return true;
                    }
                }
            }

            return true;
        }

        private object GetObjectFromState(Type classType, object[] state, string[] propertyNames)
        {
            object theObject = Activator.CreateInstance(classType);

            List<string> propertyNamesList = propertyNames.ToList();

            foreach (string propertyName in propertyNamesList)
            {
                classType.GetProperty(propertyName).SetValue(theObject, state[propertyNamesList.IndexOf(propertyName)], null);
            }

            return theObject;
        }

        private void SetStateFromObject(object theObject, Type classType, object[] state, string[] propertyNames)
        {
            List<string> propertyNamesList = propertyNames.ToList();

            foreach (string propertyName in propertyNamesList)
            {
                state[propertyNamesList.IndexOf(propertyName)] = classType.GetProperty(propertyName).GetValue(theObject, null);
            }
        }

        private DataAccess.Domain.Application GetApplicationForDomainObject(IDomainObject domainObject)
        {
            Type classType = NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(domainObject);

            if (domainObject is DataAccess.Domain.Application) { return (DataAccess.Domain.Application)domainObject; }

            List<System.Reflection.PropertyInfo> api = classType.GetProperties().Where(p => p.PropertyType == typeof(DataAccess.Domain.Application)).ToList();

            if (api.Count() > 0)
            {
                return ((DataAccess.Domain.Application)api[0].GetValue(domainObject, null));
            }

            IEnumerable<System.Reflection.PropertyInfo> pis = classType.GetProperties().Where(p => typeof(IDomainObject).IsAssignableFrom(p.PropertyType));

            DataAccess.Domain.Application parentApp = null;
            foreach (System.Reflection.PropertyInfo pi in pis)
            {
                if (pi.PropertyType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>) && p.PropertyType.GetGenericArguments()[0] == classType).ToList().Count > 0)
                {
                    IDomainObject parent = (IDomainObject)pi.GetValue(domainObject, null);

                    object refObj = DomainXmlSerializationHelper.GetObjectFromRef(parent.Id, NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(parent));

                    if (refObj != null)
                    {
                        parent = (IDomainObject)refObj;
                    }

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


        private NHibernate.SqlCommand.SqlString FilterSQLToAvoidOtherUsersNewObjects(NHibernate.SqlCommand.SqlString sql)
        {
            string sqlstr = sql.ToString();
            if (sqlstr.Contains(".LockedBy") && sqlstr.Contains(".State"))
            {
                
                int whereindex = sqlstr.IndexOf(" WHERE ", StringComparison.OrdinalIgnoreCase);
                int orderbyIndex = sqlstr.IndexOf(" ORDER BY ", StringComparison.OrdinalIgnoreCase);
                int fromIndex = sqlstr.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase);

                string tableSynonym = string.Empty;
                int substringLength = sqlstr.IndexOfAny(new char[] { ' ', ',' }, sqlstr.IndexOf(' ', fromIndex + 6) + 1) - sqlstr.IndexOf(' ', fromIndex + 6) - 1;
                if (substringLength > 0)
                {
                    tableSynonym = sqlstr.Substring(sqlstr.IndexOf(' ', fromIndex + 6) + 1, substringLength);
                }
                else
                {
                    tableSynonym = sqlstr.Substring(sqlstr.IndexOf(' ', fromIndex + 6) + 1);
                }

                tableSynonym += ".";

                string whereStatment = " ((" + tableSynonym + "LockedBy is null) OR (" + tableSynonym + "LockedBy = '" + Environment.UserName + "') OR (" + tableSynonym + "LockedBy <> '" + Environment.UserName + "' AND " + tableSynonym + "State <> 1))";

                if (whereindex > -1)
                {
                    if (sqlstr.LastIndexOf(".LockedBy") < whereindex && sqlstr.LastIndexOf(".State") < whereindex)
                    {
                        if (orderbyIndex > -1)
                        {
                            sql = sql.Insert(orderbyIndex, " AND" + whereStatment);
                        }
                        else
                        {
                            sql = sql.Append(" AND" + whereStatment);
                        }
                    }
                }
                else
                {
                    if (orderbyIndex > -1)
                    {
                        sql = sql.Insert(orderbyIndex, " WHERE" + whereStatment);
                    }
                    else
                    {
                        sql = sql.Append(" WHERE" + whereStatment);
                    }
                }
            }

            return sql;
        }

        #endregion
    }
}
