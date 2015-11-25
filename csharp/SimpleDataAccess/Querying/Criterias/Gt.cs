using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying.Criterias.Bases;

namespace SimpleDataAccess.Querying.Criterias
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