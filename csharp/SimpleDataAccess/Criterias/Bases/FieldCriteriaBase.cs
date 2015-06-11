using System;

namespace SimpleDataAccess.Criterias.Bases
{
    /// <summary>
    /// Base criteria for single fields
    /// </summary>
    public abstract class FieldCriteriaBase : CriteriaBase
    {
        private readonly int _fieldIndex;
        private readonly object _value;

        protected FieldCriteriaBase(int fieldIndex, object value)
        {
            _fieldIndex = fieldIndex;
            _value = value;
        }

        protected abstract string Op { get; }
    }
}
