using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess;
using Spring.Context.Support;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Context;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess.Dao;


namespace Cdc.MetaManager.GUI
{
    public partial class PropertyMapControl2 : UserControl
    {
        public IEnumerable<IMappableProperty> SourceProperties { get; set; }
        public IEnumerable<IMappableProperty> TargetProperties { get; set; }
        public IEnumerable<IMappableProperty> RequestProperties { get; set; }
        public List<MappedProperty> SyncAddedProperties = new List<MappedProperty>();
        public List<MappedProperty> SyncDeletedProperties = new List<MappedProperty>();
        public List<MappedProperty> SyncChangedProperties = new List<MappedProperty>();
        public PropertyMap PropertyMap { get; set; }
        public DelegateCheckName CheckTargetNameProc = null;
        public bool AllowNonUniquePropertyNames { get; set; }
        public bool EnablePropertiesByDefault { get; set; }
        
        public event EventHandler<PropertyChangedEventArgs> SelectedPropertyChanged;
        public bool MultiSelMode { get; set; }
        public bool IsEditable { get; set; }
        public bool LockPropertyGrid { get; set; }

        private IDictionary<string, object> changedMultiSelProperties;

        private IDictionary<string, object> ChangedMultiSelProperties
        {
            get
            {
                if (changedMultiSelProperties == null)
                {
                    changedMultiSelProperties = new Dictionary<string, object>();
                }

                return changedMultiSelProperties;
            }
            set
            {
                changedMultiSelProperties = value;
            }
        }

        public MappedProperty SelectedProperty
        {
            get 
            {
                if (propertyListView.SelectedItems.Count == 1)
                    return propertyListView.SelectedItems[0].Tag as MappedProperty;
                else
                    return null;
            }
        }

        public PropertyMapControl2()
        {
            InitializeComponent();

            lblInformation.Text = string.Empty;
            this.IsEditable = false;
        }

        void SourceGrid_KeyDown(object sender, KeyEventArgs e)
        {
            // If DisplayFormat field and Insert is pushed
            if (sourceGrid.SelectedGridItem.Label == "DisplayFormat" &&
                e.KeyCode == Keys.Insert &&
                !e.Alt &&
                !e.Control &&
                !e.Shift)
            {
                Type selectedObjectType = ((MappedProperty)sourceGrid.SelectedObject).Type;

                MappedProperty property = sourceGrid.SelectedObject as MappedProperty;

                using (SelectDisplayFormats form = new SelectDisplayFormats())
                {
                    form.DisplayFormat = property.DisplayFormat;
                    form.DisplayFormatDataType = selectedObjectType;
                    

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        property.DisplayFormat = form.DisplayFormat;
                        sourceGrid.Refresh();
                    }
                }
            }
        }

        public void ValidateMapping()
        {
            foreach (MappedProperty property in PropertyMap.MappedProperties)
            {
                string name = property.Name;

                if (string.IsNullOrEmpty(name))
                    name = property.Source.Name;

                if (property.IsEnabled && !property.IsMapped)
                {
                    throw new ArgumentException("Property must have a target or a default value.", name);
                }

                // Name should be checked only when target is set.
                if (CheckTargetNameProc != null && property.Target != null)
                {
                    // Call this to NOT show errormessage since it will then
                    // throw the errormessage as an exception instead if the name is wrong.
                    CheckTargetNameProc(property.Name, false);
                }

                if (!string.IsNullOrEmpty(property.DefaultValue))
                {
                    try
                    {
                        if (property.IsCustom)
                            Convert.ChangeType(property.DefaultValue, property.Target.Type);
                        else
                            Convert.ChangeType(property.DefaultValue, property.Source.Type);
                    }
                    catch (FormatException ex)
                    {
                        throw new ArgumentException(string.Format("Default value is not valid: {0}", ex.Message), name);
                    }
                    catch (InvalidCastException ex)
                    {
                        throw new ArgumentException(string.Format("Default value is not valid: {0}", ex.Message), name);
                    }
                }
            }

            // Check if any of the names are the same
            IEnumerable<string> nonUniqueNames = from property in PropertyMap.MappedProperties
                                                 where !string.IsNullOrEmpty(property.Name)
                                                 group property by property.Name into grouped
                                                 where grouped.Count() > 1
                                                 select grouped.Key;

            if ((nonUniqueNames.Count() > 0) && (!AllowNonUniquePropertyNames))
            {
                throw new ArgumentException(string.Format("One or more MappedProperties has the same name \"{0}\". The names must be unique.", nonUniqueNames.First()));
            }

        }

