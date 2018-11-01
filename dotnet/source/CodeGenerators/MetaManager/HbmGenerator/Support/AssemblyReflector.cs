using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Cdc.HbmGenerator.Support
{
    public class AssemblyReflector
    {
        public static List<string> ListClasses(Assembly assembly) 
        {
            List<string> classes = new List<string>();

            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsClass && type.IsPublic && !type.IsAbstract) 
                {
                    classes.Add(type.FullName);
                }
            }

            return classes;

        }
    }
}
