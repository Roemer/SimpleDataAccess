using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
   /// <summary>
    /// Greater or equal
    /// </summary>
    public class Ge : FieldCriteriaBase
    {
        public Ge(int fieldIndex, object value)
            : base(fieldIndex, value)
        {
        }

        protected override string Op
        {
            get { return ">="; }
        }
    }
}
