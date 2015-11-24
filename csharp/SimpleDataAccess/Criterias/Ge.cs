using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Mapping;

namespace SimpleDataAccess.Criterias
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
