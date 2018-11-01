using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.Specialized;


namespace Cdc.MetaManager.DataAccess.Domain.VisualModel
{
    public class UXLayoutGrid : UXContainer
    {
        private Dictionary<string, UXLayoutGridCell> cells = new Dictionary<string,UXLayoutGridCell>();
        private Dictionary<UXComponent, string> componentDict = new Dictionary<UXComponent, string>();

        public UXLayoutGrid()
            : this(null)
        {
        }

        public UXLayoutGrid(string name) : base(name)
        {
        }

        public UXLayoutGridCell[] Cells
        {
            get
            {
                // Order the cells by row and then column.
                // This is so that tab order will work correctly since this is used when looping
                // through all cells.
                return cells.Values.OrderBy(cell => cell.Row).ThenBy(cell => cell.Column).ToArray();
            }
            set
            {
                Children.Clear();
                cells.Clear();
                componentDict.Clear();

                foreach (UXLayoutGridCell cell in value)
                {
                    UXLayoutGridCell cellNew = AddComponent(cell.Column, cell.Row, cell.ColumnSpan, cell.Component);
                }
                
            }
        }

        public int RowCount
        {
            get
            {
                if (cells.Count() > 0)
                    return cells.Values.Max(c => c.Row) + 1;
                else
                    return 0;
            }
        }

        public int ColumnCount
        {
            get
            {
                if (cells.Count() > 0)
                    return cells.Values.Max(c => c.Column) + 1;
                else
                    return 0;
            }
        }

        public void RemoveComponent(UXComponent component)
        {
            if (component != null)
            {
                // Try to find the component
                string key = componentDict[component];

                if (!string.IsNullOrEmpty(key))
                {
                    if (cells[key].Component is UXContainer)
                    {
                        UXContainer cellContainer = cells[key].Component as UXContainer;
                        cellContainer.Children.Remove(component);
                        component.Parent = null;

                        // Check if there is just one left in container then we need to remove the container.
                        if (cellContainer.Children.Count == 1)
                        {
                            Children.Remove(cells[key].Component);
                            cells[key].Component = cellContainer.Children[0];
                            Children.Add(cells[key].Component);
                        }
                    }
                    else
                    {
                        cells.Remove(key);
                        component.Parent = null;
                        Children.Remove(component);
                    }

                    // Remove component from dictionary
                    componentDict.Remove(component);
                }
            }
        }



        public int GetColumnSpanForeComponent(UXComponent component)
        {
            int colSpan = 1;
            if (component != null)
            {
                // Try to find the component
                string key = componentDict[component];

                if (!string.IsNullOrEmpty(key))
                {
                    if (cells[key] != null)
                    {
                        colSpan = cells[key].ColumnSpan;
                    }
                }
            }
            return colSpan;
        }

        // Get all components in the gridcell where this component exists including this component.
        public IList<UXComponent> GetComponents(UXComponent component)
        {
            if (componentDict.ContainsKey(component))
            {
                string coords = componentDict[component];

                IList<UXComponent> foundComponents = null;

                if (cells[coords].Component is UXContainer)
                {
                    foundComponents = new List<UXComponent>();

                    foreach (UXComponent comp in ((UXContainer)cells[coords].Component).Children)
                    {
                        foundComponents.Add(comp);
                    }
                }
                else
                {
                    foundComponents = new List<UXComponent> { cells[coords].Component };
                }

                if (foundComponents.Count() > 0)
                {
                    return foundComponents;
                }
            }

            return new List<UXComponent>();
        }

        public UXComponent GetComponent(int row, int column)
        {
            string matchKey = Key(column, row);

            if (cells.ContainsKey(matchKey))
            {
                return cells[matchKey].Component;
            }
            else
            {
                return null;
            }
        }

        public UXLayoutGridCell GetUXLayoutGridCell(int column, int row)
        {
            string matchKey = Key(column, row);

            if (cells.ContainsKey(matchKey))
            {
                return cells[matchKey];
            }
            else
            {
                return null;
            }
        }
        
        private static string Key(int column, int row)
        {
            return string.Format("{0},{1}", column, row);
        }

        public static void MoveComponent(UXLayoutGrid from, UXLayoutGrid to, UXComponent component, bool moveAllCellContent)
        {
            IList<UXComponent> components = null;

            if (moveAllCellContent)
            {
                components = from.GetComponents(component);
            }
            else
            {
                components = new List<UXComponent> { component };
            }

            if (components.Count > 0)
            {
                int newRow = to.RowCount;

                foreach (UXComponent comp in components)
                {
                    int span2 = from.GetColumnSpanForeComponent(comp);
                    
                    // Remove component from the from grid
                    from.RemoveComponent(comp);

                    // Add the component to a new last row in the to-grid.
                    UXLayoutGridCell cellNew = to.AddComponent(0, newRow, comp);
                }
            }
        }

        public UXLayoutGridCell AddComponent(int column, int row, UXComponent newComponent)
        {
            return AddComponent(column, row, 1, newComponent);
        }
        
        public UXLayoutGridCell AddComponent(int column, int row, int columnSpan, UXComponent newComponent)
        {
            string key = Key(column, row);

            if (!componentDict.ContainsKey(newComponent))
            {
                if (cells.ContainsKey(key))
                {
                    componentDict.Add(newComponent, key);

                    if (cells[key].Component is UXContainer)
                    {
                        UXContainer cellContainer = cells[key].Component as UXContainer;
                        cellContainer.Children.Add(newComponent);
                        newComponent.Parent = cellContainer;
                        Children.Add(cellContainer);
                    }
                    else
                    {
                        // If more than one component in cell create stackpanel
                        UXComponent orgCellComponent = cells[key].Component;

                        UXStackPanel uxStackPanel = new UXStackPanel() { Parent = this, Orientation = UXPanelOrientation.Horizontal };

                        cells[key].Component = uxStackPanel;
                        

                        orgCellComponent.Parent = uxStackPanel;
                        uxStackPanel.Children.Add(orgCellComponent);

                        newComponent.Parent = uxStackPanel;
                        uxStackPanel.Children.Add(newComponent);

                        Children.Remove(orgCellComponent);
                        Children.Add(uxStackPanel);
                    }
                }
                else
                {
                    UXLayoutGridCell cell = new UXLayoutGridCell() { Row = row, Column = column, Component = newComponent, ColumnSpan = columnSpan };
                    cells[key] = cell;
                    
                    Children.Add(newComponent);

                    if (newComponent is UXContainer)
                    {
                        foreach (UXComponent comp in ((UXContainer)newComponent).Children)
                        {
                            componentDict.Add(comp, key);
                        }
                    }
                    else
                        componentDict.Add(newComponent, key);
                }
            }

            return cells[key];
        }


        public UXLayoutGridCell AddBlankCell(int column, int row, int colSpan)
        {
            string key = Key(column, row);

            if (cells.ContainsKey(key))
            {
            }
            else
            {
                UXLayoutGridCell cell = new UXLayoutGridCell() { Row = row, Column = column, Component = null };
                cells[key] = cell;
                cells[key].ColumnSpan = colSpan;
            }

            return cells[key];
        }
    }
}
