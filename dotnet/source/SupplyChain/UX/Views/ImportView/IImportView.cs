using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

namespace Imi.SupplyChain.UX.Views
{
    public interface IImportView : IDataView
    {
        Type DataType { get; set; }
        object Data { get; set; }
        bool ShowOpenFile { get; set; }
        string FileExtension { get; set; }
        List<string> ImportParameters { get; set; }
    }
}
