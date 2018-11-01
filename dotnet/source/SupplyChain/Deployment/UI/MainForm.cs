using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.DirectoryServices;
using System.Xml;
using System.Security.AccessControl;
using System.Threading;
using Ionic.Zip;
using Imi.SupplyChain.Deployment.IISHandling;
using Imi.SupplyChain.Deployment.Entities;
using Imi.SupplyChain.Deployment.Wrappers;

namespace Imi.SupplyChain.Deployment.UI
{
    public partial class MainForm : Form
    {
        // Variables
        private ConfigurationSettings Config = null;
        private ProductStandardList ProductList = null;
        private Instances InstanceList = null;
        public string MageExeFile { get; set; }
        public string StagingAreaPath { get; set; }
        private ToolTip toolTip = new ToolTip();

        // Configuration settings
        public string WebserverName { get; set; }
        public string MainVirtualDirectoryName { get; set; }
        public string MainVirtualDirectoryPath { get; set; }
        public string GlobalClickOnceVersion { get; set; }
        public string LastImportDirectory { get; set; }
        public string InternetGuestAccount { get; set; }
        public string CertificateFile { get; set; }
        public bool AskForCertificatePassword { get; set; }

        public ProductStandard SelectedProduct
        {
            get
            {
                return dgvProducts.SelectedRows.Count == 1 ? (ProductStandard)dgvProducts.SelectedRows[0].Tag : null;
            }
        }

        public Instance SelectedInstance
        {
            get
            {
                return dgvInstances.SelectedRows.Count == 1 ? (Instance)dgvInstances.SelectedRows[0].Tag : null;
            }
        }

        public MainForm()
        {
            this.Hide();
            SplashForm splashForm = new SplashForm();
            Thread ft = new Thread(new ThreadStart(splashForm.Show));

            ft.Start();
            Thread.Sleep(0); // Yield

            InitializeComponent();

            // Initialize variables
            Config = new ConfigurationSettings(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Constants.ConfigurationfileName));
            ProductList = new ProductStandardList();
            InstanceList = new Instances();

            // Fetch Staging Area Path
            StagingAreaPath = Config.GetStagingArea();

            DateTime stop = DateTime.Now.AddSeconds(12);
            splashForm.Fade();
            Thread.Sleep(0); // Yield

            // The timepsan logic is needed for remote desktop
            // where the fade doesn't happen every time for
            // some reason and the splash does not disappear.
            while (splashForm.Opacity > 0)
            {
                Thread.Sleep(10);
                if (DateTime.Now > stop)
                    ft.Interrupt();
            }
            
        }

