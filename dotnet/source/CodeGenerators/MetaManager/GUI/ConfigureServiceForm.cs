using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;


namespace Cdc.MetaManager.GUI
{
    public partial class ConfigureServiceForm : MdiChildForm
    {
        private IApplicationService applicationService;

        public ConfigureServiceForm()
        {
            InitializeComponent();

            applicationService = MetaManagerServices.GetApplicationService();

         
        }

        public UXServiceComponent UXServiceComponent { get; set; }

        private void fintBtn_Click(object sender, EventArgs e)
        {
            using (FindServiceMethodForm form = new FindServiceMethodForm())
            {
                form.BackendApplication = BackendApplication;
                form.AutoSearchName = UXServiceComponent.ServiceMethod == null ? string.Empty : UXServiceComponent.ServiceMethod.Name;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    UXServiceComponent.ServiceMethod = form.ServiceMethod;
                    PopulateComboxBoxes();
                }
            }

            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            okBtn.Enabled = false;

            if (UXServiceComponent.ServiceMethod != null &&
                keyPropertyCbx.SelectedItem != null &&
                displayProperty1Cbx.SelectedItem != null)
            {
                okBtn.Enabled = true;
            }
        }

        private void PopulateComboxBoxes()
        {
            keyPropertyCbx.Items.Clear();
            displayProperty1Cbx.Items.Clear();
            displayProperty2Cbx.Items.Clear();

            if (UXServiceComponent.ServiceMethod != null)
            {
                ServiceMethod serviceMethod = applicationService.GetServiceMethodMapsById(UXServiceComponent.ServiceMethod.Id);

                serviceMethodTbx.Text = serviceMethod.Name;

                foreach (MappedProperty property in serviceMethod.ResponseMap.MappedProperties)
                {
                    keyPropertyCbx.Items.Add(property.Name);
                    displayProperty1Cbx.Items.Add(property.Name);
                    displayProperty2Cbx.Items.Add(property.Name);
                }

                keyPropertyCbx.SelectedItem = UXServiceComponent.KeyPropertyName;

                if (UXServiceComponent.DisplayPropertyNames.Count > 0)
                {
                    displayProperty1Cbx.SelectedItem = UXServiceComponent.DisplayPropertyNames[0];

                    if (UXServiceComponent.DisplayPropertyNames.Count > 1)
                    {
                        displayProperty2Cbx.SelectedItem = UXServiceComponent.DisplayPropertyNames[1];
                    }
                }
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            serviceMethodTbx.Text = null;

            UXServiceComponent.ServiceMethod = null;
            UXServiceComponent.DisplayPropertyNames.Clear();
            UXServiceComponent.KeyPropertyName = null;
                        
            PopulateComboxBoxes();

            EnableDisableButtons();
        }

        private void ConfigureServiceForm_Load(object sender, EventArgs e)
        {
            PopulateComboxBoxes();
            EnableDisableButtons();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            UXServiceComponent.KeyPropertyName = keyPropertyCbx.Text;

            UXServiceComponent.DisplayPropertyNames.Clear();

            if (displayProperty1Cbx.SelectedItem != null)
                UXServiceComponent.DisplayPropertyNames.Add((string)displayProperty1Cbx.SelectedItem);

            if (displayProperty2Cbx.SelectedItem != null)
                UXServiceComponent.DisplayPropertyNames.Add((string)displayProperty2Cbx.SelectedItem);
                        
            Close();
        }

        private void btnClearDisplayProp2_Click(object sender, EventArgs e)
        {
            displayProperty2Cbx.SelectedItem = null;
        }

        private void keyPropertyCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void displayProperty1Cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }
    }
}
