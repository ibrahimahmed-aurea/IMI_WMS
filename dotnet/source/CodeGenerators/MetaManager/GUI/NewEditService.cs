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
    public partial class NewEditService : MdiChildForm
    {
        private Service Service = null;
        private bool Editing = false;


        private IApplicationService appService = null;
        private IModelService modelService = null;

        public Service getService
        {
            get { return Service; }
        }

        public NewEditService()
        {
            InitializeComponent();

            // Get service contexts
            appService = MetaManagerServices.GetApplicationService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void NewEditService_Load(object sender, EventArgs e)
        {
            if (ContaindDomainObjectIdAndType.Key != Guid.Empty)
            {
                Service = modelService.GetInitializedDomainObject<Service>(ContaindDomainObjectIdAndType.Key);
                Editing = true;
                tbName.Text = Service.Name;
                cbIsMobile.Checked = Service.IsMobile;
                if (Service.IsLocked && Service.LockedBy == Environment.UserName)
                {
                    this.IsEditable = true;
                }
            }
            else
            {
                // Create the new Service
                Service = new Service();
                Service.Application = BackendApplication;
                Service.IsMobile = false;
                Editing = false;
                this.IsEditable = true;
            }
            EnableDisableComponents();

            tbapp.Text = string.Format("{0}", Service.Application.Name);

            Cursor.Current = Cursors.Default;
        }

        private void EnableDisableComponents()
        {
            tbName.ReadOnly = !this.IsEditable;
            btnOk.Enabled = this.IsEditable;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!CheckIfCanSave())
            {
                return;
            }


            BackendApplication = modelService.GetInitializedDomainObject<DataAccess.Domain.Application>(BackendApplication.Id);


            // Set values for the service
            Service.Name = tbName.Text.Trim();
            Service.IsMobile = cbIsMobile.Checked;
            Service.Application = BackendApplication;

            // Save the Service
            modelService.SaveDomainObject(Service);

            if (!Editing)
            {
                CheckOutInObject(Service, true);
                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(Service.Id, typeof(Service));
            }


            DialogResult = DialogResult.OK;

        }

        private bool CheckIfCanSave()
        {
            // Check name
            if (!NamingGuidance.CheckNameFocus(tbName, "Service Name", true))
            {
                return false;
            }

            // Check if the service already exist
            if (!Editing ||
                (Editing &&
                 Service.Name.ToUpper() != tbName.Text.Trim().ToUpper()))
            {

                IEnumerable<string> serviceExistList = modelService.GetAllDomainObjectsByApplicationId<Service>(BackendApplication.Id).Select(s => s.Name);

                if (!NamingGuidance.CheckNameNotInList(tbName.Text.Trim(), "Service Name", "List of Services", serviceExistList, false, true))
                {
                    return false;
                }
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
