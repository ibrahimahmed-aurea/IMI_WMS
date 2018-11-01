using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

namespace Cdc.MetaManager.GUI
{
    public class Compiler
    {
        public static Assembly CompileSource(String csharpSource)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CompilerVersion", "v3.5");

            CSharpCodeProvider provider = new CSharpCodeProvider(parameters);

            if (provider != null)
            {
                CompilerParameters cp = new CompilerParameters()
                {
                    // Generate a dll instead of a exe
                    GenerateExecutable = false,

                    // Save the assembly as a physical file.
                    GenerateInMemory = true,
                };

                cp.ReferencedAssemblies.Clear();
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Data.Linq.dll");
                cp.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                cp.ReferencedAssemblies.Add("System.Core.dll");

                // Invoke compilation of the source code.
                CompilerResults cr = provider.CompileAssemblyFromSource(cp, new string[] { csharpSource });

                // Create compilation error message
                if (cr.Errors.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Compile failed {0} errors/warnings :", cr.Errors.Count));

                    foreach (CompilerError ce in cr.Errors)
                    {
                        sb.AppendLine(string.Format("Line ({0},{1}) : {2}", ce.Line, ce.Column, ce.ErrorText));
                    }

                    throw new Exception(sb.ToString());
                }

                return cr.CompiledAssembly;

            }
            else
            {
                throw new Exception("Failed to create CSharpCodeProvider");
            }
            
        }
    }
}
