﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace Imi.SupplyChain.Server.Job.CentiroAdapter.DataEntities {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class GenericReport {
        
        private MetaDataType metaDataField;
        
        private string dataField;
        
        /// <remarks/>
        public MetaDataType MetaData {
            get {
                return this.metaDataField;
            }
            set {
                this.metaDataField = value;
            }
        }
        
        /// <remarks/>
        public string Data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MetaDataType {
        
        private string documentTypeField;
        
        private string documentSubTypeField;
        
        private string applicationIdentityField;
        
        private string terminalIdentityField;
        
        private string printerIdentityField;
        
        private string userIdentityField;
        
        private sbyte numberOfCopiesField;
        
        private bool isCopyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string documentType {
            get {
                return this.documentTypeField;
            }
            set {
                this.documentTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string documentSubType {
            get {
                return this.documentSubTypeField;
            }
            set {
                this.documentSubTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string applicationIdentity {
            get {
                return this.applicationIdentityField;
            }
            set {
                this.applicationIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string terminalIdentity {
            get {
                return this.terminalIdentityField;
            }
            set {
                this.terminalIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string printerIdentity {
            get {
                return this.printerIdentityField;
            }
            set {
                this.printerIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string userIdentity {
            get {
                return this.userIdentityField;
            }
            set {
                this.userIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public sbyte numberOfCopies {
            get {
                return this.numberOfCopiesField;
            }
            set {
                this.numberOfCopiesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isCopy {
            get {
                return this.isCopyField;
            }
            set {
                this.isCopyField = value;
            }
        }
    }
}
