﻿using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Workflow.ComponentModel;
using NHibernate;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "WorkflowDialog")]
    public class WorkflowDialog : IXmlSerializable, IDomainObject
    {
        public WorkflowDialog()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }


        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "DialogId", IsMandatory = true, ForeignKey = "FK_WorkflowDlg_Dialog")]
        public virtual Dialog Dialog { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "WorkflowId", IsMandatory = true, ForeignKey = "FK_WorkflowDlg_Workflow")]
        public virtual Workflow Workflow { get; set; }
        
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
    }
}
