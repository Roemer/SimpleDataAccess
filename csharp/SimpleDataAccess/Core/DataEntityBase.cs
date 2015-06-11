using System.Collections.Generic;
using System.Linq;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Core
{
    /// <summary>
    /// Base class for all data entities
    /// </summary>
    public abstract class DataEntityBase
    {
        private List<FieldValue> _fieldValues;

        /// <summary>
        /// Constructor
        /// </summary>
        protected DataEntityBase()
        {
            InitializeColumns();
        }

        /// <summary>
        /// Method to get the mapping for this entity
        /// </summary>
        public abstract DataEntityMapping GetMapping();

        /// <summary>
        /// Get's the value for the given field
        /// </summary>
        protected internal T GetValue<T>(int fieldIndex)
        {
            var colValue = _fieldValues[fieldIndex];
            return colValue.Get<T>();
        }

        /// <summary>
        /// Sets the value for the given field
        /// </summary>
        protected internal void SetValue<T>(int fieldIndex, T newValue)
        {
            var colValue = _fieldValues[fieldIndex];
            colValue.Set(newValue);
        }

        /// <summary>
        /// Initializes the internal list of columns with initial values
        /// </summary>
        private void InitializeColumns()
        {
            var mapping = GetMapping();
            var totalFields = mapping.Tables.Sum(table => table.Fields.Count);
            _fieldValues = new List<FieldValue>(totalFields);

            foreach (var table in mapping.Tables)
            {
                foreach (var field in table.Fields)
                {
                    var fieldValue = new FieldValue(field);
                    _fieldValues[field.FieldIndex] = fieldValue;
                }
            }
        }
    }
}
