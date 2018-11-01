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
    public partial class SelectCustomDialog : MdiChildForm
    {
        private IDialogService dialogService = null;
        private IModelService modelService = null;

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

        public SelectCustomDialog()
        {
            InitializeComponent();

            // Get services context
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void SelectCustomDialog_Load(object sender, EventArgs e)
        {
            ReadFromDBAndShowList();
            Cursor.Current = Cursors.Default;

            // Select the first item in list
            if (lvDialogs.Items.Count > 0)
            {
                lvDialogs.Items[0].Selected = true;
            }
            EnableDisableButtons();
        }

        private void ReadFromDBAndShowList()
        {
            // Fetch the complete list of UXActions.
            if (FrontendApplication != null)
            {
                dialogs = dialogService.GetAllCustomDialogs(FrontendApplication.Id);
            }

            // Show the read list
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

        private void tbDLLName_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void tbTopic_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void lvDialogs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnEditCustomDialog.Enabled = false;
            btnDeleteCustomDialog.Enabled = false;
            btnCreateCustomDialog.Enabled = false;
            

            if (lvDialogs.SelectedItems.Count == 1 &&
                lvDialogs.SelectedItems[0].Tag != null)
            {
                btnCreateCustomDialog.Enabled = FrontendApplication.IsLocked && FrontendApplication.LockedBy == Environment.UserName;
                btnEditCustomDialog.Enabled = SelectedDialog.IsLocked && SelectedDialog.LockedBy == Environment.UserName ;
                btnDeleteCustomDialog.Enabled = SelectedDialog.IsLocked && SelectedDialog.LockedBy == Environment.UserName;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEditCustomDialog_Click(object sender, EventArgs e)
        {
            DoEditCustomDialog();
        }

        private void lvDialogs_DoubleClick(object sender, EventArgs e)
        {
            DoEditCustomDialog();
        }

        private void btnCreateCustomDialog_Click(object sender, EventArgs e)
        {
            DoCreateCustomDialog();
        }

        private void DoEditCustomDialog()
        {
            if (SelectedDialog != null)
            {
                using (AddCustomDialog editCustomDialog = new AddCustomDialog())
                {
                    editCustomDialog.FrontendApplication = FrontendApplication;
                    editCustomDialog.BackendApplication = BackendApplication;
                    editCustomDialog.ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(SelectedDialog.Id, typeof(CustomDialog));

                    if (editCustomDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Update the current row
                        UpdateListItem(editCustomDialog.getCustomDialog, lvDialogs.SelectedItems[0]);
                    }
                }
            }
        }

        private void DoCreateCustomDialog()
        {
            using (AddCustomDialog addCustomDialog = new AddCustomDialog())
            {
                addCustomDialog.FrontendApplication = FrontendApplication;
                addCustomDialog.BackendApplication = BackendApplication;

                if (addCustomDialog.ShowDialog() == DialogResult.OK)
                {
                    // Update the list
                    ReadFromDBAndShowList();
                }
            }
        }

        private void DoDeleteCustomDialog()
        {
            if (SelectedDialog != null)
            {
                DataAccess.IVersionControlled domainObject = SelectedDialog;

                domainObject = (DataAccess.IVersionControlled)modelService.GetInitializedDomainObject(domainObject.Id, modelService.GetDomainObjectType(domainObject));

                if (domainObject.IsLocked && domainObject.LockedBy == Environment.UserName)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        modelService.DeleteDomainObject(domainObject);
                        ReadFromDBAndShowList();
                        Cursor.Current = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        string mess = ex.Message;

                        if (ex is ModelAggregatedException)
                        {
                            string ids = string.Empty;
                            foreach (string id in ((ModelAggregatedException)ex).Ids)
                            {
                                ids += id + "\r\n";
                            }

                            Clipboard.SetText(ids);
                            mess += "\r\n\r\nThe Ids has been copied to the clipboard";
                        }

                        MessageBox.Show(mess);
                    }
                }
            }                                          
        }

        private void btnDeleteCustomDialog_Click(object sender, EventArgs e)
        {
            DoDeleteCustomDialog();
        }
    }
}
