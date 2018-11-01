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
    public partial class SelectDialogUXAction : MdiChildForm
    {
        public UXAction SelectedAction { get; private set; }

        private IDialogService dialogService = null;
        private IUXActionService actionService = null;
        IList<Dialog> dialogs = null;
        private IModelService modelService;

        private Dialog SelectedDialog
        {
            get
            {
                if (lvDialogs.SelectedItems.Count == 1)
                    return (Dialog)lvDialogs.SelectedItems[0].Tag;
                else
                    return null;
            }
        }

        public SelectDialogUXAction()
        {
            InitializeComponent();

            // Get services context
            dialogService = MetaManagerServices.GetDialogService();
            actionService = MetaManagerServices.GetUXActionService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void SelectUXAction_Load(object sender, EventArgs e)
        {
            // Fetch the complete list of UXActions.
            if (FrontendApplication != null)
            {
                dialogs = dialogService.GetAllDialogsWithInterfaceView(FrontendApplication.Id, DialogType.Overview);
            }

            ShowList();
        }

        private void ShowList()
        {
            if (dialogs != null)
            {
                lvDialogs.Items.Clear();

                foreach (Dialog dialog in dialogs)
                {
                    // Check name, caption and originaldialog
                    if (dialog.Name.ToUpper().Contains(tbName.Text.ToUpper()) &&
                        dialog.Title.ToUpper().Contains(tbTitle.Text.ToUpper()) &&
                        dialog.Module.Name.ToUpper().Contains(tbModule.Text.ToUpper()) &&
                        (
                          (dialog.OriginalDialogName == null && tbOriginalDialog.Text.Length == 0) ||
                          (dialog.OriginalDialogName != null) &&
                          (dialog.OriginalDialogName.ToUpper().Contains(tbOriginalDialog.Text.ToUpper()))
                        )
                       )
                    {
                        ListViewItem item = new ListViewItem();
                        UpdateListItem(dialog, item);
                        lvDialogs.Items.Add(item);
                    }
                }
            }

            EnableDisableButtons();
        }

        private static void UpdateListItem(Dialog dialog, ListViewItem item)
        {
            item.Text = dialog.Id.ToString();

            if (item.SubItems.Count == 1)
            {
                item.SubItems.Add(dialog.Name);
                item.SubItems.Add(dialog.Title);
                item.SubItems.Add(dialog.OriginalDialogName);
                item.SubItems.Add(dialog.Module.Name);
            }
            else
            {
                item.SubItems[1].Text = dialog.Name;
                item.SubItems[2].Text = dialog.Title;
                item.SubItems[3].Text = dialog.OriginalDialogName;
                item.SubItems[4].Text = dialog.Module.Name;
            }
            item.Tag = dialog;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void tbOriginalDialog_TextChanged(object sender, EventArgs e)
        {
            ShowList();
        }

        private void tbModule_TextChanged(object sender, EventArgs e)
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

            if (lvDialogs.SelectedItems.Count == 1)
            {
                btnOK.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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

                        FrontendApplication = modelService.GetInitializedDomainObject<DataAccess.Domain.Application>(FrontendApplication.Id);

                        // Create the new action
                        // Create the action and interface
                        SelectedAction = MetaManagerServices.Helpers.UXActionHelper.CreateUXActionForMappableObject(SelectedDialog, FrontendApplication, null, null);

                        // Leave the action as checked out
                        CheckOutInObject(SelectedAction, true);

                        DialogResult = DialogResult.OK;
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

        private void CheckOutInObject(DataAccess.IVersionControlled checkOutObject, bool trueCheckOut_falseCheckIn)
        {
            DataAccess.IVersionControlled domainObject = null;
            domainObject = checkOutObject;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(domainObject.GetType()))
            {
                if (trueCheckOut_falseCheckIn)
                {
                    try
                    {
                        MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(domainObject.Id, domainObject.GetType());
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(domainObject.Id, domainObject.GetType(), FrontendApplication);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }


    }
}
