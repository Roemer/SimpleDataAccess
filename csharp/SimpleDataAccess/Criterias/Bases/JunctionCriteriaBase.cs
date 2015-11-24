using System.Collections.Generic;
using System.Data;
using System.Text;
using SimpleDataAccess.Core;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Criterias.Bases
{
    /// <summary>
    /// Base criteria for junctions
    /// </summary>
    public abstract class JunctionCriteriaBase : CriteriaBase
    {
        protected JunctionCriteriaBase()
        {
            _innerExpressions = new List<CriteriaBase>();
        }

        internal abstract string Op { get; }

        private readonly List<CriteriaBase> _innerExpressions;

        public bool HasCriterias { get { return _innerExpressions.Count > 0; } }

        /// <summary>
        /// Add an expression to the junction
        /// </summary>
        public JunctionCriteriaBase Add(CriteriaBase innerCriteria)
        {
            _innerExpressions.Add(innerCriteria);
            return this;
        }

        internal override string Process(EntityHandlerBase entityHandler, DataEntityMappingBase mapping, IDbCommand cmd)
        {
            var sb = new StringBuilder();
            if (HasCriterias)
            {
                sb.Append("(");
                var firstFlag = true;
                foreach (var crit in _innerExpressions)
                {
                    if (!firstFlag)
                    {
                        sb.AppendFormat(" {0} ", Op);
                    }
                    sb.Append(crit.Process(entityHandler, mapping, cmd));
                    firstFlag = false;
                }
                sb.Append(")");
            }
            return Finalize(sb.ToString());
        }
    }
}
