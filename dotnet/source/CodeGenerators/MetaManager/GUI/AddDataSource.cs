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
using Cdc.MetaManager.DataAccess;


namespace Cdc.MetaManager.GUI
{
    public partial class AddDataSource : MdiChildForm
    {
        public DataSource DataSource { get; set; }
        public Cdc.MetaManager.DataAccess.Domain.View View { get; set; }
        private bool IsEditing;

        private IApplicationService appService = null;
        private IDialogService dialogService = null;

        private IModelService modelService = null;

        public AddDataSource()
        {
            InitializeComponent();

            DataSource = null;

            // Get the Services
            appService = MetaManagerServices.GetApplicationService();
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void AddDataSource_Load(object sender, EventArgs e)
        {
            if (DataSource == null)
            {
                DataSource = new DataSource();
                DataSource.View = View;
                IsEditing = false;
            }
            else
            {
                this.Text = "Edit DataSource";
                IsEditing = true;

                tbName.Text = DataSource.Name;
                SetServiceMethodText();
            }

            EnableDisableButtons();
        }

        private void SetServiceMethodText()
        {
            tbServiceMethod.Text = string.Format("({0}) {1}", DataSource.ServiceMethod.Id, DataSource.ServiceMethod.Name);
        }

        private void EnableDisableButtons()
        {
            btnSelectServiceMethod.Enabled = false;
            btnOK.Enabled = false;

            if (!IsEditing || DataSource.ServiceMethod == null)
            {
                btnSelectServiceMethod.Enabled = true;
            }

            if (!string.IsNullOrEmpty(tbName.Text) &&
                DataSource.ServiceMethod != null)
            {
                btnOK.Enabled = true;
            }
        }

        private void btnSelectServiceMethod_Click(object sender, EventArgs e)
        {
            using (FindServiceMethodForm findServiceMethod = new FindServiceMethodForm())
            {
                findServiceMethod.Owner = this;
                findServiceMethod.FrontendApplication = FrontendApplication;
                findServiceMethod.BackendApplication = BackendApplication;

                if (findServiceMethod.ShowDialog() == DialogResult.OK)
                {
                    // Create the Datasource with maps
                    DataSource = dialogService.CreateDataSourceMaps(DataSource, View, findServiceMethod.ServiceMethod.Id);

                    // Set the text for the ServiceMethod
                    SetServiceMethodText();

                    // Set a defaultname if name isn't set yet
                    if (string.IsNullOrEmpty(tbName.Text))
                    {
                        tbName.Text = DataSource.ServiceMethod.Name;
                    }

                    // Enable buttons
                    EnableDisableButtons();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckNameFocus(tbName, "Datasource Name", true))
                return;

            // Fetch all datasourcenames in a list
            IList<string> dataSourceNames = (from ds in View.DataSources
                                             where ds.Id != DataSource.Id
                                             select ds.Name).ToList();

            // Check name of Datasource
            if (!NamingGuidance.CheckNameNotInList(tbName.Text.Trim(), "Datasource Name", "Datasource List", dataSourceNames, false, true))
                return;

            DataSource.Name = tbName.Text.Trim();

            // Save the datasource
            modelService.SaveDomainObject(DataSource);

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }
    }

}
