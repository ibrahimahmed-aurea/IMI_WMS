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
    public partial class StatusAddToSourceControl : MdiChildForm
    {
        private DeploymentGroup deploymentGroup = null;
        private IConfigurationManagementService configurationManagementService = null;

        public StatusAddToSourceControl(DeploymentGroup deployGrp)
        {
            InitializeComponent();
            deploymentGroup = deployGrp;
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
            configurationManagementService.StatusChanged += new StatusChangedDelegate(confMgnService_StatusChanged);
        }

        void confMgnService_StatusChanged(string message, int value, int min, int max)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    Statuslabel.Text = message;
                    StatusprogressBar.Minimum = min;
                    StatusprogressBar.Maximum = max;
                    StatusprogressBar.Value = value;
                    if (message == "Add to source control done!")
                    {
                        Closebutton.Enabled = true;
                    }
                })
                );
            }
            else
            {
                Statuslabel.Text = message;
                StatusprogressBar.Minimum = min;
                StatusprogressBar.Maximum = max;
                StatusprogressBar.Value = value;
                if (message == "Add to source control done!")
                {
                    Closebutton.Enabled = true;
                }
            }
        }

        private void StatusAddToSourceControl_Load(object sender, EventArgs e)
        {
            
        }

        private void ThreadWork(object state)
        {
            try
            {
                configurationManagementService.AddDomainToConfMgn(deploymentGroup, CheckIncheckBox.Checked, FrontendcheckBox.Checked, backendcheckBox.Checked);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Startbutton_Click(object sender, EventArgs e)
        {
            Closebutton.Enabled = false;
            System.Threading.ThreadPool.QueueUserWorkItem(ThreadWork); 
        }
    }
}
