using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Workflow.ComponentModel;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.IO;
using System.Xml;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "Workflow")]
    public class Workflow : IMappableUXObject, IXmlSerializable, IVersionControlled, IMappedObject, IRuledObject
    {
        public Workflow()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true)]
        public virtual string Name { get; set; }

        [PropertyStorageHint(Length = 400, IsMandatory = false)]
        public virtual string Description { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ModuleId", IsMandatory = true, ForeignKey = "FK_Workflow_Module")]
        public virtual Module Module { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Request, SetTarget = SetTargetChoice.No)]
        [PropertyStorageHint(Column = "RequestMapId", IsMandatory = false, ForeignKey = "FK_Workflow_ReqPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap RequestMap { get; set; }

        private IList<WorkflowDialog> dialogs;

        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<WorkflowDialog> Dialogs
        {
            get
            {
                if (dialogs == null)
                {
                    dialogs = new List<WorkflowDialog>();
                }
                return dialogs;
            }
            set { dialogs = value; }

        }

        private IList<WorkflowServiceMethod> serviceMethods;

        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<WorkflowServiceMethod> ServiceMethods
        {
            get
            {
                if (serviceMethods == null)
                {
                    serviceMethods = new List<WorkflowServiceMethod>();
                }
                return serviceMethods;
            }
            set { serviceMethods = value; }

        }

        private IList<WorkflowSubworkflow> subworkflows;

        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<WorkflowSubworkflow> Subworkflows
        {
            get
            {
                if (subworkflows == null)
                {
                    subworkflows = new List<WorkflowSubworkflow>();
                }
                return subworkflows;
            }
            set { subworkflows = value; }

        }

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

        private string workflowXoml;

        [PropertyStorageHint(Column = "WorkflowXoml", SqlType = "ntext", IsMandatory = false)]
        public virtual string WorkflowXoml
        {
            get
            {
                if (string.IsNullOrEmpty(workflowXoml))
                {
                    return workflowXoml;
                }
                else
                {
                    System.Xml.XmlDocument tmpDoc = new System.Xml.XmlDocument();
                    tmpDoc.LoadXml(workflowXoml);
                    System.IO.StringWriter strWriter = new System.IO.StringWriter();
                    System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(strWriter);
                    writer.Formatting = System.Xml.Formatting.Indented;
                    tmpDoc.Save(writer);
                    return strWriter.ToString();
                }
            }
            set
            {
                workflowXoml = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }


        #region IMappableUXObject Members

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual UXActionType ActionType
        {
            get
            {
                return UXActionType.Workflow;
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
