using System;
using System.Collections.Generic;
using System.Text;
using Cdc.HbmGenerator.Attributes;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "UXApplication")]
    public class UXApplication : Application, IXmlSerializable
    {
        public UXApplication()
        {
        }

        private IList<ApplicationVersion> appVersionDependencies;

        [CollectionStorageHint(Lazy = true, Inverse = true, Cascade = CascadeOperation.All)]
        public virtual IList<ApplicationVersion> ApplicationVersionDependencies
        {
            get
            {
                if (appVersionDependencies == null)
                {
                    appVersionDependencies = new List<ApplicationVersion>();
                }
                return appVersionDependencies;
            }
            set { appVersionDependencies = value; }
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
