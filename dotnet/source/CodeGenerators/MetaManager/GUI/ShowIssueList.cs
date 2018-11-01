using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess.Domain;

namespace Cdc.MetaManager.GUI
{
    public partial class ShowIssueList : Form
    {
        private IAnalyzeService analyzeService = null;

        private AnalyzeIssueTree AnalyzeIssueTree { get; set; }

        private TreeNode SelectTreeNode { get; set; }

        private AnalyzeIssueList CurrentWorkingIssueList { get; set; }

        public ShowIssueList(AnalyzeIssueTree issueTree)
        {
            InitializeComponent();

            AnalyzeIssueTree = issueTree;

            analyzeService = MetaManagerServices.GetAnalyzeService();
        }

        private void ShowIssueList_Load(object sender, EventArgs e)
        {
            ShowIssueNodeTree();
        }

        private void ShowIssueNodeTree()
        {
            // Clear Listview
            tvIssueTree.Nodes.Clear();

            SelectTreeNode = null;

            if (AnalyzeIssueTree != null && AnalyzeIssueTree.IssueNodes.Count > 0)
            {
                foreach (AnalyzeIssueNode issueNode in AnalyzeIssueTree.IssueNodes)
                {
                    bool doHideNode = DoHideNode(issueNode, cbShowHintIssues.Checked);

                    if (cbShowHidden.Checked || !doHideNode)
                    {
                        tvIssueTree.Nodes.Add(AddNodes(issueNode, new TreeNode(), doHideNode));
                    }
                }
            }

            // Select the found Select Tree Node
            if (SelectTreeNode != null)
            {
                tvIssueTree.SelectedNode = SelectTreeNode;
            }
            else
            // If no node is selected then select the first one
            if (tvIssueTree.SelectedNode == null && tvIssueTree.Nodes.Count > 0)
            {
                tvIssueTree.SelectedNode = tvIssueTree.Nodes[0];
            }
        }

        private bool DoHideNode(AnalyzeIssueNode node, bool showHintIssues)
        {
            if (node.Children.Count > 0)
            {
                foreach (AnalyzeIssueNode childNode in node.Children)
                {
                    if (!DoHideNode(childNode, showHintIssues))
                    {
                        return false;
                    }
                }
            }

            if (node.IssueList != null)
            {
                // Then check if there are any issues that aren't hidden
                var shownIssues = from i in node.IssueList
                                where !i.Hidden && 
                                      (showHintIssues ||
                                       (!showHintIssues && !i.Text.ToUpper().Contains("NO HINT!"))
                                      )
                                select i;

                if (shownIssues.Count() > 0)
                    return false;
            }

            return true;
        }


        private TreeNode AddNodes(AnalyzeIssueNode node, TreeNode treeNode, bool doHideNode)
        {
            if (node != null && treeNode != null)
            {
                treeNode.Tag = node.IssueList;

                // Check if this IssueList is same as the current selected
                if (node.IssueList != null &&
                    CurrentWorkingIssueList != null &&
                    node.IssueList.Equals(CurrentWorkingIssueList))
                {
                    SelectTreeNode = treeNode;
                }

                SetTreeNodeText(node, treeNode, doHideNode);

                if (node.Children.Count > 0)
                {
                    foreach (AnalyzeIssueNode childNode in node.Children)
                    {
                        bool doHideChildNode = DoHideNode(childNode, cbShowHintIssues.Checked);

                        if (cbShowHidden.Checked || !doHideChildNode)
                        {
                            treeNode.Nodes.Add(AddNodes(childNode, new TreeNode(), doHideChildNode));
                        }
                    }
                }
            }

            return treeNode;
        }

        private void SetTreeNodeText(AnalyzeIssueNode node, TreeNode treeNode, bool doHideNode)
        {
            if (node != null && treeNode != null)
            {
                treeNode.Text = node.Name;

                if (node.IssueList != null && node.IssueList.Count > 0)
                {
                    treeNode.Text += string.Format(" ({0} issue{1})", node.IssueList.Count.ToString(), node.IssueList.Count > 1 ? "s" : string.Empty);

                    treeNode.ForeColor = Color.Red;
                }

                if (doHideNode)
                    treeNode.ForeColor = Color.Green;
            }
        }


        private void tvIssueTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentWorkingIssueList = null;

            if (tvIssueTree.SelectedNode != null && tvIssueTree.SelectedNode.Tag is AnalyzeIssueList)
            {
                CurrentWorkingIssueList = tvIssueTree.SelectedNode.Tag as AnalyzeIssueList;

                // Show the issuelist
                ShowCurrentWorkingIssueList();

                // Select the first in issuelist if there are any
                if (lvIssueList.Items.Count > 0)
                {
                    lvIssueList.Items[0].Selected = true;
                }
            }
        }

