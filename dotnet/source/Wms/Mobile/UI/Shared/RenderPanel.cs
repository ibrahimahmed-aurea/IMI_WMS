using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Imi.Wms.Mobile.UI.Configuration;
using Imi.Wms.Mobile.Server.Interface;
using Imi.Wms.Mobile.UI;

namespace Imi.Wms.Mobile.UI.Shared
{
    public class RenderPanel : Panel
    {
        private Dictionary<string, Event> _eventDictionary;
        private INativeDriver _nativeDriver;

        public event EventHandler<StateChangedEventArgs> StateChanged;

        public RenderPanel()
        {
            _eventDictionary = new Dictionary<string, Event>();
            AutoScaleFactor = new SizeF(1, 1);
        }
               
        public SizeF AutoScaleFactor { get; set; }

        public INativeDriver NativeDriver
        {
            get
            {
                return _nativeDriver;
            }
            set
            {
                if (value != _nativeDriver)
                {
                    _nativeDriver = value;
                    _nativeDriver.Scan += ScanEventHandler;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _nativeDriver != null)
            {
                _nativeDriver.Scan -= ScanEventHandler;
            }

            base.Dispose(disposing);
        }

        private TControl GetControl<TControl>(string targetId)
            where TControl : Control, new()
        {
            foreach (Control control in Controls)
            {
                if ((control.Tag as string) == targetId && control is TControl)
                {
                    return control as TControl;
                }
            }

            return new TControl() { Tag = targetId };
        }

        private void ScanEventHandler(object sender, ScanEventArgs e)
        {
            Invoke(new Action(() =>
            {
                Control focusControl = null;

                foreach (Control control in Controls)
                {
                    if (control.Focused)
                    {
                        focusControl = control;
                    }
                }

                if (focusControl != null && (focusControl.Tag as string != null))
                {
                    CreateEvent((string)focusControl.Tag, "Scan", e.Data);
                    OnStateChanged();
                }
            }));
        }

        public void Render(Imi.Wms.Mobile.Server.Interface.Form form)
        {
            _eventDictionary.Clear();

            SuspendLayout();

            Width = form.Width;
            Height = form.Height;

            Control focusControl;
            var controls = RenderControls(form, out focusControl);

            Control[] existingControls = new Control[Controls.Count];

            Controls.CopyTo(existingControls, 0);

            foreach (Control control in existingControls)
            {
                if (!controls.Contains(control))
                {
                    Controls.Remove(control);
                }
            }

            foreach (Control control in controls)
            {
                if (!Controls.Contains(control))
                {
                    Controls.Add(control);
                }

                control.BringToFront();
            }

            ResumeLayout();

            if (focusControl != null)
            {
                focusControl.Focus();
            }
        }

        private List<Control> RenderControls(Imi.Wms.Mobile.Server.Interface.Form form, out Control focusControl)
        {
            var controls = new List<Control>();
            focusControl = null;

            if (form.Controls.CheckBox != null)
            {
                foreach (Imi.Wms.Mobile.Server.Interface.CheckBox renderCheckBox in form.Controls.CheckBox)
                {
                    var checkBox = RenderCheckBox(renderCheckBox);

                    controls.Add(checkBox);

                    if (renderCheckBox.Focused)
                        focusControl = checkBox;
                }
            }

            if (form.Controls.Label != null)
            {
                foreach (Imi.Wms.Mobile.Server.Interface.Label renderLabel in form.Controls.Label)
                {
                    var label = RenderLabel(renderLabel);

                    controls.Add(label);
                }
            }
                        
            if (form.Controls.ListBox != null)
            {
                foreach (Imi.Wms.Mobile.Server.Interface.ListBox renderListBox in form.Controls.ListBox)
                {
                    var listBox = RenderListBox(renderListBox);

                    controls.Add(listBox);

                    if (renderListBox.Focused)
                        focusControl = listBox;
                }
            }
                        
            if (form.Controls.TextBox != null)
            {
                foreach (Imi.Wms.Mobile.Server.Interface.TextBox renderTextBox in form.Controls.TextBox)
                {
                    var textBox = RenderTextBox(renderTextBox);

                    controls.Add(textBox);

                    if (renderTextBox.Focused)
                        focusControl = textBox;
                }
            }

            if (form.Controls.Button != null)
            {
                foreach (Imi.Wms.Mobile.Server.Interface.Button renderButton in form.Controls.Button)
                {
                    var button = RenderButton(renderButton);

                    controls.Add(button);

                    if (renderButton.Focused)
                        focusControl = button;
                }
            }

            return controls;
        }

        private Control RenderButton(Imi.Wms.Mobile.Server.Interface.Button renderButton)
        {
            var button = GetControl<System.Windows.Forms.Button>(renderButton.Id);
            button.Click -= button_Click;
            button.Left = (int)(renderButton.Left * AutoScaleFactor.Width);
            button.Top = (int)(renderButton.Top * AutoScaleFactor.Height);
            button.Width = (int)(renderButton.Width * AutoScaleFactor.Width);
            button.Height = (int)(renderButton.Height * AutoScaleFactor.Height);
            button.Text = renderButton.Text;
            button.Enabled = renderButton.Enabled;
            button.Visible = renderButton.Visible;
            button.Font = GetFont(renderButton.FontName, renderButton.FontSize, "");
            button.TabIndex = renderButton.TabIndex;
#if !PocketPC
            button.FlatStyle = FlatStyle.System;
#endif
            button.Click += button_Click;
            return button;
        }

        private Control RenderLabel(Imi.Wms.Mobile.Server.Interface.Label renderLabel)
        {
            var label = GetControl<System.Windows.Forms.Label>(renderLabel.Id);

            label.Left = (int)(renderLabel.Left * AutoScaleFactor.Width);
            label.Top = (int)(renderLabel.Top * AutoScaleFactor.Height);
            label.Width = (int)(renderLabel.Width * AutoScaleFactor.Width);
            label.Height = (int)(renderLabel.Height * AutoScaleFactor.Height);

            label.Font = GetFont(renderLabel.FontName, renderLabel.FontSize, renderLabel.FontStyle);

            label.Text = renderLabel.Text;

            int preferredWidth = label.Width;
            int preferredHeight = label.Height;

#if PocketPC
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(label.Text, label.Font);
                preferredWidth = Convert.ToInt32(size.Width);
                preferredHeight = Convert.ToInt32(size.Height);
            }
#else        
            Size size = TextRenderer.MeasureText(label.Text, label.Font);
            preferredWidth = size.Width;
            preferredHeight = size.Height;
#endif

            //Adjust length of text to show as much as possible on one row.
            if (preferredWidth > label.Width)
            {
                if (Math.Abs(preferredHeight - label.Size.Height) < (preferredHeight / 2)) //Only one row label
                {
                    while (preferredWidth > label.Width)
                    {
                        label.Text = label.Text.Substring(0, label.Text.Length - 1);

#if PocketPC
                        using (Graphics g = CreateGraphics())
                        {
                            SizeF size = g.MeasureString(label.Text, label.Font);
                            preferredWidth = Convert.ToInt32(size.Width);
                            preferredHeight = Convert.ToInt32(size.Height);
                        }
#else
                        size = TextRenderer.MeasureText(label.Text, label.Font);
                        preferredWidth = size.Width;
                        preferredHeight = size.Height;
#endif
                    }
                }
            }

#if !PocketPC            
            label.Padding = new Padding(0);
            label.TabStop = false;
#endif
            label.Visible = renderLabel.Visible;
            
            return label;
        }

