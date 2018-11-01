using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;

namespace Imi.SupplyChain.Services.IdentityModel.Configuration
{
    public class IdentityConfigurationSection : ConfigurationSection
    {
        public const string SectionKey = "imi.supplychain.identityModel";

        public IdentityConfigurationSection()
        {
        }

        [ConfigurationProperty("issuerName", IsRequired = true)]
        public string IssuerName
        {
            get
            {
                return (string)this["issuerName"];
            }
            set
            {
                this["issuerName"] = value;
            }
        }
                
        [ConfigurationProperty("signingCertificateName", IsRequired = true)]
        public string SigningCertificateName
        {
            get
            {
                return (string)this["signingCertificateName"];
            }
            set
            {
                this["signingCertificateName"] = value;
            }
        }

        [ConfigurationProperty("encryptingCertificateName", IsRequired = false)]
        public string EncryptingCertificateName
        {
            get
            {
                return (string)this["encryptingCertificateName"];
            }
            set
            {
                this["encryptingCertificateName"] = value;
            }
        }

        [ConfigurationProperty("tokenLifetime", IsRequired = true)]
        public string TokenLifetime
        {
            get
            {
                return (string)this["tokenLifetime"];
            }
            set
            {
                this["tokenLifetime"] = value;
            }
        }

        [ConfigurationProperty("yubicoSettings")]
        public YubicoConfigurationElement YubicoSettings
        {
            get
            {
                return (YubicoConfigurationElement)this["yubicoSettings"];
            }
            set
            {
                this["yubicoSettings"] = value;
            }
        }

        [ConfigurationProperty("directorySettings")]
        public DirectoryConfigurationElement DirectorySettings
        {
            get
            {
                return (DirectoryConfigurationElement)this["directorySettings"];
            }
            set
            {
                this["directorySettings"] = value;
            }
        }
    }

    public class DirectoryConfigurationElement : ConfigurationElement
    {
        public DirectoryConfigurationElement()
        {

        }

        [ConfigurationProperty("contextType")]
        public string ContextType
        {
            get
            {
                return (string)this["contextType"];
            }
            set
            {
                this["contextType"] = value;
            }
        }

        [ConfigurationProperty("contextName")]
        public string ContextName
        {
            get
            {
                return (string)this["contextName"];
            }
            set
            {
                this["contextName"] = value;
            }
        }

        [ConfigurationProperty("container")]
        public string Container
        {
            get
            {
                return (string)this["container"];
            }
            set
            {
                this["container"] = value;
            }
        }

        [ConfigurationProperty("contextOptions")]
        public string ContextOptions
        {
            get
            {
                return (string)this["contextOptions"];
            }
            set
            {
                this["contextOptions"] = value;
            }
        }

        [ConfigurationProperty("userName")]
        public string UserName
        {
            get
            {
                return (string)this["userName"];
            }
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }
    }

    public class ValidationServerCollection : ConfigurationElementCollection
    {
        public ValidationServerCollection()
        {

        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ValidationServerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ValidationServerElement)element).Name;
        }

        public ValidationServerElement this[int index]
        {
            get
            {
                return (ValidationServerElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ValidationServerElement this[string name]
        {
            get
            {
                return (ValidationServerElement)BaseGet(name);
            }
        }

        public int IndexOf(ValidationServerElement server)
        {
            return BaseIndexOf(server);
        }

        public void Add(ValidationServerElement server)
        {
            BaseAdd(server);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(ValidationServerElement server)
        {
            if (BaseIndexOf(server) >= 0)
                BaseRemove(server.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class YubicoConfigurationElement : ConfigurationElement
    {
        public YubicoConfigurationElement()
        {

        }

        [ConfigurationProperty("clientId")]
        public string ClientId
        {
            get
            {
                return (string)this["clientId"];
            }
            set
            {
                this["clientId"] = value;
            }
        }

        [ConfigurationProperty("apiKey")]
        public string APIKey
        {
            get
            {
                return (string)this["apiKey"];
            }
            set
            {
                this["apiKey"] = value;
            }
        }

        [ConfigurationProperty("publicIdLength", IsRequired = true)]
        public int PublicIdLength
        {
            get
            {
                return (int)this["publicIdLength"];
            }
            set
            {
                this["publicIdLength"] = value;
            }
        }

        [ConfigurationProperty("publicIdAttributeName")]
        public string PublicIdAttributeName
        {
            get
            {
                return (string)this["publicIdAttributeName"];
            }
            set
            {
                this["publicIdAttributeName"] = value;
            }
        }
        
        [ConfigurationProperty("validationServers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ValidationServerCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ValidationServerCollection ValidationServers
        {
            get
            {
                    return (ValidationServerCollection)base["validationServers"];
            }
        }

    }

    public class ValidationServerElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }
       
    }
}
