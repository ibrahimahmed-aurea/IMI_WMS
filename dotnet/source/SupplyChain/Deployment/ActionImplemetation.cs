using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.Deployment.Entities;
using System.IO;
using Ionic.Zip;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Imi.SupplyChain.Deployment
{
    public class ActionImplemetation
    {
        public delegate void StatusChangedDelegate(string message, int value, int min, int max);
        public event StatusChangedDelegate StatusChanged;

        private string _stagingAreaPath;
        private ProductStandardList _productList = new ProductStandardList();
        private string _certificateFile;
        private string _executablePath;

        ConfigurationSettings _config;

        public ActionImplemetation()
        {
            _executablePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            _config = new ConfigurationSettings(Path.Combine(_executablePath, Constants.ConfigurationfileName));

            _stagingAreaPath = _config.GetStagingArea();
            _certificateFile = _config.GetCertificateFile();



            LoadProductList();
        }

        private void LoadProductList()
        {
            // Find all products in the staging area that can have instances.
            string[] productDirs = Directory.GetDirectories(_stagingAreaPath);

            // Clear Productlist
            _productList.Clear();

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
                            _productList.CreateOrUpdate(product, productDir);
                        }
                    }

                }
            }
        }

        public void Import(string kitFilePath, string instanceToUpdate)
        {
            string tempDir;
            string version = string.Empty;
            Product product = null;
            bool oldVersion = false;

            // Create the name for the temporary directory
            RaiseStatusChange("Creating temporary directory", 0,0,-1);

            do
            {
                tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            }
            while (Directory.Exists(tempDir));


            try
            {
                // Create the temporary directory
                Directory.CreateDirectory(tempDir);

                RaiseStatusChange("Extracting kit", 0, 0,-1);
                // Open the archive
                using (ZipFile zip = ZipFile.Read(kitFilePath))
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


                    if (product.ProductType == ProductType.Standard)
                    {
                        // Check if trying to import an older version than the latest
                        // version installed for the product.
                        if (_productList.Exists(product.ProductId) && VersionHandler.VersionStringCompare(_productList.Get(product.ProductId).LatestVersion, version) == 1)
                        {
                            oldVersion = true;
                        }
                    }



                    // Set eventhandler to do something useful.
                    zip.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zip_ExtractProgress);

                    // If no errors was found during the check then continue with extracting
                    // all files from the archive.
                    zip.ExtractAll(tempDir, true);

                    // Remove event again
                    zip.ExtractProgress -= zip_ExtractProgress;

                    // Remove readonly attribute on the extracted files since it only creates problems.
                    FileHandler.RemoveReadOnlyAttribute(tempDir, true);
                }


                if (product.ProductType == ProductType.Standard)
                {
                    RaiseStatusChange("Creating/Updating product and version", 0, 0, -1);
                    string installDir;
                    string versionInstallDir;
                    bool productExist = _productList.Exists(product.ProductId);

                    // Check if product exist
                    if (productExist)
                        installDir = _productList.Get(product.ProductId).InstallPath;
                    else
                        installDir = FileHandler.GetNewInstallDirectory(product.ProductName, _stagingAreaPath);

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
                    _productList.CreateOrUpdate(product, installDir);

                    //Update manifest to force download even if same version
                    foreach (Instance instance in _productList.Get(product.ProductId).Instances)
                    {
                        if (string.IsNullOrEmpty(instanceToUpdate))
                        {
                            if (VersionHandler.VersionStringCompare(instance.ProductVersion, version) == 0)
                            {
                                RaiseStatusChange("Updating instance using same product version: " + instance.Name, 0,0,-1);
                                UpdateDeployManifest(_productList.Get(product.ProductId), instance, true, null);
                            }
                        }
                        else
                        {
                            if (instance.Name == instanceToUpdate)
                            {
                                bool changed = false;
                                RaiseStatusChange("Updating instance: " + instance.Name, 0, 0, -1);
                                if (instance.ProductVersion != version)
                                {
                                    instance.ProductVersion = version;
                                    instance.VersionPath = Path.Combine(versionInstallDir, version);
                                    instance.ApplicationManifestFile = Directory.GetFiles(instance.VersionPath, "*.manifest")[0];
                                    changed = true;
                                }

                                UpdateDeployManifest(_productList.Get(product.ProductId), instance, true, null);

                                if (changed)
                                {
                                    SerializeHandler.SaveProductStandardData(_productList.Get(product.ProductId).GetProductData(), _productList.Get(product.ProductId).InstallPath);
                                }
                            }
                        }

                    }

                    WebpageHandler.UpdateWebpage(_config, _productList);
                }
            }
            finally
            {
                // Clean the temp dir
                Directory.Delete(tempDir, true);
            }
        }

        void zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            RaiseStatusChange(e.CurrentEntry == null ? "" : e.CurrentEntry.FileName, e.EntriesExtracted, 0, e.EntriesTotal);
        }

        private void RaiseStatusChange(string message, int value, int min, int max)
        {
            if (StatusChanged != null)
            {
                StatusChanged(message, value, min, max);
            }
        }

        private void UpdateDeployManifest(ProductStandard product, Instance instance, bool onlyUpdate, string certificatePassword)
        {
            // Deploy Manifestfile
            string deployManifestFile = Path.Combine(product.InstallPath, instance.DeployManifestFile);

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

            string providerURL = "http://" + _config.GetWebserverName() + "/" + product.VirtualDirectoryName + "/" + instance.Name + ".application";

            // Build the arguments for updating the deploy template with mage.exe
            string argument = string.Format("-Update \"{0}\" " +
                                            "-AppManifest \"{1}\" " +
                                            "-Name \"{2}\" " +
                                            "-Version {3} " +
                                            "-p x86 " +
                                            "-CertFile \"{4}\"{5} " +
                                            "-ProviderUrl \"{6}\" ",
                                            deployManifestFile,
                                            instance.ApplicationManifestFile,
                                            product.ProductName,
                                            instance.ProductVersion,
                                            _certificateFile,
                                            !string.IsNullOrEmpty(certificatePassword) ? string.Format(" -Password {0}", certificatePassword) : string.Empty,
                                            providerURL);


            Regex regEx = new Regex(Constants.DeployManifestSuccessfulPattern);

            string result = RunConsoleApp.Run(Path.Combine(_executablePath, Constants.MageExeFilename), argument);

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
    }
}
