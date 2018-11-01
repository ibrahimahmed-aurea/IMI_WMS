using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.Framework.Wpf.Controls;
using ActiproSoftware.Windows.Controls.Navigation;
using ActiproSoftware.Windows.Controls.Navigation.Serialization;
using Imi.Framework.UX.Settings;
using System.Runtime.Serialization;

namespace Imi.SupplyChain.UX.Shell.Settings
{
    public class NavigationBarSettingsProvider : ISettingsProvider
    {
        public NavigationBarSettingsProvider()
        { 
        }
                
        public void LoadSettings(object target, object settings)
        {
            if (settings != null)
            {
                NavigationBarLayoutSerializer serializer = new NavigationBarLayoutSerializer();

                try
                {
                    serializer.LoadFromString((string)settings);
                    serializer.ApplyTo((NavigationBar)target);
                }
                catch (SerializationException)
                {
                }
            }
            else
            {
                if (target is NavigationBar)
                {
                    NavigationBar navigationBar = (NavigationBar)target;
                    bool changed = true;

                    while (changed)
                    {
                        changed = false;
                        int lastIndex = -1;
                        for (int index = 0; index < navigationBar.Items.Count; index++)
                        {
                            int orderIndex = ((Imi.SupplyChain.UX.Infrastructure.IShellModule) ((NavigationPane)navigationBar.Items[index]).Tag).OrderIndex;

                            if (orderIndex < lastIndex)
                            {
                                NavigationPane previousItem = (NavigationPane)navigationBar.Items[index - 1];
                                NavigationPane currentItem = (NavigationPane)navigationBar.Items[index];

                                navigationBar.Items.RemoveAt(index);
                                navigationBar.Items.Insert(index - 1, currentItem);
                                
                                changed = true;
                            }

                            lastIndex = orderIndex;
                        }
                    }
                }
            }
        }

        public object SaveSettings(object target)
        {
            NavigationBarLayoutSerializer serializer = new NavigationBarLayoutSerializer();
            return serializer.SaveToString((NavigationBar)target);
        }
        
        public Type GetSettingsType(object target)
        {
            return typeof(string);
        }

        public string GetKey(object target)
        {
            return ((NavigationBar)target).Name;
        }
    }
}
