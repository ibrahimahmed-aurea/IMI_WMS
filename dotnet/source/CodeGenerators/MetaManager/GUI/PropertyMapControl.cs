using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Context;
using Spring.Context.Support;
using NHibernate.Mapping;
using Cdc.MetaManager.BusinessLogic;


namespace Cdc.MetaManager.GUI
{
    public partial class PropertyMapControl : UserControl
    {
        private Dictionary<PropertyMap, List<MappedProperty>> mappedPropertiesToDelete;
        private List<MappedProperty> existingProperties;

        public PropertyMap RequestMap { get; set; }
        public PropertyMap ResponseMap { get; set; }
        public List<IMappableProperty> MappableProperties { get; set; }
        public Cdc.MetaManager.DataAccess.Domain.Application BackendApplication { get; set; }
        public bool IsEditable { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
            
        public PropertyMapControl()
        {
            InitializeComponent();
            mappedPropertiesToDelete = new Dictionary<PropertyMap, List<MappedProperty>>();
            existingProperties = new List<MappedProperty>();
        }

        public Dictionary<PropertyMap, List<MappedProperty>> MappedPropertiesToDelete
        {
            get
            {
                return mappedPropertiesToDelete;
            }
        }

        public void Map()
        {
            propertyListView.Items.Clear();
            detailGrid.SelectedObject = null;

            if (RequestMap == null)
                RequestMap = new PropertyMap();

            if (ResponseMap == null)
                ResponseMap = new PropertyMap();

            CleanMaps();
           
            foreach (DbProperty dbProperty in MappableProperties.Cast<DbProperty>())
            {
                MappedProperty mappedProperty = null;

                if (dbProperty.PropertyType == DbPropertyType.In || dbProperty.PropertyType == DbPropertyType.InOut)
                {
                    var temp = (from mp in RequestMap.MappedProperties
                                where mp.Source.Name == dbProperty.Name
                                select mp).LastOrDefault();

                    if (temp == null)
                    {
                        temp = new MappedProperty();
                        temp.PropertyMap = RequestMap;
                        temp.IsEnabled = true;
                        temp.IsMandatory = dbProperty.IsMandatory.GetValueOrDefault();
                        temp.Sequence = dbProperty.Sequence;
                        temp.Target = FindTargetProperty(dbProperty);
                        RequestMap.MappedProperties.Add(temp);
                    }

                    temp.Source = dbProperty;

                    if (FindTargetProperty(dbProperty) != null)
                    {
                        existingProperties.Add(temp);
                    }

                    mappedProperty = temp;
                }

                if (dbProperty.PropertyType == DbPropertyType.Out || dbProperty.PropertyType == DbPropertyType.InOut)
                {
                    var temp = (from mp in ResponseMap.MappedProperties
                                where mp.Source.Name == dbProperty.Name
                                select mp).LastOrDefault();

                    if (temp == null)
                    {
                        temp = new MappedProperty();
                        temp.PropertyMap = ResponseMap;
                        temp.IsEnabled = true;
                        temp.IsMandatory = dbProperty.IsMandatory.GetValueOrDefault();
                        temp.Sequence = dbProperty.Sequence;
                        temp.Target = FindTargetProperty(dbProperty);
                        ResponseMap.MappedProperties.Add(temp);
                    }

                    temp.Source = dbProperty;

                    if (mappedProperty == null)
                    {
                        if (FindTargetProperty(dbProperty) != null)
                        {
                            existingProperties.Add(temp);
                        }

                        mappedProperty = temp;
                    }
                }

                ListViewItem item = propertyListView.Items.Add(dbProperty.PropertyType.ToString());
                item.SubItems.Add(mappedProperty.Source.Name);
                item.SubItems.Add(GetDataTypeName(mappedProperty));
                                                                
                if (string.IsNullOrEmpty(mappedProperty.Name))
                {
                    if (mappedProperty.Target != null)
                        item.SubItems.Add(mappedProperty.Target.Name);
                    else
                        item.SubItems.Add("");
                }
                else
                {
                    item.SubItems.Add(mappedProperty.Name);
                }

                if ((mappedProperty.Target != null) && (mappedProperty.Target.Type != null))
                {
                    item.SubItems.Add(mappedProperty.Target.Type.ToString());
                }
                else
                {
                    item.SubItems.Add("");
                }

                item.Tag = mappedProperty;
            }
        
            UpdateListViewColors();
            EnableDisableButtons();
        }

        private void CleanMaps()
        {
            foreach (MappedProperty mp in RequestMap.MappedProperties.ToArray())
            {
                var existing = (from p in MappableProperties.Cast<DbProperty>()
                                where p.Id == mp.Source.Id &&
                                p.Name == mp.Source.Name &&
                                (p.PropertyType == DbPropertyType.In ||
                                p.PropertyType == DbPropertyType.InOut)
                                select p).LastOrDefault();

                if (existing == null)
                {
                    if (mp.Id != Guid.Empty)
                    {
                        if (!mappedPropertiesToDelete.ContainsKey(RequestMap))
                        {
                            mappedPropertiesToDelete.Add(RequestMap, new List<MappedProperty>());
                        }

                        mappedPropertiesToDelete[RequestMap].Add(mp);
                    }

                    RequestMap.MappedProperties.Remove(mp);
                }
            }

            foreach (MappedProperty mp in ResponseMap.MappedProperties.ToArray())
            {
                var existing = (from p in MappableProperties.Cast<DbProperty>()
                                where p.Id == mp.Source.Id &&
                                p.Name == mp.Source.Name &&
                                (p.PropertyType == DbPropertyType.Out ||
                                p.PropertyType == DbPropertyType.InOut)
                                select p).LastOrDefault();

                if (existing == null)
                {
                    if (mp.Id != Guid.Empty)
                    {
                        if (!mappedPropertiesToDelete.ContainsKey(ResponseMap))
                        {
                            mappedPropertiesToDelete.Add(ResponseMap, new List<MappedProperty>());
                        }

                        mappedPropertiesToDelete[ResponseMap].Add(mp);
                    }

                    ResponseMap.MappedProperties.Remove(mp);
                }
            }
        }

        private void EnableDisableButtons()
        {
            mapExistingBtn.Enabled = this.IsEditable;
            mapNewBtn.Enabled = this.IsEditable;
            renameBtn.Enabled = this.IsEditable;
        }

        private void UpdateListViewColors()
        {
            foreach (ListViewItem item in propertyListView.Items)
            {
                item.Font = new Font(item.Font, FontStyle.Regular);
                item.ForeColor = Color.Black;
            }

            foreach (ListViewItem item in propertyListView.Items)
            {
                MappedProperty mappedProperty = (MappedProperty)item.Tag;

                if (IsPossibleBoolean(mappedProperty) &&
                    mappedProperty.Target != null &&
                    (mappedProperty.Target.Type != typeof(bool)))
                {
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }
            }

            IList<string> nonUniqueNames;
                        
            nonUniqueNames = GetNonUniqueNames(RequestMap);

            if (nonUniqueNames.Count > 0)
            {
                foreach (string name in nonUniqueNames)
                {
                    foreach (MappedProperty prop in RequestMap.MappedProperties)
                    {
                        if (prop.Name == name)
                        {
                            ListViewItem item = FindListViewItemTag(prop);

                            item.ForeColor = Color.OrangeRed;
                        }
                    }
                }
            }
                        
            nonUniqueNames = GetNonUniqueNames(ResponseMap);

            if (nonUniqueNames.Count > 0)
            {
                foreach (string name in nonUniqueNames)
                {
                    foreach (MappedProperty prop in ResponseMap.MappedProperties)
                    {
                        if (prop.Name == name)
                        {
                            ListViewItem item = FindListViewItemTag(prop);

                            item.ForeColor = Color.OrangeRed;
                        }
                    }
                }
            }
        }

        private ListViewItem FindListViewItemTag(MappedProperty property)
        {
            foreach (ListViewItem item in propertyListView.Items)
            {
                if (item.Tag.Equals(property))
                    return item;
            }

            return null;
        }

        private IList<string> GetNonUniqueNames(PropertyMap propertyMap)
        {
            if (propertyMap != null)
                return (from property in propertyMap.MappedProperties
                        where !string.IsNullOrEmpty(property.Name)
                        group property by property.Name into grouped
                        where grouped.Count() > 1
                        select grouped.Key).ToList();
            else
                return new List<string>();
        }
                
        private bool IsPossibleBoolean(MappedProperty mappedProperty)
        {
            DbProperty dbProperty = mappedProperty.Source as DbProperty;

            return dbProperty != null &&
                   !string.IsNullOrEmpty(dbProperty.DbDatatype) &&
                   dbProperty.DbDatatype.ToUpper() == "VARCHAR2" &&
                   dbProperty.Length == 1;
        }

        private string GetDataTypeName(MappedProperty mappedProperty)
        {
            DbProperty dbProperty = mappedProperty.Source as DbProperty;

            if (dbProperty != null)
            {
                if (string.IsNullOrEmpty(dbProperty.DbDatatype))
                    return string.Empty;
                else if (dbProperty.DbDatatype.ToUpper() == "VARCHAR2")
                    return string.Format("{0} ({1})", dbProperty.DbDatatype, dbProperty.Length);
                else if (dbProperty.DbDatatype.ToUpper() == "NUMBER")
                {
                    if (dbProperty.Scale > 0)
                        return string.Format("{0} ({1},{2})", dbProperty.DbDatatype, dbProperty.Precision, dbProperty.Scale);
                    else
                        return string.Format("{0} ({1})", dbProperty.DbDatatype, dbProperty.Precision);
                }
                else if (dbProperty.DbDatatype.ToUpper() == "DATE")
                    return string.Format("{0}", dbProperty.DbDatatype);
                else
                    return string.Format("{0} ({1},{2},{3})", dbProperty.DbDatatype, dbProperty.Length, dbProperty.Precision, dbProperty.Scale);
            }

            return null;
        }
                        
        private Cdc.MetaManager.DataAccess.Domain.Property FindTargetProperty(DbProperty property)
        {
            Dictionary<string, object> criteria = new Dictionary<string,object>();
            criteria.Add("StorageInfo.TableName", property.OriginalTable);
            criteria.Add("StorageInfo.ColumnName", property.OriginalColumn);
            criteria.Add("BusinessEntity.Application.Id", BackendApplication.Id);
                        
            return MetaManagerServices.GetModelService().GetAllDomainObjectsByPropertyValues<Cdc.MetaManager.DataAccess.Domain.Property>(criteria).FirstOrDefault();
        }

        private void mapExistingBtn_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                using (FindPropertyForm findPropertyForm = new FindPropertyForm())
                {
                    MappedProperty mappedProperty = propertyListView.SelectedItems[0].Tag as MappedProperty;

                    findPropertyForm.BackendApplication = BackendApplication;
                    findPropertyForm.CanShowCustomProperties = true;
                    findPropertyForm.HideServicMethodSearch = true;
                    findPropertyForm.CanMultiSelectProperties = false;
                    findPropertyForm.DefaultBusinessEntity = BusinessEntity;
                    findPropertyForm.AutoSearchColumn = mappedProperty.Source.Name;
                    
                    if (findPropertyForm.ShowDialog() == DialogResult.OK)
                    {
                        

                        Cdc.MetaManager.DataAccess.Domain.Property property = findPropertyForm.SelectedProperty;

                        mappedProperty.Target = property;

                        UpdateResponseProperty(mappedProperty);
                                                
                        DbProperty dbProp = mappedProperty.Source as DbProperty;

                        if (property.StorageInfo != null)
                        {
                            dbProp.DbDatatype = property.StorageInfo.StorageType;
                            dbProp.Length = property.StorageInfo.Length;
                            dbProp.Scale = property.StorageInfo.Scale;
                            dbProp.Precision = property.StorageInfo.Precision;
                        }

                        propertyListView.SelectedItems[0].SubItems[3].Text = mappedProperty.Target.Name;
                        propertyListView.SelectedItems[0].SubItems[4].Text = mappedProperty.Target.Type.ToString();
                        detailGrid.SelectedObject = mappedProperty;

                        // Update the colors
                        UpdateListViewColors();
                    }
                }
            }
        }

