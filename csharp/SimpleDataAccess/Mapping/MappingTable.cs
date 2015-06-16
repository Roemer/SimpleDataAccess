using System;
using System.Collections.Generic;

namespace SimpleDataAccess.Mapping
{
    public class MappingTable
    {
        public string TableName { get; private set; }

        public IReadOnlyList<MappingField> Fields { get; private set; }

        public MappingTable(string tableName, params MappingField[] fields)
        {
            TableName = tableName;
            Array.ForEach(fields, field => field.Table = this);
            Fields = Array.AsReadOnly(fields);
        }

        public override string ToString()
        {
            return String.Format("{0} ({1} fields)", TableName, Fields.Count);
        }
    }
}