        private bool UnsavedChangesHandled()
        {
            if (CurrentWorkingIssueList != null)
            {
                bool haveUnsavedChanges = (from i in CurrentWorkingIssueList
                                           where i.Hidden != i.IsNewHiddenValue
                                           select i).Count() > 0;

                if (haveUnsavedChanges)
                {
                    if (MessageBox.Show("There are unsaved changes. Do you want to continue anyway?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return false;
                    }
                    else
                    {
                        // Reset the issues in the list
                        foreach (Issue issue in CurrentWorkingIssueList)
                        {
                            issue.IsNewHiddenValue = issue.Hidden;
                        }
                    }
                }
            }

            return true;
        }

        private void lvIssueList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvIssueList.SelectedItems.Count == 1)
            {
                tbFullText.Text = lvIssueList.SelectedItems[0].SubItems[1].Text;
                propertyGrid.SelectedObject = ((Issue)lvIssueList.SelectedItems[0].Tag).Tag;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Check if current issuelist has been saved if there are changes
            if (!UnsavedChangesHandled())
            {
                return;
            }

            Close();
        }

        private void btnExpandAll_Click(object sender, EventArgs e)
        {
            tvIssueTree.ExpandAll();
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {
            tvIssueTree.CollapseAll();
        }

        private void cbShowHidden_CheckedChanged(object sender, EventArgs e)
        {
            // Check if current issuelist has been saved if there are changes
            if (!UnsavedChangesHandled())
            {
                return;
            }

            ShowIssueNodeTree();
            ShowCurrentWorkingIssueList();
        }

        private void ShowCurrentWorkingIssueList()
        {
            // Clear Listview
            lvIssueList.Items.Clear();
            tbFullText.Text = string.Empty;
            propertyGrid.SelectedObject = null;

            if (CurrentWorkingIssueList != null && CurrentWorkingIssueList.Count > 0)
            {
                foreach (Issue issue in CurrentWorkingIssueList)
                {
                    if (!issue.Hidden || (issue.Hidden && cbShowHidden.Checked))
                    {
                        ListViewItem item = lvIssueList.Items.Add(issue.Severity.ToString());

                        // Update forecolor depending on severity and if checked status
                        // is different than hidden status
                        UpdateIssueForeColor(issue, item);

                        item.SubItems.Add(issue.Text);
                        item.Checked = issue.IsNewHiddenValue;
                        item.Tag = issue;
                    }
                }
            }
        }

        private static void UpdateIssueForeColor(Issue issue, ListViewItem item)
        {
            switch (issue.Severity)
            {
                case IssueSeverityType.Error:
                    if (issue.Hidden)
                        item.ForeColor = Color.Green;
                    else if (issue.Hidden != issue.IsNewHiddenValue)
                        item.ForeColor = Color.FromArgb(128, 0, 0);
                    else
                        item.ForeColor = Color.FromArgb(255, 0, 0);
                    break;
                case IssueSeverityType.Warning:
                    if (issue.Hidden)
                        item.ForeColor = Color.Green;
                    else if (issue.Hidden != issue.IsNewHiddenValue)
                        item.ForeColor = Color.FromArgb(120, 64, 0);
                    else
                        item.ForeColor = Color.FromArgb(240, 128, 0);
                    break;
            }
        }

        private void lvIssueList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                Issue issue = e.Item.Tag as Issue;

                issue.IsNewHiddenValue = e.Item.Checked;

                UpdateIssueForeColor(issue, e.Item);

            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvIssueList.Items)
            {
                item.Checked = true;
            }
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvIssueList.Items)
            {
                item.Checked = false;
            }
        }

        private void btnSaveSelection_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvIssueList.Items)
            {
                Issue issue = item.Tag as Issue;

                if (issue.Hidden != issue.IsNewHiddenValue)
                {
                    issue.Hidden = issue.IsNewHiddenValue;

                    analyzeService.SaveOrUpdateIssue(issue);
                }
            }

            ShowIssueNodeTree();
            ShowCurrentWorkingIssueList();
        }

        private void tvIssueTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // Check if current issuelist has been saved if there are changes
            if (!UnsavedChangesHandled())
            {
                e.Cancel = true;
            }
        }

        private void cbShowHintIssues_CheckedChanged(object sender, EventArgs e)
        {
            // Check if current issuelist has been saved if there are changes
            if (!UnsavedChangesHandled())
            {
                return;
            }

            ShowIssueNodeTree();
            ShowCurrentWorkingIssueList();
        }


    }
}
