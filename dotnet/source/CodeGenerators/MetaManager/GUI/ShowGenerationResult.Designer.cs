namespace Cdc.MetaManager.GUI
{
    partial class ShowGenerationResult
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
            this.gbFiles = new System.Windows.Forms.GroupBox();
            this.tbGenerationInformation = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbGenerationInformation = new System.Windows.Forms.GroupBox();
            this.btnShowFiles = new System.Windows.Forms.Button();
            this.lvFiles = new ListViewSort();
            this.chFile = new System.Windows.Forms.ColumnHeader();
            this.gbFiles.SuspendLayout();
            this.gbGenerationInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFiles
            // 
            this.gbFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFiles.Controls.Add(this.lvFiles);
            this.gbFiles.Location = new System.Drawing.Point(3, 121);
            this.gbFiles.Name = "gbFiles";
            this.gbFiles.Size = new System.Drawing.Size(668, 321);
            this.gbFiles.TabIndex = 1;
            this.gbFiles.TabStop = false;
            this.gbFiles.Text = "Files";
            this.gbFiles.Visible = false;
            // 
            // tbGenerationInformation
            // 
            this.tbGenerationInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGenerationInformation.Location = new System.Drawing.Point(3, 15);
            this.tbGenerationInformation.Multiline = true;
            this.tbGenerationInformation.Name = "tbGenerationInformation";
            this.tbGenerationInformation.ReadOnly = true;
            this.tbGenerationInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbGenerationInformation.Size = new System.Drawing.Size(662, 62);
            this.tbGenerationInformation.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(587, 82);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gbGenerationInformation
            // 
            this.gbGenerationInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGenerationInformation.Controls.Add(this.btnClose);
            this.gbGenerationInformation.Controls.Add(this.btnShowFiles);
            this.gbGenerationInformation.Controls.Add(this.tbGenerationInformation);
            this.gbGenerationInformation.Location = new System.Drawing.Point(3, 6);
            this.gbGenerationInformation.MinimumSize = new System.Drawing.Size(668, 109);
            this.gbGenerationInformation.Name = "gbGenerationInformation";
            this.gbGenerationInformation.Size = new System.Drawing.Size(668, 109);
            this.gbGenerationInformation.TabIndex = 2;
            this.gbGenerationInformation.TabStop = false;
            this.gbGenerationInformation.Text = "Generation Information";
            // 
            // btnShowFiles
            // 
            this.btnShowFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowFiles.Location = new System.Drawing.Point(3, 82);
            this.btnShowFiles.MinimumSize = new System.Drawing.Size(95, 23);
            this.btnShowFiles.Name = "btnShowFiles";
            this.btnShowFiles.Size = new System.Drawing.Size(95, 23);
            this.btnShowFiles.TabIndex = 1;
            this.btnShowFiles.Text = "Show Files >>";
            this.btnShowFiles.UseVisualStyleBackColor = true;
            this.btnShowFiles.Click += new System.EventHandler(this.btnShowFiles_Click);
            // 
            // lvFiles
            // 
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFile});
            this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.GridLines = true;
            this.lvFiles.Location = new System.Drawing.Point(3, 16);
            this.lvFiles.MultiSelect = false;
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(662, 302);
            this.lvFiles.TabIndex = 0;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            // 
            // chFile
            // 
            this.chFile.Text = "Filename";
            this.chFile.Width = 638;
            // 
            // ShowGenerationResult
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(674, 448);
            this.Controls.Add(this.gbFiles);
            this.Controls.Add(this.gbGenerationInformation);
            this.Name = "ShowGenerationResult";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generation Information";
            this.Load += new System.EventHandler(this.ShowGenerationResult_Load);
            this.gbFiles.ResumeLayout(false);
            this.gbGenerationInformation.ResumeLayout(false);
            this.gbGenerationInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFiles;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gbGenerationInformation;
        private ListViewSort lvFiles;
        private System.Windows.Forms.ColumnHeader chFile;
        private System.Windows.Forms.TextBox tbGenerationInformation;
        private System.Windows.Forms.Button btnShowFiles;

    }
}