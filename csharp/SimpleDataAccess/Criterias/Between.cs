using System.Data;
using SimpleDataAccess.Core;
using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

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

        internal override string Process(EntityHandlerBase entityHandler, DataEntityMappingBase mapping, IDbCommand cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}
