using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI.Configuration;

namespace Imi.Wms.Mobile.UI
{
    public partial class OptionsForm : Form
    {
        private OptionsPresenter _presenter;
        
        public OptionsForm()
        {
            InitializeComponent();
            _presenter = new OptionsPresenter(this);
        }

        public UISection Config { get; set; }
                                
        private void okButton_Click(object sender, EventArgs e)
        {
            Config.TerminalId = terminalIdTextBox.Text;
            Config.NativeDriver = nativeDriverTextBox.Text;
            Config.LogEnabled = logCheckBox.Checked;
            
            try
            {
                _presenter.Save(Config);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "IMI iWMS Thin Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            terminalIdTextBox.Text = Config.TerminalId;
            nativeDriverTextBox.Text = Config.NativeDriver;
            logCheckBox.Checked = Logger.IsEnabled;
        }
    }
}
