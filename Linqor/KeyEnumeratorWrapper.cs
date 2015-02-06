using System;
using System.Collections;
using System.Collections.Generic;

namespace Linqor
{
    internal class KeyEnumeratorWrapper<T, TKey> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _source;
        private readonly Func<T, TKey> _keySelector;

        public KeyEnumeratorWrapper(IEnumerator<T> source, Func<T, TKey> keySelector)
        {
            _source = source;
            _keySelector = keySelector;
        }

        public T Current
        {
            get { return _source.Current; }
        }

        public void Dispose()
        {
            _source.Dispose();
        }

        object IEnumerator.Current
        {
            get { return ((IEnumerator)_source).Current; }
        }

        public bool MoveNext()
        {
            HasCurrent = _source.MoveNext();
            if (HasCurrent)
            {
                Key = _keySelector(_source.Current);
            }
            return HasCurrent;
        }

        public void Reset()
        {
            _source.Reset();
        }

        public bool HasCurrent { get; set; }
        public TKey Key { get; set; }
    }
}
