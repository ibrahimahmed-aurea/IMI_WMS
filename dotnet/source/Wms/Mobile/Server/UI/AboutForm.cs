using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Imi.Wms.Mobile.Server.UI
{
    /// <summary>
    /// Summary description for AboutForm.
    /// </summary>
    public class AboutForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Timer BlinkTimer;
        private System.Windows.Forms.Label blinkLbl;
        private System.Windows.Forms.Label productLabel;
        private System.Windows.Forms.Label versionLbl;
        private System.ComponentModel.IContainer components;

        public AboutForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.blinkLbl = new System.Windows.Forms.Label();
            this.BlinkTimer = new System.Windows.Forms.Timer(this.components);
            this.productLabel = new System.Windows.Forms.Label();
            this.versionLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(700, 450);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.PictureBox_Click);
            // 
            // blinkLbl
            // 
            this.blinkLbl.BackColor = System.Drawing.Color.White;
            this.blinkLbl.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blinkLbl.Location = new System.Drawing.Point(562, 392);
            this.blinkLbl.Name = "blinkLbl";
            this.blinkLbl.Size = new System.Drawing.Size(134, 18);
            this.blinkLbl.TabIndex = 3;
            this.blinkLbl.Text = "Click to Close";
            this.blinkLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.blinkLbl.Click += new System.EventHandler(this.PictureBox_Click);
            // 
            // BlinkTimer
            // 
            this.BlinkTimer.Interval = 180;
            this.BlinkTimer.Tick += new System.EventHandler(this.BlinkTimer_Tick);
            // 
            // productLabel
            // 
            this.productLabel.AutoSize = true;
            this.productLabel.BackColor = System.Drawing.Color.White;
            this.productLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productLabel.ForeColor = System.Drawing.Color.Black;
            this.productLabel.Location = new System.Drawing.Point(58, 161);
            this.productLabel.Name = "productLabel";
            this.productLabel.Size = new System.Drawing.Size(201, 18);
            this.productLabel.TabIndex = 4;
            this.productLabel.Text = "IMI iWMS Thin Client Server";
            // 
            // versionLbl
            // 
            this.versionLbl.AutoSize = true;
            this.versionLbl.BackColor = System.Drawing.Color.White;
            this.versionLbl.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLbl.ForeColor = System.Drawing.Color.Black;
            this.versionLbl.Location = new System.Drawing.Point(58, 182);
            this.versionLbl.Name = "versionLbl";
            this.versionLbl.Size = new System.Drawing.Size(61, 18);
            this.versionLbl.TabIndex = 7;
            this.versionLbl.Text = "Version";
            // 
            // AboutForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.versionLbl);
            this.Controls.Add(this.productLabel);
            this.Controls.Add(this.blinkLbl);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutForm";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private int BlinkCycle = 0;
        private string originalText;

        private void BlinkTimer_Tick(object sender, System.EventArgs e)
        {
            BlinkCycle++;

            if (BlinkCycle < 6)
            {
                blinkLbl.Text = originalText + "..........".Substring(0, BlinkCycle);
            }
            else
            {
                switch (BlinkCycle)
                {
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 15:
                        blinkLbl.Visible = true;
                        break;
                    case 16:
                        blinkLbl.Text = originalText;
                        BlinkCycle = 0;
                        break;
                    default:
                        blinkLbl.Visible = !blinkLbl.Visible;
                        break;
                }
            }

            this.Refresh();
        }

        private void AboutForm_Load(object sender, System.EventArgs e)
        {
            originalText = blinkLbl.Text;
            versionLbl.Text = string.Format("{0} {1}", versionLbl.Text, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            BlinkTimer.Enabled = true;
        }

        private void PictureBox_Click(object sender, System.EventArgs e)
        {
            BlinkTimer.Enabled = false;
            Close();
        }
    }
}
