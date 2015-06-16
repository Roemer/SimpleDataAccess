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
        protected abstract string Escape(string value);

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

            // Create the field select part
            var sb = new StringBuilder();
            foreach (var field in fieldsToSelect)
            {
                AddField(field, sb, query);
            }
            var cmd = sb.ToString();

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
        }*/

        private IEnumerable<MappingField> GetFields(DataEntityMapping mapping, Query query)
        {
            var retList = new List<MappingField>();
            if (query == null || query.Fields.Count == 0)
            {
                // Add all fields
                retList.AddRange(mapping.Fields);
            }
            else
            {
                // Add only the specified fields
                retList.AddRange(query.Fields.Select(fieldIndex => mapping.Fields[fieldIndex]));
            }
            return retList.ToArray();
        }

        private void AddField(MappingField field, StringBuilder sb, Query query)
        {
            var fieldName = field.FieldName;
            if (query != null && query.FullyQualifiedFieldNames)
            {
                // Add the table
                var tableName = field.Table.TableName;
                sb.Append(Escape(tableName)).Append(".");
            }

            // Add the field
            sb.Append(Escape(fieldName));
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
