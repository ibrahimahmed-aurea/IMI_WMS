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
using Cdc.Metamanager.BusinessLogic;

namespace Cdc.MetaManager.GUI
{
    public partial class ConnectViewToService : MdiChildForm
    {
        private IDialogService dialogService = null;

        public ConnectViewToService()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
        }

        private void ConnectViewToService_Load(object sender, EventArgs e)
        {
            PopulateModuleCombobox();
        }

        private void PopulateModuleCombobox()
        {
            IList<Module> modules = dialogService.GetAllModules(ApplicationVersion.Id);

            moduleBindingSource.DataSource = modules;
        }

        private void cbModules_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbModules.SelectedValue != null)
            {
                cbDialogs.Enabled = true;

                IList<Dialog> dialogs = dialogService.GetAllDialogs((int)cbModules.SelectedValue);

                if (dialogs.Count > 0)
                {
                    dialogBindingSource.DataSource = dialogs;
                }
                else
                {
                    btnShowDialog.Enabled = false;
                }
            }
            else
            {
                cbDialogs.Enabled = false;
            }
        }

        private void cbDialogs_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbDialogs.SelectedValue != null)
            {
                btnShowDialog.Enabled = true;
            }
            else
            {
                btnShowDialog.Enabled = false;
            }
        }

        private void btnShowDialog_Click(object sender, EventArgs e)
        {
            if (cbDialogs.SelectedValue != null)
            {
                Dialog currentDialog = dialogService.GetDialogWithViewTree((int)cbDialogs.SelectedValue);

                DialogObjectViewer viewer = new DialogObjectViewer();
                viewer.MdiParent = this.MdiParent;
                viewer.Dialog = currentDialog;
                viewer.Show();
            }
        }

    }
}
