using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Cdc.Framework.Parsing.OracleSQLParsing;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Context;
using Spring.Context.Support;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.BusinessLogic.Helpers;

namespace Cdc.MetaManager.GUI
{
    public partial class CreateEditReportSQL : MdiChildForm
    {
        private static IApplicationContext ctx;

        public Query CurrentQuery { get; set; }
        public Report Report { private get; set; }
        public ReportQuery ParentReportQuery { get; private set; }
        public int MaxQueryNameLength { private get; set; }

        private IApplicationService applicationService = null;
        private IDialogService dialogService = null;
        private IModelService modelService = null;
        private MetaManager.BusinessLogic.Helpers.QueryHelper queryHelper = null;
        private string originalName = string.Empty;
        private Dictionary<int, string> outParameterNames = null;

        private ReportSQLSources ExistingSources = null;
       
        public bool PropertiesAdded { get; private set; }

        public ReportSQLVariable SelectedVariable 
        {
            get
            {
                if (lvVariables.SelectedItems.Count == 1)
                {
                    return (ReportSQLVariable)lvVariables.SelectedItems[0].Tag;
                }
                else
                    return null;
            }
        }

        public CreateEditReportSQL()
        {
            InitializeComponent();
            ctx = ContextRegistry.GetContext();
            applicationService = MetaManagerServices.GetApplicationService();
            dialogService = MetaManagerServices.GetDialogService();
            queryHelper = MetaManagerServices.Helpers.QueryHelper;
            modelService = MetaManagerServices.GetModelService();
            outParameterNames = new Dictionary<int, string>();
            MaxQueryNameLength = 0;
        }

        private void AddAnalyzeSQL_Load(object sender, EventArgs e)
        {
            EnableDisableButtons();
            // Set SQL statement
            rtSQL.Text = CurrentQuery.SqlStatement;

            // Set name of SQL
            tbName.Text = CurrentQuery.Name;

            if (MaxQueryNameLength > 0)
                tbName.MaxLength = MaxQueryNameLength;

            // Get the connectionstring from the config
            tbConnectionString.Text = CurrentQuery.Schema.ConnectionString;

            originalName = CurrentQuery.Name ?? string.Empty;

            // Load all existing sources to be able to populate variables and contextmenu
            ExistingSources = new ReportSQLSources(applicationService, Report);

            // Populate the sources lists
            PopulateSources();

            // Select the first source in the list
            cbVarSource.SelectedIndex = 0;

            // Create context menu
            PopulateContextMenu();

            AnalyzeQuery();
        }

        private void EnableDisableButtons()
        {
            if (!IsEditable)
            {
                revertBtn.Enabled = false;
                btnFindDatatypes.Enabled = false;
                btnOK.Enabled = false;
                tbName.ReadOnly = true;
                rtSQL.ReadOnly = true;
                //cbVarSource.Enabled = false;
            }
        }

        private string OriginalSQL
        {
            get
            {
                return CurrentQuery.SqlStatement ?? string.Empty;
            }
        }

        private void btnFindDatatypes_Click(object sender, EventArgs e)
        {
            AnalyzeQuery();
        }

        private void AnalyzeQuery()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                btnOK.Enabled = false;

                OracleQuery oracleQuery = OracleQueryAnalyzer.Analyze(rtSQL.Text, tbConnectionString.Text, true);

