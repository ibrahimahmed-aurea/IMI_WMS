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
using Spring.Data.NHibernate.Support;
using Cdc.MetaManager.DataAccess.Dao;
using Spring.Context;
using Spring.Context.Support;

using Cdc.MetaManager.BusinessLogic;
using NHibernate;

namespace Cdc.MetaManager.GUI
{
    public partial class ActionMapForm : MdiChildForm
    {
        private static IApplicationContext ctx;

        public ActionMapForm()
        {
            InitializeComponent();
            ctx = ContextRegistry.GetContext();
        }

        public ViewAction ViewAction { get; set; }
        
        public PropertyMap PropertyMap
        {
            get
            {
                return propertyMapControl.PropertyMap;
            }
            set
            {
                propertyMapControl.PropertyMap = value;
            }
        }

        private IViewActionDao viewActionDao;

        private void PropertyMapForm2_Load(object sender, EventArgs e)
        {
            using (new SessionScope(MetaManagerServices.GetSessionFactory(), MetaManagerServices.GetDomainInterceptor(), true, FlushMode.Never, true))
            {
                viewActionDao = ctx["ViewActionDao"] as IViewActionDao;

                ViewAction = viewActionDao.FindById(ViewAction.Id);
                                
                List<IMappableProperty> targetProperties = new List<IMappableProperty>(ViewAction.ViewNode.View.ResponseMap.MappedProperties.Cast<IMappableProperty>());
                                
                IUXSessionDao sessionDao = ctx["UXSessionDao"] as IUXSessionDao;

                foreach (UXSessionProperty property in sessionDao.FindByApplicationId(FrontendApplication.Id).Properties)
                {
                    targetProperties.Add(property);
                }

                if (ViewAction.ViewToActionMap != null)
                {
                    IPropertyMapDao propertyMapDao = ctx["PropertyMapDao"] as IPropertyMapDao;
                    propertyMapControl.PropertyMap = propertyMapDao.FindById(ViewAction.ViewToActionMap.Id);
                }

                propertyMapControl.SourceProperties = ViewAction.Action.RequestMap.MappedProperties.Cast<IMappableProperty>();
                propertyMapControl.TargetProperties = targetProperties;
                propertyMapControl.Map();
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            try
            {
                propertyMapControl.ValidateMapping();
                ViewAction.ViewToActionMap = PropertyMap;
                viewActionDao.SaveOrUpdate(ViewAction);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
