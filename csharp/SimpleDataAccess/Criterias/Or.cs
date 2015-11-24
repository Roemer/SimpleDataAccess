using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    public class Or : JunctionCriteriaBase
    {
        internal override string Op
        {
            get { return "OR"; }
        }
    }
}