        public void Map()
        {
            ToolStripItem item = null;

            defaultMenuItem.DropDownItems.Clear();
            targetMenuItem.DropDownItems.Clear();
            requestMenuItem.DropDownItems.Clear();

            item = defaultMenuItem.DropDownItems.Add("(Clear)");
            item.Click += new EventHandler(DefaultSessionPropertyClickEventHandler);
            item.Enabled = this.IsEditable;

            item = targetMenuItem.DropDownItems.Add("(Clear)");
            item.Click += new EventHandler(TargetPropertyClickEventHandler);
            item.Enabled = this.IsEditable;

            item = requestMenuItem.DropDownItems.Add("(Clear)");
            item.Click += new EventHandler(RequestPropertyClickEventHandler);
            item.Enabled = this.IsEditable;

            if (TargetProperties.Count() > 0)
            {
                foreach (IMappableProperty targetProperty in TargetProperties.OrderBy(property => property.Name))
                {
                    if (targetProperty is UXSessionProperty)
                    {
                        item = defaultMenuItem.DropDownItems.Add(string.Format("[Session] {0} ({1})", targetProperty.Name, targetProperty.Type.Name));
                        item.Click += new EventHandler(DefaultSessionPropertyClickEventHandler);
                        item.Enabled = this.IsEditable;
                    }
                    else
                    {
                        item = targetMenuItem.DropDownItems.Add(string.Format("{0}{1}", targetProperty.Name, targetProperty.Type != null ? string.Format(" ({0})", targetProperty.Type.Name) : string.Empty));
                        item.Click += new EventHandler(TargetPropertyClickEventHandler);
                        item.Enabled = this.IsEditable;
                    }

                    item.Tag = targetProperty;
                }

                defaultMenuItem.Enabled = true;
                targetMenuItem.Enabled = true;
            }
            else
            {
                defaultMenuItem.Enabled = false;
                targetMenuItem.Enabled = false;
                defaultMenuItem.Visible = false;
                targetMenuItem.Visible = false;
            }

            if (RequestProperties != null && RequestProperties.Count() > 0)
            {
                foreach (IMappableProperty requestProperty in RequestProperties.OrderBy(property => property.Name))
                {
                    item = requestMenuItem.DropDownItems.Add(string.Format("{0}{1}", requestProperty.Name, requestProperty.Type != null ? string.Format(" ({0})", requestProperty.Type.Name) : string.Empty));
                    item.Click += new EventHandler(RequestPropertyClickEventHandler);
                    item.Tag = requestProperty;
                    item.Enabled = this.IsEditable;
                }

                requestMenuItem.Enabled = true;
            }
            else
            {
                requestMenuItem.Visible = false;
                requestMenuItem.Enabled = false;
            }

            if (!requestMenuItem.Enabled && !targetMenuItem.Enabled && !defaultMenuItem.Enabled)
            {
                mapContextMenu.Enabled = false;
            }
            else
            {
                mapContextMenu.Enabled = true;
            }

            propertyListView.Items.Clear();

            if (PropertyMap != null)
            {
                // Write the propertylist from the propertymap
                RecreatePropertyListFromPropertyMap();
            }
            else
            {
                PropertyMap = new PropertyMap();

                foreach (IMappableProperty sourceProperty in SourceProperties)
                {
                    MapProperty(null, sourceProperty);
                }
            }

            RefreshView();

            // Select first in list if any exist
            if (propertyListView.Items.Count > 0)
            {
                propertyListView.Items[0].Selected = true;
            }
        }

        private void RecreatePropertyListFromPropertyMap()
        {
            propertyListView.Items.Clear();

            foreach (MappedProperty property in PropertyMap.MappedProperties)
            {
                MapProperty(property, property.Source);
            }
        }

