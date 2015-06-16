using System;
using System.Collections.Generic;
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
            return StaticMappings[type];
        }
    }
}
