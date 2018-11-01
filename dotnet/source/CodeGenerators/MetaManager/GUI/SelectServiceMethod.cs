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
using NHibernate;


namespace Cdc.MetaManager.GUI
{
    public partial class SelectServiceMethod : MdiChildForm
    {
        private IApplicationService appService = null;

        private ToolTip tooltip = new ToolTip();

        public string ViewName { private get; set; }
        public IList<string> Queries { private get; set; }
        private IList<ServiceMethod> ServiceMethods { get; set; }

        // Dictionary that will contain the ID for the servicemethod and the SQL text or
        // the name of the stored procedure.
        private Dictionary<int, string> serviceMethodSqlDict = new Dictionary<int, string>();

        public ServiceMethod SelectedServiceMethod 
        {
            get
            {
                if (lvServiceMethods.SelectedItems.Count == 1)
                {
                    return (ServiceMethod)lvServiceMethods.SelectedItems[0].Tag;
                }
                else
                {
                    return null;
                }
            }
        }

        public SelectServiceMethod()
        {
            InitializeComponent();

            ServiceMethods = null;
            appService = MetaManagerServices.GetApplicationService();
        }

        private void SelectServiceMethod_Load(object sender, EventArgs e)
        {
            tbViewName.Text = ViewName;

            // Populate the ServiceMethod list
            ServiceMethods = appService.GetAllServiceMethodsToQueriesByApplication(BackendApplication.Id);        
            
            tooltip.Active = !chkDisableTooltip.Checked;
        }
                
        private void ShowServiceMethods()
        {
            btnOK.Enabled = false;

            if (ServiceMethods != null)
            {
                lvServiceMethods.Items.Clear();

                foreach (ServiceMethod serviceMethod in this.ServiceMethods)
                {
                    // Check name, caption and originaldialog
                    if (serviceMethod.Service.Name.ToUpper().Contains(tbService.Text.ToUpper()) &&
                        serviceMethod.Name.ToUpper().Contains(tbServiceMethod.Text.ToUpper()) &&
                        (
                         (serviceMethod.MappedToAction.Query != null &&
                          serviceMethod.MappedToAction.Query.Name.ToUpper().Contains(tbQuery.Text.ToUpper())
                         )
                         ||
                         (serviceMethod.MappedToAction.StoredProcedure != null &&
                          serviceMethod.MappedToAction.StoredProcedure.ProcedureName.ToUpper().Contains(tbQuery.Text.ToUpper()))
                        ))
                    {
                        ListViewItem item = new ListViewItem(serviceMethod.Id.ToString());
                        item.SubItems.Add(serviceMethod.Service.Name);
                        item.SubItems.Add(serviceMethod.Name);

                        if (serviceMethod.MappedToAction.Query != null)
                            item.SubItems.Add(serviceMethod.MappedToAction.Query.Name);
                        else
                            item.SubItems.Add(serviceMethod.MappedToAction.StoredProcedure.ProcedureName);

                        item.Tag = serviceMethod;
                        lvServiceMethods.Items.Add(item);
                    }
                }
            }
        }
                
        private void lvServiceMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = false;

            if (lvServiceMethods.SelectedItems.Count > 0)
            {
                btnOK.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            ShowServiceMethods();
        }

        private void lvServiceMethods_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            ServiceMethod serviceMethod = e.Item.Tag as ServiceMethod;

            if (serviceMethod != null &&
                NHibernateUtil.IsInitialized(serviceMethod.MappedToAction) &&
                serviceMethod.MappedToAction != null &&
                NHibernateUtil.IsInitialized(serviceMethod.MappedToAction.Query) &&
                serviceMethod.MappedToAction.Query != null)
            {
                tooltip.SetToolTip(lvServiceMethods, serviceMethod.MappedToAction.Query.SqlStatement.Trim());
            }
        }

        private void chkDisableTooltip_CheckedChanged(object sender, EventArgs e)
        {
            tooltip.Active = !chkDisableTooltip.Checked;
        }

    }
}
