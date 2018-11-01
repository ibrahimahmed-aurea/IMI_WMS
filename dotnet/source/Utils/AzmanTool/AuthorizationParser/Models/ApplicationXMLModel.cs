using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthorizationParser.Models
{
    /// <summary>
    /// Class representing an application.
    /// </summary>
    public class ApplicationXMLModel
    {
        public string Name { get; set; }
        public UXActionXMLModel UXActionXMLDocuments { get; set; }
        public DialogXMLModel DialogXmlDocuments { get; set; }
        public MenuItemModel MenuItems { get; set; }

        public ApplicationXMLModel()
        {
            UXActionXMLDocuments = new UXActionXMLModel();
            DialogXmlDocuments =new DialogXMLModel();
            MenuItems = new MenuItemModel();
        } 
    }
}
