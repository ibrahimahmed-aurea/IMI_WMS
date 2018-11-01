using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    public abstract class DbProperty : IMappableProperty
    {
        public DbProperty()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        /// <summary>
        /// Filename for StoredProcedure/column
        /// </summary>
        [PropertyStorageHint(Length = 50, IsMandatory = true)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Determines the order of the properties in the method signature.
        /// </summary>
        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Sequence { get; set; }

        /// <summary>
        /// Database native data type
        /// </summary>
        [PropertyStorageHint(Length = 50, IsMandatory = false)]
        public virtual string DbDatatype { get; set; }

        /// <summary>
        /// Total length of string, for non string types it represents 
        /// the total memory space needed.
        /// </summary>
        [PropertyStorageHint(IsMandatory = false)]
        public virtual int? Length { get; set; }

        /// <summary>
        /// Total number of digits in a numerical value
        /// </summary>
        /// <remarks>Second part of the oracle number(x,y), Precision is x</remarks>
        [PropertyStorageHint(IsMandatory = false)]
        public virtual int? Precision { get; set; }

        /// <summary>
        /// Number of decimal digits ouf the total number of digits, see Precision
        /// </summary>
        /// <remarks>Second part of the oracle number(x,y), Scale is y</remarks>
        [PropertyStorageHint(IsMandatory = false)]
        public virtual int? Scale { get; set; }

        [PropertyStorageHint(Length = 100, IsMandatory = false)]
        public virtual string OriginalTable { get; set; }

        [PropertyStorageHint(Length = 100, IsMandatory = false)]
        public virtual string OriginalColumn { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual DbPropertyType PropertyType { get; set; }

        /// <summary>
        /// Text represents the original source that is the base for
        /// this StoredProcedure, holds the expression if the StoredProcedure is not
        /// represented by a specific database StoredProcedure
        /// </summary>
        [PropertyStorageHint(Length = 2048, IsMandatory = false)]
        public virtual string Text { get; set; }

        /// <summary>
        /// Only applies to properties that are input fields in stored procedures. Detected through the /* Mandatory */ comment in 
        /// the specof a package.
        /// </summary>
        public virtual bool? IsMandatory { get; set; }
        
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual Type Type
        {
            get
            {
                return null;
            }
        }

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
