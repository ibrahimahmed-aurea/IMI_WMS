using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Context;
using Spring.Context.Support;
using Cdc.MetaManager.BusinessLogic;


namespace Cdc.MetaManager.GUI
{
    public partial class PropertyMapForm2 : MdiChildForm
    {
        public PropertyMapForm2()
        {
            InitializeComponent();
            EnablePropertiesByDefault = true;
        }

        public List<MappedProperty> AddedMappedProperties { get; set; }
        public List<MappedProperty> DeletedMappedProperties { get; set; }

        public bool UseProvidedMapObject { get; set; }

        public bool HideSyncronizeButton { get; set; }
        

        public ViewNode ViewNode { get; set; }

        public PropertyMap PropertyMap
        {
            get
            {
                return propertyMapControl.PropertyMap;
            }
            set
            {
                if (value != null)
                {
                    if (value.Id == Guid.Empty || UseProvidedMapObject)
                    {
                        propertyMapControl.PropertyMap = value;
                    }
                    else
                    {
                        propertyMapControl.PropertyMap = MetaManagerServices.GetModelService().GetInitializedDomainObject<PropertyMap>(value.Id);
                    }
                }
                else
                {
                    propertyMapControl.PropertyMap = null;
                }
            }
        }

        public IEnumerable<IMappableProperty> SourceProperties { get; set; }
        public IEnumerable<IMappableProperty> TargetProperties { get; set; }
        public IEnumerable<IMappableProperty> RequestProperties { get; set; }
        public bool AllowNonUniquePropertyNames { get; set; }
        public bool AllowAddProperty { get; set; }
        public bool EnablePropertiesByDefault { get; set; }
        public bool LockPropertyGrid { get; set; }


        private void PropertyMapForm2_Load(object sender, EventArgs e)
        {
            if (AddedMappedProperties == null) { AddedMappedProperties = new List<MappedProperty>(); }
            if (DeletedMappedProperties == null) { DeletedMappedProperties = new List<MappedProperty>(); }

            propertyMapControl.SourceProperties = this.SourceProperties;
            propertyMapControl.TargetProperties = this.TargetProperties;
            propertyMapControl.RequestProperties = this.RequestProperties;
            propertyMapControl.PropertyMap = this.PropertyMap;
            propertyMapControl.CheckTargetNameProc = NamingGuidance.CheckMappedPropertyName;
            propertyMapControl.AllowNonUniquePropertyNames = AllowNonUniquePropertyNames;
            propertyMapControl.EnablePropertiesByDefault = this.EnablePropertiesByDefault;
            propertyMapControl.IsEditable = this.IsEditable;
            propertyMapControl.CheckTargetNameProc = NamingGuidance.CheckMappedPropertyName;
            propertyMapControl.AllowNonUniquePropertyNames = AllowNonUniquePropertyNames;
            propertyMapControl.LockPropertyGrid = LockPropertyGrid;

            propertyMapControl.Map();
            propertyMapControl.SelectedPropertyChanged += new EventHandler<PropertyChangedEventArgs>(propertyMapControl_SelectedPropertyChanged);

            removePropertyBtn.Enabled = false;

            if (!AllowAddProperty)
            {
                addPropertyBtn.Enabled = false;
            }
            else
            {
                addPropertyBtn.Enabled = this.IsEditable;
            }

            if (HideSyncronizeButton)
            {
                syncBtn.Visible = false;
                syncBtn.Enabled = false;
            }
            else
            {
                syncBtn.Enabled = this.IsEditable;
            }

            okBtn.Enabled = this.IsEditable;
            autoMaptPropertiesBtn.Enabled = this.IsEditable;
        }

        void propertyMapControl_SelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.Property != null)
            {
                if (e.Property.IsCustom)
                    removePropertyBtn.Enabled = this.IsEditable;
                else
                    removePropertyBtn.Enabled = false;
            }
            else
                removePropertyBtn.Enabled = false;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            try
            {
                propertyMapControl.ValidateMapping();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            propertyMapControl.RefreshView();
        }

        private void addPropertyBtn_Click(object sender, EventArgs e)
        {
            using (FindPropertyForm form = new FindPropertyForm())
            {
                form.CanShowCustomProperties = true;
                form.CanMultiSelectProperties = true;
                form.FrontendApplication = FrontendApplication;
                form.BackendApplication = BackendApplication;
                form.Owner = this;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    foreach (Property property in form.SelectedPropertyList)
                    {
                        MappedProperty newProperty = new MappedProperty();

                        int sequence = 0;

                        if (propertyMapControl.PropertyMap.MappedProperties.Count > 0)
                            sequence = propertyMapControl.PropertyMap.MappedProperties.Max(p => p.Sequence) + 1;

                        newProperty.IsCustom = true;
                        newProperty.Target = property;
                        newProperty.PropertyMap = propertyMapControl.PropertyMap;
                        newProperty.Sequence = sequence;

                        propertyMapControl.PropertyMap.MappedProperties.Add(newProperty);

                        AddedMappedProperties.Add(newProperty);
                    }
                }

                propertyMapControl.Map();
            }
        }

        private void removePropertyBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove the selected Property?", "Remove Property", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                propertyMapControl.PropertyMap.MappedProperties.Remove(propertyMapControl.SelectedProperty);

                if (AddedMappedProperties.Contains(propertyMapControl.SelectedProperty))
                {
                    AddedMappedProperties.Remove(propertyMapControl.SelectedProperty);
                }
                else
                {
                    DeletedMappedProperties.Add(propertyMapControl.SelectedProperty);
                }

                propertyMapControl.Map();
            }
        }

        private void syncBtn_Click(object sender, EventArgs e)
        {
            propertyMapControl.SynchronizePropertyMap();
            propertyMapControl.ShowSynchronizationResult();
        }

        private void autoMaptPropertiesBtn_Click(object sender, EventArgs e)
        {
            propertyMapControl.AutoMapProperties();
        }
    }
}
