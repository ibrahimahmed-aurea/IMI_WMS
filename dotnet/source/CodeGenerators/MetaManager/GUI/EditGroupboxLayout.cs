using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.DataAccess.Domain.VisualModel;
using System.Runtime.InteropServices;
using Cdc.MetaManager.DataAccess.Domain;
using Cdc.MetaManager.BusinessLogic;
using Cdc.MetaManager.DataAccess;
using Cdc.MetaManager.BusinessLogic.Helpers;




namespace Cdc.MetaManager.GUI
{
    public partial class EditGroupboxLayout : MdiChildForm
    {
        private const int WM_SETREDRAW      = 0x000B;
        private const int WM_USER           = 0x400;
        private const int EM_GETEVENTMASK   = (WM_USER + 59);
        private const int EM_SETEVENTMASK   = (WM_USER + 69);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        public UXGroupBox EditGroupbox { get; set; }
        public PropertyMap PropertyMap { get; set; }
        public bool IsViewReadOnly { get; set; }

        private int mouseDownMousePositionX;
        private ToolTip _tooltip = null;

        private IDialogService dialogService = null;
        private IModelService modelService = null;

        private ToolTip ToolTip 
        {
            get 
            {
                if (_tooltip == null)
                {
                    _tooltip = new ToolTip();
                }
                return _tooltip;
            }
        }

        public EditGroupboxLayout()
        {
            InitializeComponent();

            dialogService = MetaManagerServices.GetDialogService();
            modelService = MetaManagerServices.GetModelService();
        }

        public List<string> ComponentNames;

        private void BeginUpdate(Control control)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Stop redrawing:
            SendMessage(control.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
        }

        private void EndUpdate(Control control)
        {
            // turn on redrawing
            SendMessage(control.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

            System.Windows.Forms.Application.DoEvents();
            control.Invalidate(true);
            Cursor.Current = Cursors.Default;
        }

        private void EditGroupboxLayout_Load(object sender, EventArgs e)
        {
            if (EditGroupbox == null)
            {
                Close();
            }
            else
            {
                if (PropertyMap != null)
                {
                    lvFields.Items.Clear();
                                                           
                    foreach (MappedProperty mappedProperty in PropertyMap.MappedProperties.OrderBy(p => p.Name))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Name = mappedProperty.Name;
                        item.Text = mappedProperty.Name;
                        item.SubItems.Add(mappedProperty.Type.Name);
                        
                        DbProperty origin = MetaManagerServices.Helpers.MappedPropertyHelper.GetOrigin(mappedProperty);

                        if (origin != null)
                        {
                            item.SubItems.Add(origin.Name);
                        }
                                                
                        item.Tag = new LocalProperty() { MappedProperty = mappedProperty, Name = mappedProperty.Name };
                        
                        lvFields.Items.Add(item);
                    }
                }

                mainGroupbox.Text = EditGroupbox.Caption;

                if (EditGroupbox.Container is UXLayoutGrid)
                {                   
                    UXLayoutGrid grid = EditGroupbox.Container as UXLayoutGrid;

                    foreach (UXLayoutGridCell cell in grid.Cells)
                    {
                        // First check if rows or columns need to be added.
                        CheckAddRowsAndColumns(cell.Column + 1, cell.Row + 1);

                        // Add the component to the cell.
                        AddComponentToCell(cell.Column, cell.Row, cell.ColumnSpan, cell.Component);
                        
                    }
                }
            }

            ShowHideToolstrip();
        }
        
        private void AddComponentToCell(int col, int row, int columnSpan, UXComponent component)
        {
            Control panel = tableLayoutPanel.GetControlFromPosition(col, row);

            if (panel is FlowLayoutPanel)
            {
                
                if (component is UXLabel)
                {
                    Label obj = CreateLabel(component as UXLabel);
                    obj.Tag = component;
                    panel.Controls.Add(obj);
                    tableLayoutPanel.SetColumnSpan(obj, columnSpan);
                }
                else if (component is UXTextBox)
                {
                    TextBox obj = CreateTextbox(component as UXTextBox);
                    obj.Tag = component;
                    panel.Controls.Add(obj);
                    tableLayoutPanel.SetColumnSpan(obj, columnSpan);
                }
                else if (component is UXCheckBox)
                {
                    CheckBox obj = CreateCheckbox(component as UXCheckBox);
                    obj.Tag = component;
                    panel.Controls.Add(obj);
                    tableLayoutPanel.SetColumnSpan(obj, columnSpan);
                }
                else if (component is UXComboBox)
                {
                    ComboBox obj = CreateCombobox(component as UXComboBox);
                    obj.Tag = component;
                    panel.Controls.Add(obj);
                    tableLayoutPanel.SetColumnSpan(obj, columnSpan);
                }
                else if (component is UXRadioGroup)
                {
                    TableLayoutPanel obj = CreateRadiogroup(component as UXRadioGroup);
                    obj.Tag = component;
                    panel.Controls.Add(obj);
                    tableLayoutPanel.SetColumnSpan(obj, columnSpan);
                }
                else if (component is UXListBox)
                {
                    ListBox obj = CreateListbox(component as UXListBox);
                    obj.Tag = component;
                    panel.Controls.Add(obj);
                    tableLayoutPanel.SetColumnSpan(obj, columnSpan);
                }
                else if (component is UXStackPanel)
                {
                    UXStackPanel stackPanel = (component as UXStackPanel);

                    foreach (UXComponent child in stackPanel.Children)
                    {
                        AddComponentToCell(col, row, columnSpan, child);
                    }
                }
                else 
                {
                    Control obj = CreateUnknown(component);
                    obj.Tag = component;
                    panel.Controls.Add(obj);
                    tableLayoutPanel.SetColumnSpan(obj, columnSpan);
                }
            }
        }

