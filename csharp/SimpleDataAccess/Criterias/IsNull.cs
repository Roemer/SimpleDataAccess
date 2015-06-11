using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    public class IsNull : CriteriaBase
    {
        private readonly int _fieldIndex;

        public IsNull(int fieldIndex)
        {
            _fieldIndex = fieldIndex;
        }
    }
}
