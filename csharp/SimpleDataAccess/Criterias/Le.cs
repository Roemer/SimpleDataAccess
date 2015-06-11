using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    /// <summary>
    /// Less or equal
    /// </summary>
    public class Le : FieldCriteriaBase
    {
        public Le(int fieldIndex, object value)
            : base(fieldIndex, value)
        {
        }

        protected override string Op
        {
            get { return "<="; }
        }
    }
}
