using System;
using System.Collections.Generic;
using System.Data;
using SimpleDataAccess.Definitions;

namespace SimpleDataAccess.Mapping
{
    public class MappingTable
    {
        private readonly List<MappingField> _fields;

        public string TableName { get; private set; }

        public IReadOnlyList<MappingField> Fields
        {
            get { return _fields.AsReadOnly(); }
        }

        public MappingTable(string tableName)
        {
            TableName = tableName;
            _fields = new List<MappingField>();
        }

        public MappingField AddField(int fieldIndex, string fieldName, DbType dbType)
        {
            return AddField(fieldIndex, fieldName, new FieldType(dbType));
        }

        public MappingField AddField(int fieldIndex, string fieldName, SpecialDbType specialDbType)
        {
            return AddField(fieldIndex, fieldName, new FieldType(specialDbType));
        }

        public MappingField AddField(int fieldIndex, string fieldName, FieldType fieldType)
        {
            var field = new MappingField
            {
                FieldIndex = fieldIndex,
                FieldName = fieldName,
                FieldType = fieldType
            };
            _fields.Add(field);
            return field;
        }

        public override string ToString()
        {
            return String.Format("{0} ({1} fields)", TableName, Fields.Count);
        }
    }
}
