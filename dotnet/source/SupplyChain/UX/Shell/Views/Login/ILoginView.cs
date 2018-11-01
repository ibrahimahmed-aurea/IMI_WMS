using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imi.SupplyChain.UX.Shell.Configuration;
using System.Globalization;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface ILoginView
    {
        void SetLanguages(LanguageCollection languages, CultureInfo defaultCulture);
    }
}
