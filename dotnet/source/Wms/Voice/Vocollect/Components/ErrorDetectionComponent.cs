using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using Imi.Framework.Integration.Engine;
using Imi.Framework.Integration.Adapter;

namespace Imi.Wms.Voice.Vocollect.Components
{
    /// <summary>
    /// Component for detecting transmission errors
    /// </summary>
    [Persistence(PersistenceMode.EndPoint)]
    public class ErrorDetectionComponent : BaseComponent
    {

        private Dictionary<string, DateTime> timeStampDictionary;

        public ErrorDetectionComponent(BasePropertyCollection configuration)
            : base(configuration)
        {
            timeStampDictionary = new Dictionary<string, DateTime>();
        }
                
        public override Collection<BaseMessage> Invoke(BaseMessage msg)
        {

            if (!IsValid(msg))
            {
                if (MessageEngine.Instance.Tracing.TraceWarning)
                    Trace.WriteLine("Message invalidated: possibly duplicate or out of sequence");

                return null;
            }    
            
            Collection<BaseMessage> resultCollection = new Collection<BaseMessage>();
            resultCollection.Add(msg);

            return resultCollection;
        }

        private bool IsValid(BaseMessage msg)
        { 
            string serialNumber = (string)msg.Properties.Read("SerialNumber");
            DateTime timeStamp = (DateTime)msg.Properties.Read("TimeStamp");
            
            if (!timeStampDictionary.ContainsKey(serialNumber))
            {
                lock (timeStampDictionary)
                {
                    timeStampDictionary.Add(serialNumber, timeStamp);
                }

                return true;
            }
            else
            {                
                if (timeStamp.CompareTo(timeStampDictionary[serialNumber]) > 0)
                    return true;
                else
                    return false;
            }
            

        }

        public override bool Supports(BaseMessage msg)
        {
            if (msg.MessageType == "http://www.im.se/wms/voice/vocollect/xml_param_base")
                return true;
            else
                return false;
        }

        
    }
}
