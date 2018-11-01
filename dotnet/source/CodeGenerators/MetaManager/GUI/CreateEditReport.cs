using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using System.Reflection;
using Cdc.Framework.Parsing.OracleSQLParsing;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplates;
using Spring.Data.NHibernate.Support;
using Cdc.Framework.Reporting.PLSQL;
using Imi.Framework.Printing.LabelParsing;
using Cdc.CodeGeneration.Caching;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using Imi.Framework.Printing;
using Cdc.MetaManager.BusinessLogic.Helpers;
using NHibernate;

namespace Cdc.MetaManager.GUI
{
    public partial class CreateEditReport : MdiChildForm
    {
        private IApplicationService appService = null;
        private IModelService modelService = null;
        private TabPage tabTempSimulation = null;

        private Report report { get; set; }

        bool? moveFocusForward = null;

        private IList<StoredProcedureParameter> lastGetXmlParameters = null;

        private bool TestButtonClicked { get; set; }
        private bool IsModified { get; set; }
        private bool IsInterfaceChanged { get; set; }
        private bool UpdateCode { get; set; }
        private bool UpdateTest { get; set; }
        private bool IsWarehouseApplication { get; set; }
        private string PrintBodyProcText { get; set; }
        private string PrintSpecProcText { get; set; }
        private string StubCallPrintText { get; set; }
        private string StubBeforePrintBodyText { get; set; }
        private string StubBeforePrintSpecText { get; set; }
        private string StubAfterPrintBodyText { get; set; }
        private string StubAfterPrintSpecText { get; set; }
        private bool IsLoading { get; set; }
        private bool IsLoadingTestParameters { get; set; }
        private XmlDocument TestDocument { get; set; }
        private ParseLabel TestLabel { get; set; }
        private bool LabelChanged { get; set; }
        private bool LabelNeedsSave { get; set; }
        private string CurrentLabelFileName { get; set; }

        public CreateEditReport()
        {
            InitializeComponent();

            appService = MetaManagerServices.GetApplicationService();
            modelService = MetaManagerServices.GetModelService();
            TestLabel = new ParseLabel();
        }

        public bool IsInterfaceSelected 
        {
            get
            {
                return tvQueries.SelectedNode != null &&
                       tvQueries.SelectedNode.Parent == null;
            }
        }

        public ReportQuery SelectedReportQuery
        {
            get
            {
                if (!IsInterfaceSelected &&
                    tvQueries.SelectedNode != null &&
                    tvQueries.SelectedNode.Tag is ReportQuery)
                {
                    return (ReportQuery)tvQueries.SelectedNode.Tag;
                }

                return null;
            }
        }

        
        private WarehouseReportType SelectedReportType
        {
            get
            {
                if (cbWarehouseReportType.SelectedItem != null &&
                    ((KeyValuePair<string, int?>)cbWarehouseReportType.SelectedItem).Value.HasValue)
                    return (WarehouseReportType)((KeyValuePair<string, int?>)cbWarehouseReportType.SelectedItem).Value;
                else
                    return WarehouseReportType.NotApplicable;
            }
            set
            {
                if (cbWarehouseReportType.Items.Count > 0)
                {
                    if (value == WarehouseReportType.NotApplicable)
                    {
                        cbWarehouseReportType.SelectedItem = cbWarehouseReportType.Items[0];
                    }
                    else
                    {
                        foreach (KeyValuePair<string, int?> item in cbWarehouseReportType.Items)
                        {
                            if (item.Value.HasValue &&
                                item.Value.Value == (int)value)
                                cbWarehouseReportType.SelectedItem = item;
                        }
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckSaveLabel())
                return;

            if (SaveReport())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!CheckSaveLabel())
                return;

            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void tsbQueryNew_Click(object sender, EventArgs e)
        {
            if (IsModified)
            {
                if (MessageBox.Show("You have to save your report first. Do you want to continue and save the report?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!SaveReport())
                    {
                        return;
                    }

                    LoadReport();
                }
                else
                {
                    return;
                }
            }

            using (CreateEditReportSQL form = new CreateEditReportSQL())
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    form.CurrentQuery = new Query();
                    form.CurrentQuery.Schema = modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Schema>(BackendApplication.Id)[0];
                    form.CurrentQuery.QueryType = QueryType.QueryForReport;
                    form.Report = report;
                    form.MaxQueryNameLength = 22;
                    form.IsEditable = IsEditable;
                    form.BackendApplication = BackendApplication;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

                if (form.ShowDialog() == DialogResult.OK)
                {
                    ReportQuery reportQuery = new ReportQuery();
                    
                    reportQuery.Query = form.CurrentQuery;
                    reportQuery.Sequence = ReportHelper.GetAllReportQueries(report).Count > 0 ? ReportHelper.GetAllReportQueries(report).Max(r => r.Sequence) + 1 : 1;
                    reportQuery.Parent = form.ParentReportQuery;
 
                    if (reportQuery.Parent == null)
                    {
                        reportQuery.Report = report;
                    }
                    else
                    {
                        reportQuery.Report = null;
                    }

                    try
                    {
                        MetaManagerServices.Helpers.ReportQueryHelper.SaveAndSynchronize(reportQuery);
                    }
                    finally
                    {
                        LoadReport();
                    }
                }
            }
        }
        
        private void tsbInterfaceEdit_Click(object sender, EventArgs e)
        {
            // Edit the interface for the report
            using (CreatePropertyMap form = new CreatePropertyMap())
            {
                form.BackendApplication = BackendApplication;
                form.FrontendApplication = FrontendApplication;
                form.IsRequestMap = true;
                form.DoSaveMapWhenExit = false;
                form.MaxPropertyLength = 30;
                form.UseColumnNames = true;
                form.NameUpperCase = true;
                form.NameSuffix = "_I";
                form.IsEditable = this.IsEditable;

                if (report.Id == Guid.Empty && report.RequestMap == null)
                {
                    report.RequestMap = new PropertyMap();
                }
          
                form.PropertyMap = report.RequestMap;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.ChangesMade)
                    {
                        IsModified = true;
                        IsInterfaceChanged = true;
                        
                        PopulateInterface();
                        EnableDisableButtons();
                        EnableDisableTabs();
                    }
                }
            }
        }

        private void PopulateInterface()
        {
            lvInterface.BeginUpdate();

            lvInterface.Items.Clear();

            int seq = 1;

            if (report.RequestMap != null &&
                report.RequestMap.MappedProperties.Count > 0)
            {
                // First loop through the user defined interface
                foreach (ReportInterfaceParameter parameter in report.GetUserDefinedInterfaceParameterList())
                {
                    ListViewItem item = new ListViewItem(seq++.ToString());
                    item.SubItems.Add(parameter.Name);
                    item.SubItems.Add(parameter.ParameterType);
                    
                    lvInterface.Items.Add(item);
                }
            }

            // Now add the static parameters
            foreach (ReportInterfaceParameter parameter in report.GetStaticInterfaceParameterList(SelectedReportType, cbABCEnabled.Checked))
            {
                ListViewItem item = new ListViewItem(seq++.ToString());
                item.SubItems.Add(parameter.Name);
                item.SubItems.Add(parameter.FullParameterType);
                lvInterface.Items.Add(item);
            }

            lvInterface.EndUpdate();
        }

        private void CreateEditReport_Load(object sender, EventArgs e)
        {
            IsLoading = true;

            // Check if Warehouse backend
            IsWarehouseApplication = BackendApplication.Name == "Warehouse";

            if (IsWarehouseApplication)
            {
                PopulateReportTypeComboBox();
                cbWarehouseReportType.Enabled = true;
            }
            else
                cbWarehouseReportType.Enabled = false;

            // Check if creating report or editing
            if (ContaindDomainObjectIdAndType.Key == Guid.Empty)
            {
                report = new Report();
                report.Application = modelService.GetInitializedDomainObject<Cdc.MetaManager.DataAccess.Domain.Application>(BackendApplication.Id);
                report.IsLocked = true;
                report.LockedBy = Environment.UserName;
                report.DataDuplicates = 1;
                report.CreatorName = Environment.UserName;
                
                LoadReport();
            }
            else
            {
                LoadReport();
            }

            tabTempSimulation = tabReport.TabPages["tpSimulation"];
                        
            IsLoading = false;
            TestButtonClicked = false;
            TestDocument = null;
            LabelChanged = false;
            CurrentLabelFileName = string.Empty;
        }

