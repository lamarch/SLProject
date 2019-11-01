using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SLProject.SLStandardLib.Types;

namespace SLProject.SLStandardLib
{
    public static class StandardLibrary
    {
        private static List<Type> sl_cs_assemblies = new List<Type>();

        public static void AddRef ( string assembly_path )
        {
            Assembly assembly = Assembly.LoadFrom(assembly_path);
            for ( int i = 0 ; i < assembly.GetTypes().Length ; i++ ) {

                if ( Attribute.GetCustomAttributes( assembly.GetTypes()[i], typeof( SLAssemblyAttribute ) ).Length > 0 ) {
                    sl_cs_assemblies.Add(assembly.GetTypes()[i]);
                }
            }
        }

        public static bool Exist ( string function_name )
        {
            return false;
        }

        public static class MainAssembly
        {
            public static void adress(Adress adress)
            {
                
            }
        }
    }
}
