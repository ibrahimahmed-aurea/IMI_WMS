using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Context;
using Spring.Context.Support;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic;
using NHibernate;


namespace Cdc.MetaManager.GUI
{
    public partial class FindServiceMethodForm : MdiChildForm
    {
        private static IApplicationContext ctx;
        private IApplicationService applicationService;

        private ToolTip tooltip = new ToolTip();

        public ServiceMethod ServiceMethod
        {
            get
            {
                if (entityListView.SelectedItems.Count == 0)
                    return null;

                return (ServiceMethod)entityListView.SelectedItems[0].Tag;
            }
        }

        public string AutoSearchName { get; set; }
        public string AutoSearchService { get; set; }

        public FindServiceMethodForm()
        {
            InitializeComponent();
            ctx = ContextRegistry.GetContext();
            applicationService = MetaManagerServices.GetApplicationService();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            Config.Backend.LastSelectedServiceInFindServiceMethod = serviceTbx.Text;
            Config.Save();

            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                {
                    IServiceMethodDao serviceMethodDao = ctx["ServiceMethodDao"] as IServiceMethodDao;

                    IList<Cdc.MetaManager.DataAccess.Domain.ServiceMethod> serviceMethods = serviceMethodDao.FindByApplicationIdNameAndService(BackendApplication.Id, string.IsNullOrEmpty(nameTbx.Text) ? "%" : nameTbx.Text, string.IsNullOrEmpty(serviceTbx.Text) ? "%" : serviceTbx.Text);

                    entityListView.BeginUpdate();

                    entityListView.Items.Clear();

                    foreach (Cdc.MetaManager.DataAccess.Domain.ServiceMethod serviceMethod in serviceMethods)
                    {
                        ListViewItem item = entityListView.Items.Add(serviceMethod.Id.ToString());
                        item.SubItems.Add(serviceMethod.Name);
                        item.SubItems.Add(serviceMethod.Service.Name);
                        item.Tag = serviceMethod;
                    }

                    entityListView.EndUpdate();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (ServiceMethod == null)
                return;

            DialogResult = DialogResult.OK;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (MessageBox.Show("Do you want to save any changes made to this service method?", "Service Method", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IServiceMethodDao serviceDao = ctx["ServiceMethodDao"] as IServiceMethodDao;
                serviceDao.SaveOrUpdate(propertyGrid.SelectedObject as Cdc.MetaManager.DataAccess.Domain.ServiceMethod);
            }

        }

        private void nameTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void entityTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void entityListView_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            ServiceMethod serviceMethod = e.Item.Tag as ServiceMethod;

            if (serviceMethod != null &&
                NHibernateUtil.IsInitialized(serviceMethod.MappedToAction) &&
                serviceMethod.MappedToAction != null)
            {
                if (NHibernateUtil.IsInitialized(serviceMethod.MappedToAction.Query) &&
                    serviceMethod.MappedToAction.Query != null)
                {
                    tooltip.SetToolTip(entityListView, serviceMethod.MappedToAction.Query.SqlStatement.Trim());
                }
                else if (NHibernateUtil.IsInitialized(serviceMethod.MappedToAction.StoredProcedure) &&
                    serviceMethod.MappedToAction.StoredProcedure != null &&
                    NHibernateUtil.IsInitialized(serviceMethod.MappedToAction.StoredProcedure.Package) &&
                    serviceMethod.MappedToAction.StoredProcedure.Package != null)
                {
                    tooltip.SetToolTip(entityListView, string.Format("Stored Procedure: {0}.{1}",
                                            serviceMethod.MappedToAction.StoredProcedure.Package.Name,
                                            serviceMethod.MappedToAction.StoredProcedure.ProcedureName));
                }
            }
        }

        private void entityListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            propertyGrid.SelectedObject = ServiceMethod;

            if (ServiceMethod != null &&
                ServiceMethod.MappedToAction != null &&
                ServiceMethod.MappedToAction.MappedToObject != null)
            {
                if (ServiceMethod.MappedToAction.Query != null)
                    rtbMappedTo.Text = ServiceMethod.MappedToAction.Query.SqlStatement;
                else
                    rtbMappedTo.Text = string.Format("{0}.{1}", ServiceMethod.MappedToAction.StoredProcedure.Package.Name, ServiceMethod.MappedToAction.StoredProcedure.ProcedureName);
            }
            else
                rtbMappedTo.Text = "";

            EnableDisableButtons();
        }
                
        private void EnableDisableButtons()
        {
            okBtn.Enabled = false;
            
            if (ServiceMethod != null)
            {
                okBtn.Enabled = true;
            }
        }

        private void FindServiceMethodForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AutoSearchName) ||
                !string.IsNullOrEmpty(AutoSearchService))
            {
                nameTbx.Text = AutoSearchName;
                serviceTbx.Text = AutoSearchService;

                searchBtn_Click(this, null);
            }
            else
            {
                serviceTbx.Text = Config.Backend.LastSelectedServiceInFindServiceMethod;
            }

            EnableDisableButtons();
        }
    }
}
