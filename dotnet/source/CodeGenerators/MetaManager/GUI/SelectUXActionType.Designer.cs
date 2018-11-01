
namespace Cdc.MetaManager.GUI
{
    partial class SelectUXActionType
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lvSelectObject = new ListViewSort();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbFilter3 = new System.Windows.Forms.TextBox();
            this.lblFilter3 = new System.Windows.Forms.Label();
            this.tbFilter2 = new System.Windows.Forms.TextBox();
            this.lblFilter2 = new System.Windows.Forms.Label();
            this.tbFilter1 = new System.Windows.Forms.TextBox();
            this.lblFilter1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnWorkflow = new System.Windows.Forms.RadioButton();
            this.rbtnJumpTo = new System.Windows.Forms.RadioButton();
            this.rbtnUnmapped = new System.Windows.Forms.RadioButton();
            this.rbtnDrilldown = new System.Windows.Forms.RadioButton();
            this.rbtnServiceMethod = new System.Windows.Forms.RadioButton();
            this.rbtnDialog = new System.Windows.Forms.RadioButton();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 465);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(797, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 465);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(792, 5);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.btnOK);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 431);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(792, 34);
            this.panel4.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(699, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(603, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(792, 426);
            this.panel5.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel7);
            this.groupBox2.Controls.Add(this.panel6);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(792, 283);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lvSelectObject);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 57);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(786, 223);
            this.panel7.TabIndex = 2;
            // 
            // lvSelectObject
            // 
            this.lvSelectObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSelectObject.FullRowSelect = true;
            this.lvSelectObject.HideSelection = false;
            this.lvSelectObject.Location = new System.Drawing.Point(0, 0);
            this.lvSelectObject.MultiSelect = false;
            this.lvSelectObject.Name = "lvSelectObject";
            this.lvSelectObject.Size = new System.Drawing.Size(786, 223);
            this.lvSelectObject.TabIndex = 0;
            this.lvSelectObject.UseCompatibleStateImageBehavior = false;
            this.lvSelectObject.View = System.Windows.Forms.View.Details;
            this.lvSelectObject.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvSelectObject_ItemSelectionChanged);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnSearch);
            this.panel6.Controls.Add(this.tbFilter3);
            this.panel6.Controls.Add(this.lblFilter3);
            this.panel6.Controls.Add(this.tbFilter2);
            this.panel6.Controls.Add(this.lblFilter2);
            this.panel6.Controls.Add(this.tbFilter1);
            this.panel6.Controls.Add(this.lblFilter1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 16);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(786, 41);
            this.panel6.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(692, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tbFilter3
            // 
            this.tbFilter3.Location = new System.Drawing.Point(339, 16);
            this.tbFilter3.Name = "tbFilter3";
            this.tbFilter3.Size = new System.Drawing.Size(160, 20);
            this.tbFilter3.TabIndex = 5;
            this.tbFilter3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFilter1_KeyDown);
            // 
            // lblFilter3
            // 
            this.lblFilter3.AutoSize = true;
            this.lblFilter3.Location = new System.Drawing.Point(336, 0);
            this.lblFilter3.Name = "lblFilter3";
            this.lblFilter3.Size = new System.Drawing.Size(35, 13);
            this.lblFilter3.TabIndex = 4;
            this.lblFilter3.Text = "Filter3";
            // 
            // tbFilter2
            // 
            this.tbFilter2.Location = new System.Drawing.Point(173, 16);
            this.tbFilter2.Name = "tbFilter2";
            this.tbFilter2.Size = new System.Drawing.Size(160, 20);
            this.tbFilter2.TabIndex = 3;
            this.tbFilter2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFilter1_KeyDown);
            // 
            // lblFilter2
            // 
            this.lblFilter2.AutoSize = true;
            this.lblFilter2.Location = new System.Drawing.Point(170, 0);
            this.lblFilter2.Name = "lblFilter2";
            this.lblFilter2.Size = new System.Drawing.Size(35, 13);
            this.lblFilter2.TabIndex = 2;
            this.lblFilter2.Text = "Filter2";
            // 
            // tbFilter1
            // 
            this.tbFilter1.Location = new System.Drawing.Point(7, 16);
            this.tbFilter1.Name = "tbFilter1";
            this.tbFilter1.Size = new System.Drawing.Size(160, 20);
            this.tbFilter1.TabIndex = 1;
            this.tbFilter1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFilter1_KeyDown);
            // 
            // lblFilter1
            // 
            this.lblFilter1.AutoSize = true;
            this.lblFilter1.Location = new System.Drawing.Point(4, 0);
            this.lblFilter1.Name = "lblFilter1";
            this.lblFilter1.Size = new System.Drawing.Size(35, 13);
            this.lblFilter1.TabIndex = 0;
            this.lblFilter1.Text = "Filter1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnWorkflow);
            this.groupBox1.Controls.Add(this.rbtnJumpTo);
            this.groupBox1.Controls.Add(this.rbtnUnmapped);
            this.groupBox1.Controls.Add(this.rbtnDrilldown);
            this.groupBox1.Controls.Add(this.rbtnServiceMethod);
            this.groupBox1.Controls.Add(this.rbtnDialog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(792, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Action Type";
            // 
            // rbtnWorkflow
            // 
            this.rbtnWorkflow.AutoSize = true;
            this.rbtnWorkflow.Location = new System.Drawing.Point(7, 79);
            this.rbtnWorkflow.Name = "rbtnWorkflow";
            this.rbtnWorkflow.Size = new System.Drawing.Size(70, 17);
            this.rbtnWorkflow.TabIndex = 5;
            this.rbtnWorkflow.Text = "Workflow";
            this.rbtnWorkflow.UseVisualStyleBackColor = true;
            // 
            // rbtnJumpTo
            // 
            this.rbtnJumpTo.AutoSize = true;
            this.rbtnJumpTo.Location = new System.Drawing.Point(7, 99);
            this.rbtnJumpTo.Name = "rbtnJumpTo";
            this.rbtnJumpTo.Size = new System.Drawing.Size(66, 17);
            this.rbtnJumpTo.TabIndex = 4;
            this.rbtnJumpTo.Text = "Jump To";
            this.rbtnJumpTo.UseVisualStyleBackColor = true;
            // 
            // rbtnUnmapped
            // 
            this.rbtnUnmapped.AutoSize = true;
            this.rbtnUnmapped.Location = new System.Drawing.Point(7, 119);
            this.rbtnUnmapped.Name = "rbtnUnmapped";
            this.rbtnUnmapped.Size = new System.Drawing.Size(165, 17);
            this.rbtnUnmapped.TabIndex = 3;
            this.rbtnUnmapped.Text = "Unmapped (Example: Cancel)";
            this.rbtnUnmapped.UseVisualStyleBackColor = true;
            this.rbtnUnmapped.CheckedChanged += new System.EventHandler(this.rbtnUnmapped_CheckedChanged);
            // 
            // rbtnDrilldown
            // 
            this.rbtnDrilldown.AutoSize = true;
            this.rbtnDrilldown.Location = new System.Drawing.Point(7, 59);
            this.rbtnDrilldown.Name = "rbtnDrilldown";
            this.rbtnDrilldown.Size = new System.Drawing.Size(68, 17);
            this.rbtnDrilldown.TabIndex = 2;
            this.rbtnDrilldown.Text = "Drilldown";
            this.rbtnDrilldown.UseVisualStyleBackColor = true;
            // 
            // rbtnServiceMethod
            // 
            this.rbtnServiceMethod.AutoSize = true;
            this.rbtnServiceMethod.Location = new System.Drawing.Point(7, 39);
            this.rbtnServiceMethod.Name = "rbtnServiceMethod";
            this.rbtnServiceMethod.Size = new System.Drawing.Size(100, 17);
            this.rbtnServiceMethod.TabIndex = 1;
            this.rbtnServiceMethod.Text = "Service Method";
            this.rbtnServiceMethod.UseVisualStyleBackColor = true;
            this.rbtnServiceMethod.CheckedChanged += new System.EventHandler(this.rbtnServiceMethod_CheckedChanged);
            // 
            // rbtnDialog
            // 
            this.rbtnDialog.AutoSize = true;
            this.rbtnDialog.Location = new System.Drawing.Point(7, 19);
            this.rbtnDialog.Name = "rbtnDialog";
            this.rbtnDialog.Size = new System.Drawing.Size(55, 17);
            this.rbtnDialog.TabIndex = 0;
            this.rbtnDialog.Text = "Dialog";
            this.rbtnDialog.UseVisualStyleBackColor = true;
            this.rbtnDialog.CheckedChanged += new System.EventHandler(this.rbtnDialog_CheckedChanged);
            // 
            // SelectUXActionType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(802, 465);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SelectUXActionType";
            this.Text = "Select Action Type";
            this.Load += new System.EventHandler(this.SelectUXActionType_Load);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox2;
        private ListViewSort lvSelectObject;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnServiceMethod;
        private System.Windows.Forms.RadioButton rbtnDialog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox tbFilter1;
        private System.Windows.Forms.Label lblFilter1;
        private System.Windows.Forms.TextBox tbFilter2;
        private System.Windows.Forms.Label lblFilter2;
        private System.Windows.Forms.TextBox tbFilter3;
        private System.Windows.Forms.Label lblFilter3;
        private System.Windows.Forms.RadioButton rbtnUnmapped;
        private System.Windows.Forms.RadioButton rbtnDrilldown;
        private System.Windows.Forms.RadioButton rbtnJumpTo;
        private System.Windows.Forms.RadioButton rbtnWorkflow;
        private System.Windows.Forms.Button btnSearch;
    }
}