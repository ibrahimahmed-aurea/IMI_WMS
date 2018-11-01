using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Imi.HbmGenerator.Attributes;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;
using Cdc.Framework.ExtensionMethods;

namespace Cdc.MetaManager.DataAccess.Domain
{
    [ClassStorageHint(TableName = "Report")]
    public class Report : IXmlSerializable, IVersionControlled, IMappedObject
    {
        public Report()
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

        [PropertyStorageHint(Length = 12, IsMandatory = true)]
        public virtual string DocumentType { get; set; }

        [PropertyStorageHint(Length = 50, IsMandatory = true)]
        public virtual string DocumentTypeDefinition { get; set; }

        [PropertyStorageHint(IsMandatory = false)]
        public virtual int DataDuplicates { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual WarehouseReportType WarehouseReportType { get; set; }

        [PropertyStorageHint(Length = 500, IsMandatory = false)]
        public virtual string Description { get; set; }

        [PropertyStorageHint(Length = 40, IsMandatory = false)]
        public virtual string CreatorName { get; set; }

        [PropertyStorageHint(IsMandatory = true)]
        public virtual bool IsABCEnabled { get; set; }

        [DataAccess.DomainXmlById]
        [PropertyStorageHint(IsMandatory = true, ForeignKey = "FK_Report_Application")]
        public virtual Application Application { get; set; }

        private PropertyMap map;

        [PropertyStorageHint(Column = "InterfaceMapId", IsMandatory = false, ForeignKey = "FK_Report_PropertyMap", Cascade = CascadeAssociationOperation.All)]
        public virtual PropertyMap RequestMap
        {
            get
            {
                return map;
            }
            set
            {
                map = value;
            }
        }

        private IList<ReportQuery> reportQueries;

        [CollectionStorageHint(Inverse = true, Cascade = CascadeOperation.AllDeleteOrphan)]
        public virtual IList<ReportQuery> ReportQueries
        {
            get
            {
                if (reportQueries == null)
                {
                    reportQueries = new List<ReportQuery>();
                }

                return reportQueries;
            }
            set
            {
                reportQueries = value;
            }
        }

        [DataAccess.DomainXmlIgnore]
        [PropertyStorageHint(Ignore = true)]
        public virtual string PackageName
        {
            get
            {
                return string.Format("Report_{0}", Name);
            }
        }

        public virtual ReportInterfaceParameterList GetReportInterfaceList()
        {
            ReportInterfaceParameterList parameterList = new ReportInterfaceParameterList();

            parameterList.AddRange(GetUserDefinedInterfaceParameterList());
            parameterList.AddRange(GetStaticInterfaceParameterList());

            return parameterList;
        }

        public virtual ReportInterfaceParameterList GetUserDefinedInterfaceParameterList()
        {
            ReportInterfaceParameterList parameterList = new ReportInterfaceParameterList();

            foreach (MappedProperty mappedProperty in this.RequestMap.MappedProperties.OrderBy(p => p.Sequence))
            {
                parameterList.Add(mappedProperty.Name, DataModelImporter.GetDbType(mappedProperty));
            }

            return parameterList;
        }


        public virtual ReportInterfaceParameterList GetStaticInterfaceParameterList()
        {
            return GetStaticInterfaceParameterList(this.WarehouseReportType, this.IsABCEnabled);
        }

        public virtual ReportInterfaceParameterList GetStaticInterfaceParameterList(WarehouseReportType warehouseReportType, bool isABCEnabled)
        {
            ReportInterfaceParameterList parameterList = new ReportInterfaceParameterList();

            // Check if this is a Warehouse report
            bool isDefinedWarehouseReport = this.Application.Name == "Warehouse" &&
                                            warehouseReportType != WarehouseReportType.NotApplicable;

            if (isDefinedWarehouseReport)
            {
                // First add CHECK parameters used for calling the "IsDefined..." procedures.
                if (warehouseReportType == WarehouseReportType.WarehouseReport)
                {
                    parameterList.Add("CHECK_WHID_I", "WH.WHID%type");
                }
                else if (warehouseReportType == WarehouseReportType.DeliveryReport)
                {
                    parameterList.Add("CHECK_DLVRYMETH_ID_I", "DMDOC.DLVRYMETH_ID%type");
                    parameterList.Add("CHECK_COMPANY_ID_I", "DMDOC.COMPANY_ID%type");
                }
                else if (warehouseReportType == WarehouseReportType.FreighterReport)
                {
                    parameterList.Add("CHECK_FREID_I", "FREDMDOC.FREID%type");
                    parameterList.Add("CHECK_DLVRYMETH_ID_I", "FREDMDOC.DLVRYMETH_ID%type");
                }
            }

            if (isABCEnabled)
            {
                // Add ACTLOG parameters used for adding a row to the Activity Log (ABC).
                parameterList.Add("ACTLOG_WHID_I", "ACTLOG.WHID%type");
                parameterList.Add("ACTLOG_COMPANY_ID_I", "ACTLOG.COMPANY_ID%type");
            }

            // Add META parameters including their default value since they don't have to be set.
            parameterList.Add("META_DOCSUBTYPE_I", "varchar2", "null");
            parameterList.Add("META_TERID_I", "varchar2", "null");
            parameterList.Add("META_PRTID_I", "varchar2", "null");
            parameterList.Add("META_EMPID_I", "varchar2", "null");
            parameterList.Add("META_NO_COPIES_I", "number", "null");

            if (isDefinedWarehouseReport)
            {
                // Add additional parameter to be able to skip the check if needed
                parameterList.Add("SKIP_CHECK_I", "varchar2", "Def.No");
            }

            // Add OUT parameter
            parameterList.Add("ALMID_O", "ALM.ALMID%type", ReportInterfaceParameterDirection.Out);

            return parameterList;
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

        #region IMappedObject Members

        public virtual Guid GetRequestMapId(IDomainObject connectedToObject, out SetTargetChoice setTarget)
        {
            setTarget = SetTargetChoice.No;
            
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
