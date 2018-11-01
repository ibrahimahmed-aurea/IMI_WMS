using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.CodeDom.Compiler;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser;

namespace Cdc.MetaManager.CodeGeneration.CodeSmithTemplateParser.GUI
{
    public partial class CompileRuntime : Form
    {
        delegate bool IsCheckedDelegate(string entity);

        public string ReferencesDirectory { get; set; }
        public string SourceDirectory { get; set; }
        public List<string> IgnoreSourceFileList { get; set; }
        public List<string> ReferencesFileList { get; set; }
        public ConfigurationSettings Config { get; set; }

        public CompileRuntime()
        {
            InitializeComponent();
        }

        private void CompileRuntime_Load(object sender, EventArgs e)
        {
            tbCompileRootDir.Text = SourceDirectory;
            tbReferenceRootDir.Text = ReferencesDirectory;

            LoadFromConfig();

            PopulateSourceFileTreeView();
            PopulateReferencesTreeView();

            CheckTreeView(tvSourceFileTree);

            EnableDisableButtons();
        }

        private void LoadFromConfig()
        {
            IgnoreSourceFileList = Config.NotCompileList;
            ReferencesFileList = Config.ReferenceList;

            tbAssemblyName.Text = Config.AssemblyName;
            cbDebugInfo.Checked = Config.DebugInfo;
        }

        private void PopulateReferencesTreeView()
        {
            if (Directory.Exists(ReferencesDirectory))
            {
                tvReferences.Nodes.Clear();

                TreeNode rootNode = new TreeNode();

                if (PopulateTreeView(ReferencesDirectory, ReferencesDirectory, ".dll", rootNode, IsReferenceChecked))
                {
                    foreach (TreeNode childNode in rootNode.Nodes)
                    {
                        tvReferences.Nodes.Add(childNode);
                    }
                }
            }
        }

        private void PopulateSourceFileTreeView()
        {
            if (Directory.Exists(SourceDirectory))
            {
                tvSourceFileTree.Nodes.Clear();

                TreeNode rootNode = new TreeNode();

                if (PopulateTreeView(SourceDirectory, SourceDirectory, ".cs", rootNode, IsDoCompile))
                {
                    foreach (TreeNode childNode in rootNode.Nodes)
                    {
                        tvSourceFileTree.Nodes.Add(childNode);
                    }
                }
            }
        }

        private bool PopulateTreeView(string fromDirectory, string rootPath, string fileExtension, TreeNode node, IsCheckedDelegate IsCheckedFunc)
        {
            bool result = false;

            node.Text = fromDirectory.Remove(0, rootPath.Length);
            node.Tag = fromDirectory;

            if (IsCheckedFunc != null)
            {
                node.Checked = IsCheckedFunc(fromDirectory);
            }

            List<string> directories = Directory.GetDirectories(fromDirectory).ToList();

            if (directories.Count > 0)
            {
                foreach (string directory in directories)
                {
                    TreeNode newNode = new TreeNode();

                    if (PopulateTreeView(directory, rootPath, fileExtension, newNode, IsCheckedFunc))
                    {
                        node.Nodes.Add(newNode);
                        result = true;
                    }
                }
            }

            List<string> fileList = Directory.GetFiles(fromDirectory, string.Format("*{0}", fileExtension)).ToList();

            if (fileList.Count > 0)
            {
                result = true;

                foreach (string fileName in fileList)
                {
                    TreeNode fileNode = node.Nodes.Add(fileName.Remove(0, rootPath.Length));
                    fileNode.Tag = fileName;

                    if (IsCheckedFunc != null)
                    {
                        fileNode.Checked = IsCheckedFunc(fileName);
                    }
                }
            }

            return result;
        }

