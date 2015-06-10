using System.Collections.Generic;

namespace SimpleDataAccess.Mapping
{
    public class DataEntityMapping
    {
        private readonly List<MappingTable> _tables;

        public IReadOnlyList<MappingTable> Tables
        {
            get { return _tables.AsReadOnly(); }
        }

        public DataEntityMapping()
        {
            _tables = new List<MappingTable>();
        }

        public MappingTable AddTable(MappingTable table)
        {
            _tables.Add(table);
            return table;
        }

        public MappingTable AddTable(string tableName)
        {
            return AddTable(new MappingTable(tableName));
        }
    }
}
