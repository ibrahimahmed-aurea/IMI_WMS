using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AuthorizationParser.Models
{
    public class DialogXMLModel
    {
        public Dictionary<string, XmlDocument> Documents { get; set; }

        public DialogXMLModel()
        {
            Documents = new Dictionary<string, XmlDocument>();
        }
    }
}
