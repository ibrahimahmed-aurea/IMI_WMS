using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Imi.Wms.Mobile.Server.UI.Controls;
using Imi.Wms.Mobile.Server.UI.Configuration;
using Imi.Wms.Mobile.Server.Interface;

namespace Imi.Wms.Mobile.Server.UI
{
    /// <summary>
    /// Summary description for ucRuntimeView.
    /// </summary>
    public class ucRuntimeView : System.Windows.Forms.UserControl
    {
        private System.Data.DataView sessionDataView;
        private System.Windows.Forms.Timer refreshTimer;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem killMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem killAllMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem logLevelMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem RefreshListMenuItem;
        private ToolStripMenuItem clientLogLevelMenuItem;
        private ToolStripMenuItem clientLogOffMenuItem;
        private ToolStripMenuItem clientLogVerboseMenuItem;
        private ToolStripMenuItem serverLogLevelMenuItem;
        private ToolStripMenuItem serverLogOffMenuItem;
        private ToolStripMenuItem serverLogErrorMenuItem;
        private ToolStripMenuItem serverLogWarningMenuItem;
        private ToolStripMenuItem serverLogInfoMenuItem;
        private ToolStripMenuItem serverLogVerboseMenuItem;
        private DataTable sessionTable;
        private DataColumn dataColumn1;
        private DataSet sessionDataSet;
        private DataColumn dataColumn2;
        private DataColumn dataColumn3;
        private SplitContainer splitContainer1;
        private WhDataGrid sessionGrid;
        private DataGridTableStyle dataGridTableStyle1;
        private DataGridTextBoxColumn dataGridTextBoxColumn1;
        private DataGridTextBoxColumn dataGridTextBoxColumn2;
        private DataGridTextBoxColumn dataGridTextBoxColumn3;
        private Mobile.UI.Shared.RenderPanel renderPanel;
        private DataColumn dataColumn4;
        private DataGridTextBoxColumn dataGridTextBoxColumn4;
        private DataColumn dataColumn5;
        private DataGridTextBoxColumn dataGridTextBoxColumn5;
        private DataColumn dataColumn6;
        private DataGridTextBoxColumn dataGridTextBoxColumn6;
        private DataColumn dataColumn7;
        private DataColumn dataColumn8;
        private DataColumn dataColumn9;
        private DataGridTextBoxColumn dataGridTextBoxColumn7;
        private DataGridTextBoxColumn dataGridTextBoxColumn8;
        private DataGridTextBoxColumn dataGridTextBoxColumn9;
        private System.ComponentModel.IContainer components;

        public ucRuntimeView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
            CurrencyManager myCurrencyManager = (CurrencyManager)this.BindingContext[sessionDataView];
            myCurrencyManager.CurrentChanged += new EventHandler(myCurrencyManager_CurrentChanged);
        }

