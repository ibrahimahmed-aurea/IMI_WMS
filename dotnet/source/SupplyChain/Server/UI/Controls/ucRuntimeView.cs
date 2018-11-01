using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Imi.SupplyChain.Server.UI.Controls;
using Imi.SupplyChain.Server.UI.Configuration;

namespace Imi.SupplyChain.Server.UI
{
    /// <summary>
    /// Summary description for ucRuntimeView.
    /// </summary>
    public class ucRuntimeView : System.Windows.Forms.UserControl
    {
        private Imi.SupplyChain.Server.UI.Controls.WhDataGrid JobGrid;
        private System.Windows.Forms.DataGridTableStyle JobGridTableStyle;
        private System.Windows.Forms.DataGridTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridTextBoxColumn StatusColumn;
        private System.Windows.Forms.DataGridTextBoxColumn NumberOfRunsColumn;
        private System.Windows.Forms.DataGridTextBoxColumn LastRunColumn;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
        private System.Data.DataTable JobRuntimeType;
        private System.Data.DataSet JobRuntimeDS;
        private System.Windows.Forms.DataGridTextBoxColumn RealTimeColumn;
        private System.Data.DataColumn dataColumn7;
        private System.Data.DataView JobRuntimeDataView;
        private System.Windows.Forms.Timer RefreshTimer;
        private ContextMenuStrip jobContextMenuStrip;
        private ToolStripMenuItem startJobMenuItem;
        private ToolStripMenuItem stopJobMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem startAllMenuItem;
        private ToolStripMenuItem stopAllMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem logLevelMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem RefreshListMenuItem;
        private ToolStripMenuItem dbLogLevelMenuItem;
        private ToolStripMenuItem LogNoneMenuItem;
        private ToolStripMenuItem LogNormalMenuItem;
        private ToolStripMenuItem LogDebugMenuItem;
        private ToolStripMenuItem JobLogLevelMenuItem;
        private ToolStripMenuItem LogJobOffMenuItem;
        private ToolStripMenuItem LogJobErrorMenuItem;
        private ToolStripMenuItem LogJobWarningMenuItem;
        private ToolStripMenuItem LogJobInfoMenuItem;
        private ToolStripMenuItem LogJobVerboseMenuItem;
        private System.ComponentModel.IContainer components;

