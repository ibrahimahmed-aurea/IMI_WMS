using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI.Shared;
using Imi.Wms.Mobile.Server.Interface;
using System.Configuration;

namespace Imi.Wms.Mobile.UI
{
    public partial class RenderForm : System.Windows.Forms.Form
    {
        private RenderPresenter _presenter;
        public event EventHandler<StateChangedEventArgs> StateChanged;

        private bool _fullScreen = true;

        public RenderForm()
        {
            InitializeComponent();

            _presenter = new RenderPresenter(this);

            renderPanel.StateChanged += (s, e) =>
            {
                if (StateChanged != null)
                {
                    try
                    {
                        this.Cursor = Cursors.AppStarting;

                        if (StateChanged != null)
                        {
                            StateChanged(s, e);
                        }
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            };
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
            bool centerToScreen = false;

            for (int i = 0; i < stateResponse.Beep; i++)
            {
                Console.Beep();
            }

            if (stateResponse.Form != null)
            {
                renderPanel.Render(stateResponse.Form);

                SuspendLayout();
                if (!_fullScreen)
                {
                    if (Text == "IMI iWMS Thin Client") //First render size might change. Center screen
                    {
                        centerToScreen = true;
                    }
                }

                Titlelabel.Text = stateResponse.Form.Text;
                Text = stateResponse.Form.Text;

                renderPanel.BorderStyle = BorderStyle.FixedSingle;

                ResumeLayout();

                calibrateRenderPanel();

                if (centerToScreen)
                {
                    CenterToScreen();
                }
            }
        }

        private void calibrateRenderPanel()
        {
            SuspendLayout();
            if (_fullScreen)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                renderPanel.Top = ((int)Height / 2) - ((int)renderPanel.Height / 2);
                renderPanel.Left = ((int)Width / 2) - ((int)renderPanel.Width / 2);
                renderPanel.Anchor = AnchorStyles.None;

                Titlelabel.Top = renderPanel.Top - 25;
                Titlelabel.Left = renderPanel.Left;
                
            }
            else
            {
                Titlelabel.Visible = false;
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                WindowState = FormWindowState.Normal;
                renderPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                renderPanel.Top = 0;
                renderPanel.Left = 0;
                renderPanel_SizeChanged(this, null);
            }
            ResumeLayout();
        }


        private void RenderForm_Load(object sender, EventArgs e)
        {
            if (Presenter.Config.WindowsDesktopSettingsCollection.ContainsKey("startAppInFullScreen"))
            {
                if (Presenter.Config.WindowsDesktopSettingsCollection["startAppInFullScreen"].Value != string.Empty)
                {
                    bool.TryParse(Presenter.Config.WindowsDesktopSettingsCollection["startAppInFullScreen"].Value, out _fullScreen);
                }
            }

            OnFullScreenChange();

            _presenter.StartRender();
        }

        private void OnFullScreenChange()
        {
            calibrateRenderPanel();

            if (!_fullScreen)
            {
               CenterToScreen(); 
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Return | Keys.Alt))
            {
                    _fullScreen = !_fullScreen;
                    OnFullScreenChange();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void renderPanel_SizeChanged(object sender, EventArgs e)
        {
            if (!_fullScreen)
            {
                Height = renderPanel.Height + 28;
                Width = renderPanel.Width+6;
            }
        }

    }
}
