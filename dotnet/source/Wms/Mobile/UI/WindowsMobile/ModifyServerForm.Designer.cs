namespace Imi.Wms.Mobile.UI
{
    partial class ModifyServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.okMenuItem = new System.Windows.Forms.MenuItem();
            this.cancelMenuItem = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.defaultCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hostTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.defaultApptextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.okMenuItem);
            this.mainMenu.MenuItems.Add(this.cancelMenuItem);
            // 
            // okMenuItem
            // 
            this.okMenuItem.Text = "OK";
            this.okMenuItem.Click += new System.EventHandler(this.okMenuItem_Click);
            // 
            // cancelMenuItem
            // 
            this.cancelMenuItem.Text = "Cancel";
            this.cancelMenuItem.Click += new System.EventHandler(this.cancelMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.defaultApptextBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.defaultCheckBox);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.portTextBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.hostTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nameTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 268);
            // 
            // defaultCheckBox
            // 
            this.defaultCheckBox.Location = new System.Drawing.Point(86, 90);
            this.defaultCheckBox.Name = "defaultCheckBox";
            this.defaultCheckBox.Size = new System.Drawing.Size(100, 20);
            this.defaultCheckBox.TabIndex = 16;
            this.defaultCheckBox.Text = "Default";
            this.defaultCheckBox.CheckStateChanged += new System.EventHandler(this.defaultCheckBox_CheckStateChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 21);
            this.label3.Text = "Port:";
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(86, 63);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(81, 21);
            this.portTextBox.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 21);
            this.label2.Text = "Host:";
            // 
            // hostTextBox
            // 
            this.hostTextBox.Location = new System.Drawing.Point(86, 36);
            this.hostTextBox.Name = "hostTextBox";
            this.hostTextBox.Size = new System.Drawing.Size(151, 21);
            this.hostTextBox.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 21);
            this.label1.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(86, 9);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(151, 21);
            this.nameTextBox.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 21);
            this.label4.Text = "Default App:";
            // 
            // defaultApptextBox
            // 
            this.defaultApptextBox.Enabled = false;
            this.defaultApptextBox.Location = new System.Drawing.Point(86, 113);
            this.defaultApptextBox.Name = "defaultApptextBox";
            this.defaultApptextBox.Size = new System.Drawing.Size(151, 21);
            this.defaultApptextBox.TabIndex = 22;
            this.defaultApptextBox.EnabledChanged += new System.EventHandler(this.defaultApptextBox_EnabledChanged);
            // 
            // ModifyServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "ModifyServerForm";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.ModifyServerForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem okMenuItem;
        private System.Windows.Forms.MenuItem cancelMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox defaultCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox hostTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox defaultApptextBox;
        private System.Windows.Forms.Label label4;

    }
}