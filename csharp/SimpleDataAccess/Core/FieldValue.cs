using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Core
{
    /// <summary>
    /// Class to store a value for a field
    /// </summary>
    public sealed class FieldValue
    {
        private readonly MappingField _mappingField;
        private object _storedValue;
        private object _currentValue;

        public FieldValue(MappingField mappingField)
        {
            _mappingField = mappingField;
        }

        /// <summary>
        /// Is set when a field is dirty and should be saved on the next update query
        /// </summary>
        public bool IsDirty { get; private set; }

        /// <summary>
        /// Returns the current value
        /// </summary>
        public T Get<T>()
        {
            return (T)_currentValue;
        }

        /// <summary>
        /// Sets the current value to a new value and sets <see cref="IsDirty"/> if it changed
        /// </summary>
        /// <typeparam name="T">The type of the underlying field</typeparam>
        /// <param name="newValue">The new value to set</param>
        /// <returns>true if the value changed, false if it's the same as before</returns>
        public bool Set<T>(T newValue)
        {
            // Check if the values are equal
            if (Equals(_currentValue, newValue))
            {
                return false;
            }
            // Set the new value
            _currentValue = newValue;
            // Set the dirty flag
            IsDirty = true;
            return true;
        }
    }
}
