using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cdc.Framework.Parsing.CSharpParsing
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "namespace")]
    public partial class NamespaceReference
    {
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "assembly")]
        public string AssemblyName { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "assembly")]
    public partial class AssemblyReference
    {
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "depends")]
        public string Depends { get; set; }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Source { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "crossreference")]
    public partial class Crossreference
    {
        [System.Xml.Serialization.XmlArrayItemAttribute("assembly", IsNullable = false)]
        [System.Xml.Serialization.XmlArray(ElementName = "assemblies")]
        public AssemblyReference[] Assemblies { get; set; }

        [System.Xml.Serialization.XmlArrayItemAttribute("namespace", IsNullable = false)]
        [System.Xml.Serialization.XmlArray(ElementName = "namespaces")]
        public NamespaceReference[] Namespaces { get; set; }
    }
}
