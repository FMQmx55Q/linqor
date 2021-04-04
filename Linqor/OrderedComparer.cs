using System;
using System.Collections.Generic;

namespace Linqor
{
    internal class OrderedComparer<T, TKey> : IComparer<T>
    {
        private readonly Func<T, TKey> _keySelector;
        private readonly IComparer<TKey> _keyComparer;
        private readonly bool _descending;

        public OrderedComparer(
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            bool descending)
        {
            _keySelector = keySelector;
            _keyComparer = keyComparer;
            _descending = descending;
        }

        public int Compare(T x, T y) => _keyComparer.Compare(_keySelector(x), _keySelector(y)) * (_descending ? -1 : 1);
    }
}