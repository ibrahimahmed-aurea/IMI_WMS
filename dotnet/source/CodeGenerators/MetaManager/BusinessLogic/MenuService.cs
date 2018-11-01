using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Transaction.Interceptor;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;

namespace Cdc.MetaManager.BusinessLogic
{
    public class MenuService : IMenuService
    {
        public IMenuDao MenuDao { get; set; }
        public IMenuItemDao MenuItemDao { get; set; }

        [Transaction(ReadOnly = true)]
        public Menu GetMenuByApplicationId(Guid applicationId)
        {
            return MenuDao.FindByApplicationId(applicationId);
        }

        [Transaction(ReadOnly = true)]
        public Menu GetMenuForCodeGeneration(Guid applicationId)
        {
            return MenuDao.FindByApplicationId(applicationId);

            /*
            if (menu != null)
            {
                InitializeMenuItemTree(menu.TopMenuItem);
            }

            return menu;
            */
        }

        [Transaction(ReadOnly = true)]
        public IList<MenuItem> FindAllMenuItemsByActionId(Guid actionId)
        {
            return MenuItemDao.FindAllByActionId(actionId);
        }

        private void InitializeMenuItemTree(MenuItem menuItem)
        {
            if (menuItem.Children.Count > 0)
            {
                foreach (MenuItem childItem in menuItem.Children)
                {
                    InitializeMenuItemTree(childItem);
                }
            }

            NHibernateUtil.Initialize(menuItem.Menu);

            if (menuItem.Action != null)
            {
                NHibernateUtil.Initialize(menuItem.Action);

                if (menuItem.Action.Dialog != null)
                {
                    NHibernateUtil.Initialize(menuItem.Action.Dialog);
                    NHibernateUtil.Initialize(menuItem.Action.Dialog.Module);
                }
            }
        }

        [Transaction(ReadOnly = true)]
        public MenuItem GetMenuItemById(Guid menuItemId)
        {
            MenuItem menuItem = MenuItemDao.FindById(menuItemId);
            NHibernateUtil.Initialize(menuItem);
            NHibernateUtil.Initialize(menuItem.Action);
            
            if (menuItem.Action != null)
            {
                NHibernateUtil.Initialize(menuItem.Action.MappedToObject);
            }

            NHibernateUtil.Initialize(menuItem.Menu);
            NHibernateUtil.Initialize(menuItem.Parent);

            foreach (MenuItem child in menuItem.Children)
            {
                GetMenuItemById(child.Id);
            }

            return menuItem;
        }

        [Transaction(ReadOnly = false)]
        public MenuItem SaveMenuItem(MenuItem menuItem)
        {
            return MenuItemDao.SaveOrUpdate(menuItem);
        }

        [Transaction(ReadOnly = false)]
        public Menu SaveMenu(Menu menu)
        {
            return MenuDao.SaveOrUpdate(menu);
        }

        [Transaction(ReadOnly = false)]
        public void DeleteMenuItem(MenuItem menuItem)
        {
            MenuItemDao.Delete(menuItem);
        }

        [Transaction(ReadOnly = false)]
        public void MoveUp(Guid moverMenuItemId, Guid staticMenuItemId, out MenuItem moverMenuItem, out MenuItem staticMenuItem)
        {
            moverMenuItem = MenuItemDao.FindById(moverMenuItemId);
            staticMenuItem = MenuItemDao.FindById(staticMenuItemId);

            int seq1 = moverMenuItem.Sequence;
            int seq2 = staticMenuItem.Sequence;

            if (seq1 != seq2)
            {
                int temp = seq2;
                seq2 = seq1;
                seq1 = temp;
            }
            else
            {
                seq2++;
            }

            moverMenuItem.Sequence = seq1;
            staticMenuItem.Sequence = seq2;

            MenuItemDao.SaveOrUpdate(moverMenuItem);
            MenuItemDao.SaveOrUpdate(staticMenuItem);
        }

    }
}
