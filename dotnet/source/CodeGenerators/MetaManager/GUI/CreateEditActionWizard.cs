using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Spring.Context;
using Spring.Context.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Cdc.Framework.Parsing.OracleSQLParsing;
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess;
using Cdc.Framework.Parsing.PLSQLPackageSpecification;
using NHibernate;
using System.Configuration;


namespace Cdc.MetaManager.GUI
{
    

    public partial class CreateEditActionWizard : MdiChildForm
    {

        private Schema schema = null;

        private IApplicationService applicationService = null;
        private IDialogService dialogService = null;
        private IModelService modelService = null;
        private List<QueryProperty> queryPropertyToDelete = new List<QueryProperty>();

        private bool newAction = true;


        public DataAccess.Domain.Action Action { get; set; }
        public BusinessEntity BusinessEntity { get; set; }
        public ServiceMethod ServiceMethodOwner { get; set; }

        public CreateEditActionWizard()
        {
            InitializeComponent();

            applicationService = MetaManagerServices.GetApplicationService();
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();


            storedProcedureControl.ChangedSP += new EventHandler(NewSpSelected);



            targetProcedureBtn.Checked = true;
            targetProcedureBtn_Click(this, null);
        }


        private void AddActionWizard_Load(object sender, EventArgs e)
        {
            if (ContaindDomainObjectIdAndType.Key != Guid.Empty)
            {
                newAction = false;
                Action = modelService.GetInitializedDomainObject<Cdc.MetaManager.DataAccess.Domain.Action>(ContaindDomainObjectIdAndType.Key);
                this.IsEditable = Action.IsLocked && Action.LockedBy == Environment.UserName;
                BusinessEntity = Action.BusinessEntity;
            }
            else
            {
                Action = new DataAccess.Domain.Action();
                newAction = true;
                this.IsEditable = true;
            }

            tabControl.TabPages.Clear();
            tabControl.TabPages.Add(targetPage);

            tabControl.SelectedTab = targetPage;

            schema = modelService.GetAllDomainObjectsByApplicationId<Schema>(BackendApplication.Id)[0];

            if (newAction)
            {
                this.Text = "Add Action";
            }
            else
            {
                this.Text = "Edit Action";
                if (this.IsEditable)
                {

                    finishBtn.Text = "Save";
                }
                else
                {
                    finishBtn.Text = "Close";
                }
            }

            EnableDisableFields(this.IsEditable);
            SwitchPage(targetPage);
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            nextBtn.Enabled = false;
            TabPage nextPage = null;

            if (!AddInfoToAction(tabControl.SelectedTab)) { return; }

            if (tabControl.SelectedTab == targetPage)
            {
                if (targetProcedureBtn.Checked)
                {
                    nextPage = procedurePage;
                }
                else
                {
                    nextPage = queryPage;
                }
            }
            else if (tabControl.SelectedTab == queryPage || tabControl.SelectedTab == procedurePage)
            {
                nextPage = paramPage;
            }
            else if (tabControl.SelectedTab == paramPage)
            {
                
                nextPage = advancedPage;
            }

            SwitchPage(nextPage);
            EnableDisableBtn();
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            prevBtn.Enabled = false;
            TabPage nextPage = null;

            if (tabControl.SelectedTab == advancedPage)
            {
                nextPage = paramPage;
            }
            else if (tabControl.SelectedTab == paramPage)
            {
                if (targetQueryBtn.Checked)
                {
                    nextPage = queryPage;
                }
                else
                {
                    nextPage = procedurePage;
                }
            }
            else if (tabControl.SelectedTab == queryPage || tabControl.SelectedTab == procedurePage)
            {
                nextPage = targetPage;
            }

            SwitchPage(nextPage);
            EnableDisableBtn();
        }

        private void SwitchPage(TabPage nextPage)
        {
            lock (tabControl)
            {
                if (nextPage != null)
                {
                    tabControl.TabPages.Clear();
                    tabControl.TabPages.Add(nextPage);
                    tabControl.SelectedTab = nextPage;

                    if (nextPage == targetPage)
                    {
                        actionNameTb.Focus();
                    }

                    if (nextPage == advancedPage)
                    {
                        RowTrackingIdTbx.Focus();
                    }
                }
            }
        }

