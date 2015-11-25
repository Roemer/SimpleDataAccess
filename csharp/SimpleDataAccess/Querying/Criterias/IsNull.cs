using SimpleDataAccess.Core;
using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;
using System.Data;

namespace SimpleDataAccess.Querying.Criterias
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