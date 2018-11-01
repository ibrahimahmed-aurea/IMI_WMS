using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Configuration;

namespace Cdc.MetaManager.GUI
{
    public partial class SelectPLSQLSpecDir : MdiChildForm
    {

        public string SelectedDirectory 
        {
            get
            {
                return tbSpecFilePath.Text;
            }
        }

        public SelectPLSQLSpecDir()
        {
            InitializeComponent();
        }

        private void SelectPLSQLSpecDir_Load(object sender, EventArgs e)
        {
            tbSpecFilePath.Text = ConfigurationManager.AppSettings[BackendApplication.Name + "PLSQLSpecDirectory"];

            EnableDisableButtons();
        }

        private void btnBrowseSpecFiles_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbSpecFilePath.Text) &&
                Directory.Exists(tbSpecFilePath.Text))
            {
                folderBrowseSpecFiles.SelectedPath = tbSpecFilePath.Text;
            }
            else
            {
                folderBrowseSpecFiles.SelectedPath = string.Empty;
            }

            bool done = false;

            do
            {
                if (folderBrowseSpecFiles.ShowDialog() == DialogResult.OK)
                {
                    // Check for .spec files
                    if (!IsValidSpecDirectory(folderBrowseSpecFiles.SelectedPath))
                    {
                        if (ShowNotValidDirectory(true) == DialogResult.Cancel)
                        {
                            done = true;
                        }
                    }
                    else
                    {
                        tbSpecFilePath.Text = folderBrowseSpecFiles.SelectedPath;
                        done = true;
                    }
                }
                else
                    done = true;
            }
            while (!done);

            EnableDisableButtons();
        }

        public static bool IsValidSpecDirectory(string path)
        {
            return !string.IsNullOrEmpty(path) &&
                   Directory.Exists(path) &&
                   Directory.GetFiles(path, "*.spec").Count() > 0;
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (IsValidSpecDirectory(tbSpecFilePath.Text))
            {
                btnOK.Enabled = true;
            }

        }

        public static void ShowNotValidDirectory()
        {
            ShowNotValidDirectory(false);
        }

        public static DialogResult ShowNotValidDirectory(bool cancelOption)
        {
            return MessageBox.Show("The path to the spec-files doesn't contain any spec-files (.spec)!\n" +
                                   "Select another directory and try again.",
                                   "Not a valid spec-file directory",
                                   cancelOption ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK,
                                   MessageBoxIcon.Exclamation);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check directory
            if (!IsValidSpecDirectory(tbSpecFilePath.Text))
            {
                ShowNotValidDirectory();
                EnableDisableButtons();
                return;
            }

            // Save directory

            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            config.AppSettings.Settings[BackendApplication.Name + "PLSQLSpecDirectory"].Value = tbSpecFilePath.Text;
            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");


            DialogResult = DialogResult.OK;
        }

        private void tbSpecFilePath_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

    }
}