        private Control CreateUnknown(UXComponent component)
        {
            TextBox txt = new TextBox();
            txt.Size = new Size(component.Width, component.Height);
            txt.Margin = new Padding(3, 3, 3, 3);
            txt.MouseDown += new MouseEventHandler(control_MouseDown);
            ToolTip.SetToolTip(txt, component.Name);
            return txt;
        }

        private TableLayoutPanel CreateRadiogroup(UXRadioGroup uXRadioGroup)
        {
            TableLayoutPanel radioGroupPanel = new TableLayoutPanel();
            radioGroupPanel.AutoSize = true;
            radioGroupPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            radioGroupPanel.ColumnCount = 1;
            radioGroupPanel.MouseDown += new MouseEventHandler(control_MouseDown);

            SetRadiogroupOptions(uXRadioGroup, radioGroupPanel);

            return radioGroupPanel;
        }

        private void SetRadiogroupOptions(UXRadioGroup uXRadioGroup, TableLayoutPanel radioGroupPanel)
        {
            // Clear before adding
            radioGroupPanel.Controls.Clear();

            // Set number of rows for the radiogroup panel
            radioGroupPanel.RowCount = uXRadioGroup.Keys.Count;

            int row = 0;
            foreach (string key in uXRadioGroup.Keys)
            {
                RadioButton radioButton = null;

                // Only add keys until there are no more matching values
                if (row < uXRadioGroup.Values.Count)
                    radioButton = CreateRadiobutton(key, uXRadioGroup.Values[row]);
                else
                    break;

                radioGroupPanel.Controls.Add(radioButton);
                radioGroupPanel.SetCellPosition(radioButton, new TableLayoutPanelCellPosition(0, row));
                row++;
            }
        }


        private RadioButton CreateRadiobutton(string key, string value)
        {
            RadioButton radio = new RadioButton();
            radio.Text = value;
            radio.AutoSize = true;
            radio.Margin = new Padding(0);
            ToolTip.SetToolTip(radio, key);
            radio.MouseDown += new MouseEventHandler(control_MouseDown_Parent);
            return radio;
        }

        private ListBox CreateListbox(UXListBox uXListBox)
        {
            ListBox listbox = new ListBox();
            listbox.Size = new Size(uXListBox.Width, uXListBox.Height);
            listbox.Margin = new Padding(3, 3, 3, 3);
            listbox.MouseDown += new MouseEventHandler(control_MouseDown);
            ToolTip.SetToolTip(listbox, string.Format("{0} ({1})", uXListBox.Name, GetMappedPropertyName(uXListBox)));
            return listbox;
        }

        private void UpdateListbox(ListBox listBox)
        {
            UXListBox uXListBox = listBox.Tag as UXListBox;

            if (uXListBox != null)
            {
                listBox.Size = new Size(uXListBox.Width, uXListBox.Height);
                ToolTip.SetToolTip(listBox, string.Format("{0} ({1})", uXListBox.Name, GetMappedPropertyName(uXListBox)));
            }
        }

        private string GetMappedPropertyName(IBindable bindable)
        {
            string name = "";

            if (bindable.MappedProperty != null)
                name = bindable.MappedProperty.Name;

            return name;
        }

        private ComboBox CreateCombobox(UXComboBox uXComboBox)
        {
            ComboBox combo = new ComboBox();
            combo.Size = new Size(uXComboBox.Width, uXComboBox.Height);
            combo.Margin = new Padding(3, 3, 3, 3);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.MouseDown += new MouseEventHandler(control_MouseDown);
            ToolTip.SetToolTip(combo, string.Format("{0} ({1})", uXComboBox.Name, GetMappedPropertyName(uXComboBox)));
            return combo;            
        }

        private void UpdateCombobox(ComboBox comboBox)
        {
            UXComboBox uXComboBox = comboBox.Tag as UXComboBox;

            if (uXComboBox != null)
            {
                comboBox.Size = new Size(uXComboBox.Width, uXComboBox.Height);
                ToolTip.SetToolTip(comboBox, string.Format("{0} ({1})", uXComboBox.Name, GetMappedPropertyName(uXComboBox)));
            }
        }

        private CheckBox CreateCheckbox(UXCheckBox uXCheckBox)
        {
            CheckBox chk = new CheckBox();
            chk.Text = "";
            chk.AutoSize = true;
            chk.Margin = new Padding(3, 5, 3, 3);
            chk.MouseDown += new MouseEventHandler(control_MouseDown);
            ToolTip.SetToolTip(chk, string.Format("{0} ({1})", uXCheckBox.Name, GetMappedPropertyName(uXCheckBox)));
            return chk;
        }

