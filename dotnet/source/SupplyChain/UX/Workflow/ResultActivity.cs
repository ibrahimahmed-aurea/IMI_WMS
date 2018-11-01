using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Reflection;

namespace Imi.SupplyChain.UX.Workflow
{
    [Designer(typeof(ResultActivityDesigner))]
    public class ResultActivity: Activity
	{
        public WorkflowResult Result
        {
            get { return (WorkflowResult)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Result.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(WorkflowResult), typeof(ResultActivity), new PropertyMetadata(WorkflowResult.Update));
        
        public ResultActivity()
		{
		}

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Activity activity = this;

            while (activity.Parent != null)
            {
                activity = activity.Parent;
            }

            PropertyInfo propertyInfo = activity.GetType().GetProperty("Result");
            propertyInfo.SetValue(activity, Result, null);

            return base.Execute(executionContext);
        }
	}
}
