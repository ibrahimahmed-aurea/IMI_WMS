using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Microsoft.Practices.CompositeUI;
using System.Diagnostics;
using Imi.SupplyChain.UX;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Media;
using Microsoft.Practices.ObjectBuilder;
using Imi.SupplyChain.UX.Infrastructure;
using Imi.SupplyChain.UX.Infrastructure.Services;
using System.Reflection;

namespace Imi.SupplyChain.UX.Shell.Services
{
    /// <summary>
    /// An implementation of <see cref="IFrameworkElementActivationService"/>.
    /// </summary>
    public class ActionActivationService : IActionActivationService, IBuilderAware
    {
        private IDictionary actionDictionary;

        public ActionActivationService()
        {
            actionDictionary = new OrderedDictionary();
        }

        [ServiceDependency]
        public IShellInteractionService ShellInteractionService
        {
            get;
            set;
        }

        public void FrameworkElementAdded(FrameworkElement frameworkElement)
        {
            if (frameworkElement is IActionProvider)
            {
                if (!actionDictionary.Contains(frameworkElement))
                    actionDictionary.Add(frameworkElement, null);

                UpdateActions();

                frameworkElement.IsVisibleChanged += IsVisibleChangedEventHandler;
            }
        }

        private void IsVisibleChangedEventHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateActions();
        }

        public void FrameworkElementRemoved(FrameworkElement frameworkElement)
        {
            if (frameworkElement is IActionProvider)
            {
                frameworkElement.IsVisibleChanged -= IsVisibleChangedEventHandler;

                //Force removal of all actions for view
                if (actionDictionary.Contains(frameworkElement))
                {
                    ICollection<ShellAction> actionCollection = actionDictionary[frameworkElement] as ICollection<ShellAction>;

                    if (actionCollection != null)
                    {
                        foreach (ShellAction action in actionCollection)
                            ShellInteractionService.Actions.Remove(action);
                    }

                    actionDictionary.Remove(frameworkElement);
                }
            }
        }

        public void UpdateActions()
        {
            var actions = from key in actionDictionary.Keys.Cast<FrameworkElement>()
                          select new KeyValuePair<FrameworkElement, ICollection<ShellAction>>(key, (ICollection<ShellAction>)actionDictionary[key]);

            //Rebuild actions
            foreach (KeyValuePair<FrameworkElement, ICollection<ShellAction>> item in actions.ToArray())
            {
                FrameworkElement frameworkElement = item.Key;
                ICollection<ShellAction> actionCollection = item.Value;

                if (actionCollection != null)
                {
                    foreach (ShellAction action in actionCollection)
                        ShellInteractionService.Actions.Remove(action);
                }

                DependencyObject parent = frameworkElement;
                bool isActive = false;

                while (parent != null)
                {
                    if (parent == ShellInteractionService.ActiveSmartPart)
                    {
                        isActive = true;
                        break;
                    }

                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (frameworkElement.IsVisible && isActive)
                {
                    //Refresh actions for visible views
                    IActionProvider actionProvider = frameworkElement as IActionProvider;

                    if (actionCollection == null)
                    {
                        actionCollection = actionProvider.GetActions();

                        if (actionCollection == null)
                            actionCollection = new List<ShellAction>();

                        actionDictionary[frameworkElement] = actionCollection;
                    }

                    PropertyInfo isDetailViewProperty = frameworkElement.GetType().GetProperty("IsDetailView");

                    bool isDetailAction = false;
                    if (isDetailViewProperty != null)
                    {
                        if ((bool)isDetailViewProperty.GetValue(frameworkElement, null))
                        {
                            isDetailAction = true;
                        }
                    }

                    foreach (ShellAction action in actionCollection)
                    {
                        action.IsDetailAction = isDetailAction;
                        ShellInteractionService.Actions.Add(action);
                    }

                }
            }
        }

        #region IBuilderAware Members

        public void OnBuiltUp(string id)
        {
            ShellInteractionService.SmartPartActivated += (s, e) =>
            {
                UpdateActions();
            };
        }

        public void OnTearingDown()
        {

        }

        #endregion
    }
}
