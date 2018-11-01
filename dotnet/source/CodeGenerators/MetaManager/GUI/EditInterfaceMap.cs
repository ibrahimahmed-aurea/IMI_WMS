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


namespace Cdc.MetaManager.GUI
{
    public partial class EditInterfaceMap : MdiChildForm
    {
        private IDialogService dialogService;
        private IModelService modelService = null;
                
        public PropertyMap RequestMap { get; set; }
        public PropertyMap ResponseMap { get; set; }

        public List<MappedProperty> AddedMappedProperties { get; private set; }
        public List<MappedProperty> DeletedMappedProperties { get; private set; }

        public bool searchableParameterUpdated = false;
        
        public bool CanRequestMap { get; set; }

        public EditInterfaceMap()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }
        
        public IList<MappedProperty> SelectedRequestProperties
        {
            get
            {
                List<MappedProperty> list = new List<MappedProperty>();

                foreach (ListViewItem item in lvRequest.SelectedItems)
                {
                    list.Add((MappedProperty)item.Tag);
                }

                return list;
            }
        }

        public IList<MappedProperty> SelectedResponseProperties
        {
            get
            {
                List<MappedProperty> list = new List<MappedProperty>();

                foreach (ListViewItem item in lvResponse.SelectedItems)
                {
                    list.Add((MappedProperty)item.Tag);
                }

                return list;
            }
        }
                
        private void EditViewInterfaceMap_Load(object sender, EventArgs e)
        {
            AddedMappedProperties = new List<MappedProperty>();
            DeletedMappedProperties = new List<MappedProperty>();
            LoadMaps();
            PopulateRequestList();
            PopulateResponseList();
            PopulateContextMenu();
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnOk.Enabled = this.IsEditable;
        }
        
        private void LoadMaps()
        {
            if (RequestMap != null && RequestMap.Id != Guid.Empty)
            {
                RequestMap = modelService.GetDynamicInitializedDomainObject<PropertyMap>(RequestMap.Id, new List<string> { "MappedProperties" });
            }

            if (ResponseMap != null)
            {
                if (ResponseMap.Id != Guid.Empty)
                {
                    ResponseMap = modelService.GetDynamicInitializedDomainObject<PropertyMap>(ResponseMap.Id, new List<string> { "MappedProperties" });
                }
            }
            else
            {
                if (tcMaps.TabPages.Contains(tpResponse))
                {
                    tcMaps.TabPages.Remove(tpResponse);
                }
            }
        }
        