        private bool AddInfoToAction(TabPage activePage)
        {

            if (activePage == targetPage)
            {
                return AddActionInfo();
            }
            else if (activePage == queryPage)
            {
                return AddQueryInfo();
            }
            else if (activePage == procedurePage)
            {
                return AddProcedureInfo();
            }
            else if (activePage == paramPage)
            {
                return AddMappingInfo();
            }
            else if (activePage == advancedPage)
            {
                AddAdvancedPropertiesInfo();
            }
            
            return true;
        }

        #region Add info functions

        private bool AddActionInfo()
        {
            if (targetProcedureBtn.Checked)
            {
                if (!NamingGuidance.CheckActionNameStoredProcedure(actionNameTb.Text.Trim(), true))
                    return false;
            }
            else
            {
                if (!NamingGuidance.CheckActionNameQuery(actionNameTb.Text.Trim(), true))
                    return false; ;
            }

            if (Action.Name != actionNameTb.Text.Trim() || newAction)
            {
                // Get all names for the actions in this businessentity
                BusinessEntity selectedBE = entityCbx.SelectedItem as BusinessEntity;

                IList<DataAccess.Domain.Action> foundActions = modelService.GetVersionControlledDomainObjectsForParent(typeof(DataAccess.Domain.Action), selectedBE.Id).Cast<DataAccess.Domain.Action>().ToList();
                IEnumerable<string> actionNames = null;

                if (foundActions != null && foundActions.Count > 0)
                {
                    // Make list to a list of strings.
                    actionNames = foundActions.Select<Cdc.MetaManager.DataAccess.Domain.Action, string>(action => action.Name);
                }

                // Check if name already exist
                if (!NamingGuidance.CheckNameNotInList(actionNameTb.Text.Trim(), "Action Name", "list of Action Names for BusinessEntities", actionNames, false, true))
                    return false;

                // Update Action name
                Action.Name = actionNameTb.Text.Trim();
                Action.BusinessEntity = selectedBE;
            }
            return true;
        }

        private bool AddQueryInfo()
        {
            bool propertiesAdded = true;

            queryControl.CompileQuery();

            if (queryControl.IsQueryCompiled)
            {
                if (!NamingGuidance.CheckQueryName(queryControl.QueryName, true))
                {
                    return false;
                }

                if (Action.Query.Id != Guid.Empty)
                {
                    using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
                    {
                        // Get a fresh copy from database (original)
                        Query dbQuery = modelService.GetInitializedDomainObject<Query>(Action.Query.Id);

                        // Syncronize the new one with the one in the database
                        //if (!MetaManagerServices.Helpers.QueryHelper.SyncProperties(Action.Query, dbQuery, HandleDeletedQueryProperties, out propertiesAdded))
                        if (!MetaManagerServices.Helpers.QueryHelper.SyncProperties(queryControl.Query, dbQuery, HandleDeletedQueryProperties, out propertiesAdded))
                        {
                            return false;
                        }

                        foreach (QueryProperty qp in queryPropertyToDelete)
                        {
                            dbQuery.Properties.Remove(qp);
                        }

                        Action.Query = dbQuery;
                    }
                }

                Action.Query.Name = queryControl.QueryName.Trim();
                Action.Query.Schema = schema;

                return true;
            }
            else
            {
                MessageBox.Show("You must compile the query first.");
                return false;
            }
        }

        private bool AddProcedureInfo()
        {
            StoredProcedure sprocedure = null;

            storedProcedureControl.UpdateExistingProcedure();

            sprocedure = storedProcedureControl.ThisStoredProcedure;

            if (sprocedure != null)
            {
                Action.Query = null;
                Action.StoredProcedure = sprocedure;
            }
            else
            {
                MessageBox.Show("You have to select a procedure before you can continue.");
                return false;
            }

            return true;
        }

