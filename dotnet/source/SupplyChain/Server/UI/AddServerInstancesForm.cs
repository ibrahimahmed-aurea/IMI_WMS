using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Xml.Serialization;
using Imi.Wms.Server.UI.Configuration;
using Imi.Wms.Server.Net.Broadcast;

namespace Imi.Wms.Server.UI
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class AddServerInstancesForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView AvailableServersView;
        private System.Windows.Forms.Button RemoveBtn;
        private System.Windows.Forms.Button SelectBtn;
        private System.Windows.Forms.ListView SelectedServersView;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Label SelectedLbl;
        private System.Windows.Forms.Label AvailableLbl;
        private System.Windows.Forms.Button OptionBtn;
        private System.Windows.Forms.ColumnHeader sInstanceHeader;
        private System.Windows.Forms.ColumnHeader sHostHeader;
        private System.Windows.Forms.ColumnHeader sPortHeader;
        private System.Windows.Forms.ColumnHeader InstanceHeader;
        private System.Windows.Forms.ColumnHeader HostHeader;
        private System.Windows.Forms.ColumnHeader PortHeader;
        private System.Windows.Forms.ColumnHeader SourceHeader;
        private System.Windows.Forms.ColumnHeader sSourceHeader;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public AddServerInstancesForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SelectedServersView = new System.Windows.Forms.ListView();
            this.sInstanceHeader = new System.Windows.Forms.ColumnHeader();
            this.sHostHeader = new System.Windows.Forms.ColumnHeader();
            this.sPortHeader = new System.Windows.Forms.ColumnHeader();
            this.AvailableServersView = new System.Windows.Forms.ListView();
            this.InstanceHeader = new System.Windows.Forms.ColumnHeader();
            this.HostHeader = new System.Windows.Forms.ColumnHeader();
            this.PortHeader = new System.Windows.Forms.ColumnHeader();
            this.SourceHeader = new System.Windows.Forms.ColumnHeader();
            this.SelectedLbl = new System.Windows.Forms.Label();
            this.AvailableLbl = new System.Windows.Forms.Label();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.RemoveBtn = new System.Windows.Forms.Button();
            this.SelectBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.OptionBtn = new System.Windows.Forms.Button();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.sSourceHeader = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 364);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(542, 330);
            this.panel3.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 330);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SelectedServersView);
            this.tabPage1.Controls.Add(this.AvailableServersView);
            this.tabPage1.Controls.Add(this.SelectedLbl);
            this.tabPage1.Controls.Add(this.AvailableLbl);
            this.tabPage1.Controls.Add(this.RefreshBtn);
            this.tabPage1.Controls.Add(this.RemoveBtn);
            this.tabPage1.Controls.Add(this.SelectBtn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(534, 304);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Choose Instances";
            // 
            // SelectedServersView
            // 
            this.SelectedServersView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                          this.sInstanceHeader,
                                                                                          this.sHostHeader,
                                                                                          this.sPortHeader,
                                                                                          this.sSourceHeader});
            this.SelectedServersView.Enabled = false;
            this.SelectedServersView.FullRowSelect = true;
            this.SelectedServersView.HideSelection = false;
            this.SelectedServersView.Location = new System.Drawing.Point(14, 201);
            this.SelectedServersView.Name = "SelectedServersView";
            this.SelectedServersView.Size = new System.Drawing.Size(415, 97);
            this.SelectedServersView.TabIndex = 5;
            this.SelectedServersView.View = System.Windows.Forms.View.Details;
            this.SelectedServersView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
            // 
            // sInstanceHeader
            // 
            this.sInstanceHeader.Text = "Instance";
            this.sInstanceHeader.Width = 112;
            // 
            // sHostHeader
            // 
            this.sHostHeader.Text = "Host";
            this.sHostHeader.Width = 97;
            // 
            // sPortHeader
            // 
            this.sPortHeader.Text = "Port";
            this.sPortHeader.Width = 50;
            // 
            // AvailableServersView
            // 
            this.AvailableServersView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                           this.InstanceHeader,
                                                                                           this.HostHeader,
                                                                                           this.PortHeader,
                                                                                           this.SourceHeader});
            this.AvailableServersView.FullRowSelect = true;
            this.AvailableServersView.HideSelection = false;
            this.AvailableServersView.Location = new System.Drawing.Point(14, 29);
            this.AvailableServersView.Name = "AvailableServersView";
            this.AvailableServersView.Size = new System.Drawing.Size(415, 147);
            this.AvailableServersView.TabIndex = 1;
            this.AvailableServersView.View = System.Windows.Forms.View.Details;
            this.AvailableServersView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListView_ColumnClick);
            // 
            // InstanceHeader
            // 
            this.InstanceHeader.Text = "Instance";
            this.InstanceHeader.Width = 112;
            // 
            // HostHeader
            // 
            this.HostHeader.Text = "Host";
            this.HostHeader.Width = 97;
            // 
            // PortHeader
            // 
            this.PortHeader.Text = "Port";
            this.PortHeader.Width = 50;
            // 
            // SourceHeader
            // 
            this.SourceHeader.Text = "Source";
            this.SourceHeader.Width = 150;
            // 
            // SelectedLbl
            // 
            this.SelectedLbl.Enabled = false;
            this.SelectedLbl.Location = new System.Drawing.Point(14, 181);
            this.SelectedLbl.Name = "SelectedLbl";
            this.SelectedLbl.Size = new System.Drawing.Size(142, 18);
            this.SelectedLbl.TabIndex = 4;
            this.SelectedLbl.Text = "&Selected Server Instances";
            this.SelectedLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AvailableLbl
            // 
            this.AvailableLbl.Location = new System.Drawing.Point(14, 9);
            this.AvailableLbl.Name = "AvailableLbl";
            this.AvailableLbl.Size = new System.Drawing.Size(142, 18);
            this.AvailableLbl.TabIndex = 0;
            this.AvailableLbl.Text = "&Available Server Instances:";
            this.AvailableLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Location = new System.Drawing.Point(442, 28);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(79, 23);
            this.RefreshBtn.TabIndex = 2;
            this.RefreshBtn.Text = "Re&fresh";
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Enabled = false;
            this.RemoveBtn.Location = new System.Drawing.Point(442, 201);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(79, 23);
            this.RemoveBtn.TabIndex = 6;
            this.RemoveBtn.Text = "&Remove";
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // SelectBtn
            // 
            this.SelectBtn.Location = new System.Drawing.Point(442, 57);
            this.SelectBtn.Name = "SelectBtn";
            this.SelectBtn.Size = new System.Drawing.Size(79, 23);
            this.SelectBtn.TabIndex = 3;
            this.SelectBtn.Text = "&Select";
            this.SelectBtn.Click += new System.EventHandler(this.SelectBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.OptionBtn);
            this.panel2.Controls.Add(this.ApplyBtn);
            this.panel2.Controls.Add(this.OKBtn);
            this.panel2.Controls.Add(this.CancelBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 330);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(542, 34);
            this.panel2.TabIndex = 1;
            // 
            // OptionBtn
            // 
            this.OptionBtn.Location = new System.Drawing.Point(8, 5);
            this.OptionBtn.Name = "OptionBtn";
            this.OptionBtn.TabIndex = 0;
            this.OptionBtn.Text = "Options...";
            this.OptionBtn.Click += new System.EventHandler(this.OptionsBtn_Click);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(462, 5);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.TabIndex = 3;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(302, 5);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(382, 5);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            // 
            // sSourceHeader
            // 
            this.sSourceHeader.Text = "Source";
            this.sSourceHeader.Width = 150;
            // 
            // AddServerInstances
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(552, 374);
            this.Controls.Add(this.panel1);
            this.DockPadding.All = 5;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddServerInstances";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Server Instances";
            this.Load += new System.EventHandler(this.AddServerInstances_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new AddServerInstancesForm());
        }


        private void SelectBtn_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem availableItem in AvailableServersView.SelectedItems)
            {
                bool found = false;

                foreach (ListViewItem selectedItem in SelectedServersView.Items)
                {
                    if (selectedItem.Tag == availableItem)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ListViewItem item = availableItem.Clone() as ListViewItem;
                    item.Tag = availableItem;
                    SelectedServersView.Items.Add(item);
                }
            }

            EnableDisable();
        }

        private void RemoveBtn_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in SelectedServersView.SelectedItems)
            {
                SelectedServersView.Items.Remove(item);
            }

            EnableDisable();
        }

        private void RefreshBtn_Click(object sender, System.EventArgs e)
        {
            AvailableServersView.Items.Clear();
            EnableDisable();
            QueryForServerList();
            EnableDisable();
        }

        private void EnableDisable()
        {
            bool haveList = (AvailableServersView.Items.Count > 0);
            bool haveSelection = (SelectedServersView.Items.Count > 0);

            SelectBtn.Enabled = haveList;
            AvailableServersView.Enabled = haveList;
            AvailableLbl.Enabled = haveList;

            ApplyBtn.Enabled = haveSelection;
            RemoveBtn.Enabled = haveSelection;
            SelectedServersView.Enabled = haveSelection;
            SelectedLbl.Enabled = haveSelection;
        }

        // todo: rename and remove hardcoding of parameters
        // todo: try broadcasting in port interval
        // todo: what if each server responds to broadcast on their own port ?
        // split presentation and the fetching of servers.
        // use event delegate ? and separate classes.

        public string Ip = "239.255.255.255";
        public int Port = 11000;
        public int Timeout = 3000;

        private void QueryForServerList()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                // Define the unique identifier for our service
                Guid serviceId = new Guid(BroadcastServer.serviceIdString);

                // Scan the network for services
                BroadcastClient.ServerResponse[] responses =
                  BroadcastClient.FindServer(serviceId, IPAddress.Parse(Ip), Port, Timeout);

                foreach (BroadcastClient.ServerResponse response in responses)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ServerConfig));
                    StringReader reader = new StringReader(response.Message);

                    try
                    {
                        ServerConfig serverList = xmlSerializer.Deserialize(reader) as ServerConfig;

                        foreach (ServerType sct in serverList.)
                        {
                            String[] v = new string[4] { sct.DisplayName,
                                           sct.HostName,
                                           sct.Port.ToString(),
                                           Dns.GetHostEntry(response.IPAddress).HostName};
                            AvailableServersView.Items.Add(new ListViewItem(v));

                            EnableDisable();
                            Refresh();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (new ConfigurationErrorsException(
                          String.Format("Problems reading the message from source {0}.\nMessage = {1}.", response.IPAddress, response.Message), ex));
                    }
                }

                Refresh();

                if (responses.Length <= 0)
                {
                    MessageBox.Show("Configuration information was not available, no broadcast server replied.",
                                     "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void LoadConfig()
        {
            UIConfig config = UIConfig.Instance();

            Ip = config.BroadcastAddress.Ip;
            Port = config.BroadcastAddress.Port;
            Timeout = config.BroadcastAddress.Timeout;
        }

        private void OptionsBtn_Click(object sender, System.EventArgs e)
        {
            AddServerSettingsForm form = new AddServerSettingsForm();
            form.ShowDialog(this);
            LoadConfig();
        }

        private void AddServerInstances_Load(object sender, System.EventArgs e)
        {
            EnableDisable();
            LoadConfig();
        }

        private void ListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            ListView listView = sender as ListView;

            // Set the ListViewItemSorter property to a new ListViewItemComparer object.
            if ((listView.ListViewItemSorter == null) || (listView.ListViewItemSorter.GetType() == typeof(ListViewItemComparerDesc)))
                listView.ListViewItemSorter = new ListViewItemComparerAsc(e.Column);
            else
                listView.ListViewItemSorter = new ListViewItemComparerDesc(e.Column);

            // Call the sort method to manually sort the column based on the ListViewItemComparer implementation.
            listView.Sort();
        }

        private void Save()
        {
            if (MessageBox.Show("Do you really want to update your configuration with the selected instances?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UIConfig config = UIConfig.Instance();

                foreach (ListViewItem i in SelectedServersView.Items)
                {
                    String name = i.SubItems[0].Text;
                    String host = i.SubItems[1].Text;
                    int port = Convert.ToInt32(i.SubItems[2].Text);
                    bool found = false;

                    // Scan for already existing instances and update the name only
                    foreach (ServerConfigType sct in config.Connections)
                    {
                        if ((sct.HostName == host) && (sct.Port == port))
                        {
                            sct.DisplayName = name;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ServerConfigType serverConfig = new ServerConfigType();
                        serverConfig.DisplayName = name;
                        serverConfig.HostName = host;
                        serverConfig.Port = port;
                        config.Connections.Add(serverConfig);
                    }
                }

                config.Save(config.Connections);
            }
        }

        private void OKBtn_Click(object sender, System.EventArgs e)
        {
            Save();
            Close();
        }

        private void ApplyBtn_Click(object sender, System.EventArgs e)
        {
            Save();
        }
    }

    // Implements the manual sorting of items by columns.
    class ListViewItemComparerAsc : IComparer
    {
        private int col;
        public ListViewItemComparerAsc()
        {
            col = 0;
        }
        public ListViewItemComparerAsc(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return string.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }

    // Implements the manual sorting of items by columns.
    class ListViewItemComparerDesc : IComparer
    {
        private int col;
        public ListViewItemComparerDesc()
        {
            col = 0;
        }
        public ListViewItemComparerDesc(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            int r = string.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            if (r < 0)
                r = 1;
            else
            {
                if (r > 0)
                    r = -1;
            }

            return (r);
        }
    }

}
