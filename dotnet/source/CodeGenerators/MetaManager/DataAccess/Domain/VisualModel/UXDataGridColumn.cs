using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [Serializable]
    public class UXDataGridColumn : UXComponent, IBindable, ILocalizable
    {
        public UXDataGridColumn()
            : base()
        {
        }

        [Localizable]
        public string Caption { get; set; }
        
        public int Sequence { get; set; }

        private MappedProperty mappedProperty;

        [XmlIgnore]
        [DomainReference]
        public MappedProperty MappedProperty
        {
            get
            {
                return mappedProperty;
            }
            set
            {
                if (mappedProperty != null && value != mappedProperty)
                    Hint = null;

                mappedProperty = value;

                if (mappedProperty == null)
                    mappedPropertyId = Guid.Empty;
            }
        }

        private Guid mappedPropertyId;

        public Guid MappedPropertyId
        {
            get
            {
                if (MappedProperty != null)
                    return MappedProperty.Id;
                else
                    return mappedPropertyId;
            }
            set
            {
                mappedPropertyId = value;
            }
        }

        private DataSource dataSource;

        [XmlIgnore]
        [DomainReference]
        public DataSource DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;

                if (dataSource == null)
                    dataSourceId = Guid.Empty;
            }
        }

        private Guid dataSourceId;

        public Guid DataSourceId
        {
            get
            {
                if (DataSource != null)
                    return DataSource.Id;
                else
                    return dataSourceId;
            }
            set
            {
                dataSourceId = value;
            }
        }


        #region ILocalizable Members

        private string metaId;

        public string MetaId
        {
            get
            {
                if (string.IsNullOrEmpty(metaId))
                    metaId = Guid.NewGuid().ToString();

                return metaId;
            }
            set
            {
                metaId = value;
            }
        }

        #endregion
    }
}
