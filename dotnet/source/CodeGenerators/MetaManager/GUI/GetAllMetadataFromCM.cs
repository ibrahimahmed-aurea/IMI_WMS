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

using System.IO;

namespace Cdc.MetaManager.GUI
{
    public partial class GetAllMetadataFromCM : MdiChildForm
    {
        private IConfigurationManagementService configurationManagementService = null;

        public GetAllMetadataFromCM()
        {
            InitializeComponent();
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
                    if (message == "All done!")
                    {
                        Closebutton.Enabled = true;
                        this.Close();
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
                if (message == "All done!")
                {
                    Closebutton.Enabled = true;
                    this.Close();
                }
            }
        }

        private void ThreadWork(object state)
        {
            try
            {
                configurationManagementService.ImportDomainObjects(new List<string>() { repositoryPathTbx.Text }, false, ExcludeZeroFilescheckBox.Checked);

                confMgnService_StatusChanged("All done!", 0, 0, 0);
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
            Startbutton.Enabled = false;
            System.Threading.ThreadPool.QueueUserWorkItem(ThreadWork);
        }

        private void browseFolderBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(repositoryPathTbx.Text.Trim()) || !Directory.Exists(repositoryPathTbx.Text))
                folderDlg.SelectedPath = Path.GetFullPath(@"..\..\..\..\..\metadata");
            else if (Directory.Exists(repositoryPathTbx.Text))
                folderDlg.SelectedPath = repositoryPathTbx.Text;
            else
                folderDlg.SelectedPath = string.Empty;

            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                repositoryPathTbx.Text = folderDlg.SelectedPath;
            }
        }

        private void EnableDisableButtons()
        {
            Startbutton.Enabled = false;
            Closebutton.Enabled = true;

            if (!string.IsNullOrEmpty(repositoryPathTbx.Text))
            {
                Startbutton.Enabled = true;
            }
        }

        private void repositoryPathTbx_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }
    }
}
