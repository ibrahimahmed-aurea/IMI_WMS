using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("FileSelector")]
    public class UXFileSelector : UXComponent, IBindable
    {
        public UXFileSelector()
            : this(null)
        {
        }

        public UXFileSelector(string name)
            :base(name)
        {
            IsReadOnly = true;
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



        //==============================================================================================================================================
        private MappedProperty fileDataMappedProperty;
        
        [XmlIgnore]
        [DomainReference]
        public MappedProperty FileDataMappedProperty
        {
            get
            {
                return fileDataMappedProperty;
            }
            set
            {
                fileDataMappedProperty = value;

                if (fileDataMappedProperty == null)
                    fileDataMappedPropertyId = Guid.Empty;
            }
        }

        private Guid fileDataMappedPropertyId;

        public Guid FileDataMappedPropertyId
        {
            get
            {
                if (FileDataMappedProperty != null)
                    return FileDataMappedProperty.Id;
                else
                    return fileDataMappedPropertyId;
            }
            set
            {
                fileDataMappedPropertyId = value;
            }
        }


        private MappedProperty fileDateModifiedMappedProperty;

        [XmlIgnore]
        [DomainReference]
        public MappedProperty FileDateModifiedMappedProperty
        {
            get
            {
                return fileDateModifiedMappedProperty;
            }
            set
            {
                fileDateModifiedMappedProperty = value;

                if (fileDateModifiedMappedProperty == null)
                    fileDateModifiedMappedPropertyId = Guid.Empty;
            }
        }

        private Guid fileDateModifiedMappedPropertyId;

        public Guid FileDateModifiedMappedPropertyId
        {
            get
            {
                if (FileDateModifiedMappedProperty != null)
                    return FileDateModifiedMappedProperty.Id;
                else
                    return fileDateModifiedMappedPropertyId;
            }
            set
            {
                fileDateModifiedMappedPropertyId = value;
            }
        }


        private MappedProperty fileSizeMappedProperty;

        [XmlIgnore]
        [DomainReference]
        public MappedProperty FileSizeMappedProperty
        {
            get
            {
                return fileSizeMappedProperty;
            }
            set
            {
                fileSizeMappedProperty = value;

                if (fileSizeMappedProperty == null)
                    fileSizeMappedPropertyId = Guid.Empty;
            }
        }

        private Guid fileSizeMappedPropertyId;

        public Guid FileSizeMappedPropertyId
        {
            get
            {
                if (FileSizeMappedProperty != null)
                    return FileSizeMappedProperty.Id;
                else
                    return fileSizeMappedPropertyId;
            }
            set
            {
                fileSizeMappedPropertyId = value;
            }
        }

        //==============================================================================================================================================


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
