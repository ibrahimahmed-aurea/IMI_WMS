using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("TextBox")]
    public class UXTextBox : UXComponent, IBindable
    {
        public UXTextBox()
            : base()
        {           
        }

        public UXTextBox(string name)
            : base(name)
        {
        }

        private bool isMultiLine;

        public bool IsMultiLine  
        {
            get
            {
                return isMultiLine;
            }
            set
            {
                isMultiLine = value;

                if (isMultiLine && (Height < 50))
                    Height = 50;
            }
        }
       
        public string PasswordChar
        { get; set; }

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

        public string DefaultValue
        {
            get
            {
                if (MappedProperty != null)
                    return MappedProperty.DefaultValue;
                else
                    return null;
            }
        }
        
        public string DisplayFormat 
        {
            get
            {
                if (MappedProperty != null)
                    return MappedProperty.DisplayFormat;
                else
                    return null;
            }
        }
    }
}
