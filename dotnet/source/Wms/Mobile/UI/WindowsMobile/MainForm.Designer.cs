namespace Imi.Wms.Mobile.UI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.connectMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.connectMenuItem2 = new System.Windows.Forms.MenuItem();
            this.addMenuItem = new System.Windows.Forms.MenuItem();
            this.modifyMenuItem = new System.Windows.Forms.MenuItem();
            this.deleteMenuItem = new System.Windows.Forms.MenuItem();
            this.optionsMenuItem = new System.Windows.Forms.MenuItem();
            this.debugMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem2 = new System.Windows.Forms.MenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.serverListBox = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(this.fileMenuItem);
            this.mainMenu.MenuItems.Add(this.connectMenuItem);
            this.mainMenu.MenuItems.Add(this.exitMenuItem);
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Text = "File";
            this.fileMenuItem.Click += new System.EventHandler(this.fileMenuItem_Click);
            // 
            // connectMenuItem
            // 
            this.connectMenuItem.Text = "Connect";
            this.connectMenuItem.Click += new System.EventHandler(this.connectMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.Add(this.connectMenuItem2);
            this.contextMenu.MenuItems.Add(this.addMenuItem);
            this.contextMenu.MenuItems.Add(this.modifyMenuItem);
            this.contextMenu.MenuItems.Add(this.deleteMenuItem);
            this.contextMenu.MenuItems.Add(this.optionsMenuItem);
            this.contextMenu.MenuItems.Add(this.debugMenuItem);
            this.contextMenu.MenuItems.Add(this.exitMenuItem2);
            this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
            // 
            // connectMenuItem2
            // 
            this.connectMenuItem2.Text = "Connect";
            this.connectMenuItem2.Click += new System.EventHandler(this.connectMenuItem2_Click);
            // 
            // addMenuItem
            // 
            this.addMenuItem.Text = "Add...";
            this.addMenuItem.Click += new System.EventHandler(this.addMenuItem_Click);
            // 
            // modifyMenuItem
            // 
            this.modifyMenuItem.Text = "Modify...";
            this.modifyMenuItem.Click += new System.EventHandler(this.modifyMenuItem_Click);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Text = "Delete";
            this.deleteMenuItem.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // optionsMenuItem
            // 
            this.optionsMenuItem.Text = "Options...";
            this.optionsMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Click);
            // 
            // debugMenuItem
            // 
            this.debugMenuItem.Text = "Debug...";
            this.debugMenuItem.Click += new System.EventHandler(this.debugMenuItem_Click);
            // 
            // exitMenuItem2
            // 
            this.exitMenuItem2.Text = "Exit";
            this.exitMenuItem2.Click += new System.EventHandler(this.exitMenuItem2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(240, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // serverListBox
            // 
            this.serverListBox.ContextMenu = this.contextMenu;
            this.serverListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverListBox.LargeImageList = this.imageList;
            this.serverListBox.Location = new System.Drawing.Point(0, 50);
            this.serverListBox.Name = "serverListBox";
            this.serverListBox.Size = new System.Drawing.Size(240, 218);
            this.serverListBox.TabIndex = 2;
            this.serverListBox.SelectedIndexChanged += new System.EventHandler(this.serverListBox_SelectedIndexChanged);
            // 
            // imageList
            // 
            this.imageList.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList.Images.Clear();
            this.imageList.Images.Add(((System.Drawing.Icon)(resources.GetObject("resource"))));
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.serverListBox);
            this.Controls.Add(this.pictureBox1);
            this.Menu = this.mainMenu;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "IMI iWMS Thin Client";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem connectMenuItem;
        private System.Windows.Forms.MenuItem exitMenuItem;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem debugMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuItem addMenuItem;
        private System.Windows.Forms.MenuItem modifyMenuItem;
        private System.Windows.Forms.MenuItem deleteMenuItem;
        private System.Windows.Forms.ListView serverListBox;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.MenuItem optionsMenuItem;
        private System.Windows.Forms.MenuItem fileMenuItem;
        private System.Windows.Forms.MenuItem connectMenuItem2;
        private System.Windows.Forms.MenuItem exitMenuItem2;
    }
}

