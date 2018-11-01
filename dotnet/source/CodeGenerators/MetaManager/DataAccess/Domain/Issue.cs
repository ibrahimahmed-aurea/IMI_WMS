using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "Issue")]
    public class Issue : IDomainObject
    {
        public Issue()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_Issue_Application")]
        public virtual Application Application { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual IssueObjectType ObjectType { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual Guid ObjectId { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual IssueSeverityType Severity { get; set; }

        [PropertyStorageHint(Length = 1000, IsMandatory = true)]
        public virtual string Title { get; set; }

        [PropertyStorageHint(Length = 2000, IsMandatory = true)]
        public virtual string Text { get; set; }

        [PropertyStorageHint(Ignore = true)]
        public virtual object Tag { get; set; }

        private bool hidden;

        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool Hidden 
        {
            get
            {
                return hidden;
            }
            set
            {
                hidden = value;
                IsNewHiddenValue = value;
            }
        }

        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsNewHiddenValue { get; set; }

        [PropertyStorageHint(Ignore = true)]
        public virtual string DialogCreator { get; set; }

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
