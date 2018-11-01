using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Shell.Views
{
    public interface IProgressView
    {
        void SetBounds(double left, double top, double width, double height);
        void Show();
        void Hide();
        void Close();
    }
}
