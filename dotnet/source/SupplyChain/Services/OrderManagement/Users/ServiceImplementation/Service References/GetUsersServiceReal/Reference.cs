﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://webservices.im.se/ocp/GetUsers", ConfigurationName="GetUsersServiceReal.GetUsersService")]
    public interface GetUsersService {
        
        // CODEGEN: Generating message contract since the wrapper name (Response) of message GetUsersResponse does not match the default value (GetUsers)
        [System.ServiceModel.OperationContractAttribute(Action="genericOperation", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersResponse GetUsers(Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18054")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://webservices.im.se/ocp/GetUsers")]
    public partial class GetUsersUser : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string loginIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string LoginId {
            get {
                return this.loginIdField;
            }
            set {
                this.loginIdField = value;
                this.RaisePropertyChanged("LoginId");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18054")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://webservices.im.se/ocp/GetUsers")]
    public partial class ResponseFailure : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string loginIdField;
        
        private string errorTextField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string LoginId {
            get {
                return this.loginIdField;
            }
            set {
                this.loginIdField = value;
                this.RaisePropertyChanged("LoginId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18054")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://webservices.im.se/ocp/GetUsers")]
    public partial class ResponseSuccess : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string loginIdField;
        
        private string userIdField;
        
        private decimal warehouseNumberField;
        
        private decimal legalEntityField;
        
        private bool legalEntityFieldSpecified;
        
        private string userNameField;
        
        private string employNumberField;
        
        private string orgUnitField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string LoginId {
            get {
                return this.loginIdField;
            }
            set {
                this.loginIdField = value;
                this.RaisePropertyChanged("LoginId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
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
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public decimal WarehouseNumber {
            get {
                return this.warehouseNumberField;
            }
            set {
                this.warehouseNumberField = value;
                this.RaisePropertyChanged("WarehouseNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public decimal LegalEntity {
            get {
                return this.legalEntityField;
            }
            set {
                this.legalEntityField = value;
                this.RaisePropertyChanged("LegalEntity");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LegalEntitySpecified {
            get {
                return this.legalEntityFieldSpecified;
            }
            set {
                this.legalEntityFieldSpecified = value;
                this.RaisePropertyChanged("LegalEntitySpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
                this.RaisePropertyChanged("UserName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string EmployNumber {
            get {
                return this.employNumberField;
            }
            set {
                this.employNumberField = value;
                this.RaisePropertyChanged("EmployNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string OrgUnit {
            get {
                return this.orgUnitField;
            }
            set {
                this.orgUnitField = value;
                this.RaisePropertyChanged("OrgUnit");
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetUsers", WrapperNamespace="http://webservices.im.se/ocp/GetUsers", IsWrapped=true)]
    public partial class GetUsersRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://webservices.im.se/ocp/GetUsers", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("User")]
        public Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersUser[] User;
        
        public GetUsersRequest() {
        }
        
        public GetUsersRequest(Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersUser[] User) {
            this.User = User;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Response", WrapperNamespace="http://webservices.im.se/ocp/GetUsers", IsWrapped=true)]
    public partial class GetUsersResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://webservices.im.se/ocp/GetUsers", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("Failure", typeof(ResponseFailure))]
        [System.Xml.Serialization.XmlElementAttribute("Success", typeof(ResponseSuccess))]
        public object[] Items;
        
        public GetUsersResponse() {
        }
        
        public GetUsersResponse(object[] Items) {
            this.Items = Items;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GetUsersServiceChannel : Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetUsersServiceClient : System.ServiceModel.ClientBase<Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersService>, Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersService {
        
        public GetUsersServiceClient() {
        }
        
        public GetUsersServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GetUsersServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetUsersServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GetUsersServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersResponse Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersService.GetUsers(Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersRequest request) {
            return base.Channel.GetUsers(request);
        }
        
        public object[] GetUsers(Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersUser[] User) {
            Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersRequest inValue = new Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersRequest();
            inValue.User = User;
            Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersResponse retVal = ((Imi.SupplyChain.Services.OrderManagement.Users.ServiceImplementation.GetUsersServiceReal.GetUsersService)(this)).GetUsers(inValue);
            return retVal.Items;
        }
    }
}