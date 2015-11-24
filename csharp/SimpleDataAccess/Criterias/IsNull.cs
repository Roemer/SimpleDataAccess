using System.Data;
using SimpleDataAccess.Core;
using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Criterias
{
    public class IsNull : CriteriaBase
    {
        private readonly int _fieldIndex;

        public IsNull(int fieldIndex)
        {
            _fieldIndex = fieldIndex;
        }

        internal override string Process(EntityHandlerBase entityHandler, DataEntityMappingBase mapping, IDbCommand cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}
