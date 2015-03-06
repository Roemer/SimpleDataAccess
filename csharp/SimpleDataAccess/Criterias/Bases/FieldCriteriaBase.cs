using System;

namespace SimpleDataAccess.Criterias.Bases
{
    public abstract class FieldCriteriaBase : CriteriaBase
    {
        private readonly Enum _field;
        private readonly object _value;

        protected FieldCriteriaBase(Enum field, object value)
        {
            _field = field;
            _value = value;
        }
    }
}
