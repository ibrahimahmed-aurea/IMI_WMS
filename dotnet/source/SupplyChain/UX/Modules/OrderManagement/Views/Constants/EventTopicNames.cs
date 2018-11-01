using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Modules.OrderManagement.Views.Constants
{
    public class EventTopicNames
    {
        public const string ShowChooseUserDialog = "event://Imi.SupplyChain.UX.Modules.OrderManagement.Views.ChooseUser/ShowDialog";
        public const string ShowOMSUserInfoPopup = "event://Imi.SupplyChain.UX.Modules.OrderManagement.Views.UserInfo/ShowDialog";
        public const string StartOMSProgram = "event://Imi.SupplyChain.UX.Modules.OrderManagement.Views/StartOrderProgram";
        public const string StartOMSProgramWithData = "event://Imi.SupplyChain.UX.Modules.OrderManagement.Views/StartOrderProgramWithData";
        public const string StartWebApp = "event://Imi.SupplyChain.UX.Modules.OrderManagement.Views.WebView/StartWebApp";
    }
}
