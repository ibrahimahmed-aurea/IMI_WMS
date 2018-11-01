using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Imi.Framework.UX.Settings;

namespace Imi.Framework.UX.Services
{
    public interface IUXSettingsService
    {
        void SaveSettings();
        void LoadSettings();
        void ApplySettings();
        void ApplySettings(object target);
        void AddProvider(object target, ISettingsProvider provider);
        void RemoveProvider(object target);
    }
}
