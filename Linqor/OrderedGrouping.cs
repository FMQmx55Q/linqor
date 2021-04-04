using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public class OrderedGrouping<TKey, T> : OrderedEnumerable<T, TKey>, IGrouping<TKey, T>
    {
        private readonly IGrouping<TKey, T> _source;

        public OrderedGrouping(
            IGrouping<TKey, T> source,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            bool descending)
            : base(source, keySelector, keyComparer, descending)
        {
            _source = source;
        }

        public TKey Key => _source.Key;
    }
}