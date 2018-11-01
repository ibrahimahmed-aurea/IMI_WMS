using System;
using System.Collections.Generic;
using System.Text;
using Cdc.HbmGenerator.Attributes;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint( TableName="ApplicationVersion")]
    public class ApplicationVersion : IXmlSerializable
    {
        public ApplicationVersion()
        {
        }

        public virtual Guid Id { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true)]
        public virtual string Name { get; set; }
                
        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Major { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Minor { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual int Build { get; set; }

        [PropertyStorageHint(Length = 255, IsMandatory = false)]
        public virtual string UserSessionTypeName { get; set; }

        private Application application;

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual Application Application
        {
            get
            {
                return application;
            }
            set
            {

                application = value;

                if (application is UXApplication)
                {
                    uxApplication = value as UXApplication;
                    servicesApplication = null;
                }
                else
                {
                    uxApplication = null;
                    servicesApplication = value as Application;
                }
            }
        }


        private Application servicesApplication;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column="ApplicationId", Lazy = false, IsMandatory = false, ForeignKey = "FK_AppVer_App")]
        public virtual Application ServicesApplication
        {
            get
            {
                return servicesApplication;
            }
            set
            {
                if (Application == null)
                    Application = value;
            }
        }

        private UXApplication uxApplication;

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(Column = "UXApplicationId", Lazy = false, IsMandatory = false, ForeignKey = "FK_AppVer_UXApp")]
        public virtual UXApplication UXApplication
        {
            get
            {
                return uxApplication;
            }
            set
            {
                if (Application == null)
                    Application = value;
            }
        }

        public virtual string GetPrefix()
        {
            if (UXApplication != null)
            {
                return string.Format("FE{0}", UXApplication.Id.ToString());
            }
            else if (ServicesApplication != null)
            {
                return string.Format("BE{0}", ServicesApplication.Id.ToString());
            }
            else
                return string.Empty;

        }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore=true)]
        public virtual string Prefix
        {
            get
            {
                if (UXApplication != null)
                {
                    return string.Format("FE{0}", UXApplication.Id.ToString());
                }
                return string.Empty;
            }
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
