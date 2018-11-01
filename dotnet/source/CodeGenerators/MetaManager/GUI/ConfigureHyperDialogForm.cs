using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess;


namespace Cdc.MetaManager.GUI
{
    public partial class ConfigureHyperDialogForm : MdiChildForm
    {
        IApplicationService applicationService;
        IDialogService dialogService;

        public ConfigureHyperDialogForm()
        {
            InitializeComponent();

            applicationService = MetaManagerServices.GetApplicationService();
            dialogService = MetaManagerServices.GetDialogService();
        }

        public UXHyperDialog HyperDialog { get; set; }
        public Cdc.MetaManager.DataAccess.Domain.View View { get; set; }

        private void dialogFindBtn_Click(object sender, EventArgs e)
        {
            using (SelectDialog form = new SelectDialog())
            {
                form.FrontendApplication = FrontendApplication;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.SelectedDialog.InterfaceView == null)
                    {
                        MessageBox.Show("The selected dialog is not valid since it has no interface view defined.");
                        return;
                    }

                    dialogTbx.Text = form.SelectedDialog.Name;
                    HyperDialog.Dialog = form.SelectedDialog;
                }
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (HyperDialog.ResultMap != null)
                HyperDialog.ResultMap = applicationService.SaveAndMergePropertyMap(HyperDialog.ResultMap);

            if (HyperDialog.ViewMap != null)
                HyperDialog.ViewMap = applicationService.SaveAndMergePropertyMap(HyperDialog.ViewMap);
        }

        private void ConfigureComboDialogForm_Load(object sender, EventArgs e)
        {
            if (HyperDialog.Dialog != null)
            {
                HyperDialog.Dialog = dialogService.GetDialog(HyperDialog.Dialog.Id);
            }

            if (HyperDialog.Dialog != null)
            {
                dialogTbx.Tag = HyperDialog.Dialog;
                dialogTbx.Text = HyperDialog.Dialog.Name;
            }

            dialogFindBtn.Enabled = this.IsEditable;
            okBtn.Enabled = this.IsEditable;

        }

        private void dialogMapBtn_Click(object sender, EventArgs e)
        {
            if (HyperDialog.Dialog != null)
            {
                PropertyMap findResultMap;
                PropertyMap viewResultMap;
                IList<IMappableProperty> temp;

                dialogService.GetViewResponseMap(View, out viewResultMap, out temp, out temp, out temp);
                dialogService.GetViewResponseMap(HyperDialog.Dialog.InterfaceView, out findResultMap, out temp, out temp, out temp);

                using (PropertyMapForm2 form = new PropertyMapForm2())
                {

                    form.PropertyMap = HyperDialog.ResultMap;

                    form.SourceProperties = viewResultMap.MappedProperties.Cast<IMappableProperty>();
                    form.TargetProperties = findResultMap.MappedProperties.Cast<IMappableProperty>();
                    form.EnablePropertiesByDefault = false;
                    form.AllowNonUniquePropertyNames = true;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        HyperDialog.ResultMap = form.PropertyMap;
                    }
                }
            }
        }

        private void modifyViewMapBtn_Click(object sender, EventArgs e)
        {

            if (HyperDialog.Dialog != null)
            {
                PropertyMap requestMap;
                PropertyMap responseMap;
                IList<IMappableProperty> targetProperties = null;
                IList<IMappableProperty> temp;

                dialogService.GetViewRequestMap(HyperDialog.Dialog.InterfaceView, out requestMap, out temp, out temp);
                dialogService.GetViewResponseMap(View, out responseMap, out temp, out temp, out temp);

                targetProperties = new List<IMappableProperty>(responseMap.MappedProperties.Cast<IMappableProperty>());

                IList<UXSessionProperty> sessionProperties = applicationService.GetUXSessionProperties(View.Application);
                targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

                using (PropertyMapForm2 form = new PropertyMapForm2())
                {

                    form.PropertyMap = HyperDialog.ViewMap;

                    form.SourceProperties = requestMap.MappedProperties.Cast<IMappableProperty>();
                    form.TargetProperties = targetProperties;
                    form.AllowNonUniquePropertyNames = true;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        HyperDialog.ViewMap = form.PropertyMap;
                    }
                }
            }

        }

    }
}