        private void UpdateCheckbox(CheckBox checkBox)
        {
            UXCheckBox uXCheckBox = checkBox.Tag as UXCheckBox;

            if (uXCheckBox != null)
            {
                ToolTip.SetToolTip(checkBox, string.Format("{0} ({1})", uXCheckBox.Name, GetMappedPropertyName(uXCheckBox)));
            }
        }

        private TextBox CreateTextbox(UXTextBox uXTextBox)
        {
            TextBox txt = new TextBox();
            txt.Size = new Size(uXTextBox.Width, uXTextBox.Height);
            txt.Margin = new Padding(3, 3, 3, 3);
            txt.MouseDown += new MouseEventHandler(control_MouseDown);
            ToolTip.SetToolTip(txt, string.Format("{0} ({1})", uXTextBox.Name, GetMappedPropertyName(uXTextBox)));
            return txt;
        }

        private void UpdateTextBox(TextBox textBox)
        {
            UXTextBox uXTextBox = textBox.Tag as UXTextBox;

            if (uXTextBox != null)
            {
                textBox.Size = new Size(uXTextBox.Width, uXTextBox.Height);
                ToolTip.SetToolTip(textBox, string.Format("{0} ({1})", uXTextBox.Name, GetMappedPropertyName(uXTextBox)));
            }
        }

        private Label CreateLabel(UXLabel uXLabel)
        {
            Label label = new Label();
            label.Text = uXLabel.Caption;
            //label.Size = new Size(uXLabel.Width, uXLabel.Height);
            label.AutoSize = true;
            label.Margin = new Padding(0, 6, 3, 3);
            label.MouseDown += new MouseEventHandler(control_MouseDown);
            ToolTip.SetToolTip(label, uXLabel.Name);
            return label;
        }

        private void UpdateLabel(Label label)
        {
            UXLabel uXLabel = label.Tag as UXLabel;

            if (uXLabel != null)
            {
                label.Text = uXLabel.Caption;
                ToolTip.SetToolTip(label, uXLabel.Name);
            }
        }

        void control_MouseDown(object sender, MouseEventArgs e)
        {
            if (((Control)sender).Parent is FlowLayoutPanel && e.Button == MouseButtons.Left)
            {
                // Set the selected object for propertygrid
                if ((sender as Control).Tag != null &&
                    (sender as Control).Tag is UXComponent)
                {
                    SetSelectedObject((sender as Control).Tag, sender);
                }
                else
                {
                    SetSelectedObject(null, null);
                }

                // Save mouse X-position
                mouseDownMousePositionX = e.X;

                // Do the drag n' drop
                DragDropEffects dde = DoDragDrop(sender, DragDropEffects.Move | DragDropEffects.Scroll);
            }
        }

        private void SetSelectedObject(object selectedObject, object tag)
        {
            // Set the new selected object
            pgDetails.SelectedObject = selectedObject;
            pgDetails.Tag = tag;

            // Show hide toolstrip since it depends on selected object
            ShowHideToolstrip();
        }

        private void ShowHideToolstrip()
        {
            bool visible = false;

            if (pgDetails.SelectedObject != null &&
                pgDetails.SelectedObject is UXRadioGroup &&
                pgDetails.Tag != null &&
                pgDetails.Tag is TableLayoutPanel)
            {
                visible = true;
            }

            tsActions.Visible = visible;
        }

