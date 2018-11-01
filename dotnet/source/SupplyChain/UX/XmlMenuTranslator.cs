using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Resources;

namespace Imi.SupplyChain.UX
{
    public class XmlMenuTranslator
    {
        private XmlMenuTranslator()
        { 
        }

        public static void Translate(XmlDocument document, ResourceManager resourceManager)
        {
            TranslateNode(document.DocumentElement, resourceManager);
        }

        private static void TranslateNode(XmlNode node, ResourceManager resourceManager)
        {
            if (node.Attributes["id"] != null)
            {
                string resourceId = string.Format("str_{0}_Caption", node.Attributes["id"].Value).Replace('-', '_');
                string caption = resourceManager.GetString(resourceId);
                
                if (!string.IsNullOrEmpty(caption))
                    node.Attributes["caption"].Value = caption;
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                TranslateNode(childNode, resourceManager);
            }
        }
    }
}
