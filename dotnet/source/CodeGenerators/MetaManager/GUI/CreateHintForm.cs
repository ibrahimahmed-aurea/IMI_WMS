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
    public partial class CreateHintForm : MdiChildForm
    {
        private IModelService modelService = null;
        public Cdc.MetaManager.DataAccess.Domain.Hint Hint { get; set; }
        public Cdc.MetaManager.DataAccess.Domain.HintCollection hintCollection { get; set; }
        
        public CreateHintForm()
        {
            InitializeComponent();
            modelService = MetaManagerServices.GetModelService();
        }

                
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.hintCollection != null)
            {
                Hint = new Hint();
                Hint.Text = textTbx.Text;
                Hint.HintCollection = hintCollection;
                Hint = (Hint)modelService.SaveDomainObject(Hint);
                hintCollection.Hints.Add(this.Hint);
            }
            Close();
        }
    }
}
