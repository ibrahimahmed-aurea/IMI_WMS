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
    public class TextBox : System.Windows.Controls.TextBox
    {
        private bool isMouseDown;
        private Type sourceType;
        private BindingExpression bindingExpression;

        static TextBox()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
        }

        public TextBox()
        {
        }

        public int? Precision
        {
            get { return (int?)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Precision.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register("Precision", typeof(int?), typeof(TextBox), new UIPropertyMetadata(null));

        public int? Scale
        {
            get { return (int?)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Scale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(int?), typeof(TextBox), new UIPropertyMetadata(null));

        private void UpdateBindingExpression()
        {
            BindingExpression expression = BindingOperations.GetBindingExpression(this, TextBox.TextProperty);

            if (expression != bindingExpression)
            {
                bindingExpression = expression;

                if ((expression != null)
                    && (expression.DataItem != null)
                    && (expression.ParentBinding != null))
                {
                    string propertyName = expression.ParentBinding.Path.Path;

                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        object source = null;

                        ICollectionView collectionView = CollectionViewSource.GetDefaultView(expression.DataItem);

                        if (collectionView != null)
                            source = collectionView.CurrentItem;
                        else
                            source = expression.DataItem;

                        if (source != null)
                        {
                            PropertyInfo info = source.GetType().GetProperty(propertyName);

                            if (info != null)
                                sourceType = info.PropertyType;

                            if (bindingExpression.ParentBinding.ConverterParameter != null &&
                                bindingExpression.ParentBinding.ConverterParameter.ToString() == "F0")

                            {
                                Scale = 0;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            UpdateBindingExpression();

            // Do not accept space when we expect numeric input.
            // Sadly space is NOT handled by the OnPreviewTextInput event.
            if (e.Key == Key.Space &&
                (sourceType == typeof(int) || sourceType == typeof(int?) ||
                 sourceType == typeof(long) || sourceType == typeof(long?) ||
                 sourceType == typeof(double) || sourceType == typeof(double?) ||
                 sourceType == typeof(decimal) || sourceType == typeof(decimal?)))
                e.Handled = true;

            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (IsReadOnly)
            {
                return;
            }

            UpdateBindingExpression();


            Regex regex = null;
            string negativeSign = Thread.CurrentThread.CurrentCulture.NumberFormat.NegativeSign;
            string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (sourceType == typeof(int) || 
                sourceType == typeof(int?) ||
                sourceType == typeof(long) || 
                sourceType == typeof(long?))
            {
                // Set precision if it hasn't been set depending on the type
                if (!Precision.HasValue)
                    Precision = 10 + (Scale.HasValue ? Scale.Value : 0);

                string pattern = string.Format(@"^{0}?(?<Precision>\d*)$", negativeSign);
                regex = new Regex(pattern);
            }
            else if (sourceType == typeof(double) || sourceType == typeof(double?))
            {
                // Set precision if it hasn't been set depending on the type
                if (!Precision.HasValue)
                    Precision = 15 + (Scale.HasValue ? Scale.Value : 0); ;

                string pattern = string.Empty;

                // If scale is set to zero then no decimals should be able to be used
                if (Scale.HasValue && Scale.Value == 0)
                    pattern = string.Format(@"^{0}?(?<Precision>\d*)$", negativeSign);
                else
                    pattern = string.Format(@"^{0}?(?<Precision>\d*)\{1}?(?<Scale>\d*)$", negativeSign, decimalSeparator);

                regex = new Regex(pattern);
            }
            else if (sourceType == typeof(decimal) || sourceType == typeof(decimal?))
            {
                // Set precision if it hasn't been set depending on the type
                if (!Precision.HasValue)
                    Precision = 38 + (Scale.HasValue ? Scale.Value : 0);

                string pattern = string.Empty;

                // If scale is set to zero then no decimals should be able to be used
                if (Scale.HasValue && Scale.Value == 0)
                    pattern = string.Format(@"^{0}?(?<Precision>\d*)$", negativeSign);
                else
                    pattern = string.Format(@"^{0}?(?<Precision>\d*)\{1}?(?<Scale>\d*)$", negativeSign, decimalSeparator);

                regex = new Regex(pattern);
            }

            if (regex != null)
            {
                // Save selection
                string orgText = Text;
                int orgSelectionStart = SelectionStart;
                int orgSelectionLength = SelectionLength;

                string text = string.Empty;

                // Check if something is selected, in that case replace with what is typed
                if (!string.IsNullOrEmpty(SelectedText))
                {
                    SelectedText = e.Text;
                    text = Text;
                }
                else
                {
                    text = Text.Insert(CaretIndex, e.Text);
                }

                Match match = regex.Match(text);

                // Check if matched regex
                if (match.Success)
                {
                    string strPrecision = match.Groups["Precision"].Value;
                    string strScale = match.Groups["Scale"].Value;

                    // Now see if we need to check Precision and Scale
                    if (!e.Handled)
                    {
                        // Only check precision and scale if scale is less than the precision.
                        if (Scale.HasValue && Scale.Value <= Precision.Value)
                        {
                            if (strPrecision.Length + strScale.Length > Precision.Value)
                                e.Handled = true;
                            else if (strPrecision.Length > Precision.Value - Scale.Value)
                                e.Handled = true;
                            else if (strScale.Length > Scale.Value)
                                e.Handled = true;
                        }
                    }
                }
                else
                    e.Handled = true;

                if (e.Handled)
                {
                    Text = orgText;
                    SelectionStart = orgSelectionStart;
                    SelectionLength = orgSelectionLength;
                }
            }

            base.OnPreviewTextInput(e);
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
                {
                    if (this.IsVisible)
                        this.SelectionLength = 0;
                }
            }
            catch (Exception) { }
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.Key == Key.LeftAlt) || (e.Key == Key.System))
            {
                if (bindingExpression != null)
                    bindingExpression.UpdateSource();
            }

            base.OnKeyDown(e);
        }

    }
}


