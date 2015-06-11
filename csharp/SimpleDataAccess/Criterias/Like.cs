using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    public class Like : CriteriaBase
    {
        private readonly int _fieldIndex;
        private readonly string _value;

        public Like(int fieldIndex, string value)
        {
            _fieldIndex = fieldIndex;
            _value = value;
        }
    }
}