        private bool AddMappingInfo()
        {
            try
            {
                CheckMappedProperties(Action.RequestMap);
                CheckMappedProperties(Action.ResponseMap);

                IMappableObject target = null;

                if (targetProcedureBtn.Checked)
                {
                    target = Action.StoredProcedure as IMappableObject;

                    if ((target.ObjectType == ActionMapTarget.StoredProcedure) && (((StoredProcedure)target).IsReturningRefCursor ?? false))
                    {
                        Action.ResponseMap.IsCollection = true;
                    }
                    else
                    {
                        Action.ResponseMap.IsCollection = false;
                    }
                }
                else
                {
                    target = Action.Query;
                    Action.ResponseMap.IsCollection = true;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating action: " + ex);
                return false;
            }
        }

        private void AddAdvancedPropertiesInfo()
        {
            Action.RowTrackingId = RowTrackingIdTbx.Text;
            Action.IsMultiEnabled = MultiSelectCb.Checked;
            Action.IsMessageHandlingEnabled = MessageHandlingCb.Checked;
            Action.IsRefCursorCommit = CommitTransactionCb.Checked;
        }

        #endregion

        private void finishBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (!IsEditable)
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    AddAdvancedPropertiesInfo();

                    //If there is a new Package
                    if (Action.StoredProcedure != null && storedProcedureControl.packageToAdd != null)
                    {
                        Action.StoredProcedure.Package = (Package)modelService.SaveDomainObject(storedProcedureControl.packageToAdd);
                    }

                    try
                    {
                        Action = MetaManagerServices.Helpers.ActionHelper.SaveAndSynchronize(Action, propertyMapControl.MappedPropertiesToDelete);

                        if (newAction)
                        {
                            CheckOutInObject(Action, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        string mess = ex.Message;

                        if (ex is ModelAggregatedException)
                        {
                            string ids = string.Empty;
                            foreach (string id in ((ModelAggregatedException)ex).Ids)
                            {
                                ids += id + "\r\n";
                            }

                            Clipboard.SetText(ids);
                            mess += "\r\n\r\nThe Ids has been copied to the clipboard";
                        }

                        MessageBox.Show(mess);

                        return;
                    }

                    ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(Action.Id, typeof(DataAccess.Domain.Action));

                    if (newAction && ServiceMethodOwner == null && cbNewServiceMethod.Checked)
                    {
                        CreateEditServiceMethod form = new CreateEditServiceMethod();
                        form.BackendApplication = BackendApplication;
                        form.FrontendApplication = FrontendApplication;
                        form.ActionOwner = Action;
                        form.Service = (Service)cbCreateInService.SelectedItem;
                        form.ShowDialog();
                    }

                    DialogResult = DialogResult.OK;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        #region UI Events

        private void NewSpSelected(object sender, EventArgs e)
        {
            EnableDisableBtn();
        }

        private void entityCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableDisableBtn();
        }

        private void actionNameTb_TextChanged(object sender, EventArgs e)
        {
            EnableDisableBtn();
        }

        private void targetProcedureBtn_Click_1(object sender, EventArgs e)
        {
            SetFormatText();
        }

        private void targetQueryBtn_Click_1(object sender, EventArgs e)
        {
            SetFormatText();
        }

        private void targetProcedureBtn_Click(object sender, EventArgs e)
        {
            queryPage.Tag = 0;
            procedurePage.Tag = null;

        }

        private void targetQueryBtn_Click(object sender, EventArgs e)
        {
            queryPage.Tag = null;
            procedurePage.Tag = 0;

        }

        #endregion



        #region Tab page activation functions
        private void targetPage_ParentChanged(object sender, EventArgs e)
        {

            if (targetPage.Parent == tabControl)
            {
                if (targetPage.Tag as string != "Loaded")
                {
                    if (entityCbx.Items.Count == 0)
                    {
                        entityCbx.Items.AddRange(modelService.GetAllDomainObjectsByApplicationId<BusinessEntity>(BackendApplication.Id).OrderBy(be => be.Name).ToArray());

                        // Find the last selected business entity
                        if (BusinessEntity != null)
                        {
                            foreach (BusinessEntity item in entityCbx.Items.Cast<BusinessEntity>())
                            {
                                if (item.Id == BusinessEntity.Id)
                                {
                                    entityCbx.SelectedItem = item;
                                }
                            }

                            if (entityCbx.SelectedItem != null)
                            {
                                entityCbx.Enabled = false;
                            }
                        }
                    }

                    if (newAction)
                    {
                        if (ServiceMethodOwner != null)
                        {
                            actionNameTb.Text = ServiceMethodOwner.Name;
                        }
                    }
                    else
                    {
                        if (Action.StoredProcedure != null)
                        {
                            targetProcedureBtn.Checked = true;
                        }
                        else
                        {
                            targetQueryBtn.Checked = true;
                        }

                        actionNameTb.Text = Action.Name;
                    }

                    TypeDescriptor.AddAttributes(Action, new Attribute[] { new ReadOnlyAttribute(true) });
                    ObjectPropertyGrid.SelectedObject = Action;

                    targetPage.Tag = "Loaded";
                }
                SetFormatText();
                EnableDisableBtn();
            }

        }

        private void queryPage_ParentChanged(object sender, EventArgs e)
        {

            if (queryPage.Parent == tabControl)
            {
                if (queryPage.Tag as string != "Loaded")
                {
                    if (newAction)
                    {
                        Action.StoredProcedure = null;
                        Action.Query = new Query();
                        queryControl.Load(true, actionNameTb.Text, schema, Action.Query, true);
                    }
                    else
                    {
                        Action.Query = modelService.GetInitializedDomainObject<Query>(Action.Query.Id);
                        queryControl.Load(false, Action.Query.Name, schema, Action.Query, this.IsEditable);
                        if (IsEditable)
                        {
                            queryControl.CompileQuery();
                        }
                    }

                    queryPage.Tag = "Loaded";
                }
                EnableDisableBtn();
            }

        }

        private void procedurePage_ParentChanged(object sender, EventArgs e)
        {

            if (procedurePage.Parent == tabControl)
            {
                if (procedurePage.Tag as string != "Loaded")
                {
                    if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[BackendApplication.Name + "PLSQLSpecDirectory"]))
                    {
                        using (SelectPLSQLSpecDir form = new SelectPLSQLSpecDir())
                        {
                            form.BackendApplication = BackendApplication;
                            form.FrontendApplication = FrontendApplication;

                            if (form.ShowDialog() == DialogResult.Cancel)
                            {
                                // When cancelling dialog then don't continue
                                SwitchPage(targetPage);
                            }
                        }
                    }

                    if (newAction)
                    {
                        Action.Query = null;
                        Action.StoredProcedure = new StoredProcedure();
                    }
                    else
                    {
                        Action.StoredProcedure = modelService.GetInitializedDomainObject<StoredProcedure>(Action.StoredProcedure.Id);
                    }

                    storedProcedureControl.BackendApplication = BackendApplication;
                    storedProcedureControl.createNewProcedure = newAction;
                    storedProcedureControl.Load(Action.StoredProcedure, this.IsEditable, schema);

                    if (IsEditable && !newAction)
                    {
                        storedProcedureControl.UpdateExistingProcedure();
                    }

                    procedurePage.Tag = "Loaded";
                }
                EnableDisableBtn();
                ShowMissingSPMessage();
            }

        }