        private void Main_Load(object sender, EventArgs e)
        {
            bool closing = false;
            
            if (!closing)
            {
                Version iisVersion = IISHelper.GetVersion();

                if (iisVersion == null || (iisVersion < new Version(5, 1)))
                {
                    MessageBox.Show("This application requires Internet Information Services (IIS) version 5.1 or later.\r\n" +
                                    "Please install the correct version of IIS and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    closing = true;
                }
            }

            // Check that "Mage.exe" exists.
            if (!closing && !CheckMageExeExist())
            {
                MessageBox.Show(string.Format("The file \"{0}\" was not found in the application directory.", Constants.MageExeFilename), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                closing = true;
            }

            // Read configuration
            if (!closing)
            {
                ReadConfiguration();
            }

            // Check if we should show configuration dialog
            if (!closing && !CheckConfiguration())
            {
                ConfigurationForm configDialog = new ConfigurationForm();

                configDialog.StartPosition = FormStartPosition.CenterScreen;
                configDialog.Config = Config;
                configDialog.ProductList = ProductList;

                if (configDialog.ShowDialog() == DialogResult.OK)
                {
                    // Reread configuration
                    ReadConfiguration();
                }
                else
                {
                    closing = true;
                }
            }

            if (!closing)
            {
                // Show staging area in statusbar
                tsStagingArea.Text = StagingAreaPath;
            }

            if (!closing && !LoadProductList())
            {
                closing = true;
            }
                        
            if (!closing)
            {
                // Refresh ProductList
                RefreshProductList();

                // Get the instance list
                RefreshInstanceList();

                // Enable disable product buttons
                EnableDisableProductButtons();

                // Enable disable instance buttons
                EnableDisableInstanceButtons();

            }

            // Check if Main Virtual Directory Exists
            if (!closing)
            {
                if (!CheckMainVirtualDirectory())
                {
                    closing = true;
                }
            }

            // Update webpage and navigate
            if (!closing)
            {
                // Create webpage
                UpdateWebpage(false);

                // Navigate to webpage on frontpage
                NavigateToMainWebpage();
            }

            if (closing)
            {
                Close();
            }
        }
                
        private void ReadConfiguration()
        {
            // Load values from configuration.
            StagingAreaPath = Config.GetStagingArea();
            WebserverName = Config.GetWebserverName();
            MainVirtualDirectoryName = Config.GetMainVirtualDirectoryName();
            MainVirtualDirectoryPath = Config.GetMainVirtualDirectoryPath();
            LastImportDirectory = Config.GetLastImportDirectory();
            InternetGuestAccount = Config.GetInternetGuestAccount();
            CertificateFile = Config.GetCertificateFile();
            AskForCertificatePassword = Config.GetAskForCertificatePassword();
        }

        private bool CheckConfiguration()
        {
            if (string.IsNullOrEmpty(StagingAreaPath) ||
                !Directory.Exists(StagingAreaPath) ||
                string.IsNullOrEmpty(WebserverName) ||
                string.IsNullOrEmpty(MainVirtualDirectoryName) ||
                string.IsNullOrEmpty(MainVirtualDirectoryPath) || 
                string.IsNullOrEmpty(InternetGuestAccount) ||
                string.IsNullOrEmpty(CertificateFile) ||
                !File.Exists(CertificateFile))
            {
                return false;
            }
            return true;
        }

        private bool CheckMageExeExist()
        {
            MageExeFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Constants.MageExeFilename);

            return File.Exists(MageExeFile);
        }

        private bool CheckMainVirtualDirectory()
        {
            bool ok = true;

            if (CheckMainVirtualDirectoryExist())
            {
                if (!CheckExistingMainVirtualDirectory())
                {
                    ok = false;
                }
            }
            else
            {
                if (!CreateVirtualDirectory(MainVirtualDirectoryName, MainVirtualDirectoryPath))
                {
                    MessageBox.Show("Error creating the Virtual Directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ok = false;
                }
            }
            return ok;
        }

        private bool CheckExistingMainVirtualDirectory()
        {
            VirtualDirectory virtualDirHandler = new VirtualDirectory();

            // Fetch the virtual directory
            DirectoryEntry virtualDirectory = virtualDirHandler.Get(MainVirtualDirectoryName);

            // Set the virtual directory to handle
            virtualDirHandler.VDir = virtualDirectory;

            // Check that the directory has the same physical path
            DirectoryInfo virDirInfo = new DirectoryInfo((string)virtualDirectory.Properties["Path"].Value);
            DirectoryInfo configDirInfo = new DirectoryInfo(MainVirtualDirectoryPath);

            if (virDirInfo.FullName != configDirInfo.FullName)
            {
                if (MessageBox.Show("The Main Virtual Directory of the Web Server has a different physical path than what is configured.\nDo you want to update the configuration?\n\nIf you answer no then the Virtual Directory will be updated with the path that exists in the configuration file.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    MainVirtualDirectoryPath = (string)virtualDirectory.Properties["Path"].Value;
                    Config.SetMainVirtualDirectoryPath((string)virtualDirectory.Properties["Path"].Value);
                    Config.Save();
                }
                else
                {
                    if (!virtualDirHandler.UpdatePhysicalPath(MainVirtualDirectoryPath))
                    {
                        MessageBox.Show("Couldn't update the Virtual Directory with the new physical path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckMainVirtualDirectoryExist()
        {
            VirtualDirectory virtualDirHandler = new VirtualDirectory();

            // Fetch the virtual directory
            DirectoryEntry virtualDirectory = virtualDirHandler.Get(MainVirtualDirectoryName);

            return virtualDirectory != null;
        }

        private bool CreateVirtualDirectory(string virDirName, string virDirPath)
        {
            VirtualDirectory virtualDirHandler = new VirtualDirectory();
            return virtualDirHandler.CreateVirtual(virDirName, virDirPath);
        }

        private void NavigateToMainWebpage()
        {
            // Set navigate event
            webBrowser.Navigating -= webBrowser_Navigating_CancelAll;

            // Create URL
            string url = string.Format(@"http://{0}:{1}/{2}/",
                                     Config.GetWebserverName(),
                                     Config.GetWebserverPort(),
                                     Config.GetMainVirtualDirectoryName());

            // Set URL to the toolstrip text
            tsAddress.Text = url;

            // Navigate to the webpage
            webBrowser.Navigate(url);
        }

        private void webBrowser_Navigating_CancelAll(object sender, WebBrowserNavigatingEventArgs e)
        {
            // Cancel all navigation in the webbrowser window
            e.Cancel = true;
        }

        private bool LoadProductList()
        {
            // Find all products in the staging area that can have instances.
            string[] productDirs = Directory.GetDirectories(StagingAreaPath);

            // Clear Productlist
            ProductList.Clear();

            // Check if any directories was found
            if (productDirs.GetUpperBound(0) >= 0)
            {
                // Loop through list of directories and find the correct product directories.
                foreach (string productDir in productDirs)
                {
                    // Read the product.xml file from the path
                    Product product = SerializeHandler.GetProduct(productDir);

                    if (product != null)
                    {
                        // Check type of product
                        if (product.ProductType == ProductType.Standard)
                        {
                            ProductList.CreateOrUpdate(product, productDir);
                        }
                    }

                }
            }
            return true;
        }

        private bool CheckCreateTempDir(string tempDir)
        {
            if (!Directory.Exists(tempDir))
            {
                try
                {
                    Directory.CreateDirectory(tempDir);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private void UpdateProductInView(ProductStandard selectProduct)
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                ProductStandard product = (ProductStandard)row.Tag;

                // Update product when found in list
                if (product != null && product == selectProduct)
                {
                    SetProductTextOnRow(row, product);
                    break;
                }
            }

            // Update the product in the list
            UpdateSelectedProduct();
        }

        private void SetProductTextOnRow(DataGridViewRow row, ProductStandard product)
        {
            // ProductName
            row.Cells["colProdName"].Value = product.ProductName;
            
            // Latest Version
            row.Cells["colProdLatestVersion"].Value = product.LatestVersion;

            // IsPublished
            row.Cells["colProdPublished"].Value = product.IsPublished;

            // Set tag for the item
            row.Tag = product;
        }

        private void RefreshProductList()
        {
            RefreshProductList(string.Empty, true);
        }

        private void RefreshProductList(string selectProductId, bool doSort)
        {
            // Clear Product list
            dgvProducts.Rows.Clear();

            // Refresh the product list
            foreach (ProductStandard product in ProductList)
            {
                // Add the row
                int rowIndex = dgvProducts.Rows.Add();

                SetProductTextOnRow(dgvProducts.Rows[rowIndex], product);

                // Select the row if it has the same product id
                if (!string.IsNullOrEmpty(selectProductId) &&
                    product.ProductId == selectProductId)
                {
                    dgvProducts.Rows[rowIndex].Selected = true;
                }
            }

            if (doSort)
                dgvProducts.Sort(colProdName, ListSortDirection.Ascending);

            // Set first product to selected if no product is selected
            if (string.IsNullOrEmpty(selectProductId) && dgvProducts.Rows.Count > 0)
            {
                dgvProducts.Rows[0].Selected = true;
            }
        }

        private void UpdateInstanceInListView(Instance selectInstance)
        {
            foreach (DataGridViewRow row in dgvInstances.Rows)
            {
                Instance instance = (Instance)row.Tag;

                // Update product when found in list
                if (instance != null && instance == selectInstance)
                {
                    SetInstanceTextOnRow(row, instance);
                    break;
                }
            }

            UpdateSelectedInstance();
        }

        private void SetInstanceTextOnRow(DataGridViewRow row, Instance instance)
        {
            // Instance name
            row.Cells["colInstanceName"].Value = instance.Name;

            // Product Version
            row.Cells["colInstanceProductVersion"].Value = instance.ProductVersion;

            // Set tag for the item
            row.Cells["colInstanceVisible"].Value = instance.Visible;

            row.Tag = instance;
        }

        private void RefreshInstanceList()
        {
            RefreshInstanceList(string.Empty);
        }

        private void RefreshInstanceList(string instanceName)
        {
            // Clear list
            dgvInstances.Rows.Clear();

            bool rowSelected = false;

            if (SelectedProduct != null)
            {
                foreach (Instance instance in SelectedProduct.Instances)
                {
                    int rowIndex = dgvInstances.Rows.Add();

                    SetInstanceTextOnRow(dgvInstances.Rows[rowIndex], instance);

                    if (!string.IsNullOrEmpty(instanceName) &&
                        instance.Name == instanceName)
                    {
                        dgvInstances.Rows[rowIndex].Selected = true;
                        rowSelected = true;
                    }
                }
            }

            // Set first instance to selected if no instance is selected
            if (!rowSelected)
            {
                if (dgvInstances.Rows.Count > 0)
                {
                    dgvInstances.Rows[0].Selected = true;
                }
            }
        }

        private void EnableDisableProductButtons()
        {
            btnRemoveProduct.Enabled = false;
            btnPublish.Enabled = false;
            btnUnpublish.Enabled = false;

            if (SelectedProduct != null)
            {
                // Check if product selected is published or not
                if (SelectedProduct.IsPublished)
                {
                    btnUnpublish.Enabled = true;
                }
                else
                {
                    btnPublish.Enabled = true;
                    btnRemoveProduct.Enabled = true;
                }
            }
        }

        private void EnableDisableInstanceButtons()
        {
            btnNewInstance.Enabled = false;
            btnChangeInstance.Enabled = false;
            btnRemoveInstance.Enabled = false;
            btnShowHideInstance.Enabled = false;

            if (tsInstances.ImageList == null)
            {
                tsInstances.ImageList = ilShowHide;
                btnShowHideInstance.ImageKey = "Show";
                btnShowHideInstance.Text = "Show";
            }

            if (SelectedProduct != null)
            {
                // Product is selected, enable New button
                btnNewInstance.Enabled = true;

                if (SelectedInstance != null)
                {
                    // Instance is selected, enable Change and Remove button
                    btnChangeInstance.Enabled = true;
                    btnRemoveInstance.Enabled = true;

                    // If selected instance is not hidden we need to check if there are
                    // any other instances that are not hidden before we enable the button.
                    bool visibleFound = false;

                    foreach (DataGridViewRow row in dgvInstances.Rows)
                    {
                        Instance instance = (Instance)row.Tag;

                        if (instance != null &&
                            !instance.Equals(SelectedInstance))
                        {
                            if (instance.Visible)
                            {
                                visibleFound = true;
                                break;
                            }
                        }
                    }

                    if (visibleFound)
                    {
                        btnShowHideInstance.Enabled = true;
                    }

                    // Set image on button
                    if (SelectedInstance.Visible)
                    {
                        btnShowHideInstance.ImageKey = "Hide";
                        btnShowHideInstance.Text = "Hide";
                    }
                    else
                    {
                        btnShowHideInstance.ImageKey = "Show";
                        btnShowHideInstance.Text = "Show";
                    }
                }
            }
        }

        private void UpdateDeployManifest(Instance instance, bool onlyUpdate, string certificatePassword)
        {
            // Deploy Manifestfile
            string deployManifestFile = Path.Combine(SelectedProduct.InstallPath, instance.DeployManifestFile);

            // Check if only update of existing file is necessary
            if (!onlyUpdate)
            {
                // Temp filename to use
                string tempTemplateFile = Path.Combine(Path.GetTempPath(), Constants.DeployManifestTemplateTemporaryName);

                // Get the template file from the assembly and overwrite the template in temp directory
                if (!FileHandler.GetFileFromAssembly(tempTemplateFile, Assembly.Load("Imi.SupplyChain.Deployment"), Constants.DeployManifestTemplateResource))
                {
                    throw new Exception("Could not copy the deployment manifest to the temporary directory.");
                }

                // Move and rename the template to the correct place.
                try
                {
                    File.Move(tempTemplateFile, deployManifestFile);
                }
                catch
                {
                    throw new Exception("Could not move the deployment manifest.");
                }
            }
                       
            
            // Build the arguments for updating the deploy template with mage.exe
            string argument = string.Format("-Update \"{0}\" " +
                                            "-AppManifest \"{1}\" " +
                                            "-Name \"{2}\" " +
                                            "-Version {3} " +
                                            "-p x86 " + 
                                            "-CertFile \"{4}\"{5} ",
                                            deployManifestFile,
                                            instance.ApplicationManifestFile,
                                            SelectedProduct.ProductName,
                                            instance.ProductVersion,
                                            CertificateFile,
                                            !string.IsNullOrEmpty(certificatePassword) ? string.Format(" -Password {0}", certificatePassword) : string.Empty);

            Regex regEx = new Regex(Constants.DeployManifestSuccessfulPattern);

            string result = RunConsoleApp.Run(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Constants.MageExeFilename), argument);

            // Check if result was successful
            if (!regEx.Match(result).Success)
            {
                if (!onlyUpdate)
                {
                    // Remove the unsigned manifest file.
                    // Only if we're not updating an existing working manifest
                    File.Delete(deployManifestFile);
                }

                if (result.Contains("The specified network password is not correct."))
                    throw new Exception("Invalid password.");
                else
                    throw new Exception(string.Format("Error when running mage.exe.\n\nArgument:\n\t{0}\n\nResult:\n\t{1}", argument, result));
            }
        }

        private bool UpdateWebpage()
        {
            return UpdateWebpage(true);
        }

        private bool UpdateWebpage(bool doRefresh)
        {
            try
            {
                if (WebpageHandler.UpdateWebpage(Config, ProductList))
                {
                    if (doRefresh)
                    {
                        webBrowser.Refresh(WebBrowserRefreshOption.Completely);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error updating the deployment web page.\n{0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void btnNewInstance_Click(object sender, EventArgs e)
        {
            NewInstanceForm newInstance = new NewInstanceForm();

            newInstance.Product = SelectedProduct;
            newInstance.EditInstance = null;

            if (newInstance.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string certificatePassword = string.Empty;

                    // Check if we should ask for password
                    if (AskForCertificatePassword)
                    {
                        if (!AskForPasswordForm.Show(this, out certificatePassword))
                            return;
                    }

                    // Set to Hourglass
                    this.Cursor = Cursors.WaitCursor;

                    // Try to create the new deploy manifest
                    try
                    {
                        UpdateDeployManifest(newInstance.EditInstance, false, certificatePassword);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Error creating the deployment manifest.\n{0}", ex.ToString()), "New Instance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Add the new instance to the product data
                    SelectedProduct.Instances.Add(newInstance.EditInstance);

                    // If the product is published then set the rights on the manifest files
                    if (SelectedProduct.IsPublished)
                    {
                        try
                        {
                            newInstance.EditInstance.UpdateManifestFileSecurity(SelectedProduct.InstallPath, InternetGuestAccount);
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("Error updating the deployment manifest. Unable to add the\n" +
                                                          "Internet Guest Account ({0}) to the Access Control List (ACL).\n" +
                                                          "\nTry republishing the product to solve the problem.", InternetGuestAccount),
                                            "New Instance",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                    }

                    // Save Instance
                    SerializeHandler.SaveProductStandardData(SelectedProduct.GetProductData(), SelectedProduct.InstallPath);

                    // Update Listview
                    RefreshInstanceList(newInstance.EditInstance != null ? newInstance.EditInstance.Name : string.Empty);

                    // Enable disable instance buttons
                    EnableDisableInstanceButtons();

                    // Update main webpage
                    UpdateWebpage();
                }
                finally
                {
                    // Set to Default
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void btnChangeInstance_Click(object sender, EventArgs e)
        {
            NewInstanceForm changeInstance = new NewInstanceForm();

            changeInstance.Product = SelectedProduct;
            changeInstance.EditInstance = SelectedInstance;

            if (changeInstance.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Check if product version is changed. In that case the deploy
                    // manifest should be updated.
                    if (changeInstance.ProductVersionChanged)
                    {
                        string certificatePassword = string.Empty;

                        // Check if we should ask for password
                        if (AskForCertificatePassword)
                        {
                            if (!AskForPasswordForm.Show(this, out certificatePassword))
                                return;
                        }

                        try
                        {
                            // Set to Hourglass
                            this.Cursor = Cursors.WaitCursor;

                            // Try to update the deploy manifest
                            UpdateDeployManifest(changeInstance.EditInstance, true, certificatePassword);
                        }
                        catch (Exception ex)
                        {
                            // Reread the product into the productlist
                            ProductList.CreateOrUpdate(SelectedProduct, SelectedProduct.InstallPath);

                            string instanceName = SelectedInstance.Name;

                            // Refresh the product in the list
                            RefreshProductList(SelectedProduct.ProductId, false);
                            RefreshInstanceList(instanceName);

                            MessageBox.Show(string.Format("Error updating the deploy manifest.\n", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Save Instances
                    SerializeHandler.SaveProductStandardData(SelectedProduct.GetProductData(), SelectedProduct.InstallPath);

                    // Update the instance in the listview
                    UpdateInstanceInListView(changeInstance.EditInstance);

                    // Update the product in the listview
                    UpdateProductInView(SelectedProduct);

                    // Enable disable instance buttons
                    EnableDisableInstanceButtons();

                    // Update main webpage
                    UpdateWebpage();
                }
                finally
                {
                    // Set to Default
                    this.Cursor = Cursors.Default;
                }
            }
        }
        
        private void btnRemoveInstance_Click(object sender, EventArgs e)
        {
            if (SelectedInstance != null)
            {
                // Question to ask user
                string message = "Are you sure you want to remove the instance \"" + SelectedInstance.Name + "\"?";

                // Check if last instance for the product and product is published.
                // In that case ask user if he wants to Unpublish the product.
                if (SelectedProduct.Instances.Count == 1 && SelectedProduct.IsPublished)
                {
                    message += "\n\nThe product will be unpublished since this is the only instance.";
                }

                // Ask user if he really wants to remove the instance
                if (MessageBox.Show(message, "Remove Instance", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    // First remove the deploy manifest.
                    if (SelectedInstance.DeleteManifestFile(SelectedProduct.InstallPath))
                    {
                        // Remove the instance from the instance list
                        SelectedProduct.Instances.Remove(SelectedInstance);

                        // Save Instances
                        SerializeHandler.SaveProductStandardData(SelectedProduct.GetProductData(), SelectedProduct.InstallPath);

                        // Check if product has any instances left and is published, if not unpublish.
                        if (SelectedProduct.Instances.Count == 0 && SelectedProduct.IsPublished)
                        {
                            // Unpublish product
                            btnUnpublish_Click(sender, e);
                        }

                        // Update the instance in the listview
                        RefreshInstanceList();

                        // Enable disable instance buttons
                        EnableDisableInstanceButtons();

                        // Update main webpage
                        UpdateWebpage();
                    }
                    else
                    {
                        MessageBox.Show("Unable to delete the deployment manifest. The file may be in use.", "Remove Instance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (SelectedProduct != null)
            {
                // First check if any instances are created. If not give user a message.
                if (SelectedProduct.Instances.Count == 0)
                {
                    if (MessageBox.Show("There are no instances created for this product.\nYou need to create atleast one instance before the application can be published.\n\nDo you want to create a new instance now?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        btnNewInstance_Click(this, null);
                    }
                    else
                    {
                        return;
                    }
                }

                // Check again if an instance has been created
                if (SelectedProduct.Instances.Count > 0)
                {
                    ProductPublishForm productPublish = new ProductPublishForm();

                    // Check if we should propose a name for the virtual directory name.
                    if (string.IsNullOrEmpty(SelectedProduct.VirtualDirectoryName))
                    {
                        // Remove the spaces in product name and use that
                        productPublish.VirtualDirectoryName = Regex.Replace(SelectedProduct.ProductName, @"[^a-zA-Z]", string.Empty);
                    }
                    else
                    {
                        productPublish.VirtualDirectoryName = SelectedProduct.VirtualDirectoryName;
                    }

                    productPublish.VirtualDirectoryPath = SelectedProduct.InstallPath;

                    if (productPublish.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            Cursor = Cursors.WaitCursor;

                            // Loop through the Instances and add/remove the Internet Guest Account to the
                            // deploymanifest files depending on if the instance is shown or not.
                            SelectedProduct.Instances.UpdateRightsOnManifestFiles(SelectedProduct.InstallPath, InternetGuestAccount);

                            SelectedProduct.VirtualDirectoryName = productPublish.VirtualDirectoryName;
                            SelectedProduct.VirtualDirectoryPath = SelectedProduct.InstallPath;
                            SelectedProduct.IsPublished = true;

                            // Save Instances
                            SerializeHandler.SaveProductStandardData(SelectedProduct.GetProductData(), SelectedProduct.InstallPath);

                            // Update the instance in the listview
                            UpdateProductInView(SelectedProduct);

                            // Enable disable buttons
                            EnableDisableProductButtons();

                            // Update main webpage
                            UpdateWebpage();
                        }
                        finally
                        {
                            Cursor = Cursors.Default;
                        }
                    }
                }
            }
        }

        private void btnUnpublish_Click(object sender, EventArgs e)
        {
            if (SelectedProduct != null)
            {
                SelectedProduct.IsPublished = false;

                // Delete the virtual directory
                try
                {
                    VirtualDirectory virDirHandler = new VirtualDirectory();
                    virDirHandler.VDir = virDirHandler.Get(SelectedProduct.VirtualDirectoryName);
                    virDirHandler.DeleteVirtualDirectory();
                }
                catch
                {
                    MessageBox.Show("An error occured when trying to delete the Virtual Directory with the name \"" + SelectedProduct.VirtualDirectoryName + "\". You need to delete this manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Save Instances
                SerializeHandler.SaveProductStandardData(SelectedProduct.GetProductData(), SelectedProduct.InstallPath);

                // Update the instance in the listview
                UpdateProductInView(SelectedProduct);

                // Update main webpage
                UpdateWebpage();
            }
        }

        private void UpdateSelectedProduct()
        {
            // Refresh Instance List
            RefreshInstanceList(SelectedInstance != null ? SelectedInstance.Name : string.Empty);

            // Enable disable buttons
            EnableDisableProductButtons();
            EnableDisableInstanceButtons();
        }

        private void UpdateSelectedInstance()
        {
            EnableDisableInstanceButtons();
        }
                
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Set navigate event
            webBrowser.Navigating += webBrowser_Navigating_CancelAll;
        }

        private void miConfigurationEdit_Click(object sender, EventArgs e)
        {
            ConfigurationForm configuration = new ConfigurationForm();

            configuration.Config = Config;
            configuration.ProductList = ProductList;

            if (configuration.ShowDialog() == DialogResult.OK)
            {
                // Reread configuration
                ReadConfiguration();

                // Check main virtual directory
                CheckMainVirtualDirectory();

                // Update mainpage
                UpdateWebpage();

                // Navigate to main webpage
                NavigateToMainWebpage();
            }

        }

        private void tsCopyURLToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tsAddress.Text);
        }

        private void btnUpdateWebpage_Click(object sender, EventArgs e)
        {
            UpdateWebpage();
        }
                
        private void btnHideUnhideInstance_Click(object sender, EventArgs e)
        {
            if (SelectedInstance != null)
            {
                // Change hidden status
                SelectedInstance.Visible = !SelectedInstance.Visible;

                try
                {
                    // Update the manifest file security
                    SelectedInstance.UpdateManifestFileSecurity(SelectedProduct.InstallPath, InternetGuestAccount);
                }
                catch
                {
                    MessageBox.Show(string.Format("Couldn't update the manifest file, for the instance, to add or remove the\n" +
                                                  "Internet Guest Account ({0}) to the Access Control List (ACL).\n" +
                                                  "\nTry to republish the product to solve the problem.", InternetGuestAccount),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }


                // Save Instances
                SerializeHandler.SaveProductStandardData(SelectedProduct.GetProductData(), SelectedProduct.InstallPath);

                // Update the instance in the listview
                UpdateInstanceInListView(SelectedInstance);

                // Enable/disable the instance buttons
                EnableDisableInstanceButtons();

                // Update main webpage
                UpdateWebpage();
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(LastImportDirectory) &&
                Directory.Exists(LastImportDirectory))
            {
                importFileDialog.InitialDirectory = LastImportDirectory;
            }
            else
            {
                importFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            }

            if (importFileDialog.ShowDialog() == DialogResult.OK)
            {
                DoImport(importFileDialog.FileName, true);
            }
        }

        private void DoImport(string importFile, bool showImportWindow)
        {
            // Save the directory to config if it has changed
            if (Config.GetLastImportDirectory() != Path.GetDirectoryName(importFile))
            {
                Config.SetLastImportDirectory(Path.GetDirectoryName(importFile));
                Config.Save();
            }

            string tempDir;
            string version = string.Empty;
            Product product = null;
            bool oldVersion = false;
            
            // Create the name for the temporary directory
            do
            {
                tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            }
            while (Directory.Exists(tempDir));

            ImportForm importingForm = null;

            try
            {
                // Set to Hourglass
                this.Cursor = Cursors.WaitCursor;

                // Create the temporary directory
                Directory.CreateDirectory(tempDir);

                // Open the archive
                using (ZipFile zip = ZipFile.Read(importFile))
                {
                    // Do checks with the zipfile and reads the Product file
                    try
                    {
                        CheckZipfile(zip, tempDir, out version, out product);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("The selected zip file doesn't contain a valid product.\n{0}", ex.Message), ex);
                    }

                    // Check if the product and/or the version exist for the product
                    // Also check if product version is newer or older
                    if (product.ProductType == ProductType.Standard)
                    {
                        if (ProductList.Exists(product.ProductId) &&
                            ProductList.Get(product.ProductId).Versions.Exists(version))
                        {
                            if (MessageBox.Show(string.Format("The product version ({0}) already exists.\nDo you want to replace it?\n", version),
                                                "Import Product",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning,
                                                MessageBoxDefaultButton.Button1) == DialogResult.No)
                            {
                                return;
                            }
                        }

                        // Check if trying to import an older version than the latest
                        // version installed for the product.
                        if (ProductList.Exists(product.ProductId) &&
                            VersionHandler.VersionStringCompare(ProductList.Get(product.ProductId).LatestVersion, version) == 1)
                        {
                            if (MessageBox.Show(string.Format("You are trying to import an older version ({0}) of this product than the latest version ({1}).\n" +
                                                              "Do you want to continue with the import?",
                                                              version,
                                                              ProductList.Get(product.ProductId).LatestVersion),
                                                "Import Product",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning,
                                                MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                oldVersion = true;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    
                    if (showImportWindow)
                    {
                        importingForm = new ImportForm();
                        importingForm.Show(this);

                        Application.DoEvents();
                    }

                    // Set eventhandler to do something useful.
                    if (importingForm != null) 
                        zip.ExtractProgress += importingForm.ZipExtractProgress;

                    // If no errors was found during the check then continue with extracting
                    // all files from the archive.
                    zip.ExtractAll(tempDir, true);

                    // Remove event again
                    if (importingForm != null) 
                        zip.ExtractProgress -= importingForm.ZipExtractProgress;

                    // Remove readonly attribute on the extracted files since it only creates problems.
                    FileHandler.RemoveReadOnlyAttribute(tempDir, true);
                }

                if (product.ProductType == ProductType.Standard)
                {
                    string installDir;
                    string versionInstallDir;
                    bool productExist = ProductList.Exists(product.ProductId);

                    // Check if product exist
                    if (productExist)
                        installDir = ProductList.Get(product.ProductId).InstallPath;
                    else
                        installDir = FileHandler.GetNewInstallDirectory(product.ProductName, StagingAreaPath);

                    // Versions install directory
                    versionInstallDir = Path.Combine(installDir, Constants.SubPathToVersions);

                    // Copy the version directory from temp to install directory
                    if (!FileHandler.CopyDirectory(Path.Combine(tempDir, version), Path.Combine(versionInstallDir, version)))
                        throw new Exception("Unable to copy the new version to the staging area.");

                    // Copy the product file
                    // Only do this if the version is newer than the LatestVersion or new parameters
                    // for newer versions will be removed etc.
                    if (!oldVersion)
                    {
                        try
                        {
                            FileHandler.CopyFileForced(Path.Combine(tempDir, SerializeHandler.ProductFilename), installDir);
                        }
                        catch
                        {
                            throw new Exception(string.Format("Error copying the file {0} to the staging area.", SerializeHandler.ProductFilename));
                        }
                    }

                    // Now create or update the ProductList
                    ProductList.CreateOrUpdate(product, installDir);
                    
                    //Update manifest to force download even if same version
                    foreach (Instance instance in ProductList.Get(product.ProductId).Instances)
                    {
                        string certificatePassword = null;

                        if (VersionHandler.VersionStringCompare(instance.ProductVersion, version) == 0)
                        {
                            // Check if we should ask for password
                            if (AskForCertificatePassword && string.IsNullOrEmpty(certificatePassword))
                            {
                                if (!AskForPasswordForm.Show(this, out certificatePassword))
                                    return;
                            }

                            UpdateDeployManifest(instance, true, certificatePassword);
                        }
                    }

                    // If product exist then let the same product be selected in the list as it
                    // was before the update.
                    // If it's a new product then select it in the list.
                    if (productExist)
                        RefreshProductList(SelectedProduct.ProductId, true);
                    else
                        RefreshProductList(product.ProductId, true);

                    RefreshInstanceList();
                }
                
                // Enable disable product buttons
                EnableDisableProductButtons();

                // Enable disable instance buttons
                EnableDisableInstanceButtons();
            }
            catch (Exception ex)
            {
                if (importingForm != null)
                {
                    importingForm.Close();
                    importingForm = null;
                }

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                // Set to Default
                this.Cursor = Cursors.Default;

                // Clean the temp dir
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    MessageBox.Show(string.Format("Error when trying to remove the temporary directory \"{0}\". You can try remove it manually.", tempDir), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (importingForm != null)
                {
                    importingForm.Close();
                    importingForm = null;
                }
            }
        }

        private void CheckZipfile(ZipFile zip, string extractDir, out string version, out Product product)
        {
            ZipEntry productXMLentry = null;
            bool manifestFound = false;
            version = string.Empty;

            // First loop and try to find the version directory
            foreach (ZipEntry entry in zip.Entries)
            {
                // Check if product.xml file
                if (entry.FileName.ToUpper() == SerializeHandler.ProductFilename.ToUpper())
                {
                    productXMLentry = entry;
                    continue;
                }

                if (entry.IsDirectory)
                {
                    // Check that the "root" directory is a versionnumber
                    string[] directories = entry.FileName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    if (directories.Length > 0)
                    {
                        // The first part of the path should always be the version number
                        if (VersionHandler.IsVersion(directories[0]))
                        {
                            // Fetch the version number
                            if (directories.Length == 1)
                            {
                                if (string.IsNullOrEmpty(version))
                                    version = directories[0];
                                else
                                {
                                    // More than one version directory was found. Error!
                                    throw new Exception("The zip file contains more than one version directory.");
                                }
                            }
                        }
                        else
                        {
                            // It's not a version directory.
                            throw new Exception("The zip file contains directories other than the version directory.");
                        }
                    }
                    else
                    {
                        // Split generated no result
                        throw new Exception("The zip file contains invalid directory entries.");
                    }

                    continue;
                }
            }

            if (string.IsNullOrEmpty(version))
            {
                // The version wasn't found
                throw new Exception("The zip file does not contain a version directory.");
            }

            if (productXMLentry == null)
            {
                throw new Exception(string.Format("The zip file is missing the file {0}.", SerializeHandler.ProductFilename));
            }

            // Read the Product.xml
            productXMLentry.Extract(extractDir);

            // Read the product.xml file from the path
            product = SerializeHandler.GetProduct(extractDir);

            // Throw an exception if the product couldn't be read.
            if (product == null)
                throw new Exception(string.Format("The {0} file couldn't be read.", productXMLentry.FileName));

            // Do additional controls of standard products.
            if (product.ProductType == ProductType.Standard)
            {
                // Check the files in the zipfile
                foreach (ZipEntry entry in zip.Entries)
                {
                    // Ignore directories since we checked them above
                    if (entry.IsDirectory)
                        continue;

                    // Check if product.xml file
                    if (entry.FileName.ToUpper() == SerializeHandler.ProductFilename.ToUpper())
                        continue;

                    // Check if it's a deploy file
                    if (Path.GetExtension(entry.FileName).ToUpper() == Constants.ValidZIPVersionDirExtension_Deploy.ToUpper())
                        continue;

                    // Check if it's a manifest file
                    if (Path.GetExtension(entry.FileName).ToUpper() == Constants.ValidZIPVersionDirExtension_Manifest.ToUpper())
                    {
                        if (!manifestFound)
                            manifestFound = true;
                        else
                        {
                            // More than one manifest file found in zip file! Error!
                            throw new Exception("The zip file contains more than one manifest file.");
                        }

                        continue;
                    }

                    // We should not reach this line since then we have found an unknown file
                    // that shouldn't be in the archive.
                    throw new Exception("The zip file contains one or more unknown files.");
                }
            }
        }

        private void dgvInstances_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectedInstance();
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            // Update selected product
            UpdateSelectedProduct();
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (SelectedProduct != null &&
                !SelectedProduct.IsPublished)
            {
                // Ask user if he really want to remove the product with all versions.
                if (MessageBox.Show(string.Format("Do you really want to remove \"{0}\" including all versions and instances?", SelectedProduct.ProductName),
                                    "Remove Product", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Remove readonly attribute
                    FileHandler.RemoveReadOnlyAttribute(SelectedProduct.InstallPath, true);

                    // It's as simple as removing the directory with the product
                    try
                    {
                        Directory.Delete(SelectedProduct.InstallPath, true);
                    }
                    catch
                    {
                        MessageBox.Show(string.Format("Error deleting the the products's directory.\r\n" +
                                                      "Try deleting the directory manually:\r\n\t{0}", SelectedProduct.InstallPath),
                                        "Remove Product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Reload the productlist
                    if (LoadProductList())
                    {
                        RefreshProductList();
                        EnableDisableProductButtons();
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void helpAboutItem_Click(object sender, EventArgs e)
        {
            AboutForm a = new AboutForm();
            a.ShowDialog(this);
        }
    }
}
