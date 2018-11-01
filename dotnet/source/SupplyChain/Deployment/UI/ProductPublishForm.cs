using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Imi.SupplyChain.Deployment.Entities;
using Imi.SupplyChain.Deployment.IISHandling;
using System.DirectoryServices;

namespace Imi.SupplyChain.Deployment.UI
{
    public partial class ProductPublishForm : Form
    {
        public string VirtualDirectoryName = string.Empty;
        public string VirtualDirectoryPath = string.Empty;

        public ProductPublishForm()
        {
            InitializeComponent();
        }

        private void CheckEnableOK()
        {
            btnOk.Enabled = false;

            if (!string.IsNullOrEmpty(tbVirtualDirectoryName.Text))
            {
                btnOk.Enabled = true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!VerifyString.IsWord(tbVirtualDirectoryName.Text))
            {
                MessageBox.Show("Virtual Directory Name is not valid. Valid characters are a-z, A-Z, 0-9 and underscore.");
                tbVirtualDirectoryName.Focus();
                return;
            }

            // Set the virtual directory
            VirtualDirectoryName = tbVirtualDirectoryName.Text;

            // Create the Virtual Directory
            if (!CreateVirtualDirectory(VirtualDirectoryName, VirtualDirectoryPath))
            {
                tbVirtualDirectoryName.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbVirtualDirectoryRoot_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private bool CreateVirtualDirectory(string virtualDirName, string virtualDirPath)
        {
            // Check if virtual directory exist and if the path is the same
            VirtualDirectory virDirHandler = new VirtualDirectory();

            // Try to fetch the virtual directory
            DirectoryEntry virtualDirectory = virDirHandler.Get(virtualDirName);

            // Check if virtual directory already exist
            if (virtualDirectory != null)
            {
                MessageBox.Show("The Virtual Directory already exists, change the name and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                // Create the Virtual Directory
                if (!virDirHandler.CreateVirtual(virtualDirName, virtualDirPath))
                {
                    MessageBox.Show("Couldn't create the Virtual Directory on the Webserver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void ProductPublish_Load(object sender, EventArgs e)
        {
            tbVirtualDirectoryName.Text = VirtualDirectoryName;

            CheckEnableOK();
        }

    }
}
