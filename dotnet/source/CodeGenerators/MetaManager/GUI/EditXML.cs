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
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace Cdc.MetaManager.GUI
{
    public partial class EditXML : Form
    {
        public Cdc.MetaManager.DataAccess.Domain.View View { get; set; }

        private IDialogService dialogService = null;

        public EditXML()
        {
            InitializeComponent();
        }

        private void EditXML_Load(object sender, EventArgs e)
        {
            dialogService = MetaManagerServices.GetDialogService();

            if (View != null)
            {
                tbXML.Text = FormatXml(View.VisualTreeXml);
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        public static string FormatXml(string inputXml) 
        { 
            XmlDocument document = new XmlDocument(); 

            document.Load(new StringReader(inputXml)); 

            StringBuilder builder = new StringBuilder(); 

            using (XmlTextWriter writer = new XmlTextWriter(new StringWriter(builder))) 
            { 
                writer.Formatting = Formatting.Indented; 
                document.Save(writer); 
            }

            return builder.ToString();
        }

        public static bool TryParseXml(string xml)
        {   
            XmlDocument doc = new XmlDocument();

            try
            {      
                doc.Load(new StringReader(xml));
                return true;
            }
            catch(XmlException)
            {
                return false;    
            }
        }

        public static bool TryParseXml(string xml, out string error)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(new StringReader(xml));
                error = string.Empty;
                return true;
            }
            catch (XmlException e)
            {
                error = e.ToString();
                return false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Check if XML is correct XML-format-wise.
            string xmlError = string.Empty;

            if (!TryParseXml(tbXML.Text, out xmlError))
            {
                MessageBox.Show(string.Format("Formatting error in XML!\n\n{0}", xmlError), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to update the XML?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            // Remove all CRLR and beginning spaces from newlines.
            View.VisualTreeXml = Regex.Replace(tbXML.Text, @"[\n\r]+\s*", "");

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbXML_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                tbXML.SelectAll();
            }
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbXML.Text);
        }

        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                tbXML.Text = Clipboard.GetText();
            }
        }
    }
}
