using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using Imi.Framework.UX.Services;
using Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.DataContracts;
using Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.ServiceContracts;
using Imi.SupplyChain.Services.OrderManagement.Menu.DataContracts;
using Imi.SupplyChain.Services.OrderManagement.Menu.ServiceContracts;
using Imi.SupplyChain.Services.OrderManagement.Users.DataContracts;
using Imi.SupplyChain.Services.OrderManagement.Users.ServiceContracts;
using Imi.SupplyChain.Services.OrderManagement.OTP.ServiceContracts;
using Imi.SupplyChain.UX.Infrastructure.Services;
using Imi.SupplyChain.UX.Modules.OrderManagement.Configuration;
using Imi.SupplyChain.UX.Modules.OrderManagement.Views.Constants;
using Microsoft.Practices.CompositeUI;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Services
{
    public class AOMWebServiceWrapper : IAOMWebServiceWrapper
    {
        [ServiceDependency]
        public IUserSessionService UserSessionService { get; set; }
        [ServiceDependency]
        public IOMSSessionContext omsSessionContext { get; set; }

        [WcfServiceDependency]
        public IGetGuiconfigurationService GetGuiconfigurationService { get; set; }

        [WcfServiceDependency]
        public IGetMenuService GetMenuService { get; set; }

        [WcfServiceDependency]
        public IGetUsersService GetUsersService { get; set; }

        [WcfServiceDependency]
        public IGetOTPService GetOTPService { get; set; }

        public XmlDocument GetMenu()
        {
            GetMenuUSER user = new GetMenuUSER();
            OMSConfigurationSection config = ConfigurationManager.GetSection("imi.supplychain.ux.modules.ordermanagement") as OMSConfigurationSection;
            if (config.SendDomainUserId)
                user.LoginId = UserSessionService.DomainUser;
            else
                user.LoginId = UserSessionService.UserId;
            user.Language = omsSessionContext.OMSLanguageCode;
            Imi.SupplyChain.Services.OrderManagement.Menu.DataContracts.Response[] orderMenu =
                GetMenuService.GetMenu(new GetMenuUSER[] { user }, config.SendDomainUserId);
            XmlDocument menu = TransformOrderMenuXml(orderMenu);
            //Debug.WriteLine(menu.OuterXml);
            return menu;
        }

        private XmlDocument TransformOrderMenuXml(Imi.SupplyChain.Services.OrderManagement.Menu.DataContracts.Response[] menuItems)
        {
            if (menuItems.Count() > 0)
            {
                if (!menuItems[0].Success)
                {
                    throw new Exception(menuItems[0].ErrorText);
                }
            }
            
            // transform from OMS xml format to SCC xml format
            IList<FolderXmlElement> processedFolders = new List<FolderXmlElement>();

            XmlDocument document = new XmlDocument();
            XmlElement rootFolder = document.CreateElement("folder");
            rootFolder.SetAttribute("caption", Resources.RootMenuItem_text);
            document.AppendChild(rootFolder);

            foreach (Imi.SupplyChain.Services.OrderManagement.Menu.DataContracts.Response menuItem in menuItems)
            {
                if (menuItem.MenuLineType.Equals("M")) // Folder
                {
                    XmlElement folder = document.CreateElement("folder");
                    folder.SetAttribute("caption", string.Format("{0} - {1}", menuItem.MenuId, menuItem.Description));
                    processedFolders.Add(new FolderXmlElement(menuItem.MenuId, folder));
                    bool parentFolderFound = false;

                    foreach (FolderXmlElement oldFolder in processedFolders.Reverse<FolderXmlElement>())
                    {
                        if (menuItem.MenuId.Length > oldFolder.key.Length)
                        {
                            oldFolder.value.AppendChild(folder);
                            parentFolderFound = true;
                            break;
                        }
                    }
                    if (!parentFolderFound)
                        rootFolder.AppendChild(folder);
                }

                if (menuItem.MenuLineType.Equals("T")) // Program
                {
                    XmlElement item = document.CreateElement("item");
                    item.SetAttribute("caption", string.Format("{0} - {1}", menuItem.MenuId, menuItem.Description));
                    item.SetAttribute("topicIdentity", EventTopicNames.StartOMSProgram);
                    item.SetAttribute("parameters", menuItem.MenuId);
                    foreach (FolderXmlElement oldFolder in processedFolders.Reverse<FolderXmlElement>())
                    {
                        if (menuItem.MenuId.Length > oldFolder.key.Length)
                        {
                            oldFolder.value.AppendChild(item);
                            break;
                        }
                    }
                }

                if (menuItem.MenuLineType.Equals("W")) // Program
                {
                    XmlElement item = document.CreateElement("item");
                    item.SetAttribute("caption", string.Format("{0} - {1}", menuItem.MenuId, menuItem.Description));
                    item.SetAttribute("topicIdentity", EventTopicNames.StartWebApp);
                    item.SetAttribute("parameters", menuItem.MenuId);
                    foreach (FolderXmlElement oldFolder in processedFolders.Reverse<FolderXmlElement>())
                    {
                        if (menuItem.MenuId.Length > oldFolder.key.Length)
                        {
                            oldFolder.value.AppendChild(item);
                            break;
                        }
                    }
                }

           }

            // append Menu for ChangeUser
            if (omsSessionContext.OMSUsersList.Count > 1)
            {
                XmlElement changeUser = document.CreateElement("item");
                changeUser.SetAttribute("caption", Resources.ChooseOMSUserMenu_text);
                changeUser.SetAttribute("id", "");
                changeUser.SetAttribute("topicIdentity", EventTopicNames.ShowChooseUserDialog);
                rootFolder.AppendChild(changeUser);
            }

            // append Menu for Show OMS Context
            XmlElement showOMSContext = document.CreateElement("item");
            showOMSContext.SetAttribute("caption", Resources.OMSUserInfo_label);
            showOMSContext.SetAttribute("id", "");
            showOMSContext.SetAttribute("topicIdentity", EventTopicNames.ShowOMSUserInfoPopup);
            rootFolder.AppendChild(showOMSContext);

            return document;
        }

        private class FolderXmlElement
        {
            public FolderXmlElement(string pKey, XmlElement pValue)
            {
                key = pKey;
                value = pValue;
            }
            public XmlElement value;
            public string key;
        }

        public IList<string> GetUsers()
        {   
            GetUsersUser user = new GetUsersUser();
            OMSConfigurationSection config = ConfigurationManager.GetSection("imi.supplychain.ux.modules.ordermanagement") as OMSConfigurationSection;
            if (config.SendDomainUserId)
                user.LoginId = UserSessionService.DomainUser;
            else
                user.LoginId = UserSessionService.UserId;

            Imi.SupplyChain.Services.OrderManagement.Users.DataContracts.Response[] responseItems =
                GetUsersService.GetUsers(new GetUsersUser[] { user }, config.SendDomainUserId);
            IList<string> userList = TransformUsers(responseItems);
            return userList;
        }

        public IList<string> TransformUsers(Imi.SupplyChain.Services.OrderManagement.Users.DataContracts.Response[] responseItems)
        {
            if (responseItems.Count() > 0)
            {
                if (!responseItems[0].Success)
                {
                    throw new Exception(responseItems[0].ErrorText);
                }
            }


            IList<string> userList = new List<string>();
            Dictionary<string, OMSLogicalUserData> omsUsersData = new Dictionary<string, OMSLogicalUserData>();
            OMSLogicalUserData userData = null;
            foreach (Imi.SupplyChain.Services.OrderManagement.Users.DataContracts.Response userEntry in responseItems)
            {
                if (userEntry.Success)
                {
                    userList.Add(userEntry.UserId);
                    userData = new OMSLogicalUserData(userEntry.LoginId, userEntry.UserId, userEntry.WarehouseNumber,
                            userEntry.LegalEntity, userEntry.UserName, userEntry.OrgUnit, userEntry.EmployNumber);
                    omsUsersData.Add(userData.omsLogicalUserId, userData);
                }
            }
            // add user data to context
            omsSessionContext.OMSUsersList = omsUsersData;
            if (String.IsNullOrEmpty(omsSessionContext.OMSLogicalUserId))
            {
                omsSessionContext.OMSLogicalUserId = omsUsersData.First().Key;
            }
            return userList;
        }

        public void GetGuiConfiguration()
        {
            GetGuiconfigurationUSER user = new GetGuiconfigurationUSER();
            user.UserId = omsSessionContext.OMSLogicalUserId;
            user.LoginId = omsSessionContext.OMSLoginUserId;
            OMSConfigurationSection config = ConfigurationManager.GetSection("imi.supplychain.ux.modules.ordermanagement") as OMSConfigurationSection;
            user.PortNumber = config.HostPort;
            Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.DataContracts.Response[] responseItems =
                GetGuiconfigurationService.GetGuiconfiguration(new GetGuiconfigurationUSER[] { user }, config.SendDomainUserId);

            if (responseItems.Count() > 0)
            {
                Imi.SupplyChain.Services.OrderManagement.Guiconfiguration.DataContracts.Response userConfig = responseItems[0];
                if (userConfig.Success)
                {
                    omsSessionContext.AutoStart = userConfig.auto_start;
                    omsSessionContext.ClientProgram = userConfig.clientprogram;
                    omsSessionContext.EnvironmentVariables = userConfig.env_vars;
                    omsSessionContext.Host = userConfig.host;
                    omsSessionContext.OMSLanguageCode = userConfig.language;
                    omsSessionContext.Parameters = userConfig.parameters;
                    omsSessionContext.Port = userConfig.port;
                    omsSessionContext.ServerProgram = userConfig.program;
                    omsSessionContext.ServerWorkingDirectory = userConfig.working_directory;
                    omsSessionContext.SystemName = userConfig.systemname;
                    omsSessionContext.HelpUrl = userConfig.help_url;
                    omsSessionContext.DecimalKey = userConfig.decimal_key;
                }
                else
                {   
                    throw new Exception(userConfig.ErrorText);
                }
            }
        }

        public string GetAWSOneTimePassword(string logicalUserID)
        {
            string oneTimePassword = GetOTPService.GetOTP(logicalUserID);
            return oneTimePassword;
        }
    }
}
