using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "DbStoredProcedure")]
    public class StoredProcedure : IMappableObject, IXmlSerializable
    {
        public StoredProcedure()
        {
            IsTransient = true;
        }

        public virtual Guid Id { get; set; }

        [Browsable(false)]
        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual bool IsTransient { get; set; }

        [Search(SearchType = SearchTypes.FreeText)]
        [PropertyStorageHint(Length = 50, IsMandatory = false)]
        public virtual string ProcedureName { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = false)]
        public virtual string RefCursorParameterName { get; set; }

        private bool? isReturningRefCursor;

        [PropertyStorageHint(IsMandatory = false)]
        public virtual bool? IsReturningRefCursor
        {
            get
            {
                // if not set
                if (isReturningRefCursor == null)
                    return false;

                return isReturningRefCursor;
            }

            set
            {
                isReturningRefCursor = value;
            }
        }

        private IList<ProcedureProperty> properties;

        [CollectionStorageHint(Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<ProcedureProperty> Properties
        {
            get
            {
                if (properties == null)
                {
                    properties = new List<ProcedureProperty>();
                }
                return properties;
            }
            set { properties = value; }

        }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual ActionMapTarget ObjectType
        {
            get
            {
                return ActionMapTarget.StoredProcedure;
            }
        }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = false, ForeignKey = "FK_StorProc_SourceFile")]
        public virtual Package Package { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        /// <summary>
        ///     Check if two StoredProcedures are equal.
        ///     It's assumed that the two are located in the same package!
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns></returns>
        public virtual bool Equals(StoredProcedure storedProcedure)
        {
            bool isEqual = false;

            // Check if same name
            if (ProcedureName == storedProcedure.ProcedureName)
            {
                if (Properties.Count == storedProcedure.Properties.Count)
                {
                    bool allParametersEqual = true;

                    // Now check all parameters. The order doesn't matter since the procedure might
                    // have been synchronized, and since the parameters are set "by name" then the order
                    // isn't important.
                    foreach (ProcedureProperty property in Properties)
                    {
                        if ((from ProcedureProperty compProp in storedProcedure.Properties
                             where compProp.Name == property.Name &&
                                  compProp.DbDatatype == property.DbDatatype &&
                                  compProp.PropertyType == property.PropertyType &&
                                  compProp.OriginalTable == property.OriginalTable &&
                                  compProp.OriginalColumn == property.OriginalColumn &&
                                  (compProp.Length == property.Length || compProp.Length == null) &&
                                  (compProp.Precision == property.Precision || compProp.Precision == null) &&
                                  (compProp.Scale == property.Scale || compProp.Scale == null) &&
                                  compProp.Text == property.Text
                             select compProp).Count() == 0)
                        {
                            allParametersEqual = false;
                            break;
                        }
                    }

                    if (allParametersEqual)
                        isEqual = true;
                }
            }

            return isEqual;
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
