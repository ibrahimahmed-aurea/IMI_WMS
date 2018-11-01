using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.Framework.Parsing.PLSQLPackageSpecification;
using System.IO;
using Cdc.Framework.Parsing.OracleSQLParsing;
using Spring.Data.NHibernate.Support;
using NHibernate;
using System.Configuration;
using Cdc.MetaManager.BusinessLogic.Helpers;



namespace Cdc.MetaManager.GUI
{    

    public partial class StoredProcedureControl : UserControl
    {

        public event EventHandler ChangedSP;

        private IDialogService dialogService = null;

        private IModelService modelService = null;

        private Schema mySchema = null;

        public PLSQLSpec lastReadPLSQLSpec = null;
        Package existingPackage = null;

        public string OverrideParseFileName { private get; set; }

        public StoredProcedure SelectStoredProcedure { get; set; }
        public PLSQLProcedure UpdatedStoreProcedure = null;

        public List<ProcedureProperty> procedurePropertiesToDelete = new List<ProcedureProperty>();
        public Package packageToAdd = null;

        private StoredProcedure myStoredProcedure = null;

        public MetaManager.DataAccess.Domain.Application BackendApplication { get; set; }
        public bool createNewProcedure = false;
        private bool isEditable = false;
        private bool procedureMissingInFileAndNoValidSelectionMade = true;

        public string FileName(){
            return OverrideParseFileName;
        }

