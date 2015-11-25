using SimpleDataAccess.Querying.Criterias.Bases;

namespace SimpleDataAccess.Querying.Criterias
{
    public class Or : JunctionCriteriaBase
    {
        internal override string Op
        {
            get { return "OR"; }
        }
    }
}