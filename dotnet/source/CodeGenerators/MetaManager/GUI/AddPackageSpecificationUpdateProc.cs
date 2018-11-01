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
    public partial class AddPackageSpecificationUpdateProc : Form
    {
        public IList<StoredProcedure> ListOfProceduresToSelectFrom { get; set; }

        public StoredProcedure SelectedProcedure 
        {
            get
            {
                if (lvProcedures.SelectedItems.Count > 0)
                {
                    return (StoredProcedure)lvProcedures.SelectedItems[0].Tag;
                }

                return null;
            }
        }

        public AddPackageSpecificationUpdateProc()
        {
            InitializeComponent();
        }

        private void AddPackageSpecificationUpdateProc_Load(object sender, EventArgs e)
        {
            PopulateProcedureList();
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (SelectedProcedure != null)
            {
                btnOK.Enabled = true;
            }
        }

        private void PopulateProcedureList()
        {
            if (ListOfProceduresToSelectFrom != null && ListOfProceduresToSelectFrom.Count > 0)
            {
                lvProcedures.Items.Clear();

                foreach (StoredProcedure proc in ListOfProceduresToSelectFrom.OrderBy(p => p.Id))
                {
                    ListViewItem item = new ListViewItem(proc.Id.ToString());
                    item.SubItems.Add(proc.ProcedureName);
                    item.Tag = proc;

                    lvProcedures.Items.Add(item);
                }

                // Select first row
                if (lvProcedures.Items.Count > 0)
                {
                    lvProcedures.Items[0].Selected = true;
                }
            }

            EnableDisableButtons();
        }

        private void PopulateParameterList()
        {
            if (SelectedProcedure != null)
            {
                lvParameters.Items.Clear();

                foreach (ProcedureProperty property in SelectedProcedure.Properties)
                {
                    ListViewItem item = new ListViewItem(property.Sequence.ToString());
                    item.SubItems.Add(property.Name);
                    item.SubItems.Add(property.PropertyType.ToString());

                    if (!string.IsNullOrEmpty(property.OriginalTable) &&
                        !string.IsNullOrEmpty(property.OriginalColumn))
                    {
                        item.SubItems.Add(string.Format("{0}.{1}", property.OriginalTable, property.OriginalColumn));
                    }
                    else
                    {
                        item.SubItems.Add(property.DbDatatype);
                    }

                    lvParameters.Items.Add(item);
                }
            }
        }

        private void lvProcedures_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (SelectedProcedure != null)
            {
                PopulateParameterList();
            }

            EnableDisableButtons();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SelectedProcedure == null)
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
