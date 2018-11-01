using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Oracle.DataAccess.Client;
using System.Diagnostics;


namespace Cdc.MetaManager.GUI
{
    public partial class FindHintForm : MdiChildForm
    {
        private IDialogService dialogService = null;
        private IModelService modelService = null;

        private bool isModified;

        public bool IsSelect { get; set; }

        public HintCollection hintCollection { get; set; }

        public Hint SelectedHint
        {
            get
            {
                if (hintListView.SelectedItems.Count > 0)
                    return hintListView.SelectedItems[0].Tag as Hint;
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    Clear();
                    idTbx.Text = value.Id.ToString();
                    searchBtn_Click(this, null);
                }
            }
        }


        public FindHintForm()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void FindPropertyForm_Load(object sender, EventArgs e)
        {
            if (IsSelect)
            {
                okBtn.Visible = true;
                cancelBtn.Text = "Cancel";
            }
            else
            {
                okBtn.Visible = false;
                cancelBtn.Text = "Close";
            }

            if (this.hintCollection == null)
            {
                hintCollection = BackendApplication.HintCollection;
            }

            hintCollection = modelService.GetInitializedDomainObject<Cdc.MetaManager.DataAccess.Domain.HintCollection>(hintCollection.Id);

            // The user must check out both hint collection to be able to change hints in the dialog
            if (hintCollection.IsLocked && hintCollection.LockedBy == Environment.UserName)
            {
                this.IsEditable = true;
            }

            EnableDisableButtons();
            EnableDisableEditFields();


            Cursor.Current = Cursors.Default;
        }

        private void EnableDisableEditFields()
        {
            fullTextTbx.ReadOnly = !this.IsEditable;
        }
       
        private void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bool inHintTo = true;
                IList<Property> properties = null;
                IList<Hint> hints = null;
                IList<object> result = new List<object>();
                
                hintListView.BeginUpdate();
                hintListView.Items.Clear();

                Dictionary<string, object> propKeys = new Dictionary<string, object>();
                Dictionary<string, object> hintKeys = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(idTbx.Text.Trim()))
                {
                    string guidId = idTbx.Text.Replace("_", "-").Trim();

                    Guid guid;
                    
                    if (Guid.TryParse(guidId, out guid))
                    {
                        propKeys.Add("Hint.Id", guid);
                        hintKeys.Add("Id", guid);
                    }
                }

                if (!string.IsNullOrEmpty(tableTbx.Text.Trim()))
                {
                    propKeys.Add("StorageInfo.TableName", tableTbx.Text.Trim());
                    inHintTo = false;
                }

                if (!string.IsNullOrEmpty(columnTbx.Text.Trim()))
                {
                    propKeys.Add("StorageInfo.ColumnName", columnTbx.Text.Trim());
                    inHintTo = false;
                }

                if (!string.IsNullOrEmpty(textTbx.Text.Trim()))
                {
                    propKeys.Add("Hint.Text", textTbx.Text.Trim());
                    hintKeys.Add("Text", textTbx.Text.Trim());
                }

                propKeys.Add("Hint.HintCollection", hintCollection.Id);
                hintKeys.Add("HintCollection", hintCollection.Id);

                properties = modelService.GetAllDomainObjectsByPropertyValues<Property>(propKeys, true, true);

                foreach (Property p in properties)
                {
                    result.Add(p);
                }

                if (inHintTo)
                {
                    hints = modelService.GetAllDomainObjectsByPropertyValues<Hint>(hintKeys, true, true);

                    foreach (Hint h in hints)
                    {
                        result.Add(h);
                    }
                }

                int c = 0;

                foreach (object o in result)
                {
                    c++;

                    ListViewItem item = new ListViewItem();

                    if (o is Property)
                    {
                        Property p = o as Property;
                        item.Text = p.Hint.Id.ToString();
                        item.SubItems.Add(p.Hint.Text);
                        item.SubItems.Add(p.StorageInfo.TableName);
                        item.SubItems.Add(p.StorageInfo.ColumnName);
                        item.Tag = p.Hint;
                    }
                    else
                    {
                        Hint h = o as Hint;
                        item.Text = h.Id.ToString();
                        item.SubItems.Add(h.Text);
                        item.Tag = h;
                    }

                    hintListView.Items.Add(item);

                    if (c == 1000)
                        break;
                }
                
                hintListView.EndUpdate();

                if (hintListView.Items.Count > 0)
                    hintListView.Items[0].Selected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                hintListView.EndUpdate();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
      

        private void propertyListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
            isModified = false;
        }
        
        private void EnableDisableButtons()
        {
            deleteBtn.Enabled = false;
            fullTextTbx.Text = null;
            fullTextTbx.Enabled = false;
            createBtn.Enabled = this.IsEditable;

            if (hintListView.SelectedItems.Count > 0)
            {
                fullTextTbx.Text = ((Hint)hintListView.SelectedItems[0].Tag).Text;
                deleteBtn.Enabled = this.IsEditable;
                fullTextTbx.Enabled = this.IsEditable;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void nameTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void tableTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void columnTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void nameTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void tableTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void columnTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void beTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void propertyListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (okBtn.Visible && okBtn.Enabled)
                    okBtn_Click(this, null);
            }
        }

        private void gbSelect_Enter(object sender, EventArgs e)
        {

        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected Hint?", "Delete Hint", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                modelService.DeleteDomainObject((Hint)hintListView.SelectedItems[0].Tag);
                hintCollection.Hints.Remove((Hint)hintListView.SelectedItems[0].Tag);
                hintListView.Items.Remove(hintListView.SelectedItems[0]);
                fullTextTbx.Text = "";
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            using (CreateHintForm form = new CreateHintForm())
            {
                form.Owner = this;
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.hintCollection = hintCollection;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    Clear();
                    idTbx.Text = form.Hint.Id.ToString();

                    searchBtn_Click(this, new EventArgs());
                }
            }
        }

        private void Clear()
        {
            textTbx.Text = "";
            columnTbx.Text = "";
            tableTbx.Text = "";
            idTbx.Text = "";
            fullTextTbx.Text = "";

            hintListView.Items.Clear();

            EnableDisableButtons();
        }

        private void idTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                searchBtn_Click(this, new EventArgs());
        }

        private void textTbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void fullTextTbx_TextChanged(object sender, EventArgs e)
        {
            isModified = true;
        }

        private void fullTextTbx_Leave(object sender, EventArgs e)
        {
            if (isModified)
            {
                if (MessageBox.Show("The text has been modified, do you want to save your changes?", "Save changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Hint hint = (Hint)hintListView.SelectedItems[0].Tag;
                    hint.Text = fullTextTbx.Text;
                    modelService.SaveDomainObject(hint);
                    UpdateAllIdenticalListItemsText(hintListView.SelectedItems[0]);
                }
                isModified = false;
            }
        }


        void UpdateAllIdenticalListItemsText(ListViewItem lvi)
        {
            int iIdColumn = 0;
            int iTextColumn = 1;

            String id = lvi.SubItems[iIdColumn].ToString();

            foreach (ListViewItem lvi2 in hintListView.Items)
            {
                if (lvi2.SubItems[iIdColumn].ToString().Equals(id))
                {
                    if (lvi2.Tag != null && lvi2.Tag is Hint)
                    {
                        Hint h = lvi2.Tag as Hint;

                        lvi2.SubItems[iTextColumn].Text = h.Text;
                    }
                }
            }
        }

    }
}
