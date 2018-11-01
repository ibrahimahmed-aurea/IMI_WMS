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
    public partial class SelectMappedPropertyFromPropertyMap : Form
    {
        public IList<MappedProperty> ValidMappedPropertyList { get; set; }
        public MappedProperty SelectedMappedProperty { get; set; }

        private IDialogService dialogService = null;
        private IUXActionService actionService = null;

        private List<ComboBoxShowMappedProperty> comboBoxList = null;

        public SelectMappedPropertyFromPropertyMap()
        {
            InitializeComponent();

            comboBoxList = new List<ComboBoxShowMappedProperty>();

            actionService = MetaManagerServices.GetUXActionService();
            dialogService = MetaManagerServices.GetDialogService();

            SelectedMappedProperty = null;
        }

        private void SelectMappedPropertyFromPropertyMap_Load(object sender, EventArgs e)
        {
            if (ValidMappedPropertyList != null)
            {
                foreach (MappedProperty property in ValidMappedPropertyList)
                {
                    comboBoxList.Add(new ComboBoxShowMappedProperty() { Title = property.Name, MappedProperty = property });
                }
            }

            MappedProperty savedMappedProperty = SelectedMappedProperty;

            comboBoxShowMappedPropertyBindingSource.DataSource = comboBoxList;

            if (savedMappedProperty != null)
            {
                if (!FindMappedProperty(savedMappedProperty))
                    comboBoxShowMappedPropertyBindingSource.MoveFirst();
            }

            EnableDisableButtons();
        }

        private bool FindMappedProperty(MappedProperty findProperty)
        {
            foreach (ComboBoxShowMappedProperty property in comboBoxList)
            {
                if (property.MappedProperty.Id == findProperty.Id)
                {
                    int foundIndex = comboBoxShowMappedPropertyBindingSource.IndexOf(property);

                    comboBoxShowMappedPropertyBindingSource.Position = foundIndex;

                    return true;
                }
            }
            return false;
        }

        private void comboBoxShowMappedPropertyBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            SelectedMappedProperty = ((ComboBoxShowMappedProperty)comboBoxShowMappedPropertyBindingSource.Current).MappedProperty;

            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnOK.Enabled = false;

            if (SelectedMappedProperty != null)
            {
                btnOK.Enabled = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

    public class ComboBoxShowMappedProperty
    {
        public string Title { get; set; }
        public MappedProperty MappedProperty { get; set; }
    }
}