        public ucRuntimeView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucRuntimeView));
            this.JobGrid = new Imi.SupplyChain.Server.UI.Controls.WhDataGrid();
            this.jobContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startJobMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopJobMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.startAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.logLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dbLogLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogNoneMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogNormalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogDebugMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.JobLogLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogJobOffMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogJobErrorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogJobWarningMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogJobInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogJobVerboseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.JobRuntimeDataView = new System.Data.DataView();
            this.JobRuntimeType = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.JobGridTableStyle = new System.Windows.Forms.DataGridTableStyle();
            this.NameColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            this.NumberOfRunsColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            this.LastRunColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            this.RealTimeColumn = new System.Windows.Forms.DataGridTextBoxColumn();
            this.JobRuntimeDS = new System.Data.DataSet();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.JobGrid)).BeginInit();
            this.jobContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JobRuntimeDataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobRuntimeType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobRuntimeDS)).BeginInit();
            this.SuspendLayout();
            // 
            // JobGrid
            // 
            this.JobGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.JobGrid.CaptionBackColor = System.Drawing.SystemColors.ControlDark;
            this.JobGrid.CaptionCountText = "(%s Items)";
            this.JobGrid.CaptionCountVisible = true;
            this.JobGrid.CaptionForeColor = System.Drawing.SystemColors.ControlText;
            this.JobGrid.CaptionText = "Jobs";
            this.JobGrid.ContextMenuStrip = this.jobContextMenuStrip;
            this.JobGrid.DataMember = "";
            this.JobGrid.DataSource = this.JobRuntimeDataView;
            this.JobGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JobGrid.FullRowKeyField = "Name";
            this.JobGrid.FullRowSelect = true;
            this.JobGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.JobGrid.LinkColor = System.Drawing.SystemColors.MenuHighlight;
            this.JobGrid.Location = new System.Drawing.Point(0, 0);
            this.JobGrid.Name = "JobGrid";
            this.JobGrid.ReadOnly = true;
            this.JobGrid.RowHeaderWidth = 75;
            this.JobGrid.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.JobGrid.SelectionForeColor = System.Drawing.SystemColors.MenuText;
            this.JobGrid.Size = new System.Drawing.Size(724, 479);
            this.JobGrid.TabIndex = 0;
            this.JobGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.JobGridTableStyle});
            // 
            // jobContextMenuStrip
            // 
            this.jobContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startJobMenuItem,
            this.stopJobMenuItem,
            this.toolStripMenuItem1,
            this.startAllMenuItem,
            this.stopAllMenuItem,
            this.toolStripMenuItem2,
            this.logLevelMenuItem,
            this.toolStripMenuItem3,
            this.RefreshListMenuItem});
            this.jobContextMenuStrip.Name = "jobContextMenuStrip";
            this.jobContextMenuStrip.Size = new System.Drawing.Size(143, 154);
            // 
            // startJobMenuItem
            // 
            this.startJobMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("startJobMenuItem.Image")));
            this.startJobMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.startJobMenuItem.Name = "startJobMenuItem";
            this.startJobMenuItem.Size = new System.Drawing.Size(142, 22);
            this.startJobMenuItem.Text = "&Start Job";
            this.startJobMenuItem.Click += new System.EventHandler(this.StartJobMenuItem_Click);
            // 
            // stopJobMenuItem
            // 
            this.stopJobMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("stopJobMenuItem.Image")));
            this.stopJobMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.stopJobMenuItem.Name = "stopJobMenuItem";
            this.stopJobMenuItem.Size = new System.Drawing.Size(142, 22);
            this.stopJobMenuItem.Text = "Sto&p Job";
            this.stopJobMenuItem.Click += new System.EventHandler(this.StopJobMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(139, 6);
            // 
            // startAllMenuItem
            // 
            this.startAllMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("startAllMenuItem.Image")));
            this.startAllMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.startAllMenuItem.Name = "startAllMenuItem";
            this.startAllMenuItem.Size = new System.Drawing.Size(142, 22);
            this.startAllMenuItem.Text = "Start &All";
            this.startAllMenuItem.Click += new System.EventHandler(this.StartAllMenuItem_Click);
            // 
            // stopAllMenuItem
            // 
            this.stopAllMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("stopAllMenuItem.Image")));
            this.stopAllMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.stopAllMenuItem.Name = "stopAllMenuItem";
            this.stopAllMenuItem.Size = new System.Drawing.Size(142, 22);
            this.stopAllMenuItem.Text = "St&op All";
            this.stopAllMenuItem.Click += new System.EventHandler(this.StopAllMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(139, 6);
            // 
            // logLevelMenuItem
            // 
            this.logLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dbLogLevelMenuItem,
            this.JobLogLevelMenuItem});
            this.logLevelMenuItem.Name = "logLevelMenuItem";
            this.logLevelMenuItem.Size = new System.Drawing.Size(142, 22);
            this.logLevelMenuItem.Text = "Log Level";
            // 
            // dbLogLevelMenuItem
            // 
            this.dbLogLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LogNoneMenuItem,
            this.LogNormalMenuItem,
            this.LogDebugMenuItem});
            this.dbLogLevelMenuItem.Name = "dbLogLevelMenuItem";
            this.dbLogLevelMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dbLogLevelMenuItem.Text = "Database";
            this.dbLogLevelMenuItem.DropDownOpening += new System.EventHandler(this.LogLevelMenuItem_Popup);
            // 
            // LogNoneMenuItem
            // 
            this.LogNoneMenuItem.Name = "LogNoneMenuItem";
            this.LogNoneMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogNoneMenuItem.Text = "Off";
            this.LogNoneMenuItem.Click += new System.EventHandler(this.LogMenuItem_Click);
            // 
            // LogNormalMenuItem
            // 
            this.LogNormalMenuItem.Name = "LogNormalMenuItem";
            this.LogNormalMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogNormalMenuItem.Text = "Info";
            this.LogNormalMenuItem.Click += new System.EventHandler(this.LogMenuItem_Click);
            // 
            // LogDebugMenuItem
            // 
            this.LogDebugMenuItem.Name = "LogDebugMenuItem";
            this.LogDebugMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogDebugMenuItem.Text = "Verbose";
            this.LogDebugMenuItem.Click += new System.EventHandler(this.LogMenuItem_Click);
            // 
            // JobLogLevelMenuItem
            // 
            this.JobLogLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LogJobOffMenuItem,
            this.LogJobErrorMenuItem,
            this.LogJobWarningMenuItem,
            this.LogJobInfoMenuItem,
            this.LogJobVerboseMenuItem});
            this.JobLogLevelMenuItem.Name = "JobLogLevelMenuItem";
            this.JobLogLevelMenuItem.Size = new System.Drawing.Size(152, 22);
            this.JobLogLevelMenuItem.Text = "Job";
            this.JobLogLevelMenuItem.DropDownOpening += new System.EventHandler(this.JobLogLevelMenuItem_Popup);
            // 
            // LogJobOffMenuItem
            // 
            this.LogJobOffMenuItem.Name = "LogJobOffMenuItem";
            this.LogJobOffMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogJobOffMenuItem.Text = "Off";
            this.LogJobOffMenuItem.Click += new System.EventHandler(this.LogJobMenuItem_Click);
            // 
            // LogJobErrorMenuItem
            // 
            this.LogJobErrorMenuItem.Name = "LogJobErrorMenuItem";
            this.LogJobErrorMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogJobErrorMenuItem.Text = "Error";
            this.LogJobErrorMenuItem.Click += new System.EventHandler(this.LogJobMenuItem_Click);
            // 
            // LogJobWarningMenuItem
            // 
            this.LogJobWarningMenuItem.Name = "LogJobWarningMenuItem";
            this.LogJobWarningMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogJobWarningMenuItem.Text = "Warning";
            this.LogJobWarningMenuItem.Click += new System.EventHandler(this.LogJobMenuItem_Click);
            // 
            // LogJobInfoMenuItem
            // 
            this.LogJobInfoMenuItem.Name = "LogJobInfoMenuItem";
            this.LogJobInfoMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogJobInfoMenuItem.Text = "Info";
            this.LogJobInfoMenuItem.Click += new System.EventHandler(this.LogJobMenuItem_Click);
            // 
            // LogJobVerboseMenuItem
            // 
            this.LogJobVerboseMenuItem.Name = "LogJobVerboseMenuItem";
            this.LogJobVerboseMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LogJobVerboseMenuItem.Text = "Verbose";
            this.LogJobVerboseMenuItem.Click += new System.EventHandler(this.LogJobMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(139, 6);
            // 
            // RefreshListMenuItem
            // 
            this.RefreshListMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RefreshListMenuItem.Image")));
            this.RefreshListMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.RefreshListMenuItem.Name = "RefreshListMenuItem";
            this.RefreshListMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.RefreshListMenuItem.Size = new System.Drawing.Size(142, 22);
            this.RefreshListMenuItem.Text = "&Refresh";
            this.RefreshListMenuItem.Click += new System.EventHandler(this.RefreshListMenuItem_Click);
            // 
            // JobRuntimeDataView
            // 
            this.JobRuntimeDataView.AllowDelete = false;
            this.JobRuntimeDataView.AllowEdit = false;
            this.JobRuntimeDataView.AllowNew = false;
            this.JobRuntimeDataView.Sort = "Name";
            this.JobRuntimeDataView.Table = this.JobRuntimeType;
            // 
            // JobRuntimeType
            // 
            this.JobRuntimeType.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6,
            this.dataColumn7});
            this.JobRuntimeType.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "Name"}, true)});
            this.JobRuntimeType.PrimaryKey = new System.Data.DataColumn[] {
        this.dataColumn1};
            this.JobRuntimeType.TableName = "JobRuntimeType";
            // 
            // dataColumn1
            // 
            this.dataColumn1.AllowDBNull = false;
            this.dataColumn1.ColumnMapping = System.Data.MappingType.Attribute;
            this.dataColumn1.ColumnName = "Name";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnMapping = System.Data.MappingType.Attribute;
            this.dataColumn2.ColumnName = "Status";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnMapping = System.Data.MappingType.Attribute;
            this.dataColumn3.ColumnName = "RunCount";
            this.dataColumn3.DataType = typeof(long);
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnMapping = System.Data.MappingType.Attribute;
            this.dataColumn4.ColumnName = "RunStarted";
            this.dataColumn4.DataType = typeof(System.DateTime);
            // 
            // dataColumn5
            // 
            this.dataColumn5.ColumnMapping = System.Data.MappingType.Attribute;
            this.dataColumn5.ColumnName = "TotalProcessorTime";
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnMapping = System.Data.MappingType.Attribute;
            this.dataColumn6.ColumnName = "TotalRealTime";
            // 
            // dataColumn7
            // 
            this.dataColumn7.AllowDBNull = false;
            this.dataColumn7.Caption = "Thread Id";
            this.dataColumn7.ColumnMapping = System.Data.MappingType.Attribute;
            this.dataColumn7.ColumnName = "ThreadId";
            // 
            // JobGridTableStyle
            // 
            this.JobGridTableStyle.DataGrid = this.JobGrid;
            this.JobGridTableStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.NameColumn,
            this.StatusColumn,
            this.NumberOfRunsColumn,
            this.LastRunColumn,
            this.RealTimeColumn});
            this.JobGridTableStyle.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
            this.JobGridTableStyle.HeaderForeColor = System.Drawing.SystemColors.WindowText;
            this.JobGridTableStyle.MappingName = "JobRuntimeType";
            this.JobGridTableStyle.ReadOnly = true;
            this.JobGridTableStyle.RowHeadersVisible = false;
            this.JobGridTableStyle.RowHeaderWidth = 75;
            this.JobGridTableStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.JobGridTableStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            // 
            // NameColumn
            // 
            this.NameColumn.Format = "";
            this.NameColumn.FormatInfo = null;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.MappingName = "Name";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.Width = 140;
            // 
            // StatusColumn
            // 
            this.StatusColumn.Format = "";
            this.StatusColumn.FormatInfo = null;
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.MappingName = "Status";
            this.StatusColumn.ReadOnly = true;
            this.StatusColumn.Width = 70;
            // 
            // NumberOfRunsColumn
            // 
            this.NumberOfRunsColumn.Format = "";
            this.NumberOfRunsColumn.FormatInfo = null;
            this.NumberOfRunsColumn.HeaderText = "Run #";
            this.NumberOfRunsColumn.MappingName = "RunCount";
            this.NumberOfRunsColumn.ReadOnly = true;
            this.NumberOfRunsColumn.Width = 50;
            // 
            // LastRunColumn
            // 
            this.LastRunColumn.Format = "G";
            this.LastRunColumn.FormatInfo = null;
            this.LastRunColumn.HeaderText = "Last Start";
            this.LastRunColumn.MappingName = "RunStarted";
            this.LastRunColumn.ReadOnly = true;
            this.LastRunColumn.Width = 140;
            // 
            // RealTimeColumn
            // 
            this.RealTimeColumn.Format = "";
            this.RealTimeColumn.FormatInfo = null;
            this.RealTimeColumn.HeaderText = "Execution Time";
            this.RealTimeColumn.MappingName = "TotalRealTime";
            this.RealTimeColumn.Width = 110;
            // 
            // JobRuntimeDS
            // 
            this.JobRuntimeDS.DataSetName = "JobRuntimeDataSet";
            this.JobRuntimeDS.Locale = new System.Globalization.CultureInfo("sv-SE");
            this.JobRuntimeDS.Tables.AddRange(new System.Data.DataTable[] {
            this.JobRuntimeType});
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Interval = 1500;
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // ucRuntimeView
            // 
            this.Controls.Add(this.JobGrid);
            this.Name = "ucRuntimeView";
            this.Size = new System.Drawing.Size(724, 479);
            ((System.ComponentModel.ISupportInitialize)(this.JobGrid)).EndInit();
            this.jobContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.JobRuntimeDataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobRuntimeType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JobRuntimeDS)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private ManagedConnection currentConnection;

        private bool OldRefreshTimerEnabled;

        public void HoldRefresh()
        {
            OldRefreshTimerEnabled = RefreshTimer.Enabled;
            RefreshTimer.Enabled = false;
        }

        public void ResumeRefresh()
        {
            RefreshTimer.Enabled = OldRefreshTimerEnabled;
        }

        public void RefreshView(bool initialize)
        {
            try
            {
                if (currentConnection == null)
                    return;

                if (! currentConnection.Connected)
                    return;

                RefreshTimer.Enabled = false;
                String s = currentConnection.Ps();
                DataSet ds = JobRuntimeDS.Clone();
                ds.Clear();
                ds.ReadXml(new StringReader(s), XmlReadMode.Auto);

                JobGrid.SuspendLayout();

                if (initialize)
                    JobRuntimeDS.Clear();

                JobRuntimeDS.Merge(ds.Tables["JobRuntimeType"]);

                ServerConfigRefreshRate r = UIConfigFileHandler.Instance().Config.RefreshRate;

                switch (r)
                {
                    case ServerConfigRefreshRate.High:
                        RefreshTimer.Interval = 1000;
                        RefreshTimer.Enabled = true;
                        break;
                    case ServerConfigRefreshRate.Low:
                        RefreshTimer.Interval = 30000;
                        RefreshTimer.Enabled = true;
                        break;
                    case ServerConfigRefreshRate.Normal:
                        RefreshTimer.Interval = 10000;
                        RefreshTimer.Enabled = true;
                        break;
                    case ServerConfigRefreshRate.Paused:
                        RefreshTimer.Enabled = false;
                        break;
                }

            }
            catch (Exception ex)
            {
                currentConnection.Disconnect();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                JobGrid.ResumeLayout(true);
            }
        }

        public void ShowView(ManagedConnection c)
        {
            this.BringToFront();
            currentConnection = c;
            RefreshView(true);
            JobGrid.SelectRow(0);
        }

        private String Field(String name)
        {
            CurrencyManager myCurrencyManager = (CurrencyManager)this.BindingContext[JobRuntimeDataView];
            return ((myCurrencyManager.Current as DataRowView).Row[name].ToString());
        }

        public void StartJob()
        {
            StartJobMenuItem_Click(null, null);
        }

        public void StopJob()
        {
            StopJobMenuItem_Click(null, null);
        }

        private void StartJobMenuItem_Click(object sender, System.EventArgs e)
        {
            String job = Field("Name");
            if (job != "")
            {
                try
                {
                    String cc = currentConnection.Instance.StartJob(job);
                    if (cc != "ok")
                    {
                        MessageBox.Show(String.Format("Failed to start job {0}. {1}", job, cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        RefreshView(false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, String.Format("Error when starting job {0}\n{1}\n{2}", job, ex.Message, ex.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StopJobMenuItem_Click(object sender, System.EventArgs e)
        {
            String job = Field("Name");
            if (job != "")
            {
                if (MessageBox.Show(this, String.Format("Do you really want to stop {0} ?", job), "Stop Job", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        String cc = currentConnection.Instance.StopJob(job);
                        if (cc != "ok")
                        {
                            MessageBox.Show(String.Format("Failed to stop jobs {0}. {1}", job, cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            RefreshView(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, String.Format("Error when stopping job {0}\n{1}\n{2}", job, ex.Message, ex.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RefreshListMenuItem_Click(object sender, System.EventArgs e)
        {
            if (currentConnection != null)
            {
                if (currentConnection.Connected)
                {
                    RefreshView(false);
                }
            }
        }

        private void JobContextMenu_Popup(object sender, System.EventArgs e)
        {
            startJobMenuItem.Enabled = (Field("Name") != "");
            stopJobMenuItem.Enabled = (Field("Name") != "");
            RefreshListMenuItem.Enabled = true;
        }

        private void JobGrid_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RefreshView(false);
            }
        }

        public void StartAllMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                String cc = currentConnection.Instance.StartAll();
                if (cc != "ok")
                {
                    MessageBox.Show(String.Format("Failed to start all jobs. {0}", cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    RefreshView(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, String.Format("Error when starting jobs.\n{0}\n{1}", ex.Message, ex.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StopAllMenuItem_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show(this, String.Format("Do you really want to stop all jobs in the {0} instance ?", currentConnection.Config.DisplayName), "Stop All", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    String cc = currentConnection.Instance.StopAll();
                    if (cc != "ok")
                    {
                        MessageBox.Show(String.Format("Failed to stop all jobs. {0}", cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        RefreshView(false);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, String.Format("Error when stopping jobs.\n{0}\n{1}", ex.Message, ex.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshTimer_Tick(object sender, System.EventArgs e)
        {
            if (currentConnection.Connected)
                RefreshView(false);
            else
                RefreshTimer.Enabled = false;
        }

        private void LogMenuItem_Click(object sender, System.EventArgs e)
        {
            String nuLevel = "2";

            if (sender == LogDebugMenuItem)
            {
                nuLevel = "1";
            }
            else
            {
                if (sender == LogNoneMenuItem)
                {
                    nuLevel = "99";
                }
            }

            String job = Field("Name");

            if (job != "")
            {
                try
                {
                    String cc = currentConnection.Instance.SetParameter(job, "LogLevel", nuLevel);
                    if (cc != "ok")
                    {
                        MessageBox.Show(String.Format("Failed to change the log level for {0}. {1}", job, cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        RefreshView(false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, String.Format("Error when changing log level for job {0}\n{1}\n{2}", job, ex.Message, ex.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogJobMenuItem_Click(object sender, System.EventArgs e)
        {
            string level;

            if (sender == LogJobOffMenuItem)
                level = "Off";
            else if (sender == LogJobErrorMenuItem)
                level = "Error";
            else if (sender == LogJobWarningMenuItem)
                level = "Warning";
            else if (sender == LogJobInfoMenuItem)
                level = "Information";
            else if (sender == LogJobVerboseMenuItem)
                level = "Verbose";
            else
                level = "Off";
                
            string job = Field("Name");

            if (!string.IsNullOrEmpty(job))
            {
                try
                {
                    String cc = currentConnection.Instance.SetTraceLevel(job, level);

                    if (cc != "ok")
                    {
                        MessageBox.Show(String.Format("Failed to change the log level for {0}. {1}", job, cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        RefreshView(false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, String.Format("Error when changing log level for job {0}\n{1}\n{2}", job, ex.Message, ex.StackTrace), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogLevelMenuItem_Popup(object sender, System.EventArgs e)
        {

            String job = Field("Name");

            if (job != "")
            {
                try
                {

                    foreach (ToolStripMenuItem t in dbLogLevelMenuItem.DropDown.Items)
                        t.Checked = false;
                    
                    String cc = currentConnection.Instance.GetParameter(job, "LogLevel");

                    if (cc != "")
                    {
                        switch (cc)
                        {
                            case "1":
                                LogDebugMenuItem.Checked = true;
                                break;

                            case "2":
                                LogNormalMenuItem.Checked = true;
                                break;

                            default:
                                LogNoneMenuItem.Checked = true;
                                break;
                        }

                        RefreshView(false);
                    }
                }
                catch (Exception) // ex) 
                {
                    //          MessageBox.Show(this,String.Format("Error when changing log level for job {0}\n{1}\n{2}",job,ex.Message, ex.StackTrace),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }

        }

                

        private void JobLogLevelMenuItem_Popup(object sender, EventArgs e)
        {
            string job = Field("Name");

            if (job != "")
            {
                try
                {
                    foreach (ToolStripMenuItem t in JobLogLevelMenuItem.DropDown.Items)
                        t.Checked = false;

                    string cc = currentConnection.Instance.GetTraceLevel(job);

                    if (!string.IsNullOrEmpty(cc))
                    {
                        switch (cc.ToLower())
                        {
                            case "off":
                                LogJobOffMenuItem.Checked = true;
                                break;
                            case "error":
                                LogJobErrorMenuItem.Checked = true;
                                break;
                            case "warning":
                                LogJobWarningMenuItem.Checked = true;
                                break;
                            case "information":
                                LogJobInfoMenuItem.Checked = true;
                                break;
                            case "verbose":
                                LogJobVerboseMenuItem.Checked = true;
                                break;
                            default:
                                LogJobOffMenuItem.Checked = true;
                                break;
                        }

                        RefreshView(false);
                    }
                }
                catch (Exception) // ex) 
                {
                    //          MessageBox.Show(this,String.Format("Error when changing log level for job {0}\n{1}\n{2}",job,ex.Message, ex.StackTrace),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        
    }
}
