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
using NHibernate;

namespace Cdc.MetaManager.GUI
{
    public partial class FindPropertyForm : MdiChildForm
    {
        //private IApplicationService appService = null;
        private IModelService modelService = null;
        private IConfigurationManagementService configurationManagementService = null;

        private IList<Service> serviceList = null;
        private IList<ServiceMethod> serviceMethodList = null;

        public List<Property> SelectedPropertyList { get; private set; }

        private bool SearchedMappedProperty { get; set; }

        public Property SelectedProperty { get; set; }
        public Schema Schema { get; set; }

        public BusinessEntity BusinessEntity { get; set; }
        public BusinessEntity DefaultBusinessEntity { get; set; }
                
        public bool CanShowCustomProperties { get; set; }
        public bool CanMultiSelectProperties { get; set; }
        public bool CanCreateProperties { get; set; }
        public bool HideServicMethodSearch { get; set; }
        public string AutoSearchColumn { get; set; }
        public string AutoSelectTableForFoundColumn { get; set; }

        public FindPropertyForm()
        {
            InitializeComponent();
                        
            AutoSelectTableForFoundColumn = string.Empty;
            modelService = MetaManagerServices.GetModelService();
            configurationManagementService = MetaManagerServices.GetConfigurationManagementService();
        }

        private void FindPropertyForm_Load(object sender, EventArgs e)
        {
            // Check if multiselect
            propertyListView.MultiSelect = CanMultiSelectProperties;

            // Check CanCreateProperties
            if (CanCreateProperties)
                btnCreateProperty.Visible = true;
            else
                btnCreateProperty.Visible = false;


            if (CanShowCustomProperties)
            {
                rbResponseMap.Checked = true;
                mapTypeGb.Visible = false;
            }

            ClearPropertyViewList();

            // Get schema
            //Schema = appService.GetSchemaByApplicationId(BackendApplication.Id);
            Schema = modelService.GetAllDomainObjectsByApplicationId<Schema>(BackendApplication.Id).First();

            // Populate servicelist
            if (serviceList == null)
            {
                //serviceList = appService.GetAllServices(BackendApplication.Id).OrderBy(s => s.Name).ToList();
                serviceList = modelService.GetAllDomainObjectsByApplicationId<Service>(BackendApplication.Id).OrderBy(s => s.Name).ToList();
            }

            // Set datasource
            serviceBindingSource.DataSource = serviceList;

            // Select first in list
            if (serviceCb.Items.Count > 0)
                serviceCb.SelectedIndex = 0;

            rbNameTableColBE.Checked = true;

            if (BusinessEntity != null)
            {
                selectSearchfieldsGb.Visible = false;
                beTbx.Text = BusinessEntity.Name;
                beTbx.ReadOnly = true;
                IsEditable =  BusinessEntity.IsLocked && BusinessEntity.LockedBy == Environment.UserName;
            }
            else
            {
                if (DefaultBusinessEntity != null)
                {
                    beTbx.Text = DefaultBusinessEntity.Name;
                }

                selectSearchfieldsGb.Visible = !HideServicMethodSearch;
                IsEditable = false;
            }

            if (IsEditable)
            {
                okBtn.Enabled = true;
                cancelBtn.Text = "Cancel";
                hintBtn.Visible = true;
            }
            else
            {
                okBtn.Enabled = true; // There is no saving functionality on OK button
                cancelBtn.Text = "Cancel";
                hintBtn.Visible = false;
            }

            if (!string.IsNullOrEmpty(AutoSearchColumn))
            {
                columnTbx.Text = AutoSearchColumn;
            }
                        
            lblInformation.Text = string.Empty;

            if (BusinessEntity != null || DefaultBusinessEntity != null || !string.IsNullOrEmpty(AutoSearchColumn))
            {
                searchBtn_Click(null, null);
                if (propertyListView.Items.Count == 0 && !string.IsNullOrEmpty(AutoSearchColumn))
                {
                    columnTbx.Text = string.Empty;
                    nameTbx.Text = AutoSearchColumn;
                    searchBtn_Click(null, null);
                    if (propertyListView.Items.Count == 0 && BusinessEntity == null && DefaultBusinessEntity != null)
                    {
                        columnTbx.Text = AutoSearchColumn;
                        nameTbx.Text = string.Empty;
                        beTbx.Text = string.Empty;
                        searchBtn_Click(null, null);
                        if (propertyListView.Items.Count == 0)
                        {
                            columnTbx.Text = string.Empty;
                            nameTbx.Text = AutoSearchColumn;
                            beTbx.Text = string.Empty;
                            searchBtn_Click(null, null);
                            if (propertyListView.Items.Count == 0)
                            {
                                beTbx.Text = DefaultBusinessEntity.Name;
                                columnTbx.Text = string.Empty;
                                nameTbx.Text = string.Empty;
                                searchBtn_Click(null, null);
                            }
                        }
                    }
                }
                

            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
            {
                IList<Property> properties = null;

                Dictionary<string, object> propKeys = new Dictionary<string, object>();

                if (BusinessEntity != null)
                {
                    propKeys.Add("BusinessEntity.Id", BusinessEntity.Id);
                }
                else if (!string.IsNullOrEmpty(beTbx.Text.Trim()))
                {
                    propKeys.Add("BusinessEntity.Name", beTbx.Text.Trim());
                    propKeys.Add("BusinessEntity.Application.Id", BackendApplication.Id);
                }
                else
                {
                    propKeys.Add("BusinessEntity.Application.Id", BackendApplication.Id);
                }

                if (!string.IsNullOrEmpty(nameTbx.Text.Trim()))
                {
                    propKeys.Add("Name", nameTbx.Text.Trim());
                }

                if (!string.IsNullOrEmpty(tableTbx.Text.Trim()))
                {
                    propKeys.Add("StorageInfo.TableName" , tableTbx.Text.Trim());
                }

                if (!string.IsNullOrEmpty(columnTbx.Text.Trim()))
                {
                    propKeys.Add("StorageInfo.ColumnName", columnTbx.Text.Trim());
                }

                if (!CanShowCustomProperties)
                {
                    propKeys.Add("StorageInfo.Schema.Id", Schema.Id);
                }

                properties = modelService.GetAllDomainObjectsByPropertyValues<Property>(propKeys, false, true);

                //if (CanShowCustomProperties)
                //{
                //    properties = appService.GetAllPropertiesByNameSchemaTableAndColumn(nameTbx.Text, Schema.Id, tableTbx.Text, columnTbx.Text, beTbx.Text, BackendApplication.Id);
                //}
                //else
                //{
                //    properties = appService.GetAllExistingPropertiesByNameSchemaTableAndColumn(nameTbx.Text, Schema.Id, tableTbx.Text, columnTbx.Text, beTbx.Text);
                //}

                if (CanMultiSelectProperties)
                {
                    gbSelect.Text = "Select Properties (Multiselect Enabled)";
                }
                else
                {
                    gbSelect.Text = "Select Property (Multiselect Disabled)";
                }

                propertyListView.BeginUpdate();

                propertyListView.Items.Clear();

                countLbl.Text = "";

                ListViewItem foundTableItem = null;

                if (properties != null)
                {
                    countLbl.Text = properties.Count.ToString();

                    int c = 0;

                    foreach (Property property in properties)
                    {
                        c++;
                        ListViewItem item = propertyListView.Items.Add(property.Id.ToString());
                        item.SubItems.Add(property.Name);

                        if (property.Type != null)
                            item.SubItems.Add(property.Type.ToString());
                        else
                            item.SubItems.Add("Unknown");

                        item.Tag = property;

                        if (property.StorageInfo != null)
                        {
                            item.SubItems.Add(property.StorageInfo.TableName);
                            item.SubItems.Add(property.StorageInfo.ColumnName);

                            if (!string.IsNullOrEmpty(AutoSelectTableForFoundColumn) &&
                                property.StorageInfo.TableName.ToUpper() == AutoSelectTableForFoundColumn.ToUpper())
                            {
                                foundTableItem = item;
                            }
                        }
                        else
                        {
                            item.SubItems.Add("");
                            item.SubItems.Add("");
                        }

                        if (property.BusinessEntity != null)
                        {
                            item.SubItems.Add(property.BusinessEntity.Name);
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }

                        if (property.Hint != null)
                        {
                            string d = property.Hint.Text;
                        }

                        if (c == 1000)
                            break;
                    }

                    propertyListView.SortByColumn(1, SortOrder.Ascending, false);
                }

                propertyListView.EndUpdate();

                // Always select first row if anything is found and AutoSelectTableForFoundColumn is not set
                if (propertyListView.Items.Count > 0)
                {
                    if (foundTableItem != null)
                    {
                        foundTableItem.Selected = true;
                    }

                    // If still nothing is selected just select first row
                    if (propertyListView.SelectedItems.Count == 0)
                    {
                        propertyListView.Items[0].Selected = true;
                        propertyGrid.SelectedObject = propertyListView.Items[0].Tag;
                    }

                    propertyListView.Focus();
                }
                else
                {
                    propertyGrid.SelectedObject = null;
                }

                CheckEnableDisableButtons();
            }

            Cursor = Cursors.Default;
        }

        private void propertyListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
                propertyGrid.SelectedObject = propertyListView.SelectedItems[0].Tag;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                if (SelectedPropertyList == null)
                {
                    SelectedPropertyList = new List<Property>();
                }

                if (SearchedMappedProperty)
                {
                    foreach (ListViewItem item in propertyListView.SelectedItems)
                    {
                        SelectedPropertyList.Add((Property)(item.Tag as MappedProperty).Target);
                    }

                    this.SelectedProperty = (Property)(propertyListView.SelectedItems[0].Tag as MappedProperty).Target;
                }
                else
                {
                    foreach (ListViewItem item in propertyListView.SelectedItems)
                    {
                        SelectedPropertyList.Add(item.Tag as Property);
                    }

                    this.SelectedProperty = propertyListView.SelectedItems[0].Tag as Property;
                }

                DialogResult = DialogResult.OK;

                Close();
            }
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            AskSaveProperty();
        }