        public void ShowSynchronizationResult()
        {
            // Show result for synchronize action
            if (SyncAddedProperties.Count > 0 || 
                SyncDeletedProperties.Count > 0 ||
                SyncChangedProperties.Count > 0)
            {
                string message = "Synchronization of the map has been done!\n";

                if (SyncAddedProperties.Count > 0)
                {
                    message += "\nNew fields (shown in blue in the map):\n";

                    foreach (MappedProperty property in SyncAddedProperties)
                    {
                        message += string.Format("\t{0}\n", property.Source.Name);
                    }
                }

                if (SyncChangedProperties.Count > 0)
                {
                    message += "\nChanged fields (shown in orange in the map):\n";

                    foreach (MappedProperty property in SyncChangedProperties)
                    {
                        message += string.Format("\t{0}\n", property.Source.Name);
                    }
                }

                if (SyncDeletedProperties.Count > 0)
                {
                    message += "\nDeleted fields:\n";

                    foreach (MappedProperty property in SyncDeletedProperties)
                    {
                        message += string.Format("\t{0}\n", property.Source.Name);
                    }
                }

                MessageBox.Show(message, "Synchronization Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (SyncDeletedProperties.Count > 0)
                {
                    MessageBox.Show("Since there was deleted fields you might get problems when trying to\n" +
                                    "save the changes for the map, when you push the OK button leaving this dialog!\n\n" +
                                    "If the deleted fields has been mapped to another object in turn it will\n" +
                                    "result in a foreign key constraint when saving the map!\n\n" +
                                    "This is a known problem but it hasn't been solved yet!",
                                    "Warning! Please note!",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Synchronization of the map has been done and no changes were made!", "Synchronization Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool SynchronizePropertyMap()
        {
            bool somethingAddedChangedOrRemoved = false;

            // Reset the Synchronization results.
            SyncAddedProperties.Clear();
            SyncDeletedProperties.Clear();
            SyncChangedProperties.Clear();

            foreach (MappedProperty fromProperty in SourceProperties)
            {
                var toMapProperties = (from MappedProperty p in PropertyMap.MappedProperties
                                       where (p.Source != null &&
                                              p.Source.Id == fromProperty.Id)
                                       select p);

                MappedProperty mappedProperty = null;

                if (toMapProperties.Count() > 0)
                {
                    // Found the property
                    mappedProperty = toMapProperties.ElementAt(0);

                    // Check if target and mappedProperty is Property and that they differ.
                    // In that case we update the mappedProperty.
                    if (fromProperty.Target is Property &&
                        mappedProperty.Target != null &&
                        fromProperty.Target != null &&
                        mappedProperty.Target is Property &&
                        mappedProperty.Target.Id != fromProperty.Target.Id)
                    {
                        mappedProperty.Target = fromProperty.Target;
                        if (!SyncChangedProperties.Contains(mappedProperty))
                            SyncChangedProperties.Add(mappedProperty);
                        somethingAddedChangedOrRemoved = true;
                    }

                    // Check if name has changed.
                    if (mappedProperty.Target != null &&
                        fromProperty.Target is Property &&
                        fromProperty != null &&
                        mappedProperty != null &&
                        mappedProperty.Target is Property &&
                        fromProperty.Name != mappedProperty.Name)
                    {
                        mappedProperty.Name = fromProperty.Name;
                        if (!SyncChangedProperties.Contains(mappedProperty))
                            SyncChangedProperties.Add(mappedProperty);
                        somethingAddedChangedOrRemoved = true;
                    }
                }
                else
                {
                    mappedProperty = new MappedProperty();
                    PropertyMap.MappedProperties.Add(mappedProperty);
                    mappedProperty.PropertyMap = PropertyMap;
                    mappedProperty.Source = fromProperty;
                    somethingAddedChangedOrRemoved = true;

                    mappedProperty.Sequence = fromProperty.Sequence;
                    mappedProperty.Name = fromProperty.Name;
                                        
                    SyncAddedProperties.Add(mappedProperty);
                }
            }

            foreach (MappedProperty toProperty in PropertyMap.MappedProperties.ToArray())
            {
                bool existsInFromMap = false;

                foreach (MappedProperty fromProperty in SourceProperties)
                {
                    if (toProperty.Source != null &&
                        toProperty.Source.Id == fromProperty.Id)
                        existsInFromMap = true;
                }

                if ((!existsInFromMap) && (!toProperty.IsCustom))
                {
                    SyncDeletedProperties.Add(toProperty);
                    PropertyMap.MappedProperties.Remove(toProperty);
                    somethingAddedChangedOrRemoved = true;
                }
            }

            // Rewrite the total list if something has changed
            if (somethingAddedChangedOrRemoved)
            {
                RecreatePropertyListFromPropertyMap();
                RefreshView();
            }

            return somethingAddedChangedOrRemoved;
        }

        private void MapProperty(MappedProperty mappedProperty, IMappableProperty sourceProperty)
        {
            bool newMappedProperty = false;

            if (mappedProperty == null)
            {
                mappedProperty = new MappedProperty();
                mappedProperty.Source = sourceProperty;
                mappedProperty.Sequence = PropertyMap.MappedProperties.Count + 1;
                mappedProperty.PropertyMap = PropertyMap;
                PropertyMap.MappedProperties.Add(mappedProperty);
                newMappedProperty = true;
            }

            if(newMappedProperty)
            {   
                mappedProperty.IsEnabled = EnablePropertiesByDefault;
            }

            ListViewItem item = CreateListItem(mappedProperty);

            UpdateListItem(item);
        }

        private ListViewItem CreateListItem(MappedProperty mappedProperty)
        {
            ListViewItem item = propertyListView.Items.Add(mappedProperty.Sequence.ToString());

            if (mappedProperty.Source != null)
            {
                item.SubItems.Add(mappedProperty.Source.Name);
                item.SubItems.Add(mappedProperty.Source.Type != null ? mappedProperty.Source.Type.Name : string.Empty);
            }
            else
            {
                item.SubItems.Add("");
                item.SubItems.Add("");
            }

            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.SubItems.Add("");

            item.Tag = mappedProperty;
            return item;
        }

        private IMappableProperty FindTarget(IMappableProperty source)
        {
            foreach (IMappableProperty targetProperty in TargetProperties)
            {
                if (targetProperty is UXSessionProperty)
                    continue;

                if ((targetProperty.Name == source.Name) && (targetProperty.Type == source.Type))
                    return targetProperty;
            }

            return null;
        }

        private void mapContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 0 ||
                propertyListView.SelectedItems.Count > 1)
                e.Cancel = true;
        }

        void DefaultSessionPropertyClickEventHandler(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1)
            {
                MappedProperty property = propertyListView.SelectedItems[0].Tag as MappedProperty;

                property.DefaultSessionProperty = (sender as ToolStripMenuItem).Tag as UXSessionProperty;

                UpdateListItem(propertyListView.SelectedItems[0]);
            }
        }

        void RequestPropertyClickEventHandler(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1)
            {
                MappedProperty property = propertyListView.SelectedItems[0].Tag as MappedProperty;

                if (sender != null)
                {
                    property.RequestMappedProperty = (sender as ToolStripMenuItem).Tag as MappedProperty;
                }
                else
                {
                    property.RequestMappedProperty = null;
                }

                UpdateListItem(propertyListView.SelectedItems[0]);
            }
        }

        void SourcePropertyClickEventHandler(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1)
            {
                MappedProperty property = propertyListView.SelectedItems[0].Tag as MappedProperty;

                property.Source = (sender as ToolStripMenuItem).Tag as IMappableProperty;

                UpdateListItem(propertyListView.SelectedItems[0]);
            }
        }

