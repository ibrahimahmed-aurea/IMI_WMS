using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic;

namespace Cdc.MetaManager.GUI
{
    public partial class CreatePropertyForm : MdiChildForm
    {
        IModelService modelService = null;

        BusinessEntity _businessEntity = null;

        public CreatePropertyForm(BusinessEntity businessEntity )
        {
            InitializeComponent();

            modelService = MetaManagerServices.GetModelService();
            DbProperty = null;
            _businessEntity = businessEntity;
        }

        public Property Property { get; set; }
        public DbProperty DbProperty { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!NamingGuidance.CheckMappedPropertyName(tbName.Text, true))
            {
                return;
            }
            
            Property = new Property();
            Property.Name = tbName.Text.Trim();
            
            Property.BusinessEntity = _businessEntity;
            
            if (rbString.Checked)
                Property.Type = typeof(string);
            else if (rbInt.Checked)
                Property.Type = typeof(int);
            else if (rbInt64.Checked)
                Property.Type = typeof(long);
            else if (rbDouble.Checked)
                Property.Type = typeof(double);
            else if (rbDecimal.Checked)
                Property.Type = typeof(decimal);
            else if (rbBoolean.Checked)
                Property.Type = typeof(bool);
            else if (rbDateTime.Checked)
                Property.Type = typeof(DateTime);

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void CreatePropertyForm_Load(object sender, EventArgs e)
        {
            rbString.Checked = true;

            if (DbProperty == null)
                return;

            try
            {
                Type type = DataModelImporter.GetClrTypeFromDbType(DbProperty.DbDatatype, DbProperty.Length, DbProperty.Precision, DbProperty.Scale, null);

                if (type == typeof(string))
                    rbString.Checked = true;
                else if (type == typeof(int))
                    rbInt.Checked = true;
                else if (type == typeof(long))
                    rbInt64.Checked = true;
                else if (type == typeof(double))
                    rbDouble.Checked = true;
                else if (type == typeof(decimal))
                    rbDecimal.Checked = true;
                else if (type == typeof(bool))
                    rbBoolean.Checked = true;
                else if (type == typeof(DateTime))
                    rbDateTime.Checked = true;
                else
                    rbString.Checked = true;
            }
            catch (ArgumentException)
            {
                rbString.Checked = true;
            }
        }
    }
}
