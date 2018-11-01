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
using Spring.Context;
using Spring.Context.Support;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess;
using NHibernate;
using Cdc.MetaManager.BusinessLogic;


namespace Cdc.MetaManager.GUI
{
    public partial class FindViewForm : MdiChildForm
    {
        private IDialogService dialogService = null;

        public Cdc.MetaManager.DataAccess.Domain.View View { get; set; }

        public FindViewForm()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();

            Cursor.Current = Cursors.Default;
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
            {
                FindViewTypes findViewType = FindViewTypes.All;

                if (rbCustomOnly.Checked)
                    findViewType = FindViewTypes.Custom;
                else if (rbDrilldowns.Checked)
                    findViewType = FindViewTypes.Drilldowns;

                // Find the views
                IList<Cdc.MetaManager.DataAccess.Domain.View> views = dialogService.GetViews(tbEntity.Text, tbName.Text, tbTitle.Text, findViewType, FrontendApplication.Id);

                lvResult.BeginUpdate();

                lvResult.Items.Clear();

                if (views != null && views.Count > 0)
                {
                    foreach (Cdc.MetaManager.DataAccess.Domain.View view in views)
                    {
                        ListViewItem item = lvResult.Items.Add(view.Id.ToString());
                        item.SubItems.Add(view.Name);
                        item.SubItems.Add(view.Title);
                        item.SubItems.Add(view.Type.ToString());
                        item.SubItems.Add(view.BusinessEntity.Name);
                        item.Tag = view;
                    }
                }

                lvResult.EndUpdate();
            }

            EnableDisableButtons();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (lvResult.SelectedItems.Count == 1)
            {
                View = lvResult.SelectedItems[0].Tag as Cdc.MetaManager.DataAccess.Domain.View;

                ReadViewBeforeExiting();

                DialogResult = DialogResult.OK;

                Close();
            }
        }

        private void ReadViewBeforeExiting()
        {
            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
            {
                View = dialogService.GetViewById(View.Id);

                NHibernateUtil.Initialize(View.Application);
                NHibernateUtil.Initialize(View.RequestMap);
                NHibernateUtil.Initialize(View.ResponseMap);
            }
        }

        private void entityListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvResult.SelectedItems.Count == 1)
            {
                pgDetails.SelectedObject = lvResult.SelectedItems[0].Tag;
            }
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnOk.Enabled = false;

            if (lvResult.SelectedItems.Count == 1)
            {
                btnOk.Enabled = true;
            }
        }

        private void FindViewForm_Load(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void tbEntity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void cbCustomOnly_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchBtn_Click(this, null);
            }
        }

        private void btnCreateCustomView_Click(object sender, EventArgs e)
        {
            using (CreateCustomView customView = new CreateCustomView())
            {
                customView.FrontendApplication = FrontendApplication;
                customView.BackendApplication = BackendApplication;
                customView.EditView = null;

                if (customView.ShowDialog() == DialogResult.OK)
                {
                    // Ask user if he also wants to connect the newly created node to the selected
                    // ViewNode as the parent node.
                    if (MessageBox.Show("Do you want to use the created Custom View as the selected View?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        View = customView.EditView;

                        ReadViewBeforeExiting();

                        DialogResult = DialogResult.OK;
                    }
                }
            }
        }

    }
}
