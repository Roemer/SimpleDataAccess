using SimpleDataAccess.Core;
using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SimpleDataAccess.Querying.Criterias
{
    public class In : CriteriaBase
    {
        private readonly int _fieldIndex;
        private readonly IEnumerable<object> _valueList;

        public In(int fieldIndex, IEnumerable<object> valueList)
        {
            _fieldIndex = fieldIndex;
            _valueList = valueList;
            if (!_valueList.Any())
            {
                throw new Exception(string.Format("IN Query for Column {0} has no Items", _fieldIndex));
            }
        }

        internal override string Process(EntityHandlerBase entityHandler, DataEntityMappingBase mapping, IDbCommand cmd)
        {
            throw new NotImplementedException();
        }
    }
}