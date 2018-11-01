using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;
using System.Workflow.Activities;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;

namespace Cdc.MetaManager.GUI.Workflow
{

    public partial class CreateWorkflow : MdiChildForm
    {
        public Cdc.MetaManager.DataAccess.Domain.Workflow workflow { get; set; }

        private IModelService modelService = null;

        public bool FixedModule { get; set; }

        public String startupModule = null;

        public CreateWorkflow()
        {
            InitializeComponent();
            modelService = MetaManagerServices.GetModelService();
        }

        private void CreateWorkflow_Load(object sender, EventArgs e)
        {


            PopulateModuleCombobox();

            EnableDisableButtons();

            moduleCbx.Text = startupModule;
            moduleCbx.Enabled = !FixedModule;

            Cursor.Current = Cursors.Default;
        }

        private void EnableDisableButtons()
        {

            btnOK.Enabled = false;

            if ((!string.IsNullOrEmpty(tbName.Text.Trim())) &&
               (!string.IsNullOrEmpty(moduleCbx.Text.Trim())) &&
                (!string.IsNullOrEmpty(tbDescription.Text.Trim())))
            {
                btnOK.Enabled = true;
            }

        }

        private void PopulateModuleCombobox()
        {

            IList<Module> modules = modelService.GetAllDomainObjectsByApplicationId<Module>(FrontendApplication.Id);

            moduleBindingSource.DataSource = modules.OrderBy(module => module.Name);

            // Find the last imported dialog module
            if (!string.IsNullOrEmpty(startupModule) &&
               moduleCbx.Items.Count > 0)
            {
                foreach (Module item in moduleCbx.Items.Cast<Module>())
                {
                    if (item.Name == startupModule)
                        moduleCbx.SelectedItem = item;
                }
            }


        }

        public void CreateNewWorkflow(Module module)
        {
            workflow = new Cdc.MetaManager.DataAccess.Domain.Workflow();
            workflow.RequestMap = new PropertyMap();
            workflow.Name = tbName.Text;
            workflow.Module = module;
            workflow.Description = tbDescription.Text;

            SequentialWorkflowActivity rootActivity = new SequentialWorkflowActivity("root");
            rootActivity.SetValue(WorkflowMarkupSerializer.XClassProperty, WorkflowTypeFactory.GetWorkflowClassFullName(workflow));

            StringBuilder sb = new StringBuilder();

            XmlWriter xmlWriter = XmlWriter.Create(sb);
            WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
            serializer.Serialize(xmlWriter, rootActivity);
            xmlWriter.Close();
            workflow.WorkflowXoml = sb.ToString();

        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (Module item in moduleCbx.Items.Cast<Module>())
            {
                if (item.Name == moduleCbx.Text)
                    moduleCbx.SelectedItem = item;
            }

            Module module = moduleCbx.SelectedItem as Module;

            try
            {

                module = modelService.GetInitializedDomainObject<DataAccess.Domain.Module>(module.Id);


                if (module == null)
                {
                    MessageBox.Show("You must first select a module.");
                    return;
                }

                CreateNewWorkflow(module);
                modelService.SaveDomainObject(workflow);
                CheckOutInObject(workflow, true);
                ContaindDomainObjectIdAndType = new KeyValuePair<Guid, Type>(workflow.Id, typeof(DataAccess.Domain.Workflow));
                DialogResult = DialogResult.OK;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        private void CheckOutInObject(DataAccess.IVersionControlled checkOutObject, bool trueCheckOut_falseCheckIn)
        {
            DataAccess.IVersionControlled domainObject = null;
            domainObject = checkOutObject;

            if (typeof(DataAccess.IVersionControlled).IsAssignableFrom(domainObject.GetType()))
            {
                if (trueCheckOut_falseCheckIn)
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
                        MetaManagerServices.GetConfigurationManagementService().CheckInDomainObject(domainObject.Id, domainObject.GetType(), FrontendApplication);
                        Cursor.Current = Cursors.Default;
                    }
                    catch (ConfigurationManagementException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void moduleCbx_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }



    }
}
