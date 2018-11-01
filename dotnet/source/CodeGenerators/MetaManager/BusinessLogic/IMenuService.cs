using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.BusinessLogic
{
    public interface IMenuService
    {
        Menu GetMenuByApplicationId(Guid applicationId);
        Menu GetMenuForCodeGeneration(Guid applicationId);
        Menu SaveMenu(Menu menu);
        MenuItem GetMenuItemById(Guid menuItemId);
        MenuItem SaveMenuItem(MenuItem menuItem);
        IList<MenuItem> FindAllMenuItemsByActionId(Guid actionId);
        void DeleteMenuItem(MenuItem menuItem);
        void MoveUp(Guid moverMenuItemId, Guid staticMenuItemId, out MenuItem moverMenuItem, out MenuItem staticMenuItem);
    }
}
