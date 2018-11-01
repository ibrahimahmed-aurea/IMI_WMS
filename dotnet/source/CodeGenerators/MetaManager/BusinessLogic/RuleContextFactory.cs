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

namespace Cdc.MetaManager.BusinessLogic
{
    public abstract class RuleContextFactory
    {
        public static Type GetTypeFromName(string name)
        {
            string[] names = name.Split(new char[] { ',' });
            string typeName = names[0].Trim();
            string fileName = names[1].Trim();

            if(string.IsNullOrEmpty(typeName))
                throw new ArgumentException(string.Format("Bad type name ({0}) extracted from ",(typeName ?? "<null>"), name),"typeName");

            if(string.IsNullOrEmpty(fileName))
                throw new ArgumentException(string.Format("Bad dll file name ({0}) extracted from ",(fileName ?? "<null>"), name),"fileName");


            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.dll", fileName));
            Assembly assembly = null;

            try
            {
                assembly = Assembly.LoadFrom(fileName);
                
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Cannot load type ({0}) from dll {1} ", (typeName ?? "<null>"), fileName), "typeName", ex);
            }

            System.Type[] types = assembly.GetTypes();
            Type loadType = assembly.GetType(typeName);

            if(loadType == null)
                throw new ArgumentException(string.Format("Cannot load type ({0}) from dll {1} ", (typeName ?? "<null>"), fileName), "typeName");

            return loadType;
        }

        public static Type LoadUserSessionType(Application application)
        {
            UXSession session = MetaManagerServices.GetUXSessionByApplicationId(application.Id);
            try
            {
                Type usType = GetTypeFromName(session.UserSessionTypeName);
                return usType;
            }
            catch (ArgumentException age)
            {
                throw new ArgumentException(string.Format("Failed to load UserSessonService interface for Application with Id {0}, looking for {1}",
                                                             application.Id, session.UserSessionTypeName), "Application.Session.UserSessionTypeName", age);
            }

        }

        public static Type CreateComponentContext(View view)
        {
            Type userSessionType = LoadUserSessionType(view.Application);

            ParameterClassTemplate template = new ParameterClassTemplate();
            template.SetProperty("propertyMap", view.ResponseMap);
            template.SetProperty("parameterClassName", template.GetViewResultClassName(view));
            template.SetProperty("parameterClassNamespace", template.GetViewsNamespace(view));

            string source = template.RenderToString();

            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });

            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Core.dll");

            CompilerResults results = provider.CompileAssemblyFromSource(options, source);

            Type resultType = results.CompiledAssembly.GetTypes()[0];

            Type ruleContextType = typeof(ComponentRuleContext<,>);

            return ruleContextType.MakeGenericType(resultType, userSessionType);
        }
        
        public static Type CreateBindableComponentContext(View view, MappedProperty property)
        {
            Type userSessionType = LoadUserSessionType(view.Application);

            ParameterClassTemplate template = new ParameterClassTemplate();
            template.SetProperty("propertyMap", view.ResponseMap);
            template.SetProperty("parameterClassName", template.GetViewResultClassName(view));
            template.SetProperty("parameterClassNamespace", template.GetViewsNamespace(view));
                                    
            string source = template.RenderToString();

            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });

            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Core.dll");

            CompilerResults results = provider.CompileAssemblyFromSource(options, source);
            
            Type valueType = property.Type;

            if (property.Type != typeof(string))
                valueType = typeof(Nullable<>).MakeGenericType(property.Type);

            Type resultType = results.CompiledAssembly.GetTypes()[0];

            Type ruleContextType = typeof(ViewComponentRuleContext<,,>);
            
            return ruleContextType.MakeGenericType(resultType, valueType, userSessionType);
        }

        public static Type CreateViewContext(ViewNode viewNode)
        {
            Type userSessionType = LoadUserSessionType(viewNode.View.Application);
                        
            ParameterClassTemplate template = new ParameterClassTemplate();
            template.SetProperty("propertyMap", viewNode.View.ResponseMap);
            template.SetProperty("parameterClassName", template.GetViewResultClassName(viewNode.View));
            template.SetProperty("parameterClassNamespace", template.GetViewsNamespace(viewNode.View));

            string source = template.RenderToString();

            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });

            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Core.dll");

            CompilerResults results = provider.CompileAssemblyFromSource(options, source);

            Type resultType = results.CompiledAssembly.GetTypes()[0];

            Type ruleContextType = typeof(ViewRuleContext<,>);

            return ruleContextType.MakeGenericType(resultType, userSessionType);
        }

        public static Type CreateActionContext(UXAction action, Cdc.MetaManager.DataAccess.Domain.Module module)
        {

            Type userSessionType = LoadUserSessionType(module.Application);

            ActionParametersTemplate template = new ActionParametersTemplate();
            template.SetProperty("action", action);
            template.SetProperty("module", module);
            
            string source = template.RenderToString();

            CSharpCodeProvider provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });

            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Core.dll");

            CompilerResults results = provider.CompileAssemblyFromSource(options, source);

            Type resultType = results.CompiledAssembly.GetTypes()[0];

            Type ruleContextType = typeof(ActionRuleContext<,>);

            return ruleContextType.MakeGenericType(resultType, userSessionType);
        }
    }
}
