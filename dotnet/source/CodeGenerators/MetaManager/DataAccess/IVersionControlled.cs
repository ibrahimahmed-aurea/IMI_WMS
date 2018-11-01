using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdc.MetaManager.DataAccess
{
    public enum VersionControlledObjectStat
    {
        Default = 0,
        New = 1,
        Deleted = 2
    }
    public interface IVersionControlled : IDomainObject
    {
        bool IsLocked { get; set; }
        string LockedBy { get; set; }
        DateTime? LockedDate { get; set; }
        string RepositoryFileName { get; }
        VersionControlledObjectStat State { get; set; }
    }
}
