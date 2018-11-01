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
    public partial class CreateCustomView : MdiChildForm
    {
        public Cdc.MetaManager.DataAccess.Domain.View EditView { get; set; }

        private bool EditingView { get; set; }

        private IDialogService dialogService;
        private IModelService modelService;

        public CreateCustomView()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void CreateCustomView_Load(object sender, EventArgs e)
        {
            EditingView = false;

            if (EditView != null)
            {
                // Read the view
                EditView = dialogService.GetViewById(EditView.Id);
                
                IsEditable = EditView.IsLocked && EditView.LockedBy == Environment.UserName;

                this.Text = "Edit Custom View";
                EditingView = true;
            }

            PopulateBusinessEntityCombobox();
            PopulateCustomDLLNameCombobox();

            if (EditView == null)
            {
                // Create the new View
                EditView = new Cdc.MetaManager.DataAccess.Domain.View();

                EditView.AlternateView = null;
                EditView.Application = FrontendApplication;
                EditView.BusinessEntity = (BusinessEntity)cbBusinessEntity.SelectedItem;
                EditView.OriginalViewName = null;
                EditView.RequestMap = new PropertyMap();
                EditView.ResponseMap = new PropertyMap();
                EditView.ServiceMethod = null;
                EditView.Type = ViewType.Custom;
                IsEditable = true;
            }
            else
            {
                PopulateViewFields();
            }

            CheckEnableDisableOK();
        }

        private void PopulateViewFields()
        {
            foreach (BusinessEntity be in cbBusinessEntity.Items)
            {
                if (be.Id == EditView.BusinessEntity.Id)
                {
                    cbBusinessEntity.SelectedItem = be;
                    break;
                }
            }

            tbClassName.Text = EditView.CustomClassName;
            cbDLLName.Text = EditView.CustomDLLName;
            tbName.Text = EditView.Name;
            tbTitle.Text = EditView.Title;
        }

        private void PopulateCustomDLLNameCombobox()
        {
            IList<string> dllList = dialogService.FindAllUniqueCustomDLLNames(FrontendApplication.Id);

            cbDLLName.Items.AddRange(dllList.ToArray());
        }

        private void PopulateBusinessEntityCombobox()
        {
            IList<BusinessEntity> beList = modelService.GetAllDomainObjectsByApplicationId<BusinessEntity>(BackendApplication.Id);

            cbBusinessEntity.DataSource = beList;
        }

        private void CheckEnableDisableOK()
        {
            btnOK.Enabled = false;

            if (!string.IsNullOrEmpty(tbName.Text.Trim()) &&
                !string.IsNullOrEmpty(tbTitle.Text.Trim()) &&
                !string.IsNullOrEmpty(cbDLLName.Text.Trim()) &&
                !string.IsNullOrEmpty(tbClassName.Text.Trim()) &&
                IsEditable &&
                cbBusinessEntity.SelectedItem != null)
            {
                btnOK.Enabled = true;
            }

            tbClassName.Enabled = IsEditable;
            tbTitle.Enabled = IsEditable;
            tbName.Enabled = IsEditable;
            cbBusinessEntity.Enabled = IsEditable;
            cbDLLName.Enabled = IsEditable;
            btnEditInterface.Enabled = IsEditable;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check name.
            if (!NamingGuidance.CheckCustomViewName(tbName.Text.Trim(), true))
            {
                tbName.Focus();
                return;
            }

            // Check Title
            if (!NamingGuidance.CheckCaptionFocus(tbTitle, "Custom View Title", true))
                return;

            // Check DLL Name
            if (!NamingGuidance.CheckDLLName(cbDLLName.Text.Trim(), "Custom View DLL Name", true))
            {
                cbDLLName.Focus();
                return;
            }

            // Check CustomClassName
            if (!NamingGuidance.CheckClassName(tbClassName.Text, "Custom View Class Name", true))
            {
                tbClassName.Focus();
                return;
            }

            // Create the view
            EditView.BusinessEntity = (BusinessEntity)cbBusinessEntity.SelectedItem;
            EditView.CustomClassName = tbClassName.Text.Trim();
            EditView.CustomDLLName = cbDLLName.Text.Trim();
            EditView.Name = tbName.Text.Trim();
            EditView.Title = tbTitle.Text.Trim();

            // Persist the view
            EditView = (DataAccess.Domain.View)modelService.SaveDomainObject(EditView);
            
            DialogResult = DialogResult.OK;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableOK();
        }

        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableOK();
        }

        private void cbDLLName_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableOK();
        }

        private void tbClassName_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableOK();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnEditInterface_Click(object sender, EventArgs e)
        {
            if (EditView != null)
            {
                using (EditInterfaceMap form = new EditInterfaceMap())
                {
                    form.Owner = this;
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.RequestMap = EditView.RequestMap;
                    form.ResponseMap = EditView.ResponseMap;
                    form.IsEditable = true;
                    form.CanRequestMap = false;
                    form.Text = string.Format("Edit View Interface - {0}", EditView.ToString());

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        EditView.RequestMap = form.RequestMap;
                        EditView.ResponseMap = form.ResponseMap;
                    }
                }
            }
        }
    }
}
