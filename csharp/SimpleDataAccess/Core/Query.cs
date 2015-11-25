using SimpleDataAccess.Definitions;
using SimpleDataAccess.Mapping;
using SimpleDataAccess.Querying;
using SimpleDataAccess.Querying.Criterias.Bases;
using System.Collections.Generic;

namespace SimpleDataAccess.Core
{
    public class Query
    {
        private readonly List<CriteriaBase> _criterias;
        private readonly List<OrderByItemBase> _orderItems;
        private readonly List<MappingField> _fields;

        public int Offset { get; set; }
        public int Limit { get; set; }
        public bool FullyQualifiedFieldNames { get; set; }
        public TableHint TableHint { get; set; }

        public IReadOnlyList<CriteriaBase> Criterias
        {
            get { return _criterias.AsReadOnly(); }
        }

        public IReadOnlyList<OrderByItemBase> OrderItems
        {
            get { return _orderItems.AsReadOnly(); }
        }

        public IReadOnlyList<MappingField> Fields
        {
            get { return _fields.AsReadOnly(); }
        }

        public Query()
        {
            _criterias = new List<CriteriaBase>();
            _orderItems = new List<OrderByItemBase>();
            _fields = new List<MappingField>();
        }

        public Query Add(CriteriaBase expression)
        {
            _criterias.Add(expression);
            return this;
        }

        public Query AddOrder(OrderByItemBase orderItem)
        {
            _orderItems.Add(orderItem);
            return this;
        }

        public Query AddOrder(int fieldIndex, SortDirection sortDirection)
        {
            var orderItem = new OrderByItem(fieldIndex, sortDirection);
            return AddOrder(orderItem);
        }

        public Query SetFieldList(params MappingField[] fields)
        {
            _fields.Clear();
            if (fields.Length > 0)
            {
                _fields.AddRange(fields);
            }
            return this;
        }
    }
}
