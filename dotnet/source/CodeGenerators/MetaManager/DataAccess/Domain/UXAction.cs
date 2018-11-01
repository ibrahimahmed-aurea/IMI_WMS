using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "UXAction")]
    public class UXAction : ILocalizable, IXmlSerializable, IVersionControlled, IMappedObject, IRuledObject
    {
        public UXAction()
        {
            IsTransient = true;
        }

        [ReadOnly(true)]
        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string AuthorizationId
        {
            get
            {
                return Id.ToString();
            }
        }

        [PropertyStorageHint(IsMandatory = true, Length = 100, UniqueKey = "UNQ_UXAction_Name_Application")]
        public virtual string Name { get; set; }

        [PropertyStorageHint(IsMandatory = false, Length = 100)]
        [Localizable]
        public virtual string Caption { get; set; }

        [Browsable(false)]
        [PropertyStorageHint(IsMandatory = false, Length = 100)]
        public virtual string OriginalDialog { get; set; }

        [PropertyStorageHint(IsMandatory = false, Length = 35)]
        public virtual string AlarmId { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ApplicationId", IsMandatory = true, ForeignKey = "FK_UXAction_Application", UniqueKey = "UNQ_UXAction_Name_Application")]
        public virtual Application Application { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual IMappableUXObject MappedToObject
        {
            get
            {
                if (dialog != null)
                    return dialog;
                else if (customDialog != null)
                    return customDialog;
                else if (serviceMethod != null)
                    return serviceMethod;
                else if (workflow != null)
                    return workflow;
                else
                    return null;
            }
            set
            {
                if (value is Dialog)
                {
                    Dialog = value as Dialog;
                }
                else if (value is CustomDialog)
                {
                    CustomDialog = value as CustomDialog;
                }
                else if (value is ServiceMethod)
                {
                    ServiceMethod = value as ServiceMethod;
                }
                else if (value is Workflow)
                {
                    Workflow = value as Workflow;
                }
                else
                {
                    dialog = null;
                    customDialog = null;
                    serviceMethod = null;
                    workflow = null;
                }
            }
        }

        private Dialog dialog;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "DialogId", IsMandatory = false, ForeignKey = "FK_UXAction_Dialog", Cascade = CascadeAssociationOperation.None)]
        public virtual Dialog Dialog
        {
            get
            {
                return dialog;
            }
            set
            {
                if (value != null)
                {
                    dialog = value;
                    serviceMethod = null;
                    customDialog = null;
                }
            }
        }

        private CustomDialog customDialog;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "CustomDialogId", IsMandatory = false, ForeignKey = "FK_UXAction_CustomDialog", Cascade = CascadeAssociationOperation.None)]
        public virtual CustomDialog CustomDialog
        {
            get
            {
                return customDialog;
            }
            set
            {
                if (value != null)
                {
                    customDialog = value;
                    dialog = null;
                    serviceMethod = null;
                    workflow = null;
                }
            }
        }

        private ServiceMethod serviceMethod;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "ServiceMethodId", IsMandatory = false, ForeignKey = "FK_UXAction_ServiceMethod", Cascade = CascadeAssociationOperation.None)]
        public virtual ServiceMethod ServiceMethod
        {
            get
            {
                return serviceMethod;
            }
            set
            {
                if (value != null)
                {
                    serviceMethod = value;
                    dialog = null;
                    customDialog = null;
                    workflow = null;
                }
            }
        }

        private Workflow workflow;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "WorkflowId", IsMandatory = false, ForeignKey = "FK_UXAction_Workflow", Cascade = CascadeAssociationOperation.None)]
        public virtual Workflow Workflow
        {
            get
            {
                return workflow;
            }
            set
            {
                if (value != null)
                {
                    workflow = value;
                    serviceMethod = null;
                    dialog = null;
                    customDialog = null;
                }
            }
        }

        [PropertyStorageHint(IsMandatory = false, Length = 255)]
        [Localizable]
        public virtual string AskQuestionToRun { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual UXDialogResult DialogResult { get; set; }

        [PropertyMapAttribute(Type = PropertyMapType.Request)]
        [PropertyStorageHint(Column = "RequestMapId", IsMandatory = false, ForeignKey = "FK_UXAction_ReqPropMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap RequestMap { get; set; }

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
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;

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

        #region IVersionControlled Members

        [ReadOnly(true)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool IsLocked { get; set; }

        [ReadOnly(true)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Length = 40, IsMandatory = false)]
        public virtual string LockedBy { get; set; }

        [ReadOnly(true)]
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

        [ReadOnly(true)]
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
                if (MappedToObject.Id == connectedToObject.Id)
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
            setTarget = SetTargetChoice.No;
            return Guid.Empty;
        }

        #endregion
    }
}
