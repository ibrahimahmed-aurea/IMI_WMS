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
    public class ComboDialog : TextBox
    {
        public UIElement DropDownContent
        {
            get { return (UIElement)GetValue(DropDownContentProperty); }
            set { SetValue(DropDownContentProperty, value); }
        }

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEditable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboDialog), new UIPropertyMetadata(true));


        // Using a DependencyProperty as the backing store for DropdownContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropDownContentProperty =
            DependencyProperty.Register("DropDownContent", typeof(UIElement), typeof(ComboDialog), new UIPropertyMetadata(null, DropDownContentChangedCallback));

        public bool IsDialogOpen
        {
            get { return (bool)GetValue(IsDialogOpenProperty); }
            set { SetValue(IsDialogOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDialogOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDialogOpenProperty =
            DependencyProperty.Register("IsDialogOpen", typeof(bool), typeof(ComboDialog), new UIPropertyMetadata(false, IsDialogOpenChangedCallback));

        #region Published Events

        public static readonly RoutedEvent DialogButtonClickEvent =
                        EventManager.RegisterRoutedEvent("DialogButtonClick",
                                                         RoutingStrategy.Bubble,
                                                         typeof(RoutedEventHandler),
                                                         typeof(ComboDialog));

        public event RoutedEventHandler DialogButtonClick
        {
            add { AddHandler(DialogButtonClickEvent, value); }
            remove { RemoveHandler(DialogButtonClickEvent, value); }
        }

        #endregion

        private Popup popup;
        private ContentPresenter dropDownContentHost;

        static ComboDialog()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboDialog), new FrameworkPropertyMetadata(typeof(ComboDialog)));
        }

        private static void DropDownContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as ComboDialog).dropDownContentHost != null)
                (d as ComboDialog).dropDownContentHost.Content = (e.NewValue as UIElement);
        }

        private static void IsDialogOpenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as ComboDialog).popup != null)
            {
                (d as ComboDialog).popup.IsOpen = (bool)e.NewValue;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ToggleButton dropDownButton = Template.FindName("PART_DropDownButton", this) as ToggleButton;

            popup = Template.FindName("PART_Popup", this) as Popup;

            if (!IsEditable)
                this.IsReadOnly = true;

            DropDownContent.SetValue(TextBoxBase.IsReadOnlyProperty, false);

            popup.Opened += (s, e) =>
            {
                DropDownContent.Focus();
            };

            popup.RequestBringIntoView += new RequestBringIntoViewEventHandler(popup_RequestBringIntoView);

            if (dropDownButton != null)
            {
                dropDownButton.Click += (s, e) =>
                {
                    e.Handled = true;
                    RaiseEvent(new RoutedEventArgs(DialogButtonClickEvent, this));
                };
            }

            Thumb thumb = Template.FindName("PART_ResizeGrip", this) as Thumb;

            if (thumb != null)
            {
                thumb.DragDelta += DragDeltaEventHandler;
            }

            dropDownContentHost = Template.FindName("PART_DropDownContentHost", this) as ContentPresenter;

            if (dropDownContentHost != null)
                dropDownContentHost.Content = DropDownContent;
        }

        //Fix size of popup window when search panel is to big
        private bool subsequentCall = false;
        void popup_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            if (!subsequentCall)
            {
                if (DropDownContent != null)
                {
                    if (DropDownContent.GetType().GetProperty("ActiveSmartPart") != null)
                    {
                        object smartPart = DropDownContent.GetType().GetProperty("ActiveSmartPart").GetValue(DropDownContent, null);
                        if (smartPart != null)
                        {
                            subsequentCall = true;

                            if (smartPart is UserControl)
                            {
                                if (((UserControl)smartPart).Content != null)
                                {
                                    object content = ((UserControl)smartPart).Content;

                                    if (content != null && content is Grid)
                                    {
                                        Grid grid = (Grid)content;

                                        if (grid.RowDefinitions.Count == 2)
                                        {

                                            RowDefinition rowDef = grid.RowDefinitions[0];

                                            if (rowDef.ActualHeight > (popup.Height / 3))
                                            {
                                                popup.Height = rowDef.ActualHeight + 200;
                                                double newWidth = (rowDef.ActualHeight + 200) * 1.618;
                                                if (newWidth > popup.Width)
                                                {
                                                    popup.Width = newWidth;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                if (e.Key == Key.Insert)
                {
                    e.Handled = true;
                    RaiseEvent(new RoutedEventArgs(DialogButtonClickEvent, this));
                    IsDialogOpen = true;
                }

                if (!IsEditable)
                {
                    if ((e.Key == Key.Delete) || (e.Key == Key.Back))
                        Text = "";
                }
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectAll();
            e.Handled = true;
        }

        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);
            e.Handled = true;
            if (this.IsFocused && !IsEditable && string.IsNullOrEmpty(this.SelectedText) && !string.IsNullOrEmpty(this.Text))
            {
                this.SelectAll();
            }
        }

        private void DragDeltaEventHandler(object sender, DragDeltaEventArgs e)
        {
            if (popup != null)
            {
                double x = popup.Width + e.HorizontalChange;
                double y = popup.Height + e.VerticalChange;

                if ((x >= 0) && (y >= 0))
                {
                    popup.Width = x;
                    popup.Height = y;
                }
            }
        }

    }
}
