using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using Imi.Framework.UX.Wpf.Workspaces;
using Imi.Framework.UX.Services;
using Imi.Framework.UX.Settings;
using System.Windows.Input;

namespace Imi.Framework.UX.Wpf.BuilderStrategies
{
	/// <summary>
	/// Implements a <see cref="BuilderStrategy"/> for <see cref="FrameworkElement"/> smart parts.
	/// </summary>
	public class FrameworkElementStrategy : BuilderStrategy
	{
        
        private WorkItem GetWorkItem(IReadableLocator locator)
		{
			return locator.Get<WorkItem>(new DependencyResolutionLocatorKey(typeof(WorkItem), null));
		}

		public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
		{
			if (existing is FrameworkElement)
			{
				AddHierarchy(GetWorkItem(context.Locator), existing as FrameworkElement);
			}

			return base.BuildUp(context, t, existing, id);
		}

		public override object TearDown(IBuilderContext context, object item)
		{
            WorkItem workItem = GetWorkItem(context.Locator);

            if (workItem.Status != WorkItemStatus.Terminated)
            {
                if (item is FrameworkElement)
                {
                    RemoveHierarchy(GetWorkItem(context.Locator), item as FrameworkElement);
                }
            }

			return base.TearDown(context, item);
		}

		private void AddHierarchy(WorkItem workItem, FrameworkElement frameworkElement)
		{
			ReplaceIfPlaceHolder(workItem, frameworkElement);

            AddSettingsProvider(workItem, frameworkElement);

            AddScanningControl(workItem, frameworkElement);
                                    
			foreach (object child in LogicalTreeHelper.GetChildren(frameworkElement))
			{
				if (!AddToWorkItem(workItem, child))
				{
					if (child is FrameworkElement)
					{
						AddHierarchy(workItem, child as FrameworkElement);
					}
				}
			}
		}

		private void RemoveHierarchy(WorkItem workItem, FrameworkElement frameworkElement)
		{
			if (frameworkElement != null)
			{
				RemoveNested(workItem, frameworkElement);
			}
		}

		private bool AddToWorkItem(WorkItem workItem, object item)
		{
            FrameworkElement frameworkElement = item as FrameworkElement;

            if (frameworkElement != null)
			{
                bool ignore = (bool)frameworkElement.GetValue(FrameworkElementStrategySettings.IsIgnoredProperty);

                if (!ignore)
                {
                    if (ShouldAddToWorkItem(workItem, frameworkElement))
                    {
                        if ((frameworkElement != null) && (!string.IsNullOrEmpty(frameworkElement.Name)))
                        {
                            workItem.Items.Add(frameworkElement, frameworkElement.Name);
                        }
                        else
                        {
                            workItem.Items.Add(frameworkElement);
                        }

                        return true;
                    }
                }
                else
                    return true;
			}

			return false;
		}

		private bool ShouldAddToWorkItem(WorkItem workItem, object item)
		{
            return !workItem.Items.ContainsObject(item) && (IsSmartPart(item) || IsWorkspace(item) || IsPlaceholder(item));
		}

		private bool IsPlaceholder(object item)
		{
			return (item is ISmartPartPlaceholder);
		}

		private bool IsSmartPart(object item)
		{
			return (item.GetType().GetCustomAttributes(typeof(SmartPartAttribute), true).Length > 0);
		}

		private bool IsWorkspace(object item)
		{
			return (item is IWorkspace);
		}

		private void RemoveNested(WorkItem workItem, FrameworkElement frameworkElement)
		{
            RemoveSettingsProvider(workItem, frameworkElement);

            RemoveScanningControl(workItem, frameworkElement);

            foreach (object child in LogicalTreeHelper.GetChildren(frameworkElement))
			{
                workItem.Items.Remove(child);

				if (child is FrameworkElement)
				{
					RemoveNested(workItem, child as FrameworkElement);
				}
			}
		}

		private void ReplaceIfPlaceHolder(WorkItem workItem, FrameworkElement frameworkElement)
		{
			ISmartPartPlaceholder placeholder = frameworkElement as ISmartPartPlaceholder;

			if (placeholder != null)
			{
				FrameworkElement replacement = workItem.Items.Get<FrameworkElement>(placeholder.SmartPartName);

				if (replacement != null)
				{
					placeholder.SmartPart = replacement;
				}
			}
		}

        private void AddSettingsProvider(WorkItem workItem, FrameworkElement frameworkElement)
        {
            Type providerType = FrameworkElementStrategySettings.GetSettingsProviderType(frameworkElement);

            if (providerType != null)
            {
                if (!string.IsNullOrEmpty(frameworkElement.Name))
                {
                    IUXSettingsService settingsService = workItem.Services.Get<IUXSettingsService>();

                    ISettingsProvider provider = Activator.CreateInstance(providerType) as ISettingsProvider;

                    settingsService.AddProvider(frameworkElement, provider);
                }
            }
        }

        private void RemoveSettingsProvider(WorkItem workItem, FrameworkElement frameworkElement)
        {
            Type providerType = FrameworkElementStrategySettings.GetSettingsProviderType(frameworkElement);

            if (providerType != null)
            {
                if (!string.IsNullOrEmpty(frameworkElement.Name))
                {
                    IUXSettingsService settingsService = workItem.Services.Get<IUXSettingsService>();
                                        
                    settingsService.RemoveProvider(frameworkElement);
                }
            }
        }

        private void AddScanningControl(WorkItem workItem, FrameworkElement frameworkElement)
        {
            bool enableScanning = FrameworkElementStrategySettings.GetEnableScanning(frameworkElement);

            if (enableScanning)
            {
                string applicationIdentifier = FrameworkElementStrategySettings.GetApplicationIdentifier(frameworkElement);
                Key completeScanKey = FrameworkElementStrategySettings.GetCompleteScanKey(frameworkElement);

                if (!workItem.Services.Contains(typeof(Services.TextControlScannerService)))
                {
                    workItem.Services.Add<Services.TextControlScannerService>(new Services.TextControlScannerService());
                }

                workItem.Services.Get<Services.TextControlScannerService>().AddControl(frameworkElement, applicationIdentifier, completeScanKey);
            }
        }

        private void RemoveScanningControl(WorkItem workItem, FrameworkElement frameworkElement)
        {
            bool enableScanning = FrameworkElementStrategySettings.GetEnableScanning(frameworkElement);
            if (enableScanning)
            {
                if (workItem.Services.Contains(typeof(Services.TextControlScannerService)))
                {
                    workItem.Services.Get<Services.TextControlScannerService>().RemoveControl(frameworkElement);
                }
            }
        }
	}
}
