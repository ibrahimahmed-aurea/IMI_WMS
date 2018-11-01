using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;

using System.IO;

namespace Cdc.MetaManager.GUI
{
    public partial class CreateDeploymentgroups : MdiChildForm
    {
        IModelService modelService = null;
        public CreateDeploymentgroups()
        {
            modelService = MetaManagerServices.GetModelService();
            InitializeComponent();
        }

        private void CreateDeploymentgroups_Load(object sender, EventArgs e)
        {
            foreach (DataAccess.Domain.Application app in modelService.GetVersionControlledDomainObjectsForParent(typeof(DataAccess.Domain.Application), Guid.Empty))
            {
                AddAppListItem(app);
            }
        }

        private void AddAppListItem(DataAccess.Domain.Application app)
        {
            ListViewItem newItem = new ListViewItem();
            newItem.Text = app.Name;
            newItem.Tag = app;
            if (app.IsFrontend.Value)
            {
                newItem.SubItems.Add("X");
                newItem.SubItems.Add("");
            }
            else
            {
                newItem.SubItems.Add("");
                newItem.SubItems.Add("X");
            }
            allAppListView.Items.Add(newItem);
        }

        private void RemoveAppListItem(ListViewItem item)
        {
            if (allAppListView.Items.Contains(item))
            {
                allAppListView.Items.Remove(item);
            }
        }

        private void allAppListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is ListViewItem)
            {
                if (((ListViewItem)e.Item).Tag is DataAccess.Domain.Application)
                {
                    if (((DataAccess.Domain.Application)((ListViewItem)e.Item).Tag).IsFrontend.Value)
                    {
                        allAppListView.DoDragDrop(e.Item, DragDropEffects.Move);
                    }
                    else
                    {
                        allAppListView.DoDragDrop(e.Item, DragDropEffects.Copy);
                    }
                }
            }
        }

        private void FrontendApptextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                if (typeof(DataAccess.Domain.Application).IsAssignableFrom(((ListViewItem)e.Data.GetData(typeof(ListViewItem))).Tag.GetType()))
                {
                    DataAccess.Domain.Application draggedApp = ((DataAccess.Domain.Application)((ListViewItem)e.Data.GetData(typeof(ListViewItem))).Tag);
                    if (draggedApp.IsFrontend.Value)
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                }
            }
        }

        private void FrontendApptextBox_DragDrop(object sender, DragEventArgs e)
        {
            if (DeployGrpNametextBox.Text == FrontendApptextBox.Text)
            {
                DeployGrpNametextBox.Text = string.Empty;
            }

            DataAccess.Domain.Application draggedApp = ((DataAccess.Domain.Application)((ListViewItem)e.Data.GetData(typeof(ListViewItem))).Tag);

            if (FrontendApptextBox.Tag is DataAccess.Domain.Application)
            {
                if (((DataAccess.Domain.Application)FrontendApptextBox.Tag).Id != draggedApp.Id)
                {
                    AddAppListItem(((DataAccess.Domain.Application)FrontendApptextBox.Tag));
                }
            }

            
            FrontendApptextBox.Text = draggedApp.Name;
            FrontendApptextBox.Tag = draggedApp;
            
            if (DeployGrpNametextBox.Text == string.Empty)
            {
                DeployGrpNametextBox.Text = FrontendApptextBox.Text;
            }

            if (DeployGrpNametextBox.Tag == null)
            {
                DeployGrpNametextBox.Tag = new DataAccess.Domain.DeploymentGroup();
            }

            ((DataAccess.Domain.DeploymentGroup)DeployGrpNametextBox.Tag).FrontendApplication = draggedApp;
           

            RemoveAppListItem(((ListViewItem)e.Data.GetData(typeof(ListViewItem))));

        }

        private void BackendApptextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                if (typeof(DataAccess.Domain.Application).IsAssignableFrom(((ListViewItem)e.Data.GetData(typeof(ListViewItem))).Tag.GetType()))
                {
                    DataAccess.Domain.Application draggedApp = ((DataAccess.Domain.Application)((ListViewItem)e.Data.GetData(typeof(ListViewItem))).Tag);
                    if (!draggedApp.IsFrontend.Value)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void BackendApptextBox_DragDrop(object sender, DragEventArgs e)
        {
            DataAccess.Domain.Application draggedApp = ((DataAccess.Domain.Application)((ListViewItem)e.Data.GetData(typeof(ListViewItem))).Tag);
            BackendApptextBox.Text = draggedApp.Name;
            BackendApptextBox.Tag = draggedApp;

            if (DeployGrpNametextBox.Tag == null)
            {
                DeployGrpNametextBox.Tag = new DataAccess.Domain.DeploymentGroup();
            }

            ((DataAccess.Domain.DeploymentGroup)DeployGrpNametextBox.Tag).BackendApplication = draggedApp;
        }

        private void DeployGrpNametextBox_TextChanged(object sender, EventArgs e)
        {
            if (DeployGrpNametextBox.Tag == null)
            {
                DeployGrpNametextBox.Tag = new DataAccess.Domain.DeploymentGroup();
            }

            ((DataAccess.Domain.DeploymentGroup)DeployGrpNametextBox.Tag).Name = DeployGrpNametextBox.Text;
        }

        private void DeploymentgroupslistView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataAccess.Domain.DeploymentGroup)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void DeployGrpNametextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (DeployGrpNametextBox.Tag is DeploymentGroup)
                {
                    DeploymentGroup depGrp = ((DeploymentGroup)DeployGrpNametextBox.Tag);
                    if (depGrp.BackendApplication != null && depGrp.FrontendApplication != null && !string.IsNullOrEmpty(depGrp.Name))
                    {
                        DeployGrpNametextBox.DoDragDrop(DeployGrpNametextBox.Tag, DragDropEffects.Move);
                    }
                }
            }
        }

        private void DeploymentgroupslistView_DragDrop(object sender, DragEventArgs e)
        {
            DataAccess.Domain.DeploymentGroup dragedDG = (DataAccess.Domain.DeploymentGroup)e.Data.GetData(typeof(DataAccess.Domain.DeploymentGroup));
            ListViewItem newItem = new ListViewItem();
            newItem.Text = dragedDG.Name;
            newItem.Tag = dragedDG;
            newItem.SubItems.Add(dragedDG.FrontendApplication.Name);
            newItem.SubItems.Add(dragedDG.BackendApplication.Name);
            DeploymentgroupslistView.Items.Add(newItem);

            DeployGrpNametextBox.Tag = null;
            FrontendApptextBox.Text = string.Empty;
            FrontendApptextBox.Tag = null;
            BackendApptextBox.Text = string.Empty;
            BackendApptextBox.Tag = null;
            DeployGrpNametextBox.Text = string.Empty;
            
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in DeploymentgroupslistView.Items)
            {
                modelService.SaveDomainObject((DataAccess.Domain.DeploymentGroup) item.Tag);
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }


    }
}
