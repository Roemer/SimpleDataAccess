using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    public class And : JunctionCriteriaBase
    {
        protected override string Op
        {
            get { return "AND"; }
        }
    }
}
