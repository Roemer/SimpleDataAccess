using SimpleDataAccess.Querying.Criterias.Bases;

namespace SimpleDataAccess.Querying.Criterias
{
    public class And : JunctionCriteriaBase
    {
        internal override string Op
        {
            get { return "AND"; }
        }
    }
}