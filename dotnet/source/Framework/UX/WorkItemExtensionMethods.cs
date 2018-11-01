using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeUI;

namespace Imi.Framework.UX
{
    public static class WorkItemExtensionMethods
    {
        public static IList<WorkItem> GetDescendants(this WorkItem workItem)
        {
            return GetDescendantsReceursive(workItem);
        }

        private static IList<WorkItem> GetDescendantsReceursive(WorkItem workItem)
        {
            List<WorkItem> workItems = new List<WorkItem>();

            foreach (var subWorkItem in workItem.WorkItems)
            {
                workItems.Add(subWorkItem.Value);
                workItems.AddRange(GetDescendantsReceursive(subWorkItem.Value));
            }

            return workItems;
        }

    }
}
