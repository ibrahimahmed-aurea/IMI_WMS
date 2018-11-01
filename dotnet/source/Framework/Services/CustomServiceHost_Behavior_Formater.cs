using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.IO;
using System.IO.Compression;
using System.ServiceModel.Channels;
using System.Xml.Serialization;

namespace Imi.Framework.Services
{
    public class CompressionOperationBehavior : IOperationBehavior
    {
        bool _useCompression = true;

        public CompressionOperationBehavior(bool useCompression = true)
        {
            _useCompression = useCompression;
        }

        #region IOperationBehavior Members

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            dispatchOperation.Formatter = new CompressionMessageFormatter(dispatchOperation.Formatter, dispatchOperation.ReplyAction, _useCompression);
        }

        #region Not used
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
        }
        public void Validate(OperationDescription operationDescription)
        {
        }
        #endregion

        #endregion
    }

    public class CompressionMessageFormatter : IDispatchMessageFormatter
    {
        bool _compression = false;
        IDispatchMessageFormatter _baseFormater;
        string _replyAction;

        public CompressionMessageFormatter(IDispatchMessageFormatter baseFormater, string replyAction, bool compression = true)
        {
            _compression = compression;
            _baseFormater = baseFormater;
            _replyAction = replyAction;
        }

        #region IDispatchMessageFormatter Members

        public void DeserializeRequest(System.ServiceModel.Channels.Message message, object[] parameters)
        {
            _baseFormater.DeserializeRequest(message, parameters);
        }

        public System.ServiceModel.Channels.Message SerializeReply(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters, object result)
        {
            Message returnMessage = null;


            if (_compression && result != null)
            {
                XmlSerializer serializer = new XmlSerializer(result.GetType());

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.Serialize(ms, result);

                    // Compress MemoryStream data and create message based on messageversion and action
                    returnMessage = Message.CreateMessage(messageVersion, _replyAction, Zip(ms.ToArray()));
                }
            }
            else
            {
                // Used defualt serialization
                returnMessage = _baseFormater.SerializeReply(messageVersion, parameters, result);
            }


            return returnMessage;
        }

        #endregion


        private byte[] Zip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var stream = new DeflateStream(mso, System.IO.Compression.CompressionMode.Compress))
                {
                    msi.WriteTo(stream);
                }
                return mso.ToArray();
            }
        }
    }
}