        void myCurrencyManager_CurrentChanged(object sender, EventArgs e)
        {
            string id = Field("Id");

            if (!string.IsNullOrEmpty(id))
            {
                StateResponse state = currentConnection.GetSessionRender(Field("Id"));
                
                if (state != null && state.Form != null)
                {
                    renderPanel.Visible = true;
                    renderPanel.Render(state.Form);
                    return;
                }
            }
            
            renderPanel.Visible = false;
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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.killMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.killAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.logLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientLogLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientLogOffMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientLogVerboseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverLogLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverLogOffMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverLogErrorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverLogWarningMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverLogInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverLogVerboseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionDataView = new System.Data.DataView();
            this.sessionTable = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn9 = new System.Data.DataColumn();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.sessionDataSet = new System.Data.DataSet();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.sessionGrid = new Imi.Wms.Mobile.Server.UI.Controls.WhDataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn7 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn6 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn8 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn9 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.renderPanel = new Imi.Wms.Mobile.UI.Shared.RenderPanel();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sessionDataView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sessionGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.killMenuItem,
            this.toolStripMenuItem1,
            this.killAllMenuItem,
            this.toolStripMenuItem2,
            this.logLevelMenuItem,
            this.toolStripMenuItem3,
            this.RefreshListMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(133, 110);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // killMenuItem
            // 
            this.killMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.killMenuItem.Name = "killMenuItem";
            this.killMenuItem.Size = new System.Drawing.Size(132, 22);
            this.killMenuItem.Text = "&Kill Session";
            this.killMenuItem.Click += new System.EventHandler(this.killSessionMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(129, 6);
            // 
            // killAllMenuItem
            // 
            this.killAllMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.killAllMenuItem.Name = "killAllMenuItem";
            this.killAllMenuItem.Size = new System.Drawing.Size(132, 22);
            this.killAllMenuItem.Text = "Kill &All";
            this.killAllMenuItem.Click += new System.EventHandler(this.KillAllMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(129, 6);
            // 
            // logLevelMenuItem
            // 
            this.logLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientLogLevelMenuItem,
            this.serverLogLevelMenuItem});
            this.logLevelMenuItem.Name = "logLevelMenuItem";
            this.logLevelMenuItem.Size = new System.Drawing.Size(132, 22);
            this.logLevelMenuItem.Text = "&Log Level";
            this.logLevelMenuItem.DropDownOpening += new System.EventHandler(this.logLevelMenuItem_DropDownOpening);
            // 
            // clientLogLevelMenuItem
            // 
            this.clientLogLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientLogOffMenuItem,
            this.clientLogVerboseMenuItem});
            this.clientLogLevelMenuItem.Name = "clientLogLevelMenuItem";
            this.clientLogLevelMenuItem.Size = new System.Drawing.Size(106, 22);
            this.clientLogLevelMenuItem.Text = "&Client";
            this.clientLogLevelMenuItem.DropDownOpening += new System.EventHandler(this.clientLogLevelMenuItem_Popup);
            // 
            // clientLogOffMenuItem
            // 
            this.clientLogOffMenuItem.Name = "clientLogOffMenuItem";
            this.clientLogOffMenuItem.Size = new System.Drawing.Size(116, 22);
            this.clientLogOffMenuItem.Text = "&Off";
            this.clientLogOffMenuItem.Click += new System.EventHandler(this.clientLogMenuItem_Click);
            // 
            // clientLogVerboseMenuItem
            // 
            this.clientLogVerboseMenuItem.Name = "clientLogVerboseMenuItem";
            this.clientLogVerboseMenuItem.Size = new System.Drawing.Size(116, 22);
            this.clientLogVerboseMenuItem.Text = "&Verbose";
            this.clientLogVerboseMenuItem.Click += new System.EventHandler(this.clientLogMenuItem_Click);
            // 
            // serverLogLevelMenuItem
            // 
            this.serverLogLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverLogOffMenuItem,
            this.serverLogErrorMenuItem,
            this.serverLogWarningMenuItem,
            this.serverLogInfoMenuItem,
            this.serverLogVerboseMenuItem});
            this.serverLogLevelMenuItem.Name = "serverLogLevelMenuItem";
            this.serverLogLevelMenuItem.Size = new System.Drawing.Size(106, 22);
            this.serverLogLevelMenuItem.Text = "&Server";
            this.serverLogLevelMenuItem.DropDownOpening += new System.EventHandler(this.serverLogLevelMenuItem_Popup);
            // 
            // serverLogOffMenuItem
            // 
            this.serverLogOffMenuItem.Name = "serverLogOffMenuItem";
            this.serverLogOffMenuItem.Size = new System.Drawing.Size(119, 22);
            this.serverLogOffMenuItem.Text = "&Off";
            this.serverLogOffMenuItem.Click += new System.EventHandler(this.serverLogMenuItem_Click);
            // 
            // serverLogErrorMenuItem
            // 
            this.serverLogErrorMenuItem.Name = "serverLogErrorMenuItem";
            this.serverLogErrorMenuItem.Size = new System.Drawing.Size(119, 22);
            this.serverLogErrorMenuItem.Text = "&Error";
            this.serverLogErrorMenuItem.Click += new System.EventHandler(this.serverLogMenuItem_Click);
            // 
            // serverLogWarningMenuItem
            // 
            this.serverLogWarningMenuItem.Name = "serverLogWarningMenuItem";
            this.serverLogWarningMenuItem.Size = new System.Drawing.Size(119, 22);
            this.serverLogWarningMenuItem.Text = "&Warning";
            this.serverLogWarningMenuItem.Click += new System.EventHandler(this.serverLogMenuItem_Click);
            // 
            // serverLogInfoMenuItem
            // 
            this.serverLogInfoMenuItem.Name = "serverLogInfoMenuItem";
            this.serverLogInfoMenuItem.Size = new System.Drawing.Size(119, 22);
            this.serverLogInfoMenuItem.Text = "&Info";
            this.serverLogInfoMenuItem.Click += new System.EventHandler(this.serverLogMenuItem_Click);
            // 
            // serverLogVerboseMenuItem
            // 
            this.serverLogVerboseMenuItem.Name = "serverLogVerboseMenuItem";
            this.serverLogVerboseMenuItem.Size = new System.Drawing.Size(119, 22);
            this.serverLogVerboseMenuItem.Text = "&Verbose";
            this.serverLogVerboseMenuItem.Click += new System.EventHandler(this.serverLogMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(129, 6);
            // 
            // RefreshListMenuItem
            // 
            this.RefreshListMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RefreshListMenuItem.Image")));
            this.RefreshListMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.RefreshListMenuItem.Name = "RefreshListMenuItem";
            this.RefreshListMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.RefreshListMenuItem.Size = new System.Drawing.Size(132, 22);
            this.RefreshListMenuItem.Text = "&Refresh";
            this.RefreshListMenuItem.Click += new System.EventHandler(this.RefreshListMenuItem_Click);
            // 
            // sessionDataView
            // 
            this.sessionDataView.AllowDelete = false;
            this.sessionDataView.AllowEdit = false;
            this.sessionDataView.AllowNew = false;
            this.sessionDataView.Table = this.sessionTable;
            // 
            // sessionTable
            // 
            this.sessionTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9});
            this.sessionTable.TableName = "Sessions";
            // 
            // dataColumn1
            // 
            this.dataColumn1.Caption = "Id";
            this.dataColumn1.ColumnName = "Id";
            // 
            // dataColumn2
            // 
            this.dataColumn2.Caption = "Application";
            this.dataColumn2.ColumnName = "Application";
            // 
            // dataColumn3
            // 
            this.dataColumn3.Caption = "Process Id";
            this.dataColumn3.ColumnName = "PID";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "LastActivity";
            // 
            // dataColumn5
            // 
            this.dataColumn5.Caption = "IP Address";
            this.dataColumn5.ColumnName = "IP";
            // 
            // dataColumn6
            // 
            this.dataColumn6.Caption = "User ID";
            this.dataColumn6.ColumnName = "UserId";
            // 
            // dataColumn7
            // 
            this.dataColumn7.Caption = "Terminal ID";
            this.dataColumn7.ColumnName = "TerminalId";
            // 
            // dataColumn8
            // 
            this.dataColumn8.ColumnName = "Version";
            // 
            // dataColumn9
            // 
            this.dataColumn9.ColumnName = "Platform";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 1500;
            this.refreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // sessionDataSet
            // 
            this.sessionDataSet.DataSetName = "Sessions";
            this.sessionDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.sessionTable});
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
            this.splitContainer1.Panel1.Controls.Add(this.sessionGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.renderPanel);
            this.splitContainer1.Size = new System.Drawing.Size(724, 479);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 1;
            // 
            // sessionGrid
            // 
            this.sessionGrid.AllowNavigation = false;
            this.sessionGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.sessionGrid.CaptionBackColor = System.Drawing.SystemColors.ControlDark;
            this.sessionGrid.CaptionCountText = "(%s Items)";
            this.sessionGrid.CaptionCountVisible = true;
            this.sessionGrid.CaptionForeColor = System.Drawing.SystemColors.ControlText;
            this.sessionGrid.CaptionText = "Sessions";
            this.sessionGrid.ContextMenuStrip = this.contextMenuStrip;
            this.sessionGrid.DataMember = "";
            this.sessionGrid.DataSource = this.sessionDataView;
            this.sessionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionGrid.FullRowKeyField = "ID";
            this.sessionGrid.FullRowSelect = true;
            this.sessionGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.sessionGrid.LinkColor = System.Drawing.SystemColors.MenuHighlight;
            this.sessionGrid.Location = new System.Drawing.Point(0, 0);
            this.sessionGrid.Name = "sessionGrid";
            this.sessionGrid.ReadOnly = true;
            this.sessionGrid.RowHeadersVisible = false;
            this.sessionGrid.RowHeaderWidth = 30;
            this.sessionGrid.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.sessionGrid.SelectionForeColor = System.Drawing.SystemColors.MenuText;
            this.sessionGrid.Size = new System.Drawing.Size(724, 264);
            this.sessionGrid.TabIndex = 1;
            this.sessionGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.sessionGrid;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridTextBoxColumn1,
            this.dataGridTextBoxColumn7,
            this.dataGridTextBoxColumn6,
            this.dataGridTextBoxColumn2,
            this.dataGridTextBoxColumn5,
            this.dataGridTextBoxColumn3,
            this.dataGridTextBoxColumn4,
            this.dataGridTextBoxColumn8,
            this.dataGridTextBoxColumn9});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "Sessions";
            this.dataGridTableStyle1.ReadOnly = true;
            this.dataGridTableStyle1.RowHeadersVisible = false;
            this.dataGridTableStyle1.RowHeaderWidth = 30;
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "ID";
            this.dataGridTextBoxColumn1.MappingName = "Id";
            this.dataGridTextBoxColumn1.ReadOnly = true;
            this.dataGridTextBoxColumn1.Width = 50;
            // 
            // dataGridTextBoxColumn7
            // 
            this.dataGridTextBoxColumn7.Format = "";
            this.dataGridTextBoxColumn7.FormatInfo = null;
            this.dataGridTextBoxColumn7.HeaderText = "Terminal ID";
            this.dataGridTextBoxColumn7.MappingName = "TerminalId";
            this.dataGridTextBoxColumn7.Width = 75;
            // 
            // dataGridTextBoxColumn6
            // 
            this.dataGridTextBoxColumn6.Format = "";
            this.dataGridTextBoxColumn6.FormatInfo = null;
            this.dataGridTextBoxColumn6.HeaderText = "User ID";
            this.dataGridTextBoxColumn6.MappingName = "UserId";
            this.dataGridTextBoxColumn6.Width = 75;
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "Application";
            this.dataGridTextBoxColumn2.MappingName = "Application";
            this.dataGridTextBoxColumn2.ReadOnly = true;
            this.dataGridTextBoxColumn2.Width = 75;
            // 
            // dataGridTextBoxColumn5
            // 
            this.dataGridTextBoxColumn5.Format = "";
            this.dataGridTextBoxColumn5.FormatInfo = null;
            this.dataGridTextBoxColumn5.HeaderText = "IP Address";
            this.dataGridTextBoxColumn5.MappingName = "IP";
            this.dataGridTextBoxColumn5.Width = 110;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "Process ID";
            this.dataGridTextBoxColumn3.MappingName = "PID";
            this.dataGridTextBoxColumn3.ReadOnly = true;
            this.dataGridTextBoxColumn3.Width = 75;
            // 
            // dataGridTextBoxColumn4
            // 
            this.dataGridTextBoxColumn4.Format = "";
            this.dataGridTextBoxColumn4.FormatInfo = null;
            this.dataGridTextBoxColumn4.HeaderText = "Last Activity";
            this.dataGridTextBoxColumn4.MappingName = "LastActivity";
            this.dataGridTextBoxColumn4.ReadOnly = true;
            this.dataGridTextBoxColumn4.Width = 125;
            // 
            // dataGridTextBoxColumn8
            // 
            this.dataGridTextBoxColumn8.Format = "";
            this.dataGridTextBoxColumn8.FormatInfo = null;
            this.dataGridTextBoxColumn8.HeaderText = "Version";
            this.dataGridTextBoxColumn8.MappingName = "Version";
            this.dataGridTextBoxColumn8.Width = 75;
            // 
            // dataGridTextBoxColumn9
            // 
            this.dataGridTextBoxColumn9.Format = "";
            this.dataGridTextBoxColumn9.FormatInfo = null;
            this.dataGridTextBoxColumn9.HeaderText = "Platform";
            this.dataGridTextBoxColumn9.MappingName = "Platform";
            this.dataGridTextBoxColumn9.Width = 75;
            // 
            // renderPanel
            // 
            this.renderPanel.AutoScaleFactor = new System.Drawing.SizeF(1F, 1F);
            this.renderPanel.AutoScroll = true;
            this.renderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.renderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderPanel.Location = new System.Drawing.Point(0, 0);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.NativeDriver = null;
            this.renderPanel.Size = new System.Drawing.Size(724, 209);
            this.renderPanel.TabIndex = 0;
            // 
            // ucRuntimeView
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucRuntimeView";
            this.Size = new System.Drawing.Size(724, 479);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sessionDataView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionDataSet)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sessionGrid)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private ManagedConnection currentConnection;

        private bool OldRefreshTimerEnabled;

        public void HoldRefresh()
        {
            OldRefreshTimerEnabled = refreshTimer.Enabled;
            refreshTimer.Enabled = false;
        }

        public void ResumeRefresh()
        {
            refreshTimer.Enabled = OldRefreshTimerEnabled;
        }

        public void RefreshView(bool initialize)
        {
            try
            {
                if (currentConnection == null)
                    return;

                if (! currentConnection.Connected)
                    return;

                refreshTimer.Enabled = false;
                                
                sessionGrid.SuspendLayout();
                
                sessionTable.Clear();        
                sessionTable.Merge(currentConnection.GetSessionList());
                
                ServerConfigRefreshRate r = UIConfigFileHandler.Instance().Config.RefreshRate;

                switch (r)
                {
                    case ServerConfigRefreshRate.High:
                        refreshTimer.Interval = 1000;
                        refreshTimer.Enabled = true;
                        break;
                    case ServerConfigRefreshRate.Low:
                        refreshTimer.Interval = 30000;
                        refreshTimer.Enabled = true;
                        break;
                    case ServerConfigRefreshRate.Normal:
                        refreshTimer.Interval = 10000;
                        refreshTimer.Enabled = true;
                        break;
                    case ServerConfigRefreshRate.Paused:
                        refreshTimer.Enabled = false;
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
                sessionGrid.ResumeLayout(true);
            }
        }

        public void ShowView(ManagedConnection c)
        {
            this.BringToFront();
            currentConnection = c;
            RefreshView(true);
            sessionGrid.SelectRow(0);
        }

        private string Field(string name)
        {
            CurrencyManager myCurrencyManager = (CurrencyManager)this.BindingContext[sessionDataView];
            
            if (myCurrencyManager.Position != -1)
            {
                return ((myCurrencyManager.Current as DataRowView).Row[name].ToString());
            }
            else
            {
                return null;
            }
        }
               
        public void KillSession()
        {
            killSessionMenuItem_Click(null, null);
        }
        
        private void killSessionMenuItem_Click(object sender, System.EventArgs e)
        {
            string sessionId = Field("ID");

            if (!string.IsNullOrEmpty(sessionId))
            {
                if (MessageBox.Show(this, string.Format("Do you really want to kill session {0}?", sessionId), "Kill Session", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string cc = currentConnection.Instance.KillSession(sessionId);

                        if (cc != "ok")
                        {
                            MessageBox.Show(string.Format("Failed to kill session {0}. {1}", sessionId, cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            RefreshView(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format("Error when killing session {0}\n{1}", sessionId, ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                
        private void sessionGrid_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RefreshView(false);
            }
        }
                
        public void KillAllMenuItem_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show(this, string.Format("Do you really want to kill all sessions in the {0} instance?", currentConnection.Config.DisplayName), "Kill All", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string cc = currentConnection.Instance.KillAllSessions();

                    if (cc != "ok")
                    {
                        MessageBox.Show(string.Format("Failed to kill all sessions. {0}", cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        RefreshView(false);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Error when killing sessions.\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RefreshTimer_Tick(object sender, System.EventArgs e)
        {
            if (currentConnection.Connected)
                RefreshView(false);
            else
                refreshTimer.Enabled = false;
        }

        private void clientLogMenuItem_Click(object sender, System.EventArgs e)
        {
            string level;

            if (sender == clientLogOffMenuItem)
                level = "Off";
            else if (sender == clientLogVerboseMenuItem)
                level = "Verbose";
            else
                level = "Off";

            
            string sessionId = Field("ID");

            if (!string.IsNullOrEmpty(sessionId))
            {
                try
                {
                    string cc = currentConnection.Instance.SetTraceLevel(sessionId, level);

                    if (cc != "ok")
                    {
                        MessageBox.Show(string.Format("Failed to change the log level for session {0}. {1}", sessionId, cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        RefreshView(false);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Error when changing log level for session {0}.\n{1}", sessionId, ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void serverLogMenuItem_Click(object sender, System.EventArgs e)
        {
            string level;

            if (sender == serverLogOffMenuItem)
                level = "Off";
            else if (sender == serverLogErrorMenuItem)
                level = "Error";
            else if (sender == serverLogWarningMenuItem)
                level = "Warning";
            else if (sender == serverLogInfoMenuItem)
                level = "Information";
            else if (sender == serverLogVerboseMenuItem)
                level = "Verbose";
            else
                level = "Off";
            
            try
            {
                string cc = currentConnection.Instance.SetTraceLevel(null, level);

                if (cc != "ok")
                {
                    MessageBox.Show(string.Format("Failed to change server log level. {0}", cc), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    RefreshView(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Error when changing server log level.\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clientLogLevelMenuItem_Popup(object sender, System.EventArgs e)
        {
            try
            {
                string sessionId = Field("ID");

                if (!string.IsNullOrEmpty(sessionId))
                {
                    foreach (ToolStripMenuItem t in clientLogLevelMenuItem.DropDown.Items)
                        t.Checked = false;

                    string cc = currentConnection.Instance.GetTraceLevel(sessionId);

                    if (!string.IsNullOrEmpty(cc))
                    {
                        switch (cc.ToLower())
                        {
                            case "verbose":
                                clientLogVerboseMenuItem.Checked = true;
                                break;
                            default:
                                clientLogOffMenuItem.Checked = true;
                                break;
                        }

                        RefreshView(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Error when getting session log level.\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void serverLogLevelMenuItem_Popup(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem t in serverLogLevelMenuItem.DropDown.Items)
                t.Checked = false;

            try
            {
                string cc = currentConnection.Instance.GetTraceLevel(null);

                if (!string.IsNullOrEmpty(cc))
                {
                    switch (cc.ToLower())
                    {
                        case "off":
                            serverLogOffMenuItem.Checked = true;
                            break;
                        case "error":
                            serverLogErrorMenuItem.Checked = true;
                            break;
                        case "warning":
                            serverLogWarningMenuItem.Checked = true;
                            break;
                        case "information":
                            serverLogInfoMenuItem.Checked = true;
                            break;
                        case "verbose":
                            serverLogVerboseMenuItem.Checked = true;
                            break;
                        default:
                            serverLogOffMenuItem.Checked = true;
                            break;
                    }

                    RefreshView(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("Error when getting server log level.\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logLevelMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            clientLogLevelMenuItem.Visible = !string.IsNullOrEmpty(Field("ID"));
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            killMenuItem.Enabled = !string.IsNullOrEmpty(Field("ID"));
        }
    }
}
