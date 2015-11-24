using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Criterias
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