        private void mapNewBtn_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                MappedProperty mappedProperty = propertyListView.SelectedItems[0].Tag as MappedProperty;

                if (FindTargetProperty((DbProperty)mappedProperty.Source) != null)
                {
                    if (MessageBox.Show("You are trying to map a field, that has been found to be connected to a table and column, to a new custom property.\nIs this what you want?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }

                using (CreatePropertyForm createPropertyForm = new CreatePropertyForm(BusinessEntity))
                {
                    createPropertyForm.BackendApplication = BackendApplication;
                    createPropertyForm.DbProperty = mappedProperty.Source as DbProperty;

                    if (createPropertyForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            mappedProperty.Target = createPropertyForm.Property;

                            UpdateResponseProperty(mappedProperty);                            
                            
                            DbProperty dbProp = mappedProperty.Source as DbProperty;

                            if (string.IsNullOrEmpty(dbProp.DbDatatype))
                            {
                                if (mappedProperty.Target.Type == typeof(string))
                                {
                                    dbProp.DbDatatype = "VARCHAR2";
                                    if ((dbProp.PropertyType == DbPropertyType.InOut) || (dbProp.PropertyType == DbPropertyType.Out))
                                    {
                                        dbProp.Length = 255;
                                    }
                                }
                            }
                            
                            propertyListView.SelectedItems[0].SubItems[3].Text = mappedProperty.Target.Name;
                            propertyListView.SelectedItems[0].SubItems[4].Text = mappedProperty.Target.Type.ToString();

                            detailGrid.SelectedObject = mappedProperty;

                            // Update the colors
                            UpdateListViewColors();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
        }

        private void renameBtn_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                MappedProperty mappedProperty = propertyListView.SelectedItems[0].Tag as MappedProperty;

                if (mappedProperty.Target != null)
                {
                    using (RenamePropertyForm form = new RenamePropertyForm())
                    {
                        if (string.IsNullOrEmpty(mappedProperty.Name))
                            form.OldName = mappedProperty.Target.Name;
                        else
                            form.OldName = mappedProperty.Name;

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            mappedProperty.Name = form.NewName;

                            UpdateResponseProperty(mappedProperty);

                            UpdateListViewItemMappedPropertyName(propertyListView.SelectedItems[0]);

                            UpdateListViewColors();
                        }
                    }
                }
            }
        }

        private void UpdateResponseProperty(MappedProperty mappedProperty)
        {
            var p = (from mp in ResponseMap.MappedProperties
                     where mp.Source.Name == mappedProperty.Source.Name &&
                     ((DbProperty)mp.Source).PropertyType == DbPropertyType.InOut
                     select mp).LastOrDefault(); ;

            if (p != null)
            {
                p.Target = mappedProperty.Target;
                p.Name = mappedProperty.Name;
            }
        }

        private void UpdateListViewItemMappedPropertyName(ListViewItem item)
        {
            if (item != null)
            {
                MappedProperty mappedProperty = (MappedProperty)item.Tag;

                if (mappedProperty != null)
                {
                    if (string.IsNullOrEmpty(mappedProperty.Name))
                    {
                        if (mappedProperty.Target != null)
                            item.SubItems[3].Text = mappedProperty.Target.Name;
                        else
                            item.SubItems[3].Text = string.Empty;
                    }
                    else
                    {
                        item.SubItems[3].Text = mappedProperty.Name;
                    }
                }
            }
        }
                
        private void propertyListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                MappedProperty mappedProperty = e.Item.Tag as MappedProperty;

                detailGrid.SelectedObject = mappedProperty;
                                
                if (existingProperties.Contains(mappedProperty))
                {
                    mapNewBtn.Enabled = false;
                }
                else
                {
                    mapNewBtn.Enabled = this.IsEditable;
                }
            }
        }

        private void PropertyListcontextMenu_Opening(object sender, CancelEventArgs e)
        {
            
                mapExistingToolStripMenuItem.Enabled = (propertyListView.SelectedItems.Count > 0);
                mapNewToolStripMenuItem.Enabled = (propertyListView.SelectedItems.Count > 0);
                renameToolStripMenuItem.Enabled = (propertyListView.SelectedItems.Count > 0);
            
        }
    }
}