        private void LoadReport()
        {
            if (report == null || report.Id != Guid.Empty)
            {
                report = MetaManagerServices.Helpers.ReportHelper.GetInitializedReport(ContaindDomainObjectIdAndType.Key);
            }

            IsEditable = report.IsLocked && report.LockedBy == Environment.UserName;

            tbName.Text = report.Name;
            tbDescription.Text = report.Description;
            tbCreator.Text = report.CreatorName;
            tbDocumentTypeDefinition.Text = report.DocumentTypeDefinition;
            tbDocumentType.Text = report.DocumentType;
            cbABCEnabled.Checked = report.IsABCEnabled;
            nudDataDuplicates.Value = report.DataDuplicates;

            if (IsWarehouseApplication)
                SelectedReportType = report.WarehouseReportType;

            IsModified = false;
            UpdateCode = true;
            UpdateTest = true;

            PopulateInterface();
            PopulateQueries();
            EnableDisableTabs();
            EnableDisableButtons();
            SetTitleBar();
        }

        private void SetTitleBar()
        {
            this.Text = string.Format("Report - ({0}) {1} [{2}]", report.Id.ToString(), report.Name, report.DocumentType);
        }

        private void EnableDisableTabs()
        {
            if (CheckReportQueries(report))
            {
                if (tabReport.TabPages["tpTestData"] == null)
                {
                    tabReport.TabPages.Add(tpTestData);
                }

                if (tabCodeView.TabPages["tpXsd"] == null)
                {
                    tabCodeView.TabPages.Add(tpXsd);
                }

                if (tabCodeView.TabPages["tpGetXmlPLSQLBody"] == null)
                {
                    tabCodeView.TabPages.Add(tpGetXmlPLSQLBody);
                }

                if (tabCodeView.TabPages["tpGetXmlPLSQLSpec"] == null)
                {
                    tabCodeView.TabPages.Add(tpGetXmlPLSQLSpec);
                }
               
            }
            else
            {
                if (tabReport.TabPages["tpTestData"] != null)
                {
                    tabReport.TabPages.Remove(tabReport.TabPages["tpTestData"]);
                }

                if (tabCodeView.TabPages["tpXsd"] != null)
                {
                    tabCodeView.TabPages.Remove(tabCodeView.TabPages["tpXsd"]);
                }

                if (tabCodeView.TabPages["tpGetXmlPLSQLBody"] != null)
                {
                    tabCodeView.TabPages.Remove(tabCodeView.TabPages["tpGetXmlPLSQLBody"]);
                }

                if (tabCodeView.TabPages["tpGetXmlPLSQLSpec"] != null)
                {
                    tabCodeView.TabPages.Remove(tabCodeView.TabPages["tpGetXmlPLSQLSpec"]);
                }
            }
        }

        private void PopulateQueries()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                EnableDisableTabs();

                // Clear all nodes
                tvQueries.Nodes.Clear();

                // Create the topnode that is the Interface
                TreeNode topNode = tvQueries.Nodes.Add("[Interface]");

                topNode.ImageIndex = 2;

                if (report.ReportQueries.Count > 0)
                {
                    // Fetch all sources from the Report
                    ReportSQLSources existingSources = new ReportSQLSources(appService, report);

                    foreach (ReportQuery reportQuery in report.ReportQueries.OrderBy(r => r.Sequence))
                    {
                        AddQueryToTree(topNode, reportQuery, existingSources);
                    }
                }

                // Select the topnode of the queries
                tvQueries.SelectedNode = tvQueries.Nodes[0];

                tvQueries.ExpandAll();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void AddQueryToTree(TreeNode node, ReportQuery reportQuery, ReportSQLSources existingSources)
        {
            TreeNode newNode = node.Nodes.Add(reportQuery.Query.Name);

            newNode.ToolTipText = string.Format("ReportQuery ({0}) - Query ({1})",
                                                reportQuery.Id,
                                                reportQuery.Query.Id);

            newNode.ImageIndex = CheckReportQuery(reportQuery, existingSources) ? 1 : 0;

            newNode.Tag = reportQuery;

            if (reportQuery.Children.Count > 0)
            {
                foreach (ReportQuery childReportQuery in reportQuery.Children.OrderBy(r => r.Sequence))
                {
                    AddQueryToTree(newNode, childReportQuery, existingSources);
                }
            }
        }

        private bool CheckReportQueries(Report report)
        {
            if (report != null && report.Id != Guid.Empty)
            {
                if (report.ReportQueries.Count > 0)
                {
                    bool check = true;

                    ReportSQLSources existingSources = new ReportSQLSources(appService, report);

                    foreach (ReportQuery reportQuery in ReportHelper.GetAllReportQueries(report))
                    {
                        if (!CheckReportQuery(reportQuery, existingSources))
                        {
                            check = false;
                            break;
                        }
                    }

                    if (check)
                        return true;
                }
            }

            return false;
        }

        private bool CheckReportQuery(ReportQuery reportQuery, ReportSQLSources existingSources)
        {
            string errorText;
            ReportQuery parentReportQuery;

            return ReportHelper.CheckReportParameterCombinations(existingSources, reportQuery.Query.SqlStatement, out errorText, out parentReportQuery);
        }
                
        private void PopulateReportTypeComboBox()
        {
            Type enumType = typeof(WarehouseReportType);

            cbWarehouseReportType.Items.Clear();

            // Add empty row
            cbWarehouseReportType.Items.Add(new KeyValuePair<string, int?>("", null));

            if (enumType.IsEnum && enumType.IsPublic)
            {
                foreach (FieldInfo fInfo in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if ((int)fInfo.GetRawConstantValue() != 0)
                        cbWarehouseReportType.Items.Add(new KeyValuePair<string, int?>(fInfo.Name, (int)fInfo.GetRawConstantValue()));
                }
            }
        }

