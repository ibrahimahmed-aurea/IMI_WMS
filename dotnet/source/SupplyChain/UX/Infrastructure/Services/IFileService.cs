using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imi.SupplyChain.UX.Infrastructure.Services
{
    public interface IFileService
    {
        IEnumerable<string> GetFileNames();
        string GetFile(string fileName);
    }
}
