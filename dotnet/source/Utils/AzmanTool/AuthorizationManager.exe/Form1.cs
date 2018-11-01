using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AuthorizationParser;
using AuthorizationManager.Utilities.TreeViewHelpers;
using AuthorizationParser.Writers;
using AuthorizationParser.Readers;
using AuthorizationParser.Utilities;
using AuthorizationParser.Models;
using System.Runtime.InteropServices;
using AuthorizationManager.Gui.exe.Utilities.TreeViewHelpers;

namespace AuthorizationManager
{
    public partial class Form1 : Form
    {
        private bool isRoleSaved = true;
        private bool isTreeViewExpanded = false;
        private string _noRolesString = "[No roles found]";
        private string _noWinUsersString = "[No Windows Users Found]";
        Dictionary<string, ApplicationXMLModel> _applications = null;
        private List<string> duplicateKeys = new List<string>();
        private string _previousRole;
        private string _previousApplication;
        public Form1()
        {
            InitializeComponent();
            //bool resultCreateMetaMenu = MetaDataParser.CreateMetaMenu("", "metadata");
            //  bool resultCreateMetaMenu = MetaDataParser.CreateMetaMenu("", "C:/Project/Views/saspegren_Trunk_ss/metadata");
            //  bool resultCreateMetaMenu = MetaDataParser.CreateMetaMenu("", "C:/Project/Views/saspegren_COOP723_SS/metadata");

            _applications = MetaDataParser.LoadMetaMenu("MetaMenu.xml");

            if (_applications.Count > 0)
            {
                PopulateApplicationsComboBox();
            }
            ApplicationTreeView.ShowNodeToolTips = true;

            if (_applications.Count == 0)
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Enabled = false;
                }
            }
        }

        private void PopulateApplicationsComboBox()
        {
            ApplicationsComboBox.Items.Clear();
            List<string> applicationList = new List<string>();
            foreach (KeyValuePair<string, ApplicationXMLModel> application in _applications)
            {
                ApplicationsComboBox.Items.Add(application.Key);
            }
            ApplicationsComboBox.SelectedIndex = 0;
        }

        private void PopulateRolesComboBox(string anApplication)
        {
            List<string> rolesList = new List<string>();
            rolesList = AzManReader.ReadRoles(anApplication);
            if (rolesList != null)
            {
                RolesComboBox.Items.Clear();
                RolesComboBox.Items.AddRange((rolesList.ToArray()));
                if (rolesList.Count == 0)
                {
                    RolesComboBox.Items.Add(_noRolesString);
                    RolesComboBox.SelectedItem = 0;
                }
                else
                {
                }
            }
        }

        private void PopulateWinUsersComboBox(string application, string role)
        {

            if (!role.Equals(_noRolesString))
            {
                List<string> winUsersList = new List<string>();
                winUsersList = AzManReader.ReadWinUsers(application, role);
                if (winUsersList != null)
                {
                    WinUserlistBox.Items.Clear();
                    WinUserlistBox.Items.AddRange(winUsersList.ToArray());
                    if (winUsersList.Count == 0)
                    {
                        WinUserlistBox.Items.Add(_noWinUsersString);
                        WinUserlistBox.SelectedItem = 0;
                    }
                    else
                    {
                        WinUserlistBox.SelectedItem = WinUserlistBox.Items[0];
                    }
                }
                else
                {
                    WinUserlistBox.Items.Add(_noWinUsersString);
                    WinUserlistBox.SelectedItem = WinUserlistBox.Items[0];
                }
            }
        }

        private void LoadTreeView(string anApplicationName)
        {
            TreeNode treeNode = MenuItemToTreeViewTranslator.Translate(MetaDataParser.Applications[anApplicationName].MenuItems);
            if (treeNode != null)
            {
                ApplicationTreeView.Nodes.Clear();
                LoadRoleActionsIntoTreeNode(treeNode);
                ApplicationTreeView.Nodes.Add(treeNode);
            }
        }

        private void LoadRoleActionsIntoTreeNode(TreeNode aTreeNode)
        {
            string currentRole;
            if (RolesComboBox.SelectedItem == null)
            {
                currentRole = RolesComboBox.Items[0].ToString();
            }
            else
            {
                currentRole = GetSelectedRole();
            }
            List<string> operationsList = AzManReader.ReadOperationsRole(GetSelectedApplication(), currentRole);
            if (operationsList != null)
            {
                OperationLoader.LoadIntoTreeNode(aTreeNode, operationsList);
            }
            List<string> allTreeOperations = TreeViewToOperationsListTranslator.GetAllNodeOperations(GetRootNode());
            duplicateKeys = allTreeOperations.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
        }

        private void treeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current 
                    Checked value of the TreeNode whose checked state changed. */
                    ProcessNode(e.Node, !e.Node.Checked);
                    e.Node.Checked = !e.Node.Checked;

                } //Check all duplicate operations
                var operations = (List<UXAction>)e.Node.Tag;

                foreach (UXAction operation in operations)
                {
                    if (duplicateKeys.Contains(operation.Identity))
                    {
                        e.Node.Checked = !e.Node.Checked;
                        //Put all nodes with the same ID in the same checked status
                        CheckNodeWithID(GetRootNode(), operation.Identity);
                    }
                }
            }
            isRoleSaved = false;
        }

        /// <summary>
        /// Method used to check/uncheck all child nodes.
        /// </summary>
        /// <param name="aTreeNode">All child nodes of this node will be put in the aStatus</param>
        /// <param name="aStatus"></param>
        private void ProcessNode(TreeNode aTreeNode, bool aStatus)
        {
            if (aTreeNode != null)
            {
                aTreeNode.Checked = aStatus;
                foreach (TreeNode node in aTreeNode.Nodes)
                {
                    ProcessNode(node, aStatus);
                }
                //Check all duplicates that have duplicates checked.
                CheckDuplicates(aTreeNode);
            }
        }

        private void CheckDuplicates(TreeNode inNode)
        {
            foreach (TreeNode node in inNode.Nodes)
            {
                    var operations = (List<UXAction>)node.Tag;
                    if (operations != null)
                    {
                        foreach (UXAction operation in operations)
                        {
                            if (duplicateKeys.Contains(operation.Identity))
                            {
                                CheckNodeWithID(GetRootNode(), operation.Identity, node.Checked);
                            }
                        }
                    }
                CheckDuplicates(node);
            }
        }

        private void CheckNodeWithID(TreeNode node, string operation)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Tag != null)
                {
                    foreach (UXAction action in (List<UXAction>)child.Tag)
                    {
                        if (action.Identity.Equals(operation))
                        {
                            child.Checked = !child.Checked;
                        }
                    }
                }
                CheckNodeWithID(child, operation);
            }
        }

        private void CheckNodeWithID(TreeNode node, string operation, bool status)
        {
            foreach (TreeNode child in node.Nodes)
            {
                if (child.Tag != null)
                {
                    foreach (UXAction action in (List<UXAction>)child.Tag)
                    {
                        if (action.Identity.Equals(operation))
                        {
                            child.Checked = status;
                        }
                    }
                }
                CheckNodeWithID(child, operation, status);
            }
        }



        private void ApplicationsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string currentApplication = GetSelectedApplication();
            string oldApplication = _previousApplication;
            string oldRole = _previousRole;
            TreeNode node = null;
            if (ApplicationTreeView.Nodes.Count > 0)
            {
                node = GetRootNode();
            }

            SwitchApplication(currentApplication, oldApplication, oldRole, node);

            _previousApplication = GetSelectedApplication();
            RemoveCheckboxes(GetRootNode());

            RolegroupBox.Text = "Role for Application [" + currentApplication + "]";
            AllNodescheckBox.Checked = false;
            ApplicationTreeView.CollapseAll();
        }

        private void AllNodescheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ProcessNode(GetRootNode(), AllNodescheckBox.Checked);
            RemoveCheckboxes(GetRootNode());
        }

        private TreeNode GetRootNode()
        {
            if (ApplicationTreeView.Nodes.Count > 0)
            {
                return ApplicationTreeView.Nodes[0];
            }
            else
            {
                return null;
            }
        }

        private bool HasRoles()
        {
            if (RolesComboBox.SelectedIndex == -1)
                return false;
            else if (GetSelectedRole() == _noRolesString)
                return false;
            else
                return true;
        }

        private void RolesComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (HasRoles())
            {
                string currentRole = GetSelectedRole();
                if (currentRole == "Administrator")
                {
                    ApplicationTreeView.CollapseAll();
                    ApplicationTreeView.Enabled = false;
                    btnSave.Enabled = false;
                    AllNodescheckBox.Enabled = false;
                    btnDeleteRole.Enabled = false;
                    btnExpandAll.Enabled = false;
                }
                else
                {
                    ApplicationTreeView.Enabled = true;
                    btnSave.Enabled = true;
                    AllNodescheckBox.Enabled = true;
                    btnDeleteRole.Enabled = true;
                    btnExpandAll.Enabled = true;
                }
                string oldRole = _previousRole;
                string oldApplication = _previousApplication;

                TreeNode node = GetRootNode();

                SwitchRole(currentRole, GetSelectedApplication(), oldRole, oldApplication, node);

                _previousRole = currentRole;

                AccessibilitygroupBox.Text = "Role [" + currentRole + "] Settings";
                lblWinUsers.Text = "Windows Users connected to [" + currentRole + "]";
            }
            else
            {
                _previousRole = null;
            }
            AllNodescheckBox.Checked = false;
            ApplicationTreeView.CollapseAll();
            isRoleSaved = true;
        }

        private void LoadTreeViewApplication(string anApplication)
        {
            TreeNode treeNode = MenuItemToTreeViewTranslator.Translate(MetaDataParser.Applications[anApplication].MenuItems);
            if (treeNode != null)
            {
                ApplicationTreeView.Nodes.Clear();
                ApplicationTreeView.Nodes.Add(treeNode);
            }
        }

        private void SwitchApplication(string anApplication, string anOldApplication, string anOldRole, TreeNode anOldTreeNode)
        {
            PopulateRolesComboBox(anApplication);
            if (RolesComboBox.SelectedItem != null)
            {
                PopulateWinUsersComboBox(anApplication, GetSelectedRole());
            }
            else
            {
                WinUserlistBox.Items.Clear();
                WinUserlistBox.Items.Add(_noWinUsersString);
                WinUserlistBox.SelectedItem = WinUserlistBox.Items[0];
            }
            //save the role in old application 
            LoadTreeViewApplication(anApplication);
            //change to first role in list, if not empty
            Object selectedRoleItem = RolesComboBox.SelectedItem;
            string selectedRole = null;
            if (selectedRoleItem != null)
            {
                selectedRole = selectedRoleItem.ToString();
            }
            if (string.IsNullOrEmpty(selectedRole) || !HasRoles())
            {
                if (RolesComboBox.Items.Count > 0)
                {
                    RolesComboBox.SelectedItem = RolesComboBox.Items[0];
                }
            }
        }

        private void SaveRole(string aRole, string anApplication, TreeNode aTreeNode)
        {
            List<string> selectedOperations = TreeViewToOperationsListTranslator.Translate(aTreeNode);
            List<string> allTreeOperations = TreeViewToOperationsListTranslator.GetAllNodeOperations(GetRootNode());
            AzManWriter.SaveRole(selectedOperations, aRole, anApplication, allTreeOperations);
            isRoleSaved = true;
        }

        private void SwitchRole(string aRole, string anApplication, string anOldRole, string anOldApplication, TreeNode aTreeNode)
        {
            PopulateWinUsersComboBox(anApplication, aRole);

            //töm settings/checks på existerande träd
            ProcessNode(GetRootNode(), false);

            //Ladda inställningarna för den nya rollen in i aTreeNode
            LoadRoleActionsIntoTreeNode(aTreeNode);
            RemoveCheckboxes(GetRootNode());

            string selectedWinUser = null;
            if (WinUserlistBox.SelectedItem != null)
            {
                selectedWinUser = GetSelectedUser();
            }
            if (string.IsNullOrEmpty(selectedWinUser) || selectedWinUser == _noWinUsersString)
            {
                if (WinUserlistBox.Items.Count > 0)
                {
                    WinUserlistBox.SelectedItem = WinUserlistBox.Items[0];
                }
            }
            isRoleSaved = true;
        }

        /// <summary>
        /// 
        /// Checkboxes that do not have a tooltip do not have an action and therefore cannot be locked.
        /// </summary>
        /// <param name="inputNode">Rootnode</param>
        private void RemoveCheckboxes(TreeNode inputNode)
        {
            if (inputNode != null)
            {
                if (string.IsNullOrEmpty(inputNode.ToolTipText))
                {
                    NativeMethods.HideCheckBox(ApplicationTreeView, inputNode);
                }
                foreach (TreeNode node in inputNode.Nodes)
                {
                    RemoveCheckboxes(node);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasRoles())
            {
                MessageBox.Show("There are no roles for this application.");
            }
            else
            {
                SaveRole(GetSelectedRole(), GetSelectedApplication(), GetRootNode());
            }
            AllNodescheckBox.Checked = false;
            LoadRoleActionsIntoTreeNode(GetRootNode());
            RemoveCheckboxes(GetRootNode());
        }

        private void btnExpandAll_Click(object sender, EventArgs e)
        {
            if (isTreeViewExpanded)
            {
                ApplicationTreeView.CollapseAll();
                isTreeViewExpanded = false;
                if (GetRootNode() != null) { GetRootNode().Expand(); }
            }
            else
            {
                ApplicationTreeView.ExpandAll();
                isTreeViewExpanded = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Hide checkboxes when form is done loading.
            RemoveCheckboxes(GetRootNode());
        }

        private void btnCreateRole_Click(object sender, EventArgs e)
        {
            string roleName = Microsoft.VisualBasic.Interaction.InputBox("Please enter a role name", "New Role", "Role", -1, -1);
            if (!string.IsNullOrEmpty(roleName))
            {
                if (roleName != "Administrator")
                {
                    bool success = AzManWriter.CreateRole(roleName, GetSelectedApplication());
                    if (success)
                    {
                        PopulateRolesComboBox(GetSelectedApplication());
                        //Put the new role as the selected role.
                        RolesComboBox.SelectedIndex = RolesComboBox.Items.Count - 1;
                        MessageBox.Show("Role successfully created!", "", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Administrator role already exist.", "", MessageBoxButtons.OK);
                }
            }
            AllNodescheckBox.Checked = false;
            ApplicationTreeView.CollapseAll();
        }

        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            if (RolesComboBox.SelectedItem.ToString() != "Administrator")
            {
                if (!HasRoles())
                {
                    MessageBox.Show("There are no roles to delete for this application.");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you really want to delete " + GetSelectedRole() + "?", "Delete Role", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        bool success = AzManWriter.DeleteRole(GetSelectedRole(), GetSelectedApplication());
                        if (success)
                        {
                            PopulateRolesComboBox(GetSelectedApplication());

                            RolesComboBox.SelectedIndex = 0;
                            PopulateWinUsersComboBox(GetSelectedApplication(), GetSelectedRole());
                            MessageBox.Show("Role successfully deleted!", "", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Could not delete role.", "", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("You can not delete the Administrator role.", "", MessageBoxButtons.OK);
            }
        }

        private void btnAddWinUser_Click(object sender, EventArgs e)
        {
            if (!HasRoles())
            {
                MessageBox.Show("There are no roles for this application.");
            }
            else
            {
                string windowsUser = Microsoft.VisualBasic.Interaction.InputBox("Please enter a Windows username", "Connect Windows User to Role", "Username", -1, -1);
                if (!string.IsNullOrEmpty(windowsUser))
                {
                    bool success = AzManWriter.AddWindowsUserToRole(GetSelectedRole(), GetSelectedApplication(), windowsUser);
                    if (success)
                    {
                        PopulateWinUsersComboBox(GetSelectedApplication(), GetSelectedRole());
                        MessageBox.Show("Added " + windowsUser + "  to " + GetSelectedRole() + ".", "", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Could not bind " + windowsUser + " to " + GetSelectedRole() + ". Please check so you spelled the username correctly.", "Could not add user to role", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void btnDelWinUser_Click(object sender, EventArgs e)
        {
            if (!HasRoles())
            {
                MessageBox.Show("There are no roles for this application.");
            }
            else if (GetSelectedUser().Equals(_noWinUsersString))
            {
                MessageBox.Show("There are no users for this role.");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you really want to disconnect " + GetSelectedUser() + "?", "Disconnect Windows User from Role", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    bool success = AzManWriter.DeleteWindowsUserFromRole(GetSelectedRole(), GetSelectedApplication(), GetSelectedUser());
                    if (success)
                    {
                        PopulateWinUsersComboBox(GetSelectedApplication(), GetSelectedRole());
                        WinUserlistBox.SelectedIndex = 0;
                        MessageBox.Show("user successfully disconnected!", "", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Could not delete user.", "", MessageBoxButtons.OK);
                    }
                }
            }
        }
        private string GetSelectedRole()
        {
            return RolesComboBox.SelectedItem.ToString();
        }
        private string GetSelectedApplication()
        {
            return ApplicationsComboBox.SelectedItem.ToString();
        }
        private string GetSelectedUser()
        {
            return WinUserlistBox.SelectedItem.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRoleSaved == false)
            {
                if (MessageBox.Show("Are you sure you want to close?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnShowUserRoles_Click(object sender, EventArgs e)
        {
            if (WinUserlistBox.SelectedItem != null)
            {
                if (WinUserlistBox.SelectedItem.ToString() != _noWinUsersString)
                {
                    string selectedUser = WinUserlistBox.SelectedItem.ToString();
                    List<string> selectedUserRoles = new List<string>();
                    List<string> list = AzManReader.ReadRoles(GetSelectedApplication());
                    if (list != null)
                    {
                        foreach (string role in list)
                        {
                            List<string> userRoles = AzManReader.ReadUserRoles(GetSelectedApplication(), role);
                            if (userRoles != null)
                            {
                                foreach (string user in userRoles)
                                {
                                    if (user.Equals(selectedUser))
                                    {
                                        selectedUserRoles.Add(role);
                                    }
                                }
                            }
                        }
                        var message = string.Join(Environment.NewLine, selectedUserRoles.ToArray());
                        MessageBox.Show(null, selectedUser + " belong to: \n" + message, "Show User's Roles", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("No user selected.");
                    }
                }
            }
        }

        private void ApplicationTreeView_Leave(object sender, EventArgs e)
        {
            if (isRoleSaved == false)
            {
                DialogResult dialogResult = MessageBox.Show("Do you wish to save these role settings?", "Save Role Settings", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveRole(GetSelectedRole(), GetSelectedApplication(), GetRootNode());
                }
                else
                {
                    isRoleSaved = true;
                    //Load Role settings
                    AllNodescheckBox.Checked = false;

                    //Ladda inställningarna för den nya rollen in i aTreeNode
                    LoadRoleActionsIntoTreeNode(GetRootNode());
                    RemoveCheckboxes(GetRootNode());
                }
            }
        }
    }
}