        private Control RenderTextBox(Imi.Wms.Mobile.Server.Interface.TextBox renderTextBox)
        {
            var textBox = GetControl<System.Windows.Forms.TextBox>(renderTextBox.Id);
            textBox.TextChanged -= textBox_TextChanged;
            textBox.KeyPress -= textBox_KeyPress;
            textBox.Left = (int)(renderTextBox.Left * AutoScaleFactor.Width);
            textBox.Top = (int)(renderTextBox.Top * AutoScaleFactor.Height);
            textBox.Width = (int)(renderTextBox.Width * AutoScaleFactor.Width);
            textBox.Height = (int)(renderTextBox.Height * AutoScaleFactor.Height);
            textBox.Text = renderTextBox.Text;
            textBox.Enabled = renderTextBox.Enabled;
            textBox.Font = GetFont(renderTextBox.FontName, renderTextBox.FontSize, renderTextBox.FontStyle);
            textBox.Visible = renderTextBox.Visible;
            textBox.TabIndex = renderTextBox.TabIndex;
            textBox.WordWrap = false;
            textBox.TextChanged += textBox_TextChanged;
            textBox.KeyPress += textBox_KeyPress;
            
            if (!string.IsNullOrEmpty(renderTextBox.PassChar))
            {
                textBox.PasswordChar = renderTextBox.PassChar[0];
            }

            return textBox;
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                System.Windows.Forms.TextBox textBox = ((System.Windows.Forms.TextBox)sender);

                CreateEvent((string)textBox.Tag, "Scan", textBox.Text);

                OnStateChanged();
            }
        }

