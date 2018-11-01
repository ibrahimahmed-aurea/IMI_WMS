using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cdc.MetaManager.CodeGeneration.CodeSmithTemplates;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Cdc.MetaManager.DataAccess.Domain;
using Imi.SupplyChain.UX.Rules;
using System.Reflection;
using System.IO;
using System.Workflow.ComponentModel;
using System.CodeDom;

namespace Cdc.MetaManager.BusinessLogic
{
    public abstract class WorkflowTypeFactory
    {
        public static string GetDialogActivityClassName(Dialog dialog)
        {
            DialogActivityTemplate dialogTemplate = new DialogActivityTemplate();
            return dialogTemplate.GetDialogActivityClassName(dialog);
        }

        public static string GetServiceMethodActivityClassName(ServiceMethod serviceMethod)
        {
            ServiceMethodActivityTemplate serviceMethodTemplate = new ServiceMethodActivityTemplate();
            return serviceMethodTemplate.GetServiceMethodActivityClassName(serviceMethod);
        }

        public static Type CreateRequestType(Workflow workflow)
        {
            IList<string> sources = new List<string>();

            ParameterClassTemplate parameterClassTemplate = new ParameterClassTemplate();
            parameterClassTemplate.SetProperty("propertyMap", workflow.RequestMap);
            parameterClassTemplate.SetProperty("parameterClassName", parameterClassTemplate.GetWorkflowParametersClassName(workflow));
            parameterClassTemplate.SetProperty("parameterClassNamespace", parameterClassTemplate.GetWorkflowNamespace(workflow));
            parameterClassTemplate.SetProperty("addRowIdentity", false);
            sources.Add(parameterClassTemplate.RenderToString());

            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });

            CompilerParameters options = new CompilerParameters();

            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Core.dll");
            
            CompilerResults results = provider.CompileAssemblyFromSource(options, sources.ToArray());

            return results.CompiledAssembly.GetTypes()[0];

        }

        public static IList<Type> CreateActivities(Workflow workflow, Application backendApplication, IEnumerable<Dialog> dialogs, IEnumerable<ServiceMethod> serviceMethods, IEnumerable<Workflow> subworkflows)
        {
            IList<string> sources = new List<string>();
                        
            AlarmActivityTemplate alarmTemplate = new AlarmActivityTemplate();
            alarmTemplate.SetProperty("module", workflow.Module);
            alarmTemplate.SetProperty("isDesignTime", true);
            alarmTemplate.SetProperty("backendApplication", backendApplication);
            sources.Add(alarmTemplate.RenderToString());

            DialogActivityTemplate dialogTemplate = new DialogActivityTemplate();
            
            foreach (Dialog dialog in dialogs)
            {
                WorkflowDialog workflowDialog = new WorkflowDialog();
                workflowDialog.Workflow = workflow;
                workflowDialog.Dialog = dialog;
                dialogTemplate.SetProperty("workflowDialog", workflowDialog);
                dialogTemplate.SetProperty("isDesignTime", true);
                sources.Add(dialogTemplate.RenderToString());
            }

            ServiceMethodActivityTemplate serviceTemplate = new ServiceMethodActivityTemplate();
                        
            foreach (ServiceMethod serviceMethod in serviceMethods)
            {
                WorkflowServiceMethod workflowServiceMethod = new WorkflowServiceMethod();
                workflowServiceMethod.Workflow = workflow;
                workflowServiceMethod.ServiceMethod = serviceMethod;
                serviceTemplate.SetProperty("workflowServiceMethod", workflowServiceMethod);
                serviceTemplate.SetProperty("isDesignTime", true);
                sources.Add(serviceTemplate.RenderToString());
            }

            SubworkflowActivityTemplate subworkflowTemplate = new SubworkflowActivityTemplate();

            foreach (Workflow subworkflow in subworkflows)
            {
                WorkflowSubworkflow workflowSubworkflow = new WorkflowSubworkflow();
                workflowSubworkflow.Workflow = workflow;
                workflowSubworkflow.SubWorkflow = subworkflow;

                subworkflowTemplate.SetProperty("workflowSubworkflow", workflowSubworkflow);
                subworkflowTemplate.SetProperty("isDesignTime", true);
                sources.Add(subworkflowTemplate.RenderToString());
            }

            ActivityThemeTemplate themeTemplate = new ActivityThemeTemplate();
            themeTemplate.SetProperty("workflow", workflow);
            sources.Add(themeTemplate.RenderToString());

            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });

            CompilerParameters options = new CompilerParameters();

            //options.TempFiles = new TempFileCollection("c:\\temp\\files\\", true);
            options.OutputAssembly = string.Format("Workflow_{0}.dll", Guid.NewGuid().ToString());
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Core.dll");
            options.ReferencedAssemblies.Add("System.Drawing.dll");
            options.ReferencedAssemblies.Add("System.Workflow.ComponentModel.dll");
            options.ReferencedAssemblies.Add("System.Workflow.Activities.dll");

            CompilerResults results = provider.CompileAssemblyFromSource(options, sources.ToArray());

            var activityTypes = new List<Type>();

            foreach (Type type in results.CompiledAssembly.GetTypes())
            {
                if (type.BaseType == typeof(Activity))
                    activityTypes.Add(type);
                
            }

            return activityTypes;
        }
        
        public static string GetWorkflowClassFullName(Workflow workflow)
        { 
            DialogActivityTemplate template = new DialogActivityTemplate();
            return string.Format("{0}.{1}", template.GetWorkflowNamespace(workflow), template.GetWorkflowClassName(workflow));
        }
    }
}
