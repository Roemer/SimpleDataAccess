using System.Collections.Generic;

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

        protected abstract string Op { get; }

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
    }
}
