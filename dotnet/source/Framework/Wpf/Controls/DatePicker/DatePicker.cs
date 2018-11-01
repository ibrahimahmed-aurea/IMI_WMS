using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.Controls;
using System.Windows.Media;

namespace Imi.Framework.Wpf.Controls
{
    public class DatePicker : Xceed.Wpf.Controls.DatePicker
    {
        private Brush _normalBrush = null;
        private bool _errorSet = false;

        protected override void OnPreviewLostKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);

            Xceed.Wpf.Controls.NavigableComboBox navCombo = ((Xceed.Wpf.Controls.NavigableComboBox)((System.Windows.Controls.Decorator)(this.GetVisualChild(0))).Child);

            Xceed.Wpf.Controls.DatePicker innerDP = (Xceed.Wpf.Controls.DatePicker)navCombo.Template.FindName("textInput", navCombo);

            Xceed.Wpf.Controls.DateTimeTextBox innerText = (Xceed.Wpf.Controls.DateTimeTextBox)innerDP.Template.FindName("PART_DatePickerFocusRoot", innerDP);

            if (innerText.HasParsingError)
            {
                innerText.Text = "";

                if (this.BorderBrush != Brushes.Red)
                {
                    _normalBrush = this.BorderBrush;
                }

                this.BorderBrush = Brushes.Red;

                _errorSet = true;
            }
            else
            {
                if (_errorSet)
                {
                    _errorSet = false;
                }
                else
                {
                    if (this.BorderBrush == Brushes.Red)
                    {
                        this.BorderBrush = _normalBrush;
                    }
                }
            }
        }

        protected override void OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "DataContext")
            {
                if (this.BorderBrush == Brushes.Red)
                {
                    this.BorderBrush = _normalBrush;
                }
            }
        }

    }
}
