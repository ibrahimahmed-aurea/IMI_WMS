using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess;
using System.Workflow.Activities.Rules.Design;


namespace Cdc.MetaManager.GUI
{
    public partial class EditUXAction : MdiChildForm
    {
        private IUXActionService uxActionService = null;
        private IModelService modelService = null;
        private UXAction UXAction;
        private bool IsNew = false;

        private List<MappedProperty> deletedMappedPropertiesInRequestMap = new List<MappedProperty>();
        private List<MappedProperty> addedMappedPropertiesInRequestMap = new List<MappedProperty>();

        public UXAction getUXAction
        {
            get { return UXAction; }
        }

        public EditUXAction()
        {
            InitializeComponent();

            UXAction = null;
            IsNew = false;

            uxActionService = MetaManagerServices.GetUXActionService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void EditUXAction_Load(object sender, EventArgs e)
        {
            // Get the action if there is an Id
            if (ContaindDomainObjectIdAndType.Key != Guid.Empty)
            {
                //UXAction = modelService.GetInitializedDomainObject<UXAction>(ContaindDomainObjectIdAndType.Key);
                UXAction = modelService.GetDynamicInitializedDomainObject<UXAction>(ContaindDomainObjectIdAndType.Key, new List<string>() { "RequestMap", "RequestMap.MappedProperties", "RuleSet" });
                IsNew = false;
            }
            else
            {
                UXAction = new UXAction();
                UXAction.Application = FrontendApplication;
                IsNew = true;
            }

            this.IsEditable = UXAction.IsLocked && UXAction.LockedBy == Environment.UserName;

            // Check if new action or editing old
            if (IsNew)
            {
                this.Text = "New Action";
            }
            else
            {
                this.Text = "Edit Action";
            }

            if (!this.IsEditable)
            {
                TypeDescriptor.AddAttributes(UXAction, new Attribute[] { new ReadOnlyAttribute(true) });
            }

            propertyGrid.SelectedObject = UXAction;

            EnableDisableButtons();

            Cursor.Current = Cursors.Default;
        }

        private void EnableDisableButtons()
        {
            editMapBtn.Enabled = false;
            editRuleBtn.Enabled = false;

            if (!IsNew && UXAction.Id != Guid.Empty && UXAction.MappedToObject != null && UXAction.RequestMap != null)
            {
                editMapBtn.Enabled = true;
                editRuleBtn.Enabled = true;
            }

            btnOK.Enabled = this.IsEditable;
        }
               
        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check Action Name
            if (UXAction.MappedToObject != null)
            {
                // Check if mapped to Dialog
                if (UXAction.MappedToObject.ActionType == UXActionType.Dialog)
                {
                    if (!NamingGuidance.CheckUXActionDialogName(UXAction.Name, true))
                        return;

                    if (!string.IsNullOrEmpty(UXAction.AlarmId) && (UXAction.Dialog.InterfaceView.ServiceMethod != null))
                    {
                        MessageBox.Show("AlarmId can only be used if the target Dialog has a Service Method defined.", "Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                // Check if mapped to Service
                else if (UXAction.MappedToObject.ActionType == UXActionType.ServiceMethod)
                {
                    if (!NamingGuidance.CheckUXActionServiceName(UXAction.Name, true))
                        return;
                }
            }
            else
            {
                // Check if not mapped
                if (!NamingGuidance.CheckName(UXAction.Name, "UXAction Name", true))
                    return;

                if (!string.IsNullOrEmpty(UXAction.AlarmId))
                {
                    MessageBox.Show("AlarmId can only be used when the Action points to a Dialog or Service Method.", "Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (!NamingGuidance.CheckCaption(UXAction.Caption, "UXAction Caption", true))
                return;

            if (UXAction.MappedToObject == null)
            {
                if (MessageBox.Show("You haven't mapped the Action to any object. Do you want to save the Action anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }
            
            if (UXAction.OriginalDialog == null)
            {
                UXAction.OriginalDialog = string.Empty;
            }
            
            Cursor = Cursors.WaitCursor;

            // Save
            try
            {
                if (addedMappedPropertiesInRequestMap.Count > 0 || deletedMappedPropertiesInRequestMap.Count > 0)
                {
                    modelService.StartSynchronizePropertyMapsInObjects(UXAction, new List<IDomainObject>(), deletedMappedPropertiesInRequestMap.Cast<IDomainObject>().ToList());
                }

                UXAction = (UXAction)modelService.SaveDomainObject(UXAction);

                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(UXAction.Id, typeof(UXAction));

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Cursor = Cursors.Default;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnTestQuestion_Click(object sender, EventArgs e)
        {
            /*
            if (!string.IsNullOrEmpty(tbQuestion.Text))
            {
                MessageBox.Show(tbQuestion.Text, "Test of Question", MessageBoxButtons.YesNo);
            }
            */
        }

        private void tbQuestion_TextChanged(object sender, EventArgs e)
        {
            //btnTestQuestion.Enabled = !string.IsNullOrEmpty(tbQuestion.Text);
        }

        private void editMapBtn_Click(object sender, EventArgs e)
        {
            using (PropertyMapForm2 form = new PropertyMapForm2())
            {
                form.FrontendApplication = this.FrontendApplication;
                form.BackendApplication = this.BackendApplication;
                form.AllowAddProperty = true;
                form.IsEditable = this.IsEditable;
                form.HideSyncronizeButton = true;
                form.UseProvidedMapObject = true;
                form.LockPropertyGrid = true;

                form.Owner = this;


                IList<IMappableProperty> sourceProperties = new List<IMappableProperty>();
                IList<IMappableProperty> targetProperties = new List<IMappableProperty>();

                form.DeletedMappedProperties = deletedMappedPropertiesInRequestMap;
                form.AddedMappedProperties = addedMappedPropertiesInRequestMap;

                //uxActionService.GetActionRequestMap(UXAction, out requestMap, out sourceProperties, out targetProperties);

                form.PropertyMap = UXAction.RequestMap;
                form.SourceProperties = sourceProperties;
                form.TargetProperties = targetProperties;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        UXAction.RequestMap = form.PropertyMap;
                        modelService.MergeSaveDomainObject(UXAction.RequestMap);

                        propertyGrid.SelectedObject = UXAction;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void editRuleBtn_Click(object sender, EventArgs e)
        {
            Module temp = new Module();
            temp.Application = FrontendApplication;
            temp.Name = "temp";

            UXAction action = uxActionService.GetUXActionByIdWithMap(UXAction.Id);

            Type context = RuleContextFactory.CreateActionContext(action, temp);

            using (RuleSetDialog ruleSetDialog = new RuleSetDialog(context, null, UXAction.RuleSet))
            {
                ruleSetDialog.Text += string.Format(" [{0}]", UXAction.Name);

                if (ruleSetDialog.ShowDialog() == DialogResult.OK)
                {
                    if (this.IsEditable)
                    {
                        if (ruleSetDialog.RuleSet.Rules.Count == 0)
                            UXAction.RuleSet = null;
                        else
                            UXAction.RuleSet = ruleSetDialog.RuleSet;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Your changes will not be saved since the action is not locked for editing. Check out the action and try again");
                        return;
                    }
                }
            }
        }
    }
}
