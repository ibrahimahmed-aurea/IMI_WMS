using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI.Configuration;

namespace Imi.Wms.Mobile.UI
{
    public partial class ModifyServerForm : Form
    {
        private ModifyServerPresenter _presenter;

        public ModifyServerForm()
        {
            InitializeComponent();
            ServerElement = new ServerElement();
            _presenter = new ModifyServerPresenter(this);
        }

        public ServerElement ServerElement { get; set; }
                
        private void okButton_Click(object sender, EventArgs e)
        {
            ServerElement.Name = nameTextBox.Text;
            ServerElement.HostName = hostTextBox.Text;
            ServerElement.Default = defaultCheckBox.Checked;
            ServerElement.DefaultApplication = defaultApptextBox.Text;

            bool success = false;

            try
            {
                ServerElement.Port = int.Parse(portTextBox.Text);
                success = true;
            }
            catch (ArgumentNullException)
            {
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }

            if (!success)
            {
                MessageBox.Show("Please enter a valid port number.", "IMI iWMS Thin Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                _presenter.Save(ServerElement);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "IMI iWMS Thin Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ModifyServerForm_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = ServerElement.Name;
            hostTextBox.Text = ServerElement.HostName;
            portTextBox.Text = ServerElement.Port.ToString();
            defaultCheckBox.Checked = ServerElement.Default;
            defaultApptextBox.Text = ServerElement.DefaultApplication;
        }

        private void defaultCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            defaultApptextBox.Enabled = defaultCheckBox.Checked;
        }

        private void defaultApptextBox_EnabledChanged(object sender, EventArgs e)
        {
            if (!defaultApptextBox.Enabled) { defaultApptextBox.Clear(); }
        }
    }
}
