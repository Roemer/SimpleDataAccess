using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;

namespace SimpleDataAccess.Querying.Criterias
{
    /// <summary>
    /// Greater or equal
    /// </summary>
    public class Ge : FieldCriteriaBase
    {
        public Ge(MappingField field, object value)
            : base(field, value)
        {
        }

        internal override string Op
        {
            get { return ">="; }
        }
    }
}