        private void paramPage_ParentChanged(object sender, EventArgs e)
        {

            if (paramPage.Parent == tabControl)
            {
                if (paramPage.Tag as string != "Loaded")
                {
                    if (newAction)
                    {
                        Action.RequestMap = new PropertyMap();
                        Action.ResponseMap = new PropertyMap();
                    }
                    else
                    {
                        List<string> initprop = new List<string>();
                        initprop.Add("MappedProperties.Source");
                        initprop.Add("MappedProperties.Target");
                    
                        Action.RequestMap = modelService.GetDynamicInitializedDomainObject<PropertyMap>(Action.RequestMap.Id, initprop);
                        Action.ResponseMap = modelService.GetDynamicInitializedDomainObject<PropertyMap>(Action.ResponseMap.Id, initprop);
                    }


                    propertyMapControl.IsEditable = this.IsEditable;
                    propertyMapControl.RequestMap = Action.RequestMap;
                    propertyMapControl.ResponseMap = Action.ResponseMap;
                    propertyMapControl.BackendApplication = BackendApplication;
                    if (Action.BusinessEntity != null)
                    {
                        propertyMapControl.BusinessEntity = modelService.GetInitializedDomainObject<BusinessEntity>(Action.BusinessEntity.Id);
                    }

                    paramPage.Tag = "Loaded";
                }

                List<IMappableProperty> properties = new List<IMappableProperty>();
                if (targetProcedureBtn.Checked)
                {
                    if (Action.StoredProcedure != null)
                    {
                        properties = Action.StoredProcedure.Properties.Cast<IMappableProperty>().ToList();
                    }
                }
                else
                {
                    if (Action.Query != null)
                    {
                        properties = Action.Query.Properties.Cast<IMappableProperty>().ToList();
                    }
                }

                propertyMapControl.MappableProperties = properties;
                propertyMapControl.Map();



                EnableDisableBtn();
            }

        }

