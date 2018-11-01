using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Imi.Wms.Mobile.Server.UI.Controls;
using Imi.Wms.Mobile.Server.UI.Configuration;
using System.Collections.Generic;

namespace Imi.Wms.Mobile.Server.UI
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class ManagerMainForm : System.Windows.Forms.Form
    {

        private System.Windows.Forms.Panel TopPanel;
        private System.Windows.Forms.ImageList treeViewImageList;
        private System.Windows.Forms.ContextMenu notifyIconContextMenu;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.NotifyIcon ManagerNotifyIcon;
        private System.Windows.Forms.MenuItem ShowServerManagerItem;
        private System.Windows.Forms.Timer instanceTimeTimer;
        private ToolStrip toolStrip;
        private ToolStripButton killBtn;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton refreshBtn;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileItem;
        private ToolStripMenuItem fileChangeServerItem;
        private ToolStripMenuItem viewItem;
        private ToolStripMenuItem toolsItem;
        private ToolStripMenuItem helpItem;
        private ToolStripMenuItem fileDeleteNodeItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem fileConnectItem;
        private ToolStripMenuItem fileDisconnectItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem fileExitItem;
        private ToolStripMenuItem refreshNowItem;
        private ToolStripMenuItem updateSpeedItem;
        private ToolStripMenuItem highSpeedItem;
        private ToolStripMenuItem normalSpeedItem;
        private ToolStripMenuItem lowSpeedItem;
        private ToolStripMenuItem pausedSpeedItem;
        private ToolStripMenuItem findServerInstancesItem;
        private ToolStripMenuItem helpAboutItem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel localInstanceTimeLabel;
        private Panel CLeftPanel;
        private TreeView MgrTreeView;
        private Splitter CLeftRightSplitter;
        private Panel CCenterPanel;
        private ucEmpty EmptyView;
        private ucRuntimeView RuntimeView;
        private ContextMenuStrip serverContextMenuStrip;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem ConnectItem;
        private ToolStripMenuItem DisconnectItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem PropertiesItem;
        private ToolStripMenuItem RenameItem;
        private ToolStripMenuItem DeleteItem;
        private ContextMenuStrip ServerManagerContextMenuStrip;
        private ToolStripMenuItem NewMenuItem;
        private ToolStripMenuItem RenameNodeItem;
        private ToolStripMenuItem DeleteNodeItem;
        private ToolStripMenuItem NewFolderItem;
        private ToolStripMenuItem NewServerItem;
        private ToolStripMenuItem fileNewMenuItem;
        private ToolStripMenuItem fileNewFolderMenuItem;
        private ToolStripMenuItem fileNewServerItem;
        private ToolStripMenuItem fileRenameNodeMenuItem;
        private Panel mainPanel;

        public ManagerMainForm()
        {
            this.Hide();
            SplashForm splashForm = new SplashForm();
            Thread ft = new Thread(new ThreadStart(splashForm.Show));
                        
            ft.Start();
            Thread.Sleep(0); // Yield

            InitializeComponent();

            DateTime stop = DateTime.Now.AddSeconds(12);
            splashForm.Fade();
            Thread.Sleep(0); // Yield

            // The timepsan logic is needed for remote desktop
            // where the fade doesn't happen every time for
            // some reason and the splash does not disappear.
            while (splashForm.Opacity > 0)
            {
                Thread.Sleep(10);
                if (DateTime.Now > stop)
                    ft.Interrupt();
            }
            
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerMainForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Server Management", 0, 0);
            this.TopPanel = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.killBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshBtn = new System.Windows.Forms.ToolStripButton();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNewFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNewServerItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileRenameNodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileChangeServerItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileDeleteNodeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fileConnectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileDisconnectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.fileExitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshNowItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateSpeedItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highSpeedItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalSpeedItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowSpeedItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pausedSpeedItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findServerInstancesItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpAboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.ManagerNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenu = new System.Windows.Forms.ContextMenu();
            this.ShowServerManagerItem = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.instanceTimeTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.localInstanceTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.CLeftPanel = new System.Windows.Forms.Panel();
            this.MgrTreeView = new System.Windows.Forms.TreeView();
            this.serverContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.ConnectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisconnectItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.PropertiesItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CLeftRightSplitter = new System.Windows.Forms.Splitter();
            this.CCenterPanel = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.ServerManagerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFolderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewServerItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameNodeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteNodeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EmptyView = new Imi.Wms.Mobile.Server.UI.Controls.ucEmpty();
            this.RuntimeView = new Imi.Wms.Mobile.Server.UI.ucRuntimeView();
            this.TopPanel.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.CLeftPanel.SuspendLayout();
            this.serverContextMenuStrip.SuspendLayout();
            this.CCenterPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.ServerManagerContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.AutoSize = true;
            this.TopPanel.Controls.Add(this.toolStrip);
            this.TopPanel.Controls.Add(this.menuStrip);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(792, 49);
            this.TopPanel.TabIndex = 1;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.killBtn,
            this.toolStripSeparator1,
            this.refreshBtn});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(792, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip_ItemClicked);
            // 
            // killBtn
            // 
            this.killBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.killBtn.Image = ((System.Drawing.Image)(resources.GetObject("killBtn.Image")));
            this.killBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.killBtn.Name = "killBtn";
            this.killBtn.Size = new System.Drawing.Size(23, 22);
            this.killBtn.Text = "toolStripButton2";
            this.killBtn.ToolTipText = "Kill Session";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // refreshBtn
            // 
            this.refreshBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshBtn.Image")));
            this.refreshBtn.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(23, 22);
            this.refreshBtn.Text = "toolStripButton3";
            this.refreshBtn.ToolTipText = "Refresh Session List";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileItem,
            this.viewItem,
            this.toolsItem,
            this.helpItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(792, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileItem
            // 
            this.fileItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNewMenuItem,
            this.fileRenameNodeMenuItem,
            this.fileChangeServerItem,
            this.fileDeleteNodeItem,
            this.toolStripMenuItem1,
            this.fileConnectItem,
            this.fileDisconnectItem,
            this.toolStripMenuItem2,
            this.fileExitItem});
            this.fileItem.Name = "fileItem";
            this.fileItem.Size = new System.Drawing.Size(37, 20);
            this.fileItem.Text = "&File";
            this.fileItem.DropDownOpened += new System.EventHandler(this.FileMenu_Popup);
            // 
            // fileNewMenuItem
            // 
            this.fileNewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNewFolderMenuItem,
            this.fileNewServerItem});
            this.fileNewMenuItem.Name = "fileNewMenuItem";
            this.fileNewMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fileNewMenuItem.Text = "New";
            // 
            // fileNewFolderMenuItem
            // 
            this.fileNewFolderMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileNewFolderMenuItem.Image")));
            this.fileNewFolderMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.fileNewFolderMenuItem.Name = "fileNewFolderMenuItem";
            this.fileNewFolderMenuItem.Size = new System.Drawing.Size(115, 22);
            this.fileNewFolderMenuItem.Text = "&Folder";
            this.fileNewFolderMenuItem.Click += new System.EventHandler(this.NewDirectoryMenuItem_Click);
            // 
            // fileNewServerItem
            // 
            this.fileNewServerItem.Image = ((System.Drawing.Image)(resources.GetObject("fileNewServerItem.Image")));
            this.fileNewServerItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.fileNewServerItem.Name = "fileNewServerItem";
            this.fileNewServerItem.Size = new System.Drawing.Size(115, 22);
            this.fileNewServerItem.Text = "&Server...";
            this.fileNewServerItem.Click += new System.EventHandler(this.NewServerMenuItem_Click);
            // 
            // fileRenameNodeMenuItem
            // 
            this.fileRenameNodeMenuItem.Name = "fileRenameNodeMenuItem";
            this.fileRenameNodeMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.fileRenameNodeMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fileRenameNodeMenuItem.Text = "Rename";
            this.fileRenameNodeMenuItem.Click += new System.EventHandler(this.RenameNodeMenuItem_Click);
            // 
            // fileChangeServerItem
            // 
            this.fileChangeServerItem.Image = ((System.Drawing.Image)(resources.GetObject("fileChangeServerItem.Image")));
            this.fileChangeServerItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.fileChangeServerItem.Name = "fileChangeServerItem";
            this.fileChangeServerItem.Size = new System.Drawing.Size(152, 22);
            this.fileChangeServerItem.Text = "Properties...";
            this.fileChangeServerItem.Click += new System.EventHandler(this.ChangeServerMenuItem_Click);
            // 
            // fileDeleteNodeItem
            // 
            this.fileDeleteNodeItem.Image = ((System.Drawing.Image)(resources.GetObject("fileDeleteNodeItem.Image")));
            this.fileDeleteNodeItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.fileDeleteNodeItem.Name = "fileDeleteNodeItem";
            this.fileDeleteNodeItem.Size = new System.Drawing.Size(152, 22);
            this.fileDeleteNodeItem.Text = "&Delete...";
            this.fileDeleteNodeItem.Click += new System.EventHandler(this.DeleteNodeMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // fileConnectItem
            // 
            this.fileConnectItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.fileConnectItem.Name = "fileConnectItem";
            this.fileConnectItem.Size = new System.Drawing.Size(152, 22);
            this.fileConnectItem.Text = "C&onnect";
            this.fileConnectItem.Click += new System.EventHandler(this.ConnectItem_Click);
            // 
            // fileDisconnectItem
            // 
            this.fileDisconnectItem.Name = "fileDisconnectItem";
            this.fileDisconnectItem.Size = new System.Drawing.Size(152, 22);
            this.fileDisconnectItem.Text = "D&isconnect";
            this.fileDisconnectItem.Click += new System.EventHandler(this.DisconnectItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // fileExitItem
            // 
            this.fileExitItem.Name = "fileExitItem";
            this.fileExitItem.Size = new System.Drawing.Size(152, 22);
            this.fileExitItem.Text = "E&xit";
            this.fileExitItem.Click += new System.EventHandler(this.ExitItem_Click);
            // 
            // viewItem
            // 
            this.viewItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshNowItem,
            this.updateSpeedItem});
            this.viewItem.Name = "viewItem";
            this.viewItem.Size = new System.Drawing.Size(44, 20);
            this.viewItem.Text = "&View";
            // 
            // refreshNowItem
            // 
            this.refreshNowItem.Image = ((System.Drawing.Image)(resources.GetObject("refreshNowItem.Image")));
            this.refreshNowItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.refreshNowItem.Name = "refreshNowItem";
            this.refreshNowItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshNowItem.Size = new System.Drawing.Size(160, 22);
            this.refreshNowItem.Text = "&Refresh Now";
            this.refreshNowItem.Click += new System.EventHandler(this.RefreshNowItem_Click);
            // 
            // updateSpeedItem
            // 
            this.updateSpeedItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highSpeedItem,
            this.normalSpeedItem,
            this.lowSpeedItem,
            this.pausedSpeedItem});
            this.updateSpeedItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.updateSpeedItem.Name = "updateSpeedItem";
            this.updateSpeedItem.Size = new System.Drawing.Size(160, 22);
            this.updateSpeedItem.Text = "&Update Speed";
            this.updateSpeedItem.DropDownOpened += new System.EventHandler(this.UpdateSpeedItem_Popup);
            // 
            // highSpeedItem
            // 
            this.highSpeedItem.Name = "highSpeedItem";
            this.highSpeedItem.Size = new System.Drawing.Size(114, 22);
            this.highSpeedItem.Text = "&High";
            this.highSpeedItem.Click += new System.EventHandler(this.SpeedItem_Click);
            // 
            // normalSpeedItem
            // 
            this.normalSpeedItem.Name = "normalSpeedItem";
            this.normalSpeedItem.Size = new System.Drawing.Size(114, 22);
            this.normalSpeedItem.Text = "&Normal";
            this.normalSpeedItem.Click += new System.EventHandler(this.SpeedItem_Click);
            // 
            // lowSpeedItem
            // 
            this.lowSpeedItem.Name = "lowSpeedItem";
            this.lowSpeedItem.Size = new System.Drawing.Size(114, 22);
            this.lowSpeedItem.Text = "&Low";
            this.lowSpeedItem.Click += new System.EventHandler(this.SpeedItem_Click);
            // 
            // pausedSpeedItem
            // 
            this.pausedSpeedItem.Name = "pausedSpeedItem";
            this.pausedSpeedItem.Size = new System.Drawing.Size(114, 22);
            this.pausedSpeedItem.Text = "&Paused";
            this.pausedSpeedItem.Click += new System.EventHandler(this.SpeedItem_Click);
            // 
            // toolsItem
            // 
            this.toolsItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findServerInstancesItem});
            this.toolsItem.Name = "toolsItem";
            this.toolsItem.Size = new System.Drawing.Size(48, 20);
            this.toolsItem.Text = "&Tools";
            this.toolsItem.Visible = false;
            // 
            // findServerInstancesItem
            // 
            this.findServerInstancesItem.Image = ((System.Drawing.Image)(resources.GetObject("findServerInstancesItem.Image")));
            this.findServerInstancesItem.Name = "findServerInstancesItem";
            this.findServerInstancesItem.Size = new System.Drawing.Size(193, 22);
            this.findServerInstancesItem.Text = "&Find Server Instances...";
            // 
            // helpItem
            // 
            this.helpItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpAboutItem});
            this.helpItem.Name = "helpItem";
            this.helpItem.Size = new System.Drawing.Size(44, 20);
            this.helpItem.Text = "&Help";
            // 
            // helpAboutItem
            // 
            this.helpAboutItem.Image = ((System.Drawing.Image)(resources.GetObject("helpAboutItem.Image")));
            this.helpAboutItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.helpAboutItem.Name = "helpAboutItem";
            this.helpAboutItem.Size = new System.Drawing.Size(201, 22);
            this.helpAboutItem.Text = "&About Server Manager...";
            this.helpAboutItem.Click += new System.EventHandler(this.AboutItem_Click);
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.treeViewImageList.Images.SetKeyName(0, "servers.ico");
            this.treeViewImageList.Images.SetKeyName(1, "Computer.ico");
            this.treeViewImageList.Images.SetKeyName(2, "VSFolder_closed.bmp");
            this.treeViewImageList.Images.SetKeyName(3, "VSFolder_open.bmp");
            // 
            // ManagerNotifyIcon
            // 
            this.ManagerNotifyIcon.ContextMenu = this.notifyIconContextMenu;
            this.ManagerNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ManagerNotifyIcon.Icon")));
            this.ManagerNotifyIcon.Text = "MgrNotifyIcon";
            // 
            // notifyIconContextMenu
            // 
            this.notifyIconContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ShowServerManagerItem,
            this.menuItem16});
            // 
            // ShowServerManagerItem
            // 
            this.ShowServerManagerItem.Index = 0;
            this.ShowServerManagerItem.Text = "Show Server Manager";
            this.ShowServerManagerItem.Click += new System.EventHandler(this.ShowServerManagerItem_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 1;
            this.menuItem16.Text = "Exit Server Manager";
            this.menuItem16.Click += new System.EventHandler(this.ExitServerManager_Click);
            // 
            // instanceTimeTimer
            // 
            this.instanceTimeTimer.Enabled = true;
            this.instanceTimeTimer.Interval = 1000;
            this.instanceTimeTimer.Tick += new System.EventHandler(this.InstanceTimeTimer_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localInstanceTimeLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 450);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(792, 22);
            this.statusStrip.TabIndex = 13;
            this.statusStrip.Text = "statusStrip";
            // 
            // localInstanceTimeLabel
            // 
            this.localInstanceTimeLabel.Name = "localInstanceTimeLabel";
            this.localInstanceTimeLabel.Size = new System.Drawing.Size(10, 17);
            this.localInstanceTimeLabel.Text = " ";
            // 
            // CLeftPanel
            // 
            this.CLeftPanel.Controls.Add(this.MgrTreeView);
            this.CLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.CLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.CLeftPanel.Name = "CLeftPanel";
            this.CLeftPanel.Size = new System.Drawing.Size(192, 401);
            this.CLeftPanel.TabIndex = 0;
            // 
            // MgrTreeView
            // 
            this.MgrTreeView.AllowDrop = true;
            this.MgrTreeView.BackColor = System.Drawing.SystemColors.Window;
            this.MgrTreeView.ContextMenuStrip = this.serverContextMenuStrip;
            this.MgrTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MgrTreeView.FullRowSelect = true;
            this.MgrTreeView.HideSelection = false;
            this.MgrTreeView.HotTracking = true;
            this.MgrTreeView.ImageIndex = 0;
            this.MgrTreeView.ImageList = this.treeViewImageList;
            this.MgrTreeView.LabelEdit = true;
            this.MgrTreeView.Location = new System.Drawing.Point(0, 0);
            this.MgrTreeView.Name = "MgrTreeView";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "Server Management";
            this.MgrTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.MgrTreeView.SelectedImageIndex = 0;
            this.MgrTreeView.Size = new System.Drawing.Size(192, 401);
            this.MgrTreeView.TabIndex = 0;
            this.MgrTreeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.MgrTreeView_BeforeLabelEdit);
            this.MgrTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.MgrTreeView_AfterLabelEdit);
            this.MgrTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.MgrTreeView_BeforeCollapse);
            this.MgrTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.MgrTreeView_BeforeExpand);
            this.MgrTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.MgrTreeView_ItemDrag);
            this.MgrTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MgrTreeView_AfterSelect);
            this.MgrTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.MgrTreeView_DragDrop);
            this.MgrTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.MgrTreeView_DragOver);
            this.MgrTreeView.DoubleClick += new System.EventHandler(this.ConnectItem_Click);
            this.MgrTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MgrTreeView_MouseDown);
            // 
            // serverContextMenuStrip
            // 
            this.serverContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.ConnectItem,
            this.DisconnectItem,
            this.toolStripMenuItem4,
            this.PropertiesItem,
            this.RenameItem,
            this.DeleteItem});
            this.serverContextMenuStrip.Name = "contextMenuStrip1";
            this.serverContextMenuStrip.Size = new System.Drawing.Size(137, 126);
            this.serverContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.serverContextMenuStrip_Opening);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(133, 6);
            // 
            // ConnectItem
            // 
            this.ConnectItem.Name = "ConnectItem";
            this.ConnectItem.Size = new System.Drawing.Size(136, 22);
            this.ConnectItem.Text = "&Connect";
            this.ConnectItem.Click += new System.EventHandler(this.ConnectItem_Click);
            // 
            // DisconnectItem
            // 
            this.DisconnectItem.Name = "DisconnectItem";
            this.DisconnectItem.Size = new System.Drawing.Size(136, 22);
            this.DisconnectItem.Text = "&Disconnect";
            this.DisconnectItem.Click += new System.EventHandler(this.DisconnectItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(133, 6);
            // 
            // PropertiesItem
            // 
            this.PropertiesItem.Image = ((System.Drawing.Image)(resources.GetObject("PropertiesItem.Image")));
            this.PropertiesItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.PropertiesItem.Name = "PropertiesItem";
            this.PropertiesItem.Size = new System.Drawing.Size(136, 22);
            this.PropertiesItem.Text = "&Properties...";
            this.PropertiesItem.Click += new System.EventHandler(this.ChangeServerMenuItem_Click);
            // 
            // RenameItem
            // 
            this.RenameItem.Name = "RenameItem";
            this.RenameItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.RenameItem.Size = new System.Drawing.Size(136, 22);
            this.RenameItem.Text = "&Rename";
            this.RenameItem.Click += new System.EventHandler(this.RenameNodeMenuItem_Click);
            // 
            // DeleteItem
            // 
            this.DeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteItem.Image")));
            this.DeleteItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.DeleteItem.Name = "DeleteItem";
            this.DeleteItem.Size = new System.Drawing.Size(136, 22);
            this.DeleteItem.Text = "De&lete...";
            this.DeleteItem.Click += new System.EventHandler(this.DeleteNodeMenuItem_Click);
            // 
            // CLeftRightSplitter
            // 
            this.CLeftRightSplitter.Location = new System.Drawing.Point(192, 0);
            this.CLeftRightSplitter.Name = "CLeftRightSplitter";
            this.CLeftRightSplitter.Size = new System.Drawing.Size(3, 401);
            this.CLeftRightSplitter.TabIndex = 1;
            this.CLeftRightSplitter.TabStop = false;
            // 
            // CCenterPanel
            // 
            this.CCenterPanel.Controls.Add(this.EmptyView);
            this.CCenterPanel.Controls.Add(this.RuntimeView);
            this.CCenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CCenterPanel.Location = new System.Drawing.Point(195, 0);
            this.CCenterPanel.Name = "CCenterPanel";
            this.CCenterPanel.Size = new System.Drawing.Size(597, 401);
            this.CCenterPanel.TabIndex = 2;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.CCenterPanel);
            this.mainPanel.Controls.Add(this.CLeftRightSplitter);
            this.mainPanel.Controls.Add(this.CLeftPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 49);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(792, 401);
            this.mainPanel.TabIndex = 2;
            // 
            // ServerManagerContextMenuStrip
            // 
            this.ServerManagerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewMenuItem,
            this.RenameNodeItem,
            this.DeleteNodeItem});
            this.ServerManagerContextMenuStrip.Name = "serverManagerContextMenuStrip";
            this.ServerManagerContextMenuStrip.Size = new System.Drawing.Size(118, 70);
            this.ServerManagerContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ServerManagerContextMenuStrip_Opening);
            // 
            // NewMenuItem
            // 
            this.NewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFolderItem,
            this.NewServerItem});
            this.NewMenuItem.Name = "NewMenuItem";
            this.NewMenuItem.Size = new System.Drawing.Size(117, 22);
            this.NewMenuItem.Text = "&New";
            // 
            // NewFolderItem
            // 
            this.NewFolderItem.Image = ((System.Drawing.Image)(resources.GetObject("NewFolderItem.Image")));
            this.NewFolderItem.ImageTransparentColor = System.Drawing.Color.White;
            this.NewFolderItem.Name = "NewFolderItem";
            this.NewFolderItem.Size = new System.Drawing.Size(115, 22);
            this.NewFolderItem.Text = "&Folder";
            this.NewFolderItem.Click += new System.EventHandler(this.NewDirectoryMenuItem_Click);
            // 
            // NewServerItem
            // 
            this.NewServerItem.Image = ((System.Drawing.Image)(resources.GetObject("NewServerItem.Image")));
            this.NewServerItem.ImageTransparentColor = System.Drawing.Color.White;
            this.NewServerItem.Name = "NewServerItem";
            this.NewServerItem.Size = new System.Drawing.Size(115, 22);
            this.NewServerItem.Text = "&Server...";
            this.NewServerItem.Click += new System.EventHandler(this.NewServerMenuItem_Click);
            // 
            // RenameNodeItem
            // 
            this.RenameNodeItem.Name = "RenameNodeItem";
            this.RenameNodeItem.Size = new System.Drawing.Size(117, 22);
            this.RenameNodeItem.Text = "&Rename";
            this.RenameNodeItem.Click += new System.EventHandler(this.RenameNodeMenuItem_Click);
            // 
            // DeleteNodeItem
            // 
            this.DeleteNodeItem.Image = ((System.Drawing.Image)(resources.GetObject("DeleteNodeItem.Image")));
            this.DeleteNodeItem.ImageTransparentColor = System.Drawing.Color.White;
            this.DeleteNodeItem.Name = "DeleteNodeItem";
            this.DeleteNodeItem.Size = new System.Drawing.Size(117, 22);
            this.DeleteNodeItem.Text = "De&lete...";
            this.DeleteNodeItem.Click += new System.EventHandler(this.DeleteNodeMenuItem_Click);
            // 
            // EmptyView
            // 
            this.EmptyView.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.EmptyView.ContextMenuStrip = this.serverContextMenuStrip;
            this.EmptyView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmptyView.Location = new System.Drawing.Point(0, 0);
            this.EmptyView.Name = "EmptyView";
            this.EmptyView.Size = new System.Drawing.Size(597, 401);
            this.EmptyView.TabIndex = 4;
            // 
            // RuntimeView
            // 
            this.RuntimeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RuntimeView.Location = new System.Drawing.Point(0, 0);
            this.RuntimeView.Name = "RuntimeView";
            this.RuntimeView.Size = new System.Drawing.Size(597, 401);
            this.RuntimeView.TabIndex = 6;
            // 
            // ManagerMainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 472);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.TopPanel);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ManagerMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IMI iWMS Thin Client Server Manager";
            this.Activated += new System.EventHandler(this.ManagerMainForm_Activated);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.CLeftPanel.ResumeLayout(false);
            this.serverContextMenuStrip.ResumeLayout(false);
            this.CCenterPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.ServerManagerContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new ManagerMainForm());
        }

        private ManagedConnection CurrentConnection
        {
            get
            {
                TreeNode n = MgrTreeView.SelectedNode;

                if (n.Tag is ManagedConnection)
                {
                    return (n.Tag as ManagedConnection);
                }
                else
                    return (null);
            }
        }

        private void ExitItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        
        private void ShowServerManagerItem_Click(object sender, System.EventArgs e)
        {
            if (ManagerNotifyIcon.Visible)
            {
                ManagerNotifyIcon.Visible = false;
                Show();
                WindowState = FormWindowState.Normal;
                BringToFront();
                Focus();
                RuntimeView.ResumeRefresh();
            }
        }

        private void ExitServerManager_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private bool      first = true;
        ManagedConnection currentViewConnection = null;

        private void PopulateTreeView()
        {
            MgrTreeView.Nodes[0].Nodes.Clear();
            ManagedConnection.ConnectStateChanged = null;

            List <TreeNode> n = TreeNodeFactory.CreateTreeNodeChildList(UIConfigFileHandler.Instance().Config.RootFolder);

            if (n.Count > 0)
            {
                MgrTreeView.Nodes[0].Nodes.AddRange(n.ToArray());
                ManagedConnection.ConnectStateChanged += new ConnectStateEventDelegate(this.ConnectStateChanged);
            }

        }

        private void ManagerMainForm_Activated(object sender, System.EventArgs e)
        {
            if (first)
            {
                first = false;

                this.TopMost = true;
                this.Refresh();
                this.TopMost = false;

                try
                {
                    PopulateTreeView();
                    MgrTreeView.Nodes[0].Expand();
                    EmptyView.BringToFront();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }

            }
        }
        
        public void ConnectStateChanged(ManagedConnection sender, bool connected)
        {
            if (CurrentConnection != null)
            {
                if (CurrentConnection == sender)
                {
                    currentViewConnection = null;
                    MgrTreeView_AfterSelect(sender, null);
                }
            }
        }

        private void ConnectItem_Click(object sender, System.EventArgs e)
        {
            // Find what node is selected and if it's connected or not
            if (CurrentConnection != null)
            {
                if (!(CurrentConnection.Connected))
                {
                    this.Refresh();
                    Cursor oldCursor = this.Cursor;

                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        CurrentConnection.Connect();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format("Failed to connect to {0}\n{1}", CurrentConnection.Config.DisplayName, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = oldCursor;
                    }
                }
            }
        }

        private void DisconnectItem_Click(object sender, System.EventArgs e)
        {
            // Find what node is selected and if it's connected or not
            if (CurrentConnection != null)
            {
                CurrentConnection.Disconnect();
            }
        }

        #region TreeViewFixup

        /// <summary>
        /// Looks for a node within a TreeNode structure that contains
        /// the X,Y coordinate and when found makes it the selected node.
        /// This code is needed due to the fact that the Selected property
        /// of the TreeView is not updated when right clicking it.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void HighlightNode(TreeNode node, int X, int Y)
        {
            if (node.Bounds.Contains(X, Y))
            {
                MgrTreeView.SelectedNode = node;
            }
            else
            {
                foreach (TreeNode n in node.Nodes)
                {
                    HighlightNode(n, X, Y);
                }
            }
        }

        /// <summary>
        /// See HighlightNode above for comments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MgrTreeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lastClickedNode = MgrTreeView.SelectedNode;
            HighlightNode(MgrTreeView.TopNode, e.X, e.Y);
        }
        #endregion

        private void MgrTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            killBtn.Enabled = false;
            refreshBtn.Enabled = false;

            if (CurrentConnection != null)
            {
                MgrTreeView.ContextMenuStrip = serverContextMenuStrip;

                if (currentViewConnection != CurrentConnection)
                {
                    currentViewConnection = CurrentConnection;

                    if (CurrentConnection.Connected)
                    {
                        RuntimeView.ShowView(CurrentConnection);
                    }
                    else
                    {
                        EmptyView.BringToFront();
                    }
                }
            }
            else
            {
                MgrTreeView.ContextMenuStrip = ServerManagerContextMenuStrip;
            }

            if (CurrentConnection != null)
            {
                killBtn.Enabled = CurrentConnection.Connected;
                refreshBtn.Enabled = killBtn.Enabled;
            }

            // Don't allow edit of root node
            MgrTreeView.LabelEdit = (MgrTreeView.SelectedNode.Parent != null);
            if (MgrTreeView.LabelEdit)
            {
                // Don't allow edit of connected node
                TreeNode n = MgrTreeView.SelectedNode;

                if (n.Tag is ManagedConnection)
                {
                    ManagedConnection mc = n.Tag as ManagedConnection;
                    MgrTreeView.LabelEdit = (mc.Connected == false);
                }
            }

        }
        
        private void SaveChanges()
        {
            FolderType f = FolderFactory.CreateFolder(MgrTreeView);
            UIConfigFileHandler.Instance().Config.RootFolder = f;
            UIConfigFileHandler.Instance().Save();
        }

        private void ChangeApplyBtn_Click(object sender, System.EventArgs e)
        {
            MgrTreeView.SelectedNode.Text = CurrentConnection.Config.DisplayName;
            SaveChanges();
        }

        private void ChangeServerMenuItem_Click(object sender, System.EventArgs e)
        {
            ServerModify sm = new ServerModify(CurrentConnection);

            sm.ApplyButton.Click += new System.EventHandler(this.ChangeApplyBtn_Click);

            DialogResult cc = sm.ShowDialog();
            switch (cc)
            {
                case DialogResult.OK:
                    MgrTreeView.SelectedNode.Text = CurrentConnection.Config.DisplayName;
                    SaveChanges();
                    break;
            }
        }


        private void AboutItem_Click(object sender, System.EventArgs e)
        {
            AboutForm a = new AboutForm();
            a.ShowDialog(this);
        }

        private void RefreshNowItem_Click(object sender, System.EventArgs e)
        {
            if (currentViewConnection != null)
            {
                if (currentViewConnection.Connected)
                {
                    RuntimeView.RefreshView(false);
                }
            }
        }

        private void UpdateSpeedItem_Popup(object sender, System.EventArgs e)
        {
            ServerConfigRefreshRate r = UIConfigFileHandler.Instance().Config.RefreshRate;

            foreach (ToolStripMenuItem mi in updateSpeedItem.DropDownItems)
                mi.Checked = false;

            switch (r)
            {
                case ServerConfigRefreshRate.High:
                    highSpeedItem.Checked = true;
                    break;
                case ServerConfigRefreshRate.Low:
                    lowSpeedItem.Checked = true;
                    break;
                case ServerConfigRefreshRate.Normal:
                    normalSpeedItem.Checked = true;
                    break;
                case ServerConfigRefreshRate.Paused:
                    pausedSpeedItem.Checked = true;
                    break;
            }
        }

        private void SpeedItem_Click(object sender, System.EventArgs e)
        {
            if (sender is ToolStripItem)
            {
                ToolStripMenuItem m = (sender as ToolStripMenuItem);

                if (!m.Checked)
                {
                    foreach (ToolStripMenuItem mi in updateSpeedItem.DropDownItems)
                        mi.Checked = false;

                    m.Checked = true;
                    ServerConfig conf = UIConfigFileHandler.Instance().Config;

                    if (m == highSpeedItem)
                    {
                        conf.RefreshRate = ServerConfigRefreshRate.High;
                    }

                    if (m == lowSpeedItem)
                    {
                        conf.RefreshRate = ServerConfigRefreshRate.Low;
                    }

                    if (m == normalSpeedItem)
                    {
                        conf.RefreshRate = ServerConfigRefreshRate.Normal;
                    }

                    if (m == pausedSpeedItem)
                    {
                        conf.RefreshRate = ServerConfigRefreshRate.Paused;
                    }

                    UIConfigFileHandler.Instance().Save();

                    if (CurrentConnection != null)
                    {
                        if (CurrentConnection.Connected)
                        {
                            RuntimeView.RefreshView(false);
                        }
                    }
                }
            }

        }

        private void InstanceTimeTimer_Tick(object sender, System.EventArgs e)
        {
            if (CurrentConnection != null)
            {
                TimeSpan timeDiff = CurrentConnection.GetInstanceTimeDiff();
                localInstanceTimeLabel.Text = DateTime.Now.Add(timeDiff).ToString();
            }
            else
                localInstanceTimeLabel.Text = DateTime.Now.ToString();
        }


        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == killBtn)
                RuntimeView.KillSession();

            if (e.ClickedItem == refreshBtn)
                RuntimeView.RefreshView(false);
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode n = MgrTreeView.SelectedNode;

            if (n.Tag is ManagedConnection)
            {
                if (MessageBox.Show(this, string.Format("Do you really want to remove {0} ?", n.Text), "Remove Instance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TreeNode nu = n.PrevNode;

                    if (nu == null)
                        nu = n.Parent;

                    MgrTreeView.SelectedNode.Remove();
                    MgrTreeView.SelectedNode = nu;
                    SaveChanges();

                    currentViewConnection = null;
                    MgrTreeView_AfterSelect(sender, null);
                }
            }
        }


        private void AddConnection(ManagedConnection c)
        {
            currentViewConnection = c;
            TreeNode t = TreeNodeFactory.CreateServerNode(c);
            // Add to treeview
            MgrTreeView.SelectedNode.Nodes.Add(t);
            MgrTreeView.SelectedNode = t;
        }

        private ManagedConnection newConn;
        private ManagedConnection oldConn;

        private void NewApplyBtn_Click(object sender, System.EventArgs e)
        {
            AddConnection(newConn);
            SaveChanges();
        }

        private void NewServerMenuItem_Click(object sender, EventArgs e)
        {
            oldConn = currentViewConnection;
            newConn = ManagedConnection.CreateDefaultConnection();
            currentViewConnection = newConn;

            ServerModify sm = new ServerModify("New Server", currentViewConnection);
            sm.ApplyButton.Click += new System.EventHandler(this.NewApplyBtn_Click);

            DialogResult cc = sm.ShowDialog();

            switch (cc)
            {
                case DialogResult.OK:
                    AddConnection(newConn);
                    SaveChanges();
                    break;
                case DialogResult.Cancel:
                    currentViewConnection = oldConn;
                    break;
            }
        }


        private TreeNode[] FindNodes(string text,TreeNodeCollection nodes)
        {
            List<TreeNode> list = new List<TreeNode>();

            foreach (TreeNode t in nodes)
            {
                if (t.Text == text)
                    list.Add(t);
            }

            return (list.ToArray());
        }

        private TreeNode AddMakeUnique(TreeNode dest, TreeNode src)
        {
            string key = src.Text;
            string baseKey = key;

            int idx = 2;
            while (FindNodes(key, dest.Nodes).Length > 0)
            {
                key = string.Format("{0} ({1})", baseKey, idx);
                idx++;
            }

            src.Text = key;
            dest.Nodes.Add(src);

            return (src);
        }

        private void NewDirectoryMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode t = AddMakeUnique(MgrTreeView.SelectedNode, new TreeNode("New Folder",  2, 2));
            MgrTreeView.SelectedNode.Expand();
            MgrTreeView.SelectedNode = t;
            lastClickedNode = t;
            SaveChanges();
            t.BeginEdit();
        }

        private void RenameNodeMenuItem_Click(object sender, EventArgs e)
        {
            forceEdit = true;
            if (MgrTreeView.LabelEdit)
                MgrTreeView.SelectedNode.BeginEdit();
            else
                forceEdit = false;

        }

        private void MgrTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                // Check for duplicates
                TreeNode p = e.Node.Parent;
                TreeNode[] ta = FindNodes(e.Label, p.Nodes);

                if (ta.Length > 0)
                {
                    MessageBox.Show(string.Format("Cannot rename {0}: A server or folder with the name you specified already exists. Specify a different name.", e.Node.Text),
                                    "Error Renaming Server or Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CancelEdit = true;
                    e.Node.BeginEdit();
                }
                else
                {
                    MgrTreeView.SelectedNode.Text = e.Label; // to make save work
                    SaveChanges();
                }
            }
        }

        private bool NodeHasActiveConnection(TreeNode t)
        {
            if (t.Tag is ManagedConnection)
            {
                ManagedConnection c = t.Tag as ManagedConnection;
                return (c.Connected);
            }
            else
            {
                foreach (TreeNode n in t.Nodes)
                {
                    if (NodeHasActiveConnection(n))
                    {
                        return (true);
                    }
                }
            }

            return (false);
        }

        private void DeleteNodeMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode n = MgrTreeView.SelectedNode;

            if (n.Tag is ManagedConnection)
            {
                if (MessageBox.Show(this, string.Format("Do you really want to remove {0} ?", n.Text), "Remove Instance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TreeNode nu = n.PrevNode;

                    if (nu == null)
                        nu = n.Parent;

                    MgrTreeView.SelectedNode.Remove();
                    MgrTreeView.SelectedNode = nu;
                    SaveChanges();

                    currentViewConnection = null;
                    MgrTreeView_AfterSelect(sender, null);
                }
            }
            else
            {
                if (MessageBox.Show(this, "Do you really want to remove this folder ? All sub folders and/or servers will be removed as well.", "Remove Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TreeNode nu = n.PrevNode;

                    if (nu == null)
                        nu = n.Parent;

                    if (NodeHasActiveConnection(MgrTreeView.SelectedNode))
                    {
                        MessageBox.Show(this, string.Format("The folder {0} or one of it's sub folders contains an active connection, please Disconnect it and try again.",MgrTreeView.SelectedNode.Text), "Remove Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MgrTreeView.SelectedNode.Remove();
                        MgrTreeView.SelectedNode = nu;
                        SaveChanges();

                        currentViewConnection = null;
                        MgrTreeView_AfterSelect(sender, null);
                    }

                }
            }
        }

        private void MgrTreeView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            Point Position = MgrTreeView.PointToClient(new Point(e.X, e.Y));
            TreeNode n = this.MgrTreeView.GetNodeAt(Position);
            if (n != null)
            {
                MgrTreeView.SelectedNode = n;
                if (!(n.Tag is ManagedConnection))
                {
                    // todo add code so we cannot drop on current parent
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void MgrTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Start the Drag Operation
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void MgrTreeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode src = e.Data.GetData("System.Windows.Forms.TreeNode") as TreeNode;
            src.Parent.Nodes.Remove(src);
            TreeNode dest =  MgrTreeView.SelectedNode;
            src = AddMakeUnique(dest, src);
            SaveChanges();
            MgrTreeView.SelectedNode = src;
        }

        private void FileMenu_Popup(object sender, System.EventArgs e)
        {
            fileNewMenuItem.Enabled = true;
            fileRenameNodeMenuItem.Enabled = false;
            fileChangeServerItem.Enabled = false;
            fileDeleteNodeItem.Enabled = false;
            fileConnectItem.Enabled = false;
            fileDisconnectItem.Enabled = false;

            // Only allow if parent is not connection
            fileNewServerItem.Enabled = (!(MgrTreeView.SelectedNode.Tag is ManagedConnection));
            fileNewFolderMenuItem.Enabled = fileNewServerItem.Enabled;

            if (CurrentConnection != null)
            {
                fileDisconnectItem.Enabled = CurrentConnection.Connected;
                fileConnectItem.Enabled = (!fileDisconnectItem.Enabled);
                fileChangeServerItem.Enabled = fileConnectItem.Enabled;
                fileRenameNodeMenuItem.Enabled = fileConnectItem.Enabled;
                fileDeleteNodeItem.Enabled = fileConnectItem.Enabled;
            }
            else
            {
                fileRenameNodeMenuItem.Enabled = (MgrTreeView.SelectedNode.Parent != null);
                fileDeleteNodeItem.Enabled = fileRenameNodeMenuItem.Enabled;
            }
        }

        private void serverContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            foreach (ToolStripItem t in cms.Items)
            {
                t.Enabled = false;
            }

            if (CurrentConnection != null)
            {
                DisconnectItem.Enabled = CurrentConnection.Connected;
                ConnectItem.Enabled = (!DisconnectItem.Enabled);
                PropertiesItem.Enabled = ConnectItem.Enabled;
                DeleteItem.Enabled = ConnectItem.Enabled;
                RenameItem.Enabled = ConnectItem.Enabled;
            }
        }

        private void ServerManagerContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            foreach (ToolStripItem t in cms.Items)
            {
                t.Enabled = false;
            }

            // Don't allow edit or delete of root node
            NewMenuItem.Enabled = true;
            RenameNodeItem.Enabled = (MgrTreeView.SelectedNode.Parent != null);
            DeleteNodeItem.Enabled = (MgrTreeView.SelectedNode.Parent != null);
        }

        private TreeNode lastClickedNode;
        private bool forceEdit;

        private void MgrTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if ((forceEdit) || (MgrTreeView.SelectedNode == lastClickedNode))
            {
                forceEdit = false;
                lastClickedNode = null;
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void MgrTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.ImageIndex == 3)
            {
                MgrTreeView.SelectedNode = e.Node;
                MgrTreeView.SelectedNode.ImageIndex = 2;
                MgrTreeView.SelectedNode.SelectedImageIndex = 2;
            }
        }

        private void MgrTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.ImageIndex == 2)
            {
                MgrTreeView.SelectedNode = e.Node;
                MgrTreeView.SelectedNode.ImageIndex = 3;
                MgrTreeView.SelectedNode.SelectedImageIndex = 3;
            }

        }
               

    }
}
