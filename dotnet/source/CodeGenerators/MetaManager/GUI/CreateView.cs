using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;

using Cdc.MetaManager.DataAccess.Domain.VisualModel;


namespace Cdc.MetaManager.GUI
{
    public partial class CreateView : MdiChildForm
    {
        public Cdc.MetaManager.DataAccess.Domain.View NewView { get; set; }

        private IDialogService dialogService;
        private IModelService modelService;



        public CreateView()
        {
            InitializeComponent();
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();

        }

        private void PopulateBusinessEntityCombobox()
        {
            IList<BusinessEntity> beList = modelService.GetAllDomainObjectsByApplicationId<BusinessEntity>(BackendApplication.Id);
            beList = beList.OrderBy(be => be.Name).ToList();
            cbBusinessEntity.DataSource = beList;
        }




        private void btnOK_Click(object sender, EventArgs e)
        {
            string newViewName = tbName.Text.Trim();

            // Check name.
            if (!NamingGuidance.CheckViewName(newViewName, true))
            {
                return;
            }

            // Check Title
            if (!NamingGuidance.CheckCaptionFocus(tbTitle, "View Title", true))
                return;

            // Check unique name

            IList<Cdc.MetaManager.DataAccess.Domain.View> viewList = dialogService.GetViewsByNameAndApplicationId(tbName.Text, FrontendApplication.Id);

            if (viewList.Count > 0)
            {
                MessageBox.Show("The name of the view is not unique, please change the value for the view name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbName.SelectAll();
                tbName.Focus();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Create the view
            NewView.BusinessEntity = (BusinessEntity)cbBusinessEntity.SelectedItem;
            NewView.Name = newViewName;
            NewView.Title = tbTitle.Text.Trim();

            // Create VisualTree
            if (rbtnGroupbox.Checked)
            {
                NewView.VisualTree = new UXWrapPanel();

                UXGroupBox groupBox = new UXGroupBox();
                groupBox.Caption = "Change Me";
                NewView.VisualTree.Children.Add(groupBox);

            }
            else if (rbtnGrid.Checked)
            {
                NewView.VisualTree = new UXDockPanel();
                UXDataGrid dataGrid = new UXDataGrid();
                dataGrid.Name = "NoName";
                NewView.VisualTree.Children.Add(dataGrid);
            }
            else if (rbtnTwoWayListBox.Checked)
            {
                NewView.VisualTree = new UXWrapPanel();
                UXTwoWayListBox twoWayListBox = new UXTwoWayListBox();
                twoWayListBox.Name = "TwoWayListBox";
                NewView.VisualTree.Children.Add(twoWayListBox);
            }

            if (NewView.ServiceMethod != null)
            {
                dialogService.CreateViewServiceMethodMap(NewView, NewView.ServiceMethod.Id);
            }
            else
            {
                NewView.RequestMap = new PropertyMap();
                NewView.ResponseMap = new PropertyMap();
            }

            FrontendApplication = modelService.GetInitializedDomainObject<DataAccess.Domain.Application>(FrontendApplication.Id);

            NewView.Application = FrontendApplication;

            // Persist the view
            NewView = (DataAccess.Domain.View)modelService.SaveDomainObject(NewView);
            ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(NewView.Id, typeof(DataAccess.Domain.View));
            //CheckOut view
            CheckOutInObject(NewView, true);



            DialogResult = DialogResult.OK;


            Cursor.Current = Cursors.Default;
        }

        private void CreateView_Load(object sender, EventArgs e)
        {
            PopulateBusinessEntityCombobox();
            NewView = new Cdc.MetaManager.DataAccess.Domain.View();
            NewView.AlternateView = null;
            NewView.Application = FrontendApplication;
            NewView.BusinessEntity = null;
            NewView.OriginalViewName = null;
            NewView.RequestMap = null;
            NewView.ResponseMap = null;
            NewView.ServiceMethod = null;
            NewView.Type = ViewType.Standard;
            EnableDisableButtons();

            Cursor.Current = Cursors.Default;
        }

        private void btnSelectServiceMethod_Click(object sender, EventArgs e)
        {
            using (FindServiceMethodForm findServiceMethod = new FindServiceMethodForm())
            {
                findServiceMethod.Owner = this;
                findServiceMethod.FrontendApplication = FrontendApplication;
                findServiceMethod.BackendApplication = BackendApplication;

                if (findServiceMethod.ShowDialog() == DialogResult.OK)
                {
                    NewView.ServiceMethod = findServiceMethod.ServiceMethod;

                    // Set the text for the ServiceMethod
                    tbServiceMethod.Text = string.Format("({0}) {1}", NewView.ServiceMethod.Id, NewView.ServiceMethod.Name);

                    // Enable buttons
                    EnableDisableButtons();
                }
            }


        }

        private void EnableDisableButtons()
        {

            btnOK.Enabled = false;

            if ((!string.IsNullOrEmpty(tbName.Text.Trim())) &&
               (!string.IsNullOrEmpty(tbTitle.Text.Trim())) &&
                cbBusinessEntity.SelectedItem != null)
            {
                btnOK.Enabled = true;
            }

        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }


        private void cbBusinessEntity_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

        }

        private void CheckOutInObject(DataAccess.IVersionControlled checkOutObject, bool trueCheckOut_falseCheckIn)
        {
            DataAccess.IVersionControlled domainObject = null;
            domainObject = checkOutObject;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(domainObject.GetType()))
            {
                if (trueCheckOut_falseCheckIn)
                {
                    try
                    {
                        MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(domainObject.Id, domainObject.GetType());
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(domainObject.Id, domainObject.GetType(), FrontendApplication);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

    }
}