        void TargetPropertyClickEventHandler(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1)
            {
                MappedProperty property = propertyListView.SelectedItems[0].Tag as MappedProperty;

                IMappableProperty target = (sender as ToolStripMenuItem).Tag as IMappableProperty;

                if (target != null)
                {
                    property.Target = target;
                    property.IsEnabled = true;
                }
                else
                {
                    property.Target = null;
                }

                UpdateListItem(propertyListView.SelectedItems[0]);
            }
        }

        private void UpdateListItem(ListViewItem item)
        {
            MappedProperty property = item.Tag as MappedProperty;

            if (property.Source != null)
            {
                item.SubItems[1].Text = string.Format("{0}", property.Source.Name);
                item.SubItems[2].Text = property.Source.Type != null ? property.Source.Type.Name : string.Empty;
            }
            else if (property.IsCustom)
            {
                item.SubItems[1].Text = "[Custom]";
                item.SubItems[2].Text = string.Empty;
            }

            if (property.RequestMappedProperty != null)
            {
                item.SubItems[1].Text = string.Format("[Request] {0}", property.RequestMappedProperty.Name);
            }

            if (property.Target != null)
            {
                item.SubItems[3].Text = property.Name;
                item.SubItems[4].Text = property.Type != null ? property.Type.Name : string.Empty;
            }
            else
            {
                item.SubItems[3].Text = string.Empty;
                item.SubItems[4].Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(property.DefaultValue))
                item.SubItems[5].Text = property.DefaultValue;
            else if (property.DefaultSessionProperty != null)
                item.SubItems[5].Text = string.Format("[{0}]", property.DefaultSessionProperty.Name);
            else
                item.SubItems[5].Text = string.Empty;

            if (property.Target != null)
            {
                if (property.Target is Property)
                {
                    PropertyStorageInfo info = ((Property)property.Target).StorageInfo;

                    if (info != null &&
                        !string.IsNullOrEmpty(info.TableName) &&
                        !string.IsNullOrEmpty(info.ColumnName))
                    {
                        item.SubItems[6].Text = string.Format("{0}.{1}", info.TableName, info.ColumnName);
                    }
                    else
                    {
                        item.SubItems[6].Text = string.Empty;
                    }
                }
                else
                {
                }
            }
            else
            {
                item.SubItems[6].Text = string.Empty;
            }

            if (property.IsEnabled)
                item.ForeColor = Color.Black;
            else
                item.ForeColor = Color.Silver;

            if (item.Selected)
                sourceGrid.SelectedObject = property;
        }

        public void RefreshView()
        {
            IEnumerable<ListViewItem> sortedItems =
                from ListViewItem i in propertyListView.Items
                orderby ((i.Tag) as MappedProperty).Sequence
                select i;

            foreach (ListViewItem vi in sortedItems)
            {
                vi.SubItems[0].Text = (vi.Tag as MappedProperty).Sequence.ToString();

                // Check if this is an added Mapped Property, in that case we make it blue color
                // so we can see it has been added.
                if (SyncAddedProperties.Contains(vi.Tag as MappedProperty))
                {
                    vi.ForeColor = Color.Blue;
                }
                // Check if this is a changed Mapped Property, in that case we make it orange color
                // so we can see it has been changed.
                else if (SyncChangedProperties.Contains(vi.Tag as MappedProperty))
                {
                    vi.ForeColor = Color.Orange;
                }
            }

            ListViewItem[] sortedArray = sortedItems.ToArray();
            propertyListView.Items.Clear();
            propertyListView.Items.AddRange(sortedArray);
        }

        private void sourceGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MappedProperty property = sourceGrid.SelectedObject as MappedProperty;

            if (e.ChangedItem.PropertyDescriptor.Name == "DefaultValue")
            {
                if (!string.IsNullOrEmpty(property.DefaultValue))
                    property.IsEnabled = true;
            }

            if (e.ChangedItem.PropertyDescriptor.Name == "Name")
            {
                if (e.ChangedItem.Value != null)
                {
                    if (!NamingGuidance.CheckMappedPropertyName(e.ChangedItem.Value.ToString(), true))
                    {
                        property.Name = (e.OldValue == null ? null : e.OldValue.ToString());
                    }
                }
            }

            if (!MultiSelMode)
            {
                foreach (ListViewItem item in propertyListView.Items)
                    UpdateListItem(item);
            }
            else
            {
                // Save all changes so we can save all later
                ChangedMultiSelProperties[e.ChangedItem.PropertyDescriptor.Name] = e.ChangedItem.Value;
            }
        }

        private void propertyListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1)
            {
                MappedProperty property = propertyListView.SelectedItems[0].Tag as MappedProperty;

                if (property != null)
                {
                    if (!this.IsEditable || LockPropertyGrid)
                    {
                        TypeDescriptor.AddAttributes(property, new Attribute[] { new ReadOnlyAttribute(true) });
                    }

                    sourceGrid.SelectedObject = property;
                }

                SetMultiSelectMode(false);
            }
            else if (propertyListView.SelectedItems.Count > 1)
            {
                NewMultiSelItem();

                SetMultiSelectMode(true);
            }
            else
            {
                sourceGrid.SelectedObject = null;

                SetMultiSelectMode(false);
            }
        }

        private void NewMultiSelItem()
        {
            MappedProperty multiProp = new MappedProperty();

            if (!this.IsEditable || LockPropertyGrid)
            {
                TypeDescriptor.AddAttributes(multiProp, new Attribute[] { new ReadOnlyAttribute(true) });
            }

            sourceGrid.SelectedObject = multiProp;
        }

        private void SetMultiSelectMode(bool isMultiSelect)
        {
            MultiSelMode = isMultiSelect;

            if (MultiSelMode)
            {
                pMulti.Height = 35;
                btnSaveMulti.Visible = true;
                btnMultiClearChangedList.Visible = true;
                btnMultiSetToChanged.Visible = true;
                ChangedMultiSelProperties.Clear();
            }
            else
            {
                btnSaveMulti.Visible = false;
                btnMultiClearChangedList.Visible = false;
                btnMultiSetToChanged.Visible = false;
                pMulti.Height = 14;
                ChangedMultiSelProperties.Clear();
            }
        }

        private void propertyListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            MappedProperty property = null;

            if (!MultiSelMode)
            {
                if (propertyListView.SelectedItems.Count == 1)
                {
                    property = propertyListView.SelectedItems[0].Tag as MappedProperty;
                }

                if (SelectedPropertyChanged != null)
                    SelectedPropertyChanged(this, new PropertyChangedEventArgs() { Property = property });
            }
        }

        private void sourceGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            foreach (Control cntrl in sourceGrid.Controls[2].Controls)
            {
                cntrl.KeyDown -= new KeyEventHandler(SourceGrid_KeyDown);
            }

            lblInformation.Text = string.Empty;

            if (e.NewSelection != null &&
                e.NewSelection.Label == "DisplayFormat")
            {
                MappedProperty mappedProperty = (MappedProperty)sourceGrid.SelectedObject;

                // Check if the type is one of DateTime or Numeric (decimal, double or int)
                if (mappedProperty != null &&
                    mappedProperty.CanChangeDisplayFormat())
                {
                    lblInformation.Text = "Push the \"Insert\" button in the inputfield for help on different used DisplayFormats.";

                    foreach (Control cntrl in sourceGrid.Controls[2].Controls)
                    {
                        cntrl.KeyDown += new KeyEventHandler(SourceGrid_KeyDown);
                    }
                }
            }
        }

        private void btnSaveMulti_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 1 &&
                ChangedMultiSelProperties.Count > 0)
            {
                string showMessage = string.Format("Updating {0} selected item(s) with the following:\n\n", propertyListView.SelectedItems.Count);

                foreach(KeyValuePair<string, object> keyVal in ChangedMultiSelProperties)
                {
                    showMessage += string.Format("{0} = {1}\n", keyVal.Key, keyVal.Value ?? "[null]");
                }

                showMessage += "\nDo you want to write these changes to the selected item(s)?";

                if (MessageBox.Show(showMessage, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    foreach (ListViewItem item in propertyListView.SelectedItems)
                    {
                        MappedProperty property = item.Tag as MappedProperty;

                        foreach (KeyValuePair<string, object> keyVal in ChangedMultiSelProperties)
                        {
                            // Check if we are changing DisplayFormat if we should update it
                            if (keyVal.Key == "DisplayFormat")
                            {
                                if (!property.CanChangeDisplayFormat())
                                    continue;
                            }

                            // Set the value with reflection
                            property.GetType().GetProperty(keyVal.Key).SetValue(property, keyVal.Value, null);
                        }

                        UpdateListItem(item);
                    }
                }
            }
        }

        private void btnMultiClearChangedList_Click(object sender, EventArgs e)
        {
            NewMultiSelItem();
            ChangedMultiSelProperties.Clear();
        }

        private void btnMultiSetToChanged_Click(object sender, EventArgs e)
        {
            if (sourceGrid.SelectedGridItem != null)
            {
                // Get the value from the sourceGrid
                ChangedMultiSelProperties[sourceGrid.SelectedGridItem.Label] = sourceGrid.SelectedGridItem.Value;
            }
        }

        public void AutoMapProperties()
        {   
            foreach (ListViewItem item in propertyListView.Items)
            {
                MappedProperty property = item.Tag as MappedProperty;
                if (property != null)
                {
                    IMappableProperty target = FindTarget(property.Source);
                    bool hasTarget = target != null;
                    
                    if (property.IsEnabled && !property.IsMapped && hasTarget)
                    {
                        property.Target = target;
                        UpdateListItem(item);
                    }
                }
            }
            RefreshView();
        }

    }

    public class PropertyChangedEventArgs : EventArgs
    {
        public MappedProperty Property { get; set; }
    }

}
