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


namespace Cdc.MetaManager.GUI
{
    public partial class SelectModuleForm : MdiChildForm
    {
        private IDialogService dialogService = null;

        public Module SelectedModule { get; set; }

        public SelectModuleForm()
        {
            dialogService = MetaManagerServices.GetDialogService();

            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            SelectedModule = cbModules.SelectedItem as Module;
        }

        private void PopulateModuleCombobox()
        {
            IList<Module> modules = dialogService.GetAllModules(FrontendApplication.Id);

            moduleBindingSource.DataSource = modules.OrderBy(module => module.Name);
        }

        private void SelectModuleForm_Load(object sender, EventArgs e)
        {            
            PopulateModuleCombobox();
        }
    }
}
