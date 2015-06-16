using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Core
{
    public static class MappingProvider
    {
        private static readonly Dictionary<Type, DataEntityMapping> StaticMappings = new Dictionary<Type, DataEntityMapping>();

        public static void AddMapping(Type type , DataEntityMapping mapping)
        {
            StaticMappings[type] = mapping;
        }

        public static DataEntityMapping GetMapping(Type type)
        {
            // Make sure the static constructor was fired
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            // Return the correct type
            return StaticMappings[type];
        }
    }
}
