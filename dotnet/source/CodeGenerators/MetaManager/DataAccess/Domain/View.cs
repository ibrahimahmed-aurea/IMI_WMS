using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Imi.HbmGenerator.Attributes;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "UXView")]
    public class View : ILocalizable, IXmlSerializable, IVersionControlled, IMappedObject
    {
        public View()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ServiceMethodId", IsMandatory = false, ForeignKey = "FK_View_ServiceMethod")]
        public virtual ServiceMethod ServiceMethod { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool LayoutManualyAdapted { get; set; }

        [PropertyStorageHint(IsMandatory = true, Length = 100)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual ViewType Type { get; set; }

        [PropertyStorageHint(IsMandatory = false, Length = 100)]
        public virtual string CustomDLLName { get; set; }

        [PropertyStorageHint(IsMandatory = false, Length = 100)]
        public virtual string CustomClassName { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_View_Application")]
        public virtual Application Application { get; set; }
           
        [PropertyStorageHint(IsMandatory = true, Length = 100)]
        [Localizable]
        public virtual string Title { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "AlternateViewId", IsMandatory = false, ForeignKey = "FK_View_View")]
        public virtual View AlternateView { get; set; }

        [Browsable(false)]
        [PropertyStorageHint(IsMandatory = false, Length = 100)]
        public virtual string OriginalViewName { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "BusinessEntityId", IsMandatory = true, Cascade = CascadeAssociationOperation.None, ForeignKey = "FK_View_BusinessEntity")]
        public virtual BusinessEntity BusinessEntity { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Request)]
        [PropertyStorageHint(Column = "RequestMapId", IsMandatory = false, ForeignKey = "FK_View_ReqPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap RequestMap { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Response)]
        [PropertyStorageHint(Column = "ResponseMapId", IsMandatory = false, ForeignKey = "FK_View_RespPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap ResponseMap { get; set; }

        private IList<DataSource> dataSources;

        [Browsable(false)]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<DataSource> DataSources
        {
            get
            {
                if (dataSources == null)
                {
                    dataSources = new List<DataSource>();
                }
                return dataSources;
            }
            set { dataSources = value; }

        }
                
        public virtual void FlushVisualTree()
        {
            StringBuilder builder = new StringBuilder();

            XmlSerializer xml = new XmlSerializer(typeof(UXContainer));

            using (XmlWriter writer = XmlWriter.Create(builder))
            {
                xml.Serialize(writer, this.VisualTree);
            }

            visualTreeXml = builder.ToString();
        }

        private string visualTreeXml;

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Column = "VisualTreeXml", SqlType="ntext", IsMandatory = false)]
        public virtual string VisualTreeXml 
        {
            get
            {
                return visualTreeXml;
            }
            set
            {
                visualTree = null;
                visualTreeXml = value;
            }
        }

        private UXContainer visualTree;

        [Browsable(false)]
        [PropertyStorageHint(Ignore = true)]
        public virtual UXContainer VisualTree
        {
            get
            {
                if ((visualTree == null) && (!string.IsNullOrEmpty(visualTreeXml)))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(UXContainer));
                    visualTree = xml.Deserialize(new StringReader(visualTreeXml)) as UXContainer;
                }

                return visualTree;
            }
            set
            {
                visualTree = value;
                FlushVisualTree();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }


        #region ILocalizable Members
        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string MetaId
        {
            get
            {
                return Id.ToString();
            }
        }

        #endregion
                
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
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(ServiceMethod))
            {
                if (ServiceMethod.Id == connectedToObject.Id)
                {
                    return RequestMap == null ? Guid.Empty : RequestMap.Id;
                }
                else
                {
                    return Guid.Empty;
                }
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
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(ServiceMethod))
            {
                if (ServiceMethod.Id == connectedToObject.Id)
                {
                    return ResponseMap == null ? Guid.Empty : ResponseMap.Id;
                }
                else
                {
                    return Guid.Empty;
                }
            }
            else 
            {
                return Guid.Empty;
            }
        }

        #endregion
    }
}
