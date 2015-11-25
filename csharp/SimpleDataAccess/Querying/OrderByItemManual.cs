using SimpleDataAccess.Definitions;
using System;

namespace SimpleDataAccess.Querying
{
    public sealed class OrderByItemManual : OrderByItemBase
    {
        private readonly string _manualString;
        private readonly SortDirection? _direction;

        public OrderByItemManual(string manualString, SortDirection? direction)
        {
            _manualString = manualString;
            _direction = direction;
        }

        public override string Generate()
        {
            if (_direction == null)
            {
                return String.Format("{0}", _manualString);
            }
            return String.Format("{0} {1}", _manualString, _direction);
        }
    }
}