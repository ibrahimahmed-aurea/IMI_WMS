using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess;


namespace Cdc.MetaManager.GUI
{
    public partial class ConfigureTwoWayListBoxForm : MdiChildForm
    {
        IApplicationService applicationService;
        IDialogService dialogService;
        IModelService modelService;

        public ConfigureTwoWayListBoxForm()
        {
            InitializeComponent();
            
            applicationService = MetaManagerServices.GetApplicationService();
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
                        
        }

        public UXTwoWayListBox ListBox { get; set; }
        public Cdc.MetaManager.DataAccess.Domain.View View { get; set; }

        private void leftServiceMethodFindBtn_Click(object sender, EventArgs e)
        {
            using (FindServiceMethodForm form = new FindServiceMethodForm())
            {
                form.BackendApplication = BackendApplication;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.LeftFindServiceMethod = form.ServiceMethod;
                    PopulateComboBoxes(ListBox.LeftFindServiceMethod, leftFindServiceMethodTbx, leftDisplayProperty1Cbx, leftDisplayProperty2Cbx);

                    if (ListBox.LeftFindServiceMethodMap != null)
                    {
                        ListBox.LeftFindServiceMethodMap = modelService.GetInitializedDomainObject<PropertyMap>(ListBox.LeftFindServiceMethodMap.Id);
                        ListBox.LeftFindServiceMethodMap.MappedProperties.Clear();
                    }

                    if (ListBox.AddServiceMethodMap != null)
                    {
                        ListBox.AddServiceMethodMap = modelService.GetInitializedDomainObject<PropertyMap>(ListBox.AddServiceMethod.Id);
                        ListBox.AddServiceMethodMap.MappedProperties.Clear();
                    }
                }
            }
        }

        private void PopulateComboBoxes(ServiceMethod serviceMethod, TextBox textBox, ComboBox combo1, ComboBox combo2)
        {
            combo1.Items.Clear();
            combo2.Items.Clear();
            textBox.Text = null;

            if (serviceMethod != null)
            {
                serviceMethod = applicationService.GetServiceMethodMapsById(serviceMethod.Id);

                textBox.Text = serviceMethod.Name;

                foreach (MappedProperty property in serviceMethod.ResponseMap.MappedProperties)
                {
                    combo1.Items.Add(property.Name);
                    combo2.Items.Add(property.Name);
                }
            }

            UpdateSelectedValues();
        }

        private void UpdateSelectedValues()
        {
            if (ListBox.LeftDisplayPropertyNames.Count > 0)
            {
                leftDisplayProperty1Cbx.SelectedItem = ListBox.LeftDisplayPropertyNames[0];

                if (ListBox.LeftDisplayPropertyNames.Count > 1)
                {
                    leftDisplayProperty2Cbx.SelectedItem = ListBox.LeftDisplayPropertyNames[1];
                }
            }

            if (ListBox.RightDisplayPropertyNames.Count > 0)
            {
                rightDisplayProperty1Cbx.SelectedItem = ListBox.RightDisplayPropertyNames[0];

                if (ListBox.RightDisplayPropertyNames.Count > 1)
                {
                    rightDisplayProperty2Cbx.SelectedItem = ListBox.RightDisplayPropertyNames[1];
                }
            }

            addServiceMethodTbx.Text = null;

            if (ListBox.AddServiceMethod != null)
                addServiceMethodTbx.Text = ListBox.AddServiceMethod.Name;

            deleteServiceMethodTbx.Text = null;

            if (ListBox.RemoveServiceMethod != null)
                deleteServiceMethodTbx.Text = ListBox.RemoveServiceMethod.Name;
        }

