using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Xml.Serialization;
using System.Xml;

using WebServiceTester.TransportationPortal;

namespace WebServiceTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (TransportationPortalWebService ops = new TransportationPortalWebService())
            {
                textBox3.Text = "call";
                ops.Url = textBox1.Text;
                string DepartureId = textBox2.Text;

                FindDepartureRouteResult r = ops.FindDepartureRoute(null, null, DepartureId);
                textBox3.Text = "null";
                if (r != null)
                {
                    string s = InterfaceClassToXml(r);
                    textBox3.Text = s;
                }
            }
        }

        public string InterfaceClassToXml(FindDepartureRouteResult interfaceClass)
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

        private String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
    }
}
