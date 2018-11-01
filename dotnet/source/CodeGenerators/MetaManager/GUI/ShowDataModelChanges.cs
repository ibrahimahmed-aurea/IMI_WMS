using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.DataAccess.Domain;


namespace Cdc.MetaManager.GUI
{
    public partial class ShowDataModelChanges : MdiChildForm
    {
        public DataModelChanges DetectedChanges { get; set; } 

        public ShowDataModelChanges()
        {
            InitializeComponent();
        }

        private void ShowDataModelChanges_Load(object sender, EventArgs e)
        {
            PopulateChangeTree();
        }

        private void PopulateChangeTree()
        {
            chkHintOnly.Checked = DetectedChanges.HintsOnly;
            // Clear all nodes
            tvChangeTree.Nodes.Clear();

            Dictionary<BusinessEntity, IList<KeyValuePair<object, DataModelChange>>> dictChanges = new Dictionary<BusinessEntity, IList<KeyValuePair<object, DataModelChange>>>();

            // Go through the list of changes
            foreach (KeyValuePair<object, DataModelChange> keyValue in DetectedChanges)
            {
                // Can either bee a business entity or a property as the key object
                if (keyValue.Key is BusinessEntity)
                {
                    if (!dictChanges.ContainsKey((BusinessEntity)keyValue.Key))
                    {
                        List<KeyValuePair<object, DataModelChange>> newList = new List<KeyValuePair<object, DataModelChange>>();
                        newList.Add(keyValue);

                        dictChanges.Add((BusinessEntity)keyValue.Key, newList);
                    }
                    else
                    {
                        dictChanges[(BusinessEntity)keyValue.Key].Add(keyValue);
                    }
                }
                else if (keyValue.Key is Property)
                {
                    if (!dictChanges.ContainsKey(((Property)keyValue.Key).BusinessEntity))
                    {
                        List<KeyValuePair<object, DataModelChange>> newList = new List<KeyValuePair<object, DataModelChange>>();
                        newList.Add(keyValue);

                        dictChanges.Add(((Property)keyValue.Key).BusinessEntity, newList);
                    }
                    else
                    {
                        dictChanges[((Property)keyValue.Key).BusinessEntity].Add(keyValue);
                    }
                }
            }

            if (dictChanges.Count > 0)
            {
                IOrderedEnumerable<KeyValuePair<BusinessEntity, IList<KeyValuePair<object, DataModelChange>>>> orderedChangeList = dictChanges.OrderBy(keyValue => keyValue.Key.Name);

                foreach(KeyValuePair<BusinessEntity, IList<KeyValuePair<object, DataModelChange>>> keyValue in orderedChangeList)
                {
                    // BusinessentityNode
                    TreeNode nodeBE = new TreeNode();

                    BusinessEntity be = (BusinessEntity)keyValue.Key;

                    nodeBE.Text = string.Format("{0} [{1}]", be.Name, be.TableName);

                    // Find the businessentity change
                    if (keyValue.Value.Where(x => x.Key is BusinessEntity).Count() == 1)
                    {
                        KeyValuePair<object, DataModelChange> businessEntityChange = keyValue.Value.Where(x => x.Key is BusinessEntity).ToList()[0];

                        SetNodeColor(nodeBE, businessEntityChange.Value);

                        nodeBE.Tag = businessEntityChange;
                        nodeBE.Checked = businessEntityChange.Value.Apply;
                    }
                    else
                    {
                        nodeBE.Tag = null;
                        nodeBE.Checked = false;
                    }

                    // Find all property changes for the businessentity
                    if (keyValue.Value.Where(x => x.Key is Property).Count() > 0)
                    {
                        // Get the list of property changes
                        IList<KeyValuePair<object, DataModelChange>> propertyChanges = keyValue.Value.Where(x => x.Key is Property).ToList();

                        foreach (KeyValuePair<object, DataModelChange> propertyKeyValue in propertyChanges)
                        {
                            TreeNode propNode = new TreeNode();

                            Property prop = (Property)propertyKeyValue.Key;

                            SetNodeColor(propNode, propertyKeyValue.Value);

                            propNode.Text = string.Format("{0} [{1}]", prop.Name, prop.StorageInfo.ColumnName);
                            propNode.Tag = propertyKeyValue;
                            propNode.Checked = propertyKeyValue.Value.Apply;

                            nodeBE.Nodes.Add(propNode);
                        }
                    }

                    tvChangeTree.Nodes.Add(nodeBE);
                }
            }

            tvChangeTree.ExpandAll();
        }

        private void SetNodeColor(TreeNode node, DataModelChange change)
        {
            if (change.ContainDataModelChangeType(DataModelChangeType.UnresolvedHint))
            {
                node.ForeColor = Color.Purple;
            }
            else if(change.ContainDataModelChangeType(DataModelChangeType.Deleted))
            {
                    node.ForeColor = Color.Red;
            }
            else if(change.ContainDataModelChangeType(DataModelChangeType.New))
            {
                    node.ForeColor = Color.Green;
            }
            else if (change.ContainDataModelChangeType(DataModelChangeType.Modified))
            {
                node.ForeColor = Color.Blue;
            }
            else if (change.ContainDataModelChangeType(DataModelChangeType.ModifiedHint))
            {
                node.ForeColor = Color.Blue;
            }
            else if (change.ContainDataModelChangeType(DataModelChangeType.NewHint))
            {
                node.ForeColor = Color.Blue;
            }
            else if (change.ContainDataModelChangeType(DataModelChangeType.DeletedHint))
            {
                node.ForeColor = Color.Blue;
            }

        }

