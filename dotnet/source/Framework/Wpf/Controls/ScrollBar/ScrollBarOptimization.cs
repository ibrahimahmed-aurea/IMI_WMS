using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Threading;

namespace Imi.Framework.Wpf.Controls
{

    public static class ScrollBarOptimization
    {
        private static Queue<ScrollBar> optimizationQueue;
        private static DispatcherTimer idleTimer;

        public static readonly DependencyProperty OptimizeCommandsProperty = DependencyProperty.RegisterAttached(
           "OptimizeCommands",
           typeof(bool),
           typeof(ScrollBarOptimization),
           new PropertyMetadata(HandleOptimizeCommandsChanged)
        );

        static ScrollBarOptimization()
        {
            optimizationQueue = new Queue<ScrollBar>();

            idleTimer = new DispatcherTimer(
            TimeSpan.FromSeconds(10),
            DispatcherPriority.ApplicationIdle, (s, e) =>
            {
                while (optimizationQueue.Count > 0)
                {
                    ScrollBar bar = optimizationQueue.Dequeue();
                    Optimize(bar);
                }
            },
            Application.Current.Dispatcher
            );

        }

        public static bool GetOptimizeCommands(DependencyObject obj)
        {
            return (bool)obj.GetValue(OptimizeCommandsProperty);
        }

        public static void SetOptimizeCommands(DependencyObject obj, bool value)
        {
            obj.SetValue(OptimizeCommandsProperty, value);
        }

        private static void HandleOptimizeCommandsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is ScrollBar))
            {
                throw new InvalidOperationException("The 'OptimizeCommands' property can only be attached to ScrollBars");
            }

            if (!(bool)e.NewValue)
            {
                return;
            }

            ScrollBar bar = (ScrollBar)sender;

            if (bar.IsLoaded)
            {
                return;
            }

            // The Buttons in the ScrollBar are defined in the ControlTemplate and not yet created
            // when the attached property is changed. Therefore the replacement of the Commands is
            // delayed until the Loaded event is raised.
            bar.Loaded += HandleScrollBarLoaded;
        }

        private static void HandleScrollBarLoaded(object sender, RoutedEventArgs e)
        {
            ScrollBar bar = (ScrollBar)sender;
            bar.Loaded -= HandleScrollBarLoaded;
            bar.SetValue(OptimizeCommandsProperty, false);
            optimizationQueue.Enqueue(bar);
        }

        private static void Optimize(ScrollBar bar)
        {
            // Some ScrollBars still don't have Buttons when the Loaded-Event is raised. We have to
            // apply the Template manually so we can replace the Commands.
            // That call isn't that bad because ApplyTemplate only does something if the Template
            // wasn't already applied.
            bar.ApplyTemplate();

            foreach (ButtonBase button in GetVisualDescendants(bar).OfType<ButtonBase>())
            {
                // The Buttons in a ScrollBar can always stay enabled, in cases when there is nothing
                // to scroll the whole ScrollBar disables itself anyway. Therefor we can use a
                // AlwaysExecutableRoutedCommand here.
                if (button.Command is RoutedCommand)
                {
                    button.Command = new AlwaysExecutableRoutedCommand((RoutedCommand)button.Command, button);
                }
            }
        }

        /// <summary>
        /// Simple recursive search for descendants in the visual tree.
        /// </summary>
        private static IEnumerable<DependencyObject> GetVisualDescendants(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                yield return child;

                foreach (DependencyObject descendant in GetVisualDescendants(child))
                {
                    yield return descendant;
                }
            }
        }
    }
}
