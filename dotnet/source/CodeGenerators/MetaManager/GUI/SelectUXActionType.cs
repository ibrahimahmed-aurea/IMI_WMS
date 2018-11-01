using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using NHibernate;


namespace Cdc.MetaManager.GUI
{
    public enum SelectedUXActionType { Dialog, ServiceMethod, DrillDown, Unmapped, JumpTo, Workflow };

    public partial class SelectUXActionType : MdiChildForm
    {
        public SelectedUXActionType SelectedActionType { get; set; }
        public bool NoDrilldowns { set; private get; }
        public UXAction SelectedAction { get; private set; }

        private IDialogService dialogService = null;
        private IApplicationService appService = null;
        private IUXActionService actionService = null;
        private IModelService modelService;
        private IMappableUXObject SelectedMappableUXObject { get; set; }

        ToolTip tooltip = new ToolTip();

        public SelectUXActionType()
        {
            InitializeComponent();

            SelectedMappableUXObject = null;
            SelectedAction = null;
            NoDrilldowns = false;

            dialogService = MetaManagerServices.GetDialogService();
            appService = MetaManagerServices.GetApplicationService();
            modelService = MetaManagerServices.GetModelService();
            actionService = MetaManagerServices.GetUXActionService();
        }

        private void rbtnDialog_CheckedChanged(object sender, EventArgs e)
        {

            UpdateFilters();

        }

        private void rbtnServiceMethod_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilters();
        }

        private void SelectUXActionType_Load(object sender, EventArgs e)
        {
            if (NoDrilldowns)
            {
                rbtnDrilldown.Enabled = false;
                rbtnDrilldown.Text += " (Drilldown not available. Possibly because you haven't mapped the grid to any fields yet.)";
            }

            rbtnDialog.Checked = true;
            UpdateFilters();
        }

        private void UpdateFilters()
        {
            tbFilter1.Text = string.Empty;
            tbFilter2.Text = string.Empty;
            tbFilter3.Text = string.Empty;

            if ((rbtnDialog.Checked) || (rbtnJumpTo.Checked) || (rbtnDrilldown.Checked))
            {
                lvSelectObject.Clear();
                lvSelectObject.Columns.Add("ID", "Id", 30);
                lvSelectObject.Columns.Add("NAME", "Name", 200);
                lvSelectObject.Columns.Add("ORIGINALNAME", "Original Dialog", 150);
                lvSelectObject.Columns.Add("MODULE", "Module", 200);

                lblFilter1.Text = "Name";
                lblFilter1.Visible = true;
                lblFilter2.Text = "Original Dialog";
                lblFilter2.Visible = true;
                lblFilter3.Text = "Module";
                lblFilter3.Visible = true;
                tbFilter3.Visible = true;
            }
            else if (rbtnServiceMethod.Checked)
            {
                lvSelectObject.Clear();
                lvSelectObject.Columns.Add("ID", "Id", 30);
                lvSelectObject.Columns.Add("NAME", "Name", 200);
                lvSelectObject.Columns.Add("SERVICENAME", "Service", 150);

                lblFilter1.Text = "Name";
                lblFilter1.Visible = true;
                tbFilter1.Visible = true;
                lblFilter2.Text = "Service";
                lblFilter2.Visible = true;
                tbFilter2.Visible = true;
                lblFilter3.Visible = false;
                tbFilter3.Visible = false;
            }
            else if (rbtnDrilldown.Checked)
            {
                lvSelectObject.Clear();
                lvSelectObject.Columns.Add("ID", "Id", 30);
                lvSelectObject.Columns.Add("NAME", "Name", 200);
                lvSelectObject.Columns.Add("ORIGINALNAME", "Original Dialog", 150);
                lvSelectObject.Columns.Add("MODULE", "Module", 100);

                lblFilter1.Text = "Name";
                lblFilter1.Visible = true;
                lblFilter2.Text = "Original Dialog";
                lblFilter2.Visible = true;
                lblFilter3.Text = "Module";
                lblFilter3.Visible = true;
                tbFilter3.Visible = true;
            }
            else if (rbtnUnmapped.Checked)
            {
                lvSelectObject.Clear();
                lvSelectObject.Columns.Add("ID", "Id", 30);
                lvSelectObject.Columns.Add("NAME", "Name", 200);
                lvSelectObject.Columns.Add("CAPTION", "Caption", 200);

                lblFilter1.Text = "Name";
                lblFilter1.Visible = true;
                lblFilter2.Text = "Caption";
                lblFilter2.Visible = true;
                lblFilter3.Visible = false;
                tbFilter3.Visible = false;
            }
            else if (rbtnWorkflow.Checked)
            {
                lvSelectObject.Clear();
                lvSelectObject.Columns.Add("ID", "Id", 30);
                lvSelectObject.Columns.Add("NAME", "Name", 200);
                lvSelectObject.Columns.Add("DESCRIPTION", "Description", 200);
                lvSelectObject.Columns.Add("MODULE", "Module", 200);

                lblFilter1.Text = "Name";
                lblFilter1.Visible = true;
                lblFilter2.Text = "Description";
                lblFilter2.Visible = true;
                lblFilter3.Text = "Module";
                lblFilter3.Visible = true;
                tbFilter3.Visible = true;
            }
        }

