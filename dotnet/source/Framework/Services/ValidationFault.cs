using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Imi.Framework.Services
{
    public enum ValidationError
    { 
        NullValue,
        InvalidValue
    }

    [DataContract(Namespace = "http://Imi.Framework.Services/2011/09", Name = "ValidationFault")]
    public partial class ValidationFault
    {
        private Guid id;
        private string message;
        private string parameterName;
        private ValidationError errorType;

        [DataMember(IsRequired = false, Name = "ValidationError", Order = 3)]
        public ValidationError ValidationError 
        {
            get
            {
                return errorType;
            }
            set
            {
                errorType = value;
            }
        }
        
        [DataMember(IsRequired = false, Name = "ParameterName", Order = 2)]
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                parameterName = value;
            }
        }

        /// <summary>
        /// Gets the system defined message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember(IsRequired = false, Name = "Message", Order = 1)]
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
        [DataMember(IsRequired = false, Name = "Id", Order = 0)]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
