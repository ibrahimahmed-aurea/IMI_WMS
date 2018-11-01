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
    public partial class CreatePropertyMap : MdiChildForm
    {
        public PropertyMap PropertyMap { get; set; }
        public bool IsRequestMap { get; set; }
        public bool DoSaveMapWhenExit { get; set; }
        public int MaxPropertyLength { get; set; }
        public bool ChangesMade { get; private set; }
        public bool UseColumnNames { private get; set; }
        public string NameSuffix { private get; set; }
        public bool NameUpperCase { private get; set; }

        private IList<MappedProperty> deletedList { get; set; }
        private IDialogService dialogService = null;

        public CreatePropertyMap()
        {
            InitializeComponent();

            deletedList = new List<MappedProperty>();
            dialogService = MetaManagerServices.GetDialogService();

            DoSaveMapWhenExit = true;
            MaxPropertyLength = 0;
            ChangesMade = false;
            UseColumnNames = false;
            NameSuffix = string.Empty;
        }

        private void CreatePropertyMap_Load(object sender, EventArgs e)
        {
            if (IsRequestMap)
                this.Text += " (inparameters, request)";
            else
                this.Text += " (outparameters, respons)";

            ShowProperties();
        }

        private void ShowProperties()
        {
            propertyListView.Items.Clear();

            foreach (MappedProperty mappedProperty in PropertyMap.MappedProperties.OrderBy(p => p.Sequence))
            {
                AddPropertyToListView(mappedProperty);
            }

            propertyGrid.SelectedObject = null;

            EnableDisableButtons();
        }

        private void AddPropertyToListView(MappedProperty mappedProperty)
        {
            ListViewItem item = propertyListView.Items.Add(mappedProperty.Sequence.ToString());
            item.SubItems.Add(mappedProperty.Name);
            item.SubItems.Add(mappedProperty.Target.Name);
            item.SubItems.Add(mappedProperty.Target.Type.ToString());

            if (mappedProperty.Target is Property &&
                mappedProperty.TargetProperty.StorageInfo != null)
            {
                item.SubItems.Add(string.Format("{0}.{1}", 
                                                mappedProperty.TargetProperty.StorageInfo.TableName,
                                                mappedProperty.TargetProperty.StorageInfo.ColumnName));
            }
            else
            {
                item.SubItems.Add(string.Empty);
            }

            item.Tag = mappedProperty;
        }

        private void propertyListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EnableDisableButtons();

            if (propertyListView.SelectedItems.Count > 0)
            {
                MappedProperty property = propertyListView.SelectedItems[0].Tag as MappedProperty;

                if (property != null)
                {
                    propertyGrid.SelectedObject = property;
                }
            }
            else
            {
                propertyGrid.SelectedObject = null;
            }
        }

        private void EnableDisableButtons()
        {
            if (IsEditable)
            {
                tsbDelete.Enabled = false;
                tsbChangeName.Enabled = false;
                tsbMoveUp.Enabled = false;
                tsbMoveDown.Enabled = false;

                if (propertyListView.SelectedItems.Count > 0)
                {
                    tsbDelete.Enabled = true;

                    if (propertyListView.SelectedItems.Count == 1)
                    {
                        tsbChangeName.Enabled = true;

                        if (propertyListView.Items.Count > 1)
                        {
                            if (propertyListView.SelectedItems[0].Index != 0)
                            {
                                tsbMoveUp.Enabled = true;
                            }

                            if (propertyListView.SelectedItems[0].Index < (propertyListView.Items.Count - 1))
                            {
                                tsbMoveDown.Enabled = true;
                            }
                        }
                    }
                }
            }
            else
            {
                tsbAdd.Enabled = false;
                tsbDelete.Enabled = false;
                tsbMoveDown.Enabled = false;
                tsbMoveUp.Enabled = false;
                tsbChangeName.Enabled = false;

                btnOK.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // First check if there are any MappedProperties with the same name.
            var props = (from prop in PropertyMap.MappedProperties
                         select prop.Name).Distinct();

            if (props.Count() != PropertyMap.MappedProperties.Count)
            {
                MessageBox.Show("One or more MappedProperties has the same name. Names must be unique.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if there are any properties that is passed the maximum length
            if (MaxPropertyLength > 0)
            {
                // Get all properties that has a name length that is over the max property length
                IList<string> list = PropertyMap.MappedProperties.Select(p => p.Name).Where(name => name.Length > MaxPropertyLength).ToList();

                if (list.Count > 0)
                {
                    string txtList = list.Aggregate((current, next) => current + ",\n\t" + next);

                    MessageBox.Show("You need to cut down the following field(s) to a maximum of " + MaxPropertyLength.ToString() + " characters!\n" +
                                    "Fields that are too long:\n\t" + txtList, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            if (DoSaveMapWhenExit)
            {
                // Save all MappedProperties
                try
                {
                    dialogService.SaveAndDeleteMappedPropertiesInMap(PropertyMap, deletedList);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public void AddPropertiesToPropertyMap(List<Property> properties)
        {
            bool tooLongPropertiesAdded = false;
            int sequence = 1;

            if (PropertyMap.MappedProperties.Count > 0)
                sequence = PropertyMap.MappedProperties.Max(p => p.Sequence) + 1;

            foreach (Property property in properties)
            {
                MappedProperty mappedProperty = new MappedProperty();

                mappedProperty.Source = null;
                mappedProperty.Target = property;

                string name = UseColumnNames && property.StorageInfo != null ? property.StorageInfo.ColumnName : property.Name;

                if (NameUpperCase)
                {
                    name = name.ToUpper();
                }

                // Check lengths
                if (MaxPropertyLength > 0 &&
                    (name.Length + NameSuffix.Length) > MaxPropertyLength)
                {
                    tooLongPropertiesAdded = true;

                    mappedProperty.Name = name.Substring(0, MaxPropertyLength - NameSuffix.Length) + NameSuffix;
                }
                else
                {
                    mappedProperty.Name = name + NameSuffix;
                }

                mappedProperty.Sequence = sequence++;
                mappedProperty.PropertyMap = PropertyMap;
                mappedProperty.IsSearchable = true;

                // If storageinfo is not set then it's custom
                mappedProperty.IsCustom = property.StorageInfo == null; 

                PropertyMap.MappedProperties.Add(mappedProperty);
            }

            if (tooLongPropertiesAdded)
            {
                MessageBox.Show("One or more properties was added that was longer than " + MaxPropertyLength.ToString() + " characters.\n" +
                                "The names of these properties has been cut to maximum length.\n" +
                                "Check the names of the properties if they are usable before saving.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            using (FindPropertyForm findProperty = new FindPropertyForm())
            {
                findProperty.CanMultiSelectProperties = true;
                findProperty.CanShowCustomProperties = false;
                findProperty.CanCreateProperties = true;
                findProperty.FrontendApplication = FrontendApplication;
                findProperty.BackendApplication = BackendApplication;

                if (findProperty.ShowDialog() == DialogResult.OK)
                {
                    AddPropertiesToPropertyMap(findProperty.SelectedPropertyList);

                    ShowProperties();
                    ChangesMade = true;
                }
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in propertyListView.SelectedItems)
                {
                    MappedProperty mp = item.Tag as MappedProperty;

                    if (mp != null)
                    {
                        // Check if has real Id then it should be deleted in database also.
                        if (mp.Id != Guid.Empty)
                        {
                            deletedList.Add(mp);
                        }

                        // Remove item in list
                        PropertyMap.MappedProperties.Remove(mp);

                        // Recalculate sequence
                        ResetSequenceOnPropertyMap();

                        ShowProperties();
                        ChangesMade = true;
                    }
                }
            }
        }

        public void ResetSequenceOnPropertyMap()
        {
            // Order mapped properties in ascending order 
            var varPropList = from prop in PropertyMap.MappedProperties
                              orderby prop.Sequence
                              select prop;

            IList<MappedProperty> propList = varPropList.ToList<MappedProperty>().Count > 0 ? varPropList.ToList<MappedProperty>() : null;

            if (propList != null)
            {
                int sequence = 1;

                foreach (MappedProperty mp in propList)
                {
                    if (sequence != mp.Sequence)
                    {
                        mp.Sequence = sequence;
                    }
                    sequence++;
                }
            }
        }

        private void tsbChangeName_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1)
            {
                MappedProperty mappedProperty = propertyListView.SelectedItems[0].Tag as MappedProperty;

                using (RenamePropertyForm form = new RenamePropertyForm())
                {
                    form.MaxPropertyLength = MaxPropertyLength;
                    form.NameSuffix = NameSuffix;
                    form.NameUpperCase = NameUpperCase;

                    if (string.IsNullOrEmpty(mappedProperty.Name))
                        form.OldName = mappedProperty.Target.Name;
                    else
                        form.OldName = mappedProperty.Name;

                    form.FormCaption = "Rename Mapped Property";

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        mappedProperty.Name = form.NewName;

                        // Update text on selected row.
                        propertyListView.SelectedItems[0].SubItems[1].Text = mappedProperty.Name;

                        propertyGrid.Refresh();
                        ChangesMade = true;
                    }
                }
            }

        }

        private void tsbMoveUp_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1 && 
                propertyListView.SelectedItems[0].Index > 0)
            {
                int selectedIndex = propertyListView.SelectedItems[0].Index;
                int exchangeIndex = selectedIndex - 1;

                // Change sequence with the neighbor above
                int tempSequence = ((MappedProperty)propertyListView.Items[exchangeIndex].Tag).Sequence;
                ((MappedProperty)propertyListView.Items[exchangeIndex].Tag).Sequence = ((MappedProperty)propertyListView.Items[selectedIndex].Tag).Sequence;
                ((MappedProperty)propertyListView.Items[selectedIndex].Tag).Sequence = tempSequence;

                // Set the sequences visibly in Listview
                propertyListView.Items[selectedIndex].Text = ((MappedProperty)propertyListView.Items[selectedIndex].Tag).Sequence.ToString();
                propertyListView.Items[exchangeIndex].Text = ((MappedProperty)propertyListView.Items[exchangeIndex].Tag).Sequence.ToString();

                // Get the selected row in the listview
                ListViewItem selectedItem = propertyListView.Items[selectedIndex];

                // Remove it
                propertyListView.Items.Remove(selectedItem);

                // Insert at the exchangeindex
                propertyListView.Items.Insert(exchangeIndex, selectedItem);

                EnableDisableButtons();
                ChangesMade = true;
            }
        }

        private void tsbMoveDown_Click(object sender, EventArgs e)
        {
            if (propertyListView.SelectedItems.Count == 1 &&
                propertyListView.SelectedItems[0].Index < (propertyListView.Items.Count - 1))
            {
                int selectedIndex = propertyListView.SelectedItems[0].Index;
                int exchangeIndex = selectedIndex + 1;

                // Change sequence with the neighbor below
                int tempSequence = ((MappedProperty)propertyListView.Items[exchangeIndex].Tag).Sequence;
                ((MappedProperty)propertyListView.Items[exchangeIndex].Tag).Sequence = ((MappedProperty)propertyListView.Items[selectedIndex].Tag).Sequence;
                ((MappedProperty)propertyListView.Items[selectedIndex].Tag).Sequence = tempSequence;

                // Set the sequences visibly in Listview
                propertyListView.Items[selectedIndex].Text = ((MappedProperty)propertyListView.Items[selectedIndex].Tag).Sequence.ToString();
                propertyListView.Items[exchangeIndex].Text = ((MappedProperty)propertyListView.Items[exchangeIndex].Tag).Sequence.ToString();

                // Get the selected row in the listview
                ListViewItem selectedItem = propertyListView.Items[selectedIndex];

                // Remove it
                propertyListView.Items.Remove(selectedItem);

                // Insert at the exchangeindex
                propertyListView.Items.Insert(exchangeIndex, selectedItem);

                EnableDisableButtons();
                ChangesMade = true;
            }
        }

    }
}