        private void advancedPage_ParentChanged(object sender, EventArgs e)
        {

            if (advancedPage.Parent == tabControl)
            {
                if (advancedPage.Tag as string != "Loaded")
                {
                    CommitTransactionCb.Enabled = false;
                    MultiSelectCb.Enabled = false;
                    MessageHandlingCb.Enabled = false;

                    if (targetProcedureBtn.Checked)
                    {
                        //Check if Ref Cursor
                        if ((Action.StoredProcedure != null) && (Action.StoredProcedure.IsReturningRefCursor ?? false))
                        {
                            CommitTransactionCb.Enabled = this.IsEditable;
                        }
                        else // This is not Ref Cursor
                        {
                            MultiSelectCb.Enabled = this.IsEditable;
                            MessageHandlingCb.Enabled = this.IsEditable;
                        }
                    }

                    RowTrackingIdTbx.Text = string.IsNullOrEmpty(Action.RowTrackingId) ? string.Empty : Action.RowTrackingId;

                    Action.RowTrackingId = RowTrackingIdTbx.Text;
                    MultiSelectCb.Checked = (Action.IsMultiEnabled ?? true);
                    MessageHandlingCb.Checked = (Action.IsMessageHandlingEnabled ?? true);
                    CommitTransactionCb.Checked = (Action.IsRefCursorCommit ?? true);


                    IList<DataAccess.Domain.Service> services = modelService.GetAllDomainObjectsByApplicationId<DataAccess.Domain.Service>(BackendApplication.Id);
                    services = services.OrderBy(service => service.Name).ToList();
                    cbCreateInService.DisplayMember = "Name";
                    cbCreateInService.DataSource = services;

                    if (!NHibernate.NHibernateUtil.IsInitialized(BusinessEntity))
                    {
                        BusinessEntity = modelService.GetInitializedDomainObject<BusinessEntity>(BusinessEntity.Id);
                    }

                    IEnumerable<Service> foundServices = services.Where(s => s.Name == BusinessEntity.Name);

                    Service selectService = null;

                    if (foundServices.Count() > 0)
                    {
                        selectService = foundServices.First();
                    }

                    if (selectService != null)
                    {
                        cbCreateInService.SelectedIndex = cbCreateInService.Items.IndexOf(selectService);
                    }
                    else
                    {
                        cbCreateInService.SelectedIndex = 0;
                    }

                    if (newAction && ServiceMethodOwner == null)
                    {
                        ServiceMethodgroupBox.Visible = true;
                    }
                    else
                    {
                        ServiceMethodgroupBox.Visible = false;
                    }

                    advancedPage.Tag = "Loaded";
                }
                EnableDisableBtn();
                
            }

        }

        #endregion


        #region Helper functions

        private void SetFormatText()
        {
            // Set format texts depending on if procedure or query is selected
            if (targetProcedureBtn.Checked)
            {
                tbFormatText.Text = string.Format("Must start with a verb.{0}May NOT start with: \"New\", \"Change\", \"Remove\" (use \"Create\", \"Modify\", \"Delete\" instead).{0}May NOT end with: \"Action\", \"Qry\"", Environment.NewLine);
            }
            else
            {
                tbFormatText.Text = string.Format("Must start with: \"Find\"{0}May NOT end with: \"Action\", \"Qry\"", Environment.NewLine);
            }
        }

