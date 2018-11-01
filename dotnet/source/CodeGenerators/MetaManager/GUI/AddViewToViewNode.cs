using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;


namespace Cdc.MetaManager.GUI
{
    public partial class AddViewToViewNode : MdiChildForm
    {
        public ViewNode ParentViewNode { get; set; }

        public Cdc.MetaManager.DataAccess.Domain.View SelectedView { get; set; }

        private IDialogService dialogService;

        public AddViewToViewNode()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
        }

        private void AddViewToViewNode_Load(object sender, EventArgs e)
        {
            if (SelectedView != null)
            {
                // Read the view
                SelectedView = dialogService.GetViewById(SelectedView.Id);
            }

            // Populate Viewfields
            PopulateViewFields();

            if (ParentViewNode != null)
            {
                // Read the viewnode
                ParentViewNode = dialogService.GetViewNodeById(ParentViewNode.Id);

                // Set connected information
                tbParentViewNode.Text = string.Format(@"{0}", ParentViewNode.Id.ToString());
                tbParentView.Text = string.Format(@"{0} ({1})", ParentViewNode.View.Title, ParentViewNode.View.Name);
            }

            CheckEnableDisableOK();
        }

        private void btnSelectView_Click(object sender, EventArgs e)
        {
            using (FindViewForm findView = new FindViewForm())
            {
                findView.FrontendApplication = FrontendApplication;
                findView.BackendApplication = BackendApplication;

                if (findView.ShowDialog() == DialogResult.OK)
                {
                    SelectedView = findView.View;
                    PopulateViewFields();
                    CheckEnableDisableOK();
                }
            }
        }

        private void PopulateViewFields()
        {
            if (SelectedView != null)
            {
                // Set the view information
                tbViewName.Text = string.Format(@"({0}) {1}", SelectedView.Id.ToString(), SelectedView.Name);
                tbViewTitle.Text = SelectedView.Title;
                cbIsCustomView.Checked = (SelectedView.Type == Cdc.MetaManager.DataAccess.Domain.ViewType.Custom);
            }
            else
            {
                tbViewName.Text = string.Empty;
                tbViewTitle.Text = string.Empty;
                cbIsCustomView.Checked = false;
            }
        }

        private void CheckEnableDisableOK()
        {
            btnOK.Enabled = false;

            if (SelectedView != null && ParentViewNode != null)
            {
                btnOK.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Create the ViewNode to the Parent ViewNode and then connect the selected
            // View to the created ViewNode.
            if (dialogService.ConnectViewToViewNode(SelectedView, ParentViewNode) == null)
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnCreateView_Click(object sender, EventArgs e)
        {
            using (CreateView createView = new CreateView())
            {
                createView.FrontendApplication = FrontendApplication;
                createView.BackendApplication = BackendApplication;
                createView.Owner = this;

                if (createView.ShowDialog() == DialogResult.OK)
                {
                    SelectedView = createView.NewView;
                    PopulateViewFields();
                    CheckEnableDisableOK();
                }

            }
        }
    }
}
