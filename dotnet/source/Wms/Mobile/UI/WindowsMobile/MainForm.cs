using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI.Configuration;
using System.Runtime.InteropServices;
using System.Reflection;


namespace Imi.Wms.Mobile.UI
{
    public partial class MainForm : BaseForm
    {
        private MainPresenter _presenter;

        [DllImport("coredll.dll", EntryPoint = "AddFontResource", SetLastError = true)]
        public static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

        public MainForm()
        {
            InitializeComponent();

            _presenter = new MainPresenter(this);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                _presenter.Initialize();
                _presenter.LoadServers(true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Cursor.Hide();
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.DisposeNativeDriver();
            Application.Exit();
        }

        private void serverListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serverListBox.SelectedIndices.Count < 1)
            {
                connectMenuItem.Enabled = false;
            }
            else
            {
                connectMenuItem.Enabled = true;
            }
        }

        public void installFont()
        {
            bool notInstalled = true;
            using (InstalledFontCollection fontCollection = new InstalledFontCollection())
            {
                foreach (FontFamily font in fontCollection.Families)
                {
                    if (font.Name == "Microsoft Sans Serif")
                    {
                        notInstalled = false;
                    }
                }
            }


            string fontFile = string.Format("{0}\\Fonts\\micross.ttf", new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).AbsolutePath.Replace("%20", " "));
            if (File.Exists(fontFile) && notInstalled)
            {
                int result = AddFontResource(fontFile);
            }

        }

        public void AddServers(IEnumerable<ServerElement> servers)
        {
            serverListBox.Items.Clear();

            foreach (ServerElement server in servers)
            {
                ListViewItem item = new ListViewItem(server.Name);
                item.ImageIndex = 0;
                serverListBox.Items.Add(item);
            }

            if (serverListBox.Items.Count > 0)
            {
                serverListBox.Items[0].Selected = true;
                connectMenuItem.Enabled = true;
            }
            else
            {
                connectMenuItem.Enabled = false;
            }
        }

        private void debugMenuItem_Click(object sender, EventArgs e)
        {
            using (DebugForm form = new DebugForm())
            {
                form.ShowDialog();
            }
        }

        private void addMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.AddServer();
        }

        private void modifyMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.ModifyServer(serverListBox.Items[serverListBox.SelectedIndices[0]].Text);
        }

        private void contextMenu_Popup(object sender, EventArgs e)
        {
            modifyMenuItem.Enabled = serverListBox.SelectedIndices.Count > 0;
            deleteMenuItem.Enabled = serverListBox.SelectedIndices.Count > 0;
            connectMenuItem2.Enabled = connectMenuItem.Enabled;
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.DeleteServer(serverListBox.Items[serverListBox.SelectedIndices[0]].Text);
        }

        private void optionsMenuItem_Click(object sender, EventArgs e)
        {
            _presenter.ModifyOptions();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if ((AutoScaleFactor.Height >= 2) || (AutoScaleFactor.Width >= 2))
            {
                serverListBox.LargeImageList.ImageSize = new Size(48, 48);
            }
        }

        private void fileMenuItem_Click(object sender, EventArgs e)
        {
            contextMenu.Show(this, new Point(0, 0));
        }

        private void connectMenuItem2_Click(object sender, EventArgs e)
        {
            connectMenuItem_Click(sender, e);
        }

        private void connectMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            try
            {
                _presenter.Connect(serverListBox.Items[serverListBox.SelectedIndices[0]].Text, "");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Cursor.Hide();
            }
        }

        private void exitMenuItem2_Click(object sender, EventArgs e)
        {
            exitMenuItem_Click(sender, e);
        }
    }
}
