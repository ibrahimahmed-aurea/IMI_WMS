using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName="Action")]
    public class Action : IXmlSerializable, IVersionControlled, IMappedObject
    {
        public Action()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_Action_BusinessEnt", UniqueKey = "UNQ_Action_BusinessEnt_Name")]
        public virtual BusinessEntity BusinessEntity { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true, UniqueKey = "UNQ_Action_BusinessEnt_Name")]
        public virtual string Name { get; set; }

        private StoredProcedure storedProcedure;

        [PropertyStorageHint(Column = "StoredProcedureId", IsMandatory = false, ForeignKey = "FK_Action_StoredProc", Cascade = CascadeAssociationOperation.All)]
        public virtual StoredProcedure StoredProcedure
        {
            get
            {
                return storedProcedure;
            }
            set
            {
                if (value != null)
                {
                    storedProcedure = value;
                    query = null;
                }
            }
        }

        private Query query;


        [PropertyStorageHint(Column = "QueryId", IsMandatory = false, ForeignKey = "FK_Action_Query", Cascade = CascadeAssociationOperation.All)]
        public virtual Query Query
        {
            get
            {
                return query;
            }
            set
            {
                if (value != null)
                {
                    query = value;
                    storedProcedure = null;
                }
            }
        }
        
        [PropertyMapAttribute(Type=PropertyMapType.Request)]
        [PropertyStorageHint(Column="RequestMapId", IsMandatory = true, ForeignKey = "FK_Action_ReqPropMap", Cascade=CascadeAssociationOperation.All)]
        public virtual PropertyMap RequestMap { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Response)]
        [PropertyStorageHint(Column = "ResponseMapId", IsMandatory = true, ForeignKey = "FK_Action_RespPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap ResponseMap { get; set; }

        /// <summary>
        /// References the SchemaObject being mapped
        /// </summary>
        ///
        [Browsable(false)]
        [Search(SearchType = SearchTypes.FreeText)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual IMappableObject MappedToObject
        {
            get 
            {
                if (storedProcedure != null)
                    return storedProcedure;
                else if (query != null)
                    return query;
                else
                    return null;
            }
            set 
            {
                if (value is StoredProcedure)
                {
                    StoredProcedure = value as StoredProcedure;
                }
                else if (value is Query)
                {
                    Query = value as Query;
                }
                else
                {
                    storedProcedure = null;
                    query = null;
                }
            } 
        }

        

        [PropertyStorageHint(Length = 100, IsMandatory = false)]
        public virtual string RowTrackingId { get; set; }


        private bool? isMultiEnabled;

        /// <summary>
        /// Only applies to stored procedures
        /// </summary>
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool? IsMultiEnabled 
        {
            get
            {
                // if not set
                if (isMultiEnabled == null)
                    return false;

                return isMultiEnabled;
            }

            set
            {
                isMultiEnabled = value;
            }
        }

        private bool? isMessageHandlingEnabled;

        /// <summary>
        /// Only applies to stored procedures
        /// </summary>
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool? IsMessageHandlingEnabled
        {
            get
            {
                // if not set
                if (isMessageHandlingEnabled == null)
                    return false;

                return isMessageHandlingEnabled;
            }

            set
            {
                isMessageHandlingEnabled = value;
            }
        }

        private bool? isRefCursorCommit;

        /// <summary>
        /// If commit should be done or not when the action is connected
        /// to a stored procedure that returns a ref cursor.
        /// </summary>
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool? IsRefCursorCommit
        {
            get
            {
                // if not set
                if (isRefCursorCommit == null)
                    return false;

                return isRefCursorCommit;
            }

            set
            {
                isRefCursorCommit = value;
            }
        }

        private bool? isImportEnabled;

        /// <summary>
        /// Only applies to stored procedures
        /// </summary>
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool? IsImportEnabled
        {
            get
            {
                // if not set
                if (isImportEnabled == null)
                    return false;

                return isImportEnabled;
            }

            set
            {
                isImportEnabled = value;
            }
        }


        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        #region IXmlSerializable Members

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            DataAccess.DomainXmlSerializationHelper.ReadXML(this, reader);
        }

        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            DataAccess.DomainXmlSerializationHelper.WriteXML(this, writer);
        }

        #endregion

        #region IVersionControlled Members

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool IsLocked { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Length = 40, IsMandatory = false)]
        public virtual string LockedBy { get; set; }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = false)]
        public virtual DateTime? LockedDate { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string RepositoryFileName
        {
            get { return this.GetType().Name + "_" + this.Id.ToString(); }
        }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = true)]
        public virtual VersionControlledObjectStat State { get; set; }

        #endregion

        #region IEquatable<IDomainObject> Members

        public virtual bool Equals(IDomainObject other)
        {
            if (other == null) { return false; }

            if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(this) == NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(other))
            {
                if (this.Id == other.Id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region IMappedObject Members

        public virtual Guid GetRequestMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            object[] propertyMapAttributes = this.GetType().GetProperty("RequestMap").GetCustomAttributes(typeof(PropertyMapAttribute), true);
            setTarget = ((PropertyMapAttribute)propertyMapAttributes[0]).SetTarget;
             
            if (connectedToObject == null)
            {
                return RequestMap == null ? Guid.Empty : RequestMap.Id;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(MappedToObject))
            {
                return RequestMap == null ? Guid.Empty : RequestMap.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public virtual Guid GetResponseMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            object[] propertyMapAttributes = this.GetType().GetProperty("ResponseMap").GetCustomAttributes(typeof(PropertyMapAttribute), true);
            setTarget = ((PropertyMapAttribute)propertyMapAttributes[0]).SetTarget;

            if (connectedToObject == null)
            {
                return ResponseMap == null ? Guid.Empty : ResponseMap.Id;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(MappedToObject))
            {
                return ResponseMap == null ? Guid.Empty : ResponseMap.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        #endregion
    }
}
