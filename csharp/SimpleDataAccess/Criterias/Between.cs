using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    public class Between : CriteriaBase
    {
        private readonly int _fieldIndex;
        private readonly object _valueLower;
        private readonly object _valueHigher;

        public Between(int fieldIndex, object valueLower, object valueHigher)
        {
            _fieldIndex = fieldIndex;
            _valueLower = valueLower;
            _valueHigher = valueHigher;
        }
    }
}
