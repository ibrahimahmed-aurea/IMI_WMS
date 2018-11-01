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
using Microsoft.Practices.ObjectBuilder;
using Controls = Imi.Framework.Wpf.Controls;
using Xceed.Wpf.DataGrid;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.CompositeUI.Utility;
using System.Collections.Specialized;
using System.Collections;
using Imi.Framework.UX.Wpf;
using Xceed.Wpf.DataGrid.Views;

namespace Imi.SupplyChain.UX.Views
{
    public partial class InlineDetailView : UserControl, IInlineDetailView
    {
        private FrameworkElement inlineElement;
        private Controls.DataGrid grid;
        private OrderedDictionary actionDictionary;

        public InlineDetailView()
        {
            InitializeComponent();

            actionDictionary = new OrderedDictionary();

            this.Loaded += (s, e) =>
            {
                if (grid == null)
                {
                    DependencyObject d = this;

                    while (d != null)
                    {
                        d = VisualTreeHelper.GetParent(d);

                        if (d is Controls.DataGrid)
                        {
                            grid = d as Controls.DataGrid;
                            break;
                        }
                    }

                    if (grid != null)
                    {
                        grid.GotKeyboardFocus += GotKeyboardFocusEventHandler;
                    }
                }
            };
        }

        private void GotKeyboardFocusEventHandler(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OriginalSource is FixedCellPanel && grid.IsKeyboardFocusWithin && actionDictionary.Count > 0)
            {

                if (!Framework.Wpf.Controls.ControlHelper.IsControlOnView(e.OriginalSource, inlineElement))
                {
                    return;
                }

                e.Handled = true;
                FocusElement();
            }
        }

        private InlineDetailPresenter presenter;

        [CreateNew]
        public InlineDetailPresenter Presenter
        {
            get { return presenter; }
            set
            {
                presenter = value;
                presenter.View = this;
            }
        }

        public void UpdateActions()
        {
            IDictionaryEnumerator enumerator = actionDictionary.GetEnumerator();

            while (enumerator.MoveNext())
            {
                ((Button)enumerator.Value).IsEnabled = Presenter.CanActionExecute((string)enumerator.Key);
            }
        }

        public void AddAction(string actionName, string caption, bool moveNextLine)
        {
            Button button = new Button();
            button.Content = caption;
            button.Margin = new Thickness(6, 0, 0, 0);
            button.MinHeight = 22;
            button.MinWidth = 80;
            actionDictionary.Add(actionName, button);

            button.Click += (s, e) =>
            {
                inlineElement.Focus();

                ExecuteAction(actionName, moveNextLine);

            };

            buttonPanel.Children.Add(button);
        }


        private void ExecuteAction(string actionName, bool moveNext)
        {
            inlineDetailWorkspace.Focus();

            if (presenter.ExecuteAction(actionName))
            {
                if (moveNext)
                    MoveNext();
            }
            else
            {
                FocusElement();
            }
        }

        public void ShowInDetailWorkspace(object view)
        {
            inlineElement = view as FrameworkElement;
            inlineDetailWorkspace.Show(view);
        }

        private void FocusElement()
        {
            IDataView view = inlineElement as IDataView;

            if (view != null)
            {
                view.SetFocus();
            }
            else
                inlineElement.Focus();
        }

        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && !((e.OriginalSource is Button) || (e.OriginalSource is ListBoxItem)))
            {
                e.Handled = true;

                IDictionaryEnumerator enumerator = actionDictionary.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (((Button)enumerator.Value).IsEnabled)
                    {
                        ((Button)enumerator.Value).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        break;
                    }
                }
            }
        }


        private void PreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (!Framework.Wpf.Controls.ControlHelper.IsControlOnView(e.OriginalSource, inlineElement))
            {
                return;
            }

            DependencyObject scope = FocusManager.GetFocusScope(inlineElement);
            UIElement currentElement = FocusManager.GetFocusedElement(scope) as UIElement;

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.Down)
                {
                    e.Handled = true;

                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Down);

                    if (currentElement != null)
                    {
                        if (currentElement.MoveFocus(request))
                        {
                            if (!inlineGrid.IsKeyboardFocusWithin)
                                MoveNext();
                        }
                    }
                }
                else if (e.Key == Key.Up)
                {
                    e.Handled = true;

                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Up);

                    if (currentElement != null)
                    {
                        if (currentElement.MoveFocus(request))
                        {
                            if (!inlineGrid.IsKeyboardFocusWithin)
                                MovePrevious();
                        }
                    }
                }
                else if (e.Key == Key.Left)
                {
                    e.Handled = true;

                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Left);

                    if (currentElement != null)
                        currentElement.MoveFocus(request);

                }
                else if (e.Key == Key.Right)
                {
                    e.Handled = true;

                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Right);

                    if (currentElement != null)
                        currentElement.MoveFocus(request);
                }
            }
            else
            {
                if (e.OriginalSource is Selector || e.OriginalSource is ListBoxItem)
                    return;

                if (e.Key == Key.Down)
                {
                    e.Handled = true;
                    MoveNext();
                }
                else if (e.Key == Key.Up)
                {
                    e.Handled = true;
                    MovePrevious();
                }
            }
        }

        private void MoveNext()
        {
            if (grid.SelectedIndex < grid.Items.Count - 1)
                grid.SelectedIndex++;

            grid.CurrentItem = grid.SelectedItem;
        }

        private void MovePrevious()
        {
            if (grid.SelectedIndex > 0)
                grid.SelectedIndex--;

            grid.CurrentItem = grid.SelectedItem;
        }
    }
}
