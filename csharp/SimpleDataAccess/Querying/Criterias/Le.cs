using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;

namespace SimpleDataAccess.Querying.Criterias
{
    /// <summary>
    /// Less or equal
    /// </summary>
    public class Le : FieldCriteriaBase
    {
        public Le(MappingField field, object value)
            : base(field, value)
        {
        }

        internal override string Op
        {
            get { return "<="; }
        }
    }
}