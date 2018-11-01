using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Imi.SupplyChain.Server.UI.Configuration;

namespace Imi.SupplyChain.Server.UI
{
    /// <summary>
    /// Summary description for ServerModify.
    /// </summary>
    public class ServerModify : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl SettingControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage ConnectionPage;
        private Imi.SupplyChain.Server.UI.Controls.WhHeaderPanel whHeaderPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DisplayNameFld;
        private System.Windows.Forms.TextBox HostNameFld;
        private System.Windows.Forms.TextBox PortFld;
        private System.Windows.Forms.Button ApplyBtn;
        private System.ComponentModel.IContainer components = null;

        public System.Windows.Forms.Button ApplyButton;
        private ManagedConnection currentConnection;

        public ServerModify(ManagedConnection c)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            ApplyButton = this.ApplyBtn;
            currentConnection = c;
        }

        public ServerModify(string newCaption, ManagedConnection c) : this(c)
        {
            this.Text = newCaption;
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
            this.SettingControl = new System.Windows.Forms.TabControl();
            this.ConnectionPage = new System.Windows.Forms.TabPage();
            this.whHeaderPanel4 = new Imi.SupplyChain.Server.UI.Controls.WhHeaderPanel();
            this.DisplayNameFld = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.HostNameFld = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PortFld = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SettingControl.SuspendLayout();
            this.ConnectionPage.SuspendLayout();
            this.whHeaderPanel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SettingControl);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(448, 246);
            this.panel1.TabIndex = 0;
            // 
            // SettingControl
            // 
            this.SettingControl.Controls.Add(this.ConnectionPage);
            this.SettingControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingControl.ItemSize = new System.Drawing.Size(100, 18);
            this.SettingControl.Location = new System.Drawing.Point(0, 0);
            this.SettingControl.Name = "SettingControl";
            this.SettingControl.SelectedIndex = 0;
            this.SettingControl.Size = new System.Drawing.Size(448, 214);
            this.SettingControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.SettingControl.TabIndex = 0;
            // 
            // ConnectionPage
            // 
            this.ConnectionPage.Controls.Add(this.whHeaderPanel4);
            this.ConnectionPage.Location = new System.Drawing.Point(4, 22);
            this.ConnectionPage.Name = "ConnectionPage";
            this.ConnectionPage.Size = new System.Drawing.Size(440, 188);
            this.ConnectionPage.TabIndex = 3;
            this.ConnectionPage.Text = "Connection";
            // 
            // whHeaderPanel4
            // 
            this.whHeaderPanel4.BackColor = System.Drawing.Color.Transparent;
            this.whHeaderPanel4.Controls.Add(this.DisplayNameFld);
            this.whHeaderPanel4.Controls.Add(this.label2);
            this.whHeaderPanel4.Controls.Add(this.HostNameFld);
            this.whHeaderPanel4.Controls.Add(this.label1);
            this.whHeaderPanel4.Controls.Add(this.PortFld);
            this.whHeaderPanel4.Controls.Add(this.label3);
            this.whHeaderPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whHeaderPanel4.Location = new System.Drawing.Point(0, 0);
            this.whHeaderPanel4.Name = "whHeaderPanel4";
            this.whHeaderPanel4.Size = new System.Drawing.Size(440, 188);
            this.whHeaderPanel4.TabIndex = 0;
            this.whHeaderPanel4.Text = "Server Connection";
            // 
            // DisplayNameFld
            // 
            this.DisplayNameFld.Location = new System.Drawing.Point(24, 56);
            this.DisplayNameFld.Name = "DisplayNameFld";
            this.DisplayNameFld.Size = new System.Drawing.Size(272, 20);
            this.DisplayNameFld.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Display Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HostNameFld
            // 
            this.HostNameFld.Location = new System.Drawing.Point(24, 104);
            this.HostNameFld.Name = "HostNameFld";
            this.HostNameFld.Size = new System.Drawing.Size(272, 20);
            this.HostNameFld.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Hostname:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PortFld
            // 
            this.PortFld.Location = new System.Drawing.Point(24, 152);
            this.PortFld.Name = "PortFld";
            this.PortFld.Size = new System.Drawing.Size(104, 20);
            this.PortFld.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Port:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ApplyBtn);
            this.panel2.Controls.Add(this.OKBtn);
            this.panel2.Controls.Add(this.CancelBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 214);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(448, 32);
            this.panel2.TabIndex = 0;
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(368, 8);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(75, 23);
            this.ApplyBtn.TabIndex = 2;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(208, 8);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(288, 8);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 23);
            this.label12.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 0;
            // 
            // ServerModify
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(458, 256);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerModify";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Properties";
            this.Load += new System.EventHandler(this.ServerModify_Load);
            this.panel1.ResumeLayout(false);
            this.SettingControl.ResumeLayout(false);
            this.ConnectionPage.ResumeLayout(false);
            this.whHeaderPanel4.ResumeLayout(false);
            this.whHeaderPanel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void Save()
        {
            ManagedConnection c = currentConnection;
            c.Config.DisplayName = DisplayNameFld.Text;
            c.Config.HostName = HostNameFld.Text;
            c.Config.Port = Convert.ToInt32(PortFld.Text);
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

        private void ServerModify_Load(object sender, System.EventArgs e)
        {
            DisplayNameFld.Text = currentConnection.Config.DisplayName;
            HostNameFld.Text = currentConnection.Config.HostName;
            PortFld.Text = currentConnection.Config.Port.ToString();
            DisplayNameFld.Focus();
            DisplayNameFld.SelectAll();

        }


    }
}
