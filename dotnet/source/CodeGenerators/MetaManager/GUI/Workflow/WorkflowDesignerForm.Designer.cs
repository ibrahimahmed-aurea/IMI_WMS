namespace Cdc.MetaManager.GUI
{
    partial class WorkflowDesignerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkflowDesignerForm));
            this.Panel2 = new System.Windows.Forms.Panel();
            this.workflowControl = new Cdc.MetaManager.GUI.Workflow.WorkflowDesignerControl();
            this.viewToolStrip = new System.Windows.Forms.ToolStrip();
            this.addDialogBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addServiceMethodBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.modifyMapBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tbxName = new System.Windows.Forms.ToolStripTextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.addWorkflowBtn = new System.Windows.Forms.ToolStripButton();
            this.Panel2.SuspendLayout();
            this.viewToolStrip.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.workflowControl);
            this.Panel2.Controls.Add(this.viewToolStrip);
            this.Panel2.Controls.Add(this.panel5);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel2.Location = new System.Drawing.Point(0, 0);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(925, 630);
            this.Panel2.TabIndex = 1;
            // 
            // workflowControl
            // 
            this.workflowControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.workflowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workflowControl.Location = new System.Drawing.Point(0, 25);
            this.workflowControl.Name = "workflowControl";
            this.workflowControl.NameSpace = "foo";
            this.workflowControl.Size = new System.Drawing.Size(925, 570);
            this.workflowControl.TabIndex = 10;
            this.workflowControl.TypeName = "Workflow1";
            // 
            // viewToolStrip
            // 
            this.viewToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.viewToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDialogBtn,
            this.toolStripSeparator1,
            this.addServiceMethodBtn,
            this.toolStripSeparator4,
            this.addWorkflowBtn,
            this.toolStripSeparator3,
            this.modifyMapBtn,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.tbxName});
            this.viewToolStrip.Location = new System.Drawing.Point(0, 0);
            this.viewToolStrip.Name = "viewToolStrip";
            this.viewToolStrip.Size = new System.Drawing.Size(925, 25);
            this.viewToolStrip.TabIndex = 9;
            this.viewToolStrip.Text = "toolStrip1";
            // 
            // addDialogBtn
            // 
            this.addDialogBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addDialogBtn.Image = ((System.Drawing.Image)(resources.GetObject("addDialogBtn.Image")));
            this.addDialogBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addDialogBtn.Name = "addDialogBtn";
            this.addDialogBtn.Size = new System.Drawing.Size(62, 22);
            this.addDialogBtn.Text = "Add Dialog";
            this.addDialogBtn.Click += new System.EventHandler(this.addDialogBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // addServiceMethodBtn
            // 
            this.addServiceMethodBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addServiceMethodBtn.Image = ((System.Drawing.Image)(resources.GetObject("addServiceMethodBtn.Image")));
            this.addServiceMethodBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addServiceMethodBtn.Name = "addServiceMethodBtn";
            this.addServiceMethodBtn.Size = new System.Drawing.Size(107, 22);
            this.addServiceMethodBtn.Text = "Add Service Method";
            this.addServiceMethodBtn.Click += new System.EventHandler(this.addServiceMethodBtn_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // modifyMapBtn
            // 
            this.modifyMapBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.modifyMapBtn.Image = ((System.Drawing.Image)(resources.GetObject("modifyMapBtn.Image")));
            this.modifyMapBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modifyMapBtn.Name = "modifyMapBtn";
            this.modifyMapBtn.Size = new System.Drawing.Size(109, 22);
            this.modifyMapBtn.Text = "Request Map";
            this.modifyMapBtn.Click += new System.EventHandler(this.modifyMapBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel1.Text = "Name:";
            // 
            // tbxName
            // 
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(150, 25);
            this.tbxName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxName_KeyDown);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.okBtn);
            this.panel5.Controls.Add(this.cancelBtn);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 595);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(925, 35);
            this.panel5.TabIndex = 7;
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(721, 6);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(93, 23);
            this.okBtn.TabIndex = 6;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(820, 6);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(93, 23);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // addWorkflowBtn
            // 
            this.addWorkflowBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addWorkflowBtn.Image = ((System.Drawing.Image)(resources.GetObject("addWorkflowBtn.Image")));
            this.addWorkflowBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addWorkflowBtn.Name = "addWorkflowBtn";
            this.addWorkflowBtn.Size = new System.Drawing.Size(78, 22);
            this.addWorkflowBtn.Text = "Add Workflow";
            this.addWorkflowBtn.Click += new System.EventHandler(this.addWorkflowBtn_Click);
            // 
            // WorkflowDesignerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(925, 630);
            this.Controls.Add(this.Panel2);
            this.Name = "WorkflowDesignerForm";
            this.Text = "Workflow Designer";
            this.Load += new System.EventHandler(this.WorkflowDesignerForm_Load);
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.viewToolStrip.ResumeLayout(false);
            this.viewToolStrip.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        internal System.Windows.Forms.Panel Panel2;
        private System.Windows.Forms.ToolStrip viewToolStrip;
        private System.Windows.Forms.ToolStripButton addDialogBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton addServiceMethodBtn;
        private Cdc.MetaManager.GUI.Workflow.WorkflowDesignerControl workflowControl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tbxName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton modifyMapBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton addWorkflowBtn;
    }
}