        private Control RenderListBox(Imi.Wms.Mobile.Server.Interface.ListBox renderListBox)
        {
            var listBox = GetControl<System.Windows.Forms.ListBox>(renderListBox.Id);

            listBox.SelectedIndexChanged -= listBox_SelectedIndexChanged;
            listBox.Left = (int)(renderListBox.Left * AutoScaleFactor.Width);
            listBox.Top = (int)(renderListBox.Top * AutoScaleFactor.Height);
            listBox.Width = (int)(renderListBox.Width * AutoScaleFactor.Width);
            listBox.Height = (int)(renderListBox.Height * AutoScaleFactor.Height);
            listBox.Enabled = renderListBox.Enabled;
            listBox.Visible = renderListBox.Visible;
            listBox.TabIndex = renderListBox.TabIndex;
            listBox.Font = GetFont(renderListBox.FontName, renderListBox.FontSize, renderListBox.FontStyle);
            
            listBox.BeginUpdate();

            listBox.Items.Clear();

            foreach (Item item in renderListBox.Items)
            {
                listBox.Items.Add(item.Text);
            }

            listBox.EndUpdate();

            listBox.SelectedIndex = renderListBox.SelectedIndex;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            return listBox;
        }

        private Control RenderCheckBox(Imi.Wms.Mobile.Server.Interface.CheckBox renderCheckBox)
        {
            var checkBox = GetControl<System.Windows.Forms.CheckBox>(renderCheckBox.Id);
#if PocketPC            
            checkBox.CheckStateChanged -= checkBox_CheckedChanged;
#else
            checkBox.CheckedChanged -= checkBox_CheckedChanged;
#endif
            checkBox.Left = (int)(renderCheckBox.Left * AutoScaleFactor.Width);
            checkBox.Top = (int)(renderCheckBox.Top * AutoScaleFactor.Height);
            checkBox.Width = (int)(renderCheckBox.Width * AutoScaleFactor.Width);
            checkBox.Height = (int)(renderCheckBox.Height * AutoScaleFactor.Height);
            checkBox.Text = renderCheckBox.Text;
            checkBox.Checked = renderCheckBox.Checked;
            checkBox.Enabled = renderCheckBox.Enabled;
            checkBox.Font = GetFont(renderCheckBox.FontName, renderCheckBox.FontSize, renderCheckBox.FontStyle);
            checkBox.Visible = renderCheckBox.Visible;
            checkBox.TabIndex = renderCheckBox.TabIndex;
#if PocketPC
            checkBox.CheckStateChanged += checkBox_CheckedChanged;
#else
            checkBox.CheckedChanged += checkBox_CheckedChanged;
#endif

            return checkBox;
        }

        private static Font GetFont(string fontName, float fontSize, string fontStyle)
        {
            var fontConversion = (from f in ConfigurationManager.LoadConfiguration().FontCollection
                       where f.OldName.ToLower() == fontName.ToLower()
                       select f).FirstOrDefault();

            if (fontConversion != null)
            {
                fontName = fontConversion.NewName;
                fontSize = fontSize + fontConversion.SizeAdjust;
            }

            FontStyle style = FontStyle.Regular;

            if (fontStyle.Contains("Bold"))
                style = style | FontStyle.Bold;

            if (fontStyle.Contains("Italic"))
                style = style | FontStyle.Italic;

            return new Font(fontName, fontSize, style);
        }

        void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ListBox listBox = ((System.Windows.Forms.ListBox)sender);

            Event ev = CreateEvent((string)listBox.Tag, "SelectedIndexChanged", listBox.SelectedIndex.ToString());

            OnStateChanged();
        }

        void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox checkBox = ((System.Windows.Forms.CheckBox)sender);

            Event ev = CreateEvent((string)checkBox.Tag, "CheckedChanged", checkBox.Checked ? "1" : "0");

            OnStateChanged();
        }

        void textBox_TextChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = ((System.Windows.Forms.TextBox)sender);

            Event ev = CreateEvent((string)textBox.Tag, "TextChanged", textBox.Text);
        }

        void button_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = ((System.Windows.Forms.Button)sender);

            CreateEvent((string)button.Tag, "Click", null);

            OnStateChanged();
        }

        private void OnStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, new StateChangedEventArgs(_eventDictionary.Values.ToList()));
            }

            _eventDictionary.Clear();
        }

        private Event CreateEvent(string targetId, string type, string data)
        {
            string key = string.Format("{0}_{1}", targetId, type);

            if (!_eventDictionary.ContainsKey(key))
            {
                Event e = new Event();
                e.TargetId = targetId;
                e.Type = type;

                _eventDictionary.Add(key, e);
            }

            _eventDictionary[key].Data = data;

            return _eventDictionary[key];
        }
    }

    public class StateChangedEventArgs : EventArgs
    {
        private IList<Event> _events;

        public StateChangedEventArgs(IList<Event> events)
        {
            _events = events;
        }

        public IList<Event> Events
        {
            get
            {
                return _events;
            }
        }
    }

}
