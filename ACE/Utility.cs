using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE
{
    public static class Utility
    {
        public static void EnsoureAssemblyLoaded(IEnumerable<string> assmblies)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (string name in assmblies)
            {
                if (!assemblies.Any(n => n.GetName().Name == name))
                {
                    System.Reflection.Assembly.Load(name);
                }
            }
        }
    }
}
