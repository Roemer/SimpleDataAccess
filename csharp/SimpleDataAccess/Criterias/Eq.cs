using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Criterias
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
