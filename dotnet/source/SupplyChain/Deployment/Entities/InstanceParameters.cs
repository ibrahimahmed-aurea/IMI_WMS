using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Imi.SupplyChain.Deployment.Entities
{
    public class InstanceParameters : SerializableDictionary<string, string>
    {
        public override string ToString()
        {
            string temp = string.Empty;

            foreach (string key in Keys)
            {
                temp = string.Concat(temp, string.Format("&{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(this[key])));
            }

            return temp;
        }
    }
}
