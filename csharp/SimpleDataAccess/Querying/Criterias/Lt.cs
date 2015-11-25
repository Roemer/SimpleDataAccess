using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;

namespace SimpleDataAccess.Querying.Criterias
{
    /// <summary>
    /// Less than
    /// </summary>
    public class Lt : FieldCriteriaBase
    {
        public Lt(MappingField field, object value)
            : base(field, value)
        {
        }

        internal override string Op
        {
            get { return "<"; }
        }
    }
}