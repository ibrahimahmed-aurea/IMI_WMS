using System;
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Xml.Serialization;
using Imi.Framework.Shared.IO;
using Imi.Framework.Shared.Xml;
using Imi.SupplyChain.Server.Job.MailAgent.Config;

namespace Imi.SupplyChain.Server.Job.MailAgent
{
    class AppMailMessage
    {
        private const String defaultNameSpace = "Imi.SupplyChain.Server.Job.MailAgent";
        private EmailMessage emailMessage;
        private string       sender;
        private const string disclaimer =
            "\n\n" +
            "The information transmitted is intended only for the person or entity to which it is addressed\n" +
            "and may contain confidential and/or privileged material. Any review, retransmission, dissemination\n" +
            "or other use of, or taking of any action in reliance upon, this information by persons or entities\n" +
            "other than the intended recipient is prohibited.\n" +
            "If you received this in error, please contact mailto:{0} and delete the material from any computer.\n";

        public AppMailMessage(string sender, string xmlMessage)
        {
            emailMessage = null;

            if (sender == null)
            {
                sender = "unKnown";
            }

            this.sender = sender;

            if (xmlMessage == null)
            {
                xmlMessage = "";
            }

            XmlSerializer s = new XmlSerializer(typeof(EmailMessage));
            StringReader  r = new StringReader(xmlMessage);

            try
            {
                emailMessage = s.Deserialize(r) as EmailMessage;

                if (emailMessage == null)
                {
                    throw (new ConfigurationErrorsException("Message is empty."));
                }

                if (emailMessage.MailList == null)
                {
                    throw (new ConfigurationErrorsException("Message is missing receipient information."));
                }

                if (emailMessage.Message == null)
                {
                    throw (new ConfigurationErrorsException("Message content is missing."));
                }

                if (emailMessage.Message.Subject == null)
                {
                    throw (new ConfigurationErrorsException("Message subject is missing."));
                }

                if (emailMessage.Message.Body == null)
                {
                    throw (new ConfigurationErrorsException("Message body is missing."));
                }

                if (emailMessage.Message.Body.Length == 0)
                {
                    throw (new ConfigurationErrorsException("Message body is empty."));
                }
            }
            catch (InvalidOperationException)
            {
                // Validate the contents
                String schemaFileName = String.Format("{0}.xsd.EmailMessage.xsd", defaultNameSpace);
                StreamReader schema;

                try
                {
                    schema = FileIO.GetFileFromResources(schemaFileName);
                }
                catch (ConfigurationErrorsException ex)
                {

                    throw (new ConfigurationErrorsException(
                      String.Format(
                      "Failed to load embedded schema file, cannot validate email message. Schema file = \"{0}\"", schemaFileName), ex));
                }

                // Validate file, get 10 errors max
                XmlValidator xv = new XmlValidator();
                String errors = xv.ValidateString(xmlMessage, schema.BaseStream, 10);

                if (errors != "")
                {
                    throw (new ConfigurationErrorsException(
                      String.Format(
                      "The email message contains syntax errors.\n{0}", errors)));
                }
            }
        }

        public string MessageBody
        {
            get { return GetMessageBody(); }
        }

        public string Subject
        {
            get { return emailMessage.Message.Subject; }
        }


        private string GetMessageBody()
        {
            StringBuilder body = new StringBuilder();

            foreach (String l in emailMessage.Message.Body)
            {
                if (body.Length > 0)
                    body.Append(Environment.NewLine);

                body.Append(l);
            }

            //
            // Add disclaimer
            //
            body.Append(String.Format(disclaimer, sender));

            return body.ToString();
        }

        public IList<string> ToList
        {
            get
            {
                IList<string> toList = new List<string>();
                foreach (MailToType m in emailMessage.MailList)
                {
                    toList.Add(m.Address);
                }
                return toList;
            }
        }

    }
}