        private void tvChangeTree_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // If there is no tag then the checked property can't be changed.
            if (e.Node.Tag == null)
            {
                e.Cancel = true;
            }
            else 
            {
                KeyValuePair<object, DataModelChange> tagObject = (KeyValuePair<object, DataModelChange>)e.Node.Tag;

                if (tagObject.Key is BusinessEntity &&
                    tagObject.Value.ContainDataModelChangeType(DataModelChangeType.New) &&
                    !e.Node.Checked &&
                    e.Action != TreeViewAction.Unknown)
                {
                    e.Cancel = true;
                }
            }
        }

        private void tvChangeTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tbChangeText.Text = string.Empty;

            if (e.Node.Tag != null)
            {
                KeyValuePair<object, DataModelChange> tagObject = (KeyValuePair<object, DataModelChange>)e.Node.Tag;

                foreach (DetectedChange change in tagObject.Value.ListedDetectedChanges)
                {
                    tbChangeText.Text += change.ToString() + Environment.NewLine +Environment.NewLine;
                }
                tbChangeText.Text.Trim();
            }
        }

        private void tvChangeTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                bool isChecked = e.Node.Checked;

                // Fetch the tag object
                KeyValuePair<object, DataModelChange> tagObject = (KeyValuePair<object, DataModelChange>)e.Node.Tag;

                // Set checked status
                SetCheckedStatus(e.Node, isChecked);

                // If unchecking a BusinessEntity and it's new, then uncheck all Properties bellow it.
                if (tagObject.Key is BusinessEntity &&
                    tagObject.Value.ContainDataModelChangeType(DataModelChangeType.New) &&
                    !isChecked)
                {
                    tvChangeTree.AfterCheck -= tvChangeTree_AfterCheck;

                    foreach (TreeNode childNode in e.Node.Nodes)
                    {
                        childNode.Checked = false;
                        SetCheckedStatus(childNode, false);
                    }

                    tvChangeTree.AfterCheck += tvChangeTree_AfterCheck;
                }

                // If checking a Property and the parent node is a Business entity that is new that is not
                // checked then also check the Business Entity
                if (tagObject.Key is Property &&
                    isChecked &&
                    e.Node.Parent != null &&
                    e.Node.Parent.Tag != null &&
                    !e.Node.Parent.Checked)
                {
                    // Fetch the parent tag object
                    KeyValuePair<object, DataModelChange> parentTagObject = (KeyValuePair<object, DataModelChange>)e.Node.Parent.Tag;

                    if (parentTagObject.Key is BusinessEntity &&
                        parentTagObject.Value.ContainDataModelChangeType(DataModelChangeType.New))
                    {
                        tvChangeTree.AfterCheck -= tvChangeTree_AfterCheck;
                        e.Node.Parent.Checked = true;
                        SetCheckedStatus(e.Node.Parent, true);
                        tvChangeTree.AfterCheck += tvChangeTree_AfterCheck;
                    }
                }
                // If parentnode is of type new then atleast one of the childnodes needs to be checked
                // or the parentnode should be unchecked also.
                else if (e.Node.Parent != null &&
                         e.Node.Parent.Tag != null &&
                         e.Node.Parent.Checked &&
                         !isChecked)
                {
                    // Fetch the parent tag object
                    KeyValuePair<object, DataModelChange> parentTagObject = (KeyValuePair<object, DataModelChange>)e.Node.Parent.Tag;

                    if (parentTagObject.Key is BusinessEntity &&
                        parentTagObject.Value.ContainDataModelChangeType(DataModelChangeType.New))
                    {
                        bool anyChecked = false;

                        foreach (TreeNode parentChildNode in e.Node.Parent.Nodes)
                        {
                            if (parentChildNode.Checked)
                            {
                                anyChecked = true;
                                break;
                            }
                        }

                        if (!anyChecked)
                        {
                            tvChangeTree.AfterCheck -= tvChangeTree_AfterCheck;
                            e.Node.Parent.Checked = false;
                            SetCheckedStatus(e.Node.Parent, false);
                            tvChangeTree.AfterCheck += tvChangeTree_AfterCheck;
                        }
                    }
                }

            }
        }

        private void SetCheckedStatus(TreeNode node, bool isChecked)
        {
            // Fetch the tag object
            KeyValuePair<object, DataModelChange> tagObject = (KeyValuePair<object, DataModelChange>)node.Tag;

            // Set checked status to it.
            tagObject.Value.Apply = isChecked;
        }
                
        private void btnCancel_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DetectedChanges.HintsOnly = chkHintOnly.Checked ;
            DialogResult = DialogResult.OK;
        }

    }
}
