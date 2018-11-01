using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Imi.Framework.Services
{   
    [MessageContract]
    public class RequestMessageBase
    {
        public const string HeaderNamespace = "http://Imi.Framework.Services/2011/09";

        private string terminalId;
        private string userId;
        private string uiCulture;
        private string sessionId;

        protected RequestMessageBase()
        { 
        }

        [MessageHeader(
         Name = "TerminalId",
         Namespace = HeaderNamespace,
         MustUnderstand = true)]
        public string TerminalId
        {
            get
            {
                return terminalId;
            }
            set
            {
                terminalId = value;
            }
        }

        [MessageHeader(
          Name = "UserId",
          Namespace = HeaderNamespace,
          MustUnderstand = true)]
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        [MessageHeader(
          Name = "UICulture",
          Namespace = HeaderNamespace,
          MustUnderstand = true)]
        public string UICulture
        {
            get
            {
                return uiCulture;
            }
            set
            {
                uiCulture = value;
            }
        }

        [MessageHeader(
          Name = "SessionId",
          Namespace = HeaderNamespace,
          MustUnderstand = true)]
        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                sessionId = value;
            }
        }
    }
}
