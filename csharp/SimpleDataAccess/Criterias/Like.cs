using System.Data;
using SimpleDataAccess.Core;
using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

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

        internal override string Process(EntityHandlerBase entityHandler, DataEntityMappingBase mapping, IDbCommand cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}
