using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.DirectoryServices;
using System.Security.Principal;
using Imi.SupplyChain.Deployment.IISHandling;
using Imi.SupplyChain.Deployment.Entities;

namespace Imi.SupplyChain.Deployment.UI
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationSettings Config { get; set; }
        public ProductStandardList ProductList { get; set; }

        public string StageArea { get; set; }
        public string WebserverName { get; set; }
        public string WebserverPort { get; set; }
        public string MainVirtualDirectoryName { get; set; }
        public string MainVirtualDirectoryPath { get; set; }
        public string GlobalClickOnceVersion { get; set; }
        public string InternetGuestAccount { get; set; }
        public string CertificateFile { get; set; }
        public bool AskForCertificatePassword { get; set; }

        private bool EditingStageArea { get; set; }

        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            // Load values from configuration.
            StageArea = Config.GetStagingArea();
            WebserverName = Config.GetWebserverName();
            WebserverPort = Config.GetWebserverPort();
            MainVirtualDirectoryName = Config.GetMainVirtualDirectoryName();
            MainVirtualDirectoryPath = Config.GetMainVirtualDirectoryPath();
            InternetGuestAccount = Config.GetInternetGuestAccount();
            CertificateFile = Config.GetCertificateFile();
            AskForCertificatePassword = Config.GetAskForCertificatePassword();

            // Get the version of the IIS
            Version iisVersion = IISHelper.GetVersion();
            lblIISVersion.Text = iisVersion.ToString();

            // Check if stage area is not set or doesn't exist
            if (!string.IsNullOrEmpty(StageArea) && Directory.Exists(StageArea))
            {
                EditingStageArea = false;
                DisableStageAreaEditing();
            }
            else
            {
                // Check if empty values then fill in with defaults
                if (string.IsNullOrEmpty(StageArea))
                {
                    StageArea = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                                            , Constants.DefaultStageAreaDirectoryName);
                }

                EditingStageArea = true;
            }

            // Check if empty values then fill in with defaults
            if (string.IsNullOrEmpty(WebserverName))
            {
                WebserverName = System.Environment.MachineName;
            }

            // Check if empty values then fill in with defaults
            if (string.IsNullOrEmpty(WebserverPort))
            {
                WebserverPort = Constants.DefaultWebserverPort;
            }

            if (string.IsNullOrEmpty(MainVirtualDirectoryName))
            {
                MainVirtualDirectoryName = Constants.DefaultVirtualDirectoryName;
            }

            if (string.IsNullOrEmpty(MainVirtualDirectoryPath))
            {
                if (Directory.Exists(IISHelper.GetWWWRootPath()))
                {
                    MainVirtualDirectoryPath = Path.Combine(IISHelper.GetWWWRootPath(), Constants.DefaultVirtualDirectoryName);
                }
            }

            if (string.IsNullOrEmpty(InternetGuestAccount))
            {
                if (iisVersion.Major < 7)
                    tbInternetGuestAccount.Text = string.Format(@"{0}\IUSR_{0}", System.Environment.MachineName);
                else
                    tbInternetGuestAccount.Text = "IUSR";
            }
            else
                tbInternetGuestAccount.Text = InternetGuestAccount;

            if (string.IsNullOrEmpty(CertificateFile))
                tbCertificateFile.Text = Path.Combine(Application.StartupPath, Constants.CertificateFilename);
            else
                tbCertificateFile.Text = CertificateFile;

            cbAskForPassword.Checked = Config.GetAskForCertificatePassword();

            // Show texts
            tbStageArea.Text = StageArea;
            tbWebserverName.Text = WebserverName;
            tbWebserverPort.Text = WebserverPort;
            tbVirtualDirectory.Text = MainVirtualDirectoryName;
            tbVirtualDirectoryPath.Text = MainVirtualDirectoryPath;
        }

        private void DisableStageAreaEditing()
        {
            btnStageBrowse.Visible = false;
            tbStageArea.ReadOnly = true;
            tbStageArea.TabStop = false;
            tbStageArea.Width = tbWebserverName.Width;
        }

        private void tbWebserverName_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void CheckEnableOK()
        {
            btnOk.Enabled = false;

            if ((!EditingStageArea ||
                 (EditingStageArea &&
                  !string.IsNullOrEmpty(tbStageArea.Text.Trim()))) &&
                !string.IsNullOrEmpty(tbWebserverName.Text.Trim()) &&
                !string.IsNullOrEmpty(tbVirtualDirectory.Text.Trim()) &&
                !string.IsNullOrEmpty(tbVirtualDirectoryPath.Text.Trim()) &&
                !string.IsNullOrEmpty(tbInternetGuestAccount.Text.Trim()) &&
                !string.IsNullOrEmpty(tbCertificateFile.Text.Trim()))
            {
                btnOk.Enabled = true;
            }
        }

        private void tbVirtualDirectory_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void tbVirtualDirectoryPath_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void tbInternetGuestAccount_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void tbStageArea_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void tbCertificateFile_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (EditingStageArea &&
                !Directory.Exists(tbStageArea.Text.Trim()))
            {
                if (MessageBox.Show("The Staging Area does not exist.\nDo you want to create the directory?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(tbStageArea.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("There was an error when trying to create the directory.\n\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbStageArea.Focus();
                        tbStageArea.SelectAll();
                        return;
                    }
                }
                else
                {
                    tbStageArea.Focus();
                    tbStageArea.SelectAll();
                    return;
                }
            }

            if (string.IsNullOrEmpty(tbWebserverName.Text.Trim()))
            {
                MessageBox.Show("Webserver Name can not be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbWebserverName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tbVirtualDirectory.Text.Trim()))
            {
                MessageBox.Show("Main Virtual Directory can not be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbVirtualDirectory.Focus();
                return;
            }

            if (!Directory.Exists(tbVirtualDirectoryPath.Text.Trim()))
            {
                if (MessageBox.Show("The Virtual Directory does not exist. Do you want to create it?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(tbVirtualDirectoryPath.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Unable to create the directory.\n\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tbVirtualDirectoryPath.Focus();
                        tbVirtualDirectory.SelectAll();
                        return;
                    }
                }
                else
                {
                    tbVirtualDirectoryPath.Focus();
                    tbVirtualDirectory.SelectAll();
                    return;
                }
            }

            // Check the virtual directory
            if (!CheckVirtualDirectory(tbVirtualDirectory.Text.Trim(), tbVirtualDirectoryPath.Text.Trim()))
            {
                return;
            }

            // Check internet guest account
            if (!AccessControlList.AccountExist(tbInternetGuestAccount.Text.Trim()))
            {
                MessageBox.Show("The Internet Guest Account is not an existing account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbInternetGuestAccount.Focus();
                return;
            }

            // Check if Internet Guest Account is changed
            if (InternetGuestAccount != tbInternetGuestAccount.Text.Trim())
            {
                // Check if any products are published since the deploy manifest file
                // should have the internet guest account in the ACL.
                if (ProductList.AnyPublished())
                {
                    if (MessageBox.Show("There are one or more published products, and since the Internet Guest Account has\n" +
                                        "changed the published manifest files, for each published instance, needs to be updated\n" +
                                        "with the correct Internet Guest Account so that they will continue to function correctly.\n" +
                                        "\nDo you want to continue with the change?",
                                        "Question",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }

                    string errorText = string.Empty;

                    foreach (ProductStandard product in ProductList)
                    {
                        try
                        {
                            product.Instances.UpdateRightsOnManifestFiles(product.InstallPath, tbInternetGuestAccount.Text.Trim(), InternetGuestAccount);
                        }
                        catch (Exception ex)
                        {
                            errorText += ex.Message + Environment.NewLine;
                        }
                    }

                    if (!string.IsNullOrEmpty(errorText))
                    {
                        MessageBox.Show(string.Format("One or more files couldn't have the ACL updated:\n" +
                                                      "\n{0}\n" +
                                                      "\nEither update the files manually or try to unpublish and publish the products again.", errorText),
                                                      "Error",
                                                      MessageBoxButtons.OK,
                                                      MessageBoxIcon.Error);
                    }
                }
            }

            // All ok, save the values to the configuration
            if (EditingStageArea)
            {
                Config.SetStagingArea(tbStageArea.Text.Trim());
            }

            Config.SetWebserverPort(tbWebserverPort.Text.Trim());
            Config.SetWebserverName(tbWebserverName.Text.Trim());
            Config.SetMainVirtualDirectoryName(tbVirtualDirectory.Text.Trim());
            Config.SetMainVirtualDirectoryPath(tbVirtualDirectoryPath.Text.Trim());
            Config.SetInternetGuestAccount(tbInternetGuestAccount.Text.Trim());
            Config.SetCertificateFile(tbCertificateFile.Text.Trim());
            Config.SetAskForCertificatePassword(cbAskForPassword.Checked);
            Config.Save();

            DialogResult = DialogResult.OK;
        }

        private bool CheckVirtualDirectory(string virtualDirName, string virtualDirPath)
        {
            // Check if virtual directory exist and if the path is the same
            VirtualDirectory virDirHandler = new VirtualDirectory();

            // Try to fetch the virtual directory
            DirectoryEntry virtualDirectory = virDirHandler.Get(virtualDirName);

            // Check if virtual directory already exist
            if (virtualDirectory != null)
            {
                // Set the virtual directory to handle
                virDirHandler.VDir = virtualDirectory;

                try
                {
                    // Check that the directory has the same physical path
                    DirectoryInfo virDirInfo = new DirectoryInfo((string)virtualDirectory.Properties["Path"].Value);
                    DirectoryInfo typedDirInfo = new DirectoryInfo(virtualDirPath);

                    if (virDirInfo.FullName != typedDirInfo.FullName)
                    {
                        if (MessageBox.Show("The Virtual Directory exists but have a different physical path. Do you want to update the Virtual Directory to the new path?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            if (!virDirHandler.UpdatePhysicalPath(virtualDirPath))
                            {
                                MessageBox.Show("Couldn't update the Virtual Directory with the new path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't read directory information.\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbVirtualDirectoryPath.Text.Trim()) &&
                 Directory.Exists(tbVirtualDirectoryPath.Text.Trim()))
            {
                folderBrowse.SelectedPath = tbVirtualDirectoryPath.Text.Trim();
            }
            else
                folderBrowse.SelectedPath = string.Empty;

            if (folderBrowse.ShowDialog() == DialogResult.OK)
            {
                tbVirtualDirectoryPath.Text = folderBrowse.SelectedPath;
            }
        }

        private void btnStageBrowse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbStageArea.Text.Trim()) &&
                 Directory.Exists(tbStageArea.Text.Trim()))
            {
                folderBrowse.SelectedPath = tbStageArea.Text.Trim();
            }
            else
                folderBrowse.SelectedPath = string.Empty;

            if (folderBrowse.ShowDialog() == DialogResult.OK)
            {
                tbStageArea.Text = folderBrowse.SelectedPath;
            }
        }

        private void btnCertificateFileBrowse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbCertificateFile.Text.Trim()) &&
                 File.Exists(tbCertificateFile.Text.Trim()))
            {
                pfxFileBrowse.FileName = tbCertificateFile.Text.Trim();
            }
            else
                pfxFileBrowse.FileName = string.Empty;

            if (pfxFileBrowse.ShowDialog() == DialogResult.OK)
            {
                tbCertificateFile.Text = pfxFileBrowse.FileName;
            }
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

    }
}
