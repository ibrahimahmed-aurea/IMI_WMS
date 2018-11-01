using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    [VisualDesigner("RadioGroup")]
    public class UXRadioGroup : UXComponent, IBindable, ILocalizable
    {
        public UXRadioGroup()
            : this(null)
        {
        }

        public UXRadioGroup(string name)
            : base(name)
        {
            Keys = new List<string>();
            Values = new List<string>();
        }

        [Browsable(false)]
        public List<string> Keys { get; set; }

        [Browsable(false)]
        public List<string> Values { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public IDictionary<string, string> KeyValues 
        {
            get
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();

                for (int i = 0; i < Keys.Count; i++)
                {
                    keyValue.Add(Keys[i], Values[i]);
                }

                return keyValue;
            }
            set
            {
                // Clear keys and values first
                Keys.Clear();
                Values.Clear();

                // Add all keys and values from the dictionary
                foreach (KeyValuePair<string, string> keyValue in value)
                {
                    Keys.Add(keyValue.Key);
                    Values.Add(keyValue.Value);
                }
            }
        }

        public string DefaultValue  { get; set;}

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
