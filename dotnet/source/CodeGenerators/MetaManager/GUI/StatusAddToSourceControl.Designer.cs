namespace Cdc.MetaManager.GUI
{
    partial class StatusAddToSourceControl
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
            this.StatusprogressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.Statuslabel = new System.Windows.Forms.Label();
            this.Closebutton = new System.Windows.Forms.Button();
            this.Startbutton = new System.Windows.Forms.Button();
            this.CheckIncheckBox = new System.Windows.Forms.CheckBox();
            this.FrontendcheckBox = new System.Windows.Forms.CheckBox();
            this.backendcheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // StatusprogressBar
            // 
            this.StatusprogressBar.Location = new System.Drawing.Point(12, 36);
            this.StatusprogressBar.Name = "StatusprogressBar";
            this.StatusprogressBar.Size = new System.Drawing.Size(659, 19);
            this.StatusprogressBar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Status :";
            // 
            // Statuslabel
            // 
            this.Statuslabel.AutoSize = true;
            this.Statuslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Statuslabel.Location = new System.Drawing.Point(84, 9);
            this.Statuslabel.Name = "Statuslabel";
            this.Statuslabel.Size = new System.Drawing.Size(0, 24);
            this.Statuslabel.TabIndex = 2;
            // 
            // Closebutton
            // 
            this.Closebutton.Location = new System.Drawing.Point(564, 86);
            this.Closebutton.Name = "Closebutton";
            this.Closebutton.Size = new System.Drawing.Size(107, 29);
            this.Closebutton.TabIndex = 3;
            this.Closebutton.Text = "Close";
            this.Closebutton.UseVisualStyleBackColor = true;
            this.Closebutton.Click += new System.EventHandler(this.Closebutton_Click);
            // 
            // Startbutton
            // 
            this.Startbutton.Location = new System.Drawing.Point(445, 86);
            this.Startbutton.Name = "Startbutton";
            this.Startbutton.Size = new System.Drawing.Size(113, 29);
            this.Startbutton.TabIndex = 4;
            this.Startbutton.Text = "Start";
            this.Startbutton.UseVisualStyleBackColor = true;
            this.Startbutton.Click += new System.EventHandler(this.Startbutton_Click);
            // 
            // CheckIncheckBox
            // 
            this.CheckIncheckBox.AutoSize = true;
            this.CheckIncheckBox.Checked = true;
            this.CheckIncheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckIncheckBox.Location = new System.Drawing.Point(334, 93);
            this.CheckIncheckBox.Name = "CheckIncheckBox";
            this.CheckIncheckBox.Size = new System.Drawing.Size(105, 17);
            this.CheckIncheckBox.TabIndex = 5;
            this.CheckIncheckBox.Text = "Check in objects";
            this.CheckIncheckBox.UseVisualStyleBackColor = true;
            // 
            // FrontendcheckBox
            // 
            this.FrontendcheckBox.AutoSize = true;
            this.FrontendcheckBox.Checked = true;
            this.FrontendcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FrontendcheckBox.Location = new System.Drawing.Point(260, 93);
            this.FrontendcheckBox.Name = "FrontendcheckBox";
            this.FrontendcheckBox.Size = new System.Drawing.Size(68, 17);
            this.FrontendcheckBox.TabIndex = 6;
            this.FrontendcheckBox.Text = "Frontend";
            this.FrontendcheckBox.UseVisualStyleBackColor = true;
            // 
            // backendcheckBox
            // 
            this.backendcheckBox.AutoSize = true;
            this.backendcheckBox.Checked = true;
            this.backendcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backendcheckBox.Location = new System.Drawing.Point(185, 93);
            this.backendcheckBox.Name = "backendcheckBox";
            this.backendcheckBox.Size = new System.Drawing.Size(69, 17);
            this.backendcheckBox.TabIndex = 7;
            this.backendcheckBox.Text = "Backend";
            this.backendcheckBox.UseVisualStyleBackColor = true;
            // 
            // StatusAddToSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 127);
            this.Controls.Add(this.backendcheckBox);
            this.Controls.Add(this.FrontendcheckBox);
            this.Controls.Add(this.CheckIncheckBox);
            this.Controls.Add(this.Startbutton);
            this.Controls.Add(this.Closebutton);
            this.Controls.Add(this.Statuslabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StatusprogressBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatusAddToSourceControl";
            this.Text = "Add to source control";
            this.Load += new System.EventHandler(this.StatusAddToSourceControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar StatusprogressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Statuslabel;
        private System.Windows.Forms.Button Closebutton;
        private System.Windows.Forms.Button Startbutton;
        private System.Windows.Forms.CheckBox CheckIncheckBox;
        private System.Windows.Forms.CheckBox FrontendcheckBox;
        private System.Windows.Forms.CheckBox backendcheckBox;
    }
}