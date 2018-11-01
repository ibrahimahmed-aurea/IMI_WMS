using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Spring.Context;
using Spring.Context.Support;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess.Dao;
using System.Diagnostics;
using System.IO;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplates;
using Cdc.CodeGeneration.Caching;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Reflection;
using Cdc.MetaManager.DataAccess;
using Domain = Cdc.MetaManager.DataAccess.Domain;

using System.Text.RegularExpressions;
using NHibernate;

namespace Cdc.MetaManager.GUI
{
    public partial class GenerateReports : MdiChildForm
    {
        private IConfigurationManagementService configurationManagementService;
        private IModelService modelService;
                
        private DateTime generateStartTime;

        private bool doBreak = false;
        
        public GenerateReports()
        {
            InitializeComponent();

            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void btnBrowseXMLSchemaFolder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbXMLSchemaFolder.Text.Trim()) || !Directory.Exists(tbXMLSchemaFolder.Text))
            {
                // If running in debug mode in Visual Studio then the relative path will work
                if (Directory.Exists(Path.GetFullPath(@"..\..\..\..\..\..\source\SupplyChain\Reporting\Reports\XSD")))
                    folderDlg.SelectedPath = Path.GetFullPath(@"..\..\..\..\..\..\source\SupplyChain\Reporting\Reports\XSD");
                else
                {
                    folderDlg.SelectedPath = string.Empty;
                }
            }
            else if (Directory.Exists(tbXMLSchemaFolder.Text))
                folderDlg.SelectedPath = tbXMLSchemaFolder.Text;
            else
                folderDlg.SelectedPath = string.Empty;

            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                tbXMLSchemaFolder.Text = folderDlg.SelectedPath;
            }
        }

        private void btnBrowsePLSQLPackageFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tbPLSQLPackageFolder.Text))
                folderDlg.SelectedPath = tbPLSQLPackageFolder.Text;
            else
                folderDlg.SelectedPath = string.Empty;

            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                // Check if user has selected a body or spec directory
                if (Path.GetFileName(folderDlg.SelectedPath).ToLower() == "body" ||
                    Path.GetFileName(folderDlg.SelectedPath).ToLower() == "spec")
                {
                    MessageBox.Show("You have selected a body or spec directory to generate the PL/SQL files to.\n" +
                                    "You should select the parent folder where the body and spec directory exists.",
                                    "Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }

                tbPLSQLPackageFolder.Text = folderDlg.SelectedPath;
            }
        }

        private void CodeSmithTemplateCallback(string text, int percentageDone)
        {
            lblProgressText.Text = text;
            progressBar.Value = percentageDone;
            UpdateTime();
            System.Windows.Forms.Application.DoEvents();

            if (doBreak)
                throw new Exception("USER STOPPED GENERATION!");
        }

        private void UpdateTime()
        {
            TimeSpan x = DateTime.Now - generateStartTime;
            lblTimeElapsed.Text = string.Format("{0}:{1}:{2}", x.Hours.ToString("00"), x.Minutes.ToString("00"), x.Seconds.ToString("00"));
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckPaths())
                    return;

                okBtn.Enabled = false;
                cancelBtn.Enabled = false;
                btnStopGeneration.Enabled = true;
                doBreak = false;

                Cursor.Current = Cursors.WaitCursor;

                // Save to config file
                Config.Backend.XMLSchemaFolder = tbXMLSchemaFolder.Text;
                Config.Backend.PLSQLPackageFolder = tbPLSQLPackageFolder.Text;
                Config.Save();

                try
                {
                    // Reset progressbar and make the panel visible
                    progressBar.Value = 0;
                    pProgress.Visible = true;
                    lblTimeElapsed.Text = string.Empty;

                    lblProgressText.Text = "Extracting Data from Database...";
                    System.Windows.Forms.Application.DoEvents();

                    using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                    {
                        IList<Report> reports = new List<Report>();

                        List<IDomainObject> selectedReports = modelService.GetAllDomainObjectsByApplicationId<Report>(BackendApplication.Id).OrderBy(r => (r.Name)).ToList<IDomainObject>();

                        foreach (Report report in selectedReports)
                        {
                            Report readReport = modelService.GetDomainObject<Report>(report.Id);
                            reports.Add(readReport);
                        }

                        ReportTemplate template = new ReportTemplate();

                        // Create the plsql body and spec directories if they don't exist
                        string plsqlBodyDirectory = Path.Combine(tbPLSQLPackageFolder.Text.Trim(), "body");
                        string plsqlSpecDirectory = Path.Combine(tbPLSQLPackageFolder.Text.Trim(), "spec");

                        if (!Directory.Exists(plsqlBodyDirectory))
                            Directory.CreateDirectory(plsqlBodyDirectory);

                        if (!Directory.Exists(plsqlSpecDirectory))
                            Directory.CreateDirectory(plsqlSpecDirectory);

                        template.reports = reports;
                        template.xsdDirectory = tbXMLSchemaFolder.Text.Trim();
                        template.plsqlBodyDirectory = plsqlBodyDirectory;
                        template.plsqlSpecDirectory = plsqlSpecDirectory;
                        template.printDocumentPackageName = "PrintDocument"; // Name of package for printing document
                        template.templateCallback = CodeSmithTemplateCallback;

                        generateStartTime = DateTime.Now;

                        template.Render(TextWriter.Null);

                        UpdateTime();
                    }

                    if (FileCacheManager.GetFilesNotSaved().Count > 0)
                    {
                        string notSavedFiles = FileCacheManager.GetFilesNotSaved().Aggregate((current, next) => current + Environment.NewLine + '\t' + next);

                        string txt = "There were one or more files that couldn't be saved.\r\n" +
                                     "Probable cause is that the file is located in a dynamic Clearcase view and is not checked out.\r\n" +
                                     "Check out the file(s) and generate again.";

                        ShowGenerationResult.Show(this, "Generation Result: Not saved files", txt, "Files not saved", FileCacheManager.GetFilesNotSaved());
                    }

                    string text = string.Format("Code generation complete.\r\n{0} files generated, {1} files written to disk.",
                                           FileCacheManager.GetWrites(),
                                           FileCacheManager.GetWritesToDisk());

                    ShowGenerationResult.Show(this, "Generation Result", text, "Files written to disk", FileCacheManager.GetWrittenFilesToDisk());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error in Codegeneration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                pProgress.Visible = false;
                EnableDisableButtons();
            }
        }

        private bool CheckPaths()
        {
            if (!Directory.Exists(tbXMLSchemaFolder.Text))
            {
                MessageBox.Show("The path to the XML Schema folder does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbXMLSchemaFolder.SelectAll();
                tbXMLSchemaFolder.Focus();
                return false;
            }
            else if (!Directory.Exists(tbPLSQLPackageFolder.Text))
            {
                MessageBox.Show("The path to the PL/SQL Package folder does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPLSQLPackageFolder.SelectAll();
                tbPLSQLPackageFolder.Focus();
                return false;
            }
            return true;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GenerateReports_Load(object sender, EventArgs e)
        {
            tbXMLSchemaFolder.Text = Config.Backend.XMLSchemaFolder;
            tbPLSQLPackageFolder.Text = Config.Backend.PLSQLPackageFolder;

            PopulateReports();

            pProgress.Visible = false;
            EnableDisableButtons();
        }

        private void PopulateReports()
        {
            lvReports.Items.Clear();

            foreach (Report report in modelService.GetAllDomainObjectsByApplicationId<Report>(BackendApplication.Id).OrderBy(r => (r.Name)))
            {
                ListViewItem item = new ListViewItem(report.Name);
                item.Tag = report;
                lvReports.Items.Add(item);
            }
        }

        private void EnableDisableButtons()
        {
            okBtn.Enabled = false;
            cancelBtn.Enabled = true;

            if (!string.IsNullOrEmpty(tbXMLSchemaFolder.Text) &&
                !string.IsNullOrEmpty(tbPLSQLPackageFolder.Text))
            {
                okBtn.Enabled = true;
            }
        }

        private void tbXMLSchemaFolder_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbPLSQLPackageFolder_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void btnStopGeneration_Click(object sender, EventArgs e)
        {
            btnStopGeneration.Enabled = false;
            doBreak = true;
        }


    }

    public class LocalReport
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public Report Report { get; set; }
        public string TooltipText { get; set; }
    }

}
