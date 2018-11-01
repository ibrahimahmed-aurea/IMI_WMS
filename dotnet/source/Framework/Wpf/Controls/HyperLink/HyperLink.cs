using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Imi.Framework.Wpf.Controls
{
    public delegate void HyperLinkEventHandler(object sender, RoutedEventArgs e);

    public class HyperLink : ContentControl
    {
        private System.Windows.Controls.TextBox _textBox;

        #region Routed Events

        public static readonly RoutedEvent ClickedEvent = EventManager.RegisterRoutedEvent("Clicked", RoutingStrategy.Bubble, typeof(HyperLinkEventHandler), typeof(HyperLink));

        public event HyperLinkEventHandler Clicked
        {
            add { AddHandler(ClickedEvent, value); }
            remove { RemoveHandler(ClickedEvent, value); }
        }

        #endregion

        static HyperLink()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HyperLink), new FrameworkPropertyMetadata(typeof(HyperLink)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // Find neccessary components in template
            _textBox = Template.FindName("PART_ContentHost", this) as System.Windows.Controls.TextBox;
        }
                
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Space) && HasContent && Content.ToString() != "")
            {
                e.Handled = true;
                RoutedEventArgs args = new RoutedEventArgs(ClickedEvent, this);
                RaiseEvent(args);
            }

            base.OnPreviewKeyDown(e);
        }
        
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_textBox == null || (string.IsNullOrEmpty(_textBox.SelectedText) 
                && _textBox.GetCharacterIndexFromPoint(e.GetPosition(_textBox), false) > -1))
            {
                RoutedEventArgs args = new RoutedEventArgs(ClickedEvent, this);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RaiseEvent(args);
                }), DispatcherPriority.ContextIdle);
            }

            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (_textBox != null)
            {
                if (_textBox.GetCharacterIndexFromPoint(e.GetPosition(_textBox), false) > -1)
                {
                    _textBox.Cursor = Cursors.Hand;
                }
                else
                {
                    _textBox.Cursor = Cursors.IBeam;
                }
            }
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (_textBox != null)
            {
                _textBox.SelectionLength = 0;
            }
        }
    }
}
