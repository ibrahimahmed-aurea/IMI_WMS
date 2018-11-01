using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

namespace Imi.Framework.Services
{
    /// <summary>
    /// Message fault contract that will send errors and warnings to the client.
    /// </summary>
    [DataContract(Namespace = "http://Imi.Framework.Services/2011/09", Name = "MessageFault")]
    public partial class MessageFault
    {
        private string messageXml;

        /// <summary>
        /// Gets the system defined message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember(IsRequired = true, Name = "MessageXML", Order = 0)]
        public string MessageXML
        {
            get
            {
                return messageXml;
            }
            set 
            {
                messageXml = value;
            }
        }

    }
}
