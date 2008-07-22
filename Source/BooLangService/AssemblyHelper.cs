using System;
using System.Reflection;
using System.Reflection.Emit;

namespace BooLangService
{
    public class AssemblyHelper
    {
        public static Assembly FindAssemblyInCurrentAppDomain(string path)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly is AssemblyBuilder) continue;
                if (assembly.CodeBase != new Uri(path).ToString()) continue;

                return assembly;
            }

            return null;
        }

        public static Assembly FindInCurrentAppDomainOrLoad(string path)
        {
            return FindAssemblyInCurrentAppDomain(path) ?? Assembly.LoadFrom(path);
        }
    }
}