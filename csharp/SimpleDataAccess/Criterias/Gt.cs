using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Criterias
{
    /// <summary>
    /// Greater than
    /// </summary>
    public class Gt : FieldCriteriaBase
    {
        public Gt(MappingField field, object value)
            : base(field, value)
        {
        }

        internal override string Op
        {
            get { return ">"; }
        }
    }
}