        private void AskSaveProperty()
        {
            if (IsEditable)
            {
                if (MessageBox.Show("Do you want to save any changes made to this property?", "Property", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {                    
                    modelService.MergeSaveDomainObject((Property)propertyGrid.SelectedObject);                    
                }
            }
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

        private void rbNameTableColBE_CheckedChanged(object sender, EventArgs e)
        {
            SearchedMappedProperty = false;

            SelectSearchFields();
            CheckEnableDisableButtons();
        }

        private void SelectSearchFields()
        {
            ClearPropertyViewList();

            if (rbNameTableColBE.Checked)
            {
                gbNameTableColBE.Visible = true;
                gbService.Visible = false;
            }
            else if (rbService.Checked)
            {
                gbNameTableColBE.Visible = false;
                gbService.Visible = true;
                InitiateServiceFields();
            }
        }

        private void ClearPropertyViewList()
        {
            // Remove everything in list including columntitles
            propertyListView.Clear();

            if (SearchedMappedProperty)
            {
                propertyListView.Columns.Add("Id", 33);
                propertyListView.Columns.Add("Name", 140);
                propertyListView.Columns.Add("Prop Id", 55);
                propertyListView.Columns.Add("Property Name", 140);
                propertyListView.Columns.Add("Property Type", 90);
                propertyListView.Columns.Add("Original Table", 90);
                propertyListView.Columns.Add("Original Column", 90);
                propertyListView.Columns.Add("Business Entity", 90);
            }
            else
            {
                propertyListView.Columns.Add("Id", 33);
                propertyListView.Columns.Add("Name", 140);
                propertyListView.Columns.Add("Type", 90);
                propertyListView.Columns.Add("Original Table", 90);
                propertyListView.Columns.Add("Original Column", 90);
                propertyListView.Columns.Add("Business Entity", 90);
            }

            countLbl.Text = "0";
            propertyGrid.SelectedObject = null;
        }

        private void rbService_CheckedChanged(object sender, EventArgs e)
        {
            SearchedMappedProperty = true;

            SelectSearchFields();
            CheckEnableDisableButtons();
        }

        private void InitiateServiceFields()
        {
        }

        private void serviceCb_SelectedValueChanged(object sender, EventArgs e)
        {
            if (serviceCb.SelectedValue != null)
            {
                serviceMethodBindingSource.DataSource = null;

                // Populate servicemethodlist
                serviceMethodList = modelService.GetVersionControlledDomainObjectsForParent(typeof(ServiceMethod), (Guid)serviceCb.SelectedValue).Cast<ServiceMethod>().ToList();

                // Sorting
                serviceMethodList = serviceMethodList.OrderBy(sm => sm.Name).ToList();

                // Set datasource
                serviceMethodBindingSource.DataSource = serviceMethodList;

                serviceMethodCb.SelectedIndex = 0;
            }

            CheckEnableSearchServiceButton();
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

        private void serviceMethodCb_SelectedValueChanged(object sender, EventArgs e)
        {
            CheckEnableSearchServiceButton();
        }

        private void CheckEnableSearchServiceButton()
        {
            if (serviceCb.SelectedValue != null && serviceMethodCb.SelectedValue != null)
            {
                searchServiceBtn.Enabled = true;
            }
            else
            {
                searchServiceBtn.Enabled = false;
            }
        }

        private void searchServiceBtn_Click(object sender, EventArgs e)
        {
            if (serviceMethodCb.SelectedValue != null)
            {
                using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                {
                    List<Property> properties = new List<Property>();
                    List<MappedProperty> mappedProperties = new List<MappedProperty>();

                    // Populate servicemethodlist
                    ServiceMethod serviceMethod = modelService.GetInitializedDomainObject<ServiceMethod>((Guid)serviceMethodCb.SelectedValue);

                    PropertyMap propertyMap = null;

                    if (serviceMethod != null)
                    {
                        // Get the correct map
                        if (rbResponseMap.Checked)
                        {
                            propertyMap = serviceMethod.ResponseMap;
                        }
                        else if (rbRequestMap.Checked)
                        {
                            propertyMap = serviceMethod.RequestMap;
                        }

                        foreach (MappedProperty mappedProp in propertyMap.MappedProperties)
                        {
                            if (mappedProp.Target is Property)
                            {
                                // Remove custom properties if they shouldn't be shown
                                if (!CanShowCustomProperties && (mappedProp.Target as Property).StorageInfo == null)
                                {
                                    continue;
                                }

                                properties.Add((Property)mappedProp.Target);
                                mappedProperties.Add(mappedProp);
                            }
                        }
                    }

                    if (CanMultiSelectProperties)
                    {
                        gbSelect.Text = "Select Mapped Properties (Multiselect Enabled)";
                    }
                    else
                    {
                        gbSelect.Text = "Select Mapped Property (Multiselect Disabled)";
                    }

                    propertyListView.BeginUpdate();

                    propertyListView.Items.Clear();

                    countLbl.Text = "";

                    if (properties != null)
                    {
                        countLbl.Text = properties.Count.ToString();

                        foreach (MappedProperty property in mappedProperties)
                        {
                            ListViewItem item = propertyListView.Items.Add(property.Id.ToString());
                            item.SubItems.Add(property.Name);
                            item.SubItems.Add(property.Target.Id.ToString());
                            item.SubItems.Add(property.Target.Name);
                            item.SubItems.Add(property.Target.Type.ToString());
                            item.Tag = property;

                            if (((Property)property.Target).StorageInfo != null)
                            {
                                item.SubItems.Add(((Property)property.Target).StorageInfo.TableName);
                                item.SubItems.Add(((Property)property.Target).StorageInfo.ColumnName);
                            }
                            else
                            {
                                item.SubItems.Add("");
                                item.SubItems.Add("");
                            }

                            if (((Property)property.Target).BusinessEntity != null)
                            {
                                item.SubItems.Add(((Property)property.Target).BusinessEntity.Name);
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }

                        }
                    }

                    propertyListView.EndUpdate();

                    // Always select first row if anything is found
                    if (propertyListView.Items.Count > 0)
                    {
                        propertyListView.Items[0].Selected = true;
                        propertyGrid.SelectedObject = propertyListView.Items[0].Tag;
                    }
                    else
                    {
                        propertyGrid.SelectedObject = null;
                    }

                    CheckEnableDisableButtons();
                }
            }
        }

        private void serviceMethodCb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchServiceBtn_Click(this, null);
            }
        }

        private void serviceCb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchServiceBtn_Click(this, null);
            }
        }

