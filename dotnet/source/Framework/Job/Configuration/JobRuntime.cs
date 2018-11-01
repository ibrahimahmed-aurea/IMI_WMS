namespace Imi.Framework.Job.Configuration {
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("JobRuntime", Namespace="", IsNullable=false)]
    public partial class JobRuntimeType {
        
        private string nameField;
        
        private int threadIdField;
        
        private JobRuntimeTypeStatus statusField;
        
        private int runCountField;
        
        private DateTime runStartedField;
        
        private DateTime startedField;
        
        private DateTime stoppedField;
        
        private TimeSpan totalRealTimeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ThreadId {
            get {
                return this.threadIdField;
            }
            set {
                this.threadIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public JobRuntimeTypeStatus Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int RunCount {
            get {
                return this.runCountField;
            }
            set {
                this.runCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime RunStarted {
            get {
                return this.runStartedField;
            }
            set {
                this.runStartedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime Started {
            get {
                return this.startedField;
            }
            set {
                this.startedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DateTime Stopped {
            get {
                return this.stoppedField;
            }
            set {
                this.stoppedField = value;
            }
        }

        private static string FormatTimeSpan(TimeSpan ts)
        {
            try
            {
                long days = Convert.ToInt64(Math.Floor(ts.TotalDays));
                TimeSpan nu = ts.Subtract(new TimeSpan(days * TimeSpan.TicksPerDay));
                long hours = Convert.ToInt64(Math.Floor(nu.TotalHours));
                nu = nu.Subtract(new TimeSpan(hours * TimeSpan.TicksPerHour));
                string s = string.Format("{0}.{1:0#}:{2:0#}:{3:0#}.{4:0##}", days, hours, nu.Minutes, nu.Seconds, nu.Milliseconds);
                return s;
            }
            catch (Exception)
            {
                return ts.ToString();
            }
        }

        /// For serialization purposes only
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="TotalRealTime")]
        public string TotalRealTimeStr
        {
            get
            {
                return (FormatTimeSpan(this.totalRealTimeField));
            }
            set
            {
                this.totalRealTimeField = TimeSpan.Parse(value);
            }
        }

        [System.Xml.Serialization.XmlIgnore()]
        public TimeSpan TotalRealTime 
        {
            get {
                return this.totalRealTimeField;
            }
            set {
                this.totalRealTimeField = value;
            }
        }

    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum JobRuntimeTypeStatus {
        
        /// <remarks/>
        Init,
        
        /// <remarks/>
        Starting,
        
        /// <remarks/>
        Run,
        
        /// <remarks/>
        Wait,
        
        /// <remarks/>
        Stopping,
        
        /// <remarks/>
        Stopped,
    }
}
