using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    public class And : JunctionCriteriaBase
    {
        internal override string Op
        {
            get { return "AND"; }
        }
    }
}