        private void CheckMappedProperties(PropertyMap propertyMap)
        {
            List<string> propNames = new List<string>();
            foreach (MappedProperty prop in propertyMap.MappedProperties)
            {
                if (prop.Target == null)
                {
                    throw new ArgumentException("Property is not mapped.", prop.Source.Name);
                }

                string name = prop.Name;

                if (string.IsNullOrEmpty(name))
                {
                    name = prop.Target.Name;
                }

                if (prop.TargetProperty != null)
                {
                    if (prop.TargetProperty.BusinessEntity == null)
                    {
                        throw new ArgumentException("Property has no BusinessEntity.", prop.Name);
                    }
                }

                // Check the names
                NamingGuidance.CheckMappedPropertyName(name, false);

                if (propNames.Contains(name))
                {
                    throw new ArgumentException("Duplicate property name.", name);
                }
                else
                {
                    propNames.Add(name);
                }
            }
        }

        private void EnableDisableBtn()
        {
            lock (this)
            {
                nextBtn.Enabled = false;
                prevBtn.Enabled = false;
                finishBtn.Enabled = false;

                if (tabControl.SelectedTab == targetPage)
                {
                    if (entityCbx.SelectedIndex > -1 && !string.IsNullOrEmpty(actionNameTb.Text))
                    {
                        prevBtn.Enabled = false;
                        nextBtn.Enabled = true;
                        finishBtn.Enabled = false;
                    }
                }

                if (tabControl.SelectedTab == queryPage)
                {
                    prevBtn.Enabled = true;
                    nextBtn.Enabled = true;
                    finishBtn.Enabled = false;
                }

                if (tabControl.SelectedTab == procedurePage)
                {
                    prevBtn.Enabled = true;
                    nextBtn.Enabled = true;
                    finishBtn.Enabled = false;

                    
                    if (storedProcedureControl.ProcedureMissingInFileAndNoValidSelectonMade())
                    {
                        prevBtn.Enabled = true;
                        nextBtn.Enabled = false;
                        finishBtn.Enabled = false;
                    }
                }

                if (tabControl.SelectedTab == paramPage)
                {
                    prevBtn.Enabled = true;
                    nextBtn.Enabled = true;
                    finishBtn.Enabled = false;
                }

                if (tabControl.SelectedTab == advancedPage)
                {
                    prevBtn.Enabled = true;
                    nextBtn.Enabled = false;
                    finishBtn.Enabled = true;
                }

            }
        }

        private void ShowMissingSPMessage()
        {
            if (!newAction)
            {
                if (tabControl.SelectedTab == procedurePage)
                {

                    if (storedProcedureControl.ProcedureMissingInFileAndNoValidSelectonMade())
                    {
                        string spName = Action.StoredProcedure.ProcedureName;
                        string fileName = storedProcedureControl.FileName();
                        string errorText = string.Format("The Stored Procedure {0} is missing in file {1}.", spName, fileName);
                        MessageBox.Show(errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CheckOutInObject(DataAccess.IVersionControlled checkOutObject, bool checkOut)
        {
            DataAccess.IVersionControlled domainObject = null;
            domainObject = checkOutObject;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(domainObject.GetType()))
            {
                if (checkOut)
                {
                    try
                    {
                        MetaManagerServices.GetConfigurationManagementService().CheckOutDomainObject(domainObject.Id, domainObject.GetType());
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(domainObject.Id, domainObject.GetType(), BackendApplication);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void EnableDisableFields(bool enableOrDisable)
        {
            actionNameTb.ReadOnly = !enableOrDisable;

            targetProcedureBtn.Enabled = newAction;
            targetQueryBtn.Enabled = newAction;
            RowTrackingIdTbx.ReadOnly = !enableOrDisable;
        }

        private bool HandleDeletedQueryProperties(IList<QueryProperty> propertyList)
        {
            int i = 0;

            while (i < queryPropertyToDelete.Count)
            {
                if (Action.Query.Properties.Where(p => p.Id == queryPropertyToDelete[i].Id).Count() > 0)
                {
                    queryPropertyToDelete.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            queryPropertyToDelete.AddRange(propertyList.ToArray());

            return true;
        }

        #endregion

        private void CreateServiceMethodcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            cbCreateInService.Enabled = cbNewServiceMethod.Checked;
        }
    }

}
