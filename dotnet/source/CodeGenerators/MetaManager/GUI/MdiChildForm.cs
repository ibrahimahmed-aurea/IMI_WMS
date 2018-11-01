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

namespace Cdc.MetaManager.GUI
{
    public partial class MdiChildForm : Form
    {
        /// <summary>
        ///     Set this before starting a form that depends on this property.
        /// </summary>
        public MetaManager.DataAccess.Domain.Application FrontendApplication { get; set; }

        /// <summary>
        ///     Set this before starting a form that depends on this property.
        /// </summary>
        public MetaManager.DataAccess.Domain.Application BackendApplication { get; set; }

        /// <summary>
        ///     Set this before starting a form that depends on this property.
        /// </summary>
        public bool IsEditable { get; set; }

        public KeyValuePair<Guid, Type> ContaindDomainObjectIdAndType { get; set; }

        public string FrontendApplicationConfigPrefix
        {
            get
            {
                if (FrontendApplication != null)
                {
                    return FrontendApplication.GetPrefix();
                }
                return string.Empty;
            }
        }

        public string BackendApplicationConfigPrefix
        {
            get
            {
                if (BackendApplication != null)
                {
                    return BackendApplication.GetPrefix();
                }
                return string.Empty;
            }
        }

        public MdiChildForm()
        {
            InitializeComponent();

            FrontendApplication = null;
            BackendApplication = null;
            IsEditable = false;

            this.ControlAdded += new ControlEventHandler(MdiChildForm_ControlAdded);
            
        }

        void MdiChildForm_ControlAdded(object sender, ControlEventArgs e)
        {
            LookForPropertyGrid(e.Control);
        }

        private void LookForPropertyGrid(Control ctrl)
        {
            List<Control> ctrlStack = new List<Control>();
            int index = 0;
            if (ctrl is PropertyGrid)
            {
                ((PropertyGrid)ctrl).ContextMenuStrip = PropertyGridcontextMenuStrip;
                ((PropertyGrid)ctrl).SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(PropertyGrid_SelectedGridItemChanged);
            }
            else
            {
                ctrlStack.AddRange(ctrl.Controls.Cast<Control>());

                while (index < ctrlStack.Count)
                {
                    if (ctrlStack[index] is PropertyGrid)
                    {
                        ((PropertyGrid)ctrlStack[index]).ContextMenuStrip = PropertyGridcontextMenuStrip;
                        ((PropertyGrid)ctrlStack[index]).SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(PropertyGrid_SelectedGridItemChanged);
                    }
                    else if (ctrlStack[index].Controls.Count > 0)
                    {
                        ctrlStack.AddRange(ctrlStack[index].Controls.Cast<Control>());
                    }

                    index++;
                }

            }
        }
                
        private void PropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            object obj = e.NewSelection.PropertyDescriptor.GetValue(((PropertyGrid)sender).SelectedObject);

            if (obj is IDomainObject || obj is Guid)
            {
                PropertyGridcontextMenuStrip.Tag = obj;
            }
            else
            {
                PropertyGridcontextMenuStrip.Tag = null;
            }
        }

