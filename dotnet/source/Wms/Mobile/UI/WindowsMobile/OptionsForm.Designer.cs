namespace Imi.Wms.Mobile.UI
{
    partial class OptionsForm
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
            this.logCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nativeDriverTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.terminalIdTextBox = new System.Windows.Forms.TextBox();
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
            this.panel1.Controls.Add(this.logCheckBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nativeDriverTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.terminalIdTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 268);
            // 
            // logCheckBox
            // 
            this.logCheckBox.Location = new System.Drawing.Point(3, 66);
            this.logCheckBox.Name = "logCheckBox";
            this.logCheckBox.Size = new System.Drawing.Size(147, 20);
            this.logCheckBox.TabIndex = 10;
            this.logCheckBox.Text = "Enable Logging";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 37);
            this.label2.Text = "Native Driver:";
            // 
            // nativeDriverTextBox
            // 
            this.nativeDriverTextBox.Location = new System.Drawing.Point(96, 39);
            this.nativeDriverTextBox.Name = "nativeDriverTextBox";
            this.nativeDriverTextBox.Size = new System.Drawing.Size(141, 21);
            this.nativeDriverTextBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 21);
            this.label1.Text = "Terminal ID:";
            // 
            // terminalIdTextBox
            // 
            this.terminalIdTextBox.Location = new System.Drawing.Point(96, 12);
            this.terminalIdTextBox.Name = "terminalIdTextBox";
            this.terminalIdTextBox.Size = new System.Drawing.Size(141, 21);
            this.terminalIdTextBox.TabIndex = 8;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem okMenuItem;
        private System.Windows.Forms.MenuItem cancelMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox logCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nativeDriverTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox terminalIdTextBox;

    }
}