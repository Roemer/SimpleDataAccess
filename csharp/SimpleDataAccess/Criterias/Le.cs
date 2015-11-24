using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Criterias
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
