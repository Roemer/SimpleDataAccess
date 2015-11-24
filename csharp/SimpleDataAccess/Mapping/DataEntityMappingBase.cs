using System;
using System.Collections.Generic;

namespace SimpleDataAccess.Mapping
{
    public abstract class DataEntityMappingBase
    {
        private readonly List<MappingField> _fields = new List<MappingField>();
        private IReadOnlyList<MappingField> _fieldsReadOnly = null;
        public string TableName { get; protected set; }

        public IReadOnlyList<MappingField> Fields
        {
            get { return _fieldsReadOnly ?? (_fieldsReadOnly = _fields.AsReadOnly()); }
        }

        public MappingField Add(MappingField field)
        {
            field.FieldIndex = _fields.Count;
            field.Mapping = this;
            _fields.Add(field);
            _fieldsReadOnly = null;
            return field;
        }

        public override string ToString()
        {
            return String.Format("{0} ({1} fields)", TableName, Fields.Count);
        }
    }
}
