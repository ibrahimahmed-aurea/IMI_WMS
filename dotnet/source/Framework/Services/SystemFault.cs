using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Imi.Framework.Services
{
    /// <summary>
    /// Fault contract that will send all the unhandled exception to the client.
    /// </summary>
    [DataContract(Namespace = "http://Imi.Framework.Services/2011/09", Name = "SystemFault")]
    public partial class SystemFault
    {
        private Guid id;
        private string message;
        /// <summary>
        /// Gets the system defined message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember(IsRequired = false, Name = "Message", Order = 0)]
        public string Message
        {
            get 
            {
                return message;
            }
            set 
            {
                message = value;
            }
        }

        /// <summary>
        /// Gets or sets the error id.
        /// </summary>
        /// <value>The id.</value>
        [DataMember(IsRequired = false, Name = "Id", Order = 1)]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
