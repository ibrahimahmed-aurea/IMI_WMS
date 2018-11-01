﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.1433.
// 
#pragma warning disable 1591

namespace WebServiceTester.TransportationPortal {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="TransportationPortalWebServiceSoap", Namespace="http://im.se/webservices/transportationportal")]
    public partial class TransportationPortalWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback FindDepartureRouteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public TransportationPortalWebService() {
            this.Url = global::WebServiceTester.Properties.Settings.Default.WebServiceTester_TransportationPortal_TransportationPortalWebService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event FindDepartureRouteCompletedEventHandler FindDepartureRouteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://im.se/webservices/transportationportal/FindDepartureRoute", RequestNamespace="http://im.se/webservices/transportationportal", ResponseNamespace="http://im.se/webservices/transportationportal", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public FindDepartureRouteResult FindDepartureRoute(string ChannelId, string Language, string DepartureIdentity) {
            object[] results = this.Invoke("FindDepartureRoute", new object[] {
                        ChannelId,
                        Language,
                        DepartureIdentity});
            return ((FindDepartureRouteResult)(results[0]));
        }
        
        /// <remarks/>
        public void FindDepartureRouteAsync(string ChannelId, string Language, string DepartureIdentity) {
            this.FindDepartureRouteAsync(ChannelId, Language, DepartureIdentity, null);
        }
        
        /// <remarks/>
        public void FindDepartureRouteAsync(string ChannelId, string Language, string DepartureIdentity, object userState) {
            if ((this.FindDepartureRouteOperationCompleted == null)) {
                this.FindDepartureRouteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnFindDepartureRouteOperationCompleted);
            }
            this.InvokeAsync("FindDepartureRoute", new object[] {
                        ChannelId,
                        Language,
                        DepartureIdentity}, this.FindDepartureRouteOperationCompleted, userState);
        }
        
        private void OnFindDepartureRouteOperationCompleted(object arg) {
            if ((this.FindDepartureRouteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.FindDepartureRouteCompleted(this, new FindDepartureRouteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/webservices/transportationportal")]
    public partial class FindDepartureRouteResult {
        
        private Route departureRouteField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Route DepartureRoute {
            get {
                return this.departureRouteField;
            }
            set {
                this.departureRouteField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/webservices/transportationportal")]
    public partial class Route {
        
        private string vehicleIdentityField;
        
        private System.DateTime estimatedTimeOfDepartureField;
        
        private string routeIdentityField;
        
        private string nameField;
        
        private System.Nullable<int> totalDrivingTimeInSecondsField;
        
        private System.Nullable<int> totalUnloadingTimeInSecondsField;
        
        private System.Nullable<int> totalTimeInSecondsField;
        
        private System.Nullable<double> totalDistanceField;
        
        private RouteStop[] routeStopsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string VehicleIdentity {
            get {
                return this.vehicleIdentityField;
            }
            set {
                this.vehicleIdentityField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime EstimatedTimeOfDeparture {
            get {
                return this.estimatedTimeOfDepartureField;
            }
            set {
                this.estimatedTimeOfDepartureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string RouteIdentity {
            get {
                return this.routeIdentityField;
            }
            set {
                this.routeIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> TotalDrivingTimeInSeconds {
            get {
                return this.totalDrivingTimeInSecondsField;
            }
            set {
                this.totalDrivingTimeInSecondsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> TotalUnloadingTimeInSeconds {
            get {
                return this.totalUnloadingTimeInSecondsField;
            }
            set {
                this.totalUnloadingTimeInSecondsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> TotalTimeInSeconds {
            get {
                return this.totalTimeInSecondsField;
            }
            set {
                this.totalTimeInSecondsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<double> TotalDistance {
            get {
                return this.totalDistanceField;
            }
            set {
                this.totalDistanceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RouteStops", IsNullable=true)]
        public RouteStop[] RouteStops {
            get {
                return this.routeStopsField;
            }
            set {
                this.routeStopsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://im.se/webservices/transportationportal")]
    public partial class RouteStop {
        
        private string nodeIdentityField;
        
        private System.Nullable<int> stopSequenceField;
        
        private System.Nullable<double> distanceFromPreviousStopField;
        
        private System.DateTime estimatedArrivalTimeField;
        
        private System.Nullable<int> totalDrivingTimeInSecondsField;
        
        private System.Nullable<int> totalUnloadingTimeInSecondsField;
        
        private System.Nullable<int> totalTimeInSecondsField;
        
        private string customerIdentityField;
        
        private string name1Field;
        
        private string name2Field;
        
        private string name3Field;
        
        private string name4Field;
        
        private string name5Field;
        
        private string address1Field;
        
        private string address2Field;
        
        private string address3Field;
        
        private string address4Field;
        
        private string cityField;
        
        private string zipCodeField;
        
        private string regionField;
        
        private string countryCodeField;
        
        private string countryField;
        
        private string latitudeField;
        
        private string longitudeField;
        
        private string contactPersonField;
        
        private string contactPhoneField;
        
        private string contactEmailField;
        
        private string instructionsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string NodeIdentity {
            get {
                return this.nodeIdentityField;
            }
            set {
                this.nodeIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> StopSequence {
            get {
                return this.stopSequenceField;
            }
            set {
                this.stopSequenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<double> DistanceFromPreviousStop {
            get {
                return this.distanceFromPreviousStopField;
            }
            set {
                this.distanceFromPreviousStopField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime EstimatedArrivalTime {
            get {
                return this.estimatedArrivalTimeField;
            }
            set {
                this.estimatedArrivalTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> TotalDrivingTimeInSeconds {
            get {
                return this.totalDrivingTimeInSecondsField;
            }
            set {
                this.totalDrivingTimeInSecondsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> TotalUnloadingTimeInSeconds {
            get {
                return this.totalUnloadingTimeInSecondsField;
            }
            set {
                this.totalUnloadingTimeInSecondsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> TotalTimeInSeconds {
            get {
                return this.totalTimeInSecondsField;
            }
            set {
                this.totalTimeInSecondsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CustomerIdentity {
            get {
                return this.customerIdentityField;
            }
            set {
                this.customerIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Name1 {
            get {
                return this.name1Field;
            }
            set {
                this.name1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Name2 {
            get {
                return this.name2Field;
            }
            set {
                this.name2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Name3 {
            get {
                return this.name3Field;
            }
            set {
                this.name3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Name4 {
            get {
                return this.name4Field;
            }
            set {
                this.name4Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Name5 {
            get {
                return this.name5Field;
            }
            set {
                this.name5Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Address1 {
            get {
                return this.address1Field;
            }
            set {
                this.address1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Address2 {
            get {
                return this.address2Field;
            }
            set {
                this.address2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Address3 {
            get {
                return this.address3Field;
            }
            set {
                this.address3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Address4 {
            get {
                return this.address4Field;
            }
            set {
                this.address4Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ZipCode {
            get {
                return this.zipCodeField;
            }
            set {
                this.zipCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Region {
            get {
                return this.regionField;
            }
            set {
                this.regionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string CountryCode {
            get {
                return this.countryCodeField;
            }
            set {
                this.countryCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Latitude {
            get {
                return this.latitudeField;
            }
            set {
                this.latitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Longitude {
            get {
                return this.longitudeField;
            }
            set {
                this.longitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ContactPerson {
            get {
                return this.contactPersonField;
            }
            set {
                this.contactPersonField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ContactPhone {
            get {
                return this.contactPhoneField;
            }
            set {
                this.contactPhoneField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ContactEmail {
            get {
                return this.contactEmailField;
            }
            set {
                this.contactEmailField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Instructions {
            get {
                return this.instructionsField;
            }
            set {
                this.instructionsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void FindDepartureRouteCompletedEventHandler(object sender, FindDepartureRouteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class FindDepartureRouteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal FindDepartureRouteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public FindDepartureRouteResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((FindDepartureRouteResult)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591