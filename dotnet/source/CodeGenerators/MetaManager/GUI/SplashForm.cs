using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Cdc.MetaManager.GUI
{
    /// <summary>
    /// Summary description for SplashForm.
    /// </summary>
    public class SplashForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label StatusLabel;
        private PictureBox pictureBox;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public SplashForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashForm));
            this.StatusLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusLabel
            // 
            this.StatusLabel.BackColor = System.Drawing.Color.White;
            this.StatusLabel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(143, 236);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(323, 29);
            this.StatusLabel.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(700, 450);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // SplashForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.StatusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashForm";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.Activated += new System.EventHandler(this.SplashForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private bool init;

        private void SplashForm_Activated(object sender, System.EventArgs e)
        {
            StatusLabel.Text = "Initializing";
            this.Refresh();
            
            init = true;

            try
            {
                while (init)
                {
                    Thread.Sleep(250);

                    for (int i = 0; i < 5; i++)
                    {
                        StatusLabel.Text += ".";
                        this.Refresh();
                        Thread.Sleep(250);
                    }
                }

                Thread.Sleep(600);

                double fadeTime = 0.5;
                DateTime start = DateTime.Now;
                DateTime stop = start.AddSeconds(fadeTime);

                while (true)
                {
                    TimeSpan ts = DateTime.Now.Subtract(start);
                    double progress = ((fadeTime - ts.TotalSeconds) / fadeTime);
                    if (progress > 0)
                    {
                        this.Opacity = progress;
                    }
                    else
                    {
                        this.Opacity = 0;
                        break;
                    }

                    this.Refresh();
                    Thread.Sleep(5);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Close();
            }

        }

        public void Fade()
        {
            init = false;
        }
    }
}
