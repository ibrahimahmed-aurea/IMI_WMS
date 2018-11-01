
namespace Cdc.MetaManager.GUI
{
    partial class ShowIssueList
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel7 = new System.Windows.Forms.Panel();
            this.tvIssueTree = new System.Windows.Forms.TreeView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnCollapseAll = new System.Windows.Forms.Button();
            this.btnExpandAll = new System.Windows.Forms.Button();
            this.cbShowHidden = new System.Windows.Forms.CheckBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel9 = new System.Windows.Forms.Panel();
            this.lvIssueList = new ListViewSort();
            this.chType = new System.Windows.Forms.ColumnHeader();
            this.chText = new System.Windows.Forms.ColumnHeader();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnUnselectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSaveSelection = new System.Windows.Forms.Button();
            this.tbFullText = new System.Windows.Forms.TextBox();
            this.gbObject = new System.Windows.Forms.GroupBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.cbShowHintIssues = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.gbObject.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(842, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 651);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(5, 616);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(837, 35);
            this.panel2.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(735, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(5, 651);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(5, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(837, 5);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.splitContainer1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(837, 611);
            this.panel5.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbObject);
            this.splitContainer1.Size = new System.Drawing.Size(837, 611);
            this.splitContainer1.SplitterDistance = 415;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(837, 415);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Issuelist";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel7);
            this.splitContainer2.Panel1.Controls.Add(this.panel6);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(831, 396);
            this.splitContainer2.SplitterDistance = 330;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.tvIssueTree);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(330, 353);
            this.panel7.TabIndex = 2;
            // 
            // tvIssueTree
            // 
            this.tvIssueTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvIssueTree.FullRowSelect = true;
            this.tvIssueTree.HideSelection = false;
            this.tvIssueTree.Location = new System.Drawing.Point(0, 0);
            this.tvIssueTree.Name = "tvIssueTree";
            this.tvIssueTree.Size = new System.Drawing.Size(330, 353);
            this.tvIssueTree.TabIndex = 0;
            this.tvIssueTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvIssueTree_AfterSelect);
            this.tvIssueTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvIssueTree_BeforeSelect);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cbShowHintIssues);
            this.panel6.Controls.Add(this.btnCollapseAll);
            this.panel6.Controls.Add(this.btnExpandAll);
            this.panel6.Controls.Add(this.cbShowHidden);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 353);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(330, 43);
            this.panel6.TabIndex = 1;
            // 
            // btnCollapseAll
            // 
            this.btnCollapseAll.Location = new System.Drawing.Point(0, 11);
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.Size = new System.Drawing.Size(75, 23);
            this.btnCollapseAll.TabIndex = 1;
            this.btnCollapseAll.Text = "Collapse All";
            this.btnCollapseAll.UseVisualStyleBackColor = true;
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
            // 
            // btnExpandAll
            // 
            this.btnExpandAll.Location = new System.Drawing.Point(81, 11);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.Size = new System.Drawing.Size(75, 23);
            this.btnExpandAll.TabIndex = 0;
            this.btnExpandAll.Text = "Expand All";
            this.btnExpandAll.UseVisualStyleBackColor = true;
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            // 
            // cbShowHidden
            // 
            this.cbShowHidden.AutoSize = true;
            this.cbShowHidden.Location = new System.Drawing.Point(169, 7);
            this.cbShowHidden.Name = "cbShowHidden";
            this.cbShowHidden.Size = new System.Drawing.Size(120, 17);
            this.cbShowHidden.TabIndex = 1;
            this.cbShowHidden.Text = "Show Hidden Errors";
            this.cbShowHidden.UseVisualStyleBackColor = true;
            this.cbShowHidden.CheckedChanged += new System.EventHandler(this.cbShowHidden_CheckedChanged);
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
            this.splitContainer3.Panel1.Controls.Add(this.panel9);
            this.splitContainer3.Panel1.Controls.Add(this.panel8);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tbFullText);
            this.splitContainer3.Size = new System.Drawing.Size(497, 396);
            this.splitContainer3.SplitterDistance = 284;
            this.splitContainer3.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.lvIssueList);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(497, 254);
            this.panel9.TabIndex = 2;
            // 
            // lvIssueList
            // 
            this.lvIssueList.CheckBoxes = true;
            this.lvIssueList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chType,
            this.chText});
            this.lvIssueList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvIssueList.FullRowSelect = true;
            this.lvIssueList.HideSelection = false;
            this.lvIssueList.Location = new System.Drawing.Point(0, 0);
            this.lvIssueList.MultiSelect = false;
            this.lvIssueList.Name = "lvIssueList";
            this.lvIssueList.Size = new System.Drawing.Size(497, 254);
            this.lvIssueList.TabIndex = 0;
            this.lvIssueList.UseCompatibleStateImageBehavior = false;
            this.lvIssueList.View = System.Windows.Forms.View.Details;
            this.lvIssueList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvIssueList_ItemChecked);
            this.lvIssueList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvIssueList_ItemSelectionChanged);
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 100;
            // 
            // chText
            // 
            this.chText.Text = "Text";
            this.chText.Width = 276;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnUnselectAll);
            this.panel8.Controls.Add(this.btnSelectAll);
            this.panel8.Controls.Add(this.btnSaveSelection);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 254);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(497, 30);
            this.panel8.TabIndex = 1;
            // 
            // btnUnselectAll
            // 
            this.btnUnselectAll.Location = new System.Drawing.Point(83, 3);
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(74, 23);
            this.btnUnselectAll.TabIndex = 3;
            this.btnUnselectAll.Text = "Unselect All";
            this.btnUnselectAll.UseVisualStyleBackColor = true;
            this.btnUnselectAll.Click += new System.EventHandler(this.btnUnselectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(3, 3);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(74, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSaveSelection
            // 
            this.btnSaveSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSelection.Location = new System.Drawing.Point(394, 3);
            this.btnSaveSelection.Name = "btnSaveSelection";
            this.btnSaveSelection.Size = new System.Drawing.Size(99, 23);
            this.btnSaveSelection.TabIndex = 0;
            this.btnSaveSelection.Text = "Save Selection";
            this.btnSaveSelection.UseVisualStyleBackColor = true;
            this.btnSaveSelection.Click += new System.EventHandler(this.btnSaveSelection_Click);
            // 
            // tbFullText
            // 
            this.tbFullText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFullText.Location = new System.Drawing.Point(0, 0);
            this.tbFullText.Multiline = true;
            this.tbFullText.Name = "tbFullText";
            this.tbFullText.ReadOnly = true;
            this.tbFullText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFullText.Size = new System.Drawing.Size(497, 108);
            this.tbFullText.TabIndex = 1;
            // 
            // gbObject
            // 
            this.gbObject.Controls.Add(this.propertyGrid);
            this.gbObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbObject.Location = new System.Drawing.Point(0, 0);
            this.gbObject.Name = "gbObject";
            this.gbObject.Size = new System.Drawing.Size(837, 192);
            this.gbObject.TabIndex = 1;
            this.gbObject.TabStop = false;
            this.gbObject.Text = "Issue Object Detail";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new System.Drawing.Point(3, 16);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(831, 173);
            this.propertyGrid.TabIndex = 0;
            // 
            // cbShowHintIssues
            // 
            this.cbShowHintIssues.AutoSize = true;
            this.cbShowHintIssues.Location = new System.Drawing.Point(169, 23);
            this.cbShowHintIssues.Name = "cbShowHintIssues";
            this.cbShowHintIssues.Size = new System.Drawing.Size(108, 17);
            this.cbShowHintIssues.TabIndex = 2;
            this.cbShowHintIssues.Text = "Show Hint Issues";
            this.cbShowHintIssues.UseVisualStyleBackColor = true;
            this.cbShowHintIssues.CheckedChanged += new System.EventHandler(this.cbShowHintIssues_CheckedChanged);
            // 
            // ShowIssueList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 651);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "ShowIssueList";
            this.Text = "Show Issue List";
            this.Load += new System.EventHandler(this.ShowIssueList_Load);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.gbObject.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView tvIssueTree;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ListViewSort lvIssueList;
        private System.Windows.Forms.GroupBox gbObject;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chText;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox tbFullText;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnCollapseAll;
        private System.Windows.Forms.Button btnExpandAll;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnSaveSelection;
        private System.Windows.Forms.CheckBox cbShowHidden;
        private System.Windows.Forms.Button btnUnselectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.CheckBox cbShowHintIssues;
    }
}