        public StoredProcedure ThisStoredProcedure
        {
            get
            {
                if (myStoredProcedure.Id == Guid.Empty)
                {
                    if (lvFoundProcedures.SelectedItems.Count == 1)
                    {
                        myStoredProcedure = GetStoredProcedureFromPLSQLProcedure(((ImportStoredProcedureWrapperClass)lvFoundProcedures.SelectedItems[0].Tag).StoredProcedure, tbPackageName.Text);
                        GetExistingPackage(tbPackageName.Text);
                        if (existingPackage == null)
                        {
                            packageToAdd = new Package();
                            packageToAdd.Schema = mySchema;
                            packageToAdd.Name = tbPackageName.Text;
                            packageToAdd.Filename = lastReadPLSQLSpec.FileNameParsed;
                            packageToAdd.Hash = lastReadPLSQLSpec.PackageHash;
                            packageToAdd.Size = lastReadPLSQLSpec.PackageLength;
                        }
                        else
                        {
                            myStoredProcedure.Package = existingPackage;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                return myStoredProcedure;
            }
        }




        public StoredProcedureControl()
        {
            InitializeComponent();

        }

        new public void Load(StoredProcedure storedProcedure, bool isEditable, Schema schema)
        {

            // Get application service context
            dialogService = MetaManagerServices.GetDialogService();

            modelService = MetaManagerServices.GetModelService();

            myStoredProcedure = storedProcedure;

            this.isEditable = isEditable;

            if (myStoredProcedure.Id != Guid.Empty)
            {
                Package package = modelService.GetInitializedDomainObject<Package>(myStoredProcedure.Package.Id);
                OverrideParseFileName = Path.Combine(ConfigurationManager.AppSettings[BackendApplication.Name + "PLSQLSpecDirectory"], Path.GetFileName(package.Filename));
            }
            else
            {
                OverrideParseFileName = string.Empty;
            }


            mySchema = modelService.GetInitializedDomainObject<DataAccess.Domain.Schema>(schema.Id);


            // Get last file imported from.
            if (!string.IsNullOrEmpty(OverrideParseFileName))
            {
                tbParseFileName.Text = OverrideParseFileName;
            }
            else
            {
                tbParseFileName.Text = Config.Backend.LastImportPackageSpec;
            }

            if (!string.IsNullOrEmpty(tbParseFileName.Text))
            {
                DoParseFile(tbParseFileName.Text);
            }


            EnableDisableButtons();
            EnableDisableFields();
        }

        private void EnableDisableFields()
        {
            tbParseFileName.ReadOnly = !this.isEditable;

            lvFoundProcedures.Enabled = false;
            lvFoundProcedures.Enabled = true;
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbParseFileName.Text))
            {
                fileBrowseDialog.FileName = tbParseFileName.Text;
            }
            else
            {
                fileBrowseDialog.InitialDirectory = ConfigurationManager.AppSettings[BackendApplication.Name +"PLSQLSpecDirectory"];
            }

            if (fileBrowseDialog.ShowDialog() == DialogResult.OK)
            {
                tbParseFileName.Text = fileBrowseDialog.FileName;
                DoParseFile(tbParseFileName.Text);
            }
        }

        private void DoParseFile(string filename)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show("The file to parse doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Get the package specification
                lastReadPLSQLSpec = PLSQLSupportedSpec.ParseSpecFile(filename);

                // Check that the file could be parsed at all.
                if (lastReadPLSQLSpec == null)
                {
                    MessageBox.Show("The selected file is not a valid PLSQL Package Specification File.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Save to config as last imported file if not overriden
                    if (!string.IsNullOrEmpty(tbParseFileName.Text))
                    {
                        Config.Backend.LastImportPackageSpec = filename;
                        Config.Save();
                    }

                    PopulateParsedFileProcedures(lastReadPLSQLSpec);


                }
            }
        }

        private void PopulateParsedFileProcedures(PLSQLSpec plSqlSpec)
        {
            if (plSqlSpec != null)
            {
                lvFoundProcedures.Items.Clear();

                // Set packagename
                tbPackageName.Text = plSqlSpec.PackageName;

                // Fetch the existing package if there is one.
                GetExistingPackage(plSqlSpec.PackageName);

                foreach (PLSQLProcedure procedure in plSqlSpec.Procedures.OrderBy(p => p.Name))
                {
                    ListViewItem item = null;

                    if (NewProcedureItem(ref item, procedure, false))
                    {
                        if (myStoredProcedure.Id != Guid.Empty)
                        {
                            if (myStoredProcedure.ProcedureName == procedure.Name)
                            {
                                lvFoundProcedures.Items.Add(item);
                                item.Selected = true;
                                item.EnsureVisible();
                                procedureMissingInFileAndNoValidSelectionMade = false;
                            }
                            else if (!((ImportStoredProcedureWrapperClass)item.Tag).IsImported)
                            {
                                lvFoundProcedures.Items.Add(item);
                            }

                        }
                        else
                        {
                            if (!((ImportStoredProcedureWrapperClass)item.Tag).IsImported)
                            {
                                lvFoundProcedures.Items.Add(item);
                            }

                        }
                    }
                }
            }

            EnableDisableButtons();
        }

        private bool NewProcedureItem(ref ListViewItem item, PLSQLProcedure procedure, bool reloadExisting)
        {
            if (item == null)
            {
                item = new ListViewItem();
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
            }

            return UpdateProcedureItem(item, procedure, reloadExisting);
        }

        private bool UpdateProcedureItem(ListViewItem item, PLSQLProcedure procedure, bool reloadExisting)
        {
            if (reloadExisting || existingPackage == null)
                GetExistingPackage(lastReadPLSQLSpec.PackageName);

            StoredProcedure storedProc = StoredProcedureHelper.CreateStoredProcedure(procedure);

            // Check if procedure is imported already
            bool isImported = (existingPackage != null &&
                               storedProc != null &&
                               existingPackage.Procedures.Where(p => p.Equals(storedProc)).Count() == 1);

            bool packageHasProcedureNameImported = (existingPackage != null &&
                                                    storedProc != null &&
                                                    existingPackage.Procedures.Where(p => p.ProcedureName == storedProc.ProcedureName).Count() > 0);


            ImportStoredProcedureWrapperClass wrapper = new ImportStoredProcedureWrapperClass();
            wrapper.StoredProcedure = procedure;
            wrapper.IsImported = isImported;

            item.Text = procedure.Name;
            item.SubItems[1].Text = procedure.Status.ToString();

            if (isImported)
            {
                item.SubItems[2].Text = "Imported";
            }
            else
            {
                if (packageHasProcedureNameImported)
                    item.SubItems[2].Text = "Overloaded/Updated";
                else
                    item.SubItems[2].Text = "Not Imported";
            }

            item.Tag = wrapper;

            if (procedure.Status != ProcedureStatus.Valid)
            {
                item.ForeColor = Color.OrangeRed;
            }
            else if (isImported)
            {
                item.ForeColor = Color.Green;
            }

            return true;

        }

        private void GetExistingPackage(string packageName)
        {
            Dictionary<string, object> searchValues = new Dictionary<string, object>();
            searchValues.Add("Name", packageName);
            searchValues.Add("Schema.Id", mySchema.Id);
            IList<Package> existingPackages = modelService.GetAllDomainObjectsByPropertyValues<Package>(searchValues);

            if (existingPackages.Count > 0)
            {
                List<string> initprops = new List<string>();
                initprops.Add("Procedures.Properties");
                existingPackage = modelService.GetDynamicInitializedDomainObject<Package>(existingPackages[0].Id, initprops);
            }
            else
            {
                existingPackage = null;
            }
        }

        private void EnableDisableButtons()
        {

            btnBrowse.Enabled = this.isEditable;

            if (lvFoundProcedures.SelectedItems.Count == 1)
            {
                ImportStoredProcedureWrapperClass wrapper = (ImportStoredProcedureWrapperClass)lvFoundProcedures.SelectedItems[0].Tag;

                if (wrapper.StoredProcedure.Status == ProcedureStatus.Valid)
                {

                    // Check if any imported procedures already exist with the name
                    if (existingPackage == null)
                    {
                        GetExistingPackage(lastReadPLSQLSpec.PackageName);
                    }

                }

            }
        }


        private void cbOnlyNotImported_CheckedChanged(object sender, EventArgs e)
        {
            // Repopulate the list again
            PopulateParsedFileProcedures(lastReadPLSQLSpec);
        }

        public void UpdateExistingProcedure()
        {
            if (lvFoundProcedures.SelectedItems.Count == 1 && myStoredProcedure.Id != Guid.Empty)
            {
                // Get selected procedure
                PLSQLProcedure selectedProcedure = ((ImportStoredProcedureWrapperClass)lvFoundProcedures.SelectedItems[0].Tag).StoredProcedure;

                if (myStoredProcedure.ProcedureName == selectedProcedure.Name)
                {

                    if (existingPackage == null)
                    {
                        GetExistingPackage(lastReadPLSQLSpec.PackageName);
                    }

                    if (UpdateStoredProcedure(selectedProcedure, existingPackage, myStoredProcedure.Id))
                    {
                    }
                    else
                    {
                        procedurePropertiesToDelete.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("You can only update the stored procedure currently connected to the Action");
                }
            }
        }


        private StoredProcedure GetStoredProcedureFromPLSQLProcedure(PLSQLProcedure procedure, string packageName)
        {
            StoredProcedure storedProc = null;

            if (procedure != null)
            {
                // Check if there are any ref cursors in the procedure parameters                
                if (procedure.IsContainingRefCursor && !procedure.IsFunction)
                {
                    // Create ref cursor stored procedure                    
                    RefCurStoredProcedure proc = RefCurStoredProcedure.Create(procedure.Name, packageName, mySchema.ConnectionString);

                    // Check if procedure is valid                    
                    if (proc.Status == RefCurStoredProcedureStatus.Valid)
                    {
                        // Convert the RefCurStoredProcedure to a StoredProcedure
                        storedProc = StoredProcedureHelper.CreateStoredProcedure(procedure, proc);
                    }
                    else
                    {
                        string errorText = string.Format("Error when trying to import procedure {0} that has a Ref Cursor parameter!" + Environment.NewLine + Environment.NewLine, proc.Name) +
                                                   string.Format("Status: {0}", proc.Status.ToString()) + Environment.NewLine + Environment.NewLine +
                                                   proc.ErrorText;

                        MessageBox.Show(errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else
                {
                    // Convert the PLSQLProcedure to a StoredProcedure                 
                    storedProc = StoredProcedureHelper.CreateStoredProcedure(procedure);
                }
            }

            return storedProc;
        }

        public bool UpdateStoredProcedure(PLSQLProcedure selectedProcedure, Package package, Guid spId)
        {
            // Check if there is more than one stored procedure which we can update and show them all
            // together with the selected procedure to update with.
            StoredProcedure procedureToUpdate = null;

            IList<StoredProcedure> procList; 

            procList = (from StoredProcedure sp in package.Procedures
                        where sp.Id == spId
                        select sp).ToList();

            if (procList != null && procList.Count == 1)
            {
                // Only one in list, then update that one.
                procedureToUpdate = procList[0];
            }
            else
            {
                MessageBox.Show("The Stored Procedure could not be synchronized.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (procedureToUpdate != null)
            {
                StoredProcedure readProc = null;

                readProc = GetStoredProcedureFromPLSQLProcedure(selectedProcedure, package.Name);

                // Check that IsReturningRefCursor flag hasn't changed or we can't support this.
                if ((procedureToUpdate.IsReturningRefCursor ?? false) != (readProc.IsReturningRefCursor ?? false))
                {
                    if (procedureToUpdate.IsReturningRefCursor ?? false)
                        MessageBox.Show("It's not possible to update a procedure when it has changed from beeing\n" +
                                        "a procedure with a ref cursor to not having a ref cursor as a parameter.\n\n" +
                                        "You have to delete the initial stored procedure and import it again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("It's not possible to update a procedure when it has changed from not having\n" +
                                        "a ref cursor to now having a ref cursor as a parameter.\n\n" +
                                        "You have to delete the initial stored procedure and import it again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }

                procedureToUpdate.ProcedureName = selectedProcedure.Name;
                // Update name of ref cursor parameter
                procedureToUpdate.RefCursorParameterName = readProc.RefCursorParameterName;

                // Syncronize the properties
                if (!SyncProperties(procedureToUpdate, readProc))
                {
                    MessageBox.Show("Failed to synchronize properties!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }



            if (myStoredProcedure.Id == procedureToUpdate.Id)
            {

                myStoredProcedure = procedureToUpdate;
            }

            return true;
        }

        private bool SyncProperties(StoredProcedure dbStoredProc, StoredProcedure newProc)
        {
            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
            {
                procedurePropertiesToDelete.Clear();
                List<ProcedureProperty> addList = new List<ProcedureProperty>();
                List<ProcedureProperty> changeList = new List<ProcedureProperty>();
                List<ProcedureProperty> deleteList = new List<ProcedureProperty>(dbStoredProc.Properties);

                // Compare new query properties to old ones
                // determine cause of action, i.e. Add new Properties, Delete removed Properties
                // or update data type
                foreach (ProcedureProperty newPp in newProc.Properties)
                {
                    ProcedureProperty original = FindPropertyByName(newPp.Name, newPp.PropertyType, deleteList);

                    if (original != null)
                    {
                        if (!ProcedureProperty.IsEqual(newPp, original))
                        {
                            // Datatype changed
                            changeList.Add(newPp);
                        }
                        else
                        {
                            // Set sequence
                            original.Sequence = newPp.Sequence;
                        }

                        // Mark as treated
                        deleteList.Remove(original);
                    }
                    else
                    {
                        // Totally new
                        addList.Add(newPp);
                    }
                }

                // What is left in deleteList should be removed
                if (deleteList.Count > 0)
                {
                    Dictionary<string, object> searchKey = new Dictionary<string, object>();
                    searchKey.Add("ProcedureProperty.Id", deleteList[0].Id);
                    IList<MappedProperty> mappedProperties = modelService.GetAllDomainObjectsByPropertyValues<MappedProperty>(searchKey);
                    
                    foreach (ProcedureProperty deleteProp in deleteList)
                    {
                        procedurePropertiesToDelete.Add(deleteProp);
                        dbStoredProc.Properties.Remove(deleteProp);
                    }
                }

                if (addList.Count > 0)
                {
                    foreach (ProcedureProperty addProp in addList)
                    {
                        addProp.StoredProcedure = dbStoredProc;
                        dbStoredProc.Properties.Add(addProp);
                    }
                }

                if (changeList.Count > 0)
                {
                    foreach (ProcedureProperty changeProp in changeList)
                    {
                        ProcedureProperty dbPp = FindPropertyByName(changeProp.Name, changeProp.PropertyType, dbStoredProc.Properties);

                        if (dbPp != null)
                        {
                            dbPp.DbDatatype = changeProp.DbDatatype;
                            dbPp.Length = changeProp.Length;
                            dbPp.Precision = changeProp.Precision;
                            dbPp.Scale = changeProp.Scale;
                            dbPp.OriginalColumn = changeProp.OriginalColumn;
                            dbPp.OriginalTable = changeProp.OriginalTable;
                            dbPp.Text = changeProp.Text;
                            dbPp.Sequence = changeProp.Sequence;
                        }
                    }
                }

                return true;
            }
        }
        
        public bool ProcedureMissingInFileAndNoValidSelectonMade()
        {
            return procedureMissingInFileAndNoValidSelectionMade;
        }
        
        private static ProcedureProperty FindPropertyByName(string propertyName, DbPropertyType direction, IList<ProcedureProperty> propertyList)
        {
            IEnumerable<ProcedureProperty> foundProperties =
                from ProcedureProperty property in propertyList
                where property.Name == propertyName &&
                      property.PropertyType == direction
                select property;

            if (foundProperties.Count() > 0)
                return foundProperties.First();
            else
                return null;
        }

        private void lvFoundProcedures_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvFoundProcedures.SelectedItems.Count == 1)
            {

                int index = e.ItemIndex;
                ListViewItem selectedItem = e.Item;

                PLSQLProcedure procedure = ((ImportStoredProcedureWrapperClass)e.Item.Tag).StoredProcedure;

                tbStoredProcText.Text = procedure.ToString();

                tbStoredProcText.WordWrap = (procedure.Status != ProcedureStatus.Valid);

                if (procedure.Status.Equals(ProcedureStatus.NotSupported) || procedure.IsFunction)
                {
                    procedureMissingInFileAndNoValidSelectionMade = true;
                }
                else
                {
                    procedureMissingInFileAndNoValidSelectionMade = false;

                    if (!procedure.Name.Equals(myStoredProcedure.ProcedureName))
                    {
                        myStoredProcedure.ProcedureName = procedure.Name;
                    }
                }


                
                ChangedSP(sender, e);
                
            }

            EnableDisableButtons();
        }


    }

    public class StoredProcedureActionWrapperClass
    {
        public StoredProcedure StoredProcedure { get; set; }
        public Cdc.MetaManager.DataAccess.Domain.Action Action { get; set; }
    }

    public class ImportStoredProcedureWrapperClass
    {
        public PLSQLProcedure StoredProcedure { get; set; }
        public bool IsImported { get; set; }
    }

}
