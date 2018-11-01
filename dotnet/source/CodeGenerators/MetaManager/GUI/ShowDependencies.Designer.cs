namespace Cdc.MetaManager.GUI
{
    partial class ShowDependencies
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
            this.btnClose = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbDependencies = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pgDependencyObject = new System.Windows.Forms.PropertyGrid();
            this.statusLabel = new System.Windows.Forms.Label();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.dependencyGraph1 = new Cdc.MetaManager.GUI.DependencyGraph();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 685);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1129, 38);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(1051, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statusLabel);
            this.splitContainer1.Panel1.Controls.Add(this.lbDependencies);
            this.splitContainer1.Panel1.Controls.Add(this.elementHost);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Size = new System.Drawing.Size(1129, 682);
            this.splitContainer1.SplitterDistance = 853;
            this.splitContainer1.TabIndex = 4;
            // 
            // lbDependencies
            // 
            this.lbDependencies.BackColor = System.Drawing.SystemColors.Window;
            this.lbDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDependencies.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDependencies.Location = new System.Drawing.Point(0, 0);
            this.lbDependencies.Name = "lbDependencies";
            this.lbDependencies.Size = new System.Drawing.Size(853, 682);
            this.lbDependencies.TabIndex = 3;
            this.lbDependencies.Text = "Building dependency graph...";
            this.lbDependencies.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pgDependencyObject);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 682);
            this.panel3.TabIndex = 2;
            // 
            // pgDependencyObject
            // 
            this.pgDependencyObject.ContextMenuStrip = this.PropertyGridcontextMenuStrip;
            this.pgDependencyObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgDependencyObject.HelpVisible = false;
            this.pgDependencyObject.Location = new System.Drawing.Point(0, 0);
            this.pgDependencyObject.Name = "pgDependencyObject";
            this.pgDependencyObject.Size = new System.Drawing.Size(272, 682);
            this.pgDependencyObject.TabIndex = 0;
            this.pgDependencyObject.ToolbarVisible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.statusLabel.AutoSize = true;
            this.statusLabel.BackColor = System.Drawing.SystemColors.Window;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(3, 663);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(54, 16);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Status...";
            // 
            // elementHost
            // 
            this.elementHost.BackColor = System.Drawing.SystemColors.Window;
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(853, 682);
            this.elementHost.TabIndex = 1;
            this.elementHost.Text = "elementHost1";
            this.elementHost.Child = this.dependencyGraph1;
            // 
            // ShowDependencies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1135, 726);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "ShowDependencies";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dependencies";
            this.Load += new System.EventHandler(this.ShowServiceMethodDependencies_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private DependencyGraph dependencyGraph1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PropertyGrid pgDependencyObject;
        private System.Windows.Forms.Label lbDependencies;
        private System.Windows.Forms.Label statusLabel;
    }
}