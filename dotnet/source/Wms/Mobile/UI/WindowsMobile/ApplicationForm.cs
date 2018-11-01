using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI.Configuration;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.UI
{
    public partial class ApplicationForm : BaseForm
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

        private void startMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            
            try
            {
                _presenter.StartApplication(appListBox.SelectedItem.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Cursor.Hide();
            }
        }
               
        private void appListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (appListBox.SelectedIndex < 0)
            {
                startMenuItem.Enabled = false;
            }
            else
            {
                startMenuItem.Enabled = true;
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
                startMenuItem.Enabled = true;
            }
            else
            {
                startMenuItem.Enabled = false;
            }
        }
                
        private void ApplicationForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
            Cursor.Hide();

            _presenter.LoadApplications();
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}