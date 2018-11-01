using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cdc.MetaManager.GUI
{
    public partial class ShowGenerationResult : Form
    {
        private IList<string> FileList;

        public ShowGenerationResult()
        {
            InitializeComponent();
        }

        public static void Show(Form owner, string title, string information, string fileListTitle, IList<string> files)
        {
            using (ShowGenerationResult form = new ShowGenerationResult())
            {
                form.Owner = owner;
                form.Text = title;
                form.FileList = files;
                form.tbGenerationInformation.Text = information;
                form.gbGenerationInformation.Text = fileListTitle;
                form.ShowDialog();
            }
        }

        private void ShowGenerationResult_Load(object sender, EventArgs e)
        {
            btnClose.Focus();

            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnShowFiles.Enabled = false;

            if (FileList.Count > 0)
                btnShowFiles.Enabled = true;
        }

        private void PopulateFileList()
        {
            lvFiles.Items.Clear();

            lvFiles.BeginUpdate();

            foreach(string file in FileList)
            {
                lvFiles.Items.Add(file);
            }

            lvFiles.EndUpdate();
        }

        private void btnShowFiles_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                btnShowFiles.Enabled = false;
                PopulateFileList();
                AutoSize = false;
                AutoSizeMode = AutoSizeMode.GrowOnly;
                Size = new Size(682, 482);
                gbFiles.Visible = true;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

    }
}
