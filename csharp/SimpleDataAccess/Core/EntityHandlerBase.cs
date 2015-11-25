using SimpleDataAccess.Definitions;
using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SimpleDataAccess.Core
{
    public abstract class EntityHandlerBase
    {
        public abstract IDbCommand CreateCommand();
        public abstract DbParameter CreateParameter(string paramName, FieldType fieldType, object value);
        public abstract string Escape(string value);
        public abstract string GetTable(string tableName, TableHint tableHint);

        /// <summary>
        /// Gets the first found entity or null if no entity could be found
        /// </summary>
        public T Get<T>(Query query) where T : DataEntityBase, new()
        {
            // Make sure to return only the first entity
            if (query == null)
            {
                query = new Query();
            }
            query.Offset = 0;
            query.Limit = 1;

            // Get the values for the wanted row
            var valueDictList = GetValueList(typeof(T), query);

            T entity = null;
            if (valueDictList.Count > 0)
            {
                // Initialize the Entity
                entity = new T { State = EntityState.Initialized };
                FillEntityFromDict(entity, valueDictList[0]);
            }
            return entity;
        }

        /// <summary>
        /// Get a list of entities with the wanted criterias
        /// </summary>
        public List<T> GetList<T>(Query query) where T : DataEntityBase, new()
        {
            // Get all wanted rows
            var valueDictList = GetValueList(typeof(T), query);

            var retList = new List<T>();
            foreach (var currValueDict in valueDictList)
            {
                var entity = new T { State = EntityState.Initialized };
                FillEntityFromDict(entity, currValueDict);
                retList.Add(entity);
            }
            return retList;
        }

        private List<Dictionary<MappingField, object>> GetValueList(Type entityType, Query query)
        {
            var mapping = MappingProvider.GetMapping(entityType);

            // Create the command
            var cmd = CreateCommand();

            // Create the select statement based on specified fields
            var cmdStr = BuildSelect(mapping, query);
            // Create the where part of the query
            cmdStr += BuildWhere(mapping, query, cmd);
            //cmdStr += " " + CreateOrderBy(query);
            // Set the command text
            cmd.CommandText = cmdStr;

            // Get the Entities
            var retValue = ExecuteEntityReader(mapping, cmd);

            return retValue;
        }

        /// <summary>
        /// Build the select part
        /// </summary>
        protected string BuildSelect(DataEntityMappingBase mapping, Query query)
        {
            var fields = BuildFieldList(mapping, query);
            var table = GetTable(mapping.TableName, query.TableHint);
            return String.Format("SELECT {0} FROM {1}", fields, table);
        }

        /// <summary>
        /// Build the where part
        /// Also adds parameters to the command if there are any
        /// </summary>
        protected string BuildWhere(DataEntityMappingBase mapping, Query query, IDbCommand cmd)
        {
            var sb = new StringBuilder();
            var firstFlag = true;
            // Loop thru all the criterias
            foreach (var crit in query.Criterias)
            {
                // Check if the current criteria is a junction criteria (and/or)
                if (crit is JunctionCriteriaBase)
                {
                    if (!((JunctionCriteriaBase)crit).HasCriterias)
                    {
                        // It doesn't have and nested criterias so skip it
                        continue;
                    }
                }
                sb.Append(firstFlag ? " WHERE " : " AND ");
                // Append the criteria
                sb.Append(crit.Process(this, mapping, cmd));
                // Mark that the first critieria was processed
                firstFlag = false;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Builds a list of all fields which should be selected
        /// </summary>
        protected string BuildFieldList(DataEntityMappingBase mapping, Query query)
        {
            // List of fields (either chosen ones or all)
            var fieldsToSelect = GetFields(mapping, query);
            var fields = String.Join(",", fieldsToSelect.Select(x => BuildField(x, query)));
            return fields;
        }

        private string BuildField(MappingField field, Query query)
        {
            if (query != null && query.FullyQualifiedFieldNames)
            {
                // Add the table and field
                return Escape(field.Mapping.TableName) + "." + Escape(field.FieldName);
            }
            // Add the field only
            return Escape(field.FieldName);
        }

        private IList<MappingField> GetFields(DataEntityMappingBase mapping, Query query)
        {
            var retList = new List<MappingField>();
            if (query == null || query.Fields == null || query.Fields.Count == 0)
            {
                // Add all fields
                retList.AddRange(mapping.Fields);
            }
            else
            {
                // Add only the specified fields
                retList.AddRange(query.Fields);
            }
            return retList.ToArray();
        }

        private List<Dictionary<MappingField, object>> ExecuteEntityReader(DataEntityMappingBase mapping, IDbCommand cmd)
        {
            // Initialize a lookup dictionary to map the column-mame to the column-field
            var sqlFieldToMappingFieldDict = mapping.Fields.ToDictionary(field => field.FieldName);

            var retList = new List<Dictionary<MappingField, object>>();
            using (var reader = SqlHelper.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    var objValueDict = new Dictionary<MappingField, object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        object value = null;
                        if (!reader.IsDBNull(i))
                        {
                            value = reader.GetValue(i);
                        }
                        objValueDict.Add(sqlFieldToMappingFieldDict[reader.GetName(i)], value);
                    }
                    retList.Add(objValueDict);
                }
            }
            return retList;
        }

        private void FillEntityFromDict(DataEntityBase entity, Dictionary<MappingField, object> valueDict)
        {
            foreach (var kvp in valueDict)
            {
                // Set the new value
                entity.SetValue(kvp.Key, kvp.Value);
            }
        }
    }
}
