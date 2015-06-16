using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDataAccess.Mapping
{
    public class DataEntityMapping
    {
        public IReadOnlyList<MappingTable> Tables { get; private set; }
        public IReadOnlyList<MappingField> Fields { get; private set; }

        public void AddTables(params MappingTable[] tables)
        {
            // Tables
            Tables = Array.AsReadOnly(tables);

            // All fields from all tables
            var totalFields = Tables.Sum(table => table.Fields.Count);
            var allFields = new MappingField[totalFields];
            foreach (var field in Tables.SelectMany(table => table.Fields))
            {
                allFields[field.FieldIndex] = field;
            }
            Fields = Array.AsReadOnly(allFields);
        }

        public override string ToString()
        {
            return String.Format("{0} tables, {1} fields", Tables.Count, Fields.Count);
        }
    }
}