        private void rbResponseMap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchServiceBtn_Click(this, null);
            }
        }

        private void rbRequestMap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                searchServiceBtn_Click(this, null);
            }
        }

        private Property GetSelectedProperty()
        {
            if (propertyListView.SelectedItems.Count == 0)
                return null;

            Property property = null;

            if (propertyListView.SelectedItems[0].Tag is MappedProperty)
            {
                property = (Property)(propertyListView.SelectedItems[0].Tag as MappedProperty).Target;
            }
            else
            {
                property = propertyListView.SelectedItems[0].Tag as Property;
            }

            return property;
        }
               
        private void propertyListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            CheckEnableDisableButtons();
        }

        private void CheckEnableDisableButtons()
        {
            okBtn.Enabled = false;
            hintBtn.Enabled = false;

            if (propertyListView.SelectedItems.Count > 0)
            {
                okBtn.Enabled = true;
                hintBtn.Enabled = IsEditable;
            }
        }

        private void propertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            foreach (Control cntrl in propertyGrid.Controls[2].Controls)
            {
                cntrl.KeyDown -= new KeyEventHandler(PropertyGrid_KeyDown);
            }

            lblInformation.Text = string.Empty;

            if (propertyGrid.SelectedObject is Property &&
                e.NewSelection != null &&
                e.NewSelection.Label == "DisplayFormat")
            {
                Property property = (Property)propertyGrid.SelectedObject;

                // Check if the type is one of DateTime or Numeric (decimal, double or int)
                if (property != null &&
                    property.CanChangeDisplayFormat())
                {
                    lblInformation.Text = "Push the \"Insert\" button in the inputfield for help on different used DisplayFormats.";

                    foreach (Control cntrl in propertyGrid.Controls[2].Controls)
                    {
                        cntrl.KeyDown += new KeyEventHandler(PropertyGrid_KeyDown);
                    }
                }
            }
        }

        void PropertyGrid_KeyDown(object sender, KeyEventArgs e)
        {
            // If DisplayFormat field and Insert is pushed
            if (propertyGrid.SelectedGridItem.Label == "DisplayFormat" &&
                e.KeyCode == Keys.Insert &&
                !e.Alt &&
                !e.Control &&
                !e.Shift)
            {
                Type selectedObjectType = ((Property)propertyGrid.SelectedObject).Type;

                Property property = propertyGrid.SelectedObject as Property;

                using (SelectDisplayFormats form = new SelectDisplayFormats())
                {
                    form.DisplayFormat = property.DisplayFormat;
                    form.DisplayFormatDataType = selectedObjectType;
                    form.IsEditable = false;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        property.DisplayFormat = form.DisplayFormat;
                        propertyGrid.Refresh();
                        AskSaveProperty();
                    }
                }
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

        private void btnCreateProperty_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> searchProp = new Dictionary<string, object>();
            searchProp.Add("Application.Id", BackendApplication.Id);
            searchProp.Add("Name", "CustomProperties");
            IList<BusinessEntity> beList = modelService.GetAllDomainObjectsByPropertyValues<BusinessEntity>(searchProp);

            if (beList.Count > 0)
            {
                using (CreatePropertyForm createPropertyForm = new CreatePropertyForm(beList[0]))
                {
                    if (createPropertyForm.ShowDialog() == DialogResult.OK)
                    {
                        if (SelectedPropertyList == null)
                        {
                            SelectedPropertyList = new List<Property>();
                        }

                        // Add the property to selected list and set as selected property
                        SelectedPropertyList.Add(createPropertyForm.Property);
                        this.SelectedProperty = createPropertyForm.Property;

                        DialogResult = DialogResult.OK;
                    }
                }
            }

            
        }

        private void hintBtn_Click(object sender, EventArgs e)
        {
            if (IsEditable)
            {
                Property property = GetSelectedProperty();

                if (property != null)
                {
                    using (FindHintForm form = new FindHintForm())
                    {
                        form.Owner = this;
                        form.FrontendApplication = FrontendApplication;
                        form.BackendApplication = BackendApplication;
                        form.hintCollection = BackendApplication.HintCollection;
                        form.SelectedHint = property.Hint;
                        form.IsSelect = true;

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            Hint hint = form.SelectedHint;
                            
                            property.Hint = hint;
                            modelService.SaveDomainObject(property);
                        }
                    }
                }
            }
        }

        private void propertyListView_DoubleClick(object sender, EventArgs e)
        {
            if (okBtn.Visible && okBtn.Enabled)
                okBtn_Click(this, null);
        }

    }
}
