using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser;
using System.IO;
using System.CodeDom.Compiler;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.GUI
{
    public partial class frmMain : Form
    {
        public ConfigurationSettings Config { get; set; }

        public frmMain()
        {
            InitializeComponent();
            Config = new ConfigurationSettings();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnParseSingle_Click(object sender, EventArgs e)
        {
            using (SingleFileParse form = new SingleFileParse())
            {
                form.Config = Config;

                form.ShowDialog();
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            Config.SourceDirectory = tbSource.Text;
            Config.DestinationDirectory = tbDestination.Text;
            Config.Save();

            FileTemplateParser.ParseTemplates(tbSource.Text, tbDestination.Text, true, tbNamespace.Text, tbInherit.Text);

            MessageBox.Show("Parsing done!");
        }

        private void tbSource_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableButtons();
        }

        private void CheckEnableDisableButtons()
        {
            btnParse.Enabled = (Directory.Exists(tbSource.Text) &&
                                Directory.Exists(tbDestination.Text) &&
                                !string.IsNullOrEmpty(tbNamespace.Text) &&
                                !string.IsNullOrEmpty(tbInherit.Text));
        }

        private void tbDestination_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableButtons();
        }

        private void tbNamespace_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableButtons();
        }

        private void tbInherit_TextChanged(object sender, EventArgs e)
        {
            CheckEnableDisableButtons();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            tbSource.Text = Config.SourceDirectory;
            tbDestination.Text = Config.DestinationDirectory;
            CheckEnableDisableButtons();
        }

        private void btnSourceDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = tbSource.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbSource.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnDestinationDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = tbDestination.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbDestination.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (CompileRuntime compile = new CompileRuntime())
            {
                //            compile.ReferencesDirectory = @"C:\Project\anja_trunk_ss\dotnet\references";
                compile.ReferencesDirectory = @"C:\Project\anja_trunk_ss\dotnet\source\CodeGenerators\MetaManager\MetaManagerGUI\bin\Debug";
                compile.SourceDirectory = tbDestination.Text;
                compile.Config = Config;

                compile.ShowDialog();
            }
        }
    }
}