        private void EnableDisableButtons()
        {
            if (IsEditable)
            {
                tsQuery.Enabled = false;
                tsbQueryDelete.Enabled = false;
                tsbQueryEdit.Enabled = false;
                tsbMoveUp.Enabled = false;
                tsbMoveDown.Enabled = false;

                if (tvQueries.SelectedNode != null &&
                    tvQueries.SelectedNode.PrevNode != null &&
                    tvQueries.SelectedNode.Tag is ReportQuery)
                {
                    tsbMoveUp.Enabled = true;
                }

                if (tvQueries.SelectedNode != null &&
                    tvQueries.SelectedNode.NextNode != null &&
                    tvQueries.SelectedNode.Tag is ReportQuery)
                {
                    tsbMoveDown.Enabled = true;
                }

                // Check if there is an interface defined
                if (report.RequestMap != null &&
                    report.RequestMap.MappedProperties.Count > 0)
                {
                    tsQuery.Enabled = true;

                    if (SelectedReportQuery != null)
                    {
                        tsbQueryEdit.Enabled = true;

                        if (tvQueries.SelectedNode.Nodes.Count == 0)
                        {
                            tsbQueryDelete.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                btnOK.Enabled = false;
                tbDocumentType.ReadOnly = true;
                tbDocumentTypeDefinition.ReadOnly = true;
                cbWarehouseReportType.Enabled = false;
                cbABCEnabled.Enabled = false;
                tbName.ReadOnly = true;
                tbDescription.ReadOnly = true;
                tbCreator.ReadOnly = true;
                nudDataDuplicates.Enabled = false;
                tsbMoveDown.Enabled = false;
                tsbMoveUp.Enabled = false;
                               
                tsbInterfaceEdit.Text = "View";
                tsbQueryEdit.Text = "View";
                tsbQueryEdit.Enabled = false;

                // Check if there is an interface defined
                if (report.RequestMap != null &&
                    report.RequestMap.MappedProperties.Count > 0 &&
                    report.Id != Guid.Empty)
                {
                    tsQuery.Enabled = true;

                    if (SelectedReportQuery != null)
                    {
                        tsbQueryEdit.Enabled = true;
                    }
                }

                tsbQueryDelete.Enabled = false;
                tsbQueryNew.Enabled = false;
            }
        }

        private void tvQueries_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EnableDisableButtons();
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }

        private void tsbQueryEdit_Click(object sender, EventArgs e)
        {
            if (IsModified)
            {
                if (MessageBox.Show("You have to save your report first. Do you want to continue and save the report?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!SaveReport())
                    {
                        return;
                    }

                    LoadReport();
                }
                else
                {
                    return;
                }
            }

            DoEditQuery();
        }

        private void DoEditQuery()
        {
            if (SelectedReportQuery != null)
            {
                using (CreateEditReportSQL form = new CreateEditReportSQL())
                {
                    ReportQuery reportQuery = SelectedReportQuery;

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        form.BackendApplication = BackendApplication;
                        form.CurrentQuery = modelService.GetInitializedDomainObject<Query>(reportQuery.Query.Id);
                        form.Report = report;
                        form.MaxQueryNameLength = 22;
                        form.IsEditable = this.IsEditable;
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            reportQuery.Query = form.CurrentQuery;
        
                            if (reportQuery.Parent != form.ParentReportQuery)
                            {
                                reportQuery.Parent = form.ParentReportQuery;
                            }

                            if (reportQuery.Parent == null)
                            {
                                reportQuery.Report = report;
                            }
                            else
                            {
                                reportQuery.Report = null;
                            }
                                                        
                            try
                            {
                                MetaManagerServices.Helpers.ReportQueryHelper.SaveAndSynchronize(reportQuery);
                            }
                            finally
                            {
                                LoadReport();
                            }
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }
        }

        private bool SaveReport()
        {
            if (!NamingGuidance.CheckNameFocus(tbName, "Report Name", true))
                return false;
            
            var reports = modelService.GetAllDomainObjectsByApplicationId<Report>(BackendApplication.Id).Where(r => (r.Id != report.Id));

            var reportNames = (from r in reports
                              select r.Name).ToList();
                                    
            // Also add "Queue" to the list, since this is a reserved word. There is a package
            // named "Report_Queue" which means we cannot name a report to Queue.
            reportNames.Add("Queue");

            // Check against the list
            if (!NamingGuidance.CheckNameNotInList(tbName.Text, "Report Name", "list of used or reserved Report Names", reportNames, false, true))
                return false;

            //if (!NamingGuidance.CheckNameFocus(tbDocumentType, "Document Type", true))
            //    return false;

            // Check against the list
            // We needs to accept duplicate reports in some cases. 
            // Fore PLCP, KPL, PLPP 
            try
            {
                if (!NamingGuidance.CheckNameNotInList(tbDocumentType.Text, "Document Type", "list of used or reserved Document Types", reports.Select(r => r.DocumentType), false, false))
                {
                    return false;
                }
            }
            catch (NamingGuidanceException e)
            {
                if (DialogResult.No == MessageBox.Show("There are more than one Report for this document type. Do you really want to save these duplicates?", "Warning", MessageBoxButtons.YesNo , MessageBoxIcon.Warning))
                {
                    return false;
                }

            }

            if (!NamingGuidance.CheckNameFocus(tbDocumentTypeDefinition, "Document Type Definition", true))
                return false;

            // Check against the list
            // We needs to accept duplicate reports in some cases. 
            // Fore DMDOCTYPE_Pack_Lbl_Case_Pick, DMDOCTYPE_Kit_Pack_Label, DMDOCTYPE_Pack_Lbl_Pallet_Pck
            try
            {
                if (!NamingGuidance.CheckNameNotInList(tbDocumentTypeDefinition.Text, "Document Type Definition", "list of used or reserved Document Type Definitions", reports.Select(r => r.DocumentTypeDefinition), false, false))
                    return false;
            }
            catch (NamingGuidanceException e)
            {
                if (DialogResult.No == MessageBox.Show("There are more than one Report for this Document Type Definition. Do you really want to save these duplicates?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    return false;
                }
            }

            if (report.RequestMap == null ||
                report.RequestMap.MappedProperties.Count == 0)
            {
                MessageBox.Show("The report must have an interface.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if we should check report type
            if (IsWarehouseApplication && 
                SelectedReportType == WarehouseReportType.NotApplicable)
            {
                MessageBox.Show("The report must have a type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            Dictionary<string, object> props = new Dictionary<string,object>();
            props.Add("Name", "CustomProperties");
                        
            BusinessEntity customProps = modelService.GetAllDomainObjectsByPropertyValues<BusinessEntity>(props).LastOrDefault();
                        
            List<string> requestNames = new List<string>();

            foreach (MappedProperty prop in report.RequestMap.MappedProperties)
            {
                string name = prop.Name;

                if (string.IsNullOrEmpty(name))
                    name = prop.Target.Name;

                if (prop.TargetProperty != null)
                    if (prop.TargetProperty.BusinessEntity == null)
                        prop.TargetProperty.BusinessEntity = customProps;

                // Check the names
                if (!NamingGuidance.CheckMappedPropertyName(name, true))
                    return false;

                if (requestNames.Contains(name))
                {
                    MessageBox.Show(string.Format("Duplicate property name \"{0}\".", name), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                    requestNames.Add(name);
            }

            // Check if name of report is changed on an existing report which have
            // quite some impact
            if (report.Id != Guid.Empty) 
            {
                // Check if name is changed
                if (report.Name != tbName.Text.Trim())
                {
                    if (MessageBox.Show("You are about to change the name of the report, this will\n" +
                                        "lead to the following consequences:\n\n" +
                                        "\t* The root tag of the XSD will change.\n" +
                                        "\t\t- Update the report to point to the new root node.\n" +
                                        "\t\t- The old generated XSD file needs to be deleted.\n" +
                                        "\n\t* The package name for fetching the report data will change.\n" +
                                        "\t\t- The call to the package needs to be updated from the PrintDocument package.\n" +
                                        "\t\t- The old package files (body and spec) needs to be deleted.\n" +
                                        "\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return false;
                    }

                    ReportPLSQL plsql = new ReportPLSQL(modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Schema>(BackendApplication.Id)[0].ConnectionString);

                    if (plsql.PackageExists(report.PackageName))
                    {
                        DialogResult msgResult = MessageBox.Show(string.Format("Do you automatically want to remove the old package ({0}) from the database?", report.PackageName),
                                                                 "Question",
                                                                 MessageBoxButtons.YesNoCancel,
                                                                 MessageBoxIcon.Question,
                                                                 MessageBoxDefaultButton.Button1);

                        if (msgResult == DialogResult.Cancel)
                            return false;
                        else if (msgResult == DialogResult.Yes)
                        {
                            // Fetch parameterlist
                            lastGetXmlParameters = plsql.GetXmlInParameterList(report.PackageName);

                            // Populate list
                            foreach (StoredProcedureParameter param in lastGetXmlParameters)
                            {
                                // Delete the config value for the parameter
                                Config.Backend.DeleteReportTestParameter(report.Name, param.Name);
                            }

                            Config.Save();

                            // Drop the package with body from database
                            plsql.Execute(string.Format("drop package {0}", report.PackageName));
                        }
                    }
                }

                if (IsInterfaceChanged)
                {
                    if (MessageBox.Show("You are about to change the interface of the report, this will\n" +
                                        "lead to the following consequences:\n\n" +
                                        "\t* All procedures and functions will have changed parameters.\n" +
                                        "\t\t- The Print procedure needs to be replaced.\n" +
                                        "\t\t- The Report package needs to be replaced.\n" +
                                        "\t\t- All places where the Print procedure is called from needs to change their parameters accordingly.\n" +
                                        "\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            report.CreatorName = tbCreator.Text.Trim();
            report.Description = tbDescription.Text.Trim();
            report.Name = tbName.Text.Trim();
            report.WarehouseReportType = SelectedReportType;
            report.DocumentType = tbDocumentType.Text.Trim();
            report.DocumentTypeDefinition = tbDocumentTypeDefinition.Text.Trim();
            report.IsABCEnabled = cbABCEnabled.Checked;

            try
            {
                report.DataDuplicates = Convert.ToInt32(nudDataDuplicates.Value);
            }
            catch
            {
                report.DataDuplicates = 1;
            }
                       
            // Save the report
            report = modelService.SaveDomainObject(report) as Report;
            
            ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(report.Id, typeof(Report));
                        
            // Set the title bar after save
            SetTitleBar();

            // Saved so now no changes is made
            IsModified = false;
            IsInterfaceChanged = false;

            return true;
        }

        private void tabReport_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // Check if user is selecting the Queries page
            if (e.TabPage.Name != "tpReportDefinition" && IsModified)
            {
                if (MessageBox.Show("You have to save the Report before continuing.\n" +
                                    "Do you want to save the report?",
                                    "Question",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!SaveReport())
                        e.Cancel = true;
                    else
                    {
                        UpdateCode = true;
                        UpdateTest = true;
                    }
                }
                else
                    e.Cancel = true;
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }

        private void tbCreator_TextChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }
                
        private void tsbQueryDelete_Click(object sender, EventArgs e)
        {
            if (IsModified)
            {
                if (!SaveReport())
                    return;
            }

            if (SelectedReportQuery != null)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        modelService.DeleteDomainObject(SelectedReportQuery);
                    }
                    finally
                    {
                        LoadReport();
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void tsbMoveUp_Click(object sender, EventArgs e)
        {
            if (tvQueries.SelectedNode != null &&
                tvQueries.SelectedNode.PrevNode != null &&
                tvQueries.SelectedNode.Tag is ReportQuery)
            {
                // Change sequence with the previous sibling above
                int tempSequence = ((ReportQuery)tvQueries.SelectedNode.PrevNode.Tag).Sequence;
                ((ReportQuery)tvQueries.SelectedNode.PrevNode.Tag).Sequence = ((ReportQuery)tvQueries.SelectedNode.Tag).Sequence;
                ((ReportQuery)tvQueries.SelectedNode.Tag).Sequence = tempSequence;

                // Save sequences
                modelService.SaveDomainObject((ReportQuery)tvQueries.SelectedNode.PrevNode.Tag);
                modelService.SaveDomainObject((ReportQuery)tvQueries.SelectedNode.Tag);
                
                // Get the parent to the nodes
                TreeNode parentNode = tvQueries.SelectedNode.Parent;

                // Get the selected row in the listview
                TreeNode tempNode = tvQueries.SelectedNode;

                // Get the index of the previous node
                int index = tvQueries.SelectedNode.PrevNode.Index;

                // Remove it from parent
                parentNode.Nodes.Remove(tvQueries.SelectedNode);

                // Insert at same parent with same index as the previous node
                parentNode.Nodes.Insert(index, tempNode);

                // Set the selectednode back again
                tvQueries.SelectedNode = tempNode;

                EnableDisableButtons();

                UpdateCode = true;
                UpdateTest = true;
            }
        }

        private void tpCodePreview_Enter(object sender, EventArgs e)
        {
            if (UpdateCode && report.Id != Guid.Empty)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                    {
                        Report tempReport = modelService.GetDomainObject<Report>(report.Id);
                        string source = string.Empty;

                        // XSD
                        XsdTemplate xsdTemplate = new XsdTemplate();
                        xsdTemplate.report = tempReport;
                        source = xsdTemplate.RenderToString();
                        tbXMLSchema.Text = source;

                        // GetXml PL/SQL Body
                        PackageBodyTemplate bodyTemplate = new PackageBodyTemplate();
                        bodyTemplate.report = tempReport;
                        bodyTemplate.doPreCompile = false;
                        source = bodyTemplate.RenderToString();
                        tbGetXmlPLSQLBody.Text = source;

                        // GetXml PL/SQL Spec
                        PackageSpecTemplate specTemplate = new PackageSpecTemplate();
                        specTemplate.report = tempReport;
                        source = specTemplate.RenderToString();
                        tbGetXmlPLSQLSpec.Text = source;

                        // Get the body procedure text only
                        CallReportPackageProcTemplate printPLSQLPackProcTemplate = new CallReportPackageProcTemplate();
                        printPLSQLPackProcTemplate.report = tempReport;
                        printPLSQLPackProcTemplate.packageName = "PrintDocument";
                        PrintBodyProcText = printPLSQLPackProcTemplate.RenderToString();
                        tbPrintPLSQLBody.Text = PrintBodyProcText;

                        // Get the spec procedure text only
                        CallReportPackageProcHeaderTemplate printPLSQLPackProcHeaderTemplate = new CallReportPackageProcHeaderTemplate();
                        printPLSQLPackProcHeaderTemplate.report = tempReport;
                        printPLSQLPackProcHeaderTemplate.isSpec = true;
                        PrintSpecProcText = printPLSQLPackProcHeaderTemplate.RenderToString();
                        tbPrintPLSQLSpec.Text = PrintSpecProcText;

                        // Call Print procedure Stub
                        StubCallPrintProcedureTemplate stubCallPrintProcedureTemplate = new StubCallPrintProcedureTemplate();
                        stubCallPrintProcedureTemplate.report = tempReport;
                        stubCallPrintProcedureTemplate.packageName = "PrintDocument";
                        StubCallPrintText = stubCallPrintProcedureTemplate.RenderToString();
                        tbStubCallPrintProc.Text = StubCallPrintText;

                        // Before Print procedure Stub Body
                        StubBeforePrintProcedureTemplate stubBeforePrintProcedureTemplate = new StubBeforePrintProcedureTemplate();
                        stubBeforePrintProcedureTemplate.report = tempReport;
                        stubBeforePrintProcedureTemplate.packageName = "PrintBeforeAfter";
                        StubBeforePrintBodyText = stubBeforePrintProcedureTemplate.RenderToString();
                        tbStubBeforePrintProc.Text = StubBeforePrintBodyText;

                        // Before Print procedure Stub Spec
                        StubBeforePrintProcedureHeaderTemplate stubBeforePrintProcedureHeaderTemplate = new StubBeforePrintProcedureHeaderTemplate();
                        stubBeforePrintProcedureHeaderTemplate.report = tempReport;
                        stubBeforePrintProcedureHeaderTemplate.isSpec = true;
                        StubBeforePrintSpecText = stubBeforePrintProcedureHeaderTemplate.RenderToString();

                        // After Print procedure Stub Body
                        StubAfterPrintProcedureTemplate stubAfterPrintProcedureTemplate = new StubAfterPrintProcedureTemplate();
                        stubAfterPrintProcedureTemplate.report = tempReport;
                        stubAfterPrintProcedureTemplate.packageName = "PrintBeforeAfter";
                        StubAfterPrintBodyText = stubAfterPrintProcedureTemplate.RenderToString();
                        tbStubAfterPrintProc.Text = StubAfterPrintBodyText;

                        // After Print procedure Stub Spec
                        StubAfterPrintProcedureHeaderTemplate stubAfterPrintProcedureHeaderTemplate = new StubAfterPrintProcedureHeaderTemplate();
                        stubAfterPrintProcedureHeaderTemplate.report = tempReport;
                        stubAfterPrintProcedureHeaderTemplate.isSpec = true;
                        StubAfterPrintSpecText = stubAfterPrintProcedureHeaderTemplate.RenderToString();
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

                UpdateCode = false;
            }
        }

        private void tsbMoveDown_Click(object sender, EventArgs e)
        {
            if (tvQueries.SelectedNode != null &&
                tvQueries.SelectedNode.NextNode != null &&
                tvQueries.SelectedNode.Tag is ReportQuery)
            {
                // Change sequence with the previous sibling above
                int tempSequence = ((ReportQuery)tvQueries.SelectedNode.NextNode.Tag).Sequence;
                ((ReportQuery)tvQueries.SelectedNode.NextNode.Tag).Sequence = ((ReportQuery)tvQueries.SelectedNode.Tag).Sequence;
                ((ReportQuery)tvQueries.SelectedNode.Tag).Sequence = tempSequence;

                // Save sequences
                modelService.SaveDomainObject((ReportQuery)tvQueries.SelectedNode.NextNode.Tag);
                modelService.SaveDomainObject((ReportQuery)tvQueries.SelectedNode.Tag);

                // Get the parent to the nodes
                TreeNode parentNode = tvQueries.SelectedNode.Parent;

                // Get the selected row in the listview
                TreeNode tempNode = tvQueries.SelectedNode;

                // Get the index of the previous node
                int index = tvQueries.SelectedNode.NextNode.Index;

                // Remove it from parent
                parentNode.Nodes.Remove(tvQueries.SelectedNode);

                // Insert at same parent with same index as the previous node
                parentNode.Nodes.Insert(index, tempNode);

                // Set the selectednode back again
                tvQueries.SelectedNode = tempNode;

                EnableDisableButtons();

                UpdateCode = true;
                UpdateTest = true;
            }
        }

        private void tvQueries_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void tvQueries_DoubleClick(object sender, EventArgs e)
        {
            if (tsbQueryEdit.Enabled)
                DoEditQuery();
        }

        private void SelectAll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (sender is TextBox)
                    (sender as TextBox).SelectAll();
            }
        }

        private void tpTestData_Enter(object sender, EventArgs e)
        {
            if (UpdateTest)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    // Load printer from config
                    tstbPrinterShare.Text = Config.Backend.GetReportRawPrinter(report.Name);

                    UpdateReportStatus();
                    EnableDisableTestButton();
                    TestButtonClicked = false;
                    EnableDisableTestXMLButtons();
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

                UpdateTest = false;
            }
        }

        private void UpdateReportStatus()
        {
            ReportPLSQL plsql = new ReportPLSQL(modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Schema>(BackendApplication.Id)[0].ConnectionString);

            string dbHash;

            try
            {
                dbHash = plsql.GetXmlHashSHA1(report.PackageName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                dbHash = string.Empty;
            }

            if (!string.IsNullOrEmpty(dbHash))
            {
                string getXmlString = string.Empty;

                using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                {
                    Report tempReport = modelService.GetDomainObject<Report>(report.Id);

                    // Check the fetched hash against what is generated with the 
                    PackageBodyGetXmlFunctionTemplate packageBodyGetXmlFunctionTemplate = new PackageBodyGetXmlFunctionTemplate();
                    packageBodyGetXmlFunctionTemplate.report = tempReport;
                    packageBodyGetXmlFunctionTemplate.doPreCompile = true;
                    getXmlString = packageBodyGetXmlFunctionTemplate.RenderToString();
                }

                // Calculate Hash for the generated function.
                string hashString = Hashing.Get(HashTypes.SHA1, getXmlString);

                if (dbHash == hashString)
                {
                    tslblProcedureStatus.Text = "Package is of current version!";
                    tslblProcedureStatus.ForeColor = Color.Green;

                    tsbDeploy.Enabled = false;

                    PopulateParameters(true);
                }
                else
                {
                    if (dbHash.Equals("TEST", StringComparison.CurrentCultureIgnoreCase))
                    {
                        tslblProcedureStatus.Text = "Package is set to Manual Test mode! Overwrite package when possible.";
                        tslblProcedureStatus.ForeColor = Color.OrangeRed;

                        tsbDeploy.Enabled = true;

                        PopulateParameters(true);
                    }
                    else
                    {
                        tslblProcedureStatus.Text = "Package has a different version!";
                        tslblProcedureStatus.ForeColor = Color.Red;

                        tsbDeploy.Enabled = true;

                        PopulateParameters(false);
                    }
                }
            }
            else
            {
                tslblProcedureStatus.Text = "Package is invalid or doesn't exist!";
                tslblProcedureStatus.ForeColor = Color.Red;

                tsbDeploy.Enabled = true;

                PopulateParameters(false);
            }
        }

        private void PopulateParameters(bool packageIsOK)
        {
            // Clear XML
            tbTextXML.Text = string.Empty;
            tsbResetTestParameters.Enabled = false;

            IsLoadingTestParameters = true;

            // Clear parameterlist
            dgvParameters.Rows.Clear();

            // If package is ok then try to populate the parameters
            if (packageIsOK)
            {
                ReportPLSQL plsql = new ReportPLSQL(modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Schema>(BackendApplication.Id)[0].ConnectionString);

                // Fetch parameterlist
                lastGetXmlParameters = plsql.GetXmlInParameterList(report.PackageName);

                // Populate list
                foreach (StoredProcedureParameter param in lastGetXmlParameters)
                {
                    int index = AddParameterRow(param);

                    // Set defaults
                    if (param.Name == "META_NO_COPIES_I")
                        dgvParameters["colValue", index].Value = "1";

                    if (param.Name == "META_TERID_I")
                        dgvParameters["colValue", index].Value = Environment.MachineName;

                    if (param.Name == "META_EMPID_I")
                        dgvParameters["colValue", index].Value = Environment.UserName;

                    // Add value from the config if it exists
                    string value = Config.Backend.GetReportTestParameter(report.Name, param.Name);

                    if (!string.IsNullOrEmpty(value))
                        dgvParameters["colValue", index].Value = value;

                    dgvParameters["colNull", index].Value = Config.Backend.GetReportTestParameterIsNull(report.Name, param.Name);
                }

                tsbResetTestParameters.Enabled = packageIsOK && dgvParameters.Rows.Count > 0;
            }

            IsLoadingTestParameters = false;
        }

        private void tsbDeploy_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ReportPLSQL plsql = new ReportPLSQL(modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Schema>(BackendApplication.Id)[0].ConnectionString);

                using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                {
                    Report tempReport = modelService.GetDomainObject<Report>(report.Id);

                    // PL/SQL Body
                    PackageBodyTemplate bodyTemplate = new PackageBodyTemplate();
                    bodyTemplate.report = tempReport;
                    bodyTemplate.doPreCompile = true;
                    string packageBody = bodyTemplate.RenderToString();

                    // PL/SQL Spec
                    PackageSpecTemplate specTemplate = new PackageSpecTemplate();
                    specTemplate.report = tempReport;
                    string packageSpec = specTemplate.RenderToString();

                    try
                    {
                        plsql.Execute(packageSpec);
                        plsql.Execute(packageBody);

                        plsql.Execute(string.Format("alter package {0} compile", tempReport.PackageName));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                UpdateReportStatus();
                EnableDisableTestButton();
                TestButtonClicked = false;
                EnableDisableTestXMLButtons();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private int AddParameterRow(StoredProcedureParameter parameter)
        {
            // Add the row
            int index = dgvParameters.Rows.Add();

            // Set the data on the new row
            dgvParameters["colName", index].Value = parameter.Name;
            dgvParameters["colName", index].Tag = parameter;
            dgvParameters["colDataType", index].Value = parameter.DataType.ToString();
            dgvParameters["colValue", index].Value = parameter.Value;
            dgvParameters["colNull", index].Value = parameter.IsNull;

            return index;
        }

        private delegate void DelegateSetCellSelection(int colIndex, int rowIndex);

        private void dgvParameters_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool IsNull = dgvParameters[2, e.RowIndex].EditedFormattedValue == null ? false : (bool)dgvParameters[2, e.RowIndex].EditedFormattedValue;

            if (!IsNull)
            {
                if (e.ColumnIndex < 2)
                    dgvParameters.BeginInvoke(new DelegateSetCellSelection(SetNextCellSelection), new object[] { 2, e.RowIndex });
                else
                    moveFocusForward = null;
            }
            else
            {
                if (e.ColumnIndex != 2)
                    dgvParameters.BeginInvoke(new DelegateSetCellSelection(SetNextCellSelection), new object[] { 2, e.RowIndex });
                else
                    moveFocusForward = null;
            }
        }

        private void SetNextCellSelection(int colIndex, int rowIndex)
        {
            bool IsNull = dgvParameters[2, rowIndex].EditedFormattedValue == null ? false : (bool)dgvParameters[2, rowIndex].EditedFormattedValue;

            if (moveFocusForward.HasValue)
            {
                if (moveFocusForward == false)
                {
                    if (rowIndex > 0)
                    {
                        bool PrevIsNull = dgvParameters[2, rowIndex-1].EditedFormattedValue == null ? false : (bool)dgvParameters[2, rowIndex-1].EditedFormattedValue;

                        if (!PrevIsNull)
                        {
                            if (dgvParameters.SelectedCells[0].ColumnIndex < 2)
                                dgvParameters[dgvParameters.Columns.Count - 1, rowIndex - 1].Selected = true;
                            else
                                dgvParameters[colIndex, rowIndex - 1].Selected = true;
                        }
                        else
                        {
                            // Since previous row is null then select the null field
                            if (dgvParameters.SelectedCells[0].ColumnIndex != 2)
                                dgvParameters[2, rowIndex - 1].Selected = true;
                            else
                                dgvParameters[colIndex, rowIndex - 1].Selected = true;
                        }
                    }
                    else
                    {
                        dgvParameters[2, 0].Selected = true;
                        DoFocus(dgvParameters, false);
                    }
                }

                moveFocusForward = null;
            }
            else if (dgvParameters.SelectedCells.Count == 0 ||
                     (!IsNull &&
                      dgvParameters.SelectedCells.Count > 0 &&
                      dgvParameters.SelectedCells[0].ColumnIndex < 2))
            {
                dgvParameters[colIndex, rowIndex].Selected = true;
            }
            else if (IsNull &&
                     dgvParameters.SelectedCells.Count > 0 &&
                     dgvParameters.SelectedCells[0].ColumnIndex != 2)
            {
                if (dgvParameters.SelectedCells[0].ColumnIndex == 3)
                {
                    if (rowIndex + 1 < dgvParameters.Rows.Count)
                    {
                        dgvParameters[colIndex, rowIndex + 1].Selected = true;
                    }
                    else
                    {
                        dgvParameters[colIndex, rowIndex].Selected = true;
                        DoFocus(dgvParameters, true);
                    }
                }
                else
                    dgvParameters[colIndex, rowIndex].Selected = true;
            }
        }

        private void DoFocus(Control control, bool forward)
        {
            if (control != null)
            {
                int insanityCount = 0;

                Control tryControl = this.GetNextControl(dgvParameters, forward);

                while (insanityCount < 100 && (!tryControl.CanFocus || !tryControl.TabStop))
                {
                    tryControl = this.GetNextControl(tryControl, forward);
                    insanityCount++;
                }

                if (insanityCount < 100)
                {
                    tryControl.Select();
                }
            }
        }

        private void dgvParameters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.Tab)
                moveFocusForward = false;
            else if (e.KeyCode == Keys.Delete)
            {
                dgvParameters["colValue", dgvParameters.SelectedCells[0].RowIndex].Value = string.Empty;
            }
        }

        private void EnableDisableTestButton()
        {
            tsbTest.Enabled = false;

            if (AllParametersOK())
            {
                tsbTest.Enabled = true;
            }
        }

        private void EnableDisableTestXMLButtons()
        {
            tsbSaveXMLToFile.Enabled = false;
            tsbCopyXMLToClipboard.Enabled = false;

            if (TestButtonClicked && !string.IsNullOrEmpty(tbTextXML.Text))
            {
                tsbSaveXMLToFile.Enabled = true;
                tsbCopyXMLToClipboard.Enabled = true;
            }
        }

        private bool AllParametersOK()
        {
            bool allOk = true;

            if (dgvParameters.Rows.Count == 0)
                allOk = false;
            else
            {
                foreach (DataGridViewRow row in dgvParameters.Rows)
                {
                    // Get the parameter
                    StoredProcedureParameter storedProdParam = (StoredProcedureParameter)row.Cells["colName"].Tag;

                    row.Cells["colValue"].ErrorText = string.Empty;

                    storedProdParam.IsNull = row.Cells["colNull"].EditedFormattedValue == null ? false : (bool)row.Cells["colNull"].EditedFormattedValue;

                    if (storedProdParam.IsNull)
                        row.Cells["colValue"].Style.BackColor = Color.Silver;
                    else
                        row.Cells["colValue"].Style.BackColor = Color.White;

                    switch (storedProdParam.DataType)
                    {
                        case StoredProcedureDataType.NotSupported:
                            {
                                row.Cells["colValue"].ErrorText = "This field is not supported to test in this environment!";
                                allOk = false;
                                break;
                            }
                        case StoredProcedureDataType.Numeric:
                            {
                                decimal decval;

                                if (!storedProdParam.IsNull)
                                {
                                    if (!decimal.TryParse((string)row.Cells["colValue"].Value, out decval))
                                    {
                                        row.Cells["colValue"].ErrorText = "This field is numeric!";
                                        allOk = false;
                                    }
                                    else
                                        storedProdParam.Value = decval;
                                }
                                else
                                {
                                    row.Cells["colValue"].Value = string.Empty;
                                    storedProdParam.Value = string.Empty;
                                }

                                break;
                            }
                        case StoredProcedureDataType.DateTime:
                            {
                                DateTime dateTimeVal;

                                if (!storedProdParam.IsNull)
                                {
                                    if (!DateTime.TryParseExact((string)row.Cells["colValue"].Value, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dateTimeVal))
                                    {
                                        row.Cells["colValue"].ErrorText = "This field is a datetime and should be on the format 'YYYY-MM-DD HH:MI:SS'!";
                                        allOk = false;
                                    }
                                    else
                                        storedProdParam.Value = dateTimeVal;
                                }
                                else
                                {
                                    row.Cells["colValue"].Value = string.Empty;
                                    storedProdParam.Value = string.Empty;
                                }

                                break;
                            }
                        case StoredProcedureDataType.String:
                            {
                                if (!storedProdParam.IsNull)
                                {
                                    if (storedProdParam.Length > 0 &&
                                        ((string)row.Cells["colValue"].Value ?? string.Empty).Length > storedProdParam.Length)
                                    {
                                        row.Cells["colValue"].ErrorText = "This field can only contain " + storedProdParam.Length.ToString() + " character(s)!";
                                        allOk = false;
                                    }
                                    else
                                        storedProdParam.Value = (string)row.Cells["colValue"].Value ?? string.Empty;
                                }
                                else
                                {
                                    row.Cells["colValue"].Value = string.Empty;
                                    storedProdParam.Value = string.Empty;
                                }

                                break;
                            }
                        default:
                            break;

                    }
                }
            }

            return allOk;
        }

        private void dgvParameters_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            EnableDisableTestButton();
        }

        private void tsbTest_Click(object sender, EventArgs e)
        {
            // Check parameters before continuing
            if (!AllParametersOK())
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Save parameters to config
                foreach (DataGridViewRow row in dgvParameters.Rows)
                {
                    // Get the parameter
                    StoredProcedureParameter storedProdParam = (StoredProcedureParameter)row.Cells["colName"].Tag;

                    string value = row.Cells["colValue"].Value == null ? string.Empty : row.Cells["colValue"].Value.ToString();
                    Config.Backend.SetReportTestParameter(report.Name, storedProdParam.Name, value);

                    bool isnull = row.Cells["colNull"].Value == null ? false : (bool)row.Cells["colNull"].Value;
                    Config.Backend.SetReportTestParameterIsNull(report.Name, storedProdParam.Name, isnull);
                }

                // Save the parameter values.
                Config.Save();

                ReportPLSQL plsql = new ReportPLSQL(modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Schema>(BackendApplication.Id)[0].ConnectionString);

                TestDocument = plsql.GetXml(report.PackageName, lastGetXmlParameters);

                tbTextXML.Text = TestDocument != null ? TestDocument.OuterXml.Replace("\n", "\r\n") : string.Empty;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            TestButtonClicked = true;
            EnableDisableTestXMLButtons();
        }

        private void btnCopyXMLToClipboard_Click(object sender, EventArgs e)
        {

        }

        private void tbDocumentTypeDefinition_TextChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }

        private void cbWarehouseReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                PopulateInterface();
                IsModified = true;
                IsInterfaceChanged = true;
            }
        }

        private void tbDocumentType_TextChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }

        private void btnStubCallPrint_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(StubCallPrintText, TextDataFormat.Text);
        }

        private void btnStubBeforePrintBody_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(StubBeforePrintBodyText, TextDataFormat.Text);
        }

        private void btnStubBeforePrintSpec_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(StubBeforePrintSpecText, TextDataFormat.Text);
        }

        private void btnStubAfterPrintBody_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(StubAfterPrintBodyText, TextDataFormat.Text);
        }

        private void btnStubAfterPrintSpec_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(StubAfterPrintSpecText, TextDataFormat.Text);
        }

        private void cbABCEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                PopulateInterface();
                IsModified = true;
                IsInterfaceChanged = true;
            }
        }

        private void tsbSaveXMLToFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbTextXML.Text))
            {
                saveXMLFileDialog.FileName = string.Format("{0}.xml", report.Name);
                saveXMLFileDialog.InitialDirectory = Config.Backend.SaveXMLFileInitialDir;

                if (saveXMLFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        TextWriter textWriter = new StreamWriter(saveXMLFileDialog.FileName, false);
                        textWriter.Write(tbTextXML.Text);
                        textWriter.Close();

                        // Save selected file to config
                        Config.Backend.SaveXMLFileInitialDir = Path.GetDirectoryName(saveXMLFileDialog.FileName);
                        Config.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Error when trying to save the XML to the file: \"{0}\". \n\n{1}", saveXMLFileDialog.FileName, ex.Message),
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tsbCopyXMLToClipboard_Click(object sender, EventArgs e)
        {
            // Copy text to clipboard.
            if (!string.IsNullOrEmpty(tbTextXML.Text))
                Clipboard.SetText(tbTextXML.Text, TextDataFormat.Text);
        }

        //private void BrowseLabelFile()
        //{
        //    string originalFileName = tbLabelFile.Text.Trim();

        //    if (string.IsNullOrEmpty(tbLabelFile.Text.Trim()) || !File.Exists(tbLabelFile.Text))
        //    {
        //        // If running in debug mode in Visual Studio then the relative path will work
        //        if (Directory.Exists(Path.GetFullPath(@"..\..\..\..\..\..\source\SupplyChain\Reporting\Reports\LBL")))
        //            openLabelFileDialog.InitialDirectory = Path.GetFullPath(@"..\..\..\..\..\..\source\SupplyChain\Reporting\Reports\LBL");
        //        else
        //        {
        //            // Try to find the path to the trunk project
        //            string trunkPath = DirectoryHelper.GetProjectTrunkDirectory();

        //            if (!string.IsNullOrEmpty(trunkPath))
        //            {
        //                string path = Path.Combine(trunkPath, @"dotnet\source\SupplyChain\Reporting\Reports\LBL");

        //                if (Directory.Exists(path))
        //                    openLabelFileDialog.InitialDirectory = path;
        //                else
        //                    openLabelFileDialog.InitialDirectory = string.Empty;
        //            }
        //            else
        //            {
        //                string projectDir = DirectoryHelper.GetProjectDirectory();

        //                if (!string.IsNullOrEmpty(projectDir))
        //                    openLabelFileDialog.InitialDirectory = projectDir;
        //                else
        //                    openLabelFileDialog.InitialDirectory = string.Empty;
        //            }
        //        }
        //    }
        //    else
        //        openLabelFileDialog.FileName = tbLabelFile.Text;

        //    if (openLabelFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        tbLabelFile.Text = openLabelFileDialog.FileName;

        //        // If not selected the same file then automatically load the selected file
        //        if (originalFileName != tbLabelFile.Text)
        //            LoadLabelFile();
        //    }
        //}

        private void GetLabelResult()
        {
            if (!string.IsNullOrEmpty(tbLabelLayout.Text.Trim()))
            {
                string errorText;

                if (!TestLabel.LoadTemplate(tbLabelLayout.Text.Trim(), out errorText))
                {
                    tbLabelResult.Text = "Compile Errors:\r\n" + errorText;
                    return;
                }

                if (TestDocument != null)
                {
                    string result = TestLabel.Execute(TestDocument, out errorText);

                    if (!string.IsNullOrEmpty(errorText))
                        tbLabelResult.Text = "Run-time/Variable Errors:\r\n" + errorText;
                    else
                        tbLabelResult.Text = result;
                }
                else
                {
                    tbLabelResult.Text = "There is no XML test data to test with!";
                }
            }
            

        }

        private void tabLabelTest_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // Check if user is selecting the Queries page
            if (e.TabPage.Name == "tpLabelResult")
            {
                GetLabelResult();
            }
        }

