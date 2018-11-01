using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Wms.WebServices.OutboundMapper.ReceiveAndForward
{
    public class XMLHelper
    {
        public string InterfaceClassToXml(IInterfaceClass interfaceClass)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(interfaceClass.GetType());

            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding());
            xs.Serialize(xmlTextWriter, interfaceClass, ns);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            return UTF8ByteArrayToString(memoryStream.ToArray());
        }

        public void DumpToFile(string xml, string filePath)
        {
            UTF8Encoding utf8 = new UTF8Encoding();

            using (StreamWriter file = new StreamWriter(filePath, false, utf8))
            {
                file.Write(xml);
            }
        }

        public string ReadXMLFromFile(string filePath)
        {
            using (StreamReader file = new StreamReader(filePath))
            {
                return file.ReadToEnd();
            }
        }

        private String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}
