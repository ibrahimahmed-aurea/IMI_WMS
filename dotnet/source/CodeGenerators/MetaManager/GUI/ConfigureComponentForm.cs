using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic.Helpers;



namespace Cdc.MetaManager.GUI
{
    
    public partial class ConfigureComponentForm : MdiChildForm
    {
        public class VisualComponentItem
        {
            public Type Type { get; set; }
            public string Name { get; set; }
            public UXComponent Instance { get; set; }
        }

        private UXComponent uxComponent;
        private IModelService modelService;
        private ServiceMethod serviceMethod;
        private PropertyMap componentMap;
        
        public UXComponent UXComponent 
        {
            get
            {
                return uxComponent;
            }
            set
            {
                if (value is UXServiceComponent)
                {
                    UXServiceComponent serviceComponent = value as UXServiceComponent;

                    if (serviceComponent.ServiceMethod != null)
                    {
                        serviceMethod = serviceComponent.ServiceMethod;
                    }

                    if (serviceComponent.ComponentMap != null)
                    {
                        componentMap = serviceComponent.ComponentMap;
                    }
                }

                uxComponent = value;
            }
        }

        public List<string> ComponentNames { get; set; }
        public bool OnlyBindables { private get; set; }

        public ConfigureComponentForm()
        {
            InitializeComponent();
            serviceMethod = null;
            componentMap = null;
            modelService = MetaManagerServices.GetModelService();
        }

        private void ConfigureComponentForm_Load(object sender, EventArgs e)
        {
            PopulateComponentTypes();
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            serviceBtn.Enabled = false;
            okBtn.Enabled = false;
            btnEditRadioGroup.Enabled = false;

            if (componentCbx.SelectedItem != null)
            {
                okBtn.Enabled = this.IsEditable;

                if (propertyGrid.SelectedObject is UXServiceComponent)
                    serviceBtn.Enabled = UXComponent is UXServiceComponent && this.IsEditable;

                if (propertyGrid.SelectedObject is UXRadioGroup)
                    btnEditRadioGroup.Enabled = true;
            }
        }

        private void PopulateComponentTypes()
        {
            componentCbx.Items.Clear();
            componentCbx.DisplayMember = "Name";

            Assembly assembly = Assembly.GetAssembly(typeof(VisualDesignerAttribute));

            VisualComponentItem selectedItem = null;

            foreach (Type t in assembly.GetTypes())
            {
                if (!OnlyBindables || (OnlyBindables && (t.GetInterface("IBindable") != null)))
                {
                    object[] attributes = t.GetCustomAttributes(typeof(VisualDesignerAttribute), false);

                    if (attributes.Count() > 0)
                    {
                        VisualComponentItem item = new VisualComponentItem();
                        item.Type = t;
                        item.Name = ((VisualDesignerAttribute)attributes[0]).ComponentName;

                        if ((UXComponent != null) && (UXComponent.GetType() == t))
                        {
                            item.Instance = UXComponent;
                            selectedItem = item;
                        }
                        
                        componentCbx.Items.Add(item);
                    }
                }
            }

            componentCbx.SelectedItem = selectedItem;
        }

        private void serviceBtn_Click(object sender, EventArgs e)
        {
            VisualComponentItem item = componentCbx.SelectedItem as VisualComponentItem;

            using (ConfigureServiceForm form = new ConfigureServiceForm())
            {
                form.BackendApplication = BackendApplication;
                form.Owner = this;
                form.UXServiceComponent = item.Instance as UXServiceComponent;

                form.ShowDialog();
            }

            propertyGrid.SelectedObject = item.Instance;
        }

        private void componentCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisualComponentItem item = componentCbx.SelectedItem as VisualComponentItem;

            if (item.Instance == null)
            {
                item.Instance = Activator.CreateInstance(item.Type) as UXComponent;
                item.Instance.Name = ViewHelper.GetDefaultComponentName(ComponentNames, item.Type);
                object[] attributes = item.Type.GetCustomAttributes(typeof(VisualDesignerAttribute), false);
                
                if (attributes != null)
                {
                    GenericMapper.Map((VisualDesignerAttribute)attributes[0], item.Instance);
                }
            }

            if (UXComponent != null)
            {
                GenericMapper.Map(UXComponent, item.Instance);
            }

            if (item.Instance is UXServiceComponent &&
                (serviceMethod != null ||
                 componentMap != null))
            {
                ((UXServiceComponent)item.Instance).ServiceMethod = serviceMethod;
                ((UXServiceComponent)item.Instance).ComponentMap = componentMap;
            }
            
            UXComponent = item.Instance;

            propertyGrid.SelectedObject = UXComponent;
                        
            EnableDisableButtons();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (UXComponent is UXRadioGroup)
            {
                UXRadioGroup radioGroup = UXComponent as UXRadioGroup;

                if (radioGroup.KeyValues.Count == 0)
                {
                    MessageBox.Show("You need to specify atleast one radio group option.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
                        
            if (componentMap != null && !(UXComponent is UXServiceComponent))
            {
                modelService.DeleteDomainObject(componentMap);
            }

            DialogResult = DialogResult.OK;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (propertyGrid.SelectedObject != null &&
                propertyGrid.SelectedObject is UXComponent)
            {
                UXComponent comp = propertyGrid.SelectedObject as UXComponent;
                                
                if (e.ChangedItem.Label == "Name")
                {
                    if (!NamingGuidance.CheckNameNotInList(comp.Name, "Component Name", "Dialog Component Namelist", ComponentNames, false, true) ||
                        !NamingGuidance.CheckName(comp.Name, "Component Name", true))
                    {
                        comp.Name = (string)e.OldValue;
                    }
                }
            }
        }

        private void btnEditRadioGroup_Click(object sender, EventArgs e)
        {
            if (propertyGrid.SelectedObject != null &&
                propertyGrid.SelectedObject is UXRadioGroup)
            {
                UXRadioGroup uxRadioGroup = propertyGrid.SelectedObject as UXRadioGroup;

                using (KeyValueEditor keyValueEditor = new KeyValueEditor())
                {
                    keyValueEditor.CaptionDialog = "Add Radiobuttons";
                    keyValueEditor.CaptionKeyColumn = "Keyvalue";
                    keyValueEditor.CaptionValueColumn = "Caption";
                    keyValueEditor.MinimumKeysEntered = 1;
                    keyValueEditor.PopulateList(uxRadioGroup.Keys, uxRadioGroup.Values);
                    keyValueEditor.IsEditable = this.IsEditable;

                    if (keyValueEditor.ShowDialog() == DialogResult.OK)
                    {
                        // Get the new set of keys and values
                        uxRadioGroup.Keys = keyValueEditor.KeyValues.Keys.ToList();
                        uxRadioGroup.Values = keyValueEditor.KeyValues.Values.ToList();
                    }
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
