using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Oracle.DataAccess.Client;
using Cdc.MetaManager.DataAccess.Domain;
using Spring.Context;
using Spring.Context.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic;
using System.Text.RegularExpressions;
using Spring.Data.NHibernate.Support;

using NHibernate;

namespace Cdc.MetaManager.GUI
{
    public partial class DataModelImportForm : MdiChildForm
    {
        private IApplicationService appService = null;
        private IDialogService dialogService = null;
        private IModelService modelService = null;
        private Schema currentSchema = null;

        public DataModelImportForm()
        {
            InitializeComponent();

            appService = MetaManagerServices.GetApplicationService();
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        private void DataModelImportForm_Load(object sender, EventArgs e)
        {
            currentSchema = appService.GetSchemaByApplicationId(BackendApplication.Id);
            BackendApplication = modelService.GetInitializedDomainObject<DataAccess.Domain.Application>(BackendApplication.Id);

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Get all tables that is available
                PopulateTables();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void PopulateTables()
        {
            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
            {
                // Save all unique dictionmary entries
                Dictionary<string, string> uniqueEntityExistingDict = new Dictionary<string, string>();
                Dictionary<string, string> uniqueEntityNewDict = new Dictionary<string, string>();

                // Fetch all businessentities to match
                IList<BusinessEntity> allExistingEntities = modelService.GetAllDomainObjectsByApplicationId<BusinessEntity>(BackendApplication.Id);

                // Try to update tablename if not set
                UpdateTableName(allExistingEntities);

                // Fetch the list of tables
                IList<DataModelTableInfo> tableList = DataModelImporter.GetAllTablesInSchema(currentSchema);

                lvTables.Items.Clear();
                lvTables.BeginUpdate();

                foreach (DataModelTableInfo table in tableList)
                {
                    LocalTableItem localTable = new LocalTableItem();
                    localTable.RemoteTable = table;
                    localTable.LocalBusinessEntity = null;
                    localTable.Syncable = true;

                    IList<BusinessEntity> findBusinessEntities = allExistingEntities.Where(e => e.TableName == table.TableName).ToList();

                    bool exists = findBusinessEntities.Count == 1;

                    if (exists)
                    {
                        localTable.LocalBusinessEntity = findBusinessEntities[0];

                        // Remove the businessentity from the list so we know which we have left afterwards
                        allExistingEntities.Remove(localTable.LocalBusinessEntity);
                    }

                    ListViewItem item = new ListViewItem(table.TableName);

                    string entityName = DataModelImporter.GetCaptionFromComment(table.TableComment);
                    if (string.IsNullOrEmpty(entityName))
                        entityName = table.TableName;

                    if (exists)
                        item.SubItems.Add(localTable.LocalBusinessEntity.Name);
                    else
                        item.SubItems.Add(entityName);

                    item.SubItems.Add(table.TableCreated.ToShortDateString());
                    item.SubItems.Add(exists ? "Existing" : "New");
                    item.SubItems.Add(table.TableComment.TrimEnd(new char[] { '\n', ' ' }).Replace("\n", " - "));

                    item.Tag = localTable;

                    if (!exists)
                        item.ForeColor = Color.Green;

                    lvTables.Items.Add(item);

                    if (exists)
                    {
                        if (!uniqueEntityExistingDict.ContainsKey(entityName))
                        {
                            uniqueEntityExistingDict[entityName] = table.TableName;
                        }
                        else
                        {
                            // Error, same entity name found again.
                            string tablename = uniqueEntityExistingDict[entityName];

                            AddToProgressText(string.Format("ERROR:\tTable {0} will have the same business entity name as table {1} after updating.\n" +
                                                            "\tIt will not be possible to update until the table comment is changed.\n",
                                                            table.TableName, tablename));

                            item.BackColor = Color.LightSalmon;

                            localTable.Syncable = false;

                            // Find the other table and set the color and set to uncheckable
                            ListViewItem otherTableItem = lvTables.FindItemWithText(tablename);

                            if (otherTableItem != null)
                            {
                                otherTableItem.BackColor = Color.LightSalmon;

                                LocalTableItem otherLocalTableItem = (LocalTableItem)otherTableItem.Tag;
                                otherLocalTableItem.Syncable = false;
                            }
                        }
                    }
                    else
                    {
                        // First check if the new table is ok with the other new tables namewise.
                        if (!uniqueEntityNewDict.ContainsKey(entityName))
                        {
                            uniqueEntityNewDict[entityName] = table.TableName;
                        }
                        else
                        {
                            // Error, same entity name found again.
                            string tablename = uniqueEntityNewDict[entityName];

                            AddToProgressText(string.Format("ERROR:\tThe new table {0} will have the same business entity name as the table {1} after updating.\n" +
                                                            "\tIt will not be possible to import this table until the table comment is changed.\n",
                                                            table.TableName, tablename));

                            item.BackColor = Color.LightSalmon;

                            localTable.Syncable = false;

                            // Find the other table and set the color and set to uncheckable
                            ListViewItem otherTableItem = lvTables.FindItemWithText(tablename);

                            if (otherTableItem != null)
                            {
                                otherTableItem.BackColor = Color.LightSalmon;

                                LocalTableItem otherLocalTableItem = (LocalTableItem)otherTableItem.Tag;
                                otherLocalTableItem.Syncable = false;
                            }
                        }

                        // Now check if the entityname is ok with the existing businessentities namewise
                        // If not then only set the new table to uncheckable.
                        if (uniqueEntityExistingDict.ContainsKey(entityName))
                        {
                            // Error, entity name found
                            string tablename = uniqueEntityExistingDict[entityName];

                            AddToProgressText(string.Format("ERROR:\tThe new table {0} will have the same business entity name as the existing table {1} after updating.\n" +
                                                            "\tIt will not be possible to import this table until the table comment is changed.\n",
                                                            table.TableName, tablename));

                            item.BackColor = Color.LightSalmon;

                            localTable.Syncable = false;
                        }

                    }
                }

                // List all businessentities left in list. These must have been removed.
                foreach (BusinessEntity entity in allExistingEntities)
                {
                    // Do not show any entities that doesn't have a tablename defined.
                    // These are used internally like CustomProperties
                    if (string.IsNullOrEmpty(entity.TableName))
                        continue;

                    LocalTableItem localTable = new LocalTableItem();
                    localTable.RemoteTable = null;
                    localTable.LocalBusinessEntity = entity;
                    localTable.Syncable = true;

                    ListViewItem item = new ListViewItem(entity.TableName);

                    item.SubItems.Add(entity.Name);
                    item.SubItems.Add(string.Empty);
                    item.SubItems.Add("Removed");
                    item.SubItems.Add(string.Empty);

                    item.Tag = localTable;

                    item.ForeColor = Color.Red;

                    lvTables.Items.Add(item);
                }

                lvTables.EndUpdate();
            }
        }

        private void UpdateTableName(IList<BusinessEntity> allExistingEntities)
        {
            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Auto, true))
            {
                foreach (BusinessEntity businessEntity in allExistingEntities)
                {
                    if (string.IsNullOrEmpty(businessEntity.TableName))
                    {
                        if (businessEntity.Properties.Count > 0 &&
                            businessEntity.Properties[0].StorageInfo != null &&
                            !string.IsNullOrEmpty(businessEntity.Properties[0].StorageInfo.TableName))
                        {
                            businessEntity.TableName = businessEntity.Properties[0].StorageInfo.TableName;
                            modelService.SaveDomainObject(businessEntity);
                        }
                    }
                }
            }
        }

