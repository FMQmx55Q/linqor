using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public class OrderedEnumerable<T, TKey> : IOrderedEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Func<T, TKey> _keySelector;
        private readonly IComparer<TKey> _keyComparer;
        private readonly bool _descending;
        private readonly IComparer<T> _comparer;

        public OrderedEnumerable(
            IEnumerable<T> source,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            bool descending)
        {
            _source = source;
            _keySelector = keySelector;
            _keyComparer = keyComparer;
            _descending = descending;
            _comparer = new OrderedComparer<T, TKey>(keySelector, keyComparer, descending);
        }

        internal IEnumerable<T> Source => _source;
        internal Func<T, TKey> KeySelector => _keySelector;
        internal IComparer<TKey> KeyComparer => _keyComparer;
        internal bool Descending => _descending;
        internal IComparer<T> Comparer => _comparer;

        public virtual IEnumerator<T> GetEnumerator() => _source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey2>(
            Func<T, TKey2> keySelector,
            IComparer<TKey2> comparer,
            bool descending)
        {
            var orderedBy = !_descending
                ? _source.OrderBy(_keySelector, _keyComparer)
                : _source.OrderByDescending(_keySelector, _keyComparer);
            
            var thenBy = !descending
                ? orderedBy.ThenBy(keySelector, comparer)
                : orderedBy.ThenByDescending(keySelector, comparer);

            return thenBy;
        }
    }
}