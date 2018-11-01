using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Shell
{
    public class EventTopicNames
    {
        public const string PrepareShutdown = "event://Imi.SupplyChain.UX.Shell/PrepareShutdown";
        public const string SettingsUpdated = "event://Imi.SupplyChain.UX.Shell/SettingsUpdated";
        public const string Close = "event://Imi.SupplyChain.UX.Shell/Close";
        public const string CloseAll = "event://Imi.SupplyChain.UX.Shell/CloseAll";
        public const string CloseAllWorkspaces = "event://Imi.SupplyChain.UX.Shell/CloseAllWorkspaces";
        public const string ShowSettingsDialog = "event://Imi.SupplyChain.UX.Shell/ShowSettingsDialog";
        public const string ShowZoomDialog = "event://Imi.SupplyChain.UX.Shell/ShowZoomDialog";
        public const string Help = "event://Imi.SupplyChain.UX.Shell/Help";
        public const string StartMenuItemExecuted = "event://Imi.SupplyChain.UX.Shell/ExecuteMenuItem";
        public const string ActionExecuted = "event://Imi.SupplyChain.UX.Shell/ExecuteAction";
        public const string ModuleActivated = "event://Imi.SupplyChain.UX.Shell/ModuleActivated";
        public const string ModuleActivating = "event://Imi.SupplyChain.UX.Shell/ModuleActivating";
        public const string ModuleLoaded = "event://Imi.SupplyChain.UX.Shell/ModuleLoaded";
        public const string ShowNotification = "event://Imi.SupplyChain.UX.Shell/ShowNotification";
        public const string FavoriteAdded = "event://Imi.SupplyChain.UX.Shell/FavoriteAdded";
        public const string DashboardRefresh = "event://Imi.SupplyChain.UX.Shell/Dashboard/Refresh";
        public const string DashboardShowDialog = "event://Imi.SupplyChain.UX.Shell/Dashboard/ShowDialog";
    }
}
