using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;

using System.Configuration;

namespace Cdc.MetaManager.GUI
{
    public partial class Analyze : MdiChildForm
    {
        private IAnalyzeService analyzeService = null;
        private IApplicationService appService = null;

        private DateTime CheckStartTime { get; set; }
        private bool DoBreakCheck { get; set; }
        private Schema schema = null;

        public Analyze()
        {
            InitializeComponent();

            analyzeService = MetaManagerServices.GetAnalyzeService();
            appService = MetaManagerServices.GetApplicationService();
        }

        private void CheckBackend_Load(object sender, EventArgs e)
        {
            tbSpecFilePath.Text = ConfigurationManager.AppSettings[BackendApplication.Name + "PLSQLSpecDirectory"];

            schema = appService.GetSchemaByApplicationId(BackendApplication.Id);

            tbDatabaseConnection.Text = schema.ConnectionString;

            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnStart.Enabled = false;

            if (cbCheckStoredProcedures.Checked)
            {
                if (SelectPLSQLSpecDir.IsValidSpecDirectory(tbSpecFilePath.Text))
                {
                    btnStart.Enabled = true;
                }
            }
            else if (cbCheckSQLQueries.Checked || 
                     cbCheckAllMaps.Checked ||
                     cbCheckAllDialogs.Checked)
                btnStart.Enabled = true;
        }

        private void cbCheckStoredProcedures_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbCheckStoredProcedures.Checked)
            {
                // Check directory
                if (!SelectPLSQLSpecDir.IsValidSpecDirectory(tbSpecFilePath.Text))
                {
                    SelectPLSQLSpecDir.ShowNotValidDirectory();
                    EnableDisableButtons();
                    return;
                }
            }

            try
            {
                // Disable Start and Close buttons
                btnStart.Enabled = false;
                btnClose.Enabled = false;
                gbChecks.Enabled = false;

                // Start the timer for Check
                CheckStartTime = DateTime.Now;

                AnalyzeIssueTree issueTree = analyzeService.Check(BackendApplication.Id,
                                                                  FrontendApplication.Id,
                                                                  cbCheckStoredProcedures.Checked,
                                                                  tbSpecFilePath.Text,
                                                                  cbCheckSQLQueries.Checked,
                                                                  tbDatabaseConnection.Text,
                                                                  cbCheckAllMaps.Checked,
                                                                  cbCheckAllDialogs.Checked,
                                                                  CheckCallback);

                ShowIssueList showIssueForm = new ShowIssueList(issueTree);

                showIssueForm.Show();
            }
            finally
            {
                btnStart.Enabled = true;
                btnClose.Enabled = true;
                gbChecks.Enabled = true;
                gbProgress.Visible = false;
            }
        }

        void CheckCallback(string passText,
                           int maxSteps,
                           int currentStep,
                           string currentStepText)
        {
            lblPass.Text = passText;
            lblProgressText.Text = currentStepText;

            if (maxSteps <= 0)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Maximum = maxSteps;
                progressBar.Value = currentStep;
            }

            lblTimeElapsed.Text = TimeText((DateTime.Now - CheckStartTime).TotalSeconds);

            if (!gbProgress.Visible)
                gbProgress.Visible = true;

            System.Windows.Forms.Application.DoEvents();

            if (DoBreakCheck)
                throw new Exception("USER STOPPED GENERATION!");
        }

        private static string TimeText(double seconds)
        {
            TimeSpan x = TimeSpan.FromSeconds(seconds);

            return string.Format("{0}:{1}:{2}", x.Hours.ToString("00"), x.Minutes.ToString("00"), x.Seconds.ToString("00"));
        }

        private void CheckBackend_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing &&
                btnClose.Enabled == false)
                e.Cancel = true;
        }

        private void cbCheckSQLQueries_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void cbCheckAllMaps_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void cbCheckAllDialogs_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void btnChangeDir_Click(object sender, EventArgs e)
        {
            using (SelectPLSQLSpecDir form = new SelectPLSQLSpecDir())
            {
                form.BackendApplication = BackendApplication;
                form.FrontendApplication = FrontendApplication;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    tbSpecFilePath.Text = ConfigurationManager.AppSettings[BackendApplication.Name + "PLSQLSpecDirectory"];
                    EnableDisableButtons();
                }
            }
        }

    }
}
