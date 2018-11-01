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
    public partial class ConfigureComboDialogForm : MdiChildForm
    {
        IApplicationService applicationService;
        IDialogService dialogService;
        IModelService modelService;

        public ConfigureComboDialogForm()
        {
            InitializeComponent();

            applicationService = MetaManagerServices.GetApplicationService();
            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();

        }

        public UXComboDialog ComboDialog { get; set; }
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
                    ComboDialog.Dialog = form.SelectedDialog;
                    loadKeyProperty();
                }
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (ComboDialog.ResultMap != null)
            {
                ComboDialog.ResultMap = applicationService.SaveAndMergePropertyMap(ComboDialog.ResultMap);
            }

            if (ComboDialog.ViewMap != null)
            {
                ComboDialog.ViewMap = applicationService.SaveAndMergePropertyMap(ComboDialog.ViewMap);
            }

            if (KeycomboBox.SelectedIndex > 0)
            {
                ComboDialog.KeyMappedProperty = (MappedProperty)KeycomboBox.SelectedItem;
            }
            else
            {
                ComboDialog.KeyMappedProperty = null;
                ComboDialog.KeyMappedPropertyId = Guid.Empty;
            }
        }

        private void ConfigureComboDialogForm_Load(object sender, EventArgs e)
        {
            if (ComboDialog.Dialog != null)
            {
                ComboDialog.Dialog = dialogService.GetDialog(ComboDialog.Dialog.Id);
            }

            if (ComboDialog.Dialog != null)
            {
                dialogTbx.Tag = ComboDialog.Dialog;
                dialogTbx.Text = ComboDialog.Dialog.Name;
                loadKeyProperty();
            }

            dialogFindBtn.Enabled = this.IsEditable;
            KeycomboBox.Enabled = this.IsEditable;
            okBtn.Enabled = this.IsEditable;
        }

        private void loadKeyProperty()
        {
            int selectIndex = 0;
            KeycomboBox.Items.Clear();
            KeycomboBox.DisplayMember = "Name";
            KeycomboBox.Items.Add("");
            Dialog tmpDialog = modelService.GetDynamicInitializedDomainObject<Dialog>(ComboDialog.DialogId, new List<string> { "InterfaceView.RequestMap" });
            PropertyMap requestMap = modelService.GetInitializedDomainObject<PropertyMap>(tmpDialog.InterfaceView.RequestMap.Id);

            if (requestMap != null)
            {
                //foreach (MappedProperty mptmp in requestMap.MappedProperties)
                //{
                //    MappedProperty mp = modelService.GetInitializedDomainObject<MappedProperty>(mptmp.Id);
                //    if (string.IsNullOrEmpty(mp.Name))
                //    {
                //        if (!string.IsNullOrEmpty(mp.Source.Name))
                //        {
                //            mptmp.Name = mp.Source.Name;
                //        }
                //        else
                //        {
                //            mptmp.Name = mptmp.Id.ToString();
                //        }
                //    }
                //}

                foreach (MappedProperty mp in requestMap.MappedProperties.OrderBy(m => m.Name))
                {
                    int index = KeycomboBox.Items.Add(mp);

                    if (mp.Id == ComboDialog.KeyMappedPropertyId)
                    {
                        selectIndex = KeycomboBox.Items.IndexOf(mp);
                    }
                }
            }

            KeycomboBox.SelectedIndex = selectIndex;
        }

        private void dialogMapBtn_Click(object sender, EventArgs e)
        {
            if (ComboDialog.Dialog != null)
            {
                PropertyMap findResultMap;
                PropertyMap viewResultMap;
                IList<IMappableProperty> temp;

                dialogService.GetViewResponseMap(View, out viewResultMap, out temp, out temp, out temp);
                dialogService.GetViewResponseMap(ComboDialog.Dialog.InterfaceView, out findResultMap, out temp, out temp, out temp);

                using (PropertyMapForm2 form = new PropertyMapForm2())
                {

                    form.PropertyMap = ComboDialog.ResultMap;

                    form.SourceProperties = viewResultMap.MappedProperties.Cast<IMappableProperty>();
                    form.TargetProperties = findResultMap.MappedProperties.Cast<IMappableProperty>();
                    form.EnablePropertiesByDefault = false;
                    form.AllowNonUniquePropertyNames = true;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        ComboDialog.ResultMap = form.PropertyMap;
                    }
                }
            }
        }

        private void modifyViewMapBtn_Click(object sender, EventArgs e)
        {

            if (ComboDialog.Dialog != null)
            {
                PropertyMap requestMap;
                PropertyMap responseMap;
                IList<IMappableProperty> targetProperties = null;
                IList<IMappableProperty> temp;

                dialogService.GetViewRequestMap(ComboDialog.Dialog.InterfaceView, out requestMap, out temp, out temp);
                dialogService.GetViewResponseMap(View, out responseMap, out temp, out temp, out temp);

                targetProperties = new List<IMappableProperty>(responseMap.MappedProperties.Cast<IMappableProperty>());

                IList<UXSessionProperty> sessionProperties = applicationService.GetUXSessionProperties(View.Application);
                targetProperties = new List<IMappableProperty>(targetProperties.Concat<IMappableProperty>(sessionProperties.Cast<IMappableProperty>()));

                using (PropertyMapForm2 form = new PropertyMapForm2())
                {

                    form.PropertyMap = ComboDialog.ViewMap;

                    form.SourceProperties = requestMap.MappedProperties.Cast<IMappableProperty>();
                    form.TargetProperties = targetProperties;
                    form.AllowNonUniquePropertyNames = true;
                    form.IsEditable = this.IsEditable;

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        ComboDialog.ViewMap = form.PropertyMap;
                    }
                }
            }

        }

    }
}
