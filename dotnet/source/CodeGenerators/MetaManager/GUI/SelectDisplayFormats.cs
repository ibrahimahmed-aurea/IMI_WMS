using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cdc.MetaManager.BusinessLogic;
using System.Globalization;
using System.Diagnostics;

namespace Cdc.MetaManager.GUI
{
    public partial class SelectDisplayFormats : MdiChildForm
    {
        private IApplicationService appService = null;

        public string DisplayFormat { get; set; }
        public Type DisplayFormatDataType { private get; set; }
        
        private string HelpLinkURL { get; set; }

        public SelectDisplayFormats()
        {
            InitializeComponent();

            appService = MetaManagerServices.GetApplicationService();
        }

        private void SelectDisplayFormats_Load(object sender, EventArgs e)
        {
            string output, errorText;

            tbDisplayFormat.Text = DisplayFormat;
            lblDataType.Text = string.Format("[{0}]", DisplayFormatDataType.Name);

            IList<string> allDisplayFormats = appService.FindAllDisplayFormatsUsed(DisplayFormatDataType);

            lvDisplayFormats.Items.Clear();

            foreach (string displayFormat in allDisplayFormats)
            {
                ListViewItem item = lvDisplayFormats.Items.Add(displayFormat);

                if (TryDisplayFormat(displayFormat, GenerateTestData(DisplayFormatDataType), DisplayFormatDataType, out output, out errorText))
                {
                    item.SubItems.Add(output);
                }
                else
                {
                    item.SubItems.Add(errorText);
                }
            }

            if (lvDisplayFormats.Items.Count > 0)
            {
                lvDisplayFormats.Items[0].Selected = true;
            }

            // Set help link for format strings
            if (DisplayFormatDataType == typeof(DateTime))
            {
                // Date and Time Format Strings
                HelpLinkURL = "http://msdn.microsoft.com/en-us/library/97x6twsz.aspx";
                llblHelpFormatStrings.Text = "Date and Time Format Strings";
                gbTestDisplayFormat.AutoSize = false; // make "Date time special Display Format" groupbox be shown
            }
            else
            {
                // Numeric Format Strings
                HelpLinkURL = "http://msdn.microsoft.com/en-us/library/427bttx3.aspx";
                llblHelpFormatStrings.Text = "Numeric Format Strings";
                gbTestDisplayFormat.AutoSize = true;
            }
            
            btnOK.Enabled = this.IsEditable;

            // Generate test data
            SetGeneratedTestData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DisplayFormat = tbDisplayFormat.Text;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void CopyDisplayFormatFromList()
        {
            if (lvDisplayFormats.SelectedItems.Count > 0)
            {
                tbDisplayFormat.Text = lvDisplayFormats.SelectedItems[0].Text;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            SetGeneratedTestData();
        }

        private void SetGeneratedTestData()
        {
            object testData = GenerateTestData(DisplayFormatDataType);
            tbTestData.Tag = testData;

            if (testData == null)
            {
                tbTestData.Text = string.Empty;
            }
            if (DisplayFormatDataType == typeof(DateTime))
            {
                tbTestData.Text = ((DateTime)testData).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (DisplayFormatDataType == typeof(decimal))
            {
                tbTestData.Text = ((decimal)testData).ToString();
            }
            else if (DisplayFormatDataType == typeof(double))
            {
                tbTestData.Text = ((double)testData).ToString();
            }
            else if (DisplayFormatDataType == typeof(int))
            {
                tbTestData.Text = ((int)testData).ToString();
            }
            else if (DisplayFormatDataType == typeof(long))
            {
                tbTestData.Text = ((long)testData).ToString();
            }
        }

        private static object GenerateTestData(Type generateTypeString)
        {
            return GenerateTestData(generateTypeString, null);
        }

        private static object GenerateTestData(Type generateTypeString, string testData)
        {
            if (generateTypeString == typeof(DateTime))
            {
                if (!string.IsNullOrEmpty(testData))
                {
                    DateTime tmp;

                    if (DateTime.TryParse(testData, out tmp))
                    {
                        return tmp;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    return DateTime.Now;
            }
            else if (generateTypeString == typeof(Decimal))
            {
                if (!string.IsNullOrEmpty(testData))
                {
                    Decimal tmp;

                    if (Decimal.TryParse(testData, out tmp))
                    {
                        return tmp;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Random rnd = new Random();

                    string tmp = string.Format("{0}{1}{2}",
                                         string.Format("{0,5}", rnd.Next()),
                                         NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                                         string.Format("{0,5}", rnd.Next()));

                    return Decimal.Parse(tmp);
                }
            }
            else if (generateTypeString == typeof(Double))
            {
                if (!string.IsNullOrEmpty(testData))
                {
                    Double tmp;

                    if (Double.TryParse(testData, out tmp))
                    {
                        return tmp;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Random rnd = new Random();

                    string tmp = string.Format("{0}{1}{2}",
                                         string.Format("{0,5}", rnd.Next()),
                                         NumberFormatInfo.CurrentInfo.NumberDecimalSeparator,
                                         string.Format("{0,5}", rnd.Next()));

                    return Double.Parse(tmp);
                }
            }
            else if (generateTypeString == typeof(int))
            {
                if (!string.IsNullOrEmpty(testData))
                {
                    int tmp;

                    if (int.TryParse(testData, out tmp))
                    {
                        return tmp;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Random rnd = new Random();

                    string tmp = String.Format("{0,5}", rnd.Next());

                    return int.Parse(tmp);
                }
            }
            else if (generateTypeString == typeof(long))
            {
                if (!string.IsNullOrEmpty(testData))
                {
                    long tmp;

                    if (long.TryParse(testData, out tmp))
                    {
                        return tmp;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Random rnd = new Random();

                    string tmp = String.Format("{0,5}", rnd.Next());

                    return long.Parse(tmp);
                }
            }
            else
                return null;
        }

        private void tbTestData_TextChanged(object sender, EventArgs e)
        {
            object testData = GenerateTestData(DisplayFormatDataType, tbTestData.Text);
            tbTestData.Tag = testData;

            if (testData != null)
            {
                tbTestData.BackColor = Color.FromName("Window");
            }
            else
            {
                tbTestData.BackColor = Color.LightCoral;
            }

            UpdateOutput();
        }

        private void UpdateOutput()
        {
            string errorText, output;

            if (tbTestData.Tag != null)
            {
                if (TryDisplayFormat(tbDisplayFormat.Text, tbTestData.Tag, DisplayFormatDataType, out output, out errorText))
                {
                    tbOutput.Text = output;
                }
                else
                {
                    tbOutput.Text = errorText;
                }
            }
            else
                tbOutput.Text = string.Empty;
        }

        private static bool TryDisplayFormat(string displayFormat, object testData, Type testDataType, out string output, out string errorText)
        {
            errorText = string.Empty;
            output = string.Empty;

            if (testDataType == typeof(DateTime))
            {
                try
                {
                    DateTime d = (DateTime)testData;

                    displayFormat = displayFormat.Replace("{", "{0:");

                    if (!string.IsNullOrEmpty(displayFormat) && displayFormat.Contains("{0:"))
                        output = string.Format(displayFormat, d);
                    else
                        output = d.ToString(displayFormat);

                    return true;
                }
                catch
                {
                    errorText = "Display Format is not on correct format.";
                    return false;
                }
            }
            else
            {
                try
                {
                    string format = string.Format("{{0:{0}}}", displayFormat);
                    output = string.Format(format, testData);
                    return true;
                }
                catch
                {
                    errorText = "Display Format is not on correct format.";
                    return false;
                }
            }
        }

        private void lvDisplayFormats_DoubleClick(object sender, EventArgs e)
        {
            CopyDisplayFormatFromList();
        }

        private void tbDisplayFormat_TextChanged(object sender, EventArgs e)
        {
            UpdateOutput();
        }

        private void llblHelpFormatStrings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(HelpLinkURL);
        }
    }
}
