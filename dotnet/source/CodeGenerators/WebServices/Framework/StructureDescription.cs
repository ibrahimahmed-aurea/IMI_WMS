namespace Imi.CodeGenerators.WebServices.Framework {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class MessageDefinition {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("parameter", IsNullable=false)]
        public ParameterType[] globalParameters;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("interface", IsNullable=false)]
        public InterfaceType[] interfaces;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("structure", IsNullable=false)]
        public StructureType[] structures;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1.0.0.0")]
        public string version = "1.0.0.0";
    }
    
    /// <remarks/>
    public class ParameterType {
        
        /// <remarks/>
        public ColumnType column;
        
        /// <remarks/>
        public UseType use;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string originTable;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string originColumn;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataType;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ParameterTypeDirection direction;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ParameterTypeFieldType fieldType;
    }
    
    /// <remarks/>
    public class ColumnType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("line", IsNullable=false)]
        public string[] comment;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string caption;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string declaration;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataType;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string length;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string precision;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scale;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool primaryKey;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool mandatory;
    }
    
    /// <remarks/>
    public class ModificationType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("line", IsNullable=false)]
        public string[] comment;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool exclude;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string overrideName;
    }
    
    /// <remarks/>
    public class ChildType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
        public string minOccurrs;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string maxOccurrs;
    }
    
    /// <remarks/>
    public class StructureType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("line", IsNullable=false)]
        public string[] comment;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("child", IsNullable=false)]
        public ChildType[] children;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("modification", IsNullable=false)]
        public ModificationType[] modifications;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string queueTable;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string insertSP;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string updateSP;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string deleteSP;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string baseTable;
    }
    
    /// <remarks/>
    public class InterfaceType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string structure;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HAPIObjectName;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public InterfaceTypeDirection direction;
    }
    
    /// <remarks/>
    public enum InterfaceTypeDirection {
        
        /// <remarks/>
        In,
        
        /// <remarks/>
        InOut,
        
        /// <remarks/>
        Out,
    }
    
    /// <remarks/>
    public class UseType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool insert;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool update;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool delete;
    }
    
    /// <remarks/>
    public enum ParameterTypeDirection {
        
        /// <remarks/>
        In,
        
        /// <remarks/>
        InOut,
        
        /// <remarks/>
        Out,
        
        /// <remarks/>
        Result,
    }
    
    /// <remarks/>
    public enum ParameterTypeFieldType {
        
        /// <remarks/>
        Normal,
        
        /// <remarks/>
        OpCode,
        
        /// <remarks/>
        SystemId,
        
        /// <remarks/>
        SystemAdmin,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class Package {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("procedure", IsNullable=false)]
        public ProcedureType[] procedures;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
    
    /// <remarks/>
    public class ProcedureType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("parameter", IsNullable=false)]
        public ParameterType[] parameters;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ProcedureTypeType type;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool overloaded;
    }
    
    /// <remarks/>
    public enum ProcedureTypeType {
        
        /// <remarks/>
        Function,
        
        /// <remarks/>
        Procedure,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class DictionaryDefinition
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("field", IsNullable = false)]
        public InterfaceFieldType[] fields;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1.0.0.0")]
        public string version = "1.0.0.0";
    }

    /// <remarks/>
    public class InterfaceFieldType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
}
