using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataAccess.Criterias.Bases
{
    public abstract class CriteriaBase
    {
        public bool IsNegated { get; set; }

        protected CriteriaBase()
        {
            IsNegated = false;
        }

        public CriteriaBase Negate()
        {
            IsNegated = true;
            return this;
        }
    }
}
