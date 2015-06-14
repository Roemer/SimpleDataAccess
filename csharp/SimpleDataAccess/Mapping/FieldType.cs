using System.Data;
using SimpleDataAccess.Definitions;

namespace SimpleDataAccess.Mapping
{
    /// <summary>
    /// Class to encapsulate the field type
    /// </summary>
    public class FieldType
    {
        public FieldType(DbType dbType)
        {
            DbType = dbType;
        }

        public FieldType(SpecialDbType specialDbType)
        {
            SpecialType = specialDbType;
        }

        public DbType? DbType { get; private set; }
        public SpecialDbType? SpecialType { get; private set; }

        public override string ToString()
        {
            return DbType.HasValue ? DbType.ToString() : SpecialType.ToString();
        }
    }
}
