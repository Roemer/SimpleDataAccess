using SimpleDataAccess.Core;
using SimpleDataAccess.Mapping;
using System;
using System.Data;

namespace SimpleDataAccess.Querying.Criterias.Bases
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

        protected string Finalize(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                // Don't format if the string is empty
                return String.Empty;
            }
            return IsNegated ? String.Format("NOT ({0})", s) : s;
        }

        protected string AddSqlParameter(EntityHandlerBase entityHandler, MappingField field, IDbCommand cmd, object value)
        {
            string sqlParamName;
            lock (cmd)
            {
                // Create the parameter name
                sqlParamName = String.Format("@SDAP_{0}{1}", field.FieldName, cmd.Parameters.Count + 1);
                // Create the parameter
                var newParameter = entityHandler.CreateParameter(sqlParamName, field.FieldType, value ?? DBNull.Value);
                // Add the prameter to the command
                cmd.Parameters.Add(newParameter);
            }
            return sqlParamName;
        }

        internal abstract string Process(EntityHandlerBase entityHandler, DataEntityMappingBase mapping, IDbCommand cmd);
    }
}