using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;

namespace Cdc.MetaManager.DataAccess
{
    public interface IRuledObject
    {
        string RuleSetXml { get; set; }
        RuleSet RuleSet { get; set; }
    }
}
