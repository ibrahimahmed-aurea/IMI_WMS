using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.Framework.UX.Settings
{
    public interface ISettingsProvider
    {
        void LoadSettings(object target, object settings);
        object SaveSettings(object target);
        Type GetSettingsType(object target);
        string GetKey(object target);
    }
}
