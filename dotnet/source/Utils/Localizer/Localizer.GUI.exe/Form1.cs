using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Imi.Utils.Localizer.GUI
{
    public partial class Form1 : Form
    {
        private Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>>> masterData;
        public Form1()
        {
            InitializeComponent();
        }

        private void loadMasterDataToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void smartClientToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ChangedResourcestreeView.Tag = "resx";

            LoadMasterData("resx", false);

            toolStripComboBoxImpLang.Items.Clear();
            toolStripComboBoxImpLang.Items.AddRange(Imi.Utils.Localizer.Localizer.GetSupportedTranslationsCultureNames("resx").ToArray());
        }

        private void LoadMasterData(string resourceType, bool reload)
        {
            ClearControls();

            if (masterData == null || reload)
            {
                masterData = Imi.Utils.Localizer.Localizer.GetMasterData();
            }

            if (masterData.ContainsKey(resourceType))
            {
                LoadStructureTree(resourceType);
                LoadChangeTree(resourceType);
            }
        }

        private void LoadChangeTree(string resourceType)
        {
            LoadTreeView(resourceType, ChangedResourcestreeView, false, true);
        }

        private void LoadStructureTree(string resourceType)
        {
            LoadTreeView(resourceType, StructuretreeView, true, false);
        }

        private void LoadTreeView(string resourceType, TreeView treeView, bool setXmlNodeInTagForAllNodes, bool onlyChanges)
        {
            treeView.Nodes.Clear();

            treeView.Nodes.Add("Modules");

            foreach (string moduleName in masterData[resourceType].Value.Keys)
            {
                if (onlyChanges)
                {
                    if (!Convert.ToBoolean(masterData[resourceType].Value[moduleName].Key.Attributes["Translate"].Value))
                    {
                        continue;
                    }
                }

                foreach (string resourceContainerKey in masterData[resourceType].Value[moduleName].Value.Keys)
                {
                    foreach (string resourceKey in masterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value.Keys)
                    {
                        KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource = masterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey];

                        if (onlyChanges)
                        {
                            if (!Convert.ToBoolean(resource.Key.Attributes["Translate"].Value))
                            {
                                continue;
                            }
                        }

                        if ((onlyChanges && !string.IsNullOrEmpty(resource.Key.Attributes["Change"].Value) && ((!handleDeletesToolStripMenuItem.Checked && !resource.Key.Attributes["Change"].Value.Contains("Deleted")) || handleDeletesToolStripMenuItem.Checked)) || !onlyChanges)
                        {
                            TreeNode moduleNode;
                            if (!treeView.Nodes[0].Nodes.ContainsKey(moduleName))
                            {
                                moduleNode = treeView.Nodes[0].Nodes.Add(moduleName, moduleName);

                                if (setXmlNodeInTagForAllNodes)
                                {
                                    moduleNode.Tag = masterData[resourceType].Value[moduleName].Key;
                                }
                            }
                            else
                            {
                                moduleNode = treeView.Nodes[0].Nodes[moduleName];
                            }

                            TreeNode resourceContainerNode;
                            if (!moduleNode.Nodes.ContainsKey(resourceContainerKey))
                            {
                                resourceContainerNode = moduleNode.Nodes.Add(resourceContainerKey, resourceContainerKey);

                                if (setXmlNodeInTagForAllNodes)
                                {
                                    resourceContainerNode.Tag = masterData[resourceType].Value[moduleName].Value[resourceContainerKey].Key;
                                }
                            }
                            else
                            {
                                resourceContainerNode = moduleNode.Nodes[resourceContainerKey];
                            }

                            TreeNode resourceNode = resourceContainerNode.Nodes.Add(resourceKey, resourceKey);
                            if (setXmlNodeInTagForAllNodes)
                            {
                                resourceNode.Tag = resource.Key;
                            }
                            else
                            {
                                resourceNode.Tag = resource;
                            }
                        }
                    }
                }
            }
        }

        private void FindSuggestedTranslations(string text, string currentResourceKey, string resourceType)
        {
            Dictionary<string, int> columnDictionary = new Dictionary<string, int>();
            SuggestedTranslationslistView.Items.Clear();
            SuggestedTranslationslistView.Columns.Clear();
            SuggestedTranslationslistView.Clear();
            SuggestedTranslationslistView.Columns.Add(Localizer.GetDefaultCultureName(resourceType));

            foreach (string moduleName in masterData[resourceType].Value.Keys)
            {
                foreach (string resourceContainerKey in masterData[resourceType].Value[moduleName].Value.Keys)
                {
                    foreach (string resourceKey in masterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value.Keys)
                    {
                        KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource = masterData[resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourceKey];

                        if (resourceKey != currentResourceKey)
                        {
                            if (resource.Value[Localizer.GetDefaultCultureName(resourceType)].InnerText == text)
                            {
                                ListViewItem newItem = SuggestedTranslationslistView.Items.Add(resource.Value[Localizer.GetDefaultCultureName(resourceType)].InnerText);

                                Dictionary<string, ListViewItem.ListViewSubItem> translations = new Dictionary<string, ListViewItem.ListViewSubItem>();

                                foreach (KeyValuePair<string, XmlNode> translation in resource.Value)
                                {
                                    if (translation.Key != Localizer.GetDefaultCultureName(resourceType))
                                    {
                                        if (!columnDictionary.ContainsKey(translation.Key))
                                        {
                                            columnDictionary.Add(translation.Key, columnDictionary.Count + 1);
                                            SuggestedTranslationslistView.Columns.Add(translation.Key);
                                        }


                                        ListViewItem.ListViewSubItem newSubItem = new ListViewItem.ListViewSubItem(newItem, translation.Value.InnerText);
                                        translations.Add(translation.Key, newSubItem);
                                    }
                                }

                                foreach (KeyValuePair<string, int> column in columnDictionary)
                                {
                                    if (translations.ContainsKey(column.Key))
                                    {
                                        newItem.SubItems.Add(translations[column.Key]);
                                    }
                                    else
                                    {
                                        newItem.SubItems.Add(string.Empty);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ChangedResourcestreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void ChangedResourcestreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                ShowSelectedRecourceForChange((KeyValuePair<XmlNode, Dictionary<string, XmlNode>>)e.Node.Tag);
            }
            else
            {
                ClearControls();
            }
        }

        private void ShowSelectedRecourceForChange(KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource)
        {
            ResourcelistView.Items.Clear();

            ResourceKeytextBox.Text = resource.Key.Attributes["Key"].Value;
            ResourceUpdatedtextBox.Text = Convert.ToDateTime(resource.Key.Attributes["Updated"].Value).ToString("yyyy-MM-dd hh:mm:ss");
            ResourceTranslatetextBox.Text = resource.Key.Attributes["Translate"].Value;
            ResourceChangetextBox.Text = resource.Key.Attributes["Change"].Value;

            foreach (KeyValuePair<string, XmlNode> translation in resource.Value)
            {
                ListViewItem newItem = ResourcelistView.Items.Add(translation.Key);
                newItem.SubItems.Add(translation.Value.InnerText);
            }

            if (!resource.Key.Attributes["Change"].Value.Contains("Deleted"))
            {
                UndoDeletebutton.Visible = false;
                string resourceType = ChangedResourcestreeView.Tag.ToString();
                FindSuggestedTranslations(resource.Value[Localizer.GetDefaultCultureName(resourceType)].InnerText, resource.Key.Attributes["Key"].Value, resourceType);
            }
            else
            {
                UndoDeletebutton.Visible = true;
                SuggestedTranslationslistView.Clear();
            }
        }

        private void useTranslationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SuggestedTranslationslistView.SelectedItems.Count == 1)
            {
                if (ChangedResourcestreeView.SelectedNode != null && ChangedResourcestreeView.SelectedNode.Tag != null)
                {
                    string resourceType = ChangedResourcestreeView.Tag.ToString();

                    KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource = (KeyValuePair<XmlNode, Dictionary<string, XmlNode>>)ChangedResourcestreeView.SelectedNode.Tag;

                    KeyValuePair<string, XmlNode> defaultTranslation = new KeyValuePair<string, XmlNode>();

                    foreach (KeyValuePair<string, XmlNode> translation in resource.Value)
                    {
                        if (translation.Key != Localizer.GetDefaultCultureName(resourceType))
                        {
                            resource.Key.RemoveChild(translation.Value);
                        }
                        else
                        {
                            defaultTranslation = new KeyValuePair<string, XmlNode>(translation.Key, translation.Value);
                        }
                    }

                    resource.Value.Clear();
                    resource.Value.Add(defaultTranslation.Key, defaultTranslation.Value);


                    for (int index = 0; index < SuggestedTranslationslistView.Columns.Count; index++)
                    {
                        if (SuggestedTranslationslistView.Columns[index].Text != Localizer.GetDefaultCultureName(resourceType))
                        {
                            string text;
                            string cultureName = SuggestedTranslationslistView.Columns[index].Text;

                            text = SuggestedTranslationslistView.SelectedItems[0].SubItems[index].Text;

                            XmlElement newTranslation = resource.Key.OwnerDocument.CreateElement("Text");
                            newTranslation.SetAttribute("CultureName", cultureName);
                            newTranslation.InnerText = text;

                            resource.Key.AppendChild(newTranslation);
                            resource.Value.Add(cultureName, newTranslation);
                        }
                    }

                    if (!resource.Key.Attributes["Change"].Value.Contains("("))
                    {
                        resource.Key.Attributes["Change"].Value += "_(Updated)";
                    }

                    ShowSelectedRecourceForChange(resource);
                }
            }
        }

        private void UndoDeletebutton_Click(object sender, EventArgs e)
        {
            if (ChangedResourcestreeView.SelectedNode.Tag != null)
            {
                KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource = (KeyValuePair<XmlNode, Dictionary<string, XmlNode>>)ChangedResourcestreeView.SelectedNode.Tag;
                if (resource.Key.Attributes["Change"].Value.Contains("Deleted"))
                {
                    if (!resource.Key.Attributes["Change"].Value.Contains("("))
                    {
                        resource.Key.Attributes["Change"].Value += "_(Undo)";
                    }
                }

                ShowSelectedRecourceForChange(resource);
            }
        }

        private void confirmChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Localizer.ConfirmChangesFromGUI(ChangedResourcestreeView.Tag.ToString(), masterData, false);

            LoadMasterData(ChangedResourcestreeView.Tag.ToString(), true);
        }


        private void ClearControls()
        {
            SuggestedTranslationslistView.Clear();
            ResourcelistView.Items.Clear();
            ResourceKeytextBox.Text = string.Empty;
            ResourceUpdatedtextBox.Text = string.Empty;
            ResourceTranslatetextBox.Text = string.Empty;
            ResourceChangetextBox.Text = string.Empty;
        }

        private void StructuretreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PropertiesKeytextBox.Text = string.Empty;
            PropertiesTranslatecheckBox.Enabled = false;
            PropertiesTranslatecheckBox.Checked = false;
            PropertiesTexttextBox.Text = string.Empty;

            if (e.Node.Tag != null)
            {
                string resourceType = ChangedResourcestreeView.Tag.ToString();
                XmlNode selectedXmlNode = (XmlNode)e.Node.Tag;

                if (selectedXmlNode.Name == "Module")
                {
                    PropertiesKeytextBox.Text = selectedXmlNode.Attributes["Name"].Value;
                }
                else
                {
                    PropertiesKeytextBox.Text = selectedXmlNode.Attributes["Key"].Value;
                }

                if (selectedXmlNode.Name == "Module" || selectedXmlNode.Name == "Resource")
                {
                    PropertiesTranslatecheckBox.Checked = Convert.ToBoolean(selectedXmlNode.Attributes["Translate"].Value);
                    PropertiesTranslatecheckBox.Enabled = true;
                }
                else
                {
                    PropertiesTranslatecheckBox.Enabled = false;
                }

                if (selectedXmlNode.Name == "Resource")
                {
                    foreach (XmlNode childNode in selectedXmlNode.ChildNodes)
                    {
                        if (childNode.Attributes["CultureName"].Value == Localizer.GetDefaultCultureName(resourceType))
                        {
                            PropertiesTexttextBox.Text = childNode.InnerText;
                        }
                    }
                }
            }
        }

        private void PropertiesTranslatecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (PropertiesTranslatecheckBox.Enabled)
            {
                if (StructuretreeView.SelectedNode.Tag != null)
                {
                    XmlNode selectedXmlNode = (XmlNode)StructuretreeView.SelectedNode.Tag;

                    if (selectedXmlNode.Attributes["Translate"].Value != PropertiesTranslatecheckBox.Checked.ToString())
                    {
                        selectedXmlNode.Attributes["Translate"].Value = PropertiesTranslatecheckBox.Checked.ToString();
                    }
                }
            }
        }

        private bool excelIsClosed;
        private object[,] excelData;

        private void EditInExcel(string resourceType)
        {
            if (masterData != null)
            {
                Dictionary<string, int> cellDictionary = new Dictionary<string, int>();
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = false;
                app.DisplayAlerts = false;
                try
                {
                    Workbook workbook = null;
                    workbook = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

                    try
                    {
                        this.Cursor = Cursors.WaitCursor;

                        //Create DataArray
                        //===================================================================================================================================================================================
                        int numberOfRows = 2;
                        int numberOfCols = -1;

                        foreach (string rt in masterData.Keys)
                        {
                            if (rt == resourceType)
                            {
                                foreach (string moduleName in masterData[rt].Value.Keys)
                                {
                                    if (Convert.ToBoolean(masterData[rt].Value[moduleName].Key.Attributes["Translate"].Value))
                                    {
                                        foreach (string resourceKey in masterData[rt].Value[moduleName].Value.Keys)
                                        {
                                            int numOfTransKeys = masterData[rt].Value[moduleName].Value[resourceKey].Value.Where(k => Convert.ToBoolean(k.Value.Key.Attributes["Translate"].Value)).Count();

                                            numberOfRows += numOfTransKeys;

                                            //Find number of columns. Only done once
                                            if (numberOfCols == -1)
                                            {
                                                if (numOfTransKeys == masterData[rt].Value[moduleName].Value[resourceKey].Value.Count)
                                                {
                                                    numberOfCols = masterData[rt].Value[moduleName].Value[resourceKey].Value.First().Value.Value.Count;
                                                    numberOfCols += 5; //Add overhead columns
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        object[,] data = new object[numberOfRows, numberOfCols];



                        //===================================================================================================================================================================================



                        Worksheet sheet = (Worksheet)workbook.Worksheets.get_Item(1);

                        sheet.Activate();
                        sheet.Application.ActiveWindow.SplitRow = 1;
                        sheet.Application.ActiveWindow.SplitColumn = 6;
                        sheet.Application.ActiveWindow.FreezePanes = true;

                        sheet.Cells[1, 5].EntireColumn.ColumnWidth = 15;

                        sheet.Cells[1, 6].EntireColumn.NumberFormat = "@";
                        sheet.Cells[1, 6].EntireColumn.WrapText = true;
                        sheet.Cells[1, 6].EntireColumn.ColumnWidth = 40;

                        data[0, 0] = "ResourceType";
                        data[0, 1] = "Module";
                        data[0, 2] = "ResourceKey";
                        data[0, 3] = "Key";
                        data[0, 4] = "Updated";
                        data[0, 5] = "text";

                        //sheet.Cells[1, 1] = "ResourceType";
                        //sheet.Cells[1, 2] = "Module";
                        //sheet.Cells[1, 3] = "ResourceKey";
                        //sheet.Cells[1, 4] = "Key";
                        //sheet.Cells[1, 5] = "Updated";
                        //sheet.Cells[1, 6] = "text";

                        int colIndex = 7;
                        Dictionary<string, int> translationDictionary = new Dictionary<string, int>();
                        foreach (string cultureName in Localizer.GetSupportedTranslationsCultureNames(resourceType))
                        {
                            sheet.Cells[1, colIndex].EntireColumn.ColumnWidth = 40;
                            sheet.Cells[1, colIndex].EntireColumn.NumberFormat = "@";
                            //sheet.Cells[1, colIndex] = cultureName;
                            data[0, colIndex - 1] = cultureName;

                            translationDictionary.Add(cultureName, colIndex - 1);

                            colIndex++;
                        }

                        sheet.Cells.Locked = false;
                        sheet.Range["1:1"].EntireRow.Locked = true;
                        sheet.Range["1:1"].EntireRow.Font.Bold = true;



                        //int insertIndex = Math.Max(sheet.UsedRange.Rows.Count, 1);
                        int insertIndex = 1;


                        foreach (string rt in masterData.Keys)
                        {
                            if (rt == resourceType)
                            {
                                foreach (string moduleName in masterData[rt].Value.Keys)
                                {
                                    if (Convert.ToBoolean(masterData[rt].Value[moduleName].Key.Attributes["Translate"].Value))
                                    {
                                        foreach (string resourceKey in masterData[rt].Value[moduleName].Value.Keys)
                                        {
                                            foreach (string key in masterData[rt].Value[moduleName].Value[resourceKey].Value.Keys)
                                            {
                                                if (Convert.ToBoolean(masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Translate"].Value))
                                                {
                                                    string updated = masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Updated"].Value;

                                                    data[insertIndex, 0] = rt;
                                                    data[insertIndex, 1] = moduleName;
                                                    data[insertIndex, 2] = resourceKey;
                                                    data[insertIndex, 3] = key;
                                                    data[insertIndex, 4] = updated;


                                                    //sheet.Cells[insertIndex, 1] = rt;
                                                    //sheet.Cells[insertIndex, 2] = moduleName;
                                                    //sheet.Cells[insertIndex, 3] = resourceKey;
                                                    //sheet.Cells[insertIndex, 4] = key;
                                                    //sheet.Cells[insertIndex, 5] = updated;

                                                    foreach (KeyValuePair<string, XmlNode> translation in masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Value)
                                                    {
                                                        if (translation.Key == Localizer.GetDefaultCultureName(rt))
                                                        {
                                                            //sheet.Cells[insertIndex, 6] = translation.Value.InnerText;
                                                            data[insertIndex, 5] = translation.Value.InnerText;
                                                        }
                                                        else
                                                        {
                                                            //sheet.Cells[insertIndex, translationDictionary[translation.Key]] = translation.Value.InnerText;
                                                            data[insertIndex, translationDictionary[translation.Key]] = translation.Value.InnerText;
                                                        }
                                                    }

                                                    insertIndex++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Range startCell = (Range)sheet.Cells[1, 1];
                        Range endCell = (Range)sheet.Cells[numberOfRows, numberOfCols];
                        Range writeRange = sheet.Range[startCell, endCell];

                        writeRange.Value2 = data;

                        //string tmpFileName = "TranslationWorkSheet_" + Guid.NewGuid().ToString() + ".xlsx";
                        //string tmpFilePath = (Path.Combine(System.Windows.Forms.Application.ExecutablePath, tmpFileName));

                        //workbook.SaveAs(tmpFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                        excelIsClosed = false;

                        Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeCloseEventHandler Event_BeforeBookClose;

                        Event_BeforeBookClose = new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeCloseEventHandler(app_WorkbookBeforeClose);
                        app.WorkbookBeforeClose += Event_BeforeBookClose;

                        app.Visible = true;

                        //Wait until excel is closed
                        while (!excelIsClosed)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }

                        if (excelData != null)
                        {
                            UpdateMasterFromExcelData(excelData);
                        }

                        this.Cursor = Cursors.Default;

                        Marshal.FinalReleaseComObject(sheet);
                        sheet = null;


                    }
                    finally
                    {

                        Marshal.FinalReleaseComObject(workbook);
                        workbook = null;
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Marshal.FinalReleaseComObject(app);
                    app = null;
                    GC.Collect();
                }
            }
        }

        private void UpdateMasterFromExcelData(object[,] data)
        {

            Dictionary<int, string> Languages = new Dictionary<int, string>();

            if (data.GetLength(0) > 1)
            {
                for (int i = 7; i <= data.GetLength(1); i++)
                {
                    if (data[1, i] != null && !string.IsNullOrEmpty(data[1, i].ToString()))
                    {
                        Languages.Add(i, data[1, i].ToString());
                    }
                }

                for (int i = 2; i <= data.GetLength(0); i++)
                {
                    if ((data[i, 1] != null) && data[i, 4] != null && !string.IsNullOrEmpty(data[i, 4].ToString().Trim()))
                    {
                        string tmp_resourceType = data[i, 1].ToString();
                        string moduleName = data[i, 2].ToString();
                        string resourceContainerKey = data[i, 3].ToString();
                        string resourcekey = data[i, 4].ToString();
                        DateTime Updated = DateTime.Now;
                        DateTime.TryParse(data[i, 5].ToString(), out Updated);
                        string text = data[i, 6].ToString();


                        Dictionary<string, string> translations = new Dictionary<string, string>();

                        foreach (KeyValuePair<int, string> language in Languages)
                        {
                            translations.Add(language.Value, data[i, language.Key] == null ? "" : data[i, language.Key].ToString());
                        }


                        if (translations != null)
                        {
                            if (masterData.ContainsKey(tmp_resourceType))
                            {
                                if (masterData[tmp_resourceType].Value.ContainsKey(moduleName))
                                {
                                    if (masterData[tmp_resourceType].Value[moduleName].Value.ContainsKey(resourceContainerKey))
                                    {
                                        if (masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value.ContainsKey(resourcekey))
                                        {
                                            bool changed = false;

                                            foreach (KeyValuePair<string, string> translation in translations)
                                            {
                                                if (masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.ContainsKey(translation.Key))
                                                {
                                                    string currentText = masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value[translation.Key].InnerText;

                                                    if (currentText != translation.Value)
                                                    {
                                                        masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value[translation.Key].InnerText = translation.Value;
                                                        changed = true;
                                                    }
                                                }
                                                else
                                                {
                                                    XmlElement newTranslation = masterData[tmp_resourceType].Key.OwnerDocument.CreateElement("Text");
                                                    newTranslation.SetAttribute("CultureName", translation.Key);
                                                    newTranslation.InnerText = translation.Value;

                                                    masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.AppendChild(newTranslation);
                                                    masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Value.Add(translation.Key, newTranslation);

                                                    changed = true;
                                                }
                                            }

                                            if (changed)
                                            {
                                                masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Updated"].Value = DateTime.Now.ToString();
                                                if (!masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Change"].Value.Contains("Deleted") && !masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Change"].Value.Contains("("))
                                                {
                                                    masterData[tmp_resourceType].Value[moduleName].Value[resourceContainerKey].Value[resourcekey].Key.Attributes["Change"].Value += "_(Updated)";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void app_WorkbookBeforeClose(Microsoft.Office.Interop.Excel.Workbook wb, ref bool cancel)
        {
            Worksheet sheet = (Worksheet)wb.Worksheets.get_Item(1);
            excelData = (object[,])sheet.UsedRange.get_Value(Missing.Value);

            excelIsClosed = !cancel;
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditInExcel(ChangedResourcestreeView.Tag.ToString());
        }

        private void saveAndRemoveDeletedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Localizer.ConfirmChangesFromGUI(ChangedResourcestreeView.Tag.ToString(), masterData, true);

            LoadMasterData(ChangedResourcestreeView.Tag.ToString(), true);
        }

        private void TranslatecontextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            if (ResourcelistView.SelectedItems.Count == 0 || ResourcelistView.SelectedItems[0].Text == Localizer.GetDefaultCultureName(ChangedResourcestreeView.Tag.ToString()))
            {
                e.Cancel = true;
            }
        }

        private void editTranslationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditTranslationtextBox.Top = ResourcelistView.Top + ResourcelistView.SelectedItems[0].Position.Y;
            EditTranslationtextBox.Left = ResourcelistView.Left + ResourcelistView.Columns[0].Width;
            EditTranslationtextBox.Width = ResourcelistView.Columns[1].Width;
            EditTranslationtextBox.Text = ResourcelistView.SelectedItems[0].SubItems[1].Text;
            EditTranslationtextBox.Visible = true;
            EditTranslationtextBox.Focus();
        }

        private void EditTranslationtextBox_Leave(object sender, EventArgs e)
        {
            if (colResize)
            {
                EditTranslationtextBox.Focus();
            }
            else
            {
                if (ChangedResourcestreeView.SelectedNode != null && ChangedResourcestreeView.SelectedNode.Tag != null)
                {
                    if (ResourcelistView.SelectedItems.Count > 0 && ResourcelistView.SelectedItems[0].Text != Localizer.GetDefaultCultureName(ChangedResourcestreeView.Tag.ToString()))
                    {
                        string resourceType = ChangedResourcestreeView.Tag.ToString();

                        KeyValuePair<XmlNode, Dictionary<string, XmlNode>> resource = (KeyValuePair<XmlNode, Dictionary<string, XmlNode>>)ChangedResourcestreeView.SelectedNode.Tag;

                        string cultureName = ResourcelistView.SelectedItems[0].Text;

                        if (resource.Value.ContainsKey(cultureName))
                        {
                            if (resource.Value[cultureName].InnerText != EditTranslationtextBox.Text)
                            {
                                resource.Value[cultureName].InnerText = EditTranslationtextBox.Text;

                                if (!resource.Key.Attributes["Change"].Value.Contains("("))
                                {
                                    resource.Key.Attributes["Change"].Value += "_(Updated)";
                                }

                                ShowSelectedRecourceForChange(resource);
                            }
                        }

                    }

                }
                EditTranslationtextBox.Visible = false;


            }

            colResize = false;
        }

        private void ResourcelistView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (EditTranslationtextBox.Visible)
            {
                if (e.ColumnIndex == 0)
                {
                    EditTranslationtextBox.Left = ResourcelistView.Left + e.NewWidth;
                }
                else
                {
                    EditTranslationtextBox.Width = e.NewWidth;
                }

                colResize = true;
            }
        }

        private bool colResize = false;

        private void handleDeletesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            saveAndRemoveDeletedToolStripMenuItem.Visible = handleDeletesToolStripMenuItem.Checked;
        }

        private void importLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripComboBoxImpLang.Items.Clear();
            if (masterData != null)
            {
                string rt = ChangedResourcestreeView.Tag.ToString();
                toolStripComboBoxImpLang.Items.AddRange(Localizer.GetSupportedTranslationsCultureNames(rt).Where(t => t != Localizer.GetDefaultCultureName(rt)).ToArray());
            }
        }

        private void toolStripComboBoxImpLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cultureToImport = toolStripComboBoxImpLang.Text;
            string pathToOpen = string.Empty;
            Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, KeyValuePair<XmlNode, Dictionary<string, XmlNode>>>>>>>>> tmpLangData;

            string resourceType = ChangedResourcestreeView.Tag.ToString();

            if (masterData != null)
            {
                OpenFileDialog xmlOpenFile = new OpenFileDialog();
                xmlOpenFile.Filter = "ResourceMaster (*.xml)|*.xml";

                if (xmlOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pathToOpen = xmlOpenFile.FileName;
                }

                xmlOpenFile.Dispose();

                Cursor = Cursors.WaitCursor;

                if (!string.IsNullOrEmpty(pathToOpen))
                {
                    if (File.Exists(pathToOpen))
                    {
                        //Check if file is valid

                        tmpLangData = Imi.Utils.Localizer.Localizer.GetMasterData(pathToOpen);

                        //Iterate throught all resources and add the language from the other file

                        foreach (string rt in masterData.Keys)
                        {
                            if (rt == resourceType && tmpLangData.ContainsKey(rt))
                            {
                                foreach (string moduleName in masterData[rt].Value.Keys)
                                {
                                    if (tmpLangData[rt].Value.ContainsKey(moduleName))
                                    {
                                        foreach (string resourceKey in masterData[rt].Value[moduleName].Value.Keys)
                                        {
                                            if (tmpLangData[rt].Value[moduleName].Value.ContainsKey(resourceKey))
                                            {
                                                foreach (string key in masterData[rt].Value[moduleName].Value[resourceKey].Value.Keys)
                                                {
                                                    //Check if the resource exists in the temporary data
                                                    if (tmpLangData[rt].Value[moduleName].Value[resourceKey].Value.ContainsKey(key))
                                                    {
                                                        if (masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Value.ContainsKey(cultureToImport))
                                                        {
                                                            masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Value[cultureToImport].InnerText = tmpLangData[rt].Value[moduleName].Value[resourceKey].Value[key].Value[cultureToImport].InnerText;
                                                        }
                                                        else
                                                        {
                                                            XmlElement newTranslation = masterData[rt].Key.OwnerDocument.CreateElement("Text");
                                                            newTranslation.SetAttribute("CultureName", cultureToImport);
                                                            newTranslation.InnerText = tmpLangData[rt].Value[moduleName].Value[resourceKey].Value[key].Value[cultureToImport].InnerText;

                                                            masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.AppendChild(newTranslation);
                                                            masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Value.Add(cultureToImport, newTranslation);
                                                        }

                                                        masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Updated"].Value = DateTime.Now.ToString();
                                                        if (!masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Change"].Value.Contains("Deleted") && !masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Change"].Value.Contains("("))
                                                        {
                                                            masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Change"].Value += "_(Updated)";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                Cursor = Cursors.Default;
            }
        }

        private void removeLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveLngtoolStripComboBox.Items.Clear();
            if (masterData != null)
            {
                string rt = ChangedResourcestreeView.Tag.ToString();
                RemoveLngtoolStripComboBox.Items.AddRange(Localizer.GetSupportedTranslationsCultureNames(rt).Where(t => t != Localizer.GetDefaultCultureName(rt)).ToArray());
            }
        }

        private void RemoveLngtoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string resourceType = ChangedResourcestreeView.Tag.ToString();
            string cultureToRemove = RemoveLngtoolStripComboBox.Text;
            Cursor = Cursors.WaitCursor;
            Localizer.RemoveSupportedTranslationCultureName(resourceType, cultureToRemove);

            //Iterate throught all resources and remove the language
            bool errorShown = false;
            foreach (string rt in masterData.Keys)
            {
                if (rt == resourceType)
                {
                    foreach (string moduleName in masterData[rt].Value.Keys)
                    {
                        foreach (string resourceKey in masterData[rt].Value[moduleName].Value.Keys)
                        {
                            foreach (string key in masterData[rt].Value[moduleName].Value[resourceKey].Value.Keys)
                            {
                                masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Updated"].Value = DateTime.Now.ToString();
                                if (!masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Change"].Value.Contains("Deleted") && !masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Change"].Value.Contains("("))
                                {
                                    masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Change"].Value += "_(Updated)";
                                }
                                else if (masterData[rt].Value[moduleName].Value[resourceKey].Value[key].Key.Attributes["Change"].Value.Contains("Deleted"))
                                {
                                    if (!errorShown)
                                    {
                                        MessageBox.Show("Deleted resources exist!\r\nPlease remove them by selecting \"Handle deletes\" and \"Save and do delete\".");
                                        errorShown = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            removeLanguageToolStripMenuItem_Click(this, null);

            Cursor = Cursors.Default;
        }
    }
}