        void DoCallback(string passText,
                        int maxSteps,
                        int currentStep,
                        string currentStepText)
        {
            if (maxSteps <= 0)
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Maximum = maxSteps;
                progressBar.Value = currentStep;
            }

            AddToProgressText(currentStepText);

            System.Windows.Forms.Application.DoEvents();
        }

        private void AddToProgressText(string text)
        {
            tbProgress.Text += text.Replace("\n", Environment.NewLine);

            tbProgress.Select(tbProgress.Text.Length + 1, 2);
            tbProgress.ScrollToCaret();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            List<string> tableNames = new List<string>();

            CallbackService callbackService = new CallbackService();
            callbackService.SetCallback(DoCallback);
            callbackService.Initialize(lvTables.CheckedItems.Count);

            tbProgress.Clear();           

            foreach (ListViewItem item in lvTables.CheckedItems)
            {
                LocalTableItem tableItem = (LocalTableItem)item.Tag;

                if (tableItem.RemoteTable != null)
                {
                    tableNames.Add(tableItem.RemoteTable.TableName);
                }
            }


            bool changesMade = false;

            if (tableNames.Count > 0)
            {
                IList<BusinessEntity> businessEntities = null; 
                DataModelChanges detectedChanges = null;
                IList<BusinessEntity> existingBusinessEntities = null;

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnClose.Enabled = false;

                    using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                    {
                        businessEntities = DataModelImporter.AnalyzeTables(currentSchema, tableNames, false);
                        existingBusinessEntities = modelService.GetAllDomainObjectsByApplicationId<BusinessEntity>(BackendApplication.Id);

                        AddToProgressText("\nAnalyzing fetched data compared to MetaManager database... ");

                        detectedChanges = DataModelImporter.CompareEntities(businessEntities, existingBusinessEntities);

                        AddToProgressText("done!\n");

                    }
                }
                catch (Exception ex)
                {
                    // Exception caught. Do nothing further!
                    MessageBox.Show(string.Format("Error caught when retreiving information from database!\r\nFix the problem and try again!\r\nError:\r\n\t{0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    btnClose.Enabled = true;
                }

                if (detectedChanges != null &&
                    detectedChanges.Count > 0)
                {
                    AddToProgressText("\nShowing changes to user to decide what changes to apply...\n");

                    using (ShowDataModelChanges form = new ShowDataModelChanges())
                    {
                        form.BackendApplication = BackendApplication;
                        form.FrontendApplication = FrontendApplication;

                        form.DetectedChanges = detectedChanges;

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            AddToProgressText("\nSaving new and changed properties and business entities... ");
                            // Update detected changes depending on user input from dialog.
                            DataModelImporter.CompareEntities(businessEntities, existingBusinessEntities, detectedChanges);

                            try
                            {
                                MetaManagerServices.Helpers.ApplicationHelper.UpdateBackendDataModel(detectedChanges, BackendApplication);
                                changesMade = true;
                            }
                            catch (ModelAggregatedException ex)
                            {
                                string mess = ex.Message;
                                string ids = string.Empty;
                                foreach (string id in ((ModelAggregatedException)ex).Ids)
                                {
                                    ids += id + "\r\n";
                                }

                                Clipboard.SetText(ids);
                                mess += "\r\n\r\nThe Ids has been copied to the clipboard";
                                AddToProgressText("\r\n");
                                AddToProgressText("\r\n");
                                AddToProgressText(mess);
                                AddToProgressText("\r\n");


                            }
                            catch (Exception ex)
                            {
                                AddToProgressText("\n\n");
                                AddToProgressText(ex.Message);
                                AddToProgressText("\n");

                            }
                        }
                        else
                        {
                            AddToProgressText("\nUser canceled!\n");
                        }
                    }
                }
                else
                {
                    AddToProgressText("\nNo changes found!\n");
                }
            }

            if (changesMade)
            {
                AddToProgressText("\nFetching all tables to repopulate the list...\n");
                PopulateTables();
                AddToProgressText("Done!\n");
            }
        }


