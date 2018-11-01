using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Workflow.ComponentModel;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "WorkflowSubworkflow")]
    public class WorkflowSubworkflow : IXmlSerializable, IDomainObject
    {
        public WorkflowSubworkflow()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "SubWorkflowId", IsMandatory = true, ForeignKey = "FK_WorkflowSubworkflow_SubWorkflow")]
        public virtual Workflow SubWorkflow { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "WorkflowId", IsMandatory = true, ForeignKey = "FK_WorkflowSubworkflow_Workflow")]
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
