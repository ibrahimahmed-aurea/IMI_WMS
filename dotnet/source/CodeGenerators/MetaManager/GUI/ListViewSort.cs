using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Cdc.MetaManager.GUI
{
    public class ListViewSort : ListView
    {
        private ListViewColumnSorter colSorter;

        private Dictionary<int, IComparer> colComparer;

        public ListViewSort()
            : base()
        {
            colComparer = new Dictionary<int, IComparer>();
            colSorter = new ListViewColumnSorter();

            this.ListViewItemSorter = colSorter;
                
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            base.OnItemCheck(ice);
        }
       

        // If clicking CTRL-E you can export the content to a .CSV file
        // easily treated in Excel.
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.E && e.Control)
            {
                ExportToCSV();
            }
        }

        protected override void OnColumnClick(ColumnClickEventArgs e)
        {
            base.OnColumnClick(e);

            SortByColumn(e.Column, SortOrder.None, AnalyzeIfNumeric(e.Column, 20));
        }

        public void SortByColumn(int column, SortOrder sortOrder, bool isNumeric)
        {
            // Do nothing if column is outside bounds
            if (column < 0 || column >= Columns.Count)
                return;

            if (!colComparer.ContainsKey(column))
            {
                if (isNumeric)
                {
                    colComparer.Add(column, new ListViewColumnSorterNumeric());
                }
                else
                {
                    colComparer.Add(column, new CaseInsensitiveComparer());
                }
            }
            else
            {
                if (isNumeric && colComparer[column] is CaseInsensitiveComparer)
                    colComparer[column] = new ListViewColumnSorterNumeric();
                else if (!isNumeric && colComparer[column] is ListViewColumnSorterNumeric)
                    colComparer[column] = new CaseInsensitiveComparer();
            }

            colSorter.Comparer = colComparer[column];

            // Determine if clicked column is already the column that is being sorted.
            if (column == colSorter.SortColumn)
            {
                // If not selected sort order then order like if clicked on the column
                if (sortOrder == SortOrder.None)
                {
                    // Reverse the current sort direction for this column.
                    if (colSorter.Order == SortOrder.Ascending)
                    {
                        colSorter.Order = SortOrder.Descending;
                    }
                    else
                    {
                        colSorter.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    colSorter.Order = sortOrder;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                colSorter.SortColumn = column;
                colSorter.Order = sortOrder == SortOrder.None ? SortOrder.Ascending : sortOrder;
            }

            // Perform the sort with these new sort options.
            this.Sort();
        }

        private bool AnalyzeIfNumeric(int column, int analyzeRows)
        {
            decimal test = 0;
            int okCnt = 0;

            for (int i = 0; i < this.Items.Count; i++)
            {
                if (i >= analyzeRows)
                    break;

                if (decimal.TryParse(this.Items[i].SubItems[column].Text, out test))
                {
                    okCnt++;
                }
                else
                {
                    okCnt = -1;
                    break;
                }
            }

            if (this.Items.Count == 0)
                return false;

            if ((this.Items.Count >= analyzeRows && okCnt == analyzeRows) ||
                (this.Items.Count < analyzeRows && okCnt == this.Items.Count))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string SelectCSVFilePath(string filePath)
        {
            // Show dialog for selecting the file to save to
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.CheckFileExists = false;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.DefaultExt = "csv";
                saveFileDialog.Filter = "CSV-Files (*.csv) | *.csv";
                saveFileDialog.Title = "Select the filename to save the list to";
                saveFileDialog.FileName = filePath;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    filePath = saveFileDialog.FileName;
                else
                    filePath = string.Empty;
            }

            return filePath;
        }

        public void ExportToCSV()
        {
            ExportToCSV(string.Empty, false);
        }

        public void ExportToCSV(string filePath)
        {
            ExportToCSV(filePath, false);
        }

        public void ExportToCSV(string filePath, bool includeHidden)
        {
            try
            {
                filePath = SelectCSVFilePath(filePath);

                if (filePath == string.Empty)
                    return;

                // Make header string 
                StringBuilder result = new StringBuilder();
                WriteCSVRow(result, Columns.Count, i => includeHidden || Columns[i].Width > 0, i => Columns[i].Text);

                // Export data rows 
                foreach (ListViewItem listItem in Items)
                    WriteCSVRow(result, Columns.Count, i => includeHidden || Columns[i].Width > 0, i => listItem.SubItems[i].Text);

                File.WriteAllText(filePath, result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error when trying to export to CSV file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                if (!isColumnNeeded(i))
                    continue;

                if (!isFirstTime)
                    result.Append(";");
                isFirstTime = false;

                string newColumnValue = columnValue(i);

                // Replace carriage return & line feed with a single space
                newColumnValue = newColumnValue.Replace("\r\n", " ");

                // Replace " with two ""
                newColumnValue = newColumnValue.Replace("\"", "\"\"");

                result.Append(String.Format("\"{0}\"", newColumnValue));
            }

            result.AppendLine();
        }

    }

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private IComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult = 0;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            if (listviewX.SubItems.Count > ColumnToSort &&
                listviewY.SubItems.Count > ColumnToSort)
            {
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            }

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

        /// <summary>
        /// Gets or sets the comparer to use when comparing items.
        /// </summary>
        public IComparer Comparer
        {
            set
            {
                ObjectCompare = value;
            }
            get
            {
                return ObjectCompare;
            }
        }
    }

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorterNumeric : IComparer
    {
        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            decimal numberX, numberY;

            // Cast the objects to be compared to decimals
            try
            {
                numberX = decimal.Parse((string)x);
            }
            catch
            {
                numberX = 0;
            }

            try
            {
                numberY = decimal.Parse((string)y);
            }
            catch
            {
                numberY = 0;
            }

            // Compare the two items
            if (numberX > numberY)
                return 1;
            else if (numberX < numberY)
                return -1;
            else
                return 0;
        }
    }
}
