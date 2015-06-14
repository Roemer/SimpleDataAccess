using System.Collections.Generic;
using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Core
{
    public class Query
    {
        private readonly List<CriteriaBase> _criterias;

        public int Offset { get; set; }
        public int Limit { get; set; }

        public Query()
        {
            _criterias = new List<CriteriaBase>();
        }

        public Query Add(CriteriaBase expression)
        {
            _criterias.Add(expression);
            return this;
        }
    }
}