        private void tsbNewLabel_Click(object sender, EventArgs e)
        {
            if (!CheckSaveLabel())
                return;

            CurrentLabelFileName = string.Empty;

            tbLabelLayout.Text = "<%" + Environment.NewLine +
                                 "  // This is the codepart. Here you can write C# code and setup variables" + Environment.NewLine +
                                 "  // that you can use in the template part below." + Environment.NewLine +
                                 "  // You don't have to have a codepart." + Environment.NewLine + Environment.NewLine +
                                 "  string x = \"Test\";       // Set a local string variable" + Environment.NewLine +
                                 "  SetVariable(\"myVar\", x); // myVar declaration" + Environment.NewLine +
                                 "%>" + Environment.NewLine +
                                 "This is the template part where you can use variables declared in the codepart." + Environment.NewLine +
                                 "It is also possible to use xpath expressions directly as variables." + Environment.NewLine +
                                 "For example this is how you show \"myVar\" variable we declared above: <%=myVar%>.";
        }

        private bool SaveLabel()
        {
            if (!string.IsNullOrEmpty(tbLabelLayout.Text))
            {
                if (!string.IsNullOrEmpty(CurrentLabelFileName))
                {
                    saveLabelFileDialog.FileName = Path.GetFileName(CurrentLabelFileName);
                    saveLabelFileDialog.InitialDirectory = Path.GetDirectoryName(CurrentLabelFileName);
                }
                else
                {
                    saveLabelFileDialog.FileName = string.Format("{0}_eng.lbl", report.Name);
                    saveLabelFileDialog.InitialDirectory = Config.Backend.SaveLabelFileInitialDir;
                }

                if (saveLabelFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (File.Exists(saveLabelFileDialog.FileName))
                        {
                            FileAttributes fileAttribs = File.GetAttributes(saveLabelFileDialog.FileName);

                            // Check if file is readonly
                            if ((fileAttribs & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                if (MessageBox.Show("The file is readonly!\r\nDo you want to overwrite it anyway?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        File.SetAttributes(saveLabelFileDialog.FileName, fileAttribs ^ FileAttributes.ReadOnly);
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Error when trying to remove readonly flag on file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                else
                                    return false;
                            }
                        }

                        TextWriter textWriter = new StreamWriter(saveLabelFileDialog.FileName, false);
                        textWriter.Write(tbLabelLayout.Text);
                        textWriter.Close();

                        // Save selected file to config
                        Config.Backend.SaveLabelFileInitialDir = Path.GetDirectoryName(saveLabelFileDialog.FileName);
                        Config.Save();

                        LabelChanged = false;
                        if (string.IsNullOrEmpty(CurrentLabelFileName))
                            CurrentLabelFileName = saveLabelFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Error when trying to save the Label to the file: \"{0}\". \n\n{1}", saveLabelFileDialog.FileName, ex.Message),
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        private void tbLabelLayout_TextChanged(object sender, EventArgs e)
        {
            LabelChanged = true;
        }

        private void cmsLabelLayout_Opening(object sender, CancelEventArgs e)
        {
            // Check if there is an XML
            if (TestDocument != null)
            {
                tsmLabelCodeXPath.Enabled = true;
                tsmLabelTemplateVariables.Enabled = true;

                // Get Data and MetaData nodes
                XmlNode xmlData = TestDocument.SelectSingleNode("/*/Data/*");
                XmlNode xmlMeta = TestDocument.SelectSingleNode("/*/MetaData");

                tsmLabelCodeXPath.DropDownItems.Clear();
                tsmLabelTemplateVariables.DropDownItems.Clear();

                FillMenuXPathWithXMLTree(xmlData, xmlData, tsmLabelCodeXPath, "Data(\"{0}\")");
                tsmLabelCodeXPath.DropDownItems.Add("-");
                FillMenuXPathWithXMLTree(xmlMeta, xmlMeta, tsmLabelCodeXPath, "MetaData(\"{0}\")");
                FillMenuXPathWithXMLTree(xmlData, xmlData, tsmLabelTemplateVariables, "<%={0}%>");
                tsmLabelTemplateVariables.DropDownItems.Add("-");
                FillMenuXPathWithXMLTree(xmlMeta, xmlMeta, tsmLabelTemplateVariables, "<%={0}%>");
            }
            else
            {
                tsmLabelCodeXPath.Enabled = false;
                tsmLabelTemplateVariables.Enabled = false;
            }

            // Get defined variables
            tsmLabelDefinedVariables.DropDownItems.Clear();
            IList<string> definedVariables = ParseLabel.GetVariablesFromCodeBlock(tbLabelLayout.Text);

            if (definedVariables.Count > 0)
            {
                tsmLabelDefinedVariables.Enabled = true;
                foreach (string varName in definedVariables.OrderBy(x => x))
                {
                    ToolStripMenuItem item = (ToolStripMenuItem)tsmLabelDefinedVariables.DropDownItems.Add(varName);
                    item.Tag = string.Format("<%={0}%>", varName);
                    item.Click += new EventHandler(LabelAttributeMenuItem_Click);
                }
            }
            else
                tsmLabelDefinedVariables.Enabled = false;

        }

        private void FillMenuXPathWithXMLTree(XmlNode xmlNode, XmlNode topNode, ToolStripMenuItem tsmItem, string formatString)
        {
            IList<string> xmlAttributeList = GetXmlAttributeList(xmlNode, topNode);

            foreach (string attributeString in xmlAttributeList.OrderBy(s => s))
            {
                ToolStripMenuItem attributeItem = (ToolStripMenuItem)tsmItem.DropDownItems.Add(attributeString);
                attributeItem.Tag = string.Format(formatString, attributeString);
                attributeItem.Click += new EventHandler(LabelAttributeMenuItem_Click);
            }
        }

        private IList<string> GetXmlAttributeList(XmlNode xmlNode, XmlNode topNode)
        {
            List<string> xmlAttributeList = new List<string>();

            FillXmlAttributeDictionary(xmlAttributeList, xmlNode, topNode);

            return xmlAttributeList;
        }

        private void FillXmlAttributeDictionary(IList<string> xmlAttributeList, XmlNode xmlNode, XmlNode topNode)
        {
            if (xmlNode != null)
            {
                if (xmlNode.HasChildNodes)
                {
                    foreach (XmlNode childXmlNode in xmlNode.ChildNodes)
                    {
                        if (childXmlNode.NodeType == XmlNodeType.Element)
                        {
                            FillXmlAttributeDictionary(xmlAttributeList, childXmlNode, topNode);
                        }
                    }
                }

                if (xmlNode.Attributes.Count > 0)
                {
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.NodeType == XmlNodeType.Attribute)
                            xmlAttributeList.Add(GetXPath(attribute, xmlNode, topNode));
                    }
                }
            }
        }


        private string GetXPath(XmlAttribute attribute, XmlNode xmlNode, XmlNode topNode)
        {
            string xpath = string.Format("@{0}", attribute.Name);

            while (xmlNode != topNode && xmlNode != null)
            {
                xpath = xmlNode.Name + "/" + xpath;

                xmlNode = xmlNode.ParentNode;
            }

            return xpath;
        }

        void LabelAttributeMenuItem_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText((string)(sender as ToolStripMenuItem).Tag);
        }

        private void LabelLayoutInsertText(string text)
        {
            int currentIndex = tbLabelLayout.SelectionStart;

            // Remove anything selected
            if (tbLabelLayout.SelectionLength > 0)
                tbLabelLayout.Text = tbLabelLayout.Text.Remove(currentIndex, tbLabelLayout.SelectionLength);

            tbLabelLayout.Text = tbLabelLayout.Text.Insert(currentIndex, text);

            // Set new caret position
            tbLabelLayout.SelectionStart = currentIndex + text.Length;

            // Set focus back
            tbLabelLayout.ScrollToCaret();
            tbLabelLayout.Focus();
        }

        private void tsmLabelFuncData_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("Data( xpathString );");
        }

        private void tsmLabelFuncMetaData_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("MetaData( xpathString );");
        }

