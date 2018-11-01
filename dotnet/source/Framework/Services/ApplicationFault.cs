using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.Framework.Services
{
    [DataContract(Namespace = "http://Imi.Framework.Services/2011/09", Name = "ApplicationFault")]
    public partial class ApplicationFault
    {
        private Guid id;
        private int? position;
        private string message;
        private string errorCode;
        private string additionalInformation;
        /// <summary>
        /// Gets the system defined message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember(IsRequired = false, Name = "Message", Order = 2)]
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

        [DataMember(IsRequired = false, Name = "ErrorCode", Order = 0)]
        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
            set
            {
                errorCode = value;
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

        [DataMember(IsRequired = false, Name = "Position", Order = 3)]
        public int? Position
        {
            get { return position; }
            set { position = value; }
        }

        [DataMember(IsRequired = false, Name = "AdditionalMessage", Order = 4)]
        public string AdditionalInformation
        {
            get { return additionalInformation; }
            set { additionalInformation = value; }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(AdditionalInformation))
            {
                return base.ToString() + " - " + AdditionalInformation;
            }
            return base.ToString();
        }
    }
}
