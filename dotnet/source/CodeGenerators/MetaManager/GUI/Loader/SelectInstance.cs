using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cdc.MetaManager.GUI.Loader
{
    public partial class SelectInstance : Form
    {
        public string SelectedInstance { get; private set; }

        public SelectInstance()
        {
            InitializeComponent();
        }

        private void SelectInstance_Load(object sender, EventArgs e)
        {
            Imi.Framework.Shared.Configuration.InstanceDataCollection collection = Imi.Framework.Shared.Configuration.InstanceConfigurationProvider.Instances;

            foreach (Imi.Framework.Shared.Configuration.InstanceDataElement instance in collection)
            {
                InstanceslistBox.Items.Add(instance.Name);
            }

            if (InstanceslistBox.Items.Count > 0)
            {
                InstanceslistBox.SelectedIndex = 0;
            }

        }

        private void StartInstancebutton_Click(object sender, EventArgs e)
        {
            if (InstanceslistBox.SelectedIndex > -1)
            {
                SelectedInstance = InstanceslistBox.Items[InstanceslistBox.SelectedIndex].ToString();
            }

            this.Close();
        }

        private void InstanceslistBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StartInstancebutton_Click(this, null);
        }

        private void InstanceslistBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StartInstancebutton_Click(this, null);
            }
        }
    }
}
