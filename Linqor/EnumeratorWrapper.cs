using System.Collections;
using System.Collections.Generic;

namespace Linqor
{
    internal class EnumeratorWrapper<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _source;

        public EnumeratorWrapper(IEnumerator<T> source)
        {
            _source = source;
        }

        public T Current 
        {
            get
            {
                return _source.Current;
            }
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
            return HasCurrent;
        }

        public void Reset()
        {
            _source.Reset();
        }

        public bool HasCurrent { get; set; }
    }
}
