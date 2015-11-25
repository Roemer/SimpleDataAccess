using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;

namespace SimpleDataAccess.Querying.Criterias
{
    /// <summary>
    /// Equal
    /// </summary>
    public class Eq : FieldCriteriaBase
    {
        public Eq(MappingField field, object value)
            : base(field, value)
        {
        }

        internal override string Op
        {
            get { return "="; }
        }
    }
}