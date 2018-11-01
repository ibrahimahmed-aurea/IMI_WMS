using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.TemplateObjects;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.GUI
{
    public partial class SingleFileParse : Form
    {
        public ConfigurationSettings Config { get; set; }

        public SingleFileParse()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = tbFile.Text;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = openFileDialog.FileName;
            }
        }

        private void tbFile_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnReadContent.Enabled = false;

            if (File.Exists(tbFile.Text))
            {
                btnReadContent.Enabled = true;
            }
        }

        private void SingleFileParse_Load(object sender, EventArgs e)
        {
            tbFile.Text = Config.SingleFileParse;
            EnableDisableButtons();
        }

        private void btnReadContent_Click(object sender, EventArgs e)
        {
            tbClassname.Text = Path.GetFileNameWithoutExtension(tbFile.Text);

            string[] rows = File.ReadAllLines(tbFile.Text);

            tbContent.Text = string.Empty;

            foreach (string row in rows)
            {
                tbContent.Text += row + Environment.NewLine;
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            Config.SingleFileParse = tbFile.Text;
            Config.Save();

            TemplateParser parser = new TemplateParser();

            List<TemplateObject> list = parser.Parse(tbContent.Text);

            tbResult.Text = ParsedClassResult.CreateContents(tbNamespace.Text, tbClassname.Text, tbInherit.Text, list, tbFile.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
