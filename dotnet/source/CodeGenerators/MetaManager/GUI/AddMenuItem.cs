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


namespace Cdc.MetaManager.GUI
{
    public partial class AddMenuItem : MdiChildForm
    {
        public bool IsRootMenuItem { private get; set; }
        public Cdc.MetaManager.DataAccess.Domain.MenuItem CurrentMenuItem { get; set; }

        private IMenuService menuService = null;

        public AddMenuItem()
        {
            InitializeComponent();

            menuService = MetaManagerServices.GetMenuService();
        }

        private void AddMenuItem_Load(object sender, EventArgs e)
        {
            if (CurrentMenuItem == null)
                DialogResult = DialogResult.Cancel;

            if (IsRootMenuItem || CurrentMenuItem.Children.Count > 0)
                btnBrowseAction.Enabled = false;

            if (IsRootMenuItem)
            {
                tbName.Enabled = false;
                tbAuthorizationId.Enabled = false;
            }

            if (CurrentMenuItem.Action != null)
            {
                // Read the menuitem with action
                CurrentMenuItem = menuService.GetMenuItemById(CurrentMenuItem.Id);
            }

            tbName.Text = CurrentMenuItem.Name;
            tbCaption.Text = CurrentMenuItem.Caption;
            tbAuthorizationId.Text = CurrentMenuItem.AuthorizationId;
            SetActionText();

            EnableDisableButtons();
        }

        private void SetActionText()
        {
            if (CurrentMenuItem.Action != null)
            {
                tbAction.Text = string.Format("({0}) {1}", CurrentMenuItem.Action.Id.ToString(), CurrentMenuItem.Action.Name);

                if (CurrentMenuItem.Action.MappedToObject is Dialog)
                    lblActionType.Text = string.Format("Action connected to Dialog - ({0}) {1}", CurrentMenuItem.Action.MappedToObject.Id.ToString(), CurrentMenuItem.Action.MappedToObject.Name);
                else if (CurrentMenuItem.Action.MappedToObject is CustomDialog)
                    lblActionType.Text = string.Format("Action connected to Custom Dialog - ({0}) {1}", CurrentMenuItem.Action.MappedToObject.Id.ToString(), CurrentMenuItem.Action.MappedToObject.Name);
                else
                    lblActionType.Text = string.Empty;
            }
            else
            {
                tbAction.Text = string.Empty;
                lblActionType.Text = string.Empty;
            }
        }

        private void btnBrowseAction_Click(object sender, EventArgs e)
        {
            using (ChooseCustomOrNormalDialog chooseDialogType = new ChooseCustomOrNormalDialog())
            {
                if (chooseDialogType.ShowDialog() == DialogResult.OK)
                {
                    if (chooseDialogType.OverviewDialogSelected)
                    {
                        using (SelectDialogUXAction form = new SelectDialogUXAction())
                        {
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;

                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                CurrentMenuItem.Action = form.SelectedAction;
                                SetActionText();
                                EnableDisableButtons();
                            }
                        }
                    }
                    else
                    {
                        using (SelectCustomDialogUXAction form = new SelectCustomDialogUXAction())
                        {
                            form.FrontendApplication = FrontendApplication;
                            form.BackendApplication = BackendApplication;

                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                CurrentMenuItem.Action = form.SelectedAction;
                                SetActionText();
                                EnableDisableButtons();
                            }
                        }
                    }
                }
            }
        }

        private void btnClearAction_Click(object sender, EventArgs e)
        {
            CurrentMenuItem.Action = null;
            SetActionText();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckCaptionFocus(tbCaption, "MenuItem Caption", true))
                return;

            if (!IsRootMenuItem && !NamingGuidance.CheckNameFocus(tbName, "MenuItem Name", true))
                return;

            if (!IsRootMenuItem && !NamingGuidance.CheckGUIDFocus(tbAuthorizationId, "MenuItem Authorization Id", true))
                return;

            CurrentMenuItem.Caption = tbCaption.Text.Trim();
            CurrentMenuItem.Name = tbName.Text.Trim();
            
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnAuthIdDefault_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbName.Text))
            {
                tbAuthorizationId.Text = Guid.NewGuid().ToString().ToUpper();
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnClearAction.Enabled = false;

            if (CurrentMenuItem.Action != null)
                btnClearAction.Enabled = true;
        }

    }
}
