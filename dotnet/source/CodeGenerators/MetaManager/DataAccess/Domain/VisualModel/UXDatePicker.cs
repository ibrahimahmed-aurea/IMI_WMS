using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("DatePicker")]
    public class UXDatePicker : UXComponent, IBindable
    {
        public UXDatePicker()
            : this(null)
        {
        }

        public UXDatePicker(string name)
            : base(name)
        {
            Height = 23;
            Width = -1;
        }

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

    }
}