        private void rightServiceMethodFindBtn_Click(object sender, EventArgs e)
        {
            using (FindServiceMethodForm form = new FindServiceMethodForm())
            {
                form.BackendApplication = BackendApplication;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.RightFindServiceMethod = form.ServiceMethod;
                    PopulateComboBoxes(ListBox.RightFindServiceMethod, rightFindServiceMethodTbx, rightDisplayProperty1Cbx, rightDisplayProperty2Cbx);

                    if (ListBox.RightFindServiceMethodMap != null)
                    {
                        ListBox.RightFindServiceMethodMap = modelService.GetInitializedDomainObject<PropertyMap>(ListBox.RightFindServiceMethodMap.Id);
                        ListBox.RightFindServiceMethodMap.MappedProperties.Clear();
                    }

                    if (ListBox.RemoveServiceMethodMap != null)
                    {
                        ListBox.RemoveServiceMethodMap = modelService.GetInitializedDomainObject<PropertyMap>(ListBox.RemoveServiceMethodMap.Id);
                        ListBox.RemoveServiceMethodMap.MappedProperties.Clear();
                    }
                }
            }
        }

        private void addServiceMethodFindBtn_Click(object sender, EventArgs e)
        {
            using (FindServiceMethodForm form = new FindServiceMethodForm())
            {
                form.BackendApplication = BackendApplication;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.AddServiceMethod = form.ServiceMethod;
                    UpdateSelectedValues();

                    if (ListBox.AddServiceMethodMap != null)
                    {
                        ListBox.AddServiceMethodMap = modelService.GetInitializedDomainObject<PropertyMap>(ListBox.AddServiceMethodMap.Id);
                        ListBox.AddServiceMethodMap.MappedProperties.Clear();
                    }
                }
            }
        }

        private void deleteServiceMethodFindBtn_Click(object sender, EventArgs e)
        {
            using (FindServiceMethodForm form = new FindServiceMethodForm())
            {
                form.BackendApplication = BackendApplication;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.RemoveServiceMethod = form.ServiceMethod;
                    UpdateSelectedValues();
                }
            }

            if (ListBox.RemoveServiceMethodMap != null)
            {
                ListBox.RemoveServiceMethodMap = modelService.GetInitializedDomainObject<PropertyMap>(ListBox.RemoveServiceMethodMap.Id);
                ListBox.RemoveServiceMethodMap.MappedProperties.Clear();
            }
            
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void leftServiceMethodMapBtn_Click(object sender, EventArgs e)
        {
            IList<IMappableProperty> sourceProperties;
            IList<IMappableProperty> targetProperties;
            IList<IMappableProperty> temp;
            PropertyMap map;

            dialogService.GetViewResponseMap(View, out map, out temp, out temp, out targetProperties);
            applicationService.GetServiceMethodRequestMap(ListBox.LeftFindServiceMethod, out map, out sourceProperties, out temp);

            using (PropertyMapForm2 form = new PropertyMapForm2())
            {
                form.PropertyMap = ListBox.LeftFindServiceMethodMap;
                form.SourceProperties = sourceProperties;
                form.TargetProperties = targetProperties;
                form.IsEditable = this.IsEditable;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.LeftFindServiceMethodMap = form.PropertyMap;
                }
            }
        }

        private void rightServiceMethodMapBtn_Click(object sender, EventArgs e)
        {
            IList<IMappableProperty> sourceProperties;
            IList<IMappableProperty> targetProperties;
            IList<IMappableProperty> temp;
            PropertyMap map;

            dialogService.GetViewResponseMap(View, out map, out temp, out temp, out targetProperties);
            applicationService.GetServiceMethodRequestMap(ListBox.RightFindServiceMethod, out map, out sourceProperties, out temp);

            using (PropertyMapForm2 form = new PropertyMapForm2())
            {
                form.PropertyMap = ListBox.RightFindServiceMethodMap;
                form.SourceProperties = sourceProperties;
                form.TargetProperties = targetProperties;
                form.IsEditable = this.IsEditable;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.RightFindServiceMethodMap = form.PropertyMap;
                }
            }
        }

        private void addServiceMethodMapBtn_Click(object sender, EventArgs e)
        {
            IList<IMappableProperty> sourceProperties;
            IList<IMappableProperty> targetProperties;
            IList<IMappableProperty> temp;
            PropertyMap map;

            applicationService.GetServiceMethodResponseMap(ListBox.LeftFindServiceMethod, out map, out temp, out targetProperties);
            applicationService.GetServiceMethodRequestMap(ListBox.AddServiceMethod, out map, out sourceProperties, out temp);

            IList<UXSessionProperty> sessionProperties = applicationService.GetUXSessionProperties(FrontendApplication);
            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            using (PropertyMapForm2 form = new PropertyMapForm2())
            {
                form.PropertyMap = ListBox.AddServiceMethodMap;
                form.SourceProperties = sourceProperties;
                form.TargetProperties = targetProperties;
                form.IsEditable = this.IsEditable;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.AddServiceMethodMap = form.PropertyMap;
                }
            }
        }

        private void deleteServiceMethodMapBtn_Click(object sender, EventArgs e)
        {
            IList<IMappableProperty> sourceProperties;
            IList<IMappableProperty> targetProperties;
            IList<IMappableProperty> temp;
            PropertyMap map;

            applicationService.GetServiceMethodResponseMap(ListBox.RightFindServiceMethod, out map, out temp, out targetProperties);
            applicationService.GetServiceMethodRequestMap(ListBox.RemoveServiceMethod, out map, out sourceProperties, out temp);

            IList<UXSessionProperty> sessionProperties = applicationService.GetUXSessionProperties(FrontendApplication);
            targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

            using (PropertyMapForm2 form = new PropertyMapForm2())
            {
                form.PropertyMap = ListBox.RemoveServiceMethodMap;
                form.SourceProperties = sourceProperties;
                form.TargetProperties = targetProperties;
                form.IsEditable = this.IsEditable;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ListBox.RemoveServiceMethodMap = form.PropertyMap;
                }
            }
        }

        private void ConfigureTwoWayListBoxForm_Load(object sender, EventArgs e)
        {
            leftCaptionTbx.Text = ListBox.LeftCaption;
            rightCaptionTbx.Text = ListBox.RightCaption;

            PopulateComboBoxes(ListBox.LeftFindServiceMethod, leftFindServiceMethodTbx, leftDisplayProperty1Cbx, leftDisplayProperty2Cbx);
            PopulateComboBoxes(ListBox.RightFindServiceMethod, rightFindServiceMethodTbx, rightDisplayProperty1Cbx, rightDisplayProperty2Cbx);
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            leftServiceMethodFindBtn.Enabled = this.IsEditable;
            rightServiceMethodFindBtn.Enabled = this.IsEditable;
            addServiceMethodFindBtn.Enabled = this.IsEditable;
            deleteServiceMethodFindBtn.Enabled = this.IsEditable;
            if (!this.IsEditable)
            {
                leftCaptionTbx.ReadOnly = true;
                rightCaptionTbx.ReadOnly = true;
            }
            okBtn.Enabled = this.IsEditable;
        }

        private void okBtn_Click_1(object sender, EventArgs e)
        {
            ListBox.LeftDisplayPropertyNames.Clear();
            ListBox.RightDisplayPropertyNames.Clear();

            if (leftDisplayProperty1Cbx.SelectedIndex >= 0)
                ListBox.LeftDisplayPropertyNames.Add(leftDisplayProperty1Cbx.Text);

            if (leftDisplayProperty2Cbx.SelectedIndex >= 0)
                ListBox.LeftDisplayPropertyNames.Add(leftDisplayProperty2Cbx.Text);

            ListBox.RightDisplayPropertyNames.Clear();

            if (rightDisplayProperty1Cbx.SelectedIndex >= 0)
                ListBox.RightDisplayPropertyNames.Add(rightDisplayProperty1Cbx.Text);

            if (rightDisplayProperty2Cbx.SelectedIndex >= 0)
                ListBox.RightDisplayPropertyNames.Add(rightDisplayProperty2Cbx.Text);


            if (ListBox.LeftFindServiceMethodMap != null)
                ListBox.LeftFindServiceMethodMap = applicationService.SaveAndMergePropertyMap(ListBox.LeftFindServiceMethodMap);
            
            if (ListBox.RightFindServiceMethodMap != null)
                ListBox.RightFindServiceMethodMap = applicationService.SaveAndMergePropertyMap(ListBox.RightFindServiceMethodMap);

            if (ListBox.AddServiceMethodMap != null)
                ListBox.AddServiceMethodMap = applicationService.SaveAndMergePropertyMap(ListBox.AddServiceMethodMap);

            if (ListBox.RemoveServiceMethodMap != null)
                ListBox.RemoveServiceMethodMap = applicationService.SaveAndMergePropertyMap(ListBox.RemoveServiceMethodMap);

            ListBox.LeftCaption = leftCaptionTbx.Text;
            ListBox.RightCaption = rightCaptionTbx.Text;

             
        }
    }
}
