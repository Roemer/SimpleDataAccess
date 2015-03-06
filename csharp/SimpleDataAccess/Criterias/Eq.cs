using SimpleDataAccess.Criterias.Bases;
using System;

namespace SimpleDataAccess.Criterias
{
    /// <summary>
    /// Equal
    /// </summary>
    public class Eq : FieldCriteriaBase
    {
        /// <summary>
        /// Equal
        /// </summary>
        public Eq(Enum field, object value)
            : base(field, value)
        {
        }
    }
}
