using SimpleDataAccess.Core;
using SimpleDataAccess.Mapping;
using System;
using System.Data;

namespace SimpleDataAccess.Criterias.Bases
{
    /// <summary>
    /// Base criteria for single fields
    /// </summary>
    public abstract class FieldCriteriaBase : CriteriaBase
    {
        private readonly MappingField _field;
        private readonly object _value;

        protected FieldCriteriaBase(MappingField field, object value)
        {
            _field = field;
            _value = value;
        }

        internal abstract string Op { get; }

        internal override string Process(EntityHandlerBase entityHandler, DataEntityMappingBase mapping, IDbCommand cmd)
        {
            // Generate the field part
            var fieldString = entityHandler.Escape(_field.FieldName);
            // Generate the Value Part
            var value = AddSqlParameter(entityHandler, _field, cmd, _value);

            // Put it together
            return Finalize(String.Format("{0} {1} {2}", fieldString, Op, value));
        }
    }
}
