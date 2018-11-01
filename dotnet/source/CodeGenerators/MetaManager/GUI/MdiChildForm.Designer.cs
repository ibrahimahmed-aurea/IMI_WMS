namespace Cdc.MetaManager.GUI
{
    partial class MdiChildForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MdiChildForm));
            this.PropertyGridcontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.jumpToObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpToAndCheckOutObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyIdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.referencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertyGridcontextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PropertyGridcontextMenuStrip
            // 
            this.PropertyGridcontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToObjectToolStripMenuItem,
            this.jumpToAndCheckOutObjectToolStripMenuItem,
            this.copyIdToolStripMenuItem,
            this.referencesToolStripMenuItem});
            this.PropertyGridcontextMenuStrip.Name = "PropertyGridcontextMenuStrip";
            this.PropertyGridcontextMenuStrip.Size = new System.Drawing.Size(236, 114);
            this.PropertyGridcontextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.PropertyGridcontextMenuStrip_Opening);
            // 
            // jumpToObjectToolStripMenuItem
            // 
            this.jumpToObjectToolStripMenuItem.Name = "jumpToObjectToolStripMenuItem";
            this.jumpToObjectToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.jumpToObjectToolStripMenuItem.Text = "Jump to object";
            this.jumpToObjectToolStripMenuItem.Click += new System.EventHandler(this.jumpToObjectToolStripMenuItem_Click);
            // 
            // jumpToAndCheckOutObjectToolStripMenuItem
            // 
            this.jumpToAndCheckOutObjectToolStripMenuItem.Name = "jumpToAndCheckOutObjectToolStripMenuItem";
            this.jumpToAndCheckOutObjectToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.jumpToAndCheckOutObjectToolStripMenuItem.Text = "Jump to and Check Out object";
            this.jumpToAndCheckOutObjectToolStripMenuItem.Click += new System.EventHandler(this.jumpToAndCheckOutObjectToolStripMenuItem_Click);
            // 
            // copyIdToolStripMenuItem
            // 
            this.copyIdToolStripMenuItem.Name = "copyIdToolStripMenuItem";
            this.copyIdToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.copyIdToolStripMenuItem.Text = "Copy Id";
            this.copyIdToolStripMenuItem.Click += new System.EventHandler(this.copyIdToolStripMenuItem_Click);
            // 
            // referencesToolStripMenuItem
            // 
            this.referencesToolStripMenuItem.Name = "referencesToolStripMenuItem";
            this.referencesToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.referencesToolStripMenuItem.Text = "References";
            // 
            // MdiChildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MdiChildForm";
            this.Text = "MdiChildForm";
            this.Activated += new System.EventHandler(this.MdiChildForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MdiChildForm_FormClosing);
            this.Load += new System.EventHandler(this.MdiChildForm_Load);
            this.PropertyGridcontextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem jumpToObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpToAndCheckOutObjectToolStripMenuItem;
        protected System.Windows.Forms.ContextMenuStrip PropertyGridcontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyIdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referencesToolStripMenuItem;
    }
}