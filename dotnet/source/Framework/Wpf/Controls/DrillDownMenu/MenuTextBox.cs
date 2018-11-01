using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Threading;

namespace Imi.Framework.Wpf.Controls
{
    /// <summary>
    /// ========================================
    /// .NET Framework 3.0 Custom Control
    /// ========================================
    ///
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.Wpf.Controls.TruckProgress"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.Wpf.Controls.TruckProgress;assembly=Imi.Framework.Wpf.Controls.TruckProgress"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file. Note that Intellisense in the
    /// XML editor does not currently work on custom controls and its child elements.
    ///
    ///     <MyNamespace:TextBox/>
    ///
    /// </summary>
    public class MenuTextBox : System.Windows.Controls.TextBox
    {
        private bool isMouseDown;
        private string oldValue;

        public bool EditComplete
        {
            get { return (bool)GetValue(EditCompleteProperty); }
            set { SetValue(EditCompleteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditComplete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditCompleteProperty =
            DependencyProperty.Register("EditComplete", typeof(bool), typeof(MenuTextBox), new UIPropertyMetadata(false, EditCompletePropertyChangedEventHandler));

        static MenuTextBox()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuTextBox), new FrameworkPropertyMetadata(typeof(MenuTextBox)));
        }
        
        private static void EditCompletePropertyChangedEventHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MenuTextBox textBox = sender as MenuTextBox;

            if (!textBox.EditComplete)
            {
                textBox.oldValue = textBox.Text;

                textBox.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        textBox.Focus();
                    }),
                    DispatcherPriority.Background,
                    null);
            }
        }

        public MenuTextBox()
        {
          
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                EditComplete = true;
            }

            if (e.Key == Key.Enter)
            {
                EditComplete = true;
            }

            if (e.Key == Key.Escape)
            {
                Text = oldValue;
                EditComplete = true;
            }

            e.Handled = true;
            base.OnKeyDown(e);

        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (!isMouseDown)
                this.SelectAll();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            try
            {
                if (this.SelectionLength != 0)
                    this.SelectionLength = 0;
            }
            catch (Exception) { }

            EditComplete = true;

            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            isMouseDown = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            isMouseDown = false;
            base.OnMouseUp(e);
        }

    }
}