        private void lvTables_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnStart.Enabled = false;
            bool syncable = true;
            int syncableCount = 0;

            foreach (ListViewItem item in lvTables.Items)
            {
                LocalTableItem localItem = (LocalTableItem)item.Tag;

                if (localItem.Syncable)
                {
                    syncableCount++;
                }
                else
                {
                    if (item.Checked)
                    {
                        syncable = false;
                    }
                }
            }

            cbSelectAll.CheckedChanged -= cbSelectAll_CheckedChanged;
            cbSelectAll.Checked = lvTables.CheckedItems.Count == syncableCount;
            cbSelectAll.CheckedChanged += cbSelectAll_CheckedChanged;

            btnStart.Enabled = syncable && lvTables.CheckedItems.Count > 0;
        }

        private void DataModelImportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing &&
                btnClose.Enabled == false)
                e.Cancel = true;
        }


        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            lvTables.ItemChecked -= lvTables_ItemChecked;

            foreach (ListViewItem item in lvTables.Items)
            {
                if (((LocalTableItem)item.Tag).Syncable)
                {
                    item.Checked = cbSelectAll.Checked;
                }
            }

            lvTables.ItemChecked += lvTables_ItemChecked;

            EnableDisableButtons();
        }


        public class LocalTableItem
        {
            public DataModelTableInfo RemoteTable { get; set; }
            public BusinessEntity LocalBusinessEntity { get; set; }
            public bool Syncable { get; set; }
        }

    }

}
