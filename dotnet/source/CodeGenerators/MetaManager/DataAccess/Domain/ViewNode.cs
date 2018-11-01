using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Workflow.ComponentModel.Serialization;
using System.IO;
using System.Xml;
using System.Workflow.Activities.Rules;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using NHibernate;
using System.Xml.Serialization;
using System.ComponentModel;


namespace Cdc.MetaManager.DataAccess.Domain
{

    [ClassStorageHint(TableName = "ViewNode")]
    public class ViewNode : ILocalizable, IXmlSerializable, IDomainObject, IMappedObject, IRuledObject
    {
        public ViewNode()
        {
            this.Visibility = UXVisibility.Visible;
            this.RenderMode = RenderMode.Normal;
            this.UpdatePropagation = UpdatePropagation.None;
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual RenderMode RenderMode { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual UpdatePropagation UpdatePropagation { get; set; }

        private IList<ViewNode> childrens;

        [Browsable(false)]
        [CollectionStorageHint(Column = "ParentId", Lazy = true, Inverse = true, Cascade = CascadeOperation.All)]
        public virtual IList<ViewNode> Children
        {
            get
            {
                if (childrens == null)
                {
                    childrens = new List<ViewNode>();
                }
                return childrens;
            }
            set { childrens = value; }

        }

        [Browsable(false)]
        [PropertyStorageHint(Ignore = true)]
        public virtual object Tag { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ViewId", IsMandatory = true, Lazy = true, ForeignKey = "FK_ViewNode_View", Cascade = CascadeAssociationOperation.None)]
        public virtual View View { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Sequence { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Request, SetTarget = SetTargetChoice.No)]
        [PropertyStorageHint(Column = "ViewNodeMapId", Cascade = CascadeAssociationOperation.All, IsMandatory = false, ForeignKey = "FK_ViewNode_PropertyMap")]
        public virtual PropertyMap ViewMap { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ParentId", IsMandatory = false, ForeignKey = "FK_ViewNode_ViewNode")]
        public virtual ViewNode Parent { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "DialogId", Lazy = true, IsMandatory = false, ForeignKey = "FK_ViewNode_Dialog")]
        public virtual Dialog Dialog { get; set; }

        [PropertyStorageHint(IsMandatory = false, Length = 50)]
        [Localizable]
        public virtual string Title { get; set; }

        private IList<ViewAction> viewActions;

        [Browsable(false)]
        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<ViewAction> ViewActions
        {
            get
            {
                if (viewActions == null)
                {
                    viewActions = new List<ViewAction>();
                }
                return viewActions;
            }
            set { viewActions = value; }

        }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual UXVisibility Visibility { get; set; }

        private string ruleSetXml;

        [Browsable(false)]
        [PropertyStorageHint(Column = "RuleSetXml", SqlType = "ntext", IsMandatory = false)]
        public virtual string RuleSetXml
        {
            get
            {
                if (ruleSet != null)
                {
                    StringBuilder builder = new StringBuilder();

                    WorkflowMarkupSerializer xml = new WorkflowMarkupSerializer();

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.NewLineOnAttributes = true;
                    settings.Indent = true;
                                        
                    XmlWriter writer = XmlWriter.Create(builder, settings);

                    xml.Serialize(writer, RuleSet);

                    return builder.ToString();
                }
                else
                    return ruleSetXml;
            }
            set
            {
                ruleSetXml = value;
            }
        }

        private RuleSet ruleSet;

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual RuleSet RuleSet
        {
            get
            {
                if ((ruleSet == null) && (!string.IsNullOrEmpty(ruleSetXml)))
                {
                    WorkflowMarkupSerializer xml = new WorkflowMarkupSerializer();
                    StringReader reader = new StringReader(ruleSetXml);
                    ruleSet = xml.Deserialize(XmlReader.Create(reader)) as RuleSet;
                }

                return ruleSet;
            }
            set
            {
                ruleSet = value;

                if (ruleSet == null)
                    ruleSetXml = null;
            }
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
            object[] propertyMapAttributes = this.GetType().GetProperty("ViewMap").GetCustomAttributes(typeof(PropertyMapAttribute), true);
            setTarget = ((PropertyMapAttribute)propertyMapAttributes[0]).SetTarget;

            if (connectedToObject == null)
            {
                return ViewMap == null ? Guid.Empty : ViewMap.Id;
            }
            else if (NHibernate.Proxy.NHibernateProxyHelper.GetClassWithoutInitializingProxy(connectedToObject) == typeof(View))
            {
                if (View.Id == connectedToObject.Id)
                {
                    return ViewMap == null ? Guid.Empty : ViewMap.Id;
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
            setTarget = SetTargetChoice.No;
            return Guid.Empty;
        }

        #endregion
    }
}
