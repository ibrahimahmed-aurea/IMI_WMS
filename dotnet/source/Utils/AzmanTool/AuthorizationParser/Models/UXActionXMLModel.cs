using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AuthorizationParser.Models
{
    public class UXActionXMLModel
    {
        public Dictionary<string, XmlDocument> Documents { get; set; }

        public UXActionXMLModel()
        {
            Documents = new Dictionary<string, XmlDocument>();
        }
    }
}
