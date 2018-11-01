using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using Imi.Wms.Mobile.Server;
using System.Threading;
using Imi.Wms.Mobile.UI.Configuration;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.UI
{
    public partial class ApplicationForm : System.Windows.Forms.Form
    {   
        private ApplicationPresenter _presenter;

        public ApplicationForm()
        {
            InitializeComponent();
            
            _presenter = new ApplicationPresenter(this);
        }

        public ApplicationPresenter Presenter
        {
            get
            {
                return _presenter;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                _presenter.StartApplication(appListBox.SelectedItem.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void appListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (appListBox.SelectedIndex < 0)
            {
                startButton.Enabled = false;
            }
            else
            {
                startButton.Enabled = true;
            }
        }

        public void AddApplications(ConfigurationResponse configurationResponse)
        {
            appListBox.Items.Clear();

            foreach (Imi.Wms.Mobile.Server.Interface.Application app in configurationResponse.Applications)
            {
                appListBox.Items.Add(app.Name);
            }

            if (appListBox.Items.Count > 0)
            {
                appListBox.SelectedIndex = 0;
                startButton.Enabled = true;
            }
            else
            {
                startButton.Enabled = false;
            }
        }
                
        private void ApplicationForm_Shown(object sender, EventArgs e)
        {
            _presenter.LoadApplications();
        }
    }
}
