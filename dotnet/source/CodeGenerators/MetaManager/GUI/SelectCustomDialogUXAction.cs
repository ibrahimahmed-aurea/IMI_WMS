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


namespace Cdc.MetaManager.GUI
{
    public partial class SelectCustomDialogUXAction : MdiChildForm
    {
        public UXAction SelectedAction { get; private set; }

        private IDialogService dialogService = null;
        private IUXActionService actionService = null;

        IList<CustomDialog> dialogs = null;

        private CustomDialog SelectedDialog
        {
            get
            {
                if (lvDialogs.SelectedItems.Count == 1)
                    return (CustomDialog)lvDialogs.SelectedItems[0].Tag;
                else
                    return null;
            }
            set
            {
                if (value is CustomDialog &&
                    lvDialogs.SelectedItems.Count == 1)
                {
                    lvDialogs.SelectedItems[0].Tag = value;
                }
            }
        }

        public SelectCustomDialogUXAction()
        {
            InitializeComponent();

            // Get services context
            dialogService = MetaManagerServices.GetDialogService();
            actionService = MetaManagerServices.GetUXActionService();
        }

        private void SelectCustomDialog_Load(object sender, EventArgs e)
        {
            ReadFromDBAndShowList();
        }

        private void ReadFromDBAndShowList()
        {
            // Fetch the complete list of UXActions.
            if (FrontendApplication != null)
            {
                dialogs = dialogService.GetAllCustomDialogs(FrontendApplication.Id);
            }

            ShowList();

            // Enable and disable buttons on page
            EnableDisableButtons();
        }

        private void ShowList()
        {
            if (dialogs != null)
            {
                lvDialogs.Items.Clear();

                foreach (CustomDialog dialog in dialogs)
                {
                    // Check name, caption and originaldialog
                    if (dialog.Name.ToUpper().Contains(tbName.Text.ToUpper()) &&
                        dialog.Topic.ToUpper().Contains(tbTopic.Text.ToUpper()) &&
                        dialog.DLLName.ToUpper().Contains(tbDLLName.Text.ToUpper()))
                    {
                        ListViewItem item = new ListViewItem();
                        UpdateListItem(dialog, item);
                        lvDialogs.Items.Add(item);
                    }
                }
            }

            EnableDisableButtons();
        }

        private static void UpdateListItem(CustomDialog dialog, ListViewItem item)
        {
            item.Text = dialog.Id.ToString();

            if (item.SubItems.Count == 1)
            {
                item.SubItems.Add(dialog.Name);
                item.SubItems.Add(dialog.Topic);
                item.SubItems.Add(dialog.DLLName);
            }
            else
            {
                item.SubItems[1].Text = dialog.Name;
                item.SubItems[2].Text = dialog.Topic;
                item.SubItems[3].Text = dialog.DLLName;
            }
            item.Tag = dialog;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void tbTopic_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void tbDLLName_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void lvDialogs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (SelectedDialog != null)
            {
                btnOK.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SelectedDialog != null)
            {
                try
                {
                    // Find all actions connected to this Dialog/CustomDialog/ServiceMethod
                    IList<UXAction> FoundActions = actionService.GetUXActionForMappableObject(SelectedDialog);

                    // If any actions is found for this object then show another dialog where you can select
                    // to reuse the actions or create new ones.
                    if (FoundActions != null && FoundActions.Count > 0)
                    {
                        using (SelectCreateUXAction selectCreateAction = new SelectCreateUXAction())
                        {
                            selectCreateAction.FrontendApplication = FrontendApplication;
                            selectCreateAction.BackendApplication = BackendApplication;
                            selectCreateAction.FoundActions = FoundActions;
                            selectCreateAction.MappableUXObject = SelectedDialog;

                            if (selectCreateAction.ShowDialog() == DialogResult.OK)
                            {
                                SelectedAction = selectCreateAction.SelectedAction;
                            }
                        }
                    }
                    else
                    {
                        // Create the action and interface
                        SelectedAction = MetaManagerServices.Helpers.UXActionHelper.CreateUXActionForMappableObject(SelectedDialog, FrontendApplication, null, null );
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