        private void PopulateContextMenu()
        {
            if (ResponseMap != null && RequestMap != null)
            {
                tsmiResponseMapFromRequest.DropDownItems.Clear();

                ToolStripMenuItem clearItem = new ToolStripMenuItem("(Clear)");
                clearItem.Tag = null;
                clearItem.Click += new EventHandler(RequestFieldMapping_Click);
                tsmiResponseMapFromRequest.DropDownItems.Add(clearItem);

                foreach (MappedProperty prop in RequestMap.MappedProperties.OrderBy(p => p.Name))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(prop.Name);
                    item.Tag = prop;
                    item.Click += new EventHandler(RequestFieldMapping_Click);
                    item.Enabled = this.IsEditable;
                    tsmiResponseMapFromRequest.DropDownItems.Add(item);
                }
            }
        }

        void RequestFieldMapping_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            MappedProperty reqProp = (MappedProperty)item.Tag;

            if (SelectedResponseProperties.Count > 0)
            {
                foreach (MappedProperty respProp in SelectedResponseProperties)
                {
                    respProp.RequestMappedProperty = reqProp;
                }

                PopulateResponseList();
                PopulateContextMenu();
            }
        }

        private void PopulateResponseList()
        {
            if (ResponseMap != null)
            {
                lvResponse.Items.Clear();

                foreach (MappedProperty prop in ResponseMap.MappedProperties)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = prop.Id.ToString();
                    item.SubItems.Add(prop.Sequence.ToString());
                    item.SubItems.Add(prop.Name);
                    item.SubItems.Add(prop.Type.ToString());

                    if (!string.IsNullOrEmpty(prop.DefaultValue))
                        item.SubItems.Add(prop.DefaultValue);
                    else if (prop.DefaultSessionProperty != null)
                        item.SubItems.Add(string.Format("[{0}]", prop.DefaultSessionProperty.Name));
                    else
                        item.SubItems.Add(string.Empty);

                    item.SubItems.Add(prop.DisplayFormat);
                    item.SubItems.Add(prop.IsCustom.ToString());

                    if (prop.RequestMappedProperty != null)
                    {
                        MappedProperty reqMP = modelService.GetDomainObject<MappedProperty>(prop.RequestMappedProperty.Id);
                        if (reqMP != null)
                        {
                            item.SubItems.Add(string.Format("{0} - {1}", reqMP.Name, reqMP.ToString()));
                        }
                        else
                        {
                            item.SubItems.Add(string.Format("{0} - {1}", prop.RequestMappedProperty.Name, prop.RequestMappedProperty.ToString()));
                        }
                    }
                    else
                    {
                        item.SubItems.Add(string.Empty);
                    }

                    if (prop.Target != null)
                    {
                        if (prop.Target is Property)
                        {
                            PropertyStorageInfo info = ((Property)prop.Target).StorageInfo;

                            if (info != null &&
                                !string.IsNullOrEmpty(info.TableName) &&
                                !string.IsNullOrEmpty(info.ColumnName))
                            {
                                item.SubItems.Add(string.Format("{0}.{1}", info.TableName, info.ColumnName));
                            }
                            else
                            {
                                item.SubItems.Add(string.Empty);
                            }
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }
                    }
                    else
                    {
                        item.SubItems.Add(string.Empty);
                    }

                    item.Tag = prop;
                    lvResponse.Items.Add(item);
                }
            }
        }

        private void PopulateRequestList()
        {
            if (RequestMap != null)
            {
                lvRequest.Items.Clear();

                foreach (MappedProperty prop in RequestMap.MappedProperties)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = prop.Id.ToString();
                    item.SubItems.Add(prop.Sequence.ToString());
                    item.SubItems.Add(prop.Name);
                    item.SubItems.Add(prop.Type.ToString());

                    if (!string.IsNullOrEmpty(prop.DefaultValue))
                        item.SubItems.Add(prop.DefaultValue);
                    else if (prop.DefaultSessionProperty != null)
                        item.SubItems.Add(string.Format("[{0}]", prop.DefaultSessionProperty.Name));
                    else
                        item.SubItems.Add(string.Empty);

                    item.SubItems.Add(prop.IsSearchable.ToString());
                    item.SubItems.Add(prop.IsCustom.ToString());

                    if (prop.Target != null)
                    {
                        if (prop.Target is Property)
                        {
                            PropertyStorageInfo info = ((Property)prop.Target).StorageInfo;

                            if (info != null &&
                                !string.IsNullOrEmpty(info.TableName) &&
                                !string.IsNullOrEmpty(info.ColumnName))
                            {
                                item.SubItems.Add(string.Format("{0}.{1}", info.TableName, info.ColumnName));
                            }
                            else
                            {
                                item.SubItems.Add(string.Empty);
                            }
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }
                    }
                    else
                    {
                        item.SubItems.Add(string.Empty);
                    }

                    item.Tag = prop;
                    lvRequest.Items.Add(item);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Check if any of the names are the same
            IEnumerable<string> nonUniqueNames = from property in RequestMap.MappedProperties
                                                 where !string.IsNullOrEmpty(property.Name)
                                                 group property by property.Name into grouped
                                                 where grouped.Count() > 1
                                                 select grouped.Key;

            if (nonUniqueNames.Count() > 0)
            {
                MessageBox.Show(string.Format("One or more Request Properties have the same name \"{0}\". Change the name of the non-unique Custom properties.", nonUniqueNames.First()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if any of the names are the same
            nonUniqueNames = from property in ResponseMap.MappedProperties
                             where !string.IsNullOrEmpty(property.Name)
                             group property by property.Name into grouped
                             where grouped.Count() > 1
                             select grouped.Key;

            if (nonUniqueNames.Count() > 0)
            {
                MessageBox.Show(string.Format("One or more Response Properties have the same name \"{0}\". Change the name of the non-unique Custom properties.", nonUniqueNames.First()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
                
        private void requestContextMenu_Opening(object sender, CancelEventArgs e)
        {
            tsmiRequestRemoveCustomField.Enabled = SelectedRequestProperties.Count == 1 && SelectedRequestProperties[0].IsCustom && this.IsEditable;
            tsmiRequestEdit.Enabled = SelectedRequestProperties.Count > 0 && this.IsEditable;
            tsmiRequestAddCustomField.Enabled = this.IsEditable;
        }

        private void lvRequest_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.IsEditable)
            {
                if (e.KeyCode == Keys.F2)
                {
                    EditSelectedRequestFields(SelectedRequestProperties);
                }
                else if (e.KeyCode == Keys.Insert)
                {
                    AddCustomFieldsToRequestMap();
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (lvRequest.SelectedItems.Count > 0)
                    {
                        RemoveCustomMappedPropertyFromMap((MappedProperty)lvRequest.SelectedItems[0].Tag, RequestMap);
                    }
                }
            }
        }

        private void lvResponse_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.IsEditable)
            {
                if (e.KeyCode == Keys.F2)
                {
                    EditSelectedResponseFields(SelectedResponseProperties);
                }
                else if (e.KeyCode == Keys.Insert)
                {
                    AddCustomFieldsToResponseMap();
                }
                else if (e.KeyCode == Keys.Delete)
                {
                    if (lvResponse.SelectedItems.Count > 0)
                    {
                        RemoveCustomMappedPropertyFromMap((MappedProperty)lvResponse.SelectedItems[0].Tag, ResponseMap);
                    }
                }
            }
        }

        private void EditSelectedRequestFields(IList<MappedProperty> requestProperties)
        {
            if (requestProperties != null && requestProperties.Count > 0)
            {
                using (EditInterfaceProperty form = new EditInterfaceProperty())
                {
                    form.Owner = this;
                    form.IsRequestProperty = true;
                    form.EditMappedProperties = requestProperties;
                    form.SessionProperties = dialogService.GetUXSessionProperties(FrontendApplication);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (searchableParameterUpdated == false) 
                        {
                            if (form.SearchableParameterUpdated == true)
                            {
                                searchableParameterUpdated = true;
                            }
                        }

                        PopulateRequestList();
                    }
                }
            }
        }

        private void EditSelectedResponseFields(IList<MappedProperty> responseProperties)
        {
            if (responseProperties != null && responseProperties.Count > 0)
            {
                using (EditInterfaceProperty form = new EditInterfaceProperty())
                {
                    form.Owner = this;
                    form.IsRequestProperty = false;
                    form.EditMappedProperties = responseProperties;
                    form.SessionProperties = dialogService.GetUXSessionProperties(FrontendApplication);
                    form.IsEditable = this.IsEditable;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        PopulateResponseList();
                    }
                }
            }
        }

        private void lvRequest_DoubleClick(object sender, EventArgs e)
        {
            if (this.IsEditable)
            {
                EditSelectedRequestFields(SelectedRequestProperties);
            }
        }

        private void lvResponse_DoubleClick(object sender, EventArgs e)
        {
            if (this.IsEditable)
            {
                EditSelectedResponseFields(SelectedResponseProperties);
            }
        }

        private void tsmiRequestEdit_Click(object sender, EventArgs e)
        {
            if (this.IsEditable)
            {
                EditSelectedRequestFields(SelectedRequestProperties);
            }
        }

        private void tsmiResponseEdit_Click(object sender, EventArgs e)
        {
            if (this.IsEditable)
            {
                EditSelectedResponseFields(SelectedResponseProperties);
            }
        }

        private void tsmiResponseAddCustomField_Click(object sender, EventArgs e)
        {
            AddCustomFieldsToResponseMap();
        }

        private void tsmiResponseRemoveCustomField_Click(object sender, EventArgs e)
        {
            RemoveCustomMappedPropertyFromMap((MappedProperty)lvResponse.SelectedItems[0].Tag, ResponseMap);
        }

        private void tsmiRequestAddCustomField_Click(object sender, EventArgs e)
        {
            AddCustomFieldsToRequestMap();
        }

        private void tsmiRequestRemoveCustomField_Click(object sender, EventArgs e)
        {
            RemoveCustomMappedPropertyFromMap((MappedProperty)lvRequest.SelectedItems[0].Tag, RequestMap);
        }

        private void AddCustomFieldsToRequestMap()
        {
            if (AddCustomMappedPropertyToMap(RequestMap))
            {
                // Refresh map
                PopulateRequestList();
                PopulateContextMenu();
            }
        }

        private void AddCustomFieldsToResponseMap()
        {
            if (AddCustomMappedPropertyToMap(ResponseMap))
            {
                // Refresh map
                PopulateResponseList();
                PopulateContextMenu();
            }
        }

        private bool AddCustomMappedPropertyToMap(PropertyMap map)
        {
            bool retVal = false;

            using (FindPropertyForm form = new FindPropertyForm())
            {
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.CanShowCustomProperties = true;
                form.CanMultiSelectProperties = true;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    foreach (Property property in form.SelectedPropertyList)
                    {
                        MappedProperty newProperty = new MappedProperty();

                        int sequence = 1;

                        if (map.MappedProperties.Count > 0)
                            sequence = map.MappedProperties.Max(p => p.Sequence) + 1;

                        newProperty.IsCustom = true;
                        newProperty.Target = property;
                        newProperty.PropertyMap = map;
                        newProperty.Sequence = sequence;

                        map.MappedProperties.Add(newProperty);

                        AddedMappedProperties.Add(newProperty);

                        retVal = true;
                    }
                }
            }

            return retVal;
        }

        private void RemoveCustomMappedPropertyFromMap(MappedProperty mappedProperty, PropertyMap map)
        {
            if (!mappedProperty.IsCustom)
            {
                return;
            }

            if (MessageBox.Show("Are you sure you want to remove the selected Property?", "Remove Property", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                map.MappedProperties.Remove(mappedProperty);

                if (AddedMappedProperties.Contains(mappedProperty))
                {
                    AddedMappedProperties.Remove(mappedProperty);
                }
                else
                {
                    DeletedMappedProperties.Add(mappedProperty);
                }

                PopulateRequestList();
                PopulateResponseList();
                PopulateContextMenu();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void responseContextMenu_Opening(object sender, CancelEventArgs e)
        {
            tsmiResponseRemoveCustomField.Enabled = SelectedResponseProperties.Count == 1 && SelectedResponseProperties[0].IsCustom && this.IsEditable;
            tsmiResponseEdit.Enabled = SelectedResponseProperties.Count > 0 && this.IsEditable;
            tsmiResponseMapFromRequest.Enabled = CanRequestMap && SelectedResponseProperties.Count > 0 && this.IsEditable;
            tsmiResponseAddCustomField.Enabled = this.IsEditable;
        }

    }
}
