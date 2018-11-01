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
    public partial class CreateEditServiceMethod : MdiChildForm
    {
        public Service Service { get; set; }
        private ServiceMethod serviceMethod = null;

        private IApplicationService appService = null;

        private IModelService modelService = null;
        private IConfigurationManagementService configurationManagementService = null;

        public DataAccess.Domain.Action ActionOwner = null;

        public CreateEditServiceMethod()
        {
            InitializeComponent();

            appService = MetaManagerServices.GetApplicationService();

            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
        }

        private void CreateEditServiceMethod_Load(object sender, EventArgs e)
        {
            IList<DataAccess.Domain.Action> actions = modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Action>(BackendApplication.Id);
            actions = actions.OrderBy(action => action.Name).ToList();
            cbAction.DisplayMember = "Name";
            cbAction.Items.Add("<New Action>");
            cbAction.Items.AddRange(actions.ToArray());
 
            if (ContaindDomainObjectIdAndType.Key == Guid.Empty)
            {
                serviceMethod = new ServiceMethod();
                serviceMethod.Service = Service;
                
                if (ActionOwner != null)
                {
                    tbServiceMethodName.Text = ActionOwner.Name;
                    cbAction.SelectedItem = cbAction.Items.OfType<DataAccess.Domain.Action>().Where<DataAccess.Domain.Action>(o => o.Id == ActionOwner.Id).First();
                    cbAction.Enabled = false;
                }
                else
                {
                    cbAction.SelectedIndex = 0;
                }

                IsEditable = true;
            }
            else
            {
                serviceMethod = modelService.GetInitializedDomainObject<ServiceMethod>(ContaindDomainObjectIdAndType.Key);
                Service = modelService.GetDomainObject<Service>(serviceMethod.Service.Id);

                IsEditable = serviceMethod.IsLocked && serviceMethod.LockedBy == Environment.UserName;

                tbServiceMethodName.Text = serviceMethod.Name;
                cbAction.SelectedItem = cbAction.Items.OfType<DataAccess.Domain.Action>().Where<DataAccess.Domain.Action>(o => o.Id == serviceMethod.MappedToAction.Id).First();
            }

            TypeDescriptor.AddAttributes(serviceMethod, new Attribute[] { new ReadOnlyAttribute(true) });
            ObjectPropertyGrid.SelectedObject = serviceMethod;

            if (!IsEditable)
            {
                tbServiceMethodName.ReadOnly = true;
                btnOk.Enabled = false;
                cbAction.Enabled = false;
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool isNew = false;
            
            if (IsEditable)
            {
                Cursor.Current = Cursors.WaitCursor;
                isNew = serviceMethod.Id == Guid.Empty;

                if (!NamingGuidance.CheckNameEmptyFocus(tbServiceMethodName, "ServiceMethod Name", true))
                {
                    return;
                }

                if (string.IsNullOrEmpty(serviceMethod.Name) || serviceMethod.Name.ToUpper() != tbServiceMethodName.Text.Trim().ToUpper())
                {
                    IEnumerable<string> serviceMethodNames = modelService.GetAllDomainObjectsByApplicationId<ServiceMethod>(BackendApplication.Id).Where(s => s.Service.Id == Service.Id).Select<ServiceMethod, string>(s => s.Name); 

                    if (!NamingGuidance.CheckNameNotInList(tbServiceMethodName.Text.Trim(), "ServiceMethod Name", "List of ServiceMethods", serviceMethodNames, false, true))
                        return;
                }


                serviceMethod.Name = tbServiceMethodName.Text;

                if (!typeof(DataAccess.Domain.Action).IsAssignableFrom(cbAction.SelectedItem.GetType()))
                {
                    if (!NHibernate.NHibernateUtil.IsInitialized(Service))
                    {
                        Service = modelService.GetInitializedDomainObject<Service>(Service.Id);
                    }

                    CreateEditActionWizard form = new CreateEditActionWizard();
                    form.FrontendApplication = FrontendApplication;
                    form.BackendApplication = BackendApplication;
                    form.ServiceMethodOwner = serviceMethod;
                    
                    Dictionary<string, object> criteria = new Dictionary<string,object>();
                    criteria.Add("Name", Service.Name);
                    
                    IList<BusinessEntity> entityList = modelService.GetAllDomainObjectsByPropertyValues<BusinessEntity>(criteria);

                    if (entityList.Count > 0)
                    {
                        form.BusinessEntity = entityList[0];
                    }

                    form.ShowDialog();

                    if (form.ContaindDomainObjectIdAndType.Key == Guid.Empty)
                    {
                        return;    
                    }

                    serviceMethod.MappedToAction = form.Action;
                }
                else
                {
                    serviceMethod.MappedToAction = (DataAccess.Domain.Action)cbAction.SelectedItem;
                }
                
                serviceMethod = MetaManagerServices.Helpers.ServiceMethodHelper.SaveAndSynchronize(serviceMethod);

                if (isNew)
                {
                    try
                    {
                        configurationManagementService.CheckOutDomainObject(serviceMethod.Id, typeof(ServiceMethod));
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }

                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(serviceMethod.Id, typeof(ServiceMethod));

                Cursor.Current = Cursors.Default;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