                if (oracleQuery != null)
                {
                    if (oracleQuery.ParseErrors.Count == 0)
                    {
                        if (oracleQuery.ParseWarnings.Count > 0)
                        {
                            Cursor.Current = Cursors.Default;

                            foreach (string warning in oracleQuery.ParseWarnings)
                            {
                                MessageBox.Show(warning, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                        queryHelper.UpdateQuery(CurrentQuery, oracleQuery);

                        ShowQueryInformation();

                        if (IsEditable)
                        {
                            btnOK.Enabled = true;
                        }
                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;

                        foreach (string error in oracleQuery.ParseErrors)
                        {
                            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Compile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            System.Windows.Forms.Application.DoEvents();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckNameFocus(tbName, "Query Name", true))
                return;

            // Check length of queryname
            if (MaxQueryNameLength > 0 &&
                tbName.Text.Trim().Length > MaxQueryNameLength)
            {
                MessageBox.Show("The Query Name is too long, it can be maximum " + MaxQueryNameLength.ToString() + " characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Find the parameters that is nonunique
            if (outParameterNames.Count > 0)
            {
                IList<string> list = outParameterNames.Values.GroupBy(s => s).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

                if (list.Count > 0)
                {
                    // Make a nice comma separated list!
                    string listNames = list.Aggregate((current, next) => current + ", " + next);

                    if (list.Count > 1)
                        MessageBox.Show("The parameters with the names " + listNames + " must be renamed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("The parameters with the name " + listNames + " must be renamed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            ReportQuery parentReportQuery;
            string errorText;

            if (!ReportHelper.CheckReportParameterCombinations(ExistingSources, rtSQL.Text, out errorText, out parentReportQuery))
            {
                MessageBox.Show(errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            ParentReportQuery = parentReportQuery;
                        
            CurrentQuery.Name = tbName.Text.Trim();
                        
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Reset everything from the analyze
            CurrentQuery.Properties.Clear();

            CurrentQuery.Name = originalName;

            DialogResult = DialogResult.Cancel;
        }

        private void revertBtn_Click(object sender, EventArgs e)
        {
            rtSQL.Text = OriginalSQL;
            CurrentQuery.SqlStatement = OriginalSQL;
        }

        private void rtSQL_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
        }

        private void PopulateSources()
        {
            cbVarSource.Items.Clear();

            foreach (ReportSQLSource source in ExistingSources.Sorted().Where(s => s.Name != CurrentQuery.Name))
            {
                // Add the source to the combobox
                cbVarSource.Items.Add(source);
            }
        }

        private void PopulateContextMenu()
        {
            ctxVariableMenu.Items.Clear();

            foreach (ReportSQLSource source in ExistingSources.Sorted().Where(s => s.Name != CurrentQuery.Name))
            {
                ToolStripMenuItem item = new ToolStripMenuItem(source.Name);

                foreach (ReportSQLVariable variable in source.Variables.OrderBy(v => v.Name))
                {
                    ToolStripMenuItem varItem;
                    
                    if (!string.IsNullOrEmpty(variable.TableColumn))
                        varItem = new ToolStripMenuItem(string.Format("{0} [{1}]", variable.Name, variable.TableColumn));
                    else
                        varItem = new ToolStripMenuItem(variable.Name);

                    varItem.Tag = variable;
                    varItem.Click += new EventHandler(varItem_Click);

                    item.DropDownItems.Add(varItem);
                }

                ctxVariableMenu.Items.Add(item);
            }
        }

        void varItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem &&
                ((ToolStripMenuItem)sender).Tag is ReportSQLVariable)
            {
                InsertVariableText((ReportSQLVariable)((ToolStripMenuItem)sender).Tag);
            }
        }


        private void cbVarSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateVariables();
        }

        private void PopulateVariables()
        {
            if (cbVarSource.SelectedItem != null)
            {
                ReportSQLSource selectedSource = (ReportSQLSource)cbVarSource.SelectedItem;

                // Clear the variable list
                lvVariables.Items.Clear();

                lvVariables.BeginUpdate();

                foreach (ReportSQLVariable variable in selectedSource.Variables.OrderBy(v => v.Name))
                {
                    ListViewItem item = lvVariables.Items.Add(variable.Name);
                    item.SubItems.Add(variable.Type);
                    item.SubItems.Add(variable.TableColumn);
                    item.Tag = variable;
                }

                lvVariables.EndUpdate();
            }
        }

        private void lvVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) // Enter
            {
                InsertVariableText();
            }
        }


        private void InsertVariableText()
        {
            InsertVariableText(SelectedVariable);
        }

        private void InsertVariableText(ReportSQLVariable variable)
        {
            // Check if there is a colon at the current position
            if (variable != null)
            {
                int currentIndex = rtSQL.SelectionStart;

                // Remove anything selected
                if (rtSQL.SelectionLength > 0)
                    rtSQL.Text = rtSQL.Text.Remove(currentIndex, rtSQL.SelectionLength);

                // Insert variable text
                bool previousIsColon = (currentIndex - 1 >= 0) && rtSQL.Text[currentIndex - 1] == ':';

                string variableText = variable.GetVariableName(!previousIsColon);

                rtSQL.Text = rtSQL.Text.Insert(currentIndex, variableText);

                // Set new caret position
                rtSQL.SelectionStart = currentIndex + variableText.Length;

                // Set focus back to SQL
                rtSQL.Focus();
            }
        }

        private void lvVariables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            InsertVariableText();
        }

        private void rtSQL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ':')
            {
                // Check how many ' exist in text. If its an even number then popup the menu.
                if (CountStringOccurrences(rtSQL.Text, "'") % 2 == 0)
                {
                    Point pt = rtSQL.GetPositionFromCharIndex(rtSQL.SelectionStart);
                    ctxVariableMenu.Show(rtSQL.PointToScreen(pt));
                }
            }
        }

        public static int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        private void ShowQueryInformation()
        {
            // Clear listviews
            lvOutput.Items.Clear();
            lvInput.Items.Clear();
            lvFrom.Items.Clear();
            lvOutParameters.Items.Clear();
            outParameterNames.Clear();

            foreach (QueryProperty qp in CurrentQuery.Properties)
            {
                // Show properties for select part
                if (qp.PropertyType == DbPropertyType.Out)
                {
                    AddToSQLOutputList(qp);
                    AddToOutParameterList(qp);
                }
                else if (qp.PropertyType == DbPropertyType.In)
                {
                    AddToSQLInputList(qp);
                }
            }

            if (OracleQueryAnalyzer.FromTables != null && OracleQueryAnalyzer.FromTables.Count > 0)
            {
                lvFrom.Enabled = true;

                foreach (string tablename in OracleQueryAnalyzer.FromTables.Keys)
                {
                    ListViewItem item = lvFrom.Items.Add(tablename);

                    string aliases = string.Empty;

                    foreach (string alias in OracleQueryAnalyzer.FromAliases.Keys)
                    {
                        if (OracleQueryAnalyzer.FromAliases[alias] == tablename)
                        {
                            aliases += alias + ", ";
                        }
                    }

                    if (aliases != string.Empty)
                    {
                        aliases = aliases.Substring(0, aliases.Length - 2);
                    }

                    item.SubItems.Add(aliases);
                }
            }
            else
            {
                lvFrom.Enabled = false;
            }
        }

        private void AddToOutParameterList(QueryProperty qp)
        {
            string propertyName = qp.Name;
            string tableColumn = string.Empty;

            if (!string.IsNullOrEmpty(qp.OriginalColumn) &&
                !string.IsNullOrEmpty(qp.OriginalTable))
            {
                Property property = applicationService.GetPropertyByTableAndColumn(qp.OriginalTable,
                                                                                   qp.OriginalColumn,
                                                                                   BackendApplication.Id);

                if (property != null)
                {
                    tableColumn = string.Format("{0}.{1}", qp.OriginalTable, qp.OriginalColumn);

                    if (qp.Name == qp.OriginalColumn)
                        propertyName = property.Name;
                    else
                        propertyName = qp.Name;
                }
            }

            ListViewItem item = lvOutParameters.Items.Add(propertyName);
            item.SubItems.Add(tableColumn);

            if (outParameterNames.Values.Contains(propertyName))
            {
                item.ForeColor = Color.Red;
            }

            outParameterNames.Add(qp.Sequence, propertyName);
        }

        private void AddToSQLInputList(QueryProperty qp)
        {
            ListViewItem item = lvInput.Items.Add(qp.Sequence.ToString());

            item.SubItems.Add(qp.Name);

            if (String.IsNullOrEmpty(qp.OriginalTable))
            {
                item.SubItems.Add("");
            }
            else
            {
                item.SubItems.Add(qp.OriginalTable + "." + qp.OriginalColumn);
            }

            item.SubItems.Add(qp.Text);
            item.Tag = qp;
        }

        private void AddToSQLOutputList(QueryProperty qp)
        {
            ListViewItem item = lvOutput.Items.Add(qp.Sequence.ToString());
            item.SubItems.Add(qp.Name);

            if (String.IsNullOrEmpty(qp.OriginalTable))
            {
                item.SubItems.Add("");
            }
            else
            {
                item.SubItems.Add(qp.OriginalTable + "." + qp.OriginalColumn);
            }

            if (qp.DbDatatype == "NUMBER")
            {
                string tmp = qp.DbDatatype;

                tmp += "(";

                if (qp.Precision != null)
                {
                    tmp += qp.Precision.ToString();
                }
                else
                {
                    tmp += "?";
                }

                if (qp.Scale != null)
                {
                    if (qp.Scale != 0)
                    {
                        tmp += ", " + qp.Scale.ToString();
                    }
                }
                else
                {
                    tmp += ", ?";
                }

                tmp += ")";

                item.SubItems.Add(tmp);
            }
            else if (qp.DbDatatype == "VARCHAR2" ||
                     qp.DbDatatype == "CHAR")
            {
                string tmp = qp.DbDatatype;

                tmp += "(";

                if (qp.Length != null)
                {
                    tmp += qp.Length.ToString();
                }
                else
                {
                    tmp += "?";
                }

                tmp += ")";

                item.SubItems.Add(tmp);
            }
            else
            {
                item.SubItems.Add(qp.DbDatatype);
            }
            item.SubItems.Add(qp.Text);
            item.Tag = qp;
        }

        private void ctxVariableMenu_Opened(object sender, EventArgs e)
        {
            ctxVariableMenu.Items[0].Select();
        }

    }

}
