using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Imi.Framework.Wpf.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.Wpf.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Imi.Framework.Wpf.Controls;assembly=Imi.Framework.Wpf.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:NullableComboBox/>
    ///
    /// </summary>
    public class NullableComboBox : ComboBox
    {
        static NullableComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NullableComboBox), new FrameworkPropertyMetadata(typeof(NullableComboBox)));
        }

        public NullableComboBox()
        {
        }
                
        public bool IsNullable
        {
            get { return (bool)GetValue(IsNullableProperty); }
            set { SetValue(IsNullableProperty, value); }
        }
                        
        public bool SelectFirstItem
        {
            get { return (bool)GetValue(SelectFirstItemProperty); }
            set { SetValue(SelectFirstItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectFirstItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectFirstItemProperty =
            DependencyProperty.Register("SelectFirstItem", typeof(bool), typeof(NullableComboBox), new UIPropertyMetadata(false));
                       
        // Using a DependencyProperty as the backing store for IsNullable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNullableProperty =
            DependencyProperty.Register("IsNullable", typeof(bool), typeof(NullableComboBox), new UIPropertyMetadata(false));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button clearButton = Template.FindName("PART_ClearButton", this) as Button;

            if (clearButton != null)
                clearButton.Click += ClearButtonClickEventHandler;
        }
        
        private void ClearButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (IsNullable)
            {
                this.SelectedItem = null;

                if (this.IsDropDownOpen)
                    this.IsDropDownOpen = false;
            }
        }
                        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Delete && IsNullable)
                ClearButtonClickEventHandler(this, new RoutedEventArgs());
        }
        
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            BindingExpression expression = BindingOperations.GetBindingExpression(this, NullableComboBox.SelectedValueProperty);

            if (expression != null)
            {
                expression.UpdateTarget();
            }

            object selectedValue = this.SelectedValue;
            
            base.OnItemsChanged(e);

            this.SelectedValue = selectedValue;

            if (this.SelectedItem == null)
                this.SelectedValue = null;

            if (SelectFirstItem && SelectedValue == null && Items.Count > 0)
                SelectedIndex = 0;
        }
    }
}