        private void copyIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PropertyGridcontextMenuStrip.Tag is IDomainObject)
            {
                Clipboard.SetText(((IDomainObject) PropertyGridcontextMenuStrip.Tag).Id.ToString());
            }
            else if (PropertyGridcontextMenuStrip.Tag is Guid)
            {
                Clipboard.SetText(((Guid)PropertyGridcontextMenuStrip.Tag).ToString());
            }
        }

        private void jumpToObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JumpTo((IDomainObject)PropertyGridcontextMenuStrip.Tag, false);
        }

        private void jumpToAndCheckOutObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JumpTo((IDomainObject)PropertyGridcontextMenuStrip.Tag, true);
        }

        private void jumpToReference_Click(object sender, EventArgs e)
        {
            IDomainObject domainObject = (IDomainObject)((ToolStripMenuItem) sender).Tag;

            if (this.Owner == null && this.MdiParent == null)
            {
                Clipboard.SetText(domainObject.Id.ToString());
            }
            else
            {
                JumpTo(domainObject, false);
            }
        }

        private void JumpTo(IDomainObject domainObject, bool checkOut)
        {
            dynamic parent;
            if (domainObject is IVersionControlled)
            {
                if (this.Owner != null && this.Owner.GetType().Namespace == "Cdc.MetaManager.GUI" && this.Owner.GetType().Name == "Main")
                {
                    parent = this.Owner;
                }
                else if (this.MdiParent != null && this.MdiParent.GetType().Namespace == "Cdc.MetaManager.GUI" && this.MdiParent.GetType().Name == "Main")
                {
                    parent = this.MdiParent;
                }
                else
                {
                    return;
                }

                parent.JumpToDomainObject(domainObject, checkOut);
            }
        }

        public override string ToString()
        {
            string returnString = this.Name;
            if (ContaindDomainObjectIdAndType.Value != null)
            {
                IDomainObject domainObject = MetaManager.BusinessLogic.MetaManagerServices.GetModelService().GetInitializedDomainObject(ContaindDomainObjectIdAndType.Key, ContaindDomainObjectIdAndType.Value);

                System.Reflection.PropertyInfo pi = domainObject.GetType().GetProperty("Name");

                if (pi != null)
                {
                    returnString = this.Name + " [" + pi.GetValue(domainObject, null).ToString() + "]";
                }
                else
                {
                    returnString = this.Name + " [" + domainObject.Id.ToString() + "]";
                }
            }
            return returnString;
        }

        private void MdiChildForm_Load(object sender, EventArgs e)
        {
            if (this.MdiParent != null && this.MdiParent.GetType().Namespace == "Cdc.MetaManager.GUI" && this.MdiParent.GetType().Name == "Main")
            {
                ToolStripComboBox childWindowList = ((ToolStripComboBox)((ToolStrip)((System.Windows.Forms.Form)this.MdiParent).Controls["tsApplication"]).Items["ChildWindowstoolStripComboBox"]);

                if (!childWindowList.Items.Contains(this))
                {
                    childWindowList.Items.Add(this);
                }
            }
        }

        private void MdiChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.MdiParent != null && this.MdiParent.GetType().Namespace == "Cdc.MetaManager.GUI" && this.MdiParent.GetType().Name == "Main")
            {
                ToolStripComboBox childWindowList = ((ToolStripComboBox)((ToolStrip)((System.Windows.Forms.Form)this.MdiParent).Controls["tsApplication"]).Items["ChildWindowstoolStripComboBox"]);

                if (childWindowList.Items.Contains(this))
                {
                    childWindowList.Items.Remove(this);
                }
            }
        }

        private void MdiChildForm_Activated(object sender, EventArgs e)
        {
            if (this.MdiParent != null && this.MdiParent.GetType().Namespace == "Cdc.MetaManager.GUI" && this.MdiParent.GetType().Name == "Main")
            {
                ToolStripComboBox childWindowList = ((ToolStripComboBox)((ToolStrip)((System.Windows.Forms.Form)this.MdiParent).Controls["tsApplication"]).Items["ChildWindowstoolStripComboBox"]);

                if (childWindowList.Items.Contains(this))
                {
                    childWindowList.SelectedIndex = childWindowList.Items.IndexOf(this);
                }
            }
        }

        private void PropertyGridcontextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (PropertyGridcontextMenuStrip.Tag is IDomainObject)
            {
                ((ToolStripMenuItem)PropertyGridcontextMenuStrip.Items[3]).DropDownItems.Clear();

                Cdc.MetaManager.BusinessLogic.IModelService modelservice = Cdc.MetaManager.BusinessLogic.MetaManagerServices.GetModelService();
                IList<IVersionControlled> refs = modelservice.GetReferencedVersionControlled((IDomainObject)PropertyGridcontextMenuStrip.Tag);

                if (refs.Count > 0)
                {
                    foreach (IVersionControlled refObj in refs)
                    {
                        Type classType = modelservice.GetDomainObjectType(refObj);
                        string name = string.Empty;

                        if (classType.GetProperty("Name") != null)
                        {
                            name = (string)classType.GetProperty("Name").GetValue(refObj, null);
                        }

                        ToolStripMenuItem item = new ToolStripMenuItem("[" + classType.Name + "] : " + (string.IsNullOrEmpty(name) ? refObj.Id.ToString() : name));
                        item.Tag = refObj;
                        item.Click += new EventHandler(jumpToReference_Click);

                        ((ToolStripMenuItem)PropertyGridcontextMenuStrip.Items[3]).DropDownItems.Add(item);
                    }

                    ((ToolStripMenuItem)PropertyGridcontextMenuStrip.Items[3]).Enabled = true;
                }
                else
                {
                    ((ToolStripMenuItem)PropertyGridcontextMenuStrip.Items[3]).Enabled = false;
                }
            }
            else
            {
                PropertyGridcontextMenuStrip.Items[3].Enabled = false;
            }

            if (PropertyGridcontextMenuStrip.Tag is IVersionControlled)
            {
                PropertyGridcontextMenuStrip.Items[0].Enabled = true;
                PropertyGridcontextMenuStrip.Items[1].Enabled = true;
                PropertyGridcontextMenuStrip.Items[2].Enabled = true;
            }
            else if (PropertyGridcontextMenuStrip.Tag is IDomainObject)
            {
                PropertyGridcontextMenuStrip.Items[0].Enabled = false;
                PropertyGridcontextMenuStrip.Items[1].Enabled = false;
                PropertyGridcontextMenuStrip.Items[2].Enabled = true;
            }
            else if (PropertyGridcontextMenuStrip.Tag is Guid)
            {
                PropertyGridcontextMenuStrip.Items[0].Enabled = false;
                PropertyGridcontextMenuStrip.Items[1].Enabled = false;
                PropertyGridcontextMenuStrip.Items[2].Enabled = true;
            }
            else
            {
                PropertyGridcontextMenuStrip.Items[0].Enabled = false;
                PropertyGridcontextMenuStrip.Items[1].Enabled = false;
                PropertyGridcontextMenuStrip.Items[2].Enabled = false;
            }
        }
    }
}
