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

namespace Imi.Wms.Mobile.UI
{
    public partial class MainForm : System.Windows.Forms.Form
    {   
        private MainPresenter _presenter;
        private bool _isLoaded;

        public MainForm()
        {
            InitializeComponent();
            
            _presenter = new MainPresenter(this);

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            _presenter.DisposeNativeDriver();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                _presenter.Connect(serverListBox.SelectedItems[0].Text, "");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void serverListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serverListBox.SelectedItems.Count == 0)
            {
                connectButton.Enabled = false;
            }
            else
            {
                connectButton.Enabled = true;
            }
        }

        public void AddServers(IEnumerable<ServerElement> servers)
        {
            serverListBox.Items.Clear();

            foreach (ServerElement server in servers)
            {
                serverListBox.Items.Add(server.Name, 0);
            }

            if (serverListBox.Items.Count > 0)
            {
                serverListBox.Items[0].Selected = true;
                connectButton.Enabled = true;
            }
            else
            {
                connectButton.Enabled = false;
            }
        }

        private void addMenuItem_Click(object sender, EventArgs e)
        {
            using (ModifyServerForm form = new ModifyServerForm())
            {
                _presenter.AddServer();
            }
        }

        private void modifyMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.ModifyServer(serverListBox.SelectedItems[0].Text);
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            modifyMenuItem.Enabled = serverListBox.SelectedItems.Count > 0;
            deleteMenuItem.Enabled = serverListBox.SelectedItems.Count > 0;
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.DeleteServer(serverListBox.SelectedItems[0].Text);
        }

        private void debugMenuItem_Click(object sender, EventArgs e)
        {
            using (DebugForm form = new DebugForm())
            {
                form.ShowDialog();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                Cursor.Current = Cursors.WaitCursor;
                
                try
                {
                    _presenter.Initialize();
                    _presenter.LoadServers(true);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void serverListBox_DoubleClick(object sender, EventArgs e)
        {
            if (serverListBox.SelectedItems.Count > 0)
            {
                connectButton_Click(this, e);
            }
        }

        private void optionsMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.ModifyOptions();
        }
    }
}
