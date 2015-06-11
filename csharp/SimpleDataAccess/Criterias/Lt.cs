using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    /// <summary>
    /// Less than
    /// </summary>
    public class Lt : FieldCriteriaBase
    {
        public Lt(int fieldIndex, object value)
            : base(fieldIndex, value)
        {
        }

        protected override string Op
        {
            get { return "<"; }
        }
    }
}