        void control_MouseDown_Parent(object sender, MouseEventArgs e)
        {
            if (((Control)sender).Parent is TableLayoutPanel && e.Button == MouseButtons.Left)
            {
                // Set the selected object for propertygrid
                if ((sender as Control).Parent.Tag != null &&
                    (sender as Control).Parent.Tag is UXComponent)
                {
                    SetSelectedObject((sender as Control).Parent.Tag, (sender as Control).Parent);
                }
                else
                {
                    SetSelectedObject(null, null);
                }

                // Save mouse X-position
                mouseDownMousePositionX = e.X;

                DragDropEffects dde = DoDragDrop(((Control)sender).Parent, DragDropEffects.Move | DragDropEffects.Scroll);
            }
        }
        
        
        void panel_Control_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender) is FlowLayoutPanel && e.Button == MouseButtons.Left)
               
            {

                TableLayoutPanelCellPosition tlpcp = tableLayoutPanel.GetCellPosition((sender as Control));
                if (EditGroupbox.Container is UXLayoutGrid)
                {                   
                    UXLayoutGrid grid = EditGroupbox.Container as UXLayoutGrid;

                    UXLayoutGridCell UXCell = grid.GetUXLayoutGridCell(tlpcp.Column, tlpcp.Row);
                    if (UXCell == null)
                    {
                        UXCell = grid.AddBlankCell(tlpcp.Column, tlpcp.Row, 1);
                    }

                    SetSelectedObject(UXCell, grid);
                }
            }
        }

        private void CheckAddRowsAndColumns(int columns, int rows)
        {
            bool update = false;

            if (tableLayoutPanel.RowCount == 1 && tableLayoutPanel.ColumnCount == 1)
            {
                update = true;
            }

            if (tableLayoutPanel.RowCount < rows)
            {
                tableLayoutPanel.RowCount = rows;
                update = true;
            }

            if (tableLayoutPanel.ColumnCount < columns)
            {
                tableLayoutPanel.ColumnCount = columns;
                update = true;
            }

            if (update)
            {
                // Add new flowpanels to new cells
                for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                {
                    for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                    {
                        Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);

                        if (getControl == null)
                        {
                            FlowLayoutPanel layoutPanel = GetNewFlowLayoutPanel();
                            
                            tableLayoutPanel.Controls.Add(layoutPanel);
                            tableLayoutPanel.SetCellPosition(layoutPanel, new TableLayoutPanelCellPosition(col, row));
                        }
                    }
                }
            }
        }

        private FlowLayoutPanel GetNewFlowLayoutPanel()
        {
            FlowLayoutPanel layoutPanel = new FlowLayoutPanel();
            layoutPanel.AutoSize = true;
            layoutPanel.MinimumSize = new System.Drawing.Size(20, 26);
            layoutPanel.Margin = new Padding(0, 0, 0, 0);
            layoutPanel.Padding = new Padding(0);
            layoutPanel.AllowDrop = true;
            layoutPanel.Dock = DockStyle.Fill;
            //layoutPanel.BorderStyle = BorderStyle.FixedSingle;
            layoutPanel.DragOver += new DragEventHandler(layoutPanel_DragOver);
            layoutPanel.DragDrop += new DragEventHandler(layoutPanel_DragDrop);
            layoutPanel.MouseDown += new MouseEventHandler(panel_Control_MouseDown);
            return layoutPanel;
        }

        void layoutPanel_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        void layoutPanel_DragDrop(object sender, DragEventArgs e)
        {
            // Convert the mouseposition
            int mouseX = 0;
            
            // Convert the mouseposition to client x-position
            if (sender is FlowLayoutPanel)
            {
                mouseX = (sender as FlowLayoutPanel).PointToClient(new Point(e.X, e.Y)).X;
            }

            try
            {
                BeginUpdate(tableLayoutPanel);

                if (e.Data.GetDataPresent(typeof(Label)))
                {
                    Control data = (Control)e.Data.GetData(typeof(Label));

                    if (CheckIfDroppedInSamePanel((FlowLayoutPanel)sender, data, mouseX))
                        AddControlToPanel((FlowLayoutPanel)sender, data);
                }
                else if (e.Data.GetDataPresent(typeof(TextBox)))
                {
                    Control data = (Control)e.Data.GetData(typeof(TextBox));

                    if (CheckIfDroppedInSamePanel((FlowLayoutPanel)sender, data, mouseX))
                        AddControlToPanel((FlowLayoutPanel)sender, data);
                }
                else if (e.Data.GetDataPresent(typeof(ComboBox)))
                {
                    Control data = (Control)e.Data.GetData(typeof(ComboBox));

                    if (CheckIfDroppedInSamePanel((FlowLayoutPanel)sender, data, mouseX))
                        AddControlToPanel((FlowLayoutPanel)sender, data);
                }
                else if (e.Data.GetDataPresent(typeof(CheckBox)))
                {
                    Control data = (Control)e.Data.GetData(typeof(CheckBox));

                    if (CheckIfDroppedInSamePanel((FlowLayoutPanel)sender, data, mouseX))
                        AddControlToPanel((FlowLayoutPanel)sender, data);
                }
                else if (e.Data.GetDataPresent(typeof(RadioButton)))
                {
                    Control data = (Control)e.Data.GetData(typeof(RadioButton));

                    if (CheckIfDroppedInSamePanel((FlowLayoutPanel)sender, data, mouseX))
                        AddControlToPanel((FlowLayoutPanel)sender, data);
                }
                else if (e.Data.GetDataPresent(typeof(ListBox)))
                {
                    Control data = (Control)e.Data.GetData(typeof(ListBox));

                    if (CheckIfDroppedInSamePanel((FlowLayoutPanel)sender, data, mouseX))
                        AddControlToPanel((FlowLayoutPanel)sender, data);
                }
                else if (e.Data.GetDataPresent(typeof(TableLayoutPanel)))
                {
                    Control data = (Control)e.Data.GetData(typeof(TableLayoutPanel));

                    if (CheckIfDroppedInSamePanel((FlowLayoutPanel)sender, data, mouseX))
                        AddControlToPanel((FlowLayoutPanel)sender, data);
                }
                else if (e.Data.GetDataPresent(typeof(LocalProperty)))
                {
                    LocalProperty localProp = (LocalProperty)e.Data.GetData(typeof(LocalProperty));

                    // Try to get the target property as the default visual component.
                    UXComponent newComponent = localProp.MappedProperty.TargetProperty != null ? localProp.MappedProperty.TargetProperty.VisualComponent : null;

                    using (ConfigureComponentForm getComponentForm = new ConfigureComponentForm())
                    {
                        getComponentForm.Owner = this;
                        getComponentForm.BackendApplication = BackendApplication;
                        getComponentForm.FrontendApplication = FrontendApplication;
                        getComponentForm.UXComponent = newComponent;
                        getComponentForm.OnlyBindables = true;
                        getComponentForm.ComponentNames = ComponentNames;
                        getComponentForm.IsEditable = IsEditable;

                        if (getComponentForm.ShowDialog() == DialogResult.OK)
                        {
                            // Now we have the component to use
                            newComponent = getComponentForm.UXComponent;

                            using (AddComponentToGroupbox addComponentForm = new AddComponentToGroupbox())
                            {
                                addComponentForm.Owner = this;
                                addComponentForm.Component = newComponent;
                                addComponentForm.ComponentNames = ComponentNames;
                                addComponentForm.FrontendApplication = FrontendApplication;
                                addComponentForm.BackendApplication = BackendApplication;
                                addComponentForm.DefaultLabelText = localProp.Name;

                                if (addComponentForm.ShowDialog() == DialogResult.OK)
                                {
                                    UXLabel label = new UXLabel();
                                    label.Name = ViewHelper.GetDefaultComponentName(ComponentNames, typeof(UXLabel));
                                    label.Caption = addComponentForm.LabelText;

                                    if (addComponentForm.Component is IBindable)
                                    {
                                        ((IBindable)addComponentForm.Component).MappedProperty = localProp.MappedProperty;
                                    }

                                    AddControlToPanel((FlowLayoutPanel)sender, label);
                                    ComponentNames.Add(label.Name);
                                    AddControlToPanel((FlowLayoutPanel)sender, addComponentForm.Component);
                                    ComponentNames.Add(addComponentForm.Component.Name);
                                }
                            }
                        }
                    }
                }
                else if (e.Data.GetDataPresent(typeof(LocalComponent)))
                {
                    using (ConfigureComponentForm getComponentForm = new ConfigureComponentForm())
                    {
                        getComponentForm.Owner = this;
                        getComponentForm.BackendApplication = BackendApplication;
                        getComponentForm.FrontendApplication = FrontendApplication;
                        getComponentForm.OnlyBindables = false;
                        getComponentForm.ComponentNames = ComponentNames;
                        getComponentForm.IsEditable = IsEditable;

                        if (getComponentForm.ShowDialog() == DialogResult.OK)
                        {
                            UXComponent newComponent = getComponentForm.UXComponent;
                            newComponent.IsReadOnly = IsViewReadOnly;

                            if (newComponent is IBindable)
                            {
                                using (AddLabelToGroupbox labelForm = new AddLabelToGroupbox())
                                {
                                    if (labelForm.ShowDialog() == DialogResult.OK)
                                    {
                                        UXLabel label = new UXLabel();
                                        label.Name = ViewHelper.GetDefaultComponentName(ComponentNames, typeof(UXLabel));
                                        label.Caption = labelForm.Caption;

                                        ComponentNames.Add(label.Name);
                                        AddControlToPanel((FlowLayoutPanel)sender, label);
                                    }
                                }
                            }

                            ComponentNames.Add(newComponent.Name);
                            AddControlToPanel((FlowLayoutPanel)sender, newComponent);
                        }
                    }
                }
            }
            finally
            {
                EndUpdate(tableLayoutPanel);
            }
        }

        private bool CheckIfDroppedInSamePanel(FlowLayoutPanel flowLayoutPanel, Control data, int mousePositionX)
        {
            // Check that the sender is the same layoutpanel as the sent datas parent.
            // In that case check if mouse has moved atleast 10 pixels.
            if (data.Parent != null &&
                data.Parent is FlowLayoutPanel &&
                flowLayoutPanel.Equals(data.Parent))
            {
                if (Math.Abs(mouseDownMousePositionX - mousePositionX) < 10)
                {
                    return false;
                }
            }

            return true;
        }

        private void AddControlToPanel(FlowLayoutPanel flowLayoutPanel, Control control)
        {
            if (control != null)
            {
                flowLayoutPanel.Controls.Add(control);
            }
        }

        private void AddControlToPanel(FlowLayoutPanel flowLayoutPanel, UXComponent component)
        {
            if (component != null)
            {
                if (component is UXLabel)
                {
                    Label obj = CreateLabel(component as UXLabel);
                    obj.Tag = component;
                    flowLayoutPanel.Controls.Add(obj);
                }
                else if (component is UXTextBox)
                {
                    TextBox obj = CreateTextbox(component as UXTextBox);
                    obj.Tag = component;
                    flowLayoutPanel.Controls.Add(obj);
                }
                else if (component is UXCheckBox)
                {
                    CheckBox obj = CreateCheckbox(component as UXCheckBox);
                    obj.Tag = component;
                    flowLayoutPanel.Controls.Add(obj);
                }
                else if (component is UXComboBox)
                {
                    ComboBox obj = CreateCombobox(component as UXComboBox);
                    obj.Tag = component;
                    flowLayoutPanel.Controls.Add(obj);
                }
                else if (component is UXRadioGroup)
                {
                    TableLayoutPanel obj = CreateRadiogroup(component as UXRadioGroup);
                    obj.Tag = component;
                    flowLayoutPanel.Controls.Add(obj);
                }
                else if (component is UXListBox)
                {
                    ListBox obj = CreateListbox(component as UXListBox);
                    obj.Tag = component;
                    flowLayoutPanel.Controls.Add(obj);
                }
                else if (component is UXStackPanel)
                {
                    UXStackPanel stackPanel = (component as UXStackPanel);

                    foreach (UXComponent child in stackPanel.Children)
                    {
                        AddControlToPanel(flowLayoutPanel, child);
                    }
                }
                else
                {
                    Control obj = CreateUnknown(component);
                    obj.Tag = component;
                    flowLayoutPanel.Controls.Add(obj);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {  
            // First remove empty rows and columns so we get the correct coordinates for all components.
            RemoveEmptyRowsAndColumns();

            if (EditGroupbox.Container == null)
            {
                EditGroupbox.Container = new UXLayoutGrid();
            }

            // Keep the old layoutGrid to be abel to read the ColumnSpan values
            UXLayoutGrid oldGrid = EditGroupbox.Container as UXLayoutGrid;

            UXLayoutGrid newGrid = new UXLayoutGrid();
            EditGroupbox.Container = newGrid;

            // Loop through the TableLayoutPanel and find all components 
            // Plase them corect in the new grid
            // but keep the columnspan.
            for (int row = 0; row < tableLayoutPanel.RowCount; row++)
            {
                for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                {
                    
                    Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);
                    int colSpan = 1;
                    UXLayoutGridCell oldCell = oldGrid.GetUXLayoutGridCell(col, row);
                    if (oldCell != null)
                    {
                        colSpan = oldCell.ColumnSpan;
                    }

                    if (getControl is FlowLayoutPanel)
                    {
                        FlowLayoutPanel panel = (getControl as FlowLayoutPanel);

                        foreach (Control control in panel.Controls)
                        {
                            UXLayoutGridCell cell = newGrid.AddComponent(col, row, colSpan, (UXComponent)control.Tag);
                        }
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void removeEmpty_Click(object sender, EventArgs e)
        {
            RemoveEmptyRowsAndColumns();
        }

        private void RemoveEmptyRowsAndColumns()
        {
            int emptyIndex;

            // Check for empty rows
            if (FindEmptyRow(out emptyIndex))
            {
                try
                {
                    BeginUpdate(tableLayoutPanel);

                    // Move rows
                    MoveRows(emptyIndex + 1, emptyIndex);

                    // Find the first empty row
                    if (FindEmptyRow(out emptyIndex))
                    {
                        // First delete everything in the cells for the rows.
                        DeleteRowContents(emptyIndex);

                        // Now cut the tablelayoutpanel by setting rowcount
                        tableLayoutPanel.RowCount = emptyIndex;
                    }
                }
                finally
                {
                    EndUpdate(tableLayoutPanel);
                }
            }

            // Check for empty columns
            if (FindEmptyColumn(out emptyIndex))
            {
                try
                {
                    BeginUpdate(tableLayoutPanel);

                    // Move columns
                    MoveColumns(emptyIndex + 1, emptyIndex);

                    // Find the first empty row
                    if (FindEmptyColumn(out emptyIndex))
                    {
                        // First delete everything in the cells for the columns.
                        DeleteColumnContents(emptyIndex);

                        // Now cut the tablelayoutpanel by setting columncount
                        tableLayoutPanel.ColumnCount = emptyIndex;
                    }
                }
                finally
                {
                    EndUpdate(tableLayoutPanel);
                }
            }
        }

        private void DeleteRowContents(int startRowIndex)
        {
            for (int row = startRowIndex; row < tableLayoutPanel.RowCount; row++)
            {
                for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                {
                    Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);

                    if (getControl is FlowLayoutPanel)
                    {
                        tableLayoutPanel.Controls.Remove(getControl);
                    }
                }
            }
        }

        private void DeleteColumnContents(int startColIndex)
        {
            for (int row = 0; row < tableLayoutPanel.RowCount; row++)
            {
                for (int col = startColIndex; col < tableLayoutPanel.ColumnCount; col++)
                {
                    Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);

                    if (getControl is FlowLayoutPanel)
                    {
                        tableLayoutPanel.Controls.Remove(getControl);
                    }
                }
            }
        }

        private bool FindEmptyRow(out int index)
        {
            index = -1;

            for (int row = 0; row < tableLayoutPanel.RowCount; row++)
            {
                bool empty = true;

                for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                {
                    Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);

                    if (getControl is FlowLayoutPanel)
                    {
                        empty = (getControl as FlowLayoutPanel).Controls.Count == 0;

                        if (!empty)
                            break;
                    }
                }

                if (empty)
                {
                    index = row;
                    break;
                }
            }

            return index != -1;
        }

        private bool FindEmptyColumn(out int index)
        {
            index = -1;

            for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
            {
                bool empty = true;

                for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                {
                    Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);

                    if (getControl is FlowLayoutPanel)
                    {
                        empty = (getControl as FlowLayoutPanel).Controls.Count == 0;

                        if (!empty)
                            break;
                    }
                }

                if (empty)
                {
                    index = col;
                    break;
                }
            }

            return index != -1;
        }

        private void MoveRows(int fromRow, int toRow)
        {
            // Moving from a row with higher number to a row with lower (upwards).
            if (fromRow < tableLayoutPanel.RowCount)
            {
                for (int row = fromRow; row < tableLayoutPanel.RowCount; row++)
                {
                    bool rowEmpty = true;

                    for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                    {
                        Control fromControl = tableLayoutPanel.GetControlFromPosition(col, row);
                        Control toControl = tableLayoutPanel.GetControlFromPosition(col, toRow);

                        if (fromControl is FlowLayoutPanel &&
                            toControl is FlowLayoutPanel)
                        {
                            FlowLayoutPanel fromPanel = fromControl as FlowLayoutPanel;
                            FlowLayoutPanel toPanel = toControl as FlowLayoutPanel;

                            if (fromPanel.Controls.Count > 0)
                            {
                                // We found something on the row.
                                rowEmpty = false;

                                // Move the items in the panel to the toPanel.
                                do
                                {
                                    ((Control)fromPanel.Controls[0]).Parent = toPanel;
                                }
                                while (fromPanel.Controls.Count > 0);
                            }
                        }
                    }

                    if (!rowEmpty)
                    {
                        toRow++;
                    }
                }
            }
        }

        private void MoveColumns(int fromCol, int toCol)
        {
            // Moving from a column with higher number to a column with lower (to the left).
            if (fromCol < tableLayoutPanel.ColumnCount)
            {
                for (int col = fromCol; col < tableLayoutPanel.ColumnCount; col++)
                {
                    bool colEmpty = true;

                    for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                    {
                        Control fromControl = tableLayoutPanel.GetControlFromPosition(col, row);
                        Control toControl = tableLayoutPanel.GetControlFromPosition(toCol, row);

                        if (fromControl is FlowLayoutPanel &&
                            toControl is FlowLayoutPanel)
                        {
                            FlowLayoutPanel fromPanel = fromControl as FlowLayoutPanel;
                            FlowLayoutPanel toPanel = toControl as FlowLayoutPanel;

                            if (fromPanel.Controls.Count > 0)
                            {
                                // We found something on the column.
                                colEmpty = false;

                                // Move the items in the panel to the toPanel.
                                do
                                {
                                    ((Control)fromPanel.Controls[0]).Parent = toPanel;
                                }
                                while (fromPanel.Controls.Count > 0);
                            }
                        }
                    }

                    if (!colEmpty)
                    {
                        toCol++;
                    }
                }
            }
        }

        private void lvFields_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListViewItem hittedItem = lvFields.GetItemAt(e.X, e.Y);

                if (hittedItem != null)
                {
                    LocalProperty localProp = (LocalProperty)hittedItem.Tag;
                    DragDropEffects dde = DoDragDrop(localProp, DragDropEffects.Move | DragDropEffects.Scroll);
                }
            }
        }

        private void btnAddLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DragDropEffects dde = DoDragDrop(new LocalComponent(), DragDropEffects.Move | DragDropEffects.Scroll);
            }
        }

        private void pgDetails_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (pgDetails.Tag != null && 
                pgDetails.Tag is Control && 
                pgDetails.SelectedObject != null &&
                pgDetails.SelectedObject is UXComponent)
            {
                UXComponent comp = pgDetails.SelectedObject as UXComponent;
                Control control = pgDetails.Tag as Control;

                if (e.ChangedItem.Label == "Name")
                {
                    if (!NamingGuidance.CheckNameNotInList(comp.Name, "Component Name", "Dialog Component Namelist", ComponentNames, false, true) ||
                        !NamingGuidance.CheckName(comp.Name, "Component Name", true))
                    {
                        comp.Name = (string)e.OldValue;
                    }
                    else
                    {
                        ComponentNames.Remove((string)e.OldValue);
                        ComponentNames.Add(comp.Name);
                    }
                }
                else if (e.ChangedItem.Label == "Caption")
                {
                    if (!NamingGuidance.CheckCaption((string)e.ChangedItem.Value, "Caption", true))
                    {
                        // Reset the caption back to the old value by using reflection since
                        // caption doesn't belong to UXComponent
                        try
                        {
                            comp.GetType().GetProperty("Caption").SetValue(comp, (string)e.OldValue, null);
                        }
                        catch
                        {
                            MessageBox.Show(string.Format("An error occured when trying to reset the Caption back to the original value \"{0}\". Please do this manually!", (string)e.OldValue), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                // Update text, width and height
                if (comp is UXLabel)
                {
                    UpdateLabel(control as Label);
                }
                else if (comp is UXTextBox)
                {
                    UpdateTextBox(control as TextBox);
                }
                else if (comp is UXCheckBox)
                {
                    UpdateCheckbox(control as CheckBox);
                }
                else if (comp is UXComboBox)
                {
                    UpdateCombobox(control as ComboBox);
                }
                else if (comp is UXListBox)
                {
                    UpdateListbox(control as ListBox);
                }

                pgDetails.Refresh();
            }
        }

        private void SetColSpanForComponentsCell(UXComponent uxComponent, int colSpan)
        {
            Control cont = GetControlAssociatedToUXComponent(uxComponent);

            if (cont != null)
            {
                tableLayoutPanel.SetColumnSpan(cont, colSpan);
            }
        }

        private Control GetControlAssociatedToUXComponent(UXComponent uxComponent)
        {
            Control componentToReturn = null;

            if (EditGroupbox.Container != null)
            {
                UXLayoutGrid layoutGrid = EditGroupbox.Container as UXLayoutGrid;

                for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                {
                    for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                    {
                        Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);
                        
                        if (getControl is FlowLayoutPanel)
                        {
                            FlowLayoutPanel panel = (getControl as FlowLayoutPanel);

                            foreach (Control control in panel.Controls)
                            {
                                if (control != null && control.Tag != null && control.Tag is UXComponent)
                                {
                                    if (((UXComponent)control.Tag).Equals(uxComponent))
                                    {
                                        componentToReturn = control;
                                    }
                                }

                            }
                        }
                    }
                }            
            }
            return componentToReturn;
        }

        private void btnRemoveComponent_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnRemoveComponent_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Label)))
            {
                RemoveControlFromPanel((Control)e.Data.GetData(typeof(Label)));
            }
            else if (e.Data.GetDataPresent(typeof(TextBox)))
            {
                RemoveControlFromPanel((Control)e.Data.GetData(typeof(TextBox)));
            }
            else if (e.Data.GetDataPresent(typeof(ComboBox)))
            {
                RemoveControlFromPanel((Control)e.Data.GetData(typeof(ComboBox)));
            }
            else if (e.Data.GetDataPresent(typeof(CheckBox)))
            {
                RemoveControlFromPanel((Control)e.Data.GetData(typeof(CheckBox)));
            }
            else if (e.Data.GetDataPresent(typeof(RadioButton)))
            {
                RemoveControlFromPanel((Control)e.Data.GetData(typeof(RadioButton)));
            }
            else if (e.Data.GetDataPresent(typeof(ListBox)))
            {
                RemoveControlFromPanel((Control)e.Data.GetData(typeof(ListBox)));
            }
            else if (e.Data.GetDataPresent(typeof(TableLayoutPanel)))
            {
                RemoveControlFromPanel((Control)e.Data.GetData(typeof(TableLayoutPanel)));
            }
        }

        private void RemoveControlFromPanel(Control control)
        {
            if (control.Parent is FlowLayoutPanel)
            {
                // Remove the name from the ComponentNames
                ComponentNames.Remove(control.Name);

                // Remove the control from the LayoutPanel
                (control.Parent as FlowLayoutPanel).Controls.Remove(control);

                // Clear the selectedobject since it's been removed
                SetSelectedObject(null, null);
            }
        }

        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.ColumnCount >= 25)
            {
                MessageBox.Show("You may not create more than 25 columns.");
                return;
            }

            try
            {
                BeginUpdate(tableLayoutPanel);

                tableLayoutPanel.ColumnCount += 1;

                // Add new flowpanels to new columns
                for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                {
                    for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                    {
                        Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);

                        if (getControl == null)
                        {
                            FlowLayoutPanel layoutPanel = GetNewFlowLayoutPanel();
                            tableLayoutPanel.Controls.Add(layoutPanel);
                            tableLayoutPanel.SetCellPosition(layoutPanel, new TableLayoutPanelCellPosition(col, row));
                        }
                    }
                }
            }
            finally
            {
                EndUpdate(tableLayoutPanel);
            }
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel.RowCount >= 50)
            {
                MessageBox.Show("You may not create more than 50 rows.");
                return;
            }

            try
            {
                BeginUpdate(tableLayoutPanel);

                tableLayoutPanel.RowCount += 1;

                // Add new flowpanels to new rows
                for (int row = 0; row < tableLayoutPanel.RowCount; row++)
                {
                    for (int col = 0; col < tableLayoutPanel.ColumnCount; col++)
                    {
                        Control getControl = tableLayoutPanel.GetControlFromPosition(col, row);

                        if (getControl == null)
                        {
                            FlowLayoutPanel layoutPanel = GetNewFlowLayoutPanel();
                            tableLayoutPanel.Controls.Add(layoutPanel);
                            tableLayoutPanel.SetCellPosition(layoutPanel, new TableLayoutPanelCellPosition(col, row));
                        }
                    }
                }
            }
            finally
            {
                EndUpdate(tableLayoutPanel);
            }
        }

        private void tsbtnConfigureComponent_Click(object sender, EventArgs e)
        {
            if (pgDetails.SelectedObject != null &&
                pgDetails.SelectedObject is UXRadioGroup && 
                pgDetails.Tag != null && 
                pgDetails.Tag is TableLayoutPanel)
            {
                UXRadioGroup uxRadioGroup = pgDetails.SelectedObject as UXRadioGroup;

                using (KeyValueEditor keyValueEditor = new KeyValueEditor())
                {
                    keyValueEditor.CaptionDialog = "Edit Radiobuttons";
                    keyValueEditor.CaptionKeyColumn = "Keyvalue";
                    keyValueEditor.CaptionValueColumn = "Caption";
                    keyValueEditor.MinimumKeysEntered = 1;
                    keyValueEditor.PopulateList(uxRadioGroup.Keys, uxRadioGroup.Values);

                    if (keyValueEditor.ShowDialog() == DialogResult.OK)
                    {
                        // Get the new set of keys and values
                        uxRadioGroup.Keys = keyValueEditor.KeyValues.Keys.ToList();
                        uxRadioGroup.Values = keyValueEditor.KeyValues.Values.ToList();

                        // Update the groupbox visually
                        SetRadiogroupOptions(uxRadioGroup, pgDetails.Tag as TableLayoutPanel);
                    }
                }
            }
        }

    }

    public class LocalProperty
    {
        public MappedProperty MappedProperty { get; set; }
        public string Name { get; set; }
    }

    public class LocalComponent {}
}
