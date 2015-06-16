using SimpleDataAccess.Definitions;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Core
{
    /// <summary>
    /// Base class for all data entities
    /// </summary>
    public abstract class DataEntityBase
    {
        private FieldValue[] _fieldValues;

        // The current state of the entity
        public EntityState State { get; set; }

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
            _fieldValues = new FieldValue[mapping.Fields.Count];
            foreach (var field in mapping.Fields)
            {
                var fieldValue = new FieldValue(field);
                _fieldValues[field.FieldIndex] = fieldValue;
            }
        }
    }
}
