using System;
using System.Collections;
using System.Collections.Generic;

namespace Linqor
{
    public class OrderedEnumerable<T, TKey> : IEnumerable<T>
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

        public virtual IEnumerator<T> GetEnumerator()
        {
            bool previousHasValue = false;
            T previousValue = default;

            foreach (var current in _source)
            {
                if (previousHasValue && _comparer.Compare(previousValue, current) > 0)
                    throw new UnorderedElementDetectedException();

                yield return current;

                previousHasValue = true;
                previousValue = current;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}