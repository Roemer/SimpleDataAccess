using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SimpleDataAccess.Definitions;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Core
{
    public abstract class EntityHandlerBase<T> where T : DataEntityBase, new()
    {
        protected abstract IDbCommand CreateCommand(FieldType fieldType);
        protected abstract StringBuilder Escape(string value, StringBuilder builder);

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

        public List<T> GetList(Query query)
        {
            return null;
        }

        private List<Dictionary<int, object>> GetValueList(Query query)
        {
            var mapping = MappingProvider.GetMapping(typeof(T));

            // Create the Select Statement based on specified Fields
            var cmdStr = CreateSelect(mapping, query);

            return null;
        }

        private string CreateSelect(DataEntityMapping mapping, Query query)
        {
            // List of fields
            var fieldsToSelect = GetFields(mapping, query);
            return null;
        }

        /*private string CreateSelect(MappingInfo mappingInfo, Query query, LockingType lockType, string refTableName)
        {
            // Generate the Field Array
            IEnumerable<MappingInfoField> fieldList = GetFields(mappingInfo, query);

            // Join the Fields together
            string fields = String.Empty;
            foreach (MappingInfoField mif in fieldList)
            {
                fields += ", " + GetFieldSelectPart(mappingInfo, mif, query);
            }
            fields = fields.Remove(0, 2);

            // Get the TOP Part if needed
            string topStr = String.Empty;
            if (query != null)
            {
                if (query.Limit > 0)
                {
                    topStr = String.Format("TOP {0} ", query.Limit);
                }
            }
            return String.Format("SELECT {0}{1} FROM {2}", topStr, fields, CreateTableAndLocking(mappingInfo.TableName, query, lockType, refTableName));
        }

        private string GetFieldSelectPart(MappingInfo mappingInfo, MappingInfoField mappingInfoField, Query query)
        {
            string fieldName = mappingInfoField.FieldName;

            if (query != null && query.FullyQualifiedFieldnames)
            {
                // Add the Column Name with the Table Name
                string tableName = mappingInfo.TableName;
                return String.Format("[{0}].[{1}]", tableName, fieldName);
            }

            // Add the Column Name
            return String.Format("[{0}]", fieldName);
        }*/

        private IEnumerable<MappingField> GetFields(DataEntityMapping mapping, Query query)
        {
            var retList = new List<MappingField>();

            if (query == null || query.Fields.Count == 0)
            {
                // Add all fields
                foreach (var x in mapping.Tables)
                {
                    retList.AddRange(x.Fields);
                }
            }
            else
            {
                foreach (var fieldIndex in query.Fields)
                {
                 // TODO   
                }
                // Add only the specified fields
                //retList.AddRange(mappingInfo.GetSpecificColumns(query.Fields));
            }
            return retList.ToArray();
        }

        private void FillEntityFromDict(T entity, Dictionary<int, object> valueDict)
        {
            foreach (var kvp in valueDict)
            {
                // Set the new v
                entity.SetValue(kvp.Key, kvp.Value);
            }
        }
    }
}
