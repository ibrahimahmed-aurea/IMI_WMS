using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace Imi.Framework.UX.Services
{
    public class DecompressionOperationBehavior : IOperationBehavior
    {
        #region IOperationBehavior Members

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            // Set the default serializer to the customized serialization
            clientOperation.Formatter = new DecompressionMessageFormatter(clientOperation.Formatter,operationDescription.SyncMethod.ReturnType);
        }

        #region Not used
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }
        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
        }
        public void Validate(OperationDescription operationDescription)
        {
        }
        #endregion

        #endregion
    }

    public class DecompressionMessageFormatter : IClientMessageFormatter
    {
        bool _compression = false;
        IClientMessageFormatter _baseFormater;
        Type _returnType;

        public DecompressionMessageFormatter(IClientMessageFormatter baseFormater, Type returnType, bool compression = true)
        {
            _compression = compression;
            _baseFormater = baseFormater;
            _returnType = returnType;
        }

        #region IClientMessageFormatter Members

        public object DeserializeReply(System.ServiceModel.Channels.Message message, object[] parameters)
        {
            object returnObject = null;

            if (_compression && !message.IsEmpty)
            {
                byte[] messageContent = null;

                // Read the reply messages data (with a XmlDictionaryReader) as a base64binary and store in the byte array
                using (XmlDictionaryReader xdr = message.GetReaderAtBodyContents())
                {
                    xdr.ReadStartElement("base64Binary");
                    messageContent = xdr.ReadContentAsBase64();
                }

                MemoryStream contentStream = new MemoryStream();

                // Send the message content byte array and the memory stream for decompression
                Unzip(messageContent, ref contentStream);

                // Reset the streams inner pointer to the start of the stream for deserialization
                contentStream.Seek(0, SeekOrigin.Begin);

                XmlSerializer deserializer = new XmlSerializer(_returnType);

                // Deserialize the stream containing the decompressed reply message data into the return object
                returnObject = deserializer.Deserialize(contentStream);

                contentStream.Dispose();
            }
            else
            {
                // Used defualt deserialization
                returnObject = _baseFormater.DeserializeReply(message, parameters);
            }

            return returnObject;
        }

        public System.ServiceModel.Channels.Message SerializeRequest(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters)
        {
            return _baseFormater.SerializeRequest(messageVersion, parameters);
        }

        #endregion

        private void Unzip(byte[] bytes, ref MemoryStream contentStream)
        {
            // Create DeflateStream with decompression mode
            using (DeflateStream stream = new DeflateStream(new MemoryStream(bytes), CompressionMode.Decompress))
            {
                // Size of partition for decompression
                const int size = 4096;

                // Temp array to put decompression parititon in
                byte[] buffer = new byte[size];

                // Total number of bytes written into the array
                int count = 0;

                // Iter until no more bytes available for decompression
                do
                {
                    // Get the number of bytes written into the array
                    count = stream.Read(buffer, 0, size);

                    // As long as the number of written bytes are more then 0
                    // Decompress them into the memory stream
                    if (count > 0)
                    {
                        contentStream.Write(buffer, 0, count);
                    }
                }
                while (count > 0);
            }
        }
    }
}
