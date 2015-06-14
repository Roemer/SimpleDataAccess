using System.Collections.Generic;
using System.Data;
using SimpleDataAccess.Definitions;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Core
{
    public abstract class EntityHandlerBase<T> where T : DataEntityBase, new()
    {
        protected abstract IDbCommand CreateCommand(FieldType fieldType);

        /// <summary>
        /// Gets the first found entity or null if no entity could be found
        /// </summary>
        public T Get(Query query)
        {
            // Make sure to return only one entity
            if (query == null)
            {
                query = new Query();
            }
            query.Limit = 1;

            // Get the values with the associated column
            var valueDictList = GetValueList(query);

            T entity = null;
            if (valueDictList.Count > 0)
            {
                // Initialize the Entity
                entity = new T();
                entity.State = EntityState.Initialized;
                FillEntityFromDict(entity, valueDictList[0]);
            }
            return entity;
        }

        private List<Dictionary<int, object>> GetValueList(Query query)
        {
            var sampleEntity = new T();
            var mapping = sampleEntity.GetMapping();
            return null;
        }

        private void FillEntityFromDict(T entity, Dictionary<int, object> valueDict)
        {
            foreach (var kvp in valueDict)
            {
                // Set the new v
                entity.SetValue(kvp.Key, kvp.Value);
            }
        }

        public List<T> GetList(Query query)
        {
            return null;
        }
    }
}
