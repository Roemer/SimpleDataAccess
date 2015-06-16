using System;
using SimpleDataAccess.Definitions;

namespace SimpleDataAccess.Core
{
    public sealed class OrderByItemManual : OrderByItemBase
    {
        private readonly string _manualSql;
        private readonly SortDirection? _direction;

        public OrderByItemManual(string manualSql, SortDirection? direction)
        {
            _manualSql = manualSql;
            _direction = direction;
        }

        public override string Generate()
        {
            if (_direction == null)
            {
                return String.Format("{0}", _manualSql);
            }
            return String.Format("{0} {1}", _manualSql, _direction);
        }
    }
}
