using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.Server;
using Imi.Wms.Mobile.UI.Shared;
using Imi.Wms.Mobile.Server.Interface;
using Microsoft.WindowsCE.Forms;

namespace Imi.Wms.Mobile.UI
{
    public partial class RenderForm : BaseForm
    {
        private RenderPresenter _presenter;
                        
        public event EventHandler<StateChangedEventArgs> StateChanged;

        public RenderForm()
        {
            InitializeComponent();

            _presenter = new RenderPresenter(this);
            
            Cursor.Current = Cursors.Default;
            Cursor.Hide();

            renderPanel.StateChanged += (s, e) =>
            {
                if (StateChanged != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Cursor.Show();
                    
                    try
                    {
                        StateChanged(s, e);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                        Cursor.Hide();
                    }
                }
            };
                        
            KeyPreview = true;
        }
                                
        public RenderPresenter Presenter
        {
            get
            {
                return _presenter;
            }
        }

        public void RenderState(StateResponse stateResponse)
        {
            renderPanel.NativeDriver = _presenter.NativeDriver;
                        
            for (int i = 0; i < stateResponse.Beep; i++)
            {
                System.Media.SystemSounds.Beep.Play();
            }

            if (stateResponse.Form != null)
            {
                Text = stateResponse.Form.Text;
                Width = stateResponse.Form.Width;
                Height = stateResponse.Form.Height;

                renderPanel.Render(stateResponse.Form);

                foreach (Control control in renderPanel.Controls)
                {
                    System.Windows.Forms.TextBox textBox = control as System.Windows.Forms.TextBox;

                    if (textBox != null)
                    {
                        //textBox.GotFocus -= textBox_GotFocus;
                        textBox.LostFocus -= textBox_LostFocus;
                        //textBox.GotFocus += textBox_GotFocus;
                        textBox.LostFocus += textBox_LostFocus;
                    }
                }
            }
        }

        void textBox_LostFocus(object sender, EventArgs e)
        {
            if (!(GetFocusedControl(this) is System.Windows.Forms.TextBox))
            {
                inputPanel.Enabled = false;
            }
        }
        
        /*
        void textBox_GotFocus(object sender, EventArgs e)
        {
            inputPanel.Enabled = !((System.Windows.Forms.TextBox)sender).ReadOnly;
        }
        */

        private void RenderForm_Load(object sender, EventArgs e)
        {
            if (SystemSettings.Platform == WinCEPlatform.WinCEGeneric)
            {
                Menu = null;
            }

            _presenter.StartRender();
        }

        private void renderPanel_Click(object sender, EventArgs e)
        {
            inputPanel.Enabled = false;
        }

        private void RenderForm_Resize(object sender, EventArgs e)
        {
            if ((AutoScaleFactor.Height != 1) || (AutoScaleFactor.Width != 1))
            {
                renderPanel.AutoScaleFactor = AutoScaleFactor;
            }
        }

        private Control GetFocusedControl(Control parent)
        {
            if (parent.Focused)
            {
                return parent;
            }

            foreach (Control control in parent.Controls)
            {
                Control temp = GetFocusedControl(control);

                if (temp != null)
                {
                    return temp;
                }
            }
          
            return null;
        } 
    }
}