        private void xmlDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("XmlDoc");
        }

        private void xmlDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("XmlData");
        }

        private void xmlMetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("XmlMeta");
        }

        private void tsbLoadLabel_Click(object sender, EventArgs e)
        {
            if (!CheckSaveLabel())
                return;

            string initialDir = Config.Backend.SaveLabelFileInitialDir;

            if (string.IsNullOrEmpty(initialDir) || !Directory.Exists(initialDir))
            {
                // If running in debug mode in Visual Studio then the relative path will work
                if (Directory.Exists(Path.GetFullPath(@"..\..\..\..\..\..\source\SupplyChain\Reporting\Reports\LBL")))
                    initialDir = Path.GetFullPath(@"..\..\..\..\..\..\source\SupplyChain\Reporting\Reports\LBL");
                else
                {
                    initialDir = string.Empty;
                }
            }

            if (string.IsNullOrEmpty(CurrentLabelFileName))
                openLabelFileDialog.FileName = string.Format("{0}_eng.lbl", report.Name);
            else
                openLabelFileDialog.FileName = string.Empty;
            openLabelFileDialog.InitialDirectory = initialDir;

            if (openLabelFileDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentLabelFileName = openLabelFileDialog.FileName;
                tbLabelLayout.Text = File.ReadAllText(openLabelFileDialog.FileName);
                LabelChanged = false;
            }
        }

        private bool CheckSaveLabel()
        {
            if (LabelChanged)
            {
                if (MessageBox.Show("Do you want to save the current label before continuing?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (!SaveLabel())
                        return false;
                }
            }
            return true;
        }

        private void tsbSaveLabel_Click(object sender, EventArgs e)
        {
            SaveLabel();
        }

        private void tsmLabelFuncGetDateTime_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("GetDateTime( string )");
        }

        private void tsmLabelFuncGetNumber_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("GetNumber( string )");
        }

        private void tsmLabelFuncGetBoolean_Click(object sender, EventArgs e)
        {
            LabelLayoutInsertText("GetBoolean( string )");
        }

        private void tsmLoadXMLAsTestDataToLabel_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.LoadXml(tbTextXML.Text);
                TestDocument.LoadXml(tbTextXML.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in XML: " + ex.Message, "Error in XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvParameters_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvParameters.Columns[dgvParameters.CurrentCell.ColumnIndex] is DataGridViewCheckBoxColumn)
            {
                dgvParameters.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvParameters_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsLoadingTestParameters)
            {
                EnableDisableTestButton();
            }
        }

        private void tsbResetTestParameters_Click(object sender, EventArgs e)
        {
            if (dgvParameters.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvParameters.Rows)
                {
                    // Get the parameter
                    StoredProcedureParameter storedProdParam = (StoredProcedureParameter)row.Cells["colName"].Tag;

                    // Delete the parameters from config
                    Config.Backend.DeleteReportTestParameter(report.Name, storedProdParam.Name);
                    Config.Backend.DeleteReportTestParameterIsNull(report.Name, storedProdParam.Name);

                    // Repopulate the parameters
                    PopulateParameters(true);
                }
            }
        }

        

        private void tsbRawPrintToShare_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tstbPrinterShare.Text) &&
                !string.IsNullOrEmpty(tbLabelResult.Text))
            {
                // Save printer to config
                Config.Backend.SetReportRawPrinter(report.Name, tstbPrinterShare.Text);
                Config.Save();

                try
                {
                    int charsWritten = RawPrinting.Print(tstbPrinterShare.Text, report.Name, Encoding.Default.GetBytes(tbLabelResult.Text));

                    if (charsWritten > 0)
                        MessageBox.Show(string.Format("Printed {0} characters successfully to printer '{1}'.", charsWritten.ToString(), tstbPrinterShare.Text), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Printing failed to printer '{0}'.\r\n{1}", tstbPrinterShare.Text, ex.Message), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
                
        private void nudDataDuplicates_ValueChanged(object sender, EventArgs e)
        {
            IsModified = true;
        }
    }
}
