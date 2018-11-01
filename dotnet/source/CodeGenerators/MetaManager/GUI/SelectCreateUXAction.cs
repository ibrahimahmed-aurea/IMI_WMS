using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic;


namespace Cdc.MetaManager.GUI
{
    public partial class SelectCreateUXAction : MdiChildForm
    {
        public IList<UXAction> FoundActions { get; set; }
        public UXAction SelectedAction { get; private set; }
        public IMappableUXObject MappableUXObject { private get; set; }
        private IList<UXAction> AllActions { get; set; }


        private IList<string> AllActionNames
        {
            get
            {
                // Get the names of the AllActions
                if (AllActions != null)
                {
                    return (from action in AllActions
                            select action.Name).ToList();
                }
                else
                    return null;
            }
        }

        private IModelService modelService;

        public UXAction ListViewSelectedAction
        {
            get
            {
                if (lvActions.SelectedItems.Count == 1)
                    return (UXAction)lvActions.SelectedItems[0].Tag;

                return null;
            }
        }

        public SelectCreateUXAction()
        {
            InitializeComponent();

            SelectedAction = null;

            modelService = MetaManagerServices.GetModelService();
        }

        private void SelectCreateUXAction_Load(object sender, EventArgs e)
        {
            PopulateCreateFields(false);

            // Populate existing list
            PopulateActions();

            // Get all existing actions for the application to be able to check
            // the name of the action if a new one is created.
            AllActions = modelService.GetAllDomainObjectsByApplicationId<UXAction>(FrontendApplication.Id);

            EnableDisableButtons();

            rbSelectAction.Checked = true;
        }

        private void PopulateCreateFields(bool doFocus)
        {
            if (MappableUXObject is Dialog ||
                MappableUXObject is CustomDialog)
            {
                if (doFocus)
                    tbName.Focus();

                if (string.IsNullOrEmpty(tbName.Text.Trim()) ||
                    tbName.Text.ToUpper().Trim() == "Show")
                {
                    tbName.Text = "Show";
                    tbName.SelectionStart = tbName.Text.Length;
                }
                else if (tbName.Text.Trim() == "Show")
                {
                    tbName.SelectionStart = tbName.Text.Length;
                }
            }
            else if (MappableUXObject is ServiceMethod)
            {
                if (doFocus)
                    tbName.Focus();

                if (string.IsNullOrEmpty(tbName.Text.Trim()))
                {
                    tbName.Text = "Run";
                    tbName.SelectionStart = tbName.Text.Length;
                }
            }
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (rbSelectAction.Checked && ListViewSelectedAction != null)
            {
                btnOK.Enabled = true;
            }
            else if (rbCreateAction.Checked)
            {
                if (!string.IsNullOrEmpty(tbName.Text) &&
                    !string.IsNullOrEmpty(tbCaption.Text))
                {
                    btnOK.Enabled = true;
                }
            }
        }

        private void PopulateActions()
        {
            // Clear all items
            lvActions.Items.Clear();

            foreach (UXAction action in FoundActions)
            {
                ListViewItem item = new ListViewItem();
                item.Text = action.Id.ToString();
                item.SubItems.Add(action.Name);
                item.SubItems.Add(action.Caption);
                item.Tag = action;

                lvActions.Items.Add(item);
            }

            // Select first row in list
            if (lvActions.Items.Count > 0)
                lvActions.Items[0].Selected = true;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (!rbCreateAction.Checked)
                rbCreateAction.Checked = true;

            EnableDisableButtons();
        }

        private void tbCaption_TextChanged(object sender, EventArgs e)
        {
            if (!rbCreateAction.Checked)
                rbCreateAction.Checked = true;

            EnableDisableButtons();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedAction = null;

            if (rbSelectAction.Checked)
            {
                // An existing action is selected
                if (ListViewSelectedAction != null)
                    SelectedAction = ListViewSelectedAction;
                DialogResult = DialogResult.OK;
            }
            else if (rbCreateAction.Checked)
            {
                // Check if name is ok
                if (MappableUXObject is Dialog ||
                    MappableUXObject is CustomDialog)
                {
                    if (!NamingGuidance.CheckUXActionDialogName(tbName.Text.Trim(), true))
                    {
                        tbName.Focus();
                        return;
                    }
                }
                else if (MappableUXObject is ServiceMethod)
                {
                    if (!NamingGuidance.CheckUXActionServiceName(tbName.Text.Trim(), true))
                    {
                        tbName.Focus();
                        return;
                    }
                }

                // Check if name exists
                if (!NamingGuidance.CheckNameNotInList(tbName.Text.Trim(), "UXAction Name", "Action Names for the current Application", AllActionNames, false, true))
                    return;

                // Check if Caption is ok
                if (!NamingGuidance.CheckCaptionFocus(tbCaption, "Caption", true))
                    return;

                FrontendApplication = modelService.GetInitializedDomainObject<DataAccess.Domain.Application>(FrontendApplication.Id);


                // Create the new action
                SelectedAction = MetaManagerServices.Helpers.UXActionHelper.CreateUXActionForMappableObject(MappableUXObject, FrontendApplication, tbName.Text.Trim(), tbCaption.Text.Trim());
                // Leave the action as checked out
                CheckOutInObject(SelectedAction, true);

                DialogResult = DialogResult.OK;

            }

            if (SelectedAction == null)
            {
                MessageBox.Show("No action selected/created!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void rbCreateAction_Click(object sender, EventArgs e)
        {
            if (rbCreateAction.Checked)
            {
                PopulateCreateFields(true);
            }
        }

        private void rbSelectAction_Click(object sender, EventArgs e)
        {
            if (rbSelectAction.Checked)
            {
                if (lvActions.SelectedItems.Count != 1)
                    lvActions.Items[0].Selected = true;

                lvActions.Focus();
            }
        }

        private void rbCreateAction_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void rbSelectAction_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void lvActions_Click(object sender, EventArgs e)
        {
            if (lvActions.SelectedItems.Count > 0)
            {
                rbSelectAction.Checked = true;
            }

            EnableDisableButtons();
        }

        private void lvActions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EnableDisableButtons();
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
