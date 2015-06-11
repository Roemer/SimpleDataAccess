﻿using SimpleDataAccess.Criterias.Bases;

namespace SimpleDataAccess.Criterias
{
    /// <summary>
    /// Equal
    /// </summary>
    public class Eq : FieldCriteriaBase
    {
        public Eq(int fieldIndex, object value)
            : base(fieldIndex, value)
        {
        }

        protected override string Op
        {
            get { return "="; }
        }
    }
}
