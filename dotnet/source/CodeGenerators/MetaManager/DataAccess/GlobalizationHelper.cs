using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using NHibernate.Proxy;

namespace Cdc.MetaManager.DataAccess
{
    public class GlobalizationHelper
    {
        private GlobalizationHelper()
        { 
        }

        public static string GetPropertyValue(ILocalizable target, string propertyName)
        {
            PropertyInfo info = target.GetType().GetProperty(propertyName);
            return info.GetValue(target, null) as string;
        }

        public static string GetResourceId(ILocalizable target, string propertyName)
        {
            PropertyInfo info = target.GetType().GetProperty(propertyName);
            Attribute attribute = Attribute.GetCustomAttribute(info, typeof(LocalizableAttribute), true);
            
            if (attribute != null)
            {
                return FormatResourceId(target, info.Name);
            }
            else
                throw new ArgumentException("The specified property is not localizable.", "propertyName");
        }
        
        public static void ExtractResources(ILocalizable target, IDictionary<string, string> resourceDictionary)
        {
            foreach (PropertyInfo info in target.GetType().GetProperties())
            {
                Attribute attribute = Attribute.GetCustomAttribute(info, typeof(LocalizableAttribute), true);

                if (attribute != null)
                {
                    string key = FormatResourceId(target, info.Name);
                    string value = info.GetValue(target, null) as string;
                    
                    if (!resourceDictionary.ContainsKey(key))
                        resourceDictionary.Add(key, value);
                }
            }
        }
        
        public static string FormatResourceId(ILocalizable target, string propertyName)
        {
            string resourceId = string.Format("str_{0}_{1}", target.MetaId, propertyName);
            resourceId = resourceId.Replace('-', '_');
            return resourceId;
        }

        public static string GetHintResourceId(UXComponent component)
        {
            if (component.Hint != null)
                return string.Format("hint_{0}", component.Hint.Id.ToString().Replace('-','_'));
            else 
                return null;
        }
    }
}
