using System.Collections.Generic;
using SimpleDataAccess.Criterias.Bases;
using SimpleDataAccess.Definitions;

namespace SimpleDataAccess.Core
{
    public class Query
    {
        private readonly List<CriteriaBase> _criterias;
        private readonly List<OrderByItemBase> _orderItems;
        private readonly List<int> _fields;

        public int Offset { get; set; }
        public int Limit { get; set; }

        public IReadOnlyCollection<int> Fields
        {
            get { return _fields.AsReadOnly(); }
        }

        public Query()
        {
            _criterias = new List<CriteriaBase>();
            _orderItems = new List<OrderByItemBase>();
            _fields = new List<int>();
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

        public Query SetFieldList(params int[] fields)
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