        private void FindUnmappedActions()
        {
            btnOK.Enabled = false;

            // Clear items
            lvSelectObject.Items.Clear();

            var criteriaDictionary = new Dictionary<string, object>();
            criteriaDictionary.Add("Application.Id", FrontendApplication.Id);
            criteriaDictionary.Add("Name", tbFilter1.Text);
            criteriaDictionary.Add("Caption", tbFilter2.Text);

            foreach (UXAction action in modelService.GetAllDomainObjectsByPropertyValues<UXAction>(criteriaDictionary, true, true).Where(x => x.MappedToObject == null))
            {
                ListViewItem item = new ListViewItem();
                item.Tag = action;
                item.Text = action.Id.ToString();
                item.SubItems.Add(action.Name);
                item.SubItems.Add(action.Caption);
                lvSelectObject.Items.Add(item);
            }

            // Set first item to selected
            if (lvSelectObject.Items.Count > 0)
            {
                lvSelectObject.Items[0].Selected = true;
            }
        }

        private void FindWorkflows()
        {
            btnOK.Enabled = false;

            // Clear items
            lvSelectObject.Items.Clear();

            var criteriaDictionary = new Dictionary<string, object>();
            criteriaDictionary.Add("Module.Application.Id", FrontendApplication.Id);
            criteriaDictionary.Add("Name", tbFilter1.Text);
            criteriaDictionary.Add("Description", tbFilter2.Text);
            criteriaDictionary.Add("Module.Name", tbFilter3.Text);

            foreach (Cdc.MetaManager.DataAccess.Domain.Workflow workflow in modelService.GetAllDomainObjectsByPropertyValues<Cdc.MetaManager.DataAccess.Domain.Workflow>(criteriaDictionary, true, true))
            {
                ListViewItem item = new ListViewItem();
                item.Tag = workflow;
                item.Text = workflow.Id.ToString();
                item.SubItems.Add(workflow.Name);
                item.SubItems.Add(workflow.Description);
                item.SubItems.Add(workflow.Module.Name);
                lvSelectObject.Items.Add(item);
            }

            // Set first item to selected
            if (lvSelectObject.Items.Count > 0)
            {
                lvSelectObject.Items[0].Selected = true;
            }
        }


        private void FindServiceMethods()
        {
            btnOK.Enabled = false;

            // Clear items
            lvSelectObject.Items.Clear();

            var criteriaDictionary = new Dictionary<string, object>();
            criteriaDictionary.Add("Service.Application.Id", BackendApplication.Id);
            criteriaDictionary.Add("Name", tbFilter1.Text);
            criteriaDictionary.Add("Service.Name", tbFilter2.Text);

            foreach (ServiceMethod serviceMethod in modelService.GetAllDomainObjectsByPropertyValues<ServiceMethod>(criteriaDictionary, true, true).OrderBy(x => x.Name))
            {
                ListViewItem item = new ListViewItem();
                item.Tag = serviceMethod;
                item.Text = serviceMethod.Id.ToString();
                item.SubItems.Add(serviceMethod.Name);
                item.SubItems.Add(serviceMethod.Service.Name);
                lvSelectObject.Items.Add(item);
            }

            // Set first item to selected
            if (lvSelectObject.Items.Count > 0)
            {
                lvSelectObject.Items[0].Selected = true;
            }
        }

        private void FindDialogs(DialogType? dialogType)
        {
            btnOK.Enabled = false;

            // Clear items
            lvSelectObject.Items.Clear();

            var criteriaDictionary = new Dictionary<string, object>();
            criteriaDictionary.Add("Module.Application.Id", FrontendApplication.Id);
            criteriaDictionary.Add("Name", tbFilter1.Text);

            if (!string.IsNullOrEmpty(tbFilter2.Text))
            {
                criteriaDictionary.Add("OriginalDialogName", tbFilter2.Text);
            }

            criteriaDictionary.Add("Module.Name", tbFilter3.Text);

            IEnumerable<Dialog> dialogs = modelService.GetAllDomainObjectsByPropertyValues<Dialog>(criteriaDictionary, true, true);

            if (dialogType != null)
            {
                dialogs = dialogs.Where(x => x.Type == dialogType);
            }

            foreach (Dialog dialog in dialogs.OrderBy(x => x.Name))
            {
                ListViewItem item = new ListViewItem();
                item.Tag = dialog;
                item.Text = dialog.Id.ToString();
                item.SubItems.Add(dialog.Name);
                item.SubItems.Add(dialog.OriginalDialogName);
                item.SubItems.Add(dialog.Module.Name);
                lvSelectObject.Items.Add(item);
            }

            // Set first item to selected
            if (lvSelectObject.Items.Count > 0)
            {
                lvSelectObject.Items[0].Selected = true;
            }
        }

