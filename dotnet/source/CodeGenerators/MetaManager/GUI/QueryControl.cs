using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Cdc.Framework.Parsing.OracleSQLParsing;



namespace Cdc.MetaManager.GUI
{
    public partial class QueryControl : UserControl
    {
        private bool isNewAction;
        private Schema schema;
        private string actionName;
        public Query Query { get; set; }        

        private IApplicationService applicationService;
        private IModelService modelService;

        public bool IsQueryCompiled;
        public bool IsEditable;
        
        public QueryControl()
        {
            InitializeComponent();
        }

        new public void Load(bool isNewAction, string actionName, Schema schema, Query existingQuery, bool isEditable)
        {
            applicationService = MetaManagerServices.GetApplicationService();
            modelService = MetaManagerServices.GetModelService();

            Query = existingQuery;
            this.isNewAction = isNewAction;
            this.schema = modelService.GetInitializedDomainObject<DataAccess.Domain.Schema>(schema.Id);                        
            this.actionName = actionName;            
            this.IsEditable = isEditable;           

            if (isNewAction)
            {
                queryNameTb.Text = actionName + "Qry";
                tbConnectionString.Text = schema.ConnectionString;
            }
            else
            {
                queryNameTb.Text = Query.Name;
                rtSQL.Text = Query.SqlStatement;
                tbConnectionString.Text = Query.Schema.ConnectionString;
                btnFindDatatypes_Click(null,null);
            }

            rtSQL.Focus();
            EnableDisableOptions();
        }

        private void EnableDisableOptions()
        {
            queryNameTb.ReadOnly = true;
            rtSQL.ReadOnly = !this.IsEditable;
            revertBtn.Enabled = this.IsEditable;
            btnFindDatatypes.Enabled = this.IsEditable;            
        }

        private string originalSQL;

        private string OriginalSQL
        {
            get
            {
                if (originalSQL == null)
                {
                    originalSQL = Query.SqlStatement;
                }
                return originalSQL;
            }
            set
            {
                if (originalSQL == null)
                {
                    originalSQL = value;
                }
            }
        }

        private void revertBtn_Click(object sender, EventArgs e)
        {
            rtSQL.Text = OriginalSQL;
            Query.SqlStatement = OriginalSQL;
        }

        private void btnFindDatatypes_Click(object sender, EventArgs e)
        {
            CompileQuery();
        }

        public void CompileQuery()
        {
            IsQueryCompiled = false;

            try
            {
                OracleQuery oracleQuery = OracleQueryAnalyzer.Analyze(rtSQL.Text, tbConnectionString.Text);

                if (oracleQuery != null)
                {
                    if (oracleQuery.ParseErrors.Count == 0)
                    {
                        if (oracleQuery.ParseWarnings.Count > 0)
                        {
                            foreach (string warning in oracleQuery.ParseWarnings)
                            {
                                MessageBox.Show(warning, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                        MetaManagerServices.Helpers.QueryHelper.UpdateQuery(Query, oracleQuery);

                        ShowQueryInformation();
                    }
                    else
                    {
                        foreach (string error in oracleQuery.ParseErrors)
                        {
                            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    IsQueryCompiled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Compile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowQueryInformation()
        {
            lvOutput.Items.Clear();
            lvInput.Items.Clear();
            lvFrom.Items.Clear();

            foreach (QueryProperty qp in Query.Properties)
            {
                if (qp.PropertyType == DbPropertyType.Out)
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
                    else if (qp.DbDatatype == "VARCHAR2")
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
                else if (qp.PropertyType == DbPropertyType.In)
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

        public string QueryName
        {
            get
            {
                return queryNameTb.Text.Trim();
            }
        }
    }
}
