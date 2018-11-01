using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;

namespace Cdc.MetaManager.GUI
{
    public partial class CreateIssueForm : MdiChildForm
    {
        private IDialogService dialogService = null;
        private IAnalyzeService analyzeService = null;

        public Issue Issue { get; set; }

        public CreateIssueForm()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            analyzeService = MetaManagerServices.GetAnalyzeService();
        }

        private void CreateIssueForm_Load(object sender, EventArgs e)
        {
        }

        private void findBtn_Click(object sender, EventArgs e)
        {
            using (SelectDialog form = new SelectDialog())
            {
                form.FrontendApplication = FrontendApplication;
                
                if (form.ShowDialog() == DialogResult.OK)
                {
                    dialogTbx.Text = form.SelectedDialog.Name;
                    dialogTbx.Tag = form.SelectedDialog;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dialogTbx.Tag == null)
            {
                MessageBox.Show("You must select a dialog.");
                return;
            }

            Issue issue = new Issue();

            issue.Severity = IssueSeverityType.Error;
            issue.Title = string.Format("{0} / {1} : {2}", (dialogTbx.Tag as Dialog).Module.Name, dialogTbx.Text, nameTbx.Text);
            issue.Text = textTbx.Text;
            issue.ObjectId = (dialogTbx.Tag as Dialog).Id;
            issue.ObjectType = IssueObjectType.Bug;
            issue.Application = FrontendApplication;

            analyzeService.SaveOrUpdateIssue(issue);

            Issue = issue;

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