        private void FindActions()
        {
            if ((rbtnDialog.Checked))
            {
                FindDialogs(null);
            }
            else if (rbtnJumpTo.Checked)
            {
                FindDialogs(DialogType.Overview);
            }
            else if (rbtnServiceMethod.Checked)
            {
                FindServiceMethods();
            }
            else if (rbtnWorkflow.Checked)
            {
                FindWorkflows();
            }
            else if (rbtnDrilldown.Checked)
            {
                FindDialogs(DialogType.Drilldown);
            }
            else if (rbtnUnmapped.Checked)
            {
                FindUnmappedActions();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedMappableUXObject != null)
                {
                    SelectedAction = null;

                    // Check if a servicemethod is selected. Then we need to do additional 
                    // select from database to retrieve correct data.
                    if (SelectedMappableUXObject is ServiceMethod)
                    {
                        ServiceMethod readServiceMethod = appService.GetServiceMethodWithRequestMap((SelectedMappableUXObject as ServiceMethod).Id);

                        SelectedMappableUXObject = readServiceMethod;
                    }

                    // Find all actions connected to this Dialog/CustomDialog/ServiceMethod
                    IList<UXAction> FoundActions = null;

                    FoundActions = actionService.GetUXActionForMappableObject(SelectedMappableUXObject);

                    // If any actions is found for this object then show another dialog where you can select
                    // to reuse the actions or create new ones.
                    if (FoundActions != null && FoundActions.Count > 0)
                    {
                        // In all other cases show the dialog to select or create a new action
                        using (SelectCreateUXAction selectCreateAction = new SelectCreateUXAction())
                        {
                            selectCreateAction.FrontendApplication = FrontendApplication;
                            selectCreateAction.BackendApplication = BackendApplication;
                            selectCreateAction.FoundActions = FoundActions;
                            selectCreateAction.MappableUXObject = SelectedMappableUXObject;

                            if (selectCreateAction.ShowDialog() == DialogResult.OK)
                            {
                                SelectedAction = selectCreateAction.SelectedAction;
                            }
                        }
                    }
                    else
                    {
                        if (SelectedMappableUXObject != null)
                        {

                            FrontendApplication = modelService.GetInitializedDomainObject<DataAccess.Domain.Application>(FrontendApplication.Id);

                            if (rbtnJumpTo.Checked)
                            {
                                Dialog d = SelectedMappableUXObject as Dialog;
                                // Create the action and interface
                                SelectedAction = MetaManagerServices.Helpers.UXActionHelper.CreateUXActionForMappableObject(SelectedMappableUXObject, FrontendApplication, string.Format("JumpTo{0}", d.Name), string.Format("Jump To {0}", d.Title));
                            }
                            else
                            {
                                // Create the action and interface
                                SelectedAction = MetaManagerServices.Helpers.UXActionHelper.CreateUXActionForMappableObject(SelectedMappableUXObject, FrontendApplication, null, null);
                            }

                            // Leave the action as checked out
                            CheckOutInObject(SelectedAction, true);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SelectedAction = null;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (SelectedAction != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void lvSelectObject_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            btnOK.Enabled = false;

            if (lvSelectObject.SelectedItems.Count == 1)
            {
                if (lvSelectObject.SelectedItems[0].Tag is UXAction)
                {
                    SelectedAction = lvSelectObject.SelectedItems[0].Tag as UXAction;
                    SelectedMappableUXObject = null;
                    SelectedActionType = SelectedUXActionType.Unmapped;
                }
                else if (lvSelectObject.SelectedItems[0].Tag is ServiceMethod)
                {
                    SelectedAction = null;
                    SelectedMappableUXObject = lvSelectObject.SelectedItems[0].Tag as ServiceMethod;
                    SelectedActionType = SelectedUXActionType.ServiceMethod;
                }
                else if (lvSelectObject.SelectedItems[0].Tag is Cdc.MetaManager.DataAccess.Domain.Workflow)
                {
                    SelectedAction = null;
                    SelectedMappableUXObject = lvSelectObject.SelectedItems[0].Tag as Cdc.MetaManager.DataAccess.Domain.Workflow;
                    SelectedActionType = SelectedUXActionType.Workflow;
                }
                else if (lvSelectObject.SelectedItems[0].Tag is Dialog)
                {
                    SelectedAction = null;
                    SelectedMappableUXObject = lvSelectObject.SelectedItems[0].Tag as Dialog;

                    if (rbtnDialog.Checked)
                    {
                        SelectedActionType = SelectedUXActionType.Dialog;
                    }
                    else if (rbtnDrilldown.Checked)
                    {
                        SelectedActionType = SelectedUXActionType.DrillDown;
                    }
                    else if (rbtnJumpTo.Checked)
                    {
                        SelectedActionType = SelectedUXActionType.JumpTo;
                    }
                }
                else
                {
                    return;
                }

                btnOK.Enabled = true;
            }
        }

        private void rbtnUnmapped_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilters();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                FindActions();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void tbFilter1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                FindActions();
            }
        }

    }
}
