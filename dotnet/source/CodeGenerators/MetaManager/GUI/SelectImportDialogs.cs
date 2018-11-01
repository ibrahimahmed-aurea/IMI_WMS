using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;


namespace Cdc.MetaManager.GUI
{
    public partial class SelectImportDialogs : Form
    {
        public List<Dialog> Dialogs;
        public List<Dialog> SelectedDialogs = new List<Dialog>();
        public bool MaySelectDialogs { get; set; }

        public SelectImportDialogs()
        {
            InitializeComponent();
        }

        private List<LocalDialog> localDialogs = new List<LocalDialog>();

        private void SelectImportDialogs_Load(object sender, EventArgs e)
        {
            if (!MaySelectDialogs)
            {
                // Hide Import column if you may not select dialogs.
                dgvDialogs.Columns["selectedDataGridViewCheckBoxColumn"].Visible = false;

                // Hide toggle button
                toggleBtn.Visible = false;

                // Set new text on form
                this.Text = "Dialogs to Import";
            }

            foreach (Dialog d in Dialogs)
            {
                localDialogs.Add(new LocalDialog() { Dialog = d, Selected = true, Name = d.Name, Title = d.Title });
            }
            dialogSource.DataSource = localDialogs;
            dialogSource.RaiseListChangedEvents = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (LocalDialog d in localDialogs)
            {
                d.Selected = !d.Selected;
            }

            dialogSource.DataSource = null;
            dialogSource.DataSource = localDialogs;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDialogs.Rows)
            {
                // Check Name
                if (!NamingGuidance.CheckDialogName((string)row.Cells[1].Value, true))
                {
                    // Start editing the faulty name cell
                    dgvDialogs.CurrentCell = row.Cells[1];
                    dgvDialogs.BeginEdit(false);
                    return;
                }

                // Check title
                if (!NamingGuidance.CheckCaption((string)row.Cells[2].Value, "Dialog Title", true))
                {
                    // Start editing the faulty name cell
                    dgvDialogs.CurrentCell = row.Cells[2];
                    dgvDialogs.BeginEdit(false);
                    return;
                }
            }

            SelectedDialogs.Clear();

            foreach (LocalDialog d in localDialogs)
            {
                if (d.Selected)
                {
                    // Now set the new Name and Title from the grid.
                    d.Dialog.Name = d.Name;
                    d.Dialog.Title = d.Title;

                    // Add the dialog to the selected ones.
                    SelectedDialogs.Add(d.Dialog);
                }
            }

            if (SelectedDialogs.Count > 0)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                cancelBtn_Click(sender, e);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            SelectedDialogs.Clear();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dgvDialogs_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Check if edited cell is Name or Title
            if (e.ColumnIndex == 1) // Name
            {
                if (!NamingGuidance.CheckDialogName(e.FormattedValue.ToString(), true))
                {
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == 2) // Title
            {
                if (!NamingGuidance.CheckCaption(e.FormattedValue.ToString(), "Dialog Title", true))
                {
                    e.Cancel = true;
                }
            }
        }

    }

    public class LocalDialog
    {
        public bool Selected { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public Dialog Dialog { get; set; }
    }

}
