using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.ComponentModel.Design.Serialization;
using System.Workflow.ComponentModel.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace Cdc.MetaManager.DataAccess
{
    [Serializable]
    public class RuleSetWrapper : IXmlSerializable
    {
        public RuleSetWrapper()
        {
        }

        public RuleSet RuleSet { get; set; }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (!reader.IsEmptyElement)
            {
                string ruleSetXml = reader.ReadInnerXml();

                XmlReader innerReader = XmlReader.Create(new StringReader(ruleSetXml));
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                RuleSet = serializer.Deserialize(innerReader) as RuleSet;
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (RuleSet != null)
            {
                WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
                serializer.Serialize(writer, RuleSet);
            }
            
        }

        #endregion
    }
}
