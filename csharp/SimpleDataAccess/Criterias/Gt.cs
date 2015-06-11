using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    /// <summary>
    /// Greater than
    /// </summary>
    public class Gt : FieldCriteriaBase
    {
        public Gt(int fieldIndex, object value)
            : base(fieldIndex, value)
        {
        }

        protected override string Op
        {
            get { return ">"; }
        }
    }
}
