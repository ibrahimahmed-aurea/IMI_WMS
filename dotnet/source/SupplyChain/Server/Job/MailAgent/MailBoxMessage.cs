using System;
using System.Net.Mail;
using Imi.SupplyChain.Server.Job.MailAgent.Config;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using Imi.Framework.Shared.Diagnostics;
using Imi.Framework.Shared.IO;
using System.Configuration;
using Imi.Framework.Shared.Xml;
using System.Text;

namespace Imi.SupplyChain.Server.Job.MailAgent
{
    /// <summary>
    /// Summary description for MailBoxMessage.
    /// </summary>
    public class MailBoxMessage
    {
        private string mailId;
        private string xmlMessageBody;

        public MailBoxMessage()
        {
        }

        public string MailId
        {
            get { return mailId; }
            set { mailId = value; }
        }

        public string XmlMessageBody
        {
            get { return xmlMessageBody; }
            set { xmlMessageBody = value; }
        }

    }
}
