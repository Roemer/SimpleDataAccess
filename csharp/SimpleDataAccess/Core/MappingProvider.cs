using SimpleDataAccess.Mapping;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SimpleDataAccess.Core
{
    public static class MappingProvider
    {
        private static readonly Dictionary<Type, DataEntityMappingBase> StaticMappings = new Dictionary<Type, DataEntityMappingBase>();

        public static void AddMapping(Type type, DataEntityMappingBase mapping)
        {
            StaticMappings[type] = mapping;
        }

        public static void AddMapping<T>(DataEntityMappingBase mapping) where T : DataEntityBase
        {
            AddMapping(typeof(T), mapping);
        }

        public static DataEntityMappingBase GetMapping<T>() where T : DataEntityBase
        {
            return GetMapping(typeof(T));
        }

        public static DataEntityMappingBase GetMapping(Type type)
        {
            DataEntityMappingBase mapping;
            if (!StaticMappings.TryGetValue(type, out mapping))
            {
                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            }
            return StaticMappings[type];
        }
    }
}
