﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://webservices.im.se/ocp/GetGuiconfiguration", ConfigurationName="GetGuiconfigurationServiceReal.GetGuiconfigurationService")]
    public interface GetGuiconfigurationService {
        
        // CODEGEN: Generating message contract since the wrapper name (Response) of message GetGuiconfigurationResponse does not match the default value (GetGuiconfiguration)
        [System.ServiceModel.OperationContractAttribute(Action="genericOperation", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationResponse GetGuiconfiguration(Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://webservices.im.se/ocp/GetGuiconfiguration")]
    public partial class GetGuiconfigurationUSER : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string userIdField;
        
        private decimal portNumberField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string UserId {
            get {
                return this.userIdField;
            }
            set {
                this.userIdField = value;
                this.RaisePropertyChanged("UserId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public decimal PortNumber {
            get {
                return this.portNumberField;
            }
            set {
                this.portNumberField = value;
                this.RaisePropertyChanged("PortNumber");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://webservices.im.se/ocp/GetGuiconfiguration")]
    public partial class ResponseFailure : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string errorTextField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string ErrorText {
            get {
                return this.errorTextField;
            }
            set {
                this.errorTextField = value;
                this.RaisePropertyChanged("ErrorText");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34234")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://webservices.im.se/ocp/GetGuiconfiguration")]
    public partial class ResponseSuccess : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string auto_startField;
        
        private string hostField;
        
        private decimal portField;
        
        private string programField;
        
        private string env_varsField;
        
        private string parametersField;
        
        private string working_directoryField;
        
        private string languageField;
        
        private string clientprogramField;
        
        private string systemnameField;
        
        private string help_urlField;
        
        private string decimal_keyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string auto_start {
            get {
                return this.auto_startField;
            }
            set {
                this.auto_startField = value;
                this.RaisePropertyChanged("auto_start");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string host {
            get {
                return this.hostField;
            }
            set {
                this.hostField = value;
                this.RaisePropertyChanged("host");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public decimal port {
            get {
                return this.portField;
            }
            set {
                this.portField = value;
                this.RaisePropertyChanged("port");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string program {
            get {
                return this.programField;
            }
            set {
                this.programField = value;
                this.RaisePropertyChanged("program");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string env_vars {
            get {
                return this.env_varsField;
            }
            set {
                this.env_varsField = value;
                this.RaisePropertyChanged("env_vars");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string parameters {
            get {
                return this.parametersField;
            }
            set {
                this.parametersField = value;
                this.RaisePropertyChanged("parameters");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string working_directory {
            get {
                return this.working_directoryField;
            }
            set {
                this.working_directoryField = value;
                this.RaisePropertyChanged("working_directory");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string language {
            get {
                return this.languageField;
            }
            set {
                this.languageField = value;
                this.RaisePropertyChanged("language");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public string clientprogram {
            get {
                return this.clientprogramField;
            }
            set {
                this.clientprogramField = value;
                this.RaisePropertyChanged("clientprogram");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string systemname {
            get {
                return this.systemnameField;
            }
            set {
                this.systemnameField = value;
                this.RaisePropertyChanged("systemname");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string help_url {
            get {
                return this.help_urlField;
            }
            set {
                this.help_urlField = value;
                this.RaisePropertyChanged("help_url");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=11)]
        public string decimal_key {
            get {
                return this.decimal_keyField;
            }
            set {
                this.decimal_keyField = value;
                this.RaisePropertyChanged("decimal_key");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetGuiconfiguration", WrapperNamespace="http://webservices.im.se/ocp/GetGuiconfiguration", IsWrapped=true)]
    public partial class GetGuiconfigurationRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://webservices.im.se/ocp/GetGuiconfiguration", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("USER")]
        public Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationUSER[] USER;
        
        public GetGuiconfigurationRequest() {
        }
        
        public GetGuiconfigurationRequest(Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationUSER[] USER) {
            this.USER = USER;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Response", WrapperNamespace="http://webservices.im.se/ocp/GetGuiconfiguration", IsWrapped=true)]
    public partial class GetGuiconfigurationResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://webservices.im.se/ocp/GetGuiconfiguration", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("Failure", typeof(ResponseFailure))]
        [System.Xml.Serialization.XmlElementAttribute("Success", typeof(ResponseSuccess))]
        public object[] Items;
        
        public GetGuiconfigurationResponse() {
        }
        
        public GetGuiconfigurationResponse(object[] Items) {
            this.Items = Items;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GetGuiconfigurationServiceChannel : Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetGuiconfigurationServiceClient : System.ServiceModel.ClientBase<Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationService>, Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationService {
        
        public GetGuiconfigurationServiceClient() {
        }
        
        public GetGuiconfigurationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GetGuiconfigurationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetGuiconfigurationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetGuiconfigurationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationResponse Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationService.GetGuiconfiguration(Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationRequest request) {
            return base.Channel.GetGuiconfiguration(request);
        }
        
        public object[] GetGuiconfiguration(Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationUSER[] USER) {
            Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationRequest inValue = new Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationRequest();
            inValue.USER = USER;
            Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationResponse retVal = ((Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceImplementation.GetGuiconfigurationServiceReal.GetGuiconfigurationService)(this)).GetGuiconfiguration(inValue);
            return retVal.Items;
        }
    }
}