        private bool IsReferenceChecked(string entity)
        {
            if (File.Exists(entity) && ReferencesFileList != null && ReferencesFileList.Count > 0)
            {
                foreach (string reference in ReferencesFileList)
                {
                    if (entity.StartsWith(reference, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        private bool IsDoCompile(string entity)
        {
            if (IgnoreSourceFileList != null && IgnoreSourceFileList.Count > 0)
            {
                foreach (string ignore in IgnoreSourceFileList)
                {
                    if (entity.StartsWith(ignore, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void CheckTreeView(TreeView treeView)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                CheckNode(node);
                ColorTree(node);
            }
        }

        private bool CheckNode(TreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                bool anyChecked = false;

                foreach (TreeNode childNode in node.Nodes)
                {
                    if (CheckNode(childNode))
                    {
                        anyChecked = true;
                    }
                }

                node.Checked = anyChecked;

                return anyChecked;
            }
            else
            {
                return node.Checked;
            }
        }

        private void tvSourceFileTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                // Remove this event so that it isn't triggered within the event
                //tvSourceFileTree.AfterCheck -= tvSourceFileTree_AfterCheck;

                // Check if node is not checked. If the node has children then
                // all of them should be unchecked.
                if (!e.Node.Checked)
                {
                    SetCheckedRecursive(e.Node.Checked, e.Node);

                    // Check if parents all childrens are unchecked, in that case uncheck that too
                    if (e.Node.Parent != null)
                    {
                        TreeNode parent = e.Node.Parent;

                        while (parent != null)
                        {
                            bool allUnchecked = true;

                            foreach (TreeNode node in parent.Nodes)
                            {
                                if (node.Checked)
                                {
                                    allUnchecked = false;
                                    break;
                                }
                            }

                            if (allUnchecked)
                                parent.Checked = false;
                            else
                                break;

                            parent = parent.Parent;
                        }
                    }
                }
                else
                {
                    // The Node is Checked
                    // Check if parent nodes upwards are checked which they should be.
                    TreeNode parent = e.Node.Parent;

                    while (parent != null)
                    {
                        parent.Checked = true;

                        parent = parent.Parent;
                    }
                }

                // Color the tree
                foreach (TreeNode node in tvSourceFileTree.Nodes)
                {
                    ColorTree(node);
                }

                // Add the event again
                //tvSourceFileTree.AfterCheck += tvSourceFileTree_AfterCheck;
            }
        }

        private bool ColorTree(TreeNode treeNode)
        {
            bool result = false;

            treeNode.ForeColor = Color.Black;

            if (treeNode.Nodes.Count > 0)
            {
                if (!treeNode.Checked)
                {
                    result = true;
                }

                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    if (ColorTree(childNode))
                    {
                        treeNode.ForeColor = Color.Red;
                        result = true;
                    }
                }
            }
            else
            {
                if (!treeNode.Checked)
                {
                    result = true;
                }
            }

            return result;
        }

        private void SetCheckedRecursive(bool isChecked, TreeNode treeNode)
        {
            treeNode.Checked = isChecked;

            if (treeNode.Nodes.Count > 0)
            {
                foreach (TreeNode childNode in treeNode.Nodes)
                {
                    SetCheckedRecursive(isChecked, childNode);
                }
            }
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            SaveToConfig();

            List<string> compileFileList = GetFileListFromTreeView(tvSourceFileTree, true, false);
            List<string> referencesFileList = GetFileListFromTreeView(tvReferences, true, false);

            if (compileFileList.Count > 0)
            {
                CompilerErrorCollection errorCollection;

                string assemblyName = FileTemplateParser.CreateAssembly(compileFileList, referencesFileList, tbAssemblyName.Text, cbDebugInfo.Checked, out errorCollection);

                PopulateCompilationErrors(errorCollection);

                if (!string.IsNullOrEmpty(assemblyName))
                {
                    MessageBox.Show(string.Format("Assembly \"{0}\" created.", assemblyName));
                }
            }
        }

        private void SaveToConfig()
        {
            // Get list of unchecked files in compile file list
            Config.NotCompileList = GetFileListFromTreeView(tvSourceFileTree, false, true);

            // Get list of checked files in references list
            Config.ReferenceList = GetFileListFromTreeView(tvReferences, true, true);

            Config.AssemblyName = tbAssemblyName.Text;

            Config.DebugInfo = cbDebugInfo.Checked;

            Config.Save();
        }

        private void PopulateCompilationErrors(CompilerErrorCollection errorCollection)
        {
            lvCompileErrors.Items.Clear();
            tbErrorDetail.Text = string.Empty;

            if (errorCollection != null && errorCollection.Count > 0)
            {
                foreach (CompilerError error in errorCollection)
                {
                    ListViewItem item = new ListViewItem(error.ErrorNumber);
                    item.SubItems.Add(error.ErrorText);
                    item.SubItems.Add(error.FileName);

                    item.Tag = error;

                    lvCompileErrors.Items.Add(item);
                }
            }
        }

        private List<string> GetFileListFromTreeView(TreeView treeView, bool getChecked, bool includeDirectories)
        {
            List<string> fileList = new List<string>();

            foreach (TreeNode node in treeView.Nodes)
            {
                GetCompileFileListFromNode(node, getChecked, includeDirectories, fileList);
            }

            return fileList;
        }

        private void GetCompileFileListFromNode(TreeNode node, bool isChecked, bool includeDirectories, List<string> fileList)
        {
            // Check if node has any children nodes
            if (node.Nodes.Count > 0)
            {
                bool anyDiffers = false;

                if (includeDirectories)
                {
                    foreach (TreeNode childNode in node.Nodes)
                    {
                        if (childNode.Checked != isChecked)
                        {
                            anyDiffers = true;
                            break;
                        }
                    }

                    if (!anyDiffers)
                    {
                        fileList.Add((string)node.Tag);
                    }
                }

                if (!includeDirectories || (includeDirectories && anyDiffers))
                {
                    foreach (TreeNode childNode in node.Nodes)
                    {
                        GetCompileFileListFromNode(childNode, isChecked, includeDirectories, fileList);
                    }
                }
            }
            else
            {
                if ((node.Checked && isChecked) || 
                    (!node.Checked && !isChecked))
                {
                    fileList.Add((string)node.Tag);
                }
            }
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = tbAssemblyName.Text;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbAssemblyName.Text = saveFileDialog.FileName;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lvCompileErrors_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            tbErrorDetail.Text = string.Empty;

            if (lvCompileErrors.SelectedItems.Count == 1)
            {
                CompilerError error = (CompilerError)lvCompileErrors.SelectedItems[0].Tag;

                tbErrorDetail.Text = error.ErrorText + Environment.NewLine +
                    string.Format(@"(Line: {0}, Col: {1}", error.Line.ToString(), error.Column.ToString());
            }
        }

        private void lvCompileErrors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbAssemblyName_TextChanged(object sender, EventArgs e)
        {
            EnableDisableButtons();
        }

        private void EnableDisableButtons()
        {
            btnCompile.Enabled = false;

            if (!string.IsNullOrEmpty(tbAssemblyName.Text))
            {
                btnCompile.Enabled = true;
            }
        }

        private void tvSourceFileTree_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown && 
                !e.Node.Checked &&
                e.Node.Nodes.Count > 0)
            {
                e.Cancel = true;
            }
        }

        private void tvReferences_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                // Check if node is not checked. If the node has children then
                // all of them should be unchecked.
                if (!e.Node.Checked)
                {
                    SetCheckedRecursive(e.Node.Checked, e.Node);

                    // Check if parents all childrens are unchecked, in that case uncheck that too
                    if (e.Node.Parent != null)
                    {
                        TreeNode parent = e.Node.Parent;

                        while (parent != null)
                        {
                            bool allUnchecked = true;

                            foreach (TreeNode node in parent.Nodes)
                            {
                                if (node.Checked)
                                {
                                    allUnchecked = false;
                                    break;
                                }
                            }

                            if (allUnchecked)
                                parent.Checked = false;
                            else
                                break;

                            parent = parent.Parent;
                        }
                    }
                }
                else
                {
                    // The Node is Checked
                    // Check if parent nodes upwards are checked which they should be.
                    TreeNode parent = e.Node.Parent;

                    while (parent != null)
                    {
                        parent.Checked = true;

                        parent = parent.Parent;
                    }
                }

                // Color the tree
                foreach (TreeNode node in tvReferences.Nodes)
                {
                    ColorTree(node);
                }

                // Add the event again
                //tvSourceFileTree.AfterCheck += tvSourceFileTree_AfterCheck;
            }
        }
    }
}
