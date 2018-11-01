using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;

namespace Imi.SupplyChain.Deployment.UI
{
    public partial class ImportForm : Form
    {
        public ImportForm()
        {
            InitializeComponent();
        }

        public void ZipExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EntriesTotal != 0 && e.EntriesExtracted != 0)
            {
                if (progressBar.Maximum != e.EntriesTotal)
                    progressBar.Maximum = e.EntriesTotal;

                if (progressBar.Value != e.EntriesExtracted)
                    progressBar.Value = e.EntriesExtracted;
            }

            Application.DoEvents();
        }

    }
}
