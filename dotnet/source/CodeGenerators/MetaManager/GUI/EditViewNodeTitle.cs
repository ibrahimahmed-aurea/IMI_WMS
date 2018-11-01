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
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using Cdc.MetaManager.BusinessLogic.Helpers;


namespace Cdc.MetaManager.GUI
{
    public partial class EditViewNodeTitle : Form
    {
        public ViewNode ViewNode { get; set; }

        private IDialogService dialogService = null;

        public EditViewNodeTitle()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
        }

        private void EditViewNode_Load(object sender, EventArgs e)
        {
            // Read the ViewNode
            ViewNode = dialogService.GetViewNodeById(ViewNode.Id);

            tbViewNodeTitle.Text = ViewNode.Title;

            btnVisible.Checked = ViewNode.Visibility == UXVisibility.Visible;

            foreach (RenderMode type in Enum.GetValues(typeof(RenderMode)))
            {
                modeCbx.Items.Add(type.ToString());
            }

            foreach (UpdatePropagation type in Enum.GetValues(typeof(UpdatePropagation)))
            {
                propagationCbx.Items.Add(type.ToString());
            }

            propagationCbx.SelectedIndex = (int)ViewNode.UpdatePropagation;

            if (ViewNode.Parent != null)
            {
                propagationCbx.Enabled = true;
            }
            else
            {
                propagationCbx.Enabled = false;
            }

            bool enabled = false;
                            
            modeCbx.SelectedIndex = (int)ViewNode.RenderMode;
                        
            if ((ViewNode.Parent != null) && (ViewNode.Parent.View != null))
            {
                Cdc.MetaManager.DataAccess.Domain.View view = dialogService.GetViewById(ViewNode.Parent.View.Id);

                if (view.ServiceMethod != null &&
                    view.ServiceMethod.Id == ViewNode.View.ServiceMethod.Id)
                {
                    if (ViewHelper.GetAllComponents<UXDataGrid>(view.VisualTree).Count > 0)
                        enabled = true;
                }
            }

            modeCbx.Enabled = enabled;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ViewNode.Title = tbViewNodeTitle.Text.Trim();

            if (btnVisible.Checked)
                ViewNode.Visibility = UXVisibility.Visible;
            else
                ViewNode.Visibility = UXVisibility.Collapsed;

            ViewNode.RenderMode = (RenderMode)Enum.Parse(typeof(RenderMode), modeCbx.SelectedItem.ToString());
            ViewNode.UpdatePropagation = (UpdatePropagation)Enum.Parse(typeof(UpdatePropagation), propagationCbx.SelectedItem.ToString());

            // Save the ViewNode
            dialogService.SaveViewNode(ViewNode);

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
                
    }
}
