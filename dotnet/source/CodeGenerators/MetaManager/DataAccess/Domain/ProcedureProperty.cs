using System;
using System.Collections.Generic;
using System.Text;
using Imi.HbmGenerator.Attributes;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "DbStoredProcProperty")]
    public class ProcedureProperty : DbProperty, IXmlSerializable
    {
        public ProcedureProperty()
        {
        }

        /// <summary>
        /// Only applicable to stored procedure
        /// </summary>
        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string DefaultValue { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_StProcProp_StProc")]
        public virtual StoredProcedure StoredProcedure { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Id);
        }

        public virtual void CheckIfEqual(ProcedureProperty property)
        {
            if (property == null)
            {
                throw new Exception(string.Format("The parameter doesn't exist! (Name: \"{0}\")", this.Name));
            }

            if (this.Name != property.Name)
            {
                throw new Exception(string.Format("Parameter names are not the same! (Original: \"{0}\"; New: \"{1}\")", this.Name, property.Name));
            }

            if (this.PropertyType != property.PropertyType)
            {
                throw new Exception(string.Format("Parameter (\"{0}\") types are not the same! (Original: \"{1}\"; New: \"{2}\")", this.Name, this.PropertyType, property.PropertyType));
            }

            if (!string.IsNullOrEmpty(this.DefaultValue) &&
                !string.IsNullOrEmpty(property.DefaultValue) &&
                this.DefaultValue != property.DefaultValue)
            {
                throw new Exception(string.Format("Parameter (\"{0}\") defaultvalues are not the same! (Original: \"{1}\"; New: \"{2}\")", this.Name, this.DefaultValue, property.DefaultValue));
            }

            // Checking if it's a reference to table & column or...
            if (!string.IsNullOrEmpty(this.OriginalTable) &&
                !string.IsNullOrEmpty(this.OriginalColumn))
            {
                // See if its the same table reference
                if ((this.OriginalTable != property.OriginalTable) ||
                    (this.OriginalColumn != property.OriginalColumn))
                {
                    // Also check if it's a new datatype instead of a table/column combination
                    if (!string.IsNullOrEmpty(property.DbDatatype))
                    {
                        throw new Exception(string.Format("Parameter (\"{0}\") points to a datatype instead of a table and column! (Original: \"{1}.{2}\"; New (datatype): \"{3}\")",
                                                          this.Name,
                                                          this.OriginalTable,
                                                          this.OriginalColumn,
                                                          property.DbDatatype));
                    }
                    else
                    {
                        throw new Exception(string.Format("Parameter (\"{0}\") points to a different table or column! (Original: \"{1}.{2}\"; New: \"{3}.{4}\")", 
                                                          this.Name,
                                                          this.OriginalTable,
                                                          this.OriginalColumn,
                                                          property.OriginalTable,
                                                          property.OriginalColumn));
                    }
                }
            }
            // ...if it's a datatype
            else if (!string.IsNullOrEmpty(this.DbDatatype)) 
            {
                // Check if it's the same datatypes 
                if (this.DbDatatype != property.DbDatatype)
                {
                    // Check if it's set to a table and column property instead
                    if (!string.IsNullOrEmpty(property.OriginalTable) ||
                        !string.IsNullOrEmpty(property.OriginalColumn))
                    {
                        throw new Exception(string.Format("Parameter (\"{0}\") points to a table and column instead of a datatype! (Original (datatype): \"{1}\"; New: \"{2}.{3}\")",
                                                          this.Name,
                                                          this.DbDatatype,
                                                          property.OriginalTable,
                                                          property.OriginalColumn));
                    }
                    else
                    {
                        throw new Exception(string.Format("Parameter (\"{0}\") points to a different datatype! (Original: \"{1}\"; New: \"{2}\")", 
                                                          this.Name,
                                                          this.DbDatatype,
                                                          property.DbDatatype));
                    }
                }

                // Check length
                if ((this.Length ?? 0) < (property.Length ?? 0))
                {
                   throw new Exception(string.Format("Parameter (\"{0}\") is set to a smaller length than the original! (Original: \"{1}\"; New: \"{2}\")", this.Name, this.Length ?? 0, this.Length ?? 0));
                }

                // Check Precision
                if ((this.Precision ?? 0) != (property.Precision ?? 0))
                {
                   throw new Exception(string.Format("Parameter (\"{0}\") has a different precision set than the original! (Original: \"{1}\"; New: \"{2}\")", this.Name, this.Precision ?? 0, this.Precision ?? 0));
                }

                if ((this.Scale ?? 0) != (property.Scale ?? 0))
                {
                   throw new Exception(string.Format("Parameter (\"{0}\") has a different scale set than the original! (Original: \"{1}\"; New: \"{2}\")", this.Name, this.Scale ?? 0, this.Scale ?? 0));
                }
            }
            // Neither table/column nor datatype, error!
            else
            {
                throw new Exception(string.Format("Parameter (\"{0}\") is not configured to have neither table/column nor a datatype!", this.Name));
            }
        }

        public static bool IsEqual(ProcedureProperty firstProp, ProcedureProperty secondProp)
        {
            return
                (
                    (firstProp.DbDatatype == secondProp.DbDatatype) &&
                    (firstProp.Length == secondProp.Length) &&
                    (firstProp.Precision == secondProp.Precision) &&
                    (firstProp.Scale == secondProp.Scale) &&
                    (firstProp.OriginalColumn == secondProp.OriginalColumn) &&
                    (firstProp.OriginalTable == secondProp.OriginalTable) &&
                    (firstProp.Text == secondProp.Text)
                );
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


    }
}
