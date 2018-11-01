using Cdc.MetaManager.GUI;

namespace CrossHost
{
    partial class XamlViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XamlViewerForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.RenderTabControl = new System.Windows.Forms.TabControl();
            this.xamlViewerTab = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.ElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.sourceTab = new System.Windows.Forms.TabPage();
            this.XamlTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.viewTreeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelConnectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeComponentTypeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapComponentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.componentMapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRuleSetItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignHintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expanderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addExpanderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeExpanderItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGroupboxItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeGroupboxItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpToActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToGroupboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDisplayFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDefaultValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.componentLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel12 = new System.Windows.Forms.Panel();
            this.lvDatasources = new ListViewSort();
            this.chDSId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDSName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDSServiceMethod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel11 = new System.Windows.Forms.Panel();
            this.dataSourceToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbDSAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDSEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDSDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDSEditRequestMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDSEditResponseMap = new System.Windows.Forms.ToolStripButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.viewToolStrip = new System.Windows.Forms.ToolStrip();
            this.upBtn = new System.Windows.Forms.ToolStripButton();
            this.downBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.editGroupboxLayout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshBtn = new System.Windows.Forms.ToolStripButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.RenderTabControl.SuspendLayout();
            this.xamlViewerTab.SuspendLayout();
            this.sourceTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel9.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel11.SuspendLayout();
            this.dataSourceToolStrip.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.viewToolStrip.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // RenderTabControl
            // 
            this.RenderTabControl.Controls.Add(this.xamlViewerTab);
            this.RenderTabControl.Controls.Add(this.sourceTab);
            this.RenderTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderTabControl.Location = new System.Drawing.Point(0, 0);
            this.RenderTabControl.Margin = new System.Windows.Forms.Padding(8);
            this.RenderTabControl.Name = "RenderTabControl";
            this.RenderTabControl.SelectedIndex = 0;
            this.RenderTabControl.Size = new System.Drawing.Size(595, 563);
            this.RenderTabControl.TabIndex = 2;
            this.RenderTabControl.SelectedIndexChanged += new System.EventHandler(this.RenderTabControlSelectedIndexChanged);
            // 
            // xamlViewerTab
            // 
            this.xamlViewerTab.Controls.Add(this.splitter1);
            this.xamlViewerTab.Controls.Add(this.ElementHost);
            this.xamlViewerTab.Location = new System.Drawing.Point(4, 22);
            this.xamlViewerTab.Name = "xamlViewerTab";
            this.xamlViewerTab.Padding = new System.Windows.Forms.Padding(3);
            this.xamlViewerTab.Size = new System.Drawing.Size(587, 537);
            this.xamlViewerTab.TabIndex = 0;
            this.xamlViewerTab.Text = "Render";
            this.xamlViewerTab.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(3, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 531);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // ElementHost
            // 
            this.ElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElementHost.Location = new System.Drawing.Point(3, 3);
            this.ElementHost.Name = "ElementHost";
            this.ElementHost.Size = new System.Drawing.Size(581, 531);
            this.ElementHost.TabIndex = 2;
            this.ElementHost.Text = " ";
            this.ElementHost.Child = null;
            // 
            // sourceTab
            // 
            this.sourceTab.Controls.Add(this.XamlTextBox);
            this.sourceTab.Location = new System.Drawing.Point(4, 22);
            this.sourceTab.Name = "sourceTab";
            this.sourceTab.Padding = new System.Windows.Forms.Padding(3);
            this.sourceTab.Size = new System.Drawing.Size(587, 537);
            this.sourceTab.TabIndex = 1;
            this.sourceTab.Text = "Source";
            this.sourceTab.UseVisualStyleBackColor = true;
            // 
            // XamlTextBox
            // 
            this.XamlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XamlTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XamlTextBox.Location = new System.Drawing.Point(3, 3);
            this.XamlTextBox.Name = "XamlTextBox";
            this.XamlTextBox.Size = new System.Drawing.Size(581, 526);
            this.XamlTextBox.TabIndex = 0;
            this.XamlTextBox.Text = resources.GetString("XamlTextBox.Text");
            this.XamlTextBox.WordWrap = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel6);
            this.splitContainer1.Panel2.Controls.Add(this.panel5);
            this.splitContainer1.Size = new System.Drawing.Size(966, 588);
            this.splitContainer1.SplitterDistance = 367;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(367, 588);
            this.splitContainer2.SplitterDistance = 439;
            this.splitContainer2.TabIndex = 3;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.panel4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer3.Size = new System.Drawing.Size(367, 439);
            this.splitContainer3.SplitterDistance = 226;
            this.splitContainer3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(367, 226);
            this.panel4.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel9);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(367, 226);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Visual Component Tree";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.viewTreeView);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 16);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(361, 207);
            this.panel9.TabIndex = 3;
            // 
            // viewTreeView
            // 
            this.viewTreeView.ContextMenuStrip = this.contextMenuStrip;
            this.viewTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewTreeView.ImageKey = "empty.GIF";
            this.viewTreeView.ImageList = this.imageList1;
            this.viewTreeView.Location = new System.Drawing.Point(0, 0);
            this.viewTreeView.Name = "viewTreeView";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Node2";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Node3";
            treeNode4.Name = "Node4";
            treeNode4.Text = "Node4";
            treeNode5.Name = "Node0";
            treeNode5.Text = "Node0";
            this.viewTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.viewTreeView.SelectedImageIndex = 0;
            this.viewTreeView.Size = new System.Drawing.Size(361, 207);
            this.viewTreeView.TabIndex = 1;
            this.viewTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.viewTreeView_AfterSelect);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editXMLToolStripMenuItem,
            this.labelConnectMenuItem,
            this.changeComponentTypeMenuItem,
            this.mapComponentMenuItem,
            this.componentMapMenuItem,
            this.editRuleSetItem,
            this.assignHintToolStripMenuItem,
            this.expanderToolStripMenuItem,
            this.groupboxToolStripMenuItem,
            this.gridColumnToolStripMenuItem,
            this.jumpToActionToolStripMenuItem,
            this.moveToGroupboxToolStripMenuItem,
            this.changeDisplayFormatToolStripMenuItem,
            this.changeDefaultValueToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(204, 312);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripOpening);
            // 
            // editXMLToolStripMenuItem
            // 
            this.editXMLToolStripMenuItem.Name = "editXMLToolStripMenuItem";
            this.editXMLToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.editXMLToolStripMenuItem.Text = "Edit XML...";
            this.editXMLToolStripMenuItem.Click += new System.EventHandler(this.editXMLToolStripMenuItem_Click);
            // 
            // labelConnectMenuItem
            // 
            this.labelConnectMenuItem.Name = "labelConnectMenuItem";
            this.labelConnectMenuItem.Size = new System.Drawing.Size(203, 22);
            this.labelConnectMenuItem.Text = "Attach Label";
            this.labelConnectMenuItem.Visible = false;
            // 
            // changeComponentTypeMenuItem
            // 
            this.changeComponentTypeMenuItem.Name = "changeComponentTypeMenuItem";
            this.changeComponentTypeMenuItem.Size = new System.Drawing.Size(203, 22);
            this.changeComponentTypeMenuItem.Text = "Configure Component...";
            this.changeComponentTypeMenuItem.Click += new System.EventHandler(this.changeComponentTypeToolStripMenuItem_Click);
            // 
            // mapComponentMenuItem
            // 
            this.mapComponentMenuItem.Name = "mapComponentMenuItem";
            this.mapComponentMenuItem.Size = new System.Drawing.Size(203, 22);
            this.mapComponentMenuItem.Text = "Map to Field";
            // 
            // componentMapMenuItem
            // 
            this.componentMapMenuItem.Name = "componentMapMenuItem";
            this.componentMapMenuItem.Size = new System.Drawing.Size(203, 22);
            this.componentMapMenuItem.Text = "Service Method Map...";
            this.componentMapMenuItem.Click += new System.EventHandler(this.componentMapMenuItem_Click);
            // 
            // editRuleSetItem
            // 
            this.editRuleSetItem.Name = "editRuleSetItem";
            this.editRuleSetItem.Size = new System.Drawing.Size(203, 22);
            this.editRuleSetItem.Text = "Rule Set...";
            this.editRuleSetItem.Click += new System.EventHandler(this.editRuleSetItem_Click);
            // 
            // assignHintToolStripMenuItem
            // 
            this.assignHintToolStripMenuItem.Name = "assignHintToolStripMenuItem";
            this.assignHintToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.assignHintToolStripMenuItem.Text = "Assign Hint...";
            this.assignHintToolStripMenuItem.Click += new System.EventHandler(this.assignHintToolStripMenuItem_Click);
            // 
            // expanderToolStripMenuItem
            // 
            this.expanderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addExpanderItem,
            this.removeExpanderItem});
            this.expanderToolStripMenuItem.Name = "expanderToolStripMenuItem";
            this.expanderToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.expanderToolStripMenuItem.Text = "Expander";
            // 
            // addExpanderItem
            // 
            this.addExpanderItem.Name = "addExpanderItem";
            this.addExpanderItem.Size = new System.Drawing.Size(168, 22);
            this.addExpanderItem.Text = "Add Expander";
            this.addExpanderItem.Click += new System.EventHandler(this.addExpanderItem_Click);
            // 
            // removeExpanderItem
            // 
            this.removeExpanderItem.Name = "removeExpanderItem";
            this.removeExpanderItem.Size = new System.Drawing.Size(168, 22);
            this.removeExpanderItem.Text = "Remove Expander";
            this.removeExpanderItem.Click += new System.EventHandler(this.removeExpanderItem_Click);
            // 
            // groupboxToolStripMenuItem
            // 
            this.groupboxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGroupboxItem,
            this.removeGroupboxItem,
            this.moveToToolStripMenuItem});
            this.groupboxToolStripMenuItem.Name = "groupboxToolStripMenuItem";
            this.groupboxToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.groupboxToolStripMenuItem.Text = "Groupbox";
            this.groupboxToolStripMenuItem.DropDownOpening += new System.EventHandler(this.groupBoxToolStripMenuItem_DropDownOpening);
            // 
            // addGroupboxItem
            // 
            this.addGroupboxItem.Name = "addGroupboxItem";
            this.addGroupboxItem.Size = new System.Drawing.Size(172, 22);
            this.addGroupboxItem.Text = "Add Groupbox";
            this.addGroupboxItem.Click += new System.EventHandler(this.addGroupboxItem_Click);
            // 
            // removeGroupboxItem
            // 
            this.removeGroupboxItem.Name = "removeGroupboxItem";
            this.removeGroupboxItem.Size = new System.Drawing.Size(172, 22);
            this.removeGroupboxItem.Text = "Remove Groupbox";
            this.removeGroupboxItem.Click += new System.EventHandler(this.removeGroupboxItem_Click);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.moveToToolStripMenuItem.Text = "Move to";
            // 
            // gridColumnToolStripMenuItem
            // 
            this.gridColumnToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addColumnToolStripMenuItem,
            this.removeColumnToolStripMenuItem});
            this.gridColumnToolStripMenuItem.Name = "gridColumnToolStripMenuItem";
            this.gridColumnToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.gridColumnToolStripMenuItem.Text = "Grid Column";
            // 
            // addColumnToolStripMenuItem
            // 
            this.addColumnToolStripMenuItem.Name = "addColumnToolStripMenuItem";
            this.addColumnToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.addColumnToolStripMenuItem.Text = "Add Column";
            this.addColumnToolStripMenuItem.Click += new System.EventHandler(this.addColumnToolStripMenuItem_Click);
            // 
            // removeColumnToolStripMenuItem
            // 
            this.removeColumnToolStripMenuItem.Name = "removeColumnToolStripMenuItem";
            this.removeColumnToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.removeColumnToolStripMenuItem.Text = "Remove Column";
            this.removeColumnToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // jumpToActionToolStripMenuItem
            // 
            this.jumpToActionToolStripMenuItem.Name = "jumpToActionToolStripMenuItem";
            this.jumpToActionToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.jumpToActionToolStripMenuItem.Text = "Jump to Action";
            // 
            // moveToGroupboxToolStripMenuItem
            // 
            this.moveToGroupboxToolStripMenuItem.Name = "moveToGroupboxToolStripMenuItem";
            this.moveToGroupboxToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.moveToGroupboxToolStripMenuItem.Text = "Move to Groupbox";
            // 
            // changeDisplayFormatToolStripMenuItem
            // 
            this.changeDisplayFormatToolStripMenuItem.Name = "changeDisplayFormatToolStripMenuItem";
            this.changeDisplayFormatToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.changeDisplayFormatToolStripMenuItem.Text = "Display Format...";
            this.changeDisplayFormatToolStripMenuItem.Click += new System.EventHandler(this.changeDisplayFormatToolStripMenuItem_Click);
            // 
            // changeDefaultValueToolStripMenuItem
            // 
            this.changeDefaultValueToolStripMenuItem.Name = "changeDefaultValueToolStripMenuItem";
            this.changeDefaultValueToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.changeDefaultValueToolStripMenuItem.Text = "Change Default Value...";
            this.changeDefaultValueToolStripMenuItem.Click += new System.EventHandler(this.changeDefaultValueToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "Control_Label.bmp");
            this.imageList1.Images.SetKeyName(1, "Control_TextBox.bmp");
            this.imageList1.Images.SetKeyName(2, "Control_Checkbox.bmp");
            this.imageList1.Images.SetKeyName(3, "Control_ComboBox.bmp");
            this.imageList1.Images.SetKeyName(4, "Control_GroupBox.bmp");
            this.imageList1.Images.SetKeyName(5, "Control_RadioButton.bmp");
            this.imageList1.Images.SetKeyName(6, "Control_Panel.bmp");
            this.imageList1.Images.SetKeyName(7, "Control_FlowLayoutPanel.bmp");
            this.imageList1.Images.SetKeyName(8, "Control_DataGridView.bmp");
            this.imageList1.Images.SetKeyName(9, "VSObject_Assembly.bmp");
            this.imageList1.Images.SetKeyName(10, "searchDocuments.bmp");
            this.imageList1.Images.SetKeyName(11, "Control_PrintPreviewDialog.bmp");
            this.imageList1.Images.SetKeyName(12, "Control_Form.bmp");
            this.imageList1.Images.SetKeyName(13, "expander.bmp");
            this.imageList1.Images.SetKeyName(14, "Control_LinkLabel.bmp");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel10);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(367, 209);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Component Properties";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.propertyGrid);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(3, 40);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(361, 166);
            this.panel10.TabIndex = 5;
            // 
            // propertyGrid
            // 
            this.propertyGrid.ContextMenuStrip = this.PropertyGridcontextMenuStrip;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(361, 166);
            this.propertyGrid.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.componentLabel);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(361, 24);
            this.panel2.TabIndex = 4;
            // 
            // componentLabel
            // 
            this.componentLabel.AutoSize = true;
            this.componentLabel.Location = new System.Drawing.Point(83, 6);
            this.componentLabel.Name = "componentLabel";
            this.componentLabel.Size = new System.Drawing.Size(31, 13);
            this.componentLabel.TabIndex = 1;
            this.componentLabel.Text = "none";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Component:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 145);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(367, 145);
            this.panel3.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel12);
            this.groupBox1.Controls.Add(this.panel11);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 145);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datasources";
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.lvDatasources);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(3, 16);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(361, 100);
            this.panel12.TabIndex = 5;
            // 
            // lvDatasources
            // 
            this.lvDatasources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDSId,
            this.chDSName,
            this.chDSServiceMethod});
            this.lvDatasources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDatasources.FullRowSelect = true;
            this.lvDatasources.HideSelection = false;
            this.lvDatasources.Location = new System.Drawing.Point(0, 0);
            this.lvDatasources.MultiSelect = false;
            this.lvDatasources.Name = "lvDatasources";
            this.lvDatasources.Size = new System.Drawing.Size(361, 100);
            this.lvDatasources.TabIndex = 0;
            this.lvDatasources.UseCompatibleStateImageBehavior = false;
            this.lvDatasources.View = System.Windows.Forms.View.Details;
            this.lvDatasources.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvDatasources_ItemSelectionChanged);
            // 
            // chDSId
            // 
            this.chDSId.Text = "Id";
            this.chDSId.Width = 31;
            // 
            // chDSName
            // 
            this.chDSName.Text = "Name";
            this.chDSName.Width = 145;
            // 
            // chDSServiceMethod
            // 
            this.chDSServiceMethod.Text = "ServiceMethod";
            this.chDSServiceMethod.Width = 158;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.dataSourceToolStrip);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(3, 116);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(361, 26);
            this.panel11.TabIndex = 4;
            // 
            // dataSourceToolStrip
            // 
            this.dataSourceToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.dataSourceToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbDSAdd,
            this.toolStripSeparator3,
            this.tsbDSEdit,
            this.toolStripSeparator4,
            this.tsbDSDelete,
            this.toolStripSeparator5,
            this.tsbDSEditRequestMap,
            this.toolStripSeparator6,
            this.tsbDSEditResponseMap});
            this.dataSourceToolStrip.Location = new System.Drawing.Point(0, 0);
            this.dataSourceToolStrip.Name = "dataSourceToolStrip";
            this.dataSourceToolStrip.Size = new System.Drawing.Size(361, 25);
            this.dataSourceToolStrip.TabIndex = 0;
            this.dataSourceToolStrip.Text = "toolStrip1";
            // 
            // tsbDSAdd
            // 
            this.tsbDSAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDSAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbDSAdd.Image")));
            this.tsbDSAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDSAdd.Name = "tsbDSAdd";
            this.tsbDSAdd.Size = new System.Drawing.Size(33, 22);
            this.tsbDSAdd.Text = "Add";
            this.tsbDSAdd.Click += new System.EventHandler(this.tsbDSAdd_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDSEdit
            // 
            this.tsbDSEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDSEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbDSEdit.Image")));
            this.tsbDSEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDSEdit.Name = "tsbDSEdit";
            this.tsbDSEdit.Size = new System.Drawing.Size(31, 22);
            this.tsbDSEdit.Text = "Edit";
            this.tsbDSEdit.Click += new System.EventHandler(this.tsbDSEdit_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDSDelete
            // 
            this.tsbDSDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDSDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDSDelete.Image")));
            this.tsbDSDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDSDelete.Name = "tsbDSDelete";
            this.tsbDSDelete.Size = new System.Drawing.Size(44, 22);
            this.tsbDSDelete.Text = "Delete";
            this.tsbDSDelete.Click += new System.EventHandler(this.tsbDSDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDSEditRequestMap
            // 
            this.tsbDSEditRequestMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDSEditRequestMap.Image = ((System.Drawing.Image)(resources.GetObject("tsbDSEditRequestMap.Image")));
            this.tsbDSEditRequestMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDSEditRequestMap.Name = "tsbDSEditRequestMap";
            this.tsbDSEditRequestMap.Size = new System.Drawing.Size(80, 22);
            this.tsbDSEditRequestMap.Text = "Request Map";
            this.tsbDSEditRequestMap.Click += new System.EventHandler(this.tsbDSEditRequestMap_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDSEditResponseMap
            // 
            this.tsbDSEditResponseMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDSEditResponseMap.Image = ((System.Drawing.Image)(resources.GetObject("tsbDSEditResponseMap.Image")));
            this.tsbDSEditResponseMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDSEditResponseMap.Name = "tsbDSEditResponseMap";
            this.tsbDSEditResponseMap.Size = new System.Drawing.Size(88, 22);
            this.tsbDSEditResponseMap.Text = "Response Map";
            this.tsbDSEditResponseMap.Click += new System.EventHandler(this.tsbDSEditResponseMap_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.RenderTabControl);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 25);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(595, 563);
            this.panel6.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.AutoSize = true;
            this.panel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel5.Controls.Add(this.viewToolStrip);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(595, 25);
            this.panel5.TabIndex = 3;
            // 
            // viewToolStrip
            // 
            this.viewToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.viewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upBtn,
            this.downBtn,
            this.toolStripSeparator2,
            this.editGroupboxLayout,
            this.toolStripSeparator1,
            this.refreshBtn});
            this.viewToolStrip.Location = new System.Drawing.Point(0, 0);
            this.viewToolStrip.Name = "viewToolStrip";
            this.viewToolStrip.Size = new System.Drawing.Size(595, 25);
            this.viewToolStrip.TabIndex = 1;
            this.viewToolStrip.Text = "toolStrip1";
            // 
            // upBtn
            // 
            this.upBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.upBtn.Image = global::Cdc.MetaManager.GUI.Properties.Resources.MoveUp;
            this.upBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.upBtn.Name = "upBtn";
            this.upBtn.Size = new System.Drawing.Size(23, 22);
            this.upBtn.Text = "Move Up";
            this.upBtn.Click += new System.EventHandler(this.upBtn_Click);
            // 
            // downBtn
            // 
            this.downBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.downBtn.Image = global::Cdc.MetaManager.GUI.Properties.Resources.MoveDown;
            this.downBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(23, 22);
            this.downBtn.Text = "Move Down";
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // editGroupboxLayout
            // 
            this.editGroupboxLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editGroupboxLayout.Image = ((System.Drawing.Image)(resources.GetObject("editGroupboxLayout.Image")));
            this.editGroupboxLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editGroupboxLayout.Name = "editGroupboxLayout";
            this.editGroupboxLayout.Size = new System.Drawing.Size(95, 22);
            this.editGroupboxLayout.Text = "Edit Grid Layout";
            this.editGroupboxLayout.Click += new System.EventHandler(this.editGroupboxLayout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // refreshBtn
            // 
            this.refreshBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.refreshBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshBtn.Image")));
            this.refreshBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(80, 22);
            this.refreshBtn.Text = "Refresh Xaml";
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.button1);
            this.panel7.Controls.Add(this.okBtn);
            this.panel7.Controls.Add(this.cancelBtn);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 588);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(966, 36);
            this.panel7.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(693, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Unmap All Fields";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(803, 7);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 1;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(884, 7);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 0;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // XamlViewerForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(966, 624);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel7);
            this.Name = "XamlViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XamlViewer";
            this.Load += new System.EventHandler(this.XamlViewerFormLoad);
            this.RenderTabControl.ResumeLayout(false);
            this.xamlViewerTab.ResumeLayout(false);
            this.sourceTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.dataSourceToolStrip.ResumeLayout(false);
            this.dataSourceToolStrip.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.viewToolStrip.ResumeLayout(false);
            this.viewToolStrip.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl RenderTabControl;
        private System.Windows.Forms.TabPage xamlViewerTab;
        private System.Windows.Forms.TabPage sourceTab;
        private System.Windows.Forms.Integration.ElementHost ElementHost;
        private System.Windows.Forms.RichTextBox XamlTextBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label componentLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TreeView viewTreeView;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStrip viewToolStrip;
        private System.Windows.Forms.ToolStripButton editGroupboxLayout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton upBtn;
        private System.Windows.Forms.ToolStripButton downBtn;
        private System.Windows.Forms.ToolStripButton refreshBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem labelConnectMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem changeComponentTypeMenuItem;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.ToolStripMenuItem mapComponentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem componentMapMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editRuleSetItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel11;
        private ListViewSort lvDatasources;
        private System.Windows.Forms.ColumnHeader chDSName;
        private System.Windows.Forms.ColumnHeader chDSServiceMethod;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.ToolStrip dataSourceToolStrip;
        private System.Windows.Forms.ToolStripButton tsbDSAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbDSEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbDSDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbDSEditRequestMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbDSEditResponseMap;
        private System.Windows.Forms.ColumnHeader chDSId;
        private System.Windows.Forms.ToolStripMenuItem expanderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addExpanderItem;
        private System.Windows.Forms.ToolStripMenuItem removeExpanderItem;
        private System.Windows.Forms.ToolStripMenuItem groupboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addGroupboxItem;
        private System.Windows.Forms.ToolStripMenuItem removeGroupboxItem;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpToActionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToGroupboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDisplayFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDefaultValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assignHintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridColumnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addColumnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeColumnToolStripMenuItem;
    }
}

