using SimpleDataAccess.Definitions;
using System;

namespace SimpleDataAccess.Querying
{
    public sealed class OrderByItem : OrderByItemBase
    {
        private readonly int _fieldIndex;
        private readonly SortDirection _direction;

        public OrderByItem(int fieldIndex, SortDirection direction)
        {
            _fieldIndex = fieldIndex;
            _direction = direction;
        }

        public override string Generate()
        {
            return String.Format("[{0}] {1}", _fieldIndex, _direction);
        }
    }
}