using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using Wms.WebServices.OutboundMapper.Configuration;
using System.IO;

namespace Wms.WebServices.OutboundMapper.ReceiveAndForward
{
    public class ForwardHelper
    {
        string debugTransaction;
        string debugMessages;
        string transactionFile;
        string directory;
        string forwarder;
        string userdata;

        string channelId;
        string channelRef;
        string transactionId;
        string HAPIObjectName;

        IForwarder forwarderClass = null;

        public void CreateContext(string channelId, string channelRef, string transactionId, string HAPIObjectName)
        {
            this.channelId = channelId;
            this.channelRef = channelRef;
            this.transactionId = transactionId;
            this.HAPIObjectName = HAPIObjectName;

            try
            {
                GetWebConfig(channelId, out forwarder, out debugTransaction, out debugMessages, out transactionFile, out directory, out userdata);
            }
            catch (Exception e)
            {
                Exception WebConfigError = new Exception("WebConfigError: File format error", e);
                throw (WebConfigError);
            }

            if (string.IsNullOrEmpty(debugTransaction))
            {
                Exception WebConfigContentsMissing = new Exception("WebConfigError: No entry matches ChannelId or entry incomplete");
                throw (WebConfigContentsMissing);
            }

            LogTransaction("Enter");

            switch (forwarder)
            {
                case "NullForwarder":
                    {
                        forwarderClass = new NullForwarder();
                    }
                    break;
                default:
                    throw new Exception("Unknown forwarder: " + forwarder);
                    break;
            }

            if (forwarderClass != null)
                forwarderClass.CreateContext(userdata, HAPIObjectName);
        
        }

        private void LogTransaction(string message)
        {
            if (debugTransaction.ToLower() == "true")
            {
                string path = transactionFile;

                using (StreamWriter w = File.AppendText(path))
                {
                    w.Write("{0} {1}.{2}",
                        System.DateTime.Now.ToShortDateString(),
                        System.DateTime.Now.ToLongTimeString(),
                        System.Convert.ToString(System.DateTime.Now.Millisecond));

                    w.Write("{0}{1}{2}{3}{4}{5} ", '\t', transactionId, '\t', HAPIObjectName, '\t', forwarder);

                    w.WriteLine(message);
                }
            }
        }
        
        public void Forward(IInterfaceClass data)
        {
            // debug the messages
            if (debugMessages.ToLower() == "true")
            {
                try
                {
                    string filepath = directory + @"\" + transactionId + "-" + HAPIObjectName + ".xml";

                    XMLHelper xmlHelper = new XMLHelper();
                    string xml = xmlHelper.InterfaceClassToXml(data);
                    xmlHelper.DumpToFile(xml, filepath);
                }
                catch
                {
                    // ignore if tracing faults
                }

            }

            if (forwarderClass != null)
            {
                LogTransaction("Processing");
                try
                {
                    forwarderClass.Forward(data);
                }
                catch (Exception e)
                {
                    LogTransaction("Error: " + e.Message);
                    throw e;
                }
            }
        }

        public void Abort()
        {
            LogTransaction("Aborting");
        }

        public void ReleaseContext()
        {
            if (forwarderClass != null)
                forwarderClass.ReleaseContext();
            LogTransaction("Exit");
        }

        private void GetWebConfig(
            string communicationChannelId,
            out string forwarder,
            out string debugTransaction,
            out string debugMessages,
            out string transactionFile,
            out string directory,
            out string userdata)
        {
            forwarder = "";
            debugTransaction = "";
            debugMessages = "";
            transactionFile = "";
            directory = "";
            userdata = "";

            OutboundMapperSection config = ConfigurationManager.GetSection(OutboundMapperSection.SectionKey) as OutboundMapperSection;

            if (config != null)
            {
                foreach (CommunicationChannelElement pt in config.CommunicationChannelList)
                {
                    // Find default
                    if (pt.Name == "*")
                    {
                        forwarder = pt.Forwarder;
                        debugTransaction = pt.DebugTransaction;
                        debugMessages = pt.DebugMessages;
                        transactionFile = pt.TransactionFile;
                        directory = pt.Directory;
                        userdata = pt.Userdata;
                        break;
                    }

                    // If a specific is specified
                    if (pt.Name == communicationChannelId)
                    {
                        forwarder = pt.Forwarder;
                        debugTransaction = pt.DebugTransaction;
                        debugMessages = pt.DebugMessages;
                        transactionFile = pt.TransactionFile;
                        directory = pt.Directory;
                        userdata = pt.Userdata;
                        break;
                    }
                }
            }

            return;
        }
    }
}
