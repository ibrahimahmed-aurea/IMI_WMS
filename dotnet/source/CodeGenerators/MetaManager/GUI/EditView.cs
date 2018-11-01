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
    public partial class EditView : MdiChildForm
    {
        public Cdc.MetaManager.DataAccess.Domain.View View { get; set; }

        private IDialogService dialogService = null;
        private IModelService modelService = null;

        public EditView()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void EditView_Load(object sender, EventArgs e)
        {
            if (View != null)
            {
                // Read the dialog to get the latest info from database
                View = modelService.GetInitializedDomainObject<DataAccess.Domain.View>(View.Id);

                tbName.Text = View.Name;
                tbTitle.Text = View.Title;
                LayoutManualyAdaptedcheckBox.Checked = View.LayoutManualyAdapted;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check name
            if (!NamingGuidance.CheckViewName(tbName.Text.Trim(), true))
                return;

            // Check title
            if (!NamingGuidance.CheckCaptionFocus(tbTitle, "View Title", true))
                return;

            // Check unique name

            IList<Cdc.MetaManager.DataAccess.Domain.View> viewList = dialogService.GetViewsByNameAndApplicationId(tbName.Text, FrontendApplication.Id);

            if (viewList.Count > 1)
            {
                MessageBox.Show("The name of the view is not unique, please change the value for the view name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbName.SelectAll();
                tbName.Focus();
                return;
            }
             

            View.Name = tbName.Text.Trim();
            View.Title = tbTitle.Text.Trim();
            View.LayoutManualyAdapted = LayoutManualyAdaptedcheckBox.Checked;

            // Save View
            modelService.SaveDomainObject(View);
            

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
