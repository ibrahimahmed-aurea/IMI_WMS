using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Imi.Framework.Messaging.Configuration
{

    /// <summary>
    /// Framework configuration Section
    /// </summary>
    public class MessagingSection : ConfigurationSection
    {
        public const string SectionKey = "imi.framework.messaging";
        /// <summary>
        /// MessagingSector default constructor
        /// </summary>
        public MessagingSection()
        {
        }

        /// <summary>
        /// PedingOperationsTimeout - timeout value in milliseconds
        /// </summary>
        [ConfigurationProperty("pendingOperationsTimeout", IsRequired = true)]
        public int PendingOperationsTimeout
        {
            get
            { return (int)this["pendingOperationsTimeout"]; }
            set
            { this["pendingOperationsTimeout"] = value; }
        }

        /// <summary>
        /// DeadlockTimeout - timeout value in milliseconds
        /// </summary>
        [ConfigurationProperty("deadlockTimeout", IsRequired = true)]
        public int DeadlockTimeout
        {
            get
            { return (int)this["deadlockTimeout"]; }
            set
            { this["deadlockTimeout"] = value; }
        }

    }

}