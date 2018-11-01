using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Imi.SupplyChain.UX.Infrastructure
{
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }

    [Serializable]
    public class ShellHyperlink
    {
        private SerializableDictionary<string, string> _dataDictionary;
        
        public ShellHyperlink()
            : this(null, null, null)
        {
        }

        public ShellHyperlink(string link, string moduleId, IDictionary<string, string> data)
        {
            Link = link;
            ModuleId = moduleId;
            _dataDictionary = new SerializableDictionary<string, string>();

            if (data != null)
            {
                foreach (var entry in data)
                {
                    _dataDictionary.Add(entry.Key, entry.Value);
                }
            }
        }

        public string Link { get; set; }
                
        public SerializableDictionary<string, string> Data
        {
            get
            {
                return _dataDictionary;
            }
            set
            {
                _dataDictionary = value;
            }
        }

        public string ModuleId { get; set; }
